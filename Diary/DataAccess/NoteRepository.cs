using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Diary.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Diary.DataAccess
{
    public class NoteRepository
    {
        #region Fields

        string connectionString;

        DateTime curTime;

        List<Note> notesSelectDay;

        #endregion // Fields

        #region Constructor

        public NoteRepository(string connectionString)
        {
            CheckConnect(connectionString);

            this.connectionString = connectionString;
        }

        #endregion // Constructor

        #region Public methods

        public List<Note> GetNotesOfDay(DateTime date)
        {
            if (date.ToShortDateString() != curTime.ToShortDateString())
            {
                string query = $"SELECT * from dbo.Note WHERE Note_date=@Note_date";
                notesSelectDay = LoadData(query, date);
                curTime = date;

                return new List<Note>(notesSelectDay);
            }

            return new List<Note>(notesSelectDay);
        }

        public List<Note> GetNotesOfDays(DateTime dateBegin, DateTime dateEnd)
        {
            string query = $"SELECT * from dbo.Note WHERE Note_date >= @Note_dateBegin" +
                $" and Note_date <= @Note_dateEnd";
            return LoadData(query, dateBegin, dateEnd);
        }

        public List<Note> GetAllNotes()
        {
            string query = $"SELECT * from dbo.Note";
            return LoadData(query);
        }

        public async void AddNoteAsync(Note note)
        {
            int idNewNote = GetCountNotes() + 1;

            string query = $"Insert Into dbo.Note (Id_note, Note_date, Id_type_job, Id_relevance, Id_progress, Time_start, Time_finish)" +
                $" values (@Id_note, @Note_date, @Id_type_job, @Id_relevance, @Id_progress, @Time_start, @Time_finish)";
            
            if (note.NoteDate.ToShortDateString() == curTime.ToShortDateString())
            {
                notesSelectDay.Add(note);
            }

            await Task.Run(() => DumpData(query, note, idNewNote));
        }
        public void AddNote(Note note)
        {
            int idNewNote = GetCountNotes() + 1;

            string query = $"Insert Into dbo.Note (Id_note, Note_date, Id_type_job, Id_relevance, Id_progress, Time_start, Time_finish)" +
                $" values (@Id_note, @Note_date, @Id_type_job, @Id_relevance, @Id_progress, @Time_start, @Time_finish)";

            if (note.NoteDate.ToShortDateString() == curTime.ToShortDateString())
            {
                notesSelectDay.Add(note);
            }

            DumpData(query, note, idNewNote);
        }
        public async void UpdateNoteAsync(Note note)
        {
            string query = $"Update dbo.Note " +
                $" SET Note_date=@Note_date, " +
                $" Id_type_job=@Id_type_job, Id_relevance=@Id_relevance, " +
                $" Id_progress=@Id_progress, Time_start=@Time_start, Time_finish=@Time_finish" +
                $" WHERE Id_note=@Id_note";

            await Task.Run(() => DumpData(query, note, note.IdNote));
        }

        public void RemoveNote(Note note)
        {
            string query = $"DELETE from dbo.Note WHERE Id_note=@Id_note";

            curTime = new DateTime();

            DumpData(query, note.IdNote);
        }

        public async void RemoveNoteAsync(Note note)
        {
            string query = $"DELETE from dbo.Note WHERE Id_note=@Id_note";

            await Task.Run(() => DumpData(query, note.IdNote));
        }
        public void RemoveNotes(DateTime date)
        {
            string query = $"DELETE from dbo.Note WHERE Note_date=@Note_date";

            DumpData(query, date);
        }
        public async void RemoveNotesAsync(DateTime date)
        {
            string query = $"DELETE from dbo.Note WHERE Note_date=@Note_date";

            await Task.Run(() => DumpData(query, date));
        }

        #endregion // Public methods

        #region Private methods

        void CheckConnect(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
            }
        }

        int GetCountNotes()
        {
            int count = 0;
            using (SqlConnection connection =
                           new SqlConnection(connectionString))
            {
                string query = $"SELECT max(Id_note) from dbo.Note";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    count = (int)reader[0];
                }
                reader.Close();
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
                                typeJob: new TypeJobRepository(connectionString).GetTypeJob((int)reader[2]),
                                relevance: new RelevanceRepository(connectionString).GetdRelevance((int)reader[3]),
                                progress: new ProgressRepository(connectionString).GetProgress((int)reader[4]),
                                timeStart: (TimeSpan)reader[5],
                                timeFinish: (TimeSpan)reader[6]
                            )
                        );
                }
                reader.Close();
            }

            return loadList;
        }
        List<Note> LoadData(string query, DateTime date)
        {
            List<Note> loadList = new List<Note>();

            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Note_date", date.ToShortDateString());


                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    loadList.Add(
                        new Note(
                                idNote: (int)reader[0],
                                noteData: (DateTime)reader[1],
                                typeJob: new TypeJobRepository(connectionString).GetTypeJob((int)reader[2]),
                                relevance: new RelevanceRepository(connectionString).GetdRelevance((int)reader[3]),
                                progress: new ProgressRepository(connectionString).GetProgress((int)reader[4]),
                                timeStart: (TimeSpan)reader[5],
                                timeFinish: (TimeSpan)reader[6]
                            )
                        );
                }
                reader.Close(); ;
            }

            return loadList;
        }
        List<Note> LoadData(string query, DateTime dateBegin, DateTime dateEnd)
        {
            List<Note> loadList = new List<Note>();

            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Note_dateBegin", dateBegin.ToShortDateString());
                command.Parameters.AddWithValue("@Note_dateEnd", dateEnd.ToShortDateString());

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    loadList.Add(
                        new Note(
                                idNote: (int)reader[0],
                                noteData: (DateTime)reader[1],
                                typeJob: new TypeJobRepository(connectionString).GetTypeJob((int)reader[2]),
                                relevance: new RelevanceRepository(connectionString).GetdRelevance((int)reader[3]),
                                progress: new ProgressRepository(connectionString).GetProgress((int)reader[4]),
                                timeStart: (TimeSpan)reader[5],
                                timeFinish: (TimeSpan)reader[6]
                            )
                        );
                }
                reader.Close();
            }

            return loadList;
        }

        void DumpData(string query, int idNote)
        {
            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id_note", idNote);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        void DumpData(string query, DateTime date)
        {
            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Note_date", date.ToShortDateString());

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        void DumpData(string query, Note note, int idNote)
        {
            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id_note", idNote);
                command.Parameters.AddWithValue("@Note_date", note.NoteDate.ToShortDateString());
                command.Parameters.AddWithValue("@Id_type_job", note.TypeJob.IdTypeJob);
                command.Parameters.AddWithValue("@Id_relevance", note.Relevance.IdRelevance);
                command.Parameters.AddWithValue("@Id_progress", note.Progress.IdProgress);
                command.Parameters.AddWithValue("@Time_start", note.TimeStart);
                command.Parameters.AddWithValue("@Time_finish", note.TimeFinish);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        #endregion // Private methods
    }
}
