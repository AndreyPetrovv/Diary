using System;
using System.Collections.Generic;
using System.Text;

namespace Diary.Model
{
    public class Relevance
    {
        #region Constructor

        public Relevance(int idRelevance, string levelRelevance)
        {
            IdRelevance = idRelevance;
            LevelRelevance = levelRelevance;
        }

        #endregion // Constructor

        #region State Properties

        public int IdRelevance { get; set; }

        public string LevelRelevance { get; set; }

        #endregion // State Properties
    }
}
