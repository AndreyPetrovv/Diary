using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Diary.ViewModel
{
    public class WorkspaceViewModel: INotifyPropertyChanged
    {

        object currentContentVM;
        public object CurrentContentVM
        {
            get
            { 
                return currentContentVM;
            }
            set
            {
                currentContentVM = value;

                OnPropertyChanged("CurrentContentVM");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
