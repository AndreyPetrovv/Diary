using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Diary.Model;


namespace Diary.DataAccess
{
    public class RelevanceRepository
    {
        #region Fields

        string connectionString;

        #endregion // Fields

        #region Constructor

        public RelevanceRepository(string connectionString)
        {
            if (!CheckConnect(connectionString))
            {
                throw new Exception();

            }

            this.connectionString = connectionString;
        }

        #endregion // Constructor

        #region Public methods

        public List<Relevance> GetAllRelevances()
        {
            string query = "SELECT *  from dbo.Relevance";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);

            return LoadData(query, connection, command);
        }

        public Relevance GetdRelevance(int idRelevance)
        {
            string query = $"SELECT *  from dbo.Relevance WHERE Id_relevance=@Id_relevance";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id_relevance", idRelevance);

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
        List<Relevance> LoadData(string query, SqlConnection connection, SqlCommand command)
        {

            List<Relevance> loadList = new List<Relevance>();

            using (connection)
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    loadList.Add(
                        new Relevance(
                                idRelevance: (int)reader[0],
                                levelRelevance: (string)reader[1]
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
