using System.Runtime.Serialization;
using System.Windows.Forms;

namespace DynamicRuling
{
    /// <summary>
    /// checkable
    /// </summary>
    public abstract class Element : IElement
    {
        #region Public Methods

        /// <summary>
        /// <see cref="IElement.AllowDrop" />
        /// </summary>
        public virtual bool AllowDrop(object objectToDrop)
        {
            return false;
        }

        /// <summary>
        /// <see cref="IElement.AttachItem" />
        /// </summary>
        public virtual void AttachItem(object objectToAttach)
        { }

        /// <summary>
        /// <see cref="IElement.Check" />
        /// </summary>
        public virtual bool Check(bool deep)
        {
            return true;
        }

        /// <summary>
        /// <see cref="IElement.CheckToString" />
        /// </summary>
        public virtual string CheckToString(bool deep)
        {
            return string.Empty;
        }

        /// <summary>
        /// <see cref="IElement.Clone" />
        /// </summary>
        public virtual object Clone()
        {
            return null;
        }

        /// <summary>
        /// <see cref="IElement.GetNode" />
        /// </summary>
        public virtual TreeNode GetNode(string prefix)
        {
            return null;
        }

        /// <summary>
        /// <see cref="IElement.GetObjectData" />
        /// </summary>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        { }

        /// <summary>
        /// <see cref="IElement.GetPrefix" />
        /// </summary>
        public virtual string GetPrefix(IElement checkable)
        {
            return string.Empty;
        }

        /// <summary>
        /// <see cref="IElement.Move" />
        /// </summary>
        public virtual void Move(int index, bool positive, IElement from, IElement to)
        { }

        /// <summary>
        /// <see cref="IElement.RemoveItem" />
        /// </summary>
        public virtual void RemoveItem(object objectToRemove)
        { }

        /// <summary>
        /// <see cref="IElement.Showable" />
        /// </summary>
        public virtual bool Showable()
        {
            return false;
        }

        #endregion
    }
}