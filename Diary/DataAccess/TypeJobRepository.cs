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
            CheckConnect(connectionString);
            this.connectionString = connectionString;
        }

        #endregion // Constructor

        #region Public methods

        public List<TypeJob> GetAllTypeJobs()
        {
            string query = $"SELECT *  from dbo.Type_job";

            return LoadData(query);
        }

        public TypeJob GetTypeJob(int IdTypeJob)
        {
            string query = $"SELECT *  from dbo.Type_job WHERE Id_type_job={IdTypeJob}";

            return LoadData(query)[0];
        }
        #endregion // Public methods

        #region Private methods

        void CheckConnect(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Close();
            }
        }
        List<TypeJob> LoadData(string query)
        {

            List<TypeJob> loadList = new List<TypeJob>();

            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                
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
