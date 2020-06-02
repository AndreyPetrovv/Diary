using System;
using Diary.DataAccess;
using Diary.DataGeneration;
using Diary.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Diary.ViewModel
{
    public class MainWindowViewModel: BaseViewModel
    {
        #region Fields
        DateTime selectedDate;

        readonly NoteRepository noteRepository;
        readonly ProgressRepository progressRepository;
        readonly RelevanceRepository relevanceRepository;
        readonly TypeJobRepository typeJobRepository;

        WorkspaceViewModel workspaceViewModel;

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

        #region Private functions

        void UpdateWorkspaceViewModel(BaseViewModel workspace)
        {
            workspaceViewModel.CurrentContentVM = workspace;
        }

        void CreateNewNote()
        {
            Note note = new Note();
            note.NoteDate = DateTime.Now;
            NoteViewModel workspace = new NoteViewModel(note, this);
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
                return new RelayCommand(
                    obj =>{
                            this.SetListNotesViewOnWorkspace();
                        });
            }
        }
        public RelayCommand CreateNewNoteCommand
        {
            get
            {
                return new RelayCommand(
                        param => {
                            this.CreateNewNote();
                        },
                        param => this.CheckTimeNote
                        );
            }
        }
        public RelayCommand ChangeNoteCommand
        {
            get
            {
                return new RelayCommand(
                        param => this.ChangeNote(),
                        param => this.CheckTimeNote
                        );
            }
        }
        public RelayCommand UpdateWorckspaceCommand
        {
            get
            {
                return new RelayCommand(
                        param => this.SetListNotesViewOnWorkspace()
                    );
            }
        }

        public RelayCommand GenerateDataCommand
        {
            get
            {
                return new RelayCommand(
                        param => new DataGenerator().GenerateNotes(noteRepository)
                    );
            }
        }
        #endregion // Commands
    }
}
