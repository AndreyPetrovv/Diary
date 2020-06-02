using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Diary.Model;
using Diary.DataAccess;

namespace Diary.ViewModel
{
    public class NotesOfDayViewModel: BaseViewModel
    {
        #region Fields

        NoteViewModel selectedNoteViewModel;

        MainWindowViewModel mainWindowViewModel;

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
            }
        }
        public ObservableCollection<NoteViewModel> NoteViewModels { get; set; }

        #endregion // Properties

        #region Private methods

        void UpdateNoteViewModels()
        {
            NoteViewModels.Clear();

            foreach (var note in mainWindowViewModel.NoteRepository.GetNotesOfDay(mainWindowViewModel.SelectedDate))
            {
                if (note.NoteDate.Date == mainWindowViewModel.SelectedDate.Date)
                {
                    NoteViewModel _noteVM = new NoteViewModel(
                            note: note,
                            mainWindowViewModel: mainWindowViewModel
                            );

                    NoteViewModels.Add(_noteVM);
                        
                }
            }
        }

        #endregion

        #region Commands

        public RelayCommand CreateNewNoteCommand
        {
            get
            {
                return mainWindowViewModel.CreateNewNoteCommand;
            }
        }

        #endregion
    }
}
