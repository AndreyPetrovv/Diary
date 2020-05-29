using System;
using System.Collections.Generic;
using System.Text;

namespace Diary.Model
{
    public class Note
    {
        #region Constructor

        public Note(
            int idNote,
            DateTime noteData,
            int idTypeJob,
            int idRelevance,
            int idProgress,
            TimeSpan timeStart,
            TimeSpan timeFinish
            )
        {
            IdNote  = idNote;
            NoteData = noteData;
            IdTypeJob = idTypeJob;
            IdRelevance = idRelevance;
            IdProgress = idProgress;
            TimeStart = timeStart;
            TimeFinish = timeFinish;
        }

        #endregion // Constructor

        #region State Properties

        /// <summary>
        /// 
        /// </summary>
        public int IdNote { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime NoteData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IdTypeJob { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IdRelevance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IdProgress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan TimeStart { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan TimeFinish { get; set; }

        #endregion // State Properties

        public string GetString => $"Note data {NoteData}, TypeJob {IdTypeJob}, Relevance {IdRelevance}";

    }
}
