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
            _snakebird.OnMove += DrawSnake;
            _snakebird.OnLoad += DrawSnake;
        }
        void OnDisable()
        {
            _snakebird.OnMove -= DrawSnake;
            _snakebird.OnLoad -= DrawSnake;
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

                SpriteRenderer currentSegment = _snakebird.SnakebirdSegments[i].SpriteRenderer;
                Vector3Int currentGridPosition = _snakebird.GameBoard.Tilemap.layoutGrid.WorldToCell(currentSegment.transform.position);
                Vector3Int lastSegmentGridPosition = currentGridPosition;
                if (i != 0)
                {
                    lastSegmentGridPosition = _snakebird.GameBoard.Tilemap.layoutGrid.WorldToCell(_snakebird.SnakebirdSegments[i - 1].SpriteRenderer.transform.position);
                }
                Vector3Int nextSegmentGridPosition = currentGridPosition;
                if (i != _snakebird.SnakebirdSegments.Count - 1)
                {
                    nextSegmentGridPosition = _snakebird.GameBoard.Tilemap.layoutGrid.WorldToCell(_snakebird.SnakebirdSegments[i + 1].SpriteRenderer.transform.position);
                }

                byteMask += lastSegmentGridPosition == currentGridPosition + Vector3Int.up || nextSegmentGridPosition == currentGridPosition + Vector3Int.up ? 1 : 0;
                byteMask += lastSegmentGridPosition == currentGridPosition + Vector3Int.right || nextSegmentGridPosition == currentGridPosition + Vector3Int.right ? 2 : 0;
                byteMask += lastSegmentGridPosition == currentGridPosition + Vector3Int.down || nextSegmentGridPosition == currentGridPosition + Vector3Int.down ? 4 : 0;
                byteMask += lastSegmentGridPosition == currentGridPosition + Vector3Int.left || nextSegmentGridPosition == currentGridPosition + Vector3Int.left ? 8 : 0;

                Sprite north;
                if (i == 0)
                {
                    north = _headNorth;
                }
                else
                {
                    north = _north;
                }

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
        #endregion
    }
}