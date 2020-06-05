using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Diary.DataAccess;
using Diary.Model;

namespace Diary.ViewModel
{
    public class StaticInfoViewModel: WorkspaceViewModel
    {

        #region Fields
        public delegate void AccountHandler();
        public event AccountHandler QuitNotify;

        Dictionary<string, int> _valueCounts;

        double _meanTime;
        double _maxTime;
        double _minTime;

        List<Note> notes;

        #endregion // Fields

        #region Constructor

        public StaticInfoViewModel(List<Note> notes)
        {
            this.notes = notes;

            this._meanTime = -1;
            this._maxTime = -1;
            this._minTime = -1;
        }

        #endregion // Constructor

        #region  Public properties

        public Dictionary<string, int> ValueCounts
        {
            get
            {
                if(_valueCounts == null)
                {
                    _valueCounts = CountValueCounts();
                }

                return _valueCounts;
            }
        }

        public double MeanTime
        {
            get
            {
                if(_meanTime == -1)
                {
                    _meanTime = CountMeanTimeJob();
                }
                    
                return _meanTime;
            }
        }

        public double MaxTime
        {
            get
            {
                if (_maxTime == -1)
                {
                    _maxTime = GetMaxTimeJob();
                }

                return _maxTime;
            }
        }

        public double MinTime
        {
            get
            {
                if (_minTime == -1)
                {
                    _minTime = GetMinTimeJob();
                }

                return _minTime;
            }
        }

        #endregion // Public properties

        #region Private methods

        Dictionary<string, int> CountValueCounts()
        {

            #region Init Dictionary

            Dictionary<string, int>  typeJobsValues = new Dictionary<string, int>();

            foreach (var item in new TypeJobRepository(Properties.Resources.ConnectCommand).GetAllTypeJobs())
            {
                typeJobsValues.Add(item.NameTypeJob, 0);
            }

            #endregion

            #region Count

            foreach (var item in notes)
            {
                typeJobsValues[item.TypeJob.NameTypeJob]++;
            }

            #endregion

            return typeJobsValues;
        }
        double CountMeanTimeJob()
        {
            double meanTimeMin = 0;

            #region Count

            foreach (var item in notes)
            {
                meanTimeMin += (item.TimeFinish - item.TimeStart).TotalMinutes;
            }

            meanTimeMin /= notes.Count;

            #endregion

            return meanTimeMin;
        }
        double GetMaxTimeJob()
        {
            double maxTimeJob = 0;

            #region Count

            foreach (var item in notes)
            {
                var time = item.TimeFinish - item.TimeStart;
                if(time.TotalMinutes > maxTimeJob)
                {
                    maxTimeJob = time.TotalMinutes;
                }
            }

            #endregion

            return maxTimeJob;
        }

        double GetMinTimeJob()
        {
            double minTimeJob = int.MaxValue;

            #region Count

            foreach (var item in notes)
            {
                var time = item.TimeFinish - item.TimeStart;
                if (time.TotalMinutes < minTimeJob)
                {
                    minTimeJob = time.TotalMinutes;
                }
            }

            #endregion

            return minTimeJob;
        }

        void QuitView()
        {
            QuitNotify?.Invoke();
        }

        #endregion // Private methods

        #region Commands

        public RelayCommand QuitCommand
        {
            get
            {
                return new RelayCommand(
                        param => this.QuitView()
                        );
            }
        }

        #endregion // Commands

    }
}
