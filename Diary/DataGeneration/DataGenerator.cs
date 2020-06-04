using System;
using System.Collections.Generic;
using System.Text;
using Diary.DataAccess;
using Diary.Model;

namespace Diary.DataGeneration
{
    public class DataGenerator
    {
        #region Public methods

        public void GenerateNotes(NoteRepository noteRepository)
        {
            Random saintRandom = new Random();
            DateTime dateNote = DateTime.Now.AddDays(-3);

            for (int i = 0; i < 35; i++)
            {
                noteRepository.RemoveNotes(dateNote);

                int[] timeLines = GetTimeLines();
                for (int j = 0; j < timeLines.Length-1; j+=1)
                {
                    Note note = new Note();
                    note.NoteDate = dateNote;
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
