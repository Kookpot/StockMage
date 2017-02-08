using System;

namespace DynamicRuling
{
    /// <summary>
    /// starteventargs
    /// arguments of the event : the start of the conversion
    /// </summary>
    public class StartEventArgs : EventArgs
    {
        #region Members

        //number of lines to process
        public int MaxCount { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="max">number of lines to process</param>
        public StartEventArgs(int max)
        {
            MaxCount = max;
        }

        #endregion
    }
}