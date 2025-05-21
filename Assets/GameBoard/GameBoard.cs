using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snakebird.Tile
{
    public class GameBoard : MonoBehaviour
    {
        #region Serialized.
        [SerializeField] Vector2 _sizeBounds;
        [SerializeField] Tilemap _tilemap;
        #endregion

        #region Structures.
        #endregion

        #region Public.
        public Tilemap Tilemap => _tilemap;
        #endregion

        #region Private.
        private Vector3Int _origin;
        #endregion

        #region Events.
        #endregion

        #region Listeners.
        void Awake()
        {
            _origin = _tilemap.origin;
        }
        #endregion

        #region Public Methods.
        public bool IsInBounds(Vector3Int gridPosition)
        {
            return (_origin.x <= gridPosition.x && gridPosition.x <= _origin.x + _sizeBounds.x - 1) && (_origin.y <= gridPosition.y && gridPosition.y <= _origin.y + _sizeBounds.y - 1);
        }
        #endregion

        #region Private Methods.
        #endregion
    }
}
