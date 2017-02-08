using System;
using System.Runtime.Serialization;

namespace DynamicRuling.Value
{
    /// <summary>
    /// responsible for transforming an (underlying) value
    /// the result is again a value
    /// </summary>
    [Serializable]
    public class TransformValue : Element, IValue
    {
        #region Members

        //the underlying value
        protected IValue _value;

        #endregion

        #region Constructors

        /// <summary>
        /// standard constructor
        /// </summary>
        public TransformValue()
        { }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        public TransformValue(SerializationInfo info, StreamingContext context)
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="IValue.GetStringValue" />
        /// </summary>
        public virtual string GetStringValue()
        {
            return string.Empty;
        }

        /// <summary>
        /// <see cref="IValue.GetIntValue" />
        /// </summary>
        public virtual int GetIntValue()
        {
            return 0;
        }

        /// <summary>
        /// set underlying value to the given value
        /// </summary>
        /// <param name="value">value to set</param>
        public void SetValue(IValue value)
        {
            _value = value;
        }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public override bool Check(bool blnDeep)
        {
            return !blnDeep || _value.Check(true);
        }

        /// <summary>
        /// <see cref="IElement.CheckToString" />
        /// </summary>
        /// <param name="deep">deep check?</param>
        /// <returns>in string format errors or warnings</returns>
        public override string CheckToString(bool deep)
        {
            var returnValue = string.Empty;
            if (deep)
            {
                if (!_value.Check(true))
                {
                    returnValue += Environment.NewLine + "The underlying value is incorrect!";
                }
            }
            return returnValue;
        }

        /// <summary>
        /// <see cref="object.ToString" />
        /// </summary>
        public override string ToString()
        {
            return string.Empty;
        }

        /// <summary>
        /// <see cref="IElement.AllowDrop" />
        /// </summary>
        public override bool AllowDrop(object objectToDrop)
        {
            return _value == null && (objectToDrop is IValue);
        }

        /// <summary>
        /// <see cref="IElement.AttachItem" />
        /// </summary>
        public override void AttachItem(object objectToAttach)
        {
            SetValue((IValue)objectToAttach);
        }

        /// <summary>
        /// <see cref="IElement.RemoveItem" />
        /// </summary>
        public override void RemoveItem(object objectToRemove)
        {
            SetValue(null);
        }

        #endregion
    }
}