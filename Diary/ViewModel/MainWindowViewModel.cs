using System;
using System.Collections.Generic;
using System.Text;
using Diary.DataAccess;
using Diary.Commands;
using System.Collections.ObjectModel;
using Diary.Properties;
using Diary.Model;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Diary.ViewModel
{
    public class MainWindowViewModel: BaseViewModel
    {
        #region Fields

        readonly NoteRepository noteRepository;
        readonly ProgressRepository progressRepository;
        readonly RelevanceRepository relevanceRepository;
        readonly TypeJobRepository typeJobRepository;

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

        private WorkspaceViewModel workspaceViewModel;
        public WorkspaceViewModel WorkspaceViewModel
        {
            get
            {
                if(workspaceViewModel == null)
                {
                    workspaceViewModel = new WorkspaceViewModel();
                    workspaceViewModel.CurrentContentVM = new NotesOfDayViewModel(noteRepository, 
                                                                                relevanceRepository,
                                                                                progressRepository, 
                                                                                typeJobRepository, 
                                                                                DateTime.Now);
                }

                return workspaceViewModel;
            }
            set
            {
                workspaceViewModel = value;
                OnPropertyChanged("WorksapceViewModel");
            }
        }


        #endregion // Properties


        #region Public functions
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        
        #endregion // Public functions

        #region Private functions

        private void UpdateWorkspaceViewModel(BaseViewModel workspace)
        {
            workspaceViewModel.CurrentContentVM = workspace;
        }

        private void ChangeWorkspaceOnNoteView(Note note)
        {
            BaseViewModel workspace = new NoteViewModel(note, noteRepository, typeJobRepository, progressRepository, relevanceRepository);
            UpdateWorkspaceViewModel(workspace);
        }
        private void ChangeWorkspaceOnListNotesView()
        {
            NotesOfDayViewModel workspace = new NotesOfDayViewModel(noteRepository, relevanceRepository,progressRepository,typeJobRepository, SelectedDate);
            workspace.CreateNewNoteCommand = this.CreateNewNoteCommand;
            UpdateWorkspaceViewModel(workspace);
        }

        #endregion // Private functions

        #region Commands

        public DateTime SelectedDate { get; set; }
        private RelayCommand selectDateCommand;
        public RelayCommand SelectDateCommand
        {
            get
            {
                return selectDateCommand ??
                    (selectDateCommand = new RelayCommand(obj =>
                    {
                        this.ChangeWorkspaceOnListNotesView();
                    }));
            }
        }


        private RelayCommand changeNoteCommand;
        public RelayCommand ChangeNoteCommand
        {
            get
            {
                return changeNoteCommand ??
                    (changeNoteCommand = new RelayCommand(obj =>
                    {
                        this.ChangeWorkspaceOnNoteView((Note)obj);
                    }));
            }
        }
        private RelayCommand createNewNoteCommand;
        public RelayCommand CreateNewNoteCommand
        {
            get
            {
                return createNewNoteCommand ??
                    (createNewNoteCommand = new RelayCommand(obj =>
                    {
                        this.ChangeWorkspaceOnNoteView(new Note());
                    }));
            }
        }
        #endregion // Commands
    }
}
