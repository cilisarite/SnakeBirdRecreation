using System.Collections.Generic;
using Snakebird.InstanceTile;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public List<SpikeInstance> Spikes => _spikes;
        public List<FinishPortalInstance> FinishPortals => _finishPortals;
        #endregion

        #region Private.
        private Vector3Int _origin;
        private List<SnakebirdInstance> _snakeBirds;
        private List<FruitInstance> _fruits;
        private List<SpikeInstance> _spikes;
        private List<FinishPortalInstance> _finishPortals;
        #endregion

        #region Events.
        #endregion

        #region Listeners.
        void Awake()
        {
            _origin = _tilemap.origin;
            _snakeBirds = new List<SnakebirdInstance>();
            _fruits = new List<FruitInstance>();
            _spikes = new List<SpikeInstance>();
            _finishPortals = new List<FinishPortalInstance>();

            _boardSaveStates = new List<BoardSaveData>();
        }
        void OnFinish()
        {
            foreach (SnakebirdInstance snakebirdInstance in _snakeBirds)
            {
                if (snakebirdInstance.IsFinished == false)
                {
                    return;
                }
            }
            SceneManager.LoadScene("LevelSelect");
        }
        #endregion

        #region Public Methods.
        public bool IsInBounds(Vector3Int gridPosition)
        {
            return (_origin.x <= gridPosition.x && gridPosition.x <= _origin.x + _sizeBounds.x - 1) && (_origin.y <= gridPosition.y && gridPosition.y <= _origin.y + _sizeBounds.y - 1);
        }
        public bool IsAllFruitEaten()
        {
            foreach (FruitInstance fruitInstance in _fruits)
            {
                if (fruitInstance.IsEaten == false)
                {
                    return false;
                }
            }
            return true;
        }
        public void AddSnakebirdInstance(SnakebirdInstance snakebirdInstance)
        {
            if (_snakeBirds.Contains(snakebirdInstance))
                return;
            snakebirdInstance.OnFinish += OnFinish;
            _snakeBirds.Add(snakebirdInstance);
        }
        public void RemoveSnakebirdInstance(SnakebirdInstance snakebirdInstance)
        {
            snakebirdInstance.OnFinish -= OnFinish;
            _snakeBirds.Remove(snakebirdInstance);
        }
        public void AddFruitInstance(FruitInstance fruitInstance)
        {
            if (_fruits.Contains(fruitInstance))
                return;
            _fruits.Add(fruitInstance);
        }
        public void RemoveFruitInstance(FruitInstance fruitInstance)
        {
            _fruits.Remove(fruitInstance);
        }
        public void AddSpikeInstance(SpikeInstance spikeInstance)
        {
            if (_spikes.Contains(spikeInstance))
                return;
            _spikes.Add(spikeInstance);
        }
        public void RemoveSpikeInstance(SpikeInstance spikeInstance)
        {
            _spikes.Remove(spikeInstance);
        }
        public void AddFinishPortalInstance(FinishPortalInstance finishPortalInstance)
        {
            _finishPortals.Add(finishPortalInstance);
        }
        public void RemoveFinishPortalInstance(FinishPortalInstance finishPortalInstance)
        {
            _finishPortals.Remove(finishPortalInstance);
        }
        public InstanceTileBase GetInstanceTile(Vector3Int gridPosition)
        {
            foreach (SnakebirdInstance snakebirdInstance in _snakeBirds)
            {
                foreach (SnakebirdSegment snakebirdSegment in snakebirdInstance.SnakebirdSegments)
                {
                    if (_tilemap.layoutGrid.WorldToCell(snakebirdSegment.transform.position) == gridPosition)
                    {
                        if (snakebirdInstance.IsDead == false)
                        {
                            return snakebirdInstance;
                        }
                    }
                }
            }
            foreach (FruitInstance fruitInstance in _fruits)
            {
                if (_tilemap.layoutGrid.WorldToCell(fruitInstance.transform.position) == gridPosition)
                {
                    if (fruitInstance.IsEaten == false)
                    {
                        return fruitInstance;
                    }
                }
            }
            foreach (SpikeInstance spikeInstance in _spikes)
            {
                if (_tilemap.layoutGrid.WorldToCell(spikeInstance.transform.position) == gridPosition)
                {
                    return spikeInstance;
                }
            }
            foreach (FinishPortalInstance finishPortal in _finishPortals)
            {
                if (_tilemap.layoutGrid.WorldToCell(finishPortal.transform.position) == gridPosition)
                {
                    return finishPortal;
                }
            }
            return null;
        }
        #endregion

        #region Private Methods.
        #endregion
    }
}
