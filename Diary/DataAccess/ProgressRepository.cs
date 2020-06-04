using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Diary.Model;


namespace Diary.DataAccess
{
    public class ProgressRepository
    {
        #region Fields

        string connectionString;

        #endregion // Fields

        #region Constructor

        public ProgressRepository(string connectionString)
        {
            CheckConnect(connectionString);
            this.connectionString = connectionString;
        }

        #endregion // Constructor

        #region Public methods

        public List<Progress> GetAllProgresses()
        {
            string query = "SELECT *  from dbo.Progress";

            return LoadData(query);
        }

        public Progress GetProgress(int IdProgress)
        {
            string query = $"SELECT *  from dbo.Progress WHERE Id_progress={IdProgress}";

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
        List<Progress> LoadData(string query)
        {

            List<Progress> loadList = new List<Progress>();

            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    loadList.Add(
                        new Progress(
                                idProgress: (int)reader[0],
                                statusProgress: (string)reader[1]
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
