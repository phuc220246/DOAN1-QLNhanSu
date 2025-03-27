using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUANQLNS.DAL
{
    public class DataHelper
    {
        private static DataHelper instance;



        public static DataHelper Instance
        {
            get { if (instance == null) instance = new DataHelper(); return DataHelper.instance; }
            private set { DataHelper.instance = value; }
        }
        public string getConectionstr()
        {
            return ConnectionString;
        }

        private DataHelper() { }

        public string ConnectionString { get; private set; } = "Data Source=Buddho\\SQLEXPRESS;Initial Catalog=DuAnQLNS;Integrated Security=True;Encrypt=False";


        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    for (int i = 0; i < parameter.Length; i++)
                    {

                        command.Parameters.AddWithValue($"@param{i}", parameter[i]);
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
                //connection.Close();
            }
            return data;
        }

        //public object ExecuteScalar(string query, object[] parameter = null)
        //{
        //    object data = 0;
        //    using (SqlConnection connection = new SqlConnection(ConnectionString))
        //    {
        //        connection.Open();
        //        SqlCommand command = new SqlCommand(query, connection);
        //        if (parameter != null)
        //        {
        //            string[] listPara = query.Split(' ');
        //            int i = 0;
        //            foreach (string item in listPara)
        //            {
        //                if (item.Contains('@'))
        //                {
        //                    command.Parameters.AddWithValue(item, parameter[i]);
        //                    i++;
        //                }
        //            }
        //        }
        //        data = command.ExecuteScalar();

        //        connection.Close();
        //    }
        //    return data;
        //}
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameter != null)
                    {
                        for (int i = 0; i < parameter.Length; i++)
                        {
                            command.Parameters.AddWithValue($"@param{i}", parameter[i]);
                        }
                    }
                    data = command.ExecuteScalar();
                }
                // connection.Close(); -> Không cần vì using tự đóng
            }
            return data;
        }



    }
}

