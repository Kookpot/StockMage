using System;
using System.Windows.Forms;

namespace RulingDesigner
{
    /// <summary>
    /// dialog to enter a searchpattern
    /// </summary>
    public partial class SearchDialog : Form
    {
        #region Members

        //the searchpattern filled in
        public string Value
        {
            get { return textBox1.Text; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// constructor with params
        /// </summary>
        /// <param name="label">the text of the label</param>
        public SearchDialog(string label)
        {
            InitializeComponent();
            label1.Text = label;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// when OK is clicked
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of event</param>
        private void Button1Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        #endregion
    }
}