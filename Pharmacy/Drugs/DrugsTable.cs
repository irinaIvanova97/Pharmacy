using Pharmacy.DataBase;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Pharmacy.Drugs
{
    public class DrugsTable : BaseTable<Drugs>
    {
        public DrugsTable() : base("DRUGS", "ID", false)
        {

        }


        // Overrides
        // ---------
        protected override Drugs DataToRecord(SqlDataReader reader)
        {
            Drugs record = new Drugs();
            record.ID = reader.GetInt32(Drugs.ColumnID);
            record.Name = reader.GetString(Drugs.ColumnName);
            record.image = byteArrayToImage(reader.GetSqlBytes(Drugs.ColumnImage).Value);

            return record;
        }

        protected override Drugs DefaultRecordConstructor()
        {
            return new Drugs();
        }

        protected override List<string> GetColumns()
        {
            return new List<string> { "NAME", "IMAGE" };
        }

        protected override int GetIDValue(Drugs record)
        {
            return record.ID;
        }

        protected override void RecordToData(SqlCommand command, Drugs record)
        {
            command.Parameters.Add("@NAME", SqlDbType.NVarChar, Drugs.NameSize).Value = record.Name;
            command.Parameters.Add("@IMAGE", SqlDbType.VarBinary).Value = imageToByteArray(new PngBitmapEncoder(), record.image);
        }

        protected override void SetIDValue(int ID, Drugs record)
        {
            record.ID = ID;
        }

        private ImageSource byteArrayToImage(byte[] imgBytes)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imgBytes);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }

        private byte[] imageToByteArray(BitmapEncoder encoder, ImageSource imageSource)
        {
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

    }
}
