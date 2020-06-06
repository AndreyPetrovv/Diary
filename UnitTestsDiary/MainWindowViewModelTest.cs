using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diary.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestsDiary
{
    [TestClass]
    public class MainWindowViewModelTest
    {
        string resConnect = @"1Data Source=DESKTOP-EJ5LSTR\SQLEXPRESS; Initial Catalog=diary; Integrated Security=true;";

        [TestMethod]
        public void GenerateNotesAsyncTest()
        {
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(resConnect);

            Assert.IsFalse(mainWindowViewModel.IsConnectToDB);

        }
    }
}
