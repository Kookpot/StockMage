using System;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.ComponentModel;

namespace DynamicRuling.Value
{
    /// <summary>
    /// responsible for trasforming the underlying value by replacing a searchpattern with a given text
    /// </summary>
    [Serializable]
    public class ReplaceValue : TransformValue
    {
        #region Private Members

        //search pattern
        private string _search;
        //replace text
        private string _replace;

        [Browsable(true), Category("Settings"), Description("String to search in the value")]
        public string Search
        {
            get { return _search; }
            set { _search = value; }
        }

        [Browsable(true), Category("Settings"), Description("String to replace the searched string in the value")]
        public string Replace
        {
            get { return _replace; }
            set { _replace = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// standard constructor
        /// </summary>
        public ReplaceValue()
        {
            _search = "_";
            _replace = string.Empty;
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">stream context</param>
        public ReplaceValue(SerializationInfo info, StreamingContext context)
        {
            _replace = (string)info.GetValue("Replace", typeof(string));
            _search = (string)info.GetValue("Search", typeof(string));
            _value = (IValue)info.GetValue("Value", typeof(IValue));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="IValue.GetStringValue" />
        /// </summary>
        public override string GetStringValue()
        {
            return _value.GetStringValue().Replace(_search, _replace);
        }

        /// <summary>
        /// <see cref="IValue.GetIntValue" />
        /// </summary>
        public override int GetIntValue()
        {
            return int.Parse(_value.GetIntValue().ToString().Replace(_search, _replace));
        }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public override bool Check(bool blnDeep)
        {
            return !_search.Equals(string.Empty);
        }

        /// <summary>
        /// <see cref="IElement.CheckToString" />
        /// </summary>
        public override string CheckToString(bool blnDeep)
        {
            var returnVal = string.Empty;
            if (_search.Equals(string.Empty))
            {
                returnVal += Environment.NewLine + "The filter you want to search by cannot be empty!";
            }
            return returnVal + base.CheckToString(blnDeep);
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public override object Clone()
        {
            var replace = new ReplaceValue { Search = _search, Replace = _replace };
            if (_value != null)
            {
                replace.SetValue((IValue)_value.Clone());
            }
            return replace;
        }

        /// <summary>
        /// <see cref="object.ToString" />
        /// </summary>
        public override string ToString()
        {
            return string.Format("Search '{0}' and replace with '{1}'", _search, _replace);
        }

        /// <summary>
        /// <see cref="IElement.GetObjectData" />
        /// </summary>
        public override void GetObjectData(SerializationInfo objInfo, StreamingContext objContext)
        {
            objInfo.AddValue("Search", _search);
            objInfo.AddValue("Replace", _replace);
            objInfo.AddValue("Value", _value);
        }

        /// <summary>
        /// <see cref="IElement.GetNode" />
        /// </summary>
        public override TreeNode GetNode(string prefix)
        {
            var node = new TreeNode(prefix + ToString()) { Tag = this, Name = "ReplaceValueNode" };
            if (_value != null)
            {
                node.Nodes.Add(_value.GetNode(string.Empty));
            }
            return node;
        }

        #endregion
    }
}