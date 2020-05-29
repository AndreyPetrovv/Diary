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
        readonly List<TypeJob> _typeJobs;

        #endregion // Fields

        #region Constructor

        public TypeJobRepository(string connectionString)
        {
            this.connectionString = connectionString;
            this._typeJobs = LoadAllData();
        }

        #endregion // Constructor

        public List<TypeJob> GetTypeJobs()
        {
            return new List<TypeJob>(_typeJobs);
        }

        List<TypeJob> LoadAllData()
        {

            List<TypeJob> loadList = new List<TypeJob>();

            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                string queryString = "SELECT *  from dbo.Type_job";
                SqlCommand command = new SqlCommand(queryString, connection);
                
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

    }
}
