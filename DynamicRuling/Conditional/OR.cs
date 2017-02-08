using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace DynamicRuling.Conditional
{
    /// <summary>
    /// OR Condition
    /// combines the boolean evaluation of two or more subconditions
    /// </summary>
    [Serializable]
    public class OR : Element, ICondition
    {
        #region Members

        //list of subconditions
        private readonly List<ICondition> _conditions;

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor
        /// </summary>
        public OR()
        {
            _conditions = new List<ICondition>();
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        public OR(SerializationInfo info, StreamingContext context)
        {
            _conditions = (List<ICondition>)info.GetValue("OR", typeof(List<ICondition>));
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
            return _conditions.Any(cond => cond.Evaluate());
        }

        /// <summary>
        /// <see cref="IElement.CheckToString" />
        /// </summary>
        public override string CheckToString(bool deep)
        {
            var returnVal = string.Empty;
            if (_conditions.Count >= 2)
            {
                returnVal += Environment.NewLine + "This condition (OR) needs 2 or more conditions attached to it.";
            }
            if (deep)
            {
                if (_conditions.Any(objCondition => !objCondition.Check(true)))
                {
                    returnVal += Environment.NewLine + "An underlying condition is invalid!";
                }
            }
            return returnVal;
        }

        /// <summary>
        /// add condition to the list of conditions
        /// </summary>
        /// <param name="conditionToAdd">condition to add</param>
        public void AddCondition(ICondition conditionToAdd)
        {
            _conditions.Add(conditionToAdd);
        }

        /// <summary>
        /// remove condition from the list of conditions
        /// </summary>
        /// <param name="conditionToRemove">condition to remove</param>
        public void RemoveCondition(ICondition conditionToRemove)
        {
            _conditions.Remove(conditionToRemove);
        }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public override bool Check(bool deep)
        {
            if (deep)
            {
                if (_conditions.Any(cond => !cond.Check(true)))
                {
                    return false;
                }
            }
            return _conditions.Count >= 2;
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public override object Clone()
        {
            var or = new OR();
            foreach (var condition in _conditions)
            {
                or.AddCondition((ICondition)condition.Clone());
            }
            return or;
        }

        /// <summary>
        /// <see cref="object.ToString" />
        /// </summary>
        public override string ToString()
        {
            return string.Format("(Count : {0})", _conditions.Count);
        }

        /// <summary>
        /// <see cref="IElement.EvaluateToString" />
        /// </summary>
        public string EvaluateToString()
        {
            if (_conditions.Count > 0)
            {
                var returnVal = _conditions.Aggregate("(", (current, cond) => current + cond.EvaluateToString() + " OR ");
                return returnVal.Substring(0, returnVal.Length - 5) + ")";
            }
            return "( )";
        }

        /// <summary>
        /// <see cref="IElement.GetObjectData" />
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("OR", _conditions);
        }

        /// <summary>
        /// <see cref="IElement.GetNode" />
        /// </summary>
        public override TreeNode GetNode(string prefix)
        {
            var node = new TreeNode(prefix + "OR  " + ToString()) { Tag = this, Name = "ORNode" };
            foreach (var condition in _conditions)
            {
                node.Nodes.Add(condition.GetNode(string.Empty));
            }
            return node;
        }

        /// <summary>
        /// <see cref="IElement.AllowDrop" />
        /// </summary>
        public override bool AllowDrop(object objectToDrop)
        {
            return objectToDrop is ICondition;
        }

        /// <summary>
        /// <see cref="IElement.AttachItem" />
        /// </summary>
        public override void AttachItem(object objectToAttach)
        {
            AddCondition((ICondition)objectToAttach);
        }

        /// <summary>
        /// <see cref="IElement.RemoveItem" />
        /// </summary>
        public override void RemoveItem(object objectToRemove)
        {
            RemoveCondition((ICondition) objectToRemove);
        }

        /// <summary>
        /// <see cref="IElement.Move" />
        /// </summary>
        public override void Move(int index, bool positive, IElement from, IElement to)
        {
            var cond = _conditions[index];
            _conditions.RemoveAt(index);
            if (positive)
            {
                _conditions.Insert(index + 1, cond);
            }
            else
            {
                _conditions.Insert(index - 1, cond);
            }
        }

        #endregion
    }
}