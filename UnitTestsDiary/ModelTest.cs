using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Diary.Model;

namespace UnitTestsDiary
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void ValidateProgressTest()
        {
            Note note = new Note();
            Assert.IsFalse(note.ValidateProgress());

           // note.Progress = new Progress(1, "test");
           //Assert.IsTrue(note.ValidateProgress());
        }

        [TestMethod]
        public void ValidateRelevanceTest()
        {
            Note note = new Note();
            Assert.IsFalse(note.ValidateRelevance());

            note.Relevance =  new Relevance(1, "test");
            Assert.IsTrue(note.ValidateRelevance());
        }

        [TestMethod]
        public void ValidateTimeTest()
        {
            Note note = new Note();
            Assert.IsFalse(note.ValidateTime());

            note.TimeFinish = new TimeSpan(1, 0, 0);
            Assert.IsTrue(note.ValidateTime());
        }

        [TestMethod]
        public void ValidateTypeJobTest()
        {
            Note note = new Note();
            Assert.IsFalse(note.ValidateTypeJob());

            note.TypeJob = new TypeJob(1, "test");
            Assert.IsTrue(note.ValidateTypeJob());
        }

        [TestMethod]
        public void IsValidTest()
        {
            Note note = new Note();
            Assert.IsFalse(note.IsValid);

            note.TypeJob = new TypeJob(1, "test");
            note.Relevance = new Relevance(1, "test");
            note.Progress = new Progress(1, "test");
            note.TimeFinish = new TimeSpan(1, 0, 0);

            Assert.IsTrue(note.IsValid);
        }

    }
}
