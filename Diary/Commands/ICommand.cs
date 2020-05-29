using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;


namespace Diary.Commands
{
    public interface ICommand
    {
        public string BaseName {get;}

        public void DoAction();
    }
}
