using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diary.DataAccess;
using Diary.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace UnitTestsDiary
{
    [TestClass]
    public class DataAccessTest
    {
        string resConnect = @"Data Source=DESKTOP-EJ5LSTR\SQLEXPRESS; Initial Catalog=diary; Integrated Security=true;";

        [TestMethod]
        public void NoteRepositoryConstructorTest()
        {
            Assert.ThrowsException<Exception>(() => new NoteRepository("test"));
        }

        [TestMethod]
        public void GetNotesOfDayTest()
        {

            NoteRepository repository = new NoteRepository(resConnect);
            List<Note> loadList = new List<Note>();
            string query = $"SELECT * from dbo.Note WHERE Note_date=@Note_date";
            using (SqlConnection connection =
               new SqlConnection(resConnect))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Note_date", DateTime.Now.ToShortDateString());

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    loadList.Add(
                        new Note(
                                idNote: (int)reader[0],
                                noteData: (DateTime)reader[1],
                                typeJob: new TypeJobRepository(resConnect).GetTypeJob((int)reader[2]),
                                relevance: new RelevanceRepository(resConnect).GetdRelevance((int)reader[3]),
                                progress: new ProgressRepository(resConnect).GetProgress((int)reader[4]),
                                timeStart: (TimeSpan)reader[5],
                                timeFinish: (TimeSpan)reader[6]
                            )
                        );
                }
                reader.Close();
            }

            int countNotes = repository.GetNotesOfDay(DateTime.Now).Count;

            Assert.AreEqual(countNotes, loadList.Count);
        }

        [TestMethod]
        public void GetNotesOfDaysTest()
        {

            NoteRepository repository = new NoteRepository(resConnect);
            DateTime dateBegin = DateTime.Now.AddDays(-3);
            DateTime dateEnd = DateTime.Now;
            List<Note> loadList = new List<Note>();
            string query = $"SELECT * from dbo.Note WHERE Note_date >= @Note_dateBegin" +
                $" and Note_date <= @Note_dateEnd";
            using (SqlConnection connection =
              new SqlConnection(resConnect))
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
                                typeJob: new TypeJobRepository(resConnect).GetTypeJob((int)reader[2]),
                                relevance: new RelevanceRepository(resConnect).GetdRelevance((int)reader[3]),
                                progress: new ProgressRepository(resConnect).GetProgress((int)reader[4]),
                                timeStart: (TimeSpan)reader[5],
                                timeFinish: (TimeSpan)reader[6]
                            )
                        );
                }
                reader.Close();
            }

            int countNotes = repository.GetNotesOfDays(dateBegin, dateEnd).Count;

            Assert.AreEqual(countNotes, loadList.Count);
        }

        [TestMethod]
        public void GetAllNotesTest()
        {

            NoteRepository repository = new NoteRepository(resConnect);
            DateTime dateBegin = DateTime.Now.AddDays(-3);
            DateTime dateEnd = DateTime.Now;
            List<Note> loadList = new List<Note>();
            string query = $"SELECT * from dbo.Note";
            using (SqlConnection connection =
              new SqlConnection(resConnect))
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
                                typeJob: new TypeJobRepository(resConnect).GetTypeJob((int)reader[2]),
                                relevance: new RelevanceRepository(resConnect).GetdRelevance((int)reader[3]),
                                progress: new ProgressRepository(resConnect).GetProgress((int)reader[4]),
                                timeStart: (TimeSpan)reader[5],
                                timeFinish: (TimeSpan)reader[6]
                            )
                        );
                }
                reader.Close();
            }

            int countNotes = repository.GetAllNotes().Count;

            Assert.AreEqual(countNotes, loadList.Count);
        }

        [TestMethod]
        public void AddNoteTest()
        {
            NoteRepository repository = new NoteRepository(resConnect);

            Assert.ThrowsException<ArgumentNullException>(() => repository.AddNote(null));
        }

        [TestMethod]
        public async Task AddNoteAsyncTest()
        {
            NoteRepository repository = new NoteRepository(resConnect);

            repository.AddNoteAsync(null);
        }

        [TestMethod]
        public void UpdateNoteTest()
        {
            NoteRepository repository = new NoteRepository(resConnect);

            Assert.ThrowsException<ArgumentNullException>(() => repository.UpdateNote(null));

        }

        [TestMethod]
        public void RemoveNoteTest()
        {
            NoteRepository repository = new NoteRepository(resConnect);

            Assert.ThrowsException<ArgumentNullException>(() => repository.RemoveNote(null));
        }

        [TestMethod]
        public void RemoveNotesTest()
        {

            NoteRepository repository = new NoteRepository(resConnect);
            repository.RemoveNotes(DateTime.Now);

            int contNotes = repository.GetNotesOfDay(DateTime.Now).Count;

            Assert.AreEqual(contNotes, 0);
        }

    }
}
