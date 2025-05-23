using Snakebird.InstanceTile;
using Snakebird.Tile;
using UnityEngine;

namespace Snakebird
{
    public class FinishPortalInstance : InstanceTileBase, IContactHandler
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
        void Start()
        {
            _gameBoard.AddFinishPortalInstance(this);
        }
        #endregion

        #region Public Methods.
        public void OnContact(SnakebirdInstance snakebirdInstance, Vector3Int direction)
        {
            if (_gameBoard.IsAllFruitEaten())
            {
                snakebirdInstance.Finish();
            }
        }
        #endregion

        #region Private Methods.
        #endregion
    }
}