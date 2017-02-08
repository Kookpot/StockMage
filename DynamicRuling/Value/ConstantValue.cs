using System;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.ComponentModel;

namespace DynamicRuling.Value
{
    /// <summary>
    /// ConstantValue
    /// A value which represent a constant string
    /// </summary>
    [Serializable]
    public class ConstantValue : Element, IValue
    {
        #region Members

        //a constant
        private string _constant;

        [Browsable(true), CategoryAttribute("Settings"), DescriptionAttribute("Constant value")]
        public string Constant
        {
            get { return _constant; }
            set { _constant = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// standard constructor
        /// </summary>
        public ConstantValue()
        {
            _constant = "1";
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        public ConstantValue(SerializationInfo info, StreamingContext context)
        {
            _constant = (string)info.GetValue("Constant", typeof(string));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="ICheckable.Showable" />
        /// </summary>
        public override bool Showable()
        {
            return true;
        }

        /// <summary>
        /// <see cref="IValue.GetStringValue" />
        /// </summary>
        public string GetStringValue()
        {
            return Check(false) ? _constant : string.Empty;
        }

        /// <summary>
        /// <see cref="IValue.GetIntValue" />
        /// </summary>
        public int GetIntValue()
        {
            return Check(false) ? int.Parse(_constant) : 0;
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public override object Clone()
        {
            return new ConstantValue { Constant = _constant };
        }

        /// <summary>
        /// <see cref="object.ToString" />
        /// </summary>
        public override string ToString()
        {
            return string.Format("Constant Value '{0}'", _constant);
        }

        /// <summary>
        /// <see cref="IElement.GetObjectData" />
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Constant", _constant);
        }

        /// <summary>
        /// <see cref="IElement.GetNode" />
        /// </summary>
        public override TreeNode GetNode(string prefix)
        {
            return new TreeNode(prefix + ToString()) { Tag = this, Name = "ConstantValueNode" };
        }

        #endregion
    }
}