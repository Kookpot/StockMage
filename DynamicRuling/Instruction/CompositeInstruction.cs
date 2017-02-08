using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace DynamicRuling.Instruction
{
    /// <summary>
    /// composite instruction by design pattern
    /// responsible for grouping instructions
    /// executes it's children on execute
    /// </summary>
    [Serializable]
    public class CompositeInstruction : Element, IInstruction
    {
        #region Members

        //list of sub-instructions
        private readonly List<IInstruction> _instructions;
        //name of the group
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor
        /// </summary>
        public CompositeInstruction()
        {
            _instructions = new List<IInstruction>();
            _name = string.Empty;
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        public CompositeInstruction(SerializationInfo info, StreamingContext context)
        {
            _instructions = (List<IInstruction>)info.GetValue("Composite", typeof(List<IInstruction>));
            try
            {
                _name = (string)info.GetValue("Name", typeof(string));
// ReSharper disable EmptyGeneralCatchClause
            } catch(Exception)
// ReSharper restore EmptyGeneralCatchClause
            {
            }
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
        /// <see cref="IInstruction.Execute" />
        /// </summary>
        public void Execute()
        {
            foreach (var instruction in _instructions)
            {
                instruction.Execute();
            }
        }

        /// <summary>
        /// add instruction
        /// </summary>
        /// <param name="instruction">instruction to add</param>
        public void AddInstruction(IInstruction instruction)
        {
            _instructions.Add(instruction);
        }

        /// <summary>
        /// remove instruction from the internal list of instructions
        /// </summary>
        /// <param name="instruction">instruction to remove</param>
        public void RemoveInstruction(IInstruction instruction)
        {
            _instructions.Remove(instruction);
        }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public override bool Check(bool deep)
        {
            return !deep || _instructions.All(objInstruction => objInstruction.Check(true));
        }

        /// <summary>
        /// <see cref="IElement.CheckToString" />
        /// </summary>
        public override string CheckToString(bool blnDeep)
        {
            var returnVal = string.Empty;
            if (blnDeep)
            {
                returnVal = _instructions.Where(objInstruction => !objInstruction.Check(true))
                    .Aggregate(returnVal, (current, objInstruction) => current + (Environment.NewLine + "An underlying instuction is incorrect!"));
            }
            return returnVal;
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public override object Clone()
        {
            var composite = new CompositeInstruction();
            foreach (var instruction in _instructions)
            {
                composite.AddInstruction((IInstruction)instruction.Clone());
            }
            return composite;
        }

        /// <summary>
        /// <see cref="object.ToString" />
        /// </summary>
        public override string ToString()
        {
            return string.Format("(Count : {0})", _instructions.Count);
        }

        /// <summary>
        /// <see cref="IElement.GetObjectData" />
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Composite", _instructions);
            info.AddValue("Name", _name);
        }

        /// <summary>
        /// <see cref="IElement.GetNode" />
        /// </summary>
        public override TreeNode GetNode(string prefix)
        {
            TreeNode node;
            if (_name == null || _name.Equals(string.Empty))
            {
                node = new TreeNode(prefix + "Group with no name " + ToString());
            }
            else
            {
                node = new TreeNode(prefix + "Group " + _name + "  " + ToString());
            }
            node.Tag = this;
            node.Name = "CompositeInstructionNode";
            foreach (var instruction in _instructions)
            {
                node.Nodes.Add(instruction.GetNode(string.Empty));
            }
            return node;
        }

        /// <summary>
        /// <see cref="IElement.AllowDrop" />
        /// </summary>
        public override bool AllowDrop(object objectToDrop)
        {
            return objectToDrop is IInstruction;
        }

        /// <summary>
        /// <see cref="IElement.AttachItem" />
        /// </summary>
        public override void AttachItem(object objectToAttach)
        {
            AddInstruction((IInstruction)objectToAttach);
        }

        /// <summary>
        /// <see cref="IElement.RemoveItem" />
        /// </summary>
        public override void RemoveItem(object objectToRemove)
        {
            RemoveInstruction((IInstruction)objectToRemove);
        }

        /// <summary>
        /// <see cref="IElement.Move" />
        /// </summary>
        public override void Move(int index, bool positive, IElement from, IElement to)
        {
            var objectToMove = _instructions[index];
            _instructions.RemoveAt(index);
            if (positive)
            {
                _instructions.Insert(index + 1, objectToMove);
            }
            else
            {
                _instructions.Insert(index - 1, objectToMove);
            }
        }

        #endregion
    }
}