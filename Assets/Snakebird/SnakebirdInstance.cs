using System.Collections.Generic;
using Snakebird.Tile;
using UnityEngine;

namespace Snakebird
{
    public class SnakebirdInstance : MonoBehaviour
    {
        #region Serialized.
        [SerializeField] GameBoard _gameBoard;
        #endregion

        #region Structures.
        #endregion

        #region Public.
        public List<SpriteRenderer> SnakebirdSegments;
        public GameBoard GameBoard => _gameBoard;
        #endregion

        #region Private.
        #endregion

        #region Events.
        #endregion

        #region Listeners.
        void Awake()
        {
            if (_gameBoard == null)
                _gameBoard = transform.parent.gameObject.GetComponent<GameBoard>();
            SnakebirdSegments = new List<SpriteRenderer>();
            GetAllSegmentsRecursive(transform);
        }
        #endregion

        #region Public Methods.
        #endregion

        #region Private Methods.
        private void GetAllSegmentsRecursive(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i).TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                {
                    SnakebirdSegments.Add(spriteRenderer);
                }
                if (parent.GetChild(i).transform.childCount > 0)
                {
                    GetAllSegmentsRecursive(parent.GetChild(i).transform);
                }
            }
        }
        #endregion
    }
}