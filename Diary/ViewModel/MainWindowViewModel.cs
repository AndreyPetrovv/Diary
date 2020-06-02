using System;
using System.Collections.Generic;
using System.Text;
using Diary.DataAccess;
using System.Collections.ObjectModel;
using Diary.Properties;
using Diary.Model;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Diary.ViewModel
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        #region Fields
        DateTime selectedDate;

        readonly NoteRepository noteRepository;
        readonly ProgressRepository progressRepository;
        readonly RelevanceRepository relevanceRepository;
        readonly TypeJobRepository typeJobRepository;

        WorkspaceViewModel workspaceViewModel;

        RelayCommand changeNoteCommand;
        RelayCommand selectDateCommand;
        RelayCommand createNewNoteCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion // Fields

        #region Constructor

        public MainWindowViewModel(string connectionString)
        {
            noteRepository = new NoteRepository(connectionString);
            progressRepository = new ProgressRepository(connectionString);
            relevanceRepository = new RelevanceRepository(connectionString);
            typeJobRepository = new TypeJobRepository(connectionString);
        }

        #endregion // Constructor

        #region Properties
        public NoteRepository NoteRepository { get => noteRepository; }
        public ProgressRepository ProgressRepository { get => progressRepository; }
        public TypeJobRepository TypeJobRepository { get => typeJobRepository; }
        public RelevanceRepository RelevanceRepository { get => relevanceRepository; }

        public WorkspaceViewModel WorkspaceViewModel
        {
            get
            {
                if(workspaceViewModel == null)
                {
                    workspaceViewModel = new WorkspaceViewModel();
                    workspaceViewModel.CurrentContentVM = new NotesOfDayViewModel(this);
                }

                return workspaceViewModel;
            }
            set
            {
                workspaceViewModel = value;
                OnPropertyChanged("WorksapceViewModel");
            }
        }

        bool CheckTimeNote
        {
            get
            {
                if (SelectedDate.Date == DateTime.Now.Date)
                {
                    return true;
                }
                return false;
            }
        }

        public DateTime SelectedDate {
            get
            {
                if(selectedDate == new DateTime())
                {
                    selectedDate = DateTime.Now;
                }

                return selectedDate;
            }
            set
            {
                selectedDate = value;
            }
        }

        #endregion // Properties

        #region Public functions

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null) 
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        
        #endregion // Public functions

        #region Private functions

        void UpdateWorkspaceViewModel(BaseViewModel workspace)
        {
            workspaceViewModel.CurrentContentVM = workspace;
        }

        void CreateNewNote()
        {
            Note note = new Note();
            note.NoteDate = DateTime.Now;
            NoteViewModel workspace = new NoteViewModel(note, noteRepository, typeJobRepository, progressRepository, relevanceRepository);
            workspace.UpdateWorckspaceCommand = UpdateWorckspaceCommand;
            UpdateWorkspaceViewModel(workspace);
        }

        void ChangeNote()
        {
            BaseViewModel workspace = (workspaceViewModel.CurrentContentVM as NotesOfDayViewModel).SelectedNoteViewModel;
            UpdateWorkspaceViewModel(workspace);
        }

        void SetListNotesViewOnWorkspace()
        {
            NotesOfDayViewModel workspace = new NotesOfDayViewModel(this);
            UpdateWorkspaceViewModel(workspace);
        }

        #endregion // Private functions

        #region Commands

        public RelayCommand SelectDateCommand
        {
            get
            {
                return selectDateCommand ??
                    (selectDateCommand = new RelayCommand(obj =>
                    {
                        this.SetListNotesViewOnWorkspace();
                    }));
            }
        }
        public RelayCommand CreateNewNoteCommand
        {
            get
            {

                return createNewNoteCommand ??
                    (createNewNoteCommand = new RelayCommand(
                        param => {
                            this.CreateNewNote();
                        },
                        param => this.CheckTimeNote));
            }
        }
        public RelayCommand ChangeNoteCommand
        {
            get
            {
                return changeNoteCommand ??
                    (changeNoteCommand = new RelayCommand(
                        param => this.ChangeNote(),
                        param => this.CheckTimeNote
                    ));
            }
        }
        RelayCommand updateWorckspaceCommand;
        public RelayCommand UpdateWorckspaceCommand
        {
            get
            {
                return updateWorckspaceCommand ??
                    (updateWorckspaceCommand = new RelayCommand(
                        param => this.SetListNotesViewOnWorkspace()
                    ));
            }
        }
        #endregion // Commands
    }
}
