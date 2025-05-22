using Snakebird.InstanceTile;
using UnityEngine;

namespace Snakebird.InstanceTile
{
    public class SpikeInstance : InstanceTileBase
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
            _gameBoard.AddSpikeInstance(this);
        }
        #endregion

        #region Public Methods.
        #endregion

        #region Private Methods.
        #endregion
    }
}