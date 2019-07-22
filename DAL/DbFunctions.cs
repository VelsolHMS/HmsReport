using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DbFunctions
    {
        public static T ExecuteCommand<T>(string cmdText, List<SqlParameter> listParams, CommandType cmdType = CommandType.Text)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sqlconstring"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;
                if (listParams != null)
                {
                    foreach (var p in listParams)
                    {
                        cmd.Parameters.Add(p);
                    }
                }

                if (typeof(T) == typeof(int))
                {
                    con.Open();
                    var count = cmd.ExecuteNonQuery();
                    con.Close();
                    return (T)(object)count;
                }
                else if (typeof(T) == typeof(object))
                {
                    con.Open();
                    var result = cmd.ExecuteScalar();
                    con.Close();
                    return (T)result;
                }  
                else if (typeof(T) == typeof(DataTable))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    da.Fill(dt);
                    return (T)(object)dt;
                }
                else
                {
                    return default(T);
                }

            }
        }
    }
}
    
    