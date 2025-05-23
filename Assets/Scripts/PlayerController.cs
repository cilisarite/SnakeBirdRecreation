using Snakebird.InstanceTile;
using Snakebird.Tile;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Snakebird.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Serialized.
        [SerializeField] SnakebirdInstance _snakebird;
        [SerializeField] GameBoard _gameBoard;
        #endregion

        #region Structures.
        #endregion

        #region Public.
        public static InputAction moveAction;
        public static InputAction undoAction;
        public static InputAction clickAction;
        public static InputAction pointAction;
        #endregion

        #region Private.
        #endregion

        #region Events.
        #endregion

        #region Listeners.
        void Awake()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            moveAction.performed += OnMove;
            undoAction = InputSystem.actions.FindAction("Undo");
            undoAction.performed += OnUndo;
            clickAction = InputSystem.actions.FindAction("Click");
            clickAction.performed += OnClick;
            pointAction = InputSystem.actions.FindAction("Point");
        }
        void OnDisable()
        {
            moveAction.performed -= OnMove;
            undoAction.performed -= OnUndo;
            clickAction.performed -= OnClick;
        }
        void OnMove(InputAction.CallbackContext callbackContext)
        {
            _snakebird?.Move(new Vector3Int((int)callbackContext.ReadValue<Vector2>().x, (int)callbackContext.ReadValue<Vector2>().y, 0));
        }
        void OnUndo(InputAction.CallbackContext callbackContext)
        {
            _gameBoard.Rollback();
        }
        void OnClick(InputAction.CallbackContext callbackContext)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(pointAction.ReadValue<Vector2>().x, pointAction.ReadValue<Vector2>().y, 24));
            InstanceTileBase instanceTileBase = _gameBoard.GetInstanceTile(_gameBoard.Tilemap.layoutGrid.WorldToCell(worldPosition));
            if (instanceTileBase is SnakebirdInstance)
            {
                if (instanceTileBase != _snakebird)
                {
                    _snakebird.Sleep();
                    _snakebird = (SnakebirdInstance)instanceTileBase;
                    _snakebird.Wake();
                }
            }
        }
        #endregion

        #region Public Methods.
        #endregion

        #region Private Methods.
        #endregion
    }
}
