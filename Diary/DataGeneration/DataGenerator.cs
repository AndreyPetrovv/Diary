using System;
using System.Collections.Generic;
using Diary.DataAccess;
using Diary.Model;
using System.Threading.Tasks;

namespace Diary.DataGeneration
{
    public class DataGenerator
    {
        #region Public methods

        public async void GenerateNotesAsync(
            NoteRepository noteRepository,
            int countNotes
            )
        {
            if(noteRepository != null && countNotes > 0)
            {
                await Task.Run(() => GenerateNotes(noteRepository, countNotes));
            }
            else
            {
                throw new ArgumentException("noteRepository is null or countNotes <0");
            }
        }

        public void GenerateNotes(
            NoteRepository noteRepository,
            int countNotes
            )
        {
            if (noteRepository == null || countNotes < 1)
            {
                throw new ArgumentException("noteRepository is null or countNotes <0");
            }

            Random saintRandom = new Random();
            DateTime dateNote = DateTime.Now.AddDays(-1);

            List<TypeJob> typeJobs = new TypeJobRepository(Properties.Resources.ConnectCommand).GetAllTypeJobs();
            List<Relevance> relevances = new RelevanceRepository(Properties.Resources.ConnectCommand).GetAllRelevances();
            List<Progress> progresses = new ProgressRepository(Properties.Resources.ConnectCommand).GetAllProgresses();


            for (int i = 0; i < countNotes; i++)
            {
                noteRepository.RemoveNotes(dateNote);

                int[] timeLines = GetTimeLines();
                for (int j = 0; j < timeLines.Length-1; j+=1)
                {
                    Note note = new Note();

                    note.NoteDate = dateNote;
                    note.TypeJob = typeJobs[saintRandom.Next(1, 6)];
                    note.Relevance = relevances[saintRandom.Next(1, 4)];
                    note.Progress = progresses[saintRandom.Next(1, 4)];
                    note.TimeStart = new TimeSpan(timeLines[j], 1, 0);
                    note.TimeFinish = new TimeSpan(timeLines[j+1], 0, 0);

                    noteRepository.AddNote(note);
                }

                dateNote = dateNote.AddDays(-1);

            }

        }

        #endregion // Public methods

        #region Private methods

        int[] GetTimeLines()
        {
            int step = 2;

            int[] timeLines = new int[12];


            for (int i = 0, time = 0; i < 12; i++)
            {
                timeLines[i] = time;
                time += step;
            }

            return timeLines;
        }

        #endregion //Private methods
    }
}
