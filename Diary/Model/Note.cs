using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Diary.Model
{
    public class Note: IDataErrorInfo
    {

        #region Fields

        int idNote = -1;
        DateTime noteData;
        int idTypeJob;
        int idRelevance;
        int idProgress;
        TimeSpan timeStart;
        TimeSpan timeFinish;

        #endregion // Fields

        #region Constructor

        public Note()
        {
            idNote = -1;
            idTypeJob = -1;
            idRelevance = -1;
            idProgress = -1;
        }

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
            this.idNote = idNote;
            this.noteData = noteData;
            this.idTypeJob = idTypeJob;
            this.idRelevance = idRelevance;
            this.idProgress = idProgress;
            this.timeStart = timeStart;
            this.timeFinish = timeFinish;
        }

        #endregion // Constructor

        #region State Properties

        public int IdNote { get => idNote; set => idNote = value; }

        public DateTime NoteData { get => noteData; set => noteData = value; }

        public int IdTypeJob { get => idTypeJob; set => idTypeJob = value; }

        public int IdRelevance { get => idRelevance; set => idRelevance = value; }

        public int IdProgress { get => idProgress; set => idProgress = value; }

        public TimeSpan TimeStart { get => timeStart; set => timeStart = value; }

        public TimeSpan TimeFinish { get => timeFinish; set => timeFinish = value; }

        #endregion // State Properties

        public bool IsValid
        {
            get
            {
                if (ValidateTime() & ValidateProgress() & ValidateRelevance() & ValidateTypeJob())
                {
                    return true;
                }

                return false;
            }
        }
        string IDataErrorInfo.Error { get { return null; } }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return this.GetValidError(propertyName); }
        }

        string GetValidError(string propertyName)
        {
            string error = null;
            switch (propertyName)
            {
                case "TypeJob":
                    if (!ValidateTypeJob())
                    {
                        error = Properties.Resources.TypeJobError;
                    }
                    break;
                case "Relevance":
                    if (!ValidateRelevance())
                    {
                        error = Properties.Resources.RelevanceError;
                    }
                    break;
                case "Progress":
                    if (!ValidateProgress())
                    {
                        error = Properties.Resources.ProgressError;
                    }
                    break;
            }

            return error;
        }

        public bool ValidateTypeJob()
        {
            if (idTypeJob == -1)
            {
                return false;
            }
            return true;
        }
        public bool ValidateRelevance()
        {
            if (idRelevance == -1)
            {
                return false;
            }
            return true;
        }
        public bool ValidateProgress()
        {
            if (idProgress == -1)
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
    }
}
