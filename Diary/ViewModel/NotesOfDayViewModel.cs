using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Diary.Model;
using Diary.DataAccess;

namespace Diary.ViewModel
{
    public class NotesOfDayViewModel: WorkspaceViewModel
    {
        #region Fields

        NoteViewModel selectedNoteViewModel;

        MainWindowViewModel mainWindowViewModel;

        public delegate void UpdateWorkstapeHandler(BaseViewModel baseViewModel);
        public event UpdateWorkstapeHandler UpdateWorkstapeNotify;

        #endregion // Fields

        #region Constructor

        public NotesOfDayViewModel(MainWindowViewModel mainWindowViewModel)
        {

            this.mainWindowViewModel = mainWindowViewModel;

            NoteViewModels = new ObservableCollection<NoteViewModel>();

            UpdateNoteViewModels();
        }

        #endregion // Constructor

        #region Properties

        public NoteViewModel SelectedNoteViewModel
        {
            get
            {
                return selectedNoteViewModel;
            }
            set
            {
                selectedNoteViewModel = value;
                OnPropertyChanged("SelectedNoteViewModel");
            }
        }
        public ObservableCollection<NoteViewModel> NoteViewModels { get; set; }

        #endregion // Properties

        #region Private methods
        
        void CreateNewNote()
        {
            Note note = new Note();
            note.NoteDate = DateTime.Now;
            NoteViewModel workspace = new NoteViewModel(note, mainWindowViewModel.NoteRepository, mainWindowViewModel.SelectedDate);
            workspace.UpdateWorkstapeNotify += mainWindowViewModel.SetListNotesViewOnWorkspace;

            UpdateWorkstapeNotify?.Invoke(workspace);
        }

        void UpdateNoteViewModels()
        {
            NoteViewModels.Clear();

            foreach (var note in mainWindowViewModel.NoteRepository.GetNotesOfDay(mainWindowViewModel.SelectedDate))
            {
                NoteViewModel _noteVM = new NoteViewModel(
                        note: note,
                        noteRepository: mainWindowViewModel.NoteRepository,
                        selectedDate: mainWindowViewModel.SelectedDate );
                _noteVM.UpdateWorkstapeNotify += mainWindowViewModel.SetListNotesViewOnWorkspace;
                _noteVM.ChangeNoteNotify += mainWindowViewModel.ChangeNote;
                NoteViewModels.Add(_noteVM);
            }
        }

        #endregion

        #region Commands

        public RelayCommand CreateNewNoteCommand
        {
            get
            {
                return new RelayCommand(
                        param => this.CreateNewNote(),
                        param => CheckTimeNote(mainWindowViewModel.SelectedDate)
                        );
            }
        }

        #endregion
    }
}
