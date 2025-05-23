using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Snakebird.InstanceTile;
using Snakebird.Tile;
using Unity.VisualScripting;
using UnityEngine;

namespace Snakebird
{
    public class SnakebirdInstance : InstanceTileBase, ISaveable<SnakebirdSaveData>, IContactHandler
    {
        #region Serialized.
        [SerializeField] SnakebirdSegment _segmentPrefab;
        #endregion

        #region Structures.
        #endregion

        #region Public.
        public List<SnakebirdSegment> SnakebirdSegments => _snakebirdSegments;
        public bool IsDead => _isDead;
        public bool IsFinished => _isFinished;
        #endregion

        #region Private.
        private List<SnakebirdSegment> _snakebirdSegments;
        private bool _isDead;
        private bool _isFinished;
        #endregion

        #region Events.
        public event Action OnMove;
        public static Action OnMoveUpdate;
        public event Action OnLoad;
        public event Action OnFinish;
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
            OnMoveUpdate += MoveUpdate;
        }
        void OnDestroy()
        {
            _gameBoard.RemoveSnakebirdInstance(this);
            OnMoveUpdate -= MoveUpdate;
        }
        void MoveUpdate()
        {
            Fall();
        }
        public void OnContact(SnakebirdInstance snakebirdInstance, Vector3Int direction)
        {
            if (CanPushInDirection(direction))
            {
                Shift(direction);
            }
        }
        #endregion

