using System;
using System.Windows.Forms;

namespace RulingDesigner
{
    /// <summary>
    /// select dialog
    /// </summary>
    public partial class SelectDialog : Form
    {
        #region Members

        public string SelectedText
        {
            get { return comboBox1.SelectedItem.ToString(); }
        }

        public string Value
        {
            get { return textBox1.Text; }
        }

        public string Value2
        {
            get { return textBox2.Text; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// constructor with params
        /// </summary>
        /// <param name="values">list of values to set in the combobox to choose from</param>
        /// <param name="label1">text of first label</param>
        /// <param name="label2">text of second label</param>
        public SelectDialog(string[] values, string label1, string label2)
        {
            InitializeComponent();
            comboBox1.Items.AddRange(values);
            this.label1.Text = label1;
            this.label2.Text = label2;
        }

        /// <summary>
        /// when the button is clicked then the dialog result is ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        #endregion
    }
}