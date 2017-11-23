using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Collections;

namespace Kalender
{
    class Database
    {
        string myConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\db.mdf;Integrated Security=True";

        public Database()
        {
            
        }
        

        public DataSet selectTermin()
        {
            string sql = "SELECT * FROM dbo.Termine";
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(myConnection))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        conn.Open();
                        da.Fill(ds, "Termine");
                        conn.Close();
                        return ds;
                    }
                    
                }
                
            }
            
        }

        public void InsertData(int id, string bez, DateTime von, DateTime bis)
        {
            string sql = @"INSERT INTO dbo.Termine
                           (tn_id, tn_bez, tn_von, tn_bis)
                           VALUES
                           (@id, @bezeichnung, @von, @bis)";
            using (SqlConnection conn = new SqlConnection(myConnection))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@bezeichnung", bez);
                    cmd.Parameters.AddWithValue("@von", von);
                    cmd.Parameters.AddWithValue("@bis", bis);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            
        }
    }
}
