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
        readonly List<Note> _notes;

        #endregion // Fields

        #region Constructor

        public NoteRepository(string connectionString)
        {
            this.connectionString = connectionString;
            this._notes = LoadAllData();
        }

        #endregion // Constructor

        public void AddNote(Note note)
        {
            _notes.Add(note);
        }
        public void RemoveNote(Note note)
        {
            _notes.Remove(note);
        }

        public List<Note> GetNotes()
        {
            return new List<Note>(_notes);
        }

        List<Note> LoadAllData()
        {

            List<Note> loadList = new List<Note>();

            using (SqlConnection connection =
               new SqlConnection(connectionString))
            {
                string queryString = "SELECT *  from dbo.Note";
                SqlCommand command = new SqlCommand(queryString, connection);

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
            }

            return loadList;
        }
    }
}
