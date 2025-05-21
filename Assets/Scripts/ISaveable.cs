using UnityEngine;

namespace Snakebird
{
    public interface ISaveable<T>
    {
        #region Serialized.
        #endregion

        #region Structures.
        #endregion

        #region Public.
        #endregion

        #region Private.
        #endregion

        #region Events.
        #endregion

        #region Listeners.
        #endregion

        #region Public Methods.
        public T Save();
        public void Load(T saveData);
        #endregion

        #region Private Methods.
        #endregion
    }
}
