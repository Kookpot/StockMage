using System;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace DynamicRuling
{
    /// <summary>
    /// Generic Item which can be dragged and can be checked and such
    /// </summary>
    public interface IElement : ICloneable, ISerializable
    {
        #region Public Methods

        /// <summary>
        /// is this item showable in standard mode?
        /// </summary>
        /// <returns>true if showable, false otherwise</returns>
        bool Showable();

        /// <summary>
        /// check if the configuration of this node/item is faulty
        /// </summary>
        /// <param name="deep">perform deep check?</param>
        /// <returns>all is well?</returns>
        bool Check(bool deep);

        /// <summary>
        /// retrieve the reason why the configuration of this node/item is faulty if any
        /// </summary>
        /// <param name="deep">parameter to check the children as well or not</param>
        /// <returns>in string format the checked values</returns>
        string CheckToString(bool deep);

        /// <summary>
        /// allowed to attach the given dragged item to the current object?
        /// </summary>
        /// <param name="objectToDrop">item to check (item which is dragged)</param>
        /// <returns>true if can be dropped, false otherwise</returns>
        bool AllowDrop(object objectToDrop);

        /// <summary>
        /// attach (sub-)item to node/object
        /// </summary>
        /// <param name="objectToAttach">object to add</param>
        void AttachItem(object objectToAttach);

        /// <summary>
        /// remove (sub-)item from node/object
        /// </summary>
        /// <param name="objectToRemove">object to remove</param>
        void RemoveItem(object objectToRemove);

        /// <summary>
        /// get node by using a prefix to the text of the node
        /// </summary>
        /// <param name="prefix">prefix to use on the text of the node</param>
        /// <returns></returns>
        TreeNode GetNode(string prefix);

        /// <summary>
        /// move node if possible
        /// we do this by using an index and wether we change the index positive or negative
        /// or we do this by giving the two cheavkable objects to switch
        /// </summary>
        /// <param name="index">index of node to move</param>
        /// <param name="positive">direction to where we change the node (up or down)</param>
        /// <param name="from">object to move</param>
        /// <param name="to">object to where we move to and thereby switch positions (this is normally an adjacent sibling)</param>
        void Move(int index, bool positive, IElement from, IElement to);

        /// <summary>
        /// get prefix if any
        /// the prefix is often depending on the child which uses the prefix
        /// hence we add it as a parameter
        /// </summary>
        /// <param name="checkable">the child to get the prefix for</param>
        /// <returns>prefix</returns>
        string GetPrefix(IElement checkable);

        #endregion Public Methods
    }
}