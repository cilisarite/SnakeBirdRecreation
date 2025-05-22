using System;
using System.Collections.Generic;
using Snakebird.InstanceTile;
using Snakebird.Tile;
using UnityEngine;

namespace Snakebird
{
    public class SnakebirdInstance : InstanceTileBase, ISaveable<SnakebirdSaveData>
    {
        #region Serialized.
        [SerializeField] SnakebirdSegment _segmentPrefab;
        #endregion

        #region Structures.
        #endregion

        #region Public.
        public List<SnakebirdSegment> SnakebirdSegments => _snakebirdSegments;
        #endregion

        #region Private.
        private List<SnakebirdSegment> _snakebirdSegments;
        private bool _isDead;
        #endregion

        #region Events.
        public event Action OnMove;
        public event Action OnLoad;
        #endregion

        #region Listeners.
        protected override void Awake()
        {
            base.Awake();
            _snakebirdSegments = new List<SnakebirdSegment>();
            GetAllSegments();
        }
        void Start()
        {
            _gameBoard.AddSnakebirdInstance(this);
        }
        #endregion

        #region Public Methods.
        public void Move(Vector3Int direction)
        {
            Vector3Int nextGridPosition = _gameBoard.Tilemap.layoutGrid.WorldToCell(_snakebirdSegments[0].transform.position) + direction;
            InstanceTileBase instanceTileAtNextPosition = _gameBoard.GetInstanceTile(nextGridPosition);

            if (_gameBoard.IsInBounds(nextGridPosition) == false || _gameBoard.Tilemap.GetTile(nextGridPosition) != null || instanceTileAtNextPosition == this)
                return;

            _gameBoard.SaveState();

            if (instanceTileAtNextPosition is IContactHandler)
            {
                IContactHandler contactHandler = instanceTileAtNextPosition as IContactHandler;
                contactHandler.OnContact(this);
            }

            Vector3Int currentGridPosition;
            for (int i = 0; i < _snakebirdSegments.Count; i++)
            {
                Transform currentSegment = _snakebirdSegments[i].transform;
                currentGridPosition = _gameBoard.Tilemap.layoutGrid.WorldToCell(currentSegment.position);
                currentSegment.position = _gameBoard.Tilemap.layoutGrid.CellToWorld(nextGridPosition);
                nextGridPosition = currentGridPosition;
            }

            while (CanShiftInDirection(Vector3Int.down))
            {
                if (CheckForSpike())
                {
                    Die();
                    break;
                }
                else
                {
                    Shift(Vector3Int.down);
                }
            }
            OnMove?.Invoke();
        }
        public void Sleep()
        {

        }
        public void Wake()
        {

        }
        public void Die()
        {
            _isDead = true;
            foreach (SnakebirdSegment snakebirdSegment in SnakebirdSegments)
            {
                snakebirdSegment.gameObject.SetActive(false);
            }
        }
        public void AddSegment()
        {
            SnakebirdSegment segment = Instantiate(_segmentPrefab, transform);
            _snakebirdSegments.Add(segment);
        }
        public void AddSegment(int segmentCount)
        {
            for (int i = 0; i < segmentCount; i++)
            {
                SnakebirdSegment segment = Instantiate(_segmentPrefab, transform);
                _snakebirdSegments.Add(segment);
            }
        }
        public SnakebirdSaveData Save()
        {
            SnakebirdSaveData snakebirdSaveData = new SnakebirdSaveData();
            snakebirdSaveData.segmentsGridPosition = new Vector3Int[_snakebirdSegments.Count];
            for (int i = 0; i < snakebirdSaveData.segmentsGridPosition.Length; i++)
            {
                snakebirdSaveData.segmentsGridPosition[i] = _gameBoard.Tilemap.layoutGrid.WorldToCell(_snakebirdSegments[i].transform.position);
            }
            snakebirdSaveData.isDead = _isDead;
            return snakebirdSaveData;
        }
        public void Load(SnakebirdSaveData saveData)
        {
            _snakebirdSegments = new List<SnakebirdSegment>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            AddSegment(saveData.segmentsGridPosition.Length);
            for (int i = 0; i < saveData.segmentsGridPosition.Length; i++)
            {
                _snakebirdSegments[i].transform.position = saveData.segmentsGridPosition[i];
            }

            _isDead = saveData.isDead;
            if (_isDead == true)
            {
                foreach (SnakebirdSegment snakebirdSegment in SnakebirdSegments)
                {
                    snakebirdSegment.gameObject.SetActive(false);
                }
            }
            OnLoad?.Invoke();
        }
        #endregion

        #region Private Methods.
        private void GetAllSegments()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent<SnakebirdSegment>(out SnakebirdSegment snakebirdSegment))
                {
                    _snakebirdSegments.Add(snakebirdSegment);
                }
            }
        }
        private bool CanShiftInDirection(Vector3Int direction)
        {
            foreach (SnakebirdSegment snakebirdSegment in _snakebirdSegments)
            {
                Vector3Int nextGridPosition = _gameBoard.Tilemap.layoutGrid.WorldToCell(snakebirdSegment.transform.position) + direction;
                InstanceTileBase instanceTileAtNextPosition = _gameBoard.GetInstanceTile(nextGridPosition);

                if (direction.y < 0)
                {
                    if (_gameBoard.IsInBounds(nextGridPosition) == false || _gameBoard.Tilemap.GetTile(nextGridPosition) != null || (instanceTileAtNextPosition != null && instanceTileAtNextPosition is not SpikeInstance && instanceTileAtNextPosition != this))
                        return false;
                }
                else
                {
                    if (_gameBoard.IsInBounds(nextGridPosition) == false || _gameBoard.Tilemap.GetTile(nextGridPosition) != null || (instanceTileAtNextPosition != null && instanceTileAtNextPosition != this))
                        return false;
                }
            }
            return true;
        }
        private bool CheckForSpike()
        {
            foreach (SnakebirdSegment snakebirdSegment in _snakebirdSegments)
            {
                Vector3Int nextGridPosition = _gameBoard.Tilemap.layoutGrid.WorldToCell(snakebirdSegment.transform.position) + Vector3Int.down;
                InstanceTileBase instanceTileAtNextPosition = _gameBoard.GetInstanceTile(nextGridPosition);

                if (instanceTileAtNextPosition != null)
                {
                    if (instanceTileAtNextPosition is SpikeInstance)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void Shift(Vector3Int direction)
        {
            for (int i = 0; i < _snakebirdSegments.Count; i++)
            {
                Transform currentSegment = _snakebirdSegments[i].transform;
                currentSegment.position = _gameBoard.Tilemap.layoutGrid.CellToWorld(_gameBoard.Tilemap.layoutGrid.WorldToCell(currentSegment.position) + direction);
            }
        }
        #endregion
    }
    public struct SnakebirdSaveData
    {
        public Vector3Int[] segmentsGridPosition;
        public bool isDead;
    }
}