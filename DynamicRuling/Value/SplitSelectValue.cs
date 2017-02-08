using System;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.ComponentModel;

namespace DynamicRuling.Value
{
    /// <summary>
    /// transform the underlying value by splitting it with a given delimiter and taking the part that corresponds to the given index
    /// </summary>
    [Serializable]
    public class SplitSelectValue : TransformValue
    {
        #region Members

        //delimiter to use in the splitting
        private string _delimiter;
        //the index of the part that we need
        private int _index;

        [Browsable(true), Category("Settings"), Description("String to split the value by")]
        public string Delimiter
        {
            get { return _delimiter; }
            set { _delimiter = value; }
        }

        [Browsable(true), Category("Settings"), Description("Index of the splitted value")]
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// standard constructor
        /// </summary>
        public SplitSelectValue()
        {
            _index = 0;
            _delimiter = ".";
        }
        
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">stream context</param>
        public SplitSelectValue(SerializationInfo info, StreamingContext context)
        {
            _delimiter = (string)info.GetValue("Delimiter", typeof(string));
            _index = (int)info.GetValue("Index", typeof(int));
            _value = (IValue)info.GetValue("Value", typeof(IValue));
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
        /// <see cref="IValue.GetStringValue" />
        /// </summary>
        public override string GetStringValue()
        {
            var splits = _value.GetStringValue().Split(_delimiter.ToCharArray());
            return splits.Length > _index ? splits[_index] : string.Empty;
        }

        /// <summary>
        /// <see cref="IValue.GetIntValue" />
        /// </summary>
        public override int GetIntValue()
        {
            return int.Parse(_value.GetIntValue().ToString().Split(_delimiter.ToCharArray())[_index]);
        }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public override bool Check(bool blnDeep)
        {
            if (_index >= 0 && !_delimiter.Equals(string.Empty))
            {
                return base.Check(blnDeep);
            }
            return false;
        }

        /// <summary>
        /// <see cref="IElement.CheckToString" />
        /// </summary>
        public override string CheckToString(bool blnDeep)
        {
            var returnVal = string.Empty;
            if (_index < 0)
            {
                returnVal += Environment.NewLine + "The index of the splitted array must be a positive number!";
            }
            if (_delimiter.Equals(string.Empty))
            {
                returnVal += Environment.NewLine + "The delimiter to split by, may not be empty!";
            }
            return returnVal+ base.CheckToString(blnDeep);
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public override object Clone()
        {
            var splitSelect = new SplitSelectValue { Delimiter = _delimiter, Index = _index };
            if (_value != null)
            {
                splitSelect.SetValue((IValue)_value.Clone());
            }
            return splitSelect;
        }

        /// <summary>
        /// <see cref="object.ToString" />
        /// </summary>
        public override string ToString()
        {
            return string.Format("Split with Delimiter '{0}' and get Index {1}", _delimiter , _index);
        }

        /// <summary>
        /// <see cref="IElement.GetObjectData" />
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Delimiter", _delimiter);
            info.AddValue("Index", _index);
            info.AddValue("Value", _value);
        }

        /// <summary>
        /// <see cref="IElement.GetNode" />
        /// </summary>
        public override TreeNode GetNode(string strPrefix)
        {
            var node = new TreeNode(strPrefix + ToString()) { Tag = this, Name = "SplitSelectValueNode" };
            if (_value != null)
            {
                node.Nodes.Add(_value.GetNode(string.Empty));
            }
            return node;
        }

        #endregion
    }
}