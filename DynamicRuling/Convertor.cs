using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using DynamicRuling.Instruction;
using System.Threading;

namespace DynamicRuling
{
    /// <summary>
    /// singleton convertor
    /// contains the current configuration (set of instructions and temporary values
    /// </summary>
    [Serializable]
    public class Converter
    {
        #region Members

        //the currently set convertor
        private static Converter _convertor;
        //instructions
        private readonly List<IInstruction> _instructions;
        //temporary values
        private Dictionary<string, string> _tempValues;
        //ended?
        private bool _ended;

        public Dictionary<string, string> TempValues
        {
            get { return _tempValues; }
            set { _tempValues = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// private constructor
        /// </summary>
        private Converter()
        {
            _instructions = new List<IInstruction>();
            _tempValues = new Dictionary<string, string>();
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">context stream</param>
        public Converter(SerializationInfo info, StreamingContext context) : this()
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// clear the current configuration
        /// </summary>
        public static void Clear()
        {
            _convertor = null;
        }

        /// <summary>
        /// execute the current configuration with no extra parameters
        /// </summary>
        public void Execute()
        {
            _ended = false;
            _tempValues = new Dictionary<string, string>();
            while (!_ended)
            {
                var objNew = new Thread(new ThreadStart(CalcRules));
                objNew.Start(); 
                Thread.Sleep(20000);
            }
        }

        /// <summary>
        /// calc rules
        /// </summary>
        public void CalcRules()
        {
            foreach (var instruction in _instructions)
            {
                instruction.Execute();
            } 
        }

        /// <summary>
        /// end
        /// </summary>
        public void End()
        {
            _ended = true;
        }

        /// <summary>
        /// get instance (current configuration)
        /// create a new one if the current is empty
        /// </summary>
        /// <returns>current configuration</returns>
        public static Converter GetInstance()
        {
            return _convertor ?? (_convertor = new Converter());
        }

        /// <summary>
        /// set the current configuration to a given configuration
        /// </summary>
        /// <param name="converter">the configuration to be set</param>
        public static void SetInstance(Converter converter)
        {
            _convertor = converter;
        }

        /// <summary>
        /// move instruction in the internal list of instructions (if possible)
        /// </summary>
        /// <param name="index">index of the item to move</param>
        /// <param name="positive">direction of change, positive is further down the list, negative is up in the list</param>
        /// <param name="from">item that will be moved</param>
        /// <param name="to">item where the instruction will be moved to</param>
        public void Move(int index, bool positive, IElement from, IElement to)
        {
            var instruction = _instructions[index];
            _instructions.RemoveAt(index);
            if (positive)
            {
                _instructions.Insert(index + 1, instruction);
            }
            else
            {
                _instructions.Insert(index - 1, instruction);
            }
        }

        /// <summary>
        /// add instruction to the internal list of instructions
        /// </summary>
        /// <param name="instruction">instruction to be added</param>
        public void AddInstruction(IInstruction instruction)
        {
            _instructions.Add(instruction);
        }

        /// <summary>
        /// remove instruction from the internal list of instructions
        /// </summary>
        /// <param name="instruction">instruction to be deleted</param>
        public void RemoveInstruction(IInstruction instruction)
        {
            _instructions.Remove(instruction);
        }

        /// <summary>
        /// get object data
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">streaming context</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        { }

        /// <summary>
        /// get instructions as treenodes
        /// </summary>
        /// <returns>list of treenodes that are the represenations of the list of instructions of this configuration</returns>
        public List<TreeNode> GetInstructionNodes()
        {
            return _instructions.Select(intstruction => intstruction.GetNode(string.Empty)).ToList();
        }

        #endregion
    }
}