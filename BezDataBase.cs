using System.Data;
using System.Data.OleDb;

namespace Bezetting2
{
    class BezDataBase
    {
        private string cnn;
        private OleDbConnection connection;
        private OleDbDataAdapter dataadapter;
        public DataSet datasetFull;

        public BezDataBase(string databaselocatie)
        {
            cnn = string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = \"{0}\"; Jet OLEDB:Database Password = fcl721", databaselocatie);

            string sql = "SELECT * FROM BEZETTING";

            connection = new OleDbConnection(cnn);
            connection.Open();

            dataadapter = new OleDbDataAdapter(sql, connection);
            datasetFull = new DataSet();
            dataadapter.Fill(datasetFull, "Namen_table");
            connection.Close();
        }

        //public void InsertRow(string connectionString, string insertSQL)
        //{
        //    using (OleDbConnection connection = new OleDbConnection(connectionString))
        //    {
        //        // The insertSQL string contains a SQL statement that
        //        // inserts a new row in the source table.
        //        OleDbCommand command = new OleDbCommand(insertSQL);

        //        // Set the Connection to the new OleDbConnection.
        //        command.Connection = connection;

        //        // Open the connection and execute the insert command.
        //        try
        //        {
        //            connection.Open();
        //            command.ExecuteNonQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //        // The connection is automatically closed when the
        //        // code exits the using block.
        //    }
        //}




















        /// <summary>
        /// Maak dataview met juiste filte
        /// </summary>
        /// <param name="tabel">tabel welk gebruikt wordt</param>
        /// <param name="filter">string naar filter</param>
        /// <param name="sorteer">string hoe sorteren</param>
        /// <returns>dataview</returns>
        public DataView ZetFilter(DataTable tabel, string filter, string sorteer)
        {
            DataView dataView;
            dataView = new DataView(tabel, @filter, sorteer, DataViewRowState.CurrentRows);
            return dataView;
        }


        public DataTable GetTabel(int nummer)
        {
            return datasetFull.Tables[nummer];
        }
    }
}
