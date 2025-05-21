using Snakebird.Tile;
using UnityEngine;

namespace Snakebird.InstanceTile
{
    public class InstanceTileBase : MonoBehaviour
    {
        #region Serialized.
        [SerializeField] protected GameBoard _gameBoard;
        #endregion

        #region Structures.
        #endregion

        #region Public.
        public GameBoard GameBoard => _gameBoard;
        #endregion

        #region Private.
        #endregion

        #region Events.
        #endregion

        #region Listeners.
        protected virtual void Awake()
        {
            if (_gameBoard == null)
                _gameBoard = transform.parent.gameObject.GetComponent<GameBoard>();
        }
        #endregion

        #region Public Methods.
        #endregion

        #region Private Methods.
        #endregion
    }
}