using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Diary.Model;
using Diary.DataAccess;
using System.ComponentModel;

namespace Diary.ViewModel
{
    public class NoteViewModel: WorkspaceViewModel
    {

        #region Fields

        Note _note;
        DateTime selectedDate;

        public delegate void AccountHandler();
        public event AccountHandler UpdateWorkstapeNotify;
        public event AccountHandler ChangeNoteNotify;

        NoteRepository noteRepository;
        TypeJobRepository typeJobRepository;
        ProgressRepository progressRepository;
        RelevanceRepository relevanceRepository;

        string timeStartHours;
        string timeStartMinutes;
        string timeFinishHours;
        string timeFinishMinutes;

        TypeJob[] _noteTypeJobs;
        Relevance[] _noteRelevances;
        Progress[] _noteProgresses;

        #endregion // Fields

        #region Constructor

        public NoteViewModel(Note note, NoteRepository noteRepository, DateTime selectedDate)
        {
            this._note = note;
            this.noteRepository = noteRepository;
            this.selectedDate = selectedDate;

            typeJobRepository = new TypeJobRepository(Properties.Resources.ConnectCommand);
            progressRepository = new ProgressRepository(Properties.Resources.ConnectCommand);
            relevanceRepository = new RelevanceRepository(Properties.Resources.ConnectCommand);

            TimeStartHours = note.TimeStart.Hours.ToString();
            TimeStartMinutes = note.TimeStart.Minutes.ToString();

            TimeFinishHours = note.TimeFinish.Hours.ToString();
            TimeFinishMinutes = note.TimeFinish.Minutes.ToString();

        }

        #endregion // Constructor

        #region State Properties
        
        public TypeJob NoteTypeJob {
            get
            {
                return _note.TypeJob;
            }
            set
            {

                _note.TypeJob = value;

                OnPropertyChanged("NoteTypeJob");
            }
        }
        public Relevance NoteRelevance
        {
            get
            {
                return _note.Relevance;
            }
            set
            {

                _note.Relevance = value;

                OnPropertyChanged("NoteRelevance");
            }
        }
        public Progress NoteProgress
        {
            get
            {
                return _note.Progress;
            }
            set
            {

                _note.Progress = value;

                OnPropertyChanged("NoteProgress");
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
                OnPropertyChanged("TimeStartHours");
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
                OnPropertyChanged("TimeStartMinutes");
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
                OnPropertyChanged("TimeFinishHours");
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
                OnPropertyChanged("TimeFinishMinutes");
            }
        }

        public string GetString => $"{_note.NoteDate.ToShortDateString()}, Занятие: {NoteTypeJob.NameTypeJob};\n" +
            $"Важность: {NoteRelevance.LevelRelevance} Прогресс: {NoteProgress.StatusProgress}\n" +
            $"Start: {_note.TimeStart.Hours}:{_note.TimeStart.Minutes} " +
            $"Finish: {_note.TimeFinish.Hours}:{_note.TimeFinish.Minutes}";

        #endregion // State Properties

        #region Presentation Properties

        public TypeJob[] NoteTypeJobs
        {
            get
            {
                if (_noteTypeJobs == null)
                {
                    _noteTypeJobs = new TypeJob[typeJobRepository.GetAllTypeJobs().Count];

                    int i = 0;

                    foreach (var item in typeJobRepository.GetAllTypeJobs())
                    {
                        _noteTypeJobs[i] = item;
                        i++;
                    }
                }
                return _noteTypeJobs;
            }
        }

        public Relevance[] NoteRelevances
        {
            get
            {
                if (_noteRelevances == null)
                {
                    _noteRelevances = new Relevance[relevanceRepository.GetAllRelevances().Count];

                    int i = 0;

                    foreach (var item in relevanceRepository.GetAllRelevances())
                    {
                        _noteRelevances[i] = item;
                        i++;
                    }
                }
                return _noteRelevances;
            }
        }

        public Progress[] NoteProgresses
        {
            get
            {
                if (_noteProgresses == null)
                {
                    _noteProgresses = new Progress[progressRepository.GetAllProgresses().Count];

                    int i = 0;

                    foreach (var item in progressRepository.GetAllProgresses())
                    {
                        _noteProgresses[i] = item;
                        i++;
                    }
                }
                return _noteProgresses;
            }
        }

        #endregion // Presentation Properties

        #region Private methods
        
        void ChangeNote()
        {
            ChangeNoteNotify?.Invoke();
        }

        void Save()
        {
            if (_note.IdNote == -1)
            {
                noteRepository.AddNote(_note);
            }
            else
            {
                noteRepository.UpdateNote(_note);
            }

            UpdateWorkstapeNotify?.Invoke();
        }

        void Delete()
        {
            noteRepository.RemoveNote(_note);

            UpdateWorkstapeNotify?.Invoke();
        }

        bool CheckValidTimeH(string time)
        {
            int testTime;
            if (time.Length >= 0 && time.Length <= 2 && int.TryParse(time, out testTime))
            {
                if (testTime >= 0 && testTime < 24)
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
                return new RelayCommand(
                        param => this.ChangeNote(),
                        param => CheckTimeNote(selectedDate)
                        );
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand(
                        param =>  this.Save(),
                        param => this.CanSave
                        );
            }
        }
        public RelayCommand DeleteNoteCommand 
        {
            get
            {
                return new RelayCommand(
                    param => this.Delete(),
                    param => CheckTimeNote(selectedDate)
                    );
            }
        }

        #endregion // Commands

    }
}
