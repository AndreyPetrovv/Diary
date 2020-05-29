using System;
using System.Collections.Generic;
using System.Text;
using Diary.Model;

namespace Diary.ViewModel
{
    public class NoteViewModel
    {

        #region Constructor
        public NoteViewModel(
            Note note,
            List<Progress> progresses,
            List<Relevance> relevances,
            List<TypeJob> typeJobs)
        {
            NoteData = note.NoteData.ToShortDateString();
            TypeJob = GetTypeJob(note, typeJobs);
            Relevance = GetRelevance(note, relevances);
            Progress = GetProgress(note, progresses);
            TimeStart = note.TimeStart.ToString();
            TimeFinish = note.TimeFinish.ToString();
        }

        #endregion // Constructor

        #region State Properties

        /// <summary>
        /// 
        /// </summary>
        public string NoteData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeJob { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Relevance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Progress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TimeStart { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TimeFinish { get; set; }

        public string GetString => $"Data: {NoteData},  {TypeJob},  {Relevance} {Progress}\n" +
            $"Start: {TimeStart} Finish: {TimeFinish}";

        #endregion // State Properties

        #region Private methods

        string GetTypeJob(Note note ,List<TypeJob> typeJobs)
        {
            foreach (var item in typeJobs)
            {
                if( item.IdTypeJob == note.IdTypeJob)
                {
                    return item.NameTypeJob;
                }
            }

            return "None";
        }

        string GetRelevance(Note note, List<Relevance> relevances)
        {
            foreach (var item in relevances)
            {
                if (item.IdRelevance == note.IdRelevance)
                {
                    return item.LevelRelevance;
                }
            }

            return "None";
        }

        string GetProgress(Note note, List<Progress> progresses)
        {
            foreach (var item in progresses)
            {
                if (item.IdProgress == note.IdProgress)
                {
                    return item.StatusProgress;
                }
            }

            return "None";
        }

        #endregion

    }
}
