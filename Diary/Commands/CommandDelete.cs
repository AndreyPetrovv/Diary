using System;
using System.Collections.Generic;
using System.Text;
using Diary.DataAccess;

namespace Diary.Commands
{
    class CommandDelete : ICommand
    {
        public string BaseName => Properties.Resources.CommandDeleteNote;

        public CommandDelete(NoteRepository noteRepository, int index)
        {

        }

        public void DoAction()
        {
            throw new NotImplementedException();
        }
    }
}
