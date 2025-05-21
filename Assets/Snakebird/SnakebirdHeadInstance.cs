using System.Collections.Generic;
using Snakebird.Tile;
using UnityEngine;
using UnityEngine.UIElements;

namespace Snakebird.Tile
{
    public class SnakebirdHeadInstance : TileInstance
    {
        #region Serialized.
        #endregion

        #region Structures.
        #endregion

        #region Public.
        #endregion

        #region Private.
        private List<SnakebirdBodyInstance> _snakebirdBodies;
        #endregion

        #region Events.
        #endregion

        #region Listeners.
        void Awake()
        {
            _snakebirdBodies = new List<SnakebirdBodyInstance>();
            GetAllBodyChainRecursive(transform);
        }
        #endregion

        #region Public Methods.
        public override void Move(Vector3Int direction)
        {
            Vector3Int originalPosition = _gridPosition;
            base.Move(direction);
            foreach (SnakebirdBodyInstance snakebirdBody in _snakebirdBodies)
            {
                direction = originalPosition - snakebirdBody.GridPosition;
                originalPosition = snakebirdBody.GridPosition;
                snakebirdBody.Move(direction);
            }
        }
        #endregion

        #region Private Methods.
        private void GetAllBodyChainRecursive(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i).TryGetComponent<SnakebirdBodyInstance>(out SnakebirdBodyInstance snakebirdBody))
                {
                    _snakebirdBodies.Add(snakebirdBody);
                    if (snakebirdBody.transform.childCount > 0)
                    {
                        GetAllBodyChainRecursive(snakebirdBody.transform);
                    }
                }
            }
        }
        #endregion
    }
}
