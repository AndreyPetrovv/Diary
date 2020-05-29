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
        readonly List<Relevance> _relevances;

        #endregion // Fields

        #region Constructor

        public RelevanceRepository(string connectionString)
        {
            this.connectionString = connectionString;
            this._relevances = LoadAllData();
        }

        #endregion // Constructor

        public List<Relevance> GetRelevances()
        {
            return new List<Relevance>(_relevances);
        }

        List<Relevance> LoadAllData()
        {

            List<Relevance> loadList = new List<Relevance>();

            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                string queryString = "SELECT *  from dbo.Relevance";
                SqlCommand command = new SqlCommand(queryString, connection);

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
    }
}
