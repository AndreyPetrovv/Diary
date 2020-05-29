using System;
using System.Collections.Generic;
using System.Text;

namespace Diary.Commands
{
    public class CommandCreate : ICommand
    {
        public string BaseName => Properties.Resources.CommandCreateNewNote;

        public CommandCreate()
        {

        }

        public void DoAction()
        {
            throw new NotImplementedException();
        }
    }
}
