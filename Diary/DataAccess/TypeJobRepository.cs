using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Diary.Model;

namespace Diary.DataAccess
{
    public class TypeJobRepository
    {
        #region Fields

        string connectionString;

        #endregion // Fields

        #region Constructor

        public TypeJobRepository(string connectionString)
        {
            if (!CheckConnect(connectionString))
            {
                throw new Exception();

            }

            this.connectionString = connectionString;
        }

        #endregion // Constructor

        #region Public methods

        public List<TypeJob> GetAllTypeJobs()
        {
            string query = $"SELECT *  from dbo.Type_job";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);


            return LoadData(query, connection, command);
        }

        public TypeJob GetTypeJob(int idTypeJob)
        {
            string query = $"SELECT *  from dbo.Type_job WHERE Id_type_job=@Id_type_job";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id_type_job", idTypeJob);

            return LoadData(query, connection, command)[0];
        }
        #endregion // Public methods

        #region Private methods

        bool CheckConnect(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        List<TypeJob> LoadData(string query, SqlConnection connection, SqlCommand command)
        {

            List<TypeJob> loadList = new List<TypeJob>();

            using (connection)
            {
                
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    loadList.Add(
                        new TypeJob(
                                idTypeJob: (int)reader[0],
                                nameTypeJob: (string)reader[1]
                            )
                        );
                }
                reader.Close();
            }

            return loadList;
        }

        #endregion // Private methods
    }
}
