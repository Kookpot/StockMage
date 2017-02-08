using System.Windows.Forms;

namespace RulingDesigner
{
    /// <summary>
    /// dialog to show properties of an element
    /// </summary>
    public partial class PropertyDialog : Form
    {
        #region Constructors

        #region Public Members

        //object of which we show the properties
        public object PropertyObject
        {
            get { return propertyGrid1.SelectedObject; }
            set { propertyGrid1.SelectedObject = value; }
        }

        #endregion

        /// <summary>
        /// standard constructor
        /// </summary>
        public PropertyDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// if a key was given and it's enter -> close this form
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of keyevent</param>
        private void ThisEntered(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Close();
            }
        }

        #endregion
    }
}