using Pharmacy;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class DataBaseConnection
{
    #region Declarations

    public SqlConnection sqlConnection = null;
    private static DataBaseConnection databaseInstance = null;

    public static string ConnectionString { get { return "Data Source=ASUS-PC\\SQLEXPRESS;Initial Catalog=Pharmacy;Integrated Security=True"; } }

    public event EventHandler OnConnectingStart = null;
    public event EventHandler OnConnectingEnd = null;
    public event EventHandler<EventArgs<Exception>> OnConnectingError = null;

    #endregion

    private DataBaseConnection()
    {
    }

    public static DataBaseConnection Instance
    {
        get
        {
            if (databaseInstance == null)
                databaseInstance = new DataBaseConnection();

            return databaseInstance;
        }
    }

    private void Init()
    {
        sqlConnection = new SqlConnection();
        sqlConnection.ConnectionString = ConnectionString;
    }

    public static void OpenConnection()
    {
        if (databaseInstance == null)
        {
            databaseInstance = new DataBaseConnection();
            Log.LogWarning("Database instance is null!");
        }

        if (databaseInstance.sqlConnection != null)
        {
            Log.LogError("Database sql connection is different from null! First close the current connection then open a new one.");
            return;
        }

        databaseInstance.Init();

        try
        {
            if (databaseInstance.OnConnectingStart != null)
                databaseInstance.OnConnectingStart(databaseInstance.sqlConnection, null);

            Task.Run(() => databaseInstance.sqlConnection.Open());

            if (databaseInstance.OnConnectingEnd != null)
                databaseInstance.OnConnectingEnd(databaseInstance.sqlConnection, null);
        }
        catch (Exception exception)
        {
            Log.LogException(exception);

            if (databaseInstance.OnConnectingError != null)
                databaseInstance.OnConnectingError(databaseInstance.sqlConnection, new EventArgs<Exception>(exception));
        }
    }

    public static void CloseConnection()
    {
        if (databaseInstance == null)
        {
            throw new Exception("databaseInstance == null");
        }

        if (databaseInstance.sqlConnection == null)
        {
            return;
        }

        try
        {
            databaseInstance.sqlConnection.Close();
            databaseInstance.sqlConnection.Dispose();
            databaseInstance.sqlConnection = null;

            Log.LogInfo("Connection closed.");
        }
        catch (Exception exception)
        {
            Log.LogException(exception);
        }
    }
}

public class EventArgs<T> : EventArgs
{
    public T Value { get; private set; }

    public EventArgs(T value)
    {
        Value = value;
    }
}
