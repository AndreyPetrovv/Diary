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
            try
            {
                CheckConnect(connectionString);
                this.connectionString = connectionString;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion // Constructor

        #region Public methods

        public List<Relevance> GetAllRelevances()
        {
            string query = "SELECT *  from dbo.Relevance";

            return LoadData(query);
        }

        public Relevance GetdRelevance(int idRelevance)
        {
            string query = $"SELECT *  from dbo.Relevance WHERE Id_relevance=@Id_relevance";

            return LoadData(query, idRelevance)[0];
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
        List<Relevance> LoadData(string query)
        {

            List<Relevance> loadList = new List<Relevance>();

            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

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
        List<Relevance> LoadData(string query, int idRelevance)
        {

            List<Relevance> loadList = new List<Relevance>();

            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id_relevance", idRelevance);

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
