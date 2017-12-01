using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace DeviseConverter.Utility
{
    public class DB
    {
        private MySqlConnection connection;
        public static readonly int ERROR_RETURN = -1;

        public bool CloseConnection()
        {
            if (this.connection != null)
            {
                this.connection.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable ExecuteSql(MySqlCommand command)
        {
            DataTable table = new DataTable();

            try
            {
                if (this.connection == null)
                {
                    this.connection = new MySqlConnection("SERVER=localhost; DATABASE=osef; UID=root; PASSWORD=;Convert Zero Datetime=True");
                }
               
                command.Connection = this.connection;
                command.Connection.Open();

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    adapter.Fill(table);
                    //foreach (DataRow row in table.Rows)
                    //{
                    //    Debug.WriteLine(row.Field<string>("UserName"));
                    //}
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("=================== EXCEPTION ====================");
                Debug.WriteLine(ex.ToString());
                Debug.WriteLine("=================== FIN EXCEPTION ====================");
            }

            return table;
        }

        public void ExecuteNonQuery(MySqlCommand command)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = new MySqlConnection("SERVER=localhost; DATABASE=osef; UID=root; PASSWORD=");
                    connection.Open();
                }
                command.Connection = this.connection;
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("=================== EXCEPTION ====================");
                Debug.WriteLine(ex.ToString());
                Debug.WriteLine("=================== FIN EXCEPTION ====================");
            }
        }

        public int ExecuteScalar(MySqlCommand command)
        {
            int result = DB.ERROR_RETURN;
            try
            {
                if (this.connection == null)
                {
                    this.connection = new MySqlConnection("SERVER=localhost; DATABASE=osef; UID=root; PASSWORD=");
                    connection.Open();
                }
                command.Connection = this.connection;
                result = Convert.ToInt32(command.ExecuteScalar());
                return result;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("=================== EXCEPTION ====================");
                Debug.WriteLine(ex.ToString());
                Debug.WriteLine("=================== FIN EXCEPTION ====================");
                return result;
            }
        }

        public static FieldType GetMapping<FieldType>(DataRow row, string columnname, FieldType returnvalue)
        {
            if (row == null) throw new ArgumentNullException("Le DataRow est null !");
            if (string.IsNullOrWhiteSpace(columnname)) throw new ArgumentNullException("Le nom de la colonne est null !");

            return Utilities.iif(row[columnname] != null, row.Field<FieldType>(columnname), returnvalue);
        }

        public static int lazyExecuteScalar(MySqlCommand cmd)
        {
            DB hlp = new DB();
            return hlp.ExecuteScalar(cmd);
        }

        public static DataTable lazyExecuteQuery(MySqlCommand cmd)
        {
            DB hlp = new DB();
            return (hlp.ExecuteSql(cmd));
        }
    }
}
