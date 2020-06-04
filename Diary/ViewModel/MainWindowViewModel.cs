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

        WorkspaceViewModel workspaceViewModel;

        #endregion // Fields

        #region Constructor

        public MainWindowViewModel(string connectionString)
        {
            try
            {
                noteRepository = new NoteRepository(connectionString);
            }
            catch
            {

            }
        }

        #endregion // Constructor

        #region Properties

        public NoteRepository NoteRepository { get => noteRepository; }

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

        #region Public methods

        public void ChangeNote()
        {
            BaseViewModel workspace = (workspaceViewModel.CurrentContentVM as NotesOfDayViewModel).SelectedNoteViewModel;
            (workspace as NoteViewModel).UpdateWorkstapeNotify += SetListNotesViewOnWorkspace;
            UpdateWorkspaceViewModel(workspace);
        }

        public void SetListNotesViewOnWorkspace()
        {
            NotesOfDayViewModel workspace = new NotesOfDayViewModel(this);
            workspace.Notify += UpdateWorkspaceViewModel;
            UpdateWorkspaceViewModel(workspace);
        }

        #endregion // Public methods

        #region Private methods

        void UpdateWorkspaceViewModel(BaseViewModel workspace)
        {
            workspaceViewModel.CurrentContentVM = workspace;
        }

        #endregion // Private functions

        #region Commands

        public RelayCommand SelectDateCommand
        {
            get
            {
                return new RelayCommand(
                         param => this.SetListNotesViewOnWorkspace()
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
