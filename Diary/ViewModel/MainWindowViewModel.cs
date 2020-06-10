using System;
using System.Windows;
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
                IsConnectToDB = true;
            }
            catch
            {
                MessageBox.Show("Can't connect to database");
                IsConnectToDB = false;
            }
        }

        #endregion // Constructor

        #region Properties
        public bool IsConnectToDB { get; private set; }

        public NoteRepository NoteRepository { get => noteRepository; }

        public WorkspaceViewModel WorkspaceViewModel
        {
            get
            {
                if(workspaceViewModel == null && IsConnectToDB)
                {
                    workspaceViewModel = new WorkspaceViewModel();
                    NotesOfDayViewModel notesOfDayView = new NotesOfDayViewModel(this);
                    notesOfDayView.UpdateWorkstapeNotify += UpdateWorkspaceViewModel;
                    workspaceViewModel.CurrentContentVM = notesOfDayView;

                }

                return workspaceViewModel;
            }
            set
            {
                workspaceViewModel = value;
                OnPropertyChanged();
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

        public void ChangeNote(NoteViewModel workspace)
        {
            if (workspace != null)
            {
                (workspace as NoteViewModel).UpdateWorkstapeNotify += SetListNotesViewOnWorkspace;
                UpdateWorkspaceViewModel(workspace);
            }
            else
            {
                MessageBox.Show("Select item !");
            }
        }

        public void SetListNotesViewOnWorkspace()
        {
            NotesOfDayViewModel workspace = new NotesOfDayViewModel(this);
            workspace.UpdateWorkstapeNotify += UpdateWorkspaceViewModel;
            UpdateWorkspaceViewModel(workspace);
        }

        #endregion // Public methods

        #region Private methods

        void UpdateWorkspaceViewModel(BaseViewModel workspace)
        {
            workspaceViewModel.CurrentContentVM = workspace;
        }

        void SetStaticInfoViewOnWorkspace(string key)
        {
            StatisticInfoViewModel workspace = null;
            DateTime dateBegin;
            switch (key)
            {
                case "AllTime":
                    workspace = new StatisticInfoViewModel(noteRepository.GetAllNotes());
                    break;
                case "LastMonth":
                    dateBegin = DateTime.Now.AddDays(-30);
                    workspace = new StatisticInfoViewModel(noteRepository.GetNotesOfDays(dateBegin, DateTime.Now));
                    break;
                case "LastWeek":
                    dateBegin = DateTime.Now.AddDays(-7);
                    workspace = new StatisticInfoViewModel(noteRepository.GetNotesOfDays(dateBegin, DateTime.Now));
                    break;
            }

            workspace.QuitNotify += SetListNotesViewOnWorkspace;
            UpdateWorkspaceViewModel(workspace);
        }

        #endregion // Private functions

        #region Commands

        public RelayCommand SelectDateCommand
        {
            get
            {
                return new RelayCommand(
                         param => this.SetListNotesViewOnWorkspace(),
                         param => IsConnectToDB
                    );
            }
        }

        public RelayCommand GenerateDataCommand
        {
            get
            {
                return new RelayCommand(

                        param => {
                            new DataGenerator().GenerateNotesAsync(noteRepository, int.Parse((string)param));
                            this.SetListNotesViewOnWorkspace();
                        },
                        param => IsConnectToDB
                    );
            }
        }

        public RelayCommand CountStaticInfCommand
        {
            get
            {
                return new RelayCommand(
                        param => this.SetStaticInfoViewOnWorkspace((string)param),
                        param => IsConnectToDB
                    ) ;
            }
        }

        #endregion // Commands
    }
}
