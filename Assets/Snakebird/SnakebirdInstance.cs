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
        #endregion

        #region Events.
        public event Action OnMove;
        public event Action OnLoad;
        #endregion

        #region Listeners.
        protected override void Awake()
        {
            base.Awake();
        }
        void Start()
        {
            _snakebirdSegments = new List<SnakebirdSegment>();
            _gameBoard.AddSnakebirdInstance(this);
            GetAllSegments();
        }
        #endregion

        #region Public Methods.
        public void Move(Vector3Int direction)
        {
            Vector3Int nextGridPosition = _gameBoard.Tilemap.layoutGrid.WorldToCell(_snakebirdSegments[0].transform.position) + direction;

            if (_gameBoard.IsInBounds(nextGridPosition) == false || _gameBoard.Tilemap.GetTile(nextGridPosition) != null)
                return;

            if (_gameBoard.InstanceTilesByGridPosition.ContainsKey(nextGridPosition))
            {
                if (_gameBoard.InstanceTilesByGridPosition[nextGridPosition] is IContactHandler)
                {
                    IContactHandler contactHandler = _gameBoard.InstanceTilesByGridPosition[nextGridPosition] as IContactHandler;
                    contactHandler.OnContact(this);
                }
            }

            Vector3Int currentGridPosition;
            for (int i = 0; i < _snakebirdSegments.Count; i++)
            {
                Transform currentSegment = _snakebirdSegments[i].transform;
                currentGridPosition = _gameBoard.Tilemap.layoutGrid.WorldToCell(currentSegment.position);
                currentSegment.position = _gameBoard.Tilemap.layoutGrid.CellToWorld(nextGridPosition);
                nextGridPosition = currentGridPosition;
            }
            OnMove?.Invoke();
        }
        public void Sleep()
        {

        }
        public void Wake()
        {

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
        #endregion
    }
    public struct SnakebirdSaveData
    {
        public Vector3Int[] segmentsGridPosition;
    }
}