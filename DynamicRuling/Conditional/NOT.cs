using System;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace DynamicRuling.Conditional
{
    /// <summary>
    /// NOT Condition
    /// reverts the evalutation of one (and only one) subcondition
    /// </summary>
    [Serializable]
    public class NOT : Element, ICondition
    {
        #region Members

        //underlying condition
        private ICondition _condition;

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor
        /// </summary>
        public NOT()
        { }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">context</param>
        public NOT(SerializationInfo info, StreamingContext context)
        {
            _condition = (ICondition)info.GetValue("NOT", typeof(ICondition));
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
        /// <see cref="ICondition.SetCondition" />
        /// </summary>
        public void SetCondition(ICondition condition)
        {
            _condition = condition;
        }

        /// <summary>
        /// <see cref="ICondition.Evaluate" />
        /// </summary>
        public bool Evaluate()
        {
            return !_condition.Evaluate();
        }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public override bool Check(bool blnDeep)
        {
            return _condition != null && (!blnDeep || _condition.Check(true));
        }

        /// <summary>
        /// <see cref="IElement.CheckToString" />
        /// </summary>
        public override string CheckToString(bool deep)
        {
            var strReturn = string.Empty;
            if (_condition == null)
            {
                strReturn += Environment.NewLine + "This condition (NOT) needs exactly one condition attached to it!";
            }
            if (deep && _condition != null && !_condition.Check(true))
            {
                strReturn += Environment.NewLine + "The underlying condition is invalid!";
            }
            return strReturn;
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public override object Clone()
        {
           var not = new NOT();
           if (_condition != null)
           {
               not.SetCondition((ICondition)_condition.Clone());
           }
           return not;
        }

        /// <summary>
        /// <see cref="object.ToString" />
        /// </summary>
        public override string ToString()
        {
            return string.Empty;
        }

        /// <summary>
        /// <see cref="ICondition.EvaluateToString" />
        /// </summary>
        public string EvaluateToString()
        {
            if (_condition != null)
            {
                return string.Format("(NOT {0})", _condition.EvaluateToString());
            }
            return "(NOT )";
        }

        /// <summary>
        /// <see cref="IElement.GetObjectData" />
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("NOT", _condition);
        }

        /// <summary>
        /// <see cref="IElement.GetNode" />
        /// </summary>
        public override TreeNode GetNode(string prefix)
        {
            var node = new TreeNode(prefix + "NOT  " + ToString()) { Tag = this, Name = "NOTNode" };
            if (_condition != null)
            {
                node.Nodes.Add(_condition.GetNode(string.Empty));
            }
            return node;
        }

        /// <summary>
        /// <see cref="IElement.AllowDrop" />
        /// </summary>
        public override bool AllowDrop(object objectToDrop)
        {
            return _condition == null && objectToDrop is ICondition;
        }

        /// <summary>
        /// <see cref="IElement.AttachItem" />
        /// </summary>
        public override void AttachItem(object objectToAttach)
        {
            SetCondition((ICondition)objectToAttach);
        }

        /// <summary>
        /// <see cref="IElement.RemoveItem" />
        /// </summary>
        public override void RemoveItem(object objectToRemove)
        {
            SetCondition(null);
        }

        #endregion
    }
}