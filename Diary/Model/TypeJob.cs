using System;
using System.Collections.Generic;
using System.Text;

namespace Diary.Model
{
    public class TypeJob
    {
        #region Constructor

        public TypeJob(int idTypeJob, string nameTypeJob)
        {
            IdTypeJob = idTypeJob;
            NameTypeJob = nameTypeJob;
        }

        #endregion // Constructor

        #region State Properties

        public int IdTypeJob { get; set; }
        
        public string NameTypeJob { get; set; }

        #endregion // State Properties

    }
}
