using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Diary.Model;
using Diary.DataAccess;
using System.ComponentModel;

namespace Diary.ViewModel
{
    public class NoteViewModel: BaseViewModel
    {

        #region Fields

        Note _note;
        MainWindowViewModel mainWindowViewModel;

        string typeJob;
        string relevance;
        string progress;

        string timeStartHours;
        string timeStartMinutes;
        string timeFinishHours;
        string timeFinishMinutes;

        string[] _noteTypeJobs;
        string[] _noteRelevances;
        string[] _noteProgresses;

        #endregion // Fields

        #region Constructor

        public NoteViewModel(
            Note note,
            MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            this._note = note;

            typeJob = IdTypeJobToName();
            relevance = IdRelevanceToName();
            progress = IdProgressToName();

            TimeStartHours = note.TimeStart.Hours.ToString();
            TimeStartMinutes = note.TimeStart.Minutes.ToString();

            TimeFinishHours = note.TimeFinish.Hours.ToString();
            TimeFinishMinutes = note.TimeFinish.Minutes.ToString();

        }

        #endregion // Constructor

        #region State Properties
        
        public string TypeJob {
            get
            {
                return typeJob;
            }
            set
            {
                foreach (var item in mainWindowViewModel.TypeJobRepository.GetTypeJobs())
                {
                    if (item.NameTypeJob == value)
                    {
                        _note.IdTypeJob = item.IdTypeJob;
                    }
                }
                typeJob = value;
            }
        }
        public string Relevance {
            get
            {
                return relevance;
            }
            set
            {
                foreach (var item in mainWindowViewModel.RelevanceRepository.GetRelevances())
                {
                    if (item.LevelRelevance == value)
                    {
                        _note.IdRelevance = item.IdRelevance;
                    }
                }
                relevance = value;
            }
        }
        public string Progress {
            get
            {
                return progress;
            }
            set
            {
                foreach (var item in mainWindowViewModel.ProgressRepository.GetProgresses())
                {
                    if (item.StatusProgress == value)
                    {
                        this._note.IdProgress = item.IdProgress;
                    }
                }
                progress = value;
            } 
        }
        public string TimeStartHours {
            get
            {
                return timeStartHours;
            }
            set
            {
                if (CheckValidTimeH(value))
                {
                    if (value.Length != 0) 
                    {
                        _note.TimeStart  = new TimeSpan(int.Parse(value), _note.TimeStart.Minutes, 0);
                    }
                    timeStartHours = value;
                }
            }
        }
        public string TimeStartMinutes
        {
            get
            {
                return timeStartMinutes;
            }
            set
            {
                if (CheckValidTimeM(value))
                {
                    if (value.Length != 0)
                    {
                        _note.TimeStart = new TimeSpan(_note.TimeStart.Hours, int.Parse(value), 0);
                    }
                    timeStartMinutes = value;
                }
            }
        }
        public string TimeFinishHours
        {
            get
            {
                return timeFinishHours;
            }
            set
            {
                if (CheckValidTimeH(value))
                {
                    if (value.Length != 0)
                    {
                        _note.TimeFinish = new TimeSpan(int.Parse(value), _note.TimeFinish.Minutes, 0);
                    }
                    timeFinishHours = value;
                }
            }
        }
        public string TimeFinishMinutes
        {
            get
            {
                return timeFinishMinutes;
            }
            set
            {
                if (CheckValidTimeM(value))
                {
                    if (value.Length != 0)
                    {
                        _note.TimeFinish = new TimeSpan(_note.TimeFinish.Hours, int.Parse(value), 0);
                    }
                    timeFinishMinutes = value;
                }
            }
        }

        public string GetString => $"{_note.NoteDate.ToShortDateString()}, Занятие: {TypeJob};\n" +
            $"Важность: {Relevance} Прогресс: {Progress}\n" +
            $"Start: {_note.TimeStart.Hours}:{_note.TimeStart.Minutes} " +
            $"Finish: {_note.TimeFinish.Hours}:{_note.TimeFinish.Minutes}";

        #endregion // State Properties

        #region Presentation Properties