        #region Public Methods.
        public void Move(Vector3Int direction)
        {
            Vector3Int nextGridPosition = _gameBoard.Tilemap.layoutGrid.WorldToCell(_snakebirdSegments[0].transform.position) + direction;
            InstanceTileBase instanceTileAtNextPosition = _gameBoard.GetInstanceTile(nextGridPosition);

            if (_gameBoard.IsInBounds(nextGridPosition) == false || _gameBoard.Tilemap.GetTile(nextGridPosition) != null || instanceTileAtNextPosition == this || instanceTileAtNextPosition is SpikeInstance)
                return;

            _gameBoard.SaveState();

            if (instanceTileAtNextPosition is IContactHandler)
            {
                IContactHandler contactHandler = instanceTileAtNextPosition as IContactHandler;
                contactHandler.OnContact(this, direction);
                instanceTileAtNextPosition = _gameBoard.GetInstanceTile(nextGridPosition);
                if (instanceTileAtNextPosition != null && instanceTileAtNextPosition is not FinishPortalInstance)
                    return;
            }

            Vector3Int currentGridPosition;
            for (int i = 0; i < _snakebirdSegments.Count; i++)
            {
                Transform currentSegment = _snakebirdSegments[i].transform;
                currentGridPosition = _gameBoard.Tilemap.layoutGrid.WorldToCell(currentSegment.position);
                currentSegment.position = _gameBoard.Tilemap.layoutGrid.CellToWorld(nextGridPosition);
                nextGridPosition = currentGridPosition;
            }

            Fall();
            OnMove?.Invoke();
            OnMoveUpdate?.Invoke();
        }
        public void Sleep()
        {

        }
        public void Wake()
        {

        }
        public void Fall()
        {
            while (CanFall(out List<SnakebirdInstance> snakebirdsToShift))
            {
                if (CheckForSpike())
                {
                    Die();
                    break;
                }
                else
                {
                    Shift(Vector3Int.down, snakebirdsToShift);
                }
            }
        }
        public void Die()
        {
            _isDead = true;
            foreach (SnakebirdSegment snakebirdSegment in SnakebirdSegments)
            {
                snakebirdSegment.gameObject.SetActive(false);
            }
        }
        public void Finish()
        {
            Die();
            _isFinished = true;
            OnFinish?.Invoke();
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
            snakebirdSaveData.isFinished = _isFinished;
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
            _isFinished = saveData.isFinished;
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
        protected bool CanFall(out List<SnakebirdInstance> snakebirdsToShift)
        {
            Vector3Int direction = Vector3Int.down;
            List<SnakebirdInstance> snakebirdsContacted = FindAllContactedSnakebirdsRecursive(Vector3Int.down);

            snakebirdsToShift = new List<SnakebirdInstance>();
            foreach (SnakebirdInstance snakebirdInstance in snakebirdsContacted)
            {
                foreach (SnakebirdSegment snakebirdSegment in snakebirdInstance.SnakebirdSegments)
                {
                    Vector3Int nextGridPosition = _gameBoard.Tilemap.layoutGrid.WorldToCell(snakebirdSegment.transform.position) + direction;
                    InstanceTileBase instanceTileAtNextPosition = _gameBoard.GetInstanceTile(nextGridPosition);

                    if (_gameBoard.IsInBounds(nextGridPosition) == false || _gameBoard.Tilemap.GetTile(nextGridPosition) != null || (instanceTileAtNextPosition != null && instanceTileAtNextPosition is not SpikeInstance && instanceTileAtNextPosition is not SnakebirdInstance && instanceTileAtNextPosition is not FinishPortalInstance))
                        return false;
                }
            }

            snakebirdsToShift.AddRange(snakebirdsContacted);
            return true;
        }
        private List<SnakebirdInstance> FindAllContactedSnakebirdsRecursive(SnakebirdInstance snakebirdInstance, List<SnakebirdInstance> snakebirdsContacted, Vector3Int direction)
        {
            foreach (SnakebirdSegment snakebirdSegment in snakebirdInstance.SnakebirdSegments)
            {
                Vector3Int nextGridPosition = _gameBoard.Tilemap.layoutGrid.WorldToCell(snakebirdSegment.transform.position) + direction;
                InstanceTileBase instanceTileAtNextPosition = _gameBoard.GetInstanceTile(nextGridPosition);

                if (instanceTileAtNextPosition is SnakebirdInstance)
                {
                    if (snakebirdsContacted.Contains((SnakebirdInstance)instanceTileAtNextPosition) == false)
                    {
                        snakebirdsContacted.Add((SnakebirdInstance)instanceTileAtNextPosition);
                        snakebirdsContacted = FindAllContactedSnakebirdsRecursive((SnakebirdInstance)instanceTileAtNextPosition, snakebirdsContacted, direction);
                    }
                }
            }
            return snakebirdsContacted;
        }
        private List<SnakebirdInstance> FindAllContactedSnakebirdsRecursive(Vector3Int direction)
        {
            return FindAllContactedSnakebirdsRecursive(this, new List<SnakebirdInstance> { this }, direction);
        }
        private bool CanPushInDirection(Vector3Int direction)
        {
            foreach (SnakebirdSegment snakebirdSegment in _snakebirdSegments)
            {
                Vector3Int nextGridPosition = _gameBoard.Tilemap.layoutGrid.WorldToCell(snakebirdSegment.transform.position) + direction;
                InstanceTileBase instanceTileAtNextPosition = _gameBoard.GetInstanceTile(nextGridPosition);

                if (_gameBoard.IsInBounds(nextGridPosition) == false || _gameBoard.Tilemap.GetTile(nextGridPosition) != null || (instanceTileAtNextPosition != null && instanceTileAtNextPosition != this && instanceTileAtNextPosition is not FinishPortalInstance))
                    return false;
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
        private void Shift(Vector3Int direction, List<SnakebirdInstance> snakebirdsToShift)
        {
            foreach (SnakebirdInstance snakebirdInstance in snakebirdsToShift)
            {
                for (int i = 0; i < snakebirdInstance.SnakebirdSegments.Count; i++)
                {
                    Transform currentSegment = snakebirdInstance.SnakebirdSegments[i].transform;
                    currentSegment.position = _gameBoard.Tilemap.layoutGrid.CellToWorld(_gameBoard.Tilemap.layoutGrid.WorldToCell(currentSegment.position) + direction);
                }
            }
        }
        #endregion
    }
    public struct SnakebirdSaveData
    {
        public Vector3Int[] segmentsGridPosition;
        public bool isDead;
        public bool isFinished;
    }
}