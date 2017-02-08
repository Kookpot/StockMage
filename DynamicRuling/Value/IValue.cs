namespace DynamicRuling.Value
{
    /// <summary>
    /// interface of a value
    /// a value returns a string or an int when asked
    /// </summary>
    public interface IValue : IElement
    {
        #region Public Methods

        /// <summary>
        /// get a string representation of the value
        /// </summary>
        /// <returns>the values in string for this object</returns>
        string GetStringValue();

        /// <summary>
        /// get an integer of the value
        /// </summary>
        /// <returns>the value as integer for this object</returns>
        int GetIntValue();

        #endregion
    }
}