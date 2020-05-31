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

        #endregion // Fields

        #region Constructor
        
        public NotesOfDayViewModel(
            NoteRepository noteRepository,
            RelevanceRepository relevanceRepository,
            ProgressRepository progressRepository,
            TypeJobRepository typeJobRepository,
            DateTime SelectedDate)
        {

            NoteViewModels = new ObservableCollection<NoteViewModel>();

            UpdateNoteViewModels(
                noteRepository,
                relevanceRepository,
                progressRepository,
                typeJobRepository,
                SelectedDate);
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

        #region Public methods

        #endregion

        #region Private methods

        void UpdateNoteViewModels(
            NoteRepository noteRepository,
            RelevanceRepository relevanceRepository,
            ProgressRepository progressRepository,
            TypeJobRepository typeJobRepository,
            DateTime SelectedDate)
        {
            NoteViewModels.Clear();

            foreach (var note in noteRepository.GetNotes())
            {
                if (note.NoteData.Date == SelectedDate.Date)
                {
                    NoteViewModel _noteVM = new NoteViewModel(
                            note: note,
                            noteRepository: noteRepository,
                            progresses: progressRepository,
                            relevances: relevanceRepository,
                            typeJobs: typeJobRepository
                            );
                    _noteVM.ChangeNoteCommand = null;

                    NoteViewModels.Add(_noteVM);
                        
                }
            }
        }


        #endregion

        #region Commands

        private RelayCommand createNewNoteCommand;
        public RelayCommand CreateNewNoteCommand
        {
            get
            {
                return createNewNoteCommand;
            }
            set
            {
                createNewNoteCommand = value;
            }
        }

        #endregion
    }
}
