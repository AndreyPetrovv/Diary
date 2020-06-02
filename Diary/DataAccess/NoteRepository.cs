using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Diary.Model;


namespace Diary.DataAccess
{
    public class NoteRepository
    {
        #region Fields

        string connectionString;

        #endregion // Fields

        #region Constructor

        public NoteRepository(string connectionString)
        {
            CheckConnect(connectionString);

            this.connectionString = connectionString;
        }

        #endregion // Constructor

        public List<Note> GetNotesOfDay(DateTime date)
        {
            string query = $"SELECT *  from dbo.Note WHERE Note_date='{date.Date.ToShortDateString()}'";
            return LoadData(query);
        }
        public List<Note> GetAllNotes()
        {
            string query = $"SELECT *  from dbo.Note";
            return LoadData(query);
        }

        public void AddNote(Note note)
        {
            int idNewNote = GetCountNotes() + 1;
            string query = $"Insert Into dbo.Note (Id_note, Note_date, Id_type_job, Id_relevance, Id_progress, Time_start, Time_finish)" +
                $" values ({idNewNote}, @Note_date, @Id_type_job, @Id_relevance, @Id_progress, @Time_start, @Time_finish)";

            DumpData(query, note);
        }
        public void UpdateNote(Note note)
        {
            string query = $"Update dbo.Note " +
                $" SET Note_date=@Note_date, " +
                $" Id_type_job=@Id_type_job, Id_relevance=@Id_relevance, " +
                $" Id_progress=@Id_progress, Time_start=@Time_start, Time_finish=@Time_finish" +
                $" WHERE Id_note={note.IdNote}";

            DumpData(query, note);
        }
        public void RemoveNote(Note note)
        {
            string query = $"DELETE from dbo.Note WHERE Id_note={note.IdNote}";

            DumpData(query);
        }
        public void RemoveNotes(DateTime date)
        {
            string query = $"DELETE from dbo.Note WHERE Note_date='{date.ToShortDateString()}'";

            DumpData(query);
        }

        void CheckConnect(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Close();
            }
        }

        int GetCountNotes()
        {
            int count = 0;
            using (SqlConnection connection =
                           new SqlConnection(connectionString))
            {
                string query = $"SELECT COUNT(*) from dbo.Note";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    count = (int)reader[0];
                }
                reader.Close();
                connection.Close();

            }

            return count;
        }

        List<Note> LoadData(string query)
        {
            List<Note> loadList = new List<Note>();

            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    loadList.Add(
                        new Note(
                                idNote: (int)reader[0],
                                noteData: (DateTime)reader[1],
                                idTypeJob: (int)reader[2],
                                idRelevance: (int)reader[3],
                                idProgress: (int)reader[4],
                                timeStart: (TimeSpan) reader[5],
                                timeFinish: (TimeSpan)reader[6]
                            )
                        );
                }
                reader.Close();
                connection.Close();

            }

            return loadList;
        }

        void DumpData(string query)
        {
            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                command.ExecuteNonQuery();

                connection.Close();
            }
        }
        void DumpData(string query, Note note)
        {
            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Note_date", note.NoteDate.ToShortDateString());
                command.Parameters.AddWithValue("@Id_type_job", note.IdTypeJob);
                command.Parameters.AddWithValue("@Id_relevance", note.IdRelevance);
                command.Parameters.AddWithValue("@Id_progress", note.IdProgress);
                command.Parameters.AddWithValue("@Time_start", note.TimeStart);
                command.Parameters.AddWithValue("@Time_finish", note.TimeFinish);

                connection.Open();
                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
