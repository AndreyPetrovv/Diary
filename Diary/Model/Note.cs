using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Diary.Model
{
    public class Note
    {

        #region Fields

        int idNote = -1;
        DateTime noteDate;
        TypeJob typeJob;
        Relevance relevance;
        Progress progress;
        TimeSpan timeStart;
        TimeSpan timeFinish;

        #endregion // Fields

        #region Constructor

        public Note()
        {
            idNote = -1;
        }

        public Note(
            int idNote,
            DateTime noteData,
            TypeJob typeJob,
            Relevance relevance,
            Progress progress,
            TimeSpan timeStart,
            TimeSpan timeFinish
            )
        {
            this.IdNote = idNote;
            this.NoteDate = noteData;
            this.TypeJob = typeJob;
            this.Relevance = relevance;
            this.Progress = progress;
            this.TimeStart = timeStart;
            this.TimeFinish = timeFinish;
        }

        #endregion // Constructor

        #region Properties

        public int IdNote { get => idNote; set => idNote = value; }

        public DateTime NoteDate { get => noteDate; set => noteDate = value; }

        public TypeJob TypeJob { get => typeJob; set => typeJob = value; }

        public Relevance Relevance { get => relevance; set => relevance = value; }

        public Progress Progress { get => progress; set => progress = value; }

        public TimeSpan TimeStart { get => timeStart; set => timeStart = value; }

        public TimeSpan TimeFinish { get => timeFinish; set => timeFinish = value; }

        #endregion // Properties

        #region Public methods
        public bool IsValid
        {
            get
            {
                if (ValidateTime() && ValidateProgress() && ValidateRelevance() && ValidateTypeJob())
                {
                    return true;
                }

                return false;
            }
        }

        public bool ValidateTypeJob()
        {
            if (typeJob == null)
            {
                return false;
            }
            return true;
        }

        public bool ValidateRelevance()
        {
            if (relevance == null)
            {
                return false;
            }
            return true;
        }

        public bool ValidateProgress()
        {
            if (progress == null)
            {
                return false;
            }
            return true;
        }

        public bool ValidateTime()
        {
            if (timeStart >= timeFinish)
            {
                return false;
            }
            return true;
        }

        #endregion // Public methods
    }
}
