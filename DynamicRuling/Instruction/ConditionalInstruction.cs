using System;
using DynamicRuling.Conditional;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.ComponentModel;
using DynamicRuling.Value;

namespace DynamicRuling.Instruction
{
    /// <summary>
    /// conditional instruction
    /// executes the first instruction if the condition evaluates positively
    /// executes the second instruction if it evaluates false and if there is a second instruction
    /// </summary>
    [Serializable]
    public class ConditionalInstruction : Element, IInstruction
    {
        #region Members

        //if instruction (first instruction)
        private IInstruction _ifInstruction;
        //else instruction (else instruction)
        private IInstruction _elseInstruction;
        //condition
        private ICondition _ifCondition;


        [Browsable(false)]
        public IInstruction IFInstruction
        {
            get { return _ifInstruction; }
            set { _ifInstruction = value; }
        }

        [Browsable(false)]
        public IInstruction ELSEInstruction
        {
            get { return _elseInstruction; }
            set { _elseInstruction = value; }
        }

        [Browsable(false)]
        public ICondition IFCondition
        {
            get { return _ifCondition; }
            set { _ifCondition = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor
        /// </summary>
        public ConditionalInstruction()
        {
            _ifInstruction = null;
            _elseInstruction = null;
            _ifCondition = null;
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">stream context</param>
        public ConditionalInstruction(SerializationInfo info, StreamingContext context)
        {
            _ifInstruction = (IInstruction)info.GetValue("IF", typeof(IInstruction));
            _elseInstruction = (IInstruction)info.GetValue("ELSE", typeof(IInstruction));
            _ifCondition = (ICondition)info.GetValue("Condition", typeof(ICondition));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="IElement.GetPrefix" />
        /// </summary>
        public override string GetPrefix(IElement obj)
        {
            if (obj is ICondition)
            {
                return string.Empty;
            }
            return _ifInstruction == obj ? "THEN " : "ELSE ";
        }

        /// <summary>
        /// <see cref="IElement.Showable" />
        /// </summary>
        public override bool Showable()
        {
            return true;
        }

        /// <summary>
        /// <see cref="IInstruction.Execute" />
        /// </summary>
        public void Execute()
        {
            if (_ifCondition.Evaluate())
            {
                _ifInstruction.Execute();
            }
            else if (_elseInstruction != null)
            {
                _elseInstruction.Execute();
            }
        }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public override bool Check(bool deep)
        {
            return _ifCondition != null && (!deep || (_ifCondition.Check(true) && (_ifInstruction == null || _ifInstruction.Check(true)) && 
                (_elseInstruction == null || _elseInstruction.Check(true))));
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public override string CheckToString(bool deep)
        {
            var returnVal = string.Empty;
            if (_ifCondition == null)
            {
                returnVal += Environment.NewLine + "The underlying condition is incorrect!";
            }
            if (_ifInstruction == null && _elseInstruction == null)
            {
                returnVal += Environment.NewLine + "This conditional instruction needs at least one underlying instruction when the condition is true or false!";
            }
            if (deep)
            {
                if (_ifCondition != null)
                    if (!_ifCondition.Check(true))
                    {
                        returnVal += Environment.NewLine + "The underlying condition is incorrect!";
                    }
                if ((_ifInstruction != null && _ifInstruction.Check(true)) || (_elseInstruction != null && _elseInstruction.Check(true)))
                {
                    returnVal += Environment.NewLine + "This conditional instruction needs at least one underlying instruction when the condition is true or false!";
                }
            }
            return returnVal;
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public override object Clone()
        {
            var conditional = new ConditionalInstruction();
            if (_ifCondition != null)
            {
                conditional.IFCondition = (ICondition)_ifCondition.Clone();
            }
            if (_ifInstruction != null)
            {
                conditional.IFInstruction = (IInstruction)_ifInstruction.Clone();
            }
            if (_elseInstruction != null)
            {
                conditional.ELSEInstruction = (IInstruction)_elseInstruction.Clone();
            }
            return conditional;
        }

        /// <summary>
        /// <see cref="object.ToString" />
        /// </summary>
        public override string ToString()
        {
            if (_ifCondition != null)
            {
                return "IF " + _ifCondition.EvaluateToString();
            }
            return "IF";
        }

        /// <summary>
        /// <see cref="IElement.GetObjectData" />
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("IF", _ifInstruction);
            info.AddValue("ELSE", _elseInstruction);
            info.AddValue("Condition", _ifCondition);
        }

        /// <summary>
        /// <see cref="IElement.GetNode" />
        /// </summary>
        public override TreeNode GetNode(string prefix)
        {
            var node = new TreeNode(prefix + ToString()) { Tag = this, Name = "ConditionalInstructionNode" };
            if (_ifInstruction != null)
            {
                node.Nodes.Add(_ifInstruction.GetNode("THEN "));
            }
            if (_elseInstruction != null)
            {
                node.Nodes.Add(_elseInstruction.GetNode("ELSE "));
            }
            if (_ifCondition != null)
            {
                node.Nodes.Add(_ifCondition.GetNode(string.Empty));
            }
            return node;
        }

        /// <summary>
        /// <see cref="IElement.AllowDrop" />
        /// </summary>
        public override bool AllowDrop(object objectToDrop)
        {
            return !(objectToDrop is IValue);
        }

        /// <summary>
        /// <see cref="IElement.AttachItem" />
        /// </summary>
        public override void AttachItem(object objectToAttach)
        {
            try
            {
                _ifCondition = (ICondition)objectToAttach;
            }
            catch (Exception)
            {
                if (_ifInstruction != null)
                {
                    _elseInstruction = (IInstruction)objectToAttach;
                }
                else
                {
                    _ifInstruction = (IInstruction)objectToAttach;
                }
            }
        }

        /// <summary>
        /// <see cref="IElement.RemoveItem" />
        /// </summary>
        public override void RemoveItem(object objectToRemove)
        {
            if (typeof(ICondition) == objectToRemove.GetType())
            {
                _ifCondition = null;
            } 
            else
            {
                if (_ifInstruction != null && _ifInstruction.Equals(objectToRemove))
                {
                    _ifInstruction = null;
                }
                else
                {
                    _elseInstruction = null;
                }
            }
        }

        /// <summary>
        /// <see cref="IElement.Move" />
        /// </summary>
        public override void Move(int index, bool positive, IElement from, IElement to)
        {
            if (from == _ifInstruction && to == _elseInstruction)
            {
                _ifInstruction = (IInstruction)to;
                _elseInstruction = (IInstruction)from;
                return;
            }
            else if (from == _elseInstruction && to == _ifInstruction)
            {
                _ifInstruction = (IInstruction)from;
                _elseInstruction = (IInstruction)to;
            }
        }

        #endregion
    }
}