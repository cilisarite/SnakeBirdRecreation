using Unity.VisualScripting;
using UnityEngine;

namespace Snakebird
{
    public class SnakebirdVisual : MonoBehaviour
    {
        #region Serialized.
        [SerializeField] SnakebirdInstance _snakebird;
        [SerializeField] Sprite _headNorth;
        [SerializeField] Sprite _north;
        [SerializeField] Sprite _eastWest;
        [SerializeField] Sprite _northEast;
        #endregion

        #region Structures.
        #endregion

        #region Public.
        #endregion

        #region Private.
        #endregion

        #region Events.
        #endregion

        #region Listeners.
        void Start()
        {
            DrawSnake();
        }
        #endregion

        #region Public Methods.
        #endregion

        #region Private Methods.
        private void DrawSnake()
        {
            for (int i = 0; i < _snakebird.SnakebirdSegments.Count; i++)
            {
                int byteMask = 0;
                SpriteRenderer currentSegment = _snakebird.SnakebirdSegments[i];
                Vector3Int gridPosition = _snakebird.GameBoard.Tilemap.layoutGrid.WorldToCell(currentSegment.transform.position);
                byteMask += IsSnakeSegmentAtGridPosition(gridPosition + Vector3Int.up) ? 1 : 0;
                byteMask += IsSnakeSegmentAtGridPosition(gridPosition + Vector3Int.right) ? 2 : 0;
                byteMask += IsSnakeSegmentAtGridPosition(gridPosition + Vector3Int.down) ? 4 : 0;
                byteMask += IsSnakeSegmentAtGridPosition(gridPosition + Vector3Int.left) ? 8 : 0;

                Sprite north;
                if (i == 0)
                {
                    north = _headNorth;
                }
                else
                {
                    north = _north;
                }

                Debug.Log((byte)byteMask);

                switch ((byte)byteMask)
                {
                    // 0000
                    case 0:
                        break;
                    // 0001
                    case 1:
                        currentSegment.sprite = north;
                        currentSegment.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        break;
                    // 0010
                    case 2:
                        currentSegment.sprite = north;
                        currentSegment.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                        break;
                    // 0011
                    case 3:
                        currentSegment.sprite = _northEast;
                        currentSegment.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        break;
                    // 0100
                    case 4:
                        currentSegment.sprite = north;
                        currentSegment.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                        break;
                    // 0101
                    case 5:
                        currentSegment.sprite = _eastWest;
                        currentSegment.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        break;
                    // 0110
                    case 6:
                        currentSegment.sprite = _northEast;
                        currentSegment.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                        break;
                    // 0111
                    case 7:
                        break;
                    // 1000
                    case 8:
                        currentSegment.sprite = north;
                        currentSegment.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        break;
                    // 1001
                    case 9:
                        currentSegment.sprite = _northEast;
                        currentSegment.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        break;
                    // 1010
                    case 10:
                        currentSegment.sprite = _eastWest;
                        currentSegment.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        break;
                    // 1011
                    case 11:
                        break;
                    // 1100
                    case 12:
                        currentSegment.sprite = _northEast;
                        currentSegment.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                        break;
                    // 1101
                    case 13:
                        break;
                    // 1110
                    case 14:
                        break;
                    // 1111
                    case 15:
                        break;
                }
            }
        }
        private bool IsSnakeSegmentAtGridPosition(Vector3Int gridPosition)
        {
            Vector3 worldPosition = _snakebird.GameBoard.Tilemap.layoutGrid.GetCellCenterWorld(gridPosition);
            Collider2D collider2D = Physics2D.OverlapPoint(new Vector2(worldPosition.x, worldPosition.y));
            if (collider2D != null)
            {
                if (collider2D.attachedRigidbody.gameObject == gameObject)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}