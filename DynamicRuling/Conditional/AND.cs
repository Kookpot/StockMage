using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace DynamicRuling.Conditional
{
    /// <summary>
    /// AND Condition
    /// combines the boolean evaluation of two or more subconditions
    /// </summary>
    [Serializable]
    public class AND : Element, ICondition
    {
        #region Members

        //list of subconditions
        private readonly List<ICondition> _conditions;

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor
        /// </summary>
        public AND()
        {
            _conditions = new List<ICondition>();
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">stream context</param>
        public AND(SerializationInfo info, StreamingContext context)
        {
            _conditions = (List<ICondition>)info.GetValue("AND", typeof(List<ICondition>));
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
        /// add condition to the list of subconditions
        /// </summary>
        /// <param name="condition">condition to add</param>
        public void AddCondition(ICondition condition)
        {
            _conditions.Add(condition);
        }

        /// <summary>
        /// remove condition from the list of subconditions
        /// </summary>
        /// <param name="condition">condition to remove</param>
        public void RemoveCondition(ICondition condition)
        {
            _conditions.Remove(condition);
        }

        /// <summary>
        /// <see cref="ICondition.Evaluate" />
        /// </summary>
        public bool Evaluate()
        {
            return _conditions.All(condition => condition.Evaluate());
        }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public override bool Check(bool deep)
        {
            if (deep)
            {
                if (_conditions.Any(condition => !condition.Check(true)))
                {
                    return false;
                }
            }
            return _conditions.Count >= 2;
        }

        /// <summary>
        /// <see cref="IElement.CheckToString" />
        /// </summary>
        public override string CheckToString(bool deep)
        {
            var returnVal = string.Empty;
            if (_conditions.Count >= 2)
            {
                returnVal += Environment.NewLine + "This condition (AND) needs 2 or more conditions attached to it!";
            }
            if (deep)
            {
                if (_conditions.Any(condition => !condition.Check(true)))
                {
                    returnVal += Environment.NewLine + "An underlying condition is invalid!";
                }
            }
            return returnVal;
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public override object Clone()
        {
            var and = new AND();
            foreach (var condition in _conditions)
            {
                and.AddCondition((ICondition)condition.Clone());
            }
            return and;
        }

        /// <summary>
        /// <see cref="object.ToString" />
        /// </summary>
        public override string ToString()
        {
            return string.Format("(Count : {0})", _conditions.Count);
        }

        /// <summary>
        /// <see cref="IElement.GetObjectData" />
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AND", _conditions);
        }

        /// <summary>
        /// <see cref="IElement.GetNode" />
        /// </summary>
        public override TreeNode GetNode(string prefix)
        {
            var node = new TreeNode(prefix + "AND  " + this) {Tag = this, Name = "ANDNode"};
            foreach (var condition in _conditions)
            {
                node.Nodes.Add(condition.GetNode(string.Empty));
            }
            return node;
        }

        /// <summary>
        /// <see cref="ICondition.EvualuateToString" />
        /// </summary>
        public string EvaluateToString()
        {
            if (_conditions.Count > 0)
            {
                var temp = _conditions.Aggregate("(", (current, objCond) => current + objCond.EvaluateToString() + " AND ");
                return temp.Substring(0, temp.Length - 5) + ")";
            }
            return "( )";
        }

        /// <summary>
        /// <see cref="IElement.AllowDrop"/>
        /// </summary>
        public override bool AllowDrop(object objectToDrop)
        {
            return objectToDrop is ICondition;
        }

        /// <summary>
        /// <see cref="IElement.AttachItem"/>
        /// </summary>
        public override void AttachItem(object objectToAttach)
        {
            AddCondition((ICondition)objectToAttach);
        }

        /// <summary>
        /// <see cref="IElement.RemoveItem"/>
        /// </summary>
        public override void RemoveItem(object objectToRemove)
        {
            RemoveCondition((ICondition)objectToRemove);
        }

        /// <summary>
        /// <see cref="IElement.Move"/>
        /// </summary>
        public override void Move(int index, bool positive, IElement from, IElement to)
        {
            var toMove = _conditions[index];
            _conditions.RemoveAt(index);
            if (positive)
            {
                _conditions.Insert(index + 1, toMove);
            }
            else
            {
                _conditions.Insert(index - 1, toMove);
            }
        }

        #endregion
    }
}