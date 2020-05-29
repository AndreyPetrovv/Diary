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

        /// <summary>
        /// 
        /// </summary>
        public int IdProgress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StatusProgress { get; set; }

        #endregion // State Properties
    }
}
