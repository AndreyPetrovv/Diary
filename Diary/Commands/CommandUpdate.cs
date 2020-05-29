using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Diary.DataAccess;
using Diary.Model;

namespace Diary.Commands
{
    class CommandUpdate: ICommand
    {
        public string BaseName => Properties.Resources.CommandUpdateNote;

        public CommandUpdate(NoteRepository noteRepository, Note newNote)
        {
            
        }

        public void DoAction()
        {
            int a = 10;
        }

    }
}