        public string[] NoteTypeJobs
        {
            get
            {
                if (_noteTypeJobs == null)
                {
                    _noteTypeJobs = new string[mainWindowViewModel.TypeJobRepository.GetTypeJobs().Count];

                    int i = 0;

                    foreach (var item in mainWindowViewModel.TypeJobRepository.GetTypeJobs())
                    {
                        _noteTypeJobs[i] = item.NameTypeJob;
                        i++;
                    }
                }
                return _noteTypeJobs;
            }
        }

        public string[] NoteRelevances
        {
            get
            {
                if (_noteRelevances == null)
                {
                    _noteRelevances = new string[mainWindowViewModel.RelevanceRepository.GetRelevances().Count];

                    int i = 0;

                    foreach (var item in mainWindowViewModel.RelevanceRepository.GetRelevances())
                    {
                        _noteRelevances[i] = item.LevelRelevance;
                        i++;
                    }
                }
                return _noteRelevances;
            }
        }

        public string[] NoteProgresses
        {
            get
            {
                if (_noteProgresses == null)
                {
                    _noteProgresses = new string[mainWindowViewModel.ProgressRepository.GetProgresses().Count];

                    int i = 0;

                    foreach (var item in mainWindowViewModel.ProgressRepository.GetProgresses())
                    {
                        _noteProgresses[i] = item.StatusProgress;
                        i++;
                    }
                }
                return _noteProgresses;
            }
        }

        #endregion // Presentation Properties

        #region Private methods

        string IdTypeJobToName()
        {
            foreach (var item in mainWindowViewModel.TypeJobRepository.GetTypeJobs())
            {
                if (item.IdTypeJob == _note.IdTypeJob)
                {
                    return item.NameTypeJob;
                }
            }
            return "none";
        }
        string IdRelevanceToName()
        {
            foreach (var item in mainWindowViewModel.RelevanceRepository.GetRelevances())
            {
                if (item.IdRelevance == _note.IdRelevance)
                {
                    return item.LevelRelevance;
                }
            }
            return "none";
        }
        string IdProgressToName()
        {
            foreach (var item in mainWindowViewModel.ProgressRepository.GetProgresses())
            {
                if (item.IdProgress == _note.IdProgress)
                {
                    return item.StatusProgress;
                }
            }
            return "none";
        }

        void Save()
        {
            if (_note.IdNote == -1)
            {
                mainWindowViewModel.NoteRepository.AddNote(_note);
            }
            else
            {
                mainWindowViewModel.NoteRepository.UpdateNote(_note);
            }
        }

        void Delete()
        {
            mainWindowViewModel.NoteRepository.RemoveNote(_note);

            base.OnPropertyChanged("CurrentContentVM");
        }

        bool CheckValidTimeH(string time)
        {
            int testTime;
            if (time.Length >= 0 && time.Length <= 2 && int.TryParse(time, out testTime))
            {
                if (testTime >= 0 && testTime <= 24)
                {
                    return true;
                }
            }

            return false;
        }
        bool CheckValidTimeM(string time)
        {
            int testTime;
            if (time.Length >= 0 && time.Length <= 2 && int.TryParse(time, out testTime))
            {
                if (testTime >= 0 && testTime <= 59)
                {
                    return true;
                }
            }

            return false;
        }

        bool CanSave
        {
            get { return _note.IsValid; }
        }

        #endregion // Private methods

        #region Commands
        public RelayCommand ChangeNoteCommand 
        {
            get
            {
                return mainWindowViewModel.ChangeNoteCommand;
            }
        }
        public RelayCommand UpdateWorckspaceCommand 
        {
            get
            {
                return mainWindowViewModel.UpdateWorckspaceCommand;
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand(
                        param => {
                            this.Save();
                            this.UpdateWorckspaceCommand.Execute(null);
                            }
                        ,
                        param => this.CanSave
                        );
            }
        }
        public RelayCommand DeleteNoteCommand 
        {
            get
            {
                return new RelayCommand(
                    param =>
                        {
                            this.Delete();
                            this.UpdateWorckspaceCommand.Execute(null);
                        }
                    );
            }
        }

        #endregion // Commands

    }
}
