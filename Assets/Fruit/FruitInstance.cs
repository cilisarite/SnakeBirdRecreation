using System;
using Snakebird.Tile;
using UnityEngine;

namespace Snakebird.InstanceTile
{
    public class FruitInstance : InstanceTileBase, IContactHandler, ISaveable<FruitSaveData>
    {
        #region Serialized.
        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] Sprite _sprite;
        [SerializeField] BoxCollider2D _boxCollider;
        #endregion

        #region Structures.
        #endregion

        #region Public.
        public bool IsEaten => _isEaten;
        #endregion

        #region Private.
        private bool _isEaten;
        #endregion

        #region Events.
        #endregion

        #region Listeners.
        protected override void Awake()
        {
            base.Awake();
        }
        void Start()
        {
            _gameBoard.AddFruitInstance(this);
        }
        #endregion

        #region Public Methods.
        public void OnContact(SnakebirdInstance eventPlayer, Vector3Int direction)
        {
            if (_isEaten == false)
            {
                eventPlayer.AddSegment();
                _spriteRenderer.sprite = null;
                _boxCollider.enabled = false;
                _isEaten = true;
            }
        }
        public FruitSaveData Save()
        {
            FruitSaveData fruitSaveData = new FruitSaveData();
            fruitSaveData.gridPosition = _gameBoard.Tilemap.layoutGrid.WorldToCell(transform.position);
            fruitSaveData.isEaten = _isEaten;
            return fruitSaveData;
        }
        public void Load(FruitSaveData saveData)
        {
            transform.position = _gameBoard.Tilemap.layoutGrid.CellToWorld(saveData.gridPosition);
            _isEaten = saveData.isEaten;
            if (_isEaten == false)
            {
                _spriteRenderer.sprite = _sprite;
                _boxCollider.enabled = true;
            }
        }
        #endregion

        #region Private Methods.
        #endregion
    }

    public struct FruitSaveData
    {
        public Vector3Int gridPosition;
        public bool isEaten;
    }
}