using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Collections.Generic;

namespace DynamicRuling.Value
{
    /// <summary>
    /// concat Value
    /// a value which concatenates other (sub-)values
    /// </summary>
    [Serializable]
    public class ConcatValue : Element, IValue
    {
        #region Members

        //list of sub-values
        private readonly List<IValue> _values;

        #endregion

        #region Constructors

        /// <summary>
        /// standard constructor
        /// </summary>
        public ConcatValue()
        {
            _values = new List<IValue>();
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        public ConcatValue(SerializationInfo info, StreamingContext context)
        {
            _values = (List<IValue>)info.GetValue("Values", typeof(List<IValue>));
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
        /// Add a value to the list of sub-values
        /// </summary>
        /// <param name="value">the value to add</param>
        public void AddValue(IValue value)
        {
            _values.Add(value);
        }

        /// <summary>
        /// Remove a value from the list of the sub-values
        /// </summary>
        /// <param name="value">the value to remove</param>
        public void RemoveValue(IValue value)
        {
            _values.Remove(value);
        }

        /// <summary>
        /// <see cref="IValue.GetStringValue" />
        /// </summary>
        public string GetStringValue()
        {
            return _values.Aggregate(string.Empty, (current, objValue) => current + objValue.GetStringValue());
        }

        /// <summary>
        /// <see cref="IValue.GetIntValue" />
        /// </summary>
        public int GetIntValue()
        {
            return _values.Sum(objValue => objValue.GetIntValue());
        }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public override bool Check(bool deep)
        {
            return (_values.Count > 0 && !deep) || _values.All(val => val.Check(true));
        }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public override string CheckToString(bool deep)
        {
            var returnVal = string.Empty;
            if (deep)
            {
                if (_values.Any(objValue => !objValue.Check(true)))
                {
                    returnVal = "An underlying value is invalid!";
                }
            }
            return returnVal;
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public override object Clone()
        {
            var cloneObject = new ConcatValue();
            foreach (var val in _values)
            {
                cloneObject.AddValue((IValue)val.Clone());
            }
            return cloneObject;
        }

        /// <summary>
        /// <see cref="object.ToString" />
        /// </summary>
        public override string ToString()
        {
            return "Concatenated Value";
        }

        /// <summary>
        /// <see cref="IElement.GetObjectData" />
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Values", _values);
        }

        /// <summary>
        /// <see cref="IElement.GetNode" />
        /// </summary>
        public override TreeNode GetNode(string prefix)
        {
            var node = new TreeNode(prefix + ToString()) { Tag = this, Name = "ConcatValueNode" };
            foreach (var val in _values)
            {
                node.Nodes.Add(val.GetNode(string.Empty));
            }
            return node;
        }

        /// <summary>
        /// <see cref="IElement.AllowDrop" />
        /// </summary>
        public override bool AllowDrop(object objectToDrop)
        {
            return objectToDrop is IValue;
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

        /// <summary>
        /// <see cref="IElement.Move" />
        /// </summary>
        public override void Move(int index, bool positive, IElement from, IElement to)
        {
            var val = _values[index];
            _values.RemoveAt(index);
            if (positive)
            {
                _values.Insert(index + 1, val);
            }
            else
            {
                _values.Insert(index - 1, val);
            }
        }

        #endregion
    }
}