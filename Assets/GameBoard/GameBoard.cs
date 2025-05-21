using System.Collections.Generic;
using Snakebird.InstanceTile;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snakebird.Tile
{
    public partial class GameBoard : MonoBehaviour
    {
        #region Serialized.
        [SerializeField] Vector2 _sizeBounds;
        [SerializeField] Tilemap _tilemap;
        #endregion

        #region Structures.
        #endregion

        #region Public.
        public Tilemap Tilemap => _tilemap;
        public List<SnakebirdInstance> SnakeBirds => _snakeBirds;
        public List<FruitInstance> Fruits => _fruits;
        public Dictionary<Vector3Int, InstanceTileBase> InstanceTilesByGridPosition => _instanceTilesByGridPosition;
        #endregion

        #region Private.
        private Vector3Int _origin;
        private List<SnakebirdInstance> _snakeBirds;
        private List<FruitInstance> _fruits;
        private Dictionary<Vector3Int, InstanceTileBase> _instanceTilesByGridPosition;
        #endregion

        #region Events.
        #endregion

        #region Listeners.
        void Awake()
        {
            _origin = _tilemap.origin;
            _snakeBirds = new List<SnakebirdInstance>();
            _fruits = new List<FruitInstance>();
            _instanceTilesByGridPosition = new Dictionary<Vector3Int, InstanceTileBase>();

            _boardSaveStates = new List<BoardSaveData>();
        }
        #endregion

        #region Public Methods.
        public bool IsInBounds(Vector3Int gridPosition)
        {
            return (_origin.x <= gridPosition.x && gridPosition.x <= _origin.x + _sizeBounds.x - 1) && (_origin.y <= gridPosition.y && gridPosition.y <= _origin.y + _sizeBounds.y - 1);
        }
        public void AddSnakebirdInstance(SnakebirdInstance snakebirdInstance)
        {
            _snakeBirds.Add(snakebirdInstance);
            foreach (SnakebirdSegment snakebirdSegment in snakebirdInstance.SnakebirdSegments)
            {
                _instanceTilesByGridPosition.Add(_tilemap.layoutGrid.WorldToCell(snakebirdSegment.transform.position), snakebirdInstance);
            }
        }
        public void AddFruitInstance(FruitInstance fruitInstance)
        {
            _fruits.Add(fruitInstance);
            _instanceTilesByGridPosition.Add(_tilemap.layoutGrid.WorldToCell(fruitInstance.transform.position), fruitInstance);
        }
        public void AddInstanceTileBase(InstanceTileBase instanceTileBase)
        {
            _instanceTilesByGridPosition.Add(_tilemap.layoutGrid.WorldToCell(instanceTileBase.transform.position), instanceTileBase);
        }
        #endregion

        #region Private Methods.
        #endregion
    }
}
