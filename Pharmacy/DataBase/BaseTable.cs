using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Pharmacy.DataBase
{
    public abstract class BaseTable<T>
    {
        protected T Record;

        protected string ColumnID { get; private set; }
        protected string TableName { get; private set; }
        protected bool UseUpdateCounter { get; private set; }
        protected SqlTransaction externalTransaction { get; private set; }
        protected SqlTransaction sqlTransaction { get; private set; }
        public bool UseExternalTransaction { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnID"></param>
        /// <param name="useUpdateCounter"></param>
        /// <param name="useExternalTransaction"></param>
        /// <param name="ExternalTransaction"></param>
        public BaseTable(string tableName, string columnID, bool useUpdateCounter, bool useExternalTransaction = false, SqlTransaction ExternalTransaction = null)
        {
            TableName = tableName;
            ColumnID = columnID;
            UseUpdateCounter = useUpdateCounter;
            UseExternalTransaction = useExternalTransaction;
            externalTransaction = ExternalTransaction;
        }

        /// <summary>
        /// Връща ид от „record“ променливата
        /// </summary>
        /// <param name="record"></param>
        protected abstract int GetIDValue(T record);

        /// <summary>
        /// Задава ид на „record“ променливата по референция
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="record"></param>
        protected abstract void SetIDValue(int ID, T record);

        /// <summary>
        /// Връща стойност за  update counter-а
        /// </summary>
        /// <returns></returns>
        protected virtual int GetUpdateCounterValue(T record)
        {
            if (UseUpdateCounter)
                throw new NotImplementedException();

            return 0;
        }

        /// <summary>
        /// Увеличава стойността на  update counter-а
        /// </summary>
        /// <returns></returns>
        protected virtual void IncreaseUpdateCounterValue(T record)
        {
            if (UseUpdateCounter)
                throw new NotImplementedException();
        }

        /// <summary>
        /// Копира данните от reader към структурата
        /// </summary>
        /// <param name="record"></param>
        /// <param name="reader"></param>
        protected abstract T DataToRecord(SqlDataReader reader);

        /// <summary>
        /// Копира данните от структурата към reader
        /// </summary>
        /// <param name="record"></param>
        /// <param name="reader"></param>
        protected abstract void RecordToData(SqlCommand command, T record);

        /// <summary>
        /// Връща списък с имена на колони
        /// </summary>
        /// <returns></returns>
        protected abstract List<string> GetColumns();

        /// <summary>
        /// Връща обект чрез по подразбиращ конструктор на типа „T“
        /// </summary>
        /// <returns></returns>
        protected abstract T DefaultRecordConstructor();

        /// <summary>
        /// Метод за селектиране на всички записи от БД
        /// </summary>
        /// <param name="recordsArray"></param>
        /// <param name="whereClause"></param>
        public bool SelectAllRecords(List<T> recordsArray, string whereClause = "")
        {
            try
            {
                string query = string.Format("SELECT * FROM {0} {1}", TableName, whereClause);

                SqlCommand command = CreateSqlCommand(query);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Record = DataToRecord(reader);
                    recordsArray.Add(Record);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Метод, който селектира само един запис по зададено ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="record"></param>
        public bool SelectRecord(int ID, out T record)
        {
            record = DefaultRecordConstructor();
            List<T> recordsArray = new List<T>();

            string whereClause = string.Format(" WHERE {0} = {1} ", ColumnID, ID);

            if (!SelectAllRecords(recordsArray, whereClause))
            {
                return false;
            }

            if (recordsArray.Count() > 1)
            {
                Log.LogError("More than one record with the same id!");
                return false;
            }

            if (recordsArray.Count() == 0)
            {
                Log.LogError("Record with id {0} not found.", ID);
                return false;
            }

            record = recordsArray.First();

            return true;
        }

        /// <summary>
        /// Метод за вмъкване на запис в БД
        /// </summary>
        /// <param name="record"></param>
        public bool InsertRecord(T record)
        {
            try
            {
                string Columns = GetColumnsStringForInsert();
                string Parameters = GetParametersStringForInsert();

                string sqlQuery = string.Format("INSERT INTO {0}({1}) output INSERTED.ID VALUES({2})", TableName, Columns, Parameters);

                SqlCommand sqlCommand = CreateSqlCommand(sqlQuery);

                RecordToData(sqlCommand, record);

                sqlCommand.CommandType = CommandType.Text;
                int ID = Convert.ToInt32(sqlCommand.ExecuteScalar());

                SetIDValue(ID, record);
            }
            catch (Exception exception)
            {
                Log.LogException(exception);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Метод за актуализиране на запис в БД
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="record"></param>
        public bool EditRecord(int ID, T record)
        {
            try
            {
                if (!UseExternalTransaction)
                    sqlTransaction = DataBaseConnection.Instance.sqlConnection.BeginTransaction(IsolationLevel.Serializable);

                if (UseExternalTransaction && externalTransaction == null)
                    throw new Exception("External transaction is null.");

                T rec = DefaultRecordConstructor();
                if (!SelectRecord(ID, out rec))
                {
                    throw new Exception("Record not found");
                }

                if (UseUpdateCounter)
                {
                    if (GetUpdateCounterValue(rec) != GetUpdateCounterValue(record))
                    {
                        throw new Exception(string.Format("The record with id {0} can't be updated because it is modified by another session.", ID));
                    }

                    IncreaseUpdateCounterValue(record);
                }

                string columnsAndParametersString = GetColumnsAndParametersStringForEdit();
                if (columnsAndParametersString.Equals(""))
                    throw new Exception("columnsAndParametersString is empty.");

                string sqlQuery = string.Format("UPDATE {0} SET {1} WHERE {2} = @ID", TableName, columnsAndParametersString, ColumnID);

                SqlCommand sqlCommand = CreateSqlCommand(sqlQuery);
                sqlCommand.Parameters.AddWithValue("@ID", ID);

                RecordToData(sqlCommand, record);

                sqlCommand.ExecuteNonQuery();

                if (!UseExternalTransaction)
                    sqlTransaction.Commit();
            }
            catch (Exception exception)
            {
                Log.LogException(exception);

                if (!UseExternalTransaction && sqlTransaction != null)
                    RollbackTransaction(sqlTransaction);

                DisposeTransaction();

                return false;
            }

            DisposeTransaction();

            return true;
        }

        /// <summary>
        /// Метод за изтриване на запис от БД по ID
        /// </summary>
        /// <param name="ID"></param>
        public bool DeleteRecord(int ID)
        {
            try
            {
                T record = DefaultRecordConstructor();
                if (!SelectRecord(ID, out record))
                {
                    return false;
                }

                string sqlQuery = string.Format("DELETE FROM {0} WHERE {1} = @ID", TableName, ColumnID);

                SqlCommand sqlCommand = CreateSqlCommand(sqlQuery);
                sqlCommand.Parameters.AddWithValue("@ID", ID);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Метод за синхронизация на записите
        /// </summary>
        /// <param name="newRecords"></param>
        /// <param name="whereClause"></param>
        public bool SyncRecords(List<T> newRecords, string whereClause = "")
        {
            List<T> oldRecords = new List<T>();
            if (!SelectAllRecords(oldRecords, whereClause))
            {
                return false;
            }

            List<T> tempList = newRecords.Where(x => !oldRecords.Any(y => GetIDValue(x) == GetIDValue(y))).ToList();
            foreach (T record in tempList)
            {
                if (!InsertRecord(record))
                {
                    return false;
                }
            }

            tempList = oldRecords.Where(x => !newRecords.Any(y => GetIDValue(x) == GetIDValue(y))).ToList();
            foreach (T record in tempList)
            {
                int ID = GetIDValue(record);
                if (!DeleteRecord(ID))
                {
                    return false;
                }
            }

            tempList = newRecords.Intersect(oldRecords, GetEqualityComparerByContent()).ToList();
            foreach (T newRecord in tempList)
            {
                if (!EditRecord(GetIDValue(newRecord), newRecord))
                {
                    return false;
                }
            }

            return true;
        }

        private string GetColumnsStringForInsert()
        {
            List<string> columnsList = GetColumns();

            string Columns = "";
            foreach (var column in columnsList)
                Columns += (column + ", ");

            if (!Columns.Equals(""))
                Columns = Columns.Remove(Columns.Length - 2); // remove last ', '

            return Columns;
        }

        private string GetParametersStringForInsert()
        {
            List<string> columnsList = GetColumns();

            string Params = "";
            for (int i = 0; i < columnsList.Count(); i++)
                Params += string.Format("@{0}, ", columnsList.ElementAt(i));

            if (!Params.Equals(""))
                Params = Params.Remove(Params.Length - 2); // remove last ', '

            return Params;
        }

        private string GetColumnsAndParametersStringForEdit()
        {
            string str = "";

            List<string> Columns = GetColumns();
            string ParametersString = GetParametersStringForInsert();
            string[] Parameters = ParametersString.Split(',');

            if (Columns.Count != Parameters.Count())
            {
                Log.LogError("Columns.Count != Parameters.Count()");
                return str;
            }

            for (int i = 0; i < Columns.Count; i++)
            {
                str += string.Format(" {0} = {1} ", Columns.ElementAt(i), Parameters.ElementAt(i));

                if (i < Columns.Count - 1)
                    str += ",";
            }

            return str;
        }

        protected SqlCommand CreateSqlCommand(string query)
        {
            SqlCommand sqlCommand = null;
            if (UseExternalTransaction)
            {
                if (externalTransaction == null)
                    throw new Exception("External transaction is null.");

                sqlCommand = new SqlCommand(query, DataBaseConnection.Instance.sqlConnection, externalTransaction);
            }
            else
            {
                if (sqlTransaction == null)
                {
                    sqlCommand = new SqlCommand(query, DataBaseConnection.Instance.sqlConnection);
                    return sqlCommand;
                }

                sqlCommand = new SqlCommand(query, DataBaseConnection.Instance.sqlConnection, sqlTransaction);
            }

            return sqlCommand;
        }

        private void DisposeTransaction()
        {
            if (sqlTransaction != null)
            {
                sqlTransaction.Dispose();
                sqlTransaction = null;
            }
        }

        private bool RollbackTransaction(SqlTransaction sqlTransaction)
        {
            try
            {
                if (sqlTransaction != null)
                    sqlTransaction.Rollback();
            }
            catch (Exception exception)
            {
                Log.LogException(exception);
                return false;
            }

            return true;
        }

        protected virtual IEqualityComparer<T> GetEqualityComparerByContent()
        {
            throw new NotImplementedException();
        }
    }
}
