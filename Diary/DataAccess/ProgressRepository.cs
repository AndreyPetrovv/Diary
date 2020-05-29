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
        readonly List<Progress> _progresses;

        #endregion // Fields

        #region Constructor

        public ProgressRepository(string connectionString)
        {
            this.connectionString = connectionString;
            this._progresses = LoadAllData();
        }

        #endregion // Constructor

        public List<Progress> GetProgresses()
        {
            return new List<Progress>(_progresses);
        }

        List<Progress> LoadAllData()
        {

            List<Progress> loadList = new List<Progress>();

            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                string queryString = "SELECT *  from dbo.Progress";
                SqlCommand command = new SqlCommand(queryString, connection);

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
    }
}
