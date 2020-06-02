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
        NoteRepository _noteRepository;
        TypeJobRepository _typeJobs;
        ProgressRepository _progresses;
        RelevanceRepository _relevances;

        string timeStartHours;
        string timeStartMinutes;
        string timeFinishHours;
        string timeFinishMinutes;

        string[] _noteTypeJobs;
        string[] _noteRelevances;
        string[] _noteProgresses;

        
        RelayCommand _saveNoteCommand;
        RelayCommand _deleteNoteCommand;

        #endregion // Fields

        #region Constructor
        public NoteViewModel(
            Note note,
            NoteRepository noteRepository,
            TypeJobRepository typeJobs,
            ProgressRepository progresses,
            RelevanceRepository relevances)
        {
            this._noteRepository = noteRepository;
            this._note = note;
            this._typeJobs = typeJobs;
            this._progresses = progresses;
            this._relevances = relevances;

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
                return GetTypeJob(this._note, this._typeJobs);
            }
            set
            {
                foreach (var item in this._typeJobs.GetTypeJobs())
                {
                    if (item.NameTypeJob == value)
                    {
                        this._note.IdTypeJob = item.IdTypeJob;
                    }
                }
            }
        }
        public string Relevance {
            get
            {
                return GetRelevance(this._note, this._relevances);
            }
            set
            {
                foreach (var item in _relevances.GetRelevances())
                {
                    if (item.LevelRelevance == value)
                    {
                        this._note.IdRelevance = item.IdRelevance;
                    }
                }
            }
        }
        public string Progress {
            get
            {
                return GetProgress(this._note, this._progresses);
            }
            set
            {
                foreach (var item in _progresses.GetProgresses())
                {
                    if (item.StatusProgress == value)
                    {
                        this._note.IdProgress = item.IdProgress;
                    }
                }
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

        public string GetString => $"{_note.NoteDate.Date}, Занятие: {TypeJob};\n" +
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
                    _noteTypeJobs = new string[_typeJobs.GetTypeJobs().Count];

                    int i = 0;

                    foreach (var item in _typeJobs.GetTypeJobs())
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
                    _noteRelevances = new string[_relevances.GetRelevances().Count];

                    int i = 0;

                    foreach (var item in _relevances.GetRelevances())
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
                    _noteProgresses = new string[_progresses.GetProgresses().Count];

                    int i = 0;

                    foreach (var item in _progresses.GetProgresses())
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

        void Save()
        {
            if (_note.IdNote == -1)
            {
                _noteRepository.AddNote(_note);
            }
            else
            {
                _noteRepository.UpdateNote(_note);
            }
        }

        void Delete()
        {
            _noteRepository.RemoveNote(_note);

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

        string GetTypeJob(Note note , TypeJobRepository typeJobs)
        {
            foreach (var item in typeJobs.GetTypeJobs())
            {
                if( item.IdTypeJob == note.IdTypeJob)
                {
                    return item.NameTypeJob;
                }
            }

            return "None";
        }
        string GetRelevance(Note note, RelevanceRepository relevances)
        {
            foreach (var item in relevances.GetRelevances())
            {
                if (item.IdRelevance == note.IdRelevance)
                {
                    return item.LevelRelevance;
                }
            }

            return "None";
        }
        string GetProgress(Note note, ProgressRepository progresses)
        {
            foreach (var item in progresses.GetProgresses())
            {
                if (item.IdProgress == note.IdProgress)
                {
                    return item.StatusProgress;
                }
            }

            return "None";
        }

        bool CanSave
        {
            get { return _note.IsValid; }
        }

        #endregion // Private methods

        #region Commands
        public RelayCommand ChangeNoteCommand { get; set; }
        public RelayCommand UpdateWorckspaceCommand { get; set; }

        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveNoteCommand == null)
                {
                    _saveNoteCommand = new RelayCommand(
                        param => {
                            this.Save();
                            this.UpdateWorckspaceCommand.Execute(null);
                            }
                        ,
                        param => this.CanSave
                        );
                }
                return _saveNoteCommand;
            }
        }

        public RelayCommand DeleteNoteCommand 
        {
            get 
            {
                if (_deleteNoteCommand == null)
                {
                    _deleteNoteCommand = new RelayCommand(
                        param => this.Delete()
                        );
                }
                return _deleteNoteCommand;
            }
        }

        #endregion // Commands

    }
}
