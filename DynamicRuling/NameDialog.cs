using System;
using System.Windows.Forms;

namespace DynamicRuling
{
    /// <summary>
    /// name dialog for renaming unique items in our designer
    /// </summary>
    public partial class NameDialog : Form
    {
        #region Members

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public string NewName { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// standard constructor
        /// </summary>
        /// <param name="title">title of the dialog</param>
        /// <param name="label">question in the label</param>
        public NameDialog(string title, string label)
        {
            InitializeComponent();
            Text = title;
            label1.Text = label;
            DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// user click on ok
        /// the content of the textbox is stored in the variable newname
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of event</param>
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            NewName = textBox1.Text;
        }

        /// <summary>
        /// if a key was entered and it was enter...
        /// the content of the textbox is stored in the variable newname
        /// yes, I copy/pasted the two lines of code... sue me :-)
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of keyevent</param>
        private void ThisEntered(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                DialogResult = DialogResult.OK;
                NewName = textBox1.Text;
            }
        }

        #endregion
    }
}