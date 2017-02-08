using System;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.ComponentModel;

namespace DynamicRuling.Value
{
    /// <summary>
    /// responsible for a temporary variable in the configuration
    /// the variable can be set and be retrieved
    /// </summary>
    [Serializable]
    public class TempValue : Element, IValue
    {
        #region Members

        //name of the variable
        private string _tempName;

        [Browsable(true), Category("Settings"), Description("Temp name")]
        public string TempName
        {
            get { return _tempName; }
            set { _tempName = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// standard constructor
        /// </summary>
        public TempValue()
        {
            _tempName = "temp1";
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">stream context</param>
        public TempValue(SerializationInfo info, StreamingContext context)
        {
            _tempName = (string)info.GetValue("TempName", typeof(string));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="IElement.Showable" />
        /// </summary>
        public override bool Showable()
        {
            return true;
        }

        /// <summary>
        /// <see cref="IElement.GetStringValue" />
        /// </summary>
        public string GetStringValue()
        {
            var converter = Converter.GetInstance();
            return converter.TempValues.ContainsKey(_tempName) ? converter.TempValues[_tempName] : string.Empty;
        }

        /// <summary>
        /// <see cref="IValue.GetIntValue" />
        /// </summary>
        public int GetIntValue()
        {
            var converter = Converter.GetInstance();
            return converter.TempValues.ContainsKey(_tempName) ? int.Parse(converter.TempValues[_tempName]) : 0;
        }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public override bool Check(bool deep)
        {
            return !_tempName.Equals(string.Empty);
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public override object Clone()
        {
            return new TempValue { TempName = _tempName };
        }

        /// <summary>
        /// <see cref="object.ToString" />
        /// </summary>
        public override string ToString()
        {
            return string.Format("Temp Value '{0}'", _tempName);
        }

        /// <summary>
        /// <see cref="IElement.GetObjectData" />
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TempName", _tempName);
        }

        /// <summary>
        /// <see cref="IElement.GetNode" />
        /// </summary>
        public override TreeNode GetNode(string prefix)
        {
            return new TreeNode(prefix + ToString()) { Tag = this, Name = "TempValueNode" };
        }

        #endregion
    }
}