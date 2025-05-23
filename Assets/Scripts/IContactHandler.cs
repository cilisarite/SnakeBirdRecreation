using UnityEngine;

namespace Snakebird.Tile
{
    public interface IContactHandler
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
        public void OnContact(SnakebirdInstance eventPlayer, Vector3Int direction);
        #endregion

        #region Private Methods.
        #endregion
    }
}
