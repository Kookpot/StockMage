using System;
using System.Windows.Forms;
using System.Threading;

namespace DynamicRuling
{
    /// <summary>
    /// progressform which initializes a conversion and keeps track of it's progress
    /// </summary>
    [Serializable]
    public partial class ProgressForm : Form
    {
        #region Constructor

        /// <summary>
        /// standard constructor for progressform
        /// the beginning
        /// </summary>
        public ProgressForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// show this dialog
        /// set begindate
        /// attach the 3 events (start, update,end) to the convertor
        /// execute the conversion in a separate thread
        /// </summary>
        /// <returns>dialog result</returns>
        public DialogResult ShowThisDialog()
        {
            var thread = new Thread(() => Converter.GetInstance().Execute());
            thread.Start();
            return ShowDialog();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Converter.GetInstance().End();
        }

        #endregion
    }
}