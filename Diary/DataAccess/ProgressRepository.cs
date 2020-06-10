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
            if (!CheckConnect(connectionString))
            {
                throw new Exception();

            }

            this.connectionString = connectionString;
        }

        #endregion // Constructor

        #region Public methods

        public List<Progress> GetAllProgresses()
        {
            string query = "SELECT *  from dbo.Progress";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);

            return LoadData(query, connection, command);
        }

        public Progress GetProgress(int idProgress)
        {
            string query = $"SELECT *  from dbo.Progress WHERE Id_progress=@Id_progress";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id_progress", idProgress);

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
        List<Progress> LoadData(string query, SqlConnection connection, SqlCommand command)
        {

            List<Progress> loadList = new List<Progress>();

            using (connection)
            {

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
