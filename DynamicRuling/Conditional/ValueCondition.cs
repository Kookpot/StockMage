using System;
using DynamicRuling.Value;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.ComponentModel;

namespace DynamicRuling.Conditional
{
    /// <summary>
    /// Value Condition
    /// condition which compares two values with an equationtype
    /// </summary>
    [Serializable]
    public class ValueCondition : Element, ICondition
    {
        #region Private Members

        //first value of comparison
        private IValue _leftValue;
        //second value of comparison
        private IValue _rightValue;
        //used equationtype
        private EquationType _equationtype;

        [Browsable(true), Category("Settings"), Description("Type of equation for condition")]
        public EquationType Equationtype
        {
            get { return _equationtype; }
            set { _equationtype = value; }
        }

        [Browsable(false)]
        public IValue LeftValue
        {
            get { return _leftValue; }
            set { _leftValue = value; }
        }

        [Browsable(false)]
        public IValue RightValue
        {
            get { return _rightValue; }
            set { _rightValue = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor
        /// </summary>
        public ValueCondition()
        {
            _equationtype = EquationType.EQ;
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">stream context</param>
        public ValueCondition(SerializationInfo info, StreamingContext context)
        {
            _leftValue = (IValue)info.GetValue("Left", typeof(IValue));
            _rightValue = (IValue)info.GetValue("Right", typeof(IValue));
            _equationtype = (EquationType)info.GetValue("Equation", typeof(EquationType));
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
        /// <see cref="ICondition.Evaluate" />
        /// </summary>
        public bool Evaluate()
        {
            try{
                switch (_equationtype)
                {
                    case EquationType.EQ :
                        return (_leftValue.GetIntValue() == _rightValue.GetIntValue());
                    case EquationType.GR :
                        return (_leftValue.GetIntValue() > _rightValue.GetIntValue());
                    case EquationType.LT:
                        return (_leftValue.GetIntValue() < _rightValue.GetIntValue());
                    case EquationType.GREQ:
                        return (_leftValue.GetIntValue() >= _rightValue.GetIntValue());
                    case EquationType.LTEQ:
                        return (_leftValue.GetIntValue() <= _rightValue.GetIntValue());
                    case EquationType.STREQ:
                        return (_leftValue.GetStringValue().Equals(_rightValue.GetStringValue()));
                    case EquationType.CONT:
                        return ((_leftValue.GetStringValue().Contains(_rightValue.GetStringValue()) && 
                            !_rightValue.GetStringValue().Equals(string.Empty)) || 
                            (_rightValue.GetStringValue().Contains(_leftValue.GetStringValue()) && 
                            !_leftValue.GetStringValue().Equals(string.Empty)));

                    case EquationType.STRCONT:
                        return ((_leftValue.GetStringValue().Contains(_rightValue.GetStringValue()) && 
                            !_rightValue.GetStringValue().Equals(string.Empty)));

                    default :
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public override bool Check(bool deep)
        {
            return _leftValue != null && _rightValue != null && (!deep || (_leftValue.Check(true) && _rightValue.Check(true)));
        }

        /// <summary>
        /// <see cref="IElement.CheckToString" />
        /// </summary>
        public override string CheckToString(bool deep)
        {
            var strReturn = string.Empty;
            if (_leftValue == null || _rightValue == null)
            {
                strReturn += Environment.NewLine + "This condition needs exactly two values attached to it!";
            }
            if (deep && ((_leftValue != null && !_leftValue.Check(true)) || (_rightValue != null && !_rightValue.Check(true))))
            {
                strReturn += Environment.NewLine + "An underlying value is invalid!";
            }
            return strReturn;
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public override object Clone()
        {
            var valueCondition = new ValueCondition();
            if (_leftValue != null)
            {
                valueCondition.LeftValue = (IValue)_leftValue.Clone();
            }
            if (_rightValue != null)
            {
                valueCondition.RightValue = (IValue)_rightValue.Clone();
            }
            valueCondition.Equationtype = _equationtype;
            return valueCondition;
        }

        /// <summary>
        /// <see cref="object.ToString" />
        /// </summary>
        public override string ToString()
        {
            var eq = string.Empty;
            switch(_equationtype)
            {
                case EquationType.STREQ:
                    eq = "==";
                    break;
                case EquationType.STRCONT:
                    eq = " like ";
                    break;
                case EquationType.LTEQ:
                    eq = "<=";
                    break;
                case EquationType.GREQ:
                    eq = ">=";
                    break;
                case EquationType.LT:
                    eq = "<";
                    break;
                case EquationType.GR:
                    eq = ">";
                    break;
                case EquationType.EQ:
                    eq = "==";
                    break;
                case EquationType.CONT:
                    eq = "%like%";
                    break;
            }
            return _leftValue + " " +  eq + " "+ _rightValue;
        }

        /// <summary>
        /// <see cref="ICondition.EvaluateToString" />
        /// </summary>
        public string EvaluateToString()
        {
            return string.Format("({0})", ToString());
        }

        /// <summary>
        /// <see cref="IElement.GetObjectData" />
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Left", _leftValue);
            info.AddValue("Right", _rightValue);
            info.AddValue("Equation", _equationtype);
        }

        /// <summary>
        /// <see cref="IElement.GetNode" />
        /// </summary>
        public override TreeNode GetNode(string prefix)
        {
            var node = new TreeNode(prefix + ToString()) { Tag = this, Name = "ValueConditionNode" };
            if (_leftValue != null)
            {
                node.Nodes.Add(_leftValue.GetNode(string.Empty));
            }
            if (_rightValue != null)
            {
                node.Nodes.Add(_rightValue.GetNode(string.Empty));
            }
            return node;
        }

        /// <summary>
        /// <see cref="IElement.AllowDrop" />
        /// </summary>
        public override bool AllowDrop(object objectToDrop)
        {
            if (_leftValue != null && _rightValue != null)
            {
                return false;
            }
            return objectToDrop is IValue;
        }

        /// <summary>
        /// <see cref="IElement.AttachItem" />
        /// </summary>
        public override void AttachItem(object objectToAttach)
        {
            if (_leftValue != null)
            {
                _rightValue = (IValue)objectToAttach;
            } 
            else 
            {
                _leftValue = (IValue)objectToAttach;
            }
        }

        /// <summary>
        /// <see cref="IElement.RemoveItem" />
        /// </summary>
        public override void RemoveItem(object objectToRemove)
        {
            if (_leftValue != null && _leftValue.Equals(objectToRemove))
            {
                _leftValue = null;
            }
            else
            {
                _rightValue = null;
            }
        }

        /// <summary>
        /// <see cref="IElement.Move" />
        /// </summary>
        public override void Move(int intIndex, bool positive, IElement from, IElement to)
        {
            if ((intIndex == 1 && !positive) || (intIndex == 0 && positive))
            {
                var val = LeftValue;
                LeftValue = RightValue;
                RightValue = val;
            }
        }

        #endregion
    }
}