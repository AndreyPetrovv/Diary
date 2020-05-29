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
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        #region Fields
        List<ICommand> _commands;

        NoteViewModel selectedNoteViewModel;

        readonly NoteRepository noteRepository;
        readonly ProgressRepository progressRepository;
        readonly RelevanceRepository relevanceRepository;
        readonly TypeJobRepository typeJobRepository;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion // Fields

        #region Constructor

        public MainWindowViewModel(string connectionString)
        {
            noteRepository = new NoteRepository(connectionString);
            progressRepository = new ProgressRepository(connectionString);
            relevanceRepository = new RelevanceRepository(connectionString);
            typeJobRepository = new TypeJobRepository(connectionString);

            NoteViewModels = new ObservableCollection<NoteViewModel>();
        }

        #endregion // Constructor

        #region Properties

        public ObservableCollection<NoteViewModel> NoteViewModels { get; set; }

        public NoteViewModel SelectedNoteViewModel
        {
            get
            {
                return selectedNoteViewModel;
            }
            set
            {
                selectedNoteViewModel = value;
                OnPropertyChanged("SelectedNoteViewModel ");
            }
        }

        public DateTime SelectedDate { get; set; }
        private RelayCommand selectDateCommand;
        public RelayCommand SelectDateCommand
        {
            get
            {
                return selectDateCommand ??
                    (selectDateCommand = new RelayCommand(obj =>
                    {
                        NoteViewModels.Clear();

                        foreach (var note in noteRepository.GetNotes())
                        {
                            if (note.NoteData.Date == SelectedDate.Date)
                            {
                                NoteViewModels.Add(
                                    new NoteViewModel(
                                        note: note,
                                        progresses: progressRepository.GetProgresses(),
                                        relevances: relevanceRepository.GetRelevances(),
                                        typeJobs: typeJobRepository.GetTypeJobs()
                                        ));
                            }
                        }
                    }));
            }
        }


        #endregion // Properties


        #region Public functions

        #endregion // Public functions

        #region Private functions
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion // Private functions

        #region Commands
        public List<ICommand> Commands
        {
            get
            {
                if (_commands == null)
                {
                    List<ICommand> cmds = this.CreateCommands();
                    _commands = new List<ICommand>(cmds);
                }
                return _commands;
            }
        }

        List<ICommand> CreateCommands()
        {
            return new List<ICommand>
            {
                new CommandCreate(),
            };
        }

      
        #endregion // Commands
    }
}
