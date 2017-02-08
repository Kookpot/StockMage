using System;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.ComponentModel;
using DynamicRuling.Value;

namespace DynamicRuling.Instruction
{
    /// <summary>
    /// AssignTemp instruction
    /// assign a value to a temporary value
    /// </summary>
    [Serializable]
    public class AssignTempInstruction : Element, IInstruction
    {
        #region Private Members

        //name of the temporary value
        private string _tempName;
        //value to retrieve the data from
        private IValue _value;

        [Browsable(true), Category("Settings"), Description("TempName")]
        public string TempName
        {
            get { return _tempName; }
            set { _tempName = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor
        /// </summary>
        public AssignTempInstruction()
        {
            _tempName = "temp1";
            _value = null;
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        public AssignTempInstruction(SerializationInfo info, StreamingContext context)
        {
            _tempName = (string)info.GetValue("TempName", typeof(string));
            _value = (IValue)info.GetValue("Value", typeof(IValue));
        }

        /// <summary>
        /// constructor with params
        /// </summary>
        /// <param name="tempName">name of the temporary value</param>
        public AssignTempInstruction(string tempName)
        {
            _tempName = tempName;
            _value = null;
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
        /// add value (only one allowed)
        /// </summary>
        /// <param name="valueToAdd">value to be added</param>
        public void AddValue(IValue valueToAdd)
        {
            _value = valueToAdd;
        }

        /// <summary>
        /// remove a value
        /// </summary>
        /// <param name="valueToRemove">value to be removed</param>
        public void RemoveValue(IValue valueToRemove)
        {
            _value = null;
        }

        /// <summary>
        /// <see cref="IInstruction.Execute" />
        /// </summary>
        public void Execute()
        {
            _value.GetStringValue();
            var converter = Converter.GetInstance();
            if (!converter.TempValues.ContainsKey(_tempName))
            {
                converter.TempValues.Add(_tempName, _value.GetStringValue());
            }
            else
            {
                converter.TempValues[_tempName] = _value.GetStringValue();
            }
        }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public override bool Check(bool deep)
        {
            if (deep)
            {
                if (_value == null || !_value.Check(true))
                {
                    return false;
                }
            }
            return (!_tempName.Equals(string.Empty) && _value!=null);
        }

        /// <summary>
        /// <see cref="IElement.CheckToString" />
        /// </summary>
        public override string CheckToString(bool deep)
        {
            var returnVal = string.Empty;
            if (deep)
            {
                if (_value==null || !_value.Check(true))
                {
                    returnVal = "The value is invalid";
                }
            }
            if (_tempName.Equals(string.Empty))
            {
                returnVal += Environment.NewLine + "The tempname of this instruction must be valid and cannot be empty!";
            }
            return returnVal;
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public override object Clone()
        {
            var temp = new AssignTempInstruction { TempName = _tempName };
            if (_value != null)
            {
                temp.AddValue((IValue)_value.Clone());
            }
            return temp;
        }

        /// <summary>
        /// <see cref="object.ToString" />
        /// </summary>
        public override string ToString()
        {
            return string.Format("Asign Value to Temp Value '{0}'");
        }

        /// <summary>
        /// <see cref="IElement.GetObjectData" />
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TempName", _tempName);
            info.AddValue("Value", _value);
        }

        /// <summary>
        /// <see cref="IElement.GetNode" />
        /// </summary>
        public override TreeNode GetNode(string prefix)
        {
            var node = new TreeNode(prefix + ToString()) { Tag = this, Name = "AssignTempInstructionNode" };
            if (_value != null)
            {
                node.Nodes.Add(_value.GetNode(string.Empty));
            }
            return node;
        }

        /// <summary>
        /// <see cref="IElement.AllowDrop" />
        /// </summary>
        public override bool AllowDrop(object objectToDrop)
        {
            return objectToDrop is IValue && _value == null;
        }

        /// <summary>
        /// <see cref="IElement.AttachItem" />
        /// </summary>
        public override void AttachItem(object objectToAttach)
        {
            AddValue((IValue)objectToAttach);
        }

        /// <summary>
        /// <see cref="IElement.RemoveItem" />
        /// </summary>
        public override void RemoveItem(object objectToRemove)
        {
            RemoveValue((IValue)objectToRemove);
        }

        #endregion
    }
}