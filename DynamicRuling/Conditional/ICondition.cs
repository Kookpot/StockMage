namespace DynamicRuling.Conditional
{
    /// <summary>
    /// Interface on Condition
    /// </summary>
    public interface ICondition : IElement
    {
        #region Public Methods

        /// <summary>
        /// evaluate condition
        /// </summary>
        /// <returns>true if condition evaluates positive, false otherwise</returns>
        bool Evaluate();

        /// <summary>
        /// evaluate condition in text
        /// </summary>
        /// <returns>string representation of evalutation</returns>
        string EvaluateToString();

        #endregion
    }
}