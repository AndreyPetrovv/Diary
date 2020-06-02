using System;
using System.Collections.Generic;
using System.Text;

namespace Diary.Model
{
    public class Progress
    {
        #region Constructor

        public Progress(int idProgress, string statusProgress)
        {
            IdProgress = idProgress;
            StatusProgress = statusProgress;
        }

        #endregion // Constructor

        #region State Properties

        public int IdProgress { get; set; }

        public string StatusProgress { get; set; }

        #endregion // State Properties
    }
}
