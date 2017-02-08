namespace DynamicRuling.Instruction
{
    /// <summary>
    /// instruction
    /// </summary>
    public interface IInstruction : IElement
    {
        #region Public Methods
        
        /// <summary>
        /// execute this instruction
        /// </summary>
        void Execute();

        #endregion
    }
}