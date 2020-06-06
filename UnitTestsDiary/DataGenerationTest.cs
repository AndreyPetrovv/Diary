using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Diary.DataGeneration;
using Diary.DataAccess;
using System.Threading.Tasks;

namespace UnitTestsDiary
{
    [TestClass]
    public  class DataGeneration
    {
        string resConnect = @"Data Source=DESKTOP-EJ5LSTR\SQLEXPRESS; Initial Catalog=diary; Integrated Security=true;";

        [TestMethod]
        public async Task GenerateNotesAsyncTest()
        {
            NoteRepository noteRepository = new NoteRepository(resConnect);
            DataGenerator dataGenerator = new DataGenerator();


            dataGenerator.GenerateNotesAsync(noteRepository, 0);
        }
    }
}
