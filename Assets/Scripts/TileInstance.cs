using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snakebird.Tile
{
    public class TileInstance : MonoBehaviour
    {
        #region Serialized.
        [SerializeField] TileBase _tile;
        [SerializeField] GameBoard _gameBoard;
        #endregion

        #region Structures.
        #endregion

        #region Public.
        public Vector3Int GridPosition => _gridPosition;
        #endregion

        #region Private.
        protected Vector3Int _gridPosition;
        #endregion

        #region Events.
        #endregion

        #region Listeners.
        void Start()
        {
            if (_gameBoard == null)
                _gameBoard = transform.parent.gameObject.GetComponent<GameBoard>();
            _gridPosition = _gameBoard.Tilemap.layoutGrid.WorldToCell(transform.position);
            _gameBoard.Tilemap.SetTile(_gridPosition, _tile);
        }
        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_gameBoard.Tilemap.layoutGrid.GetCellCenterWorld(_gameBoard.Tilemap.layoutGrid.WorldToCell(transform.position)), _gameBoard.Tilemap.cellSize.x / 2);
        }
        #endregion

        #region Public Methods.
        public virtual void Move(Vector3Int direction)
        {
            if (_gameBoard.Tilemap.HasTile(_gridPosition + direction) || _gameBoard.IsInBounds(_gridPosition + direction) == false)
                return;
            _gameBoard.Tilemap.SetTile(_gridPosition, null);

            _gridPosition += direction;
            transform.position = _gameBoard.Tilemap.layoutGrid.CellToWorld(_gridPosition);

            _gameBoard.Tilemap.SetTile(_gridPosition, _tile);
        }
        #endregion

        #region Private Methods.
        #endregion
    }
}
