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
        }
        void OnDisable()
        {
            moveAction.performed -= OnMove;
            undoAction.performed -= OnUndo;
        }
        void OnMove(InputAction.CallbackContext callbackContext)
        {
            _snakebird?.Move(new Vector3Int((int)callbackContext.ReadValue<Vector2>().x, (int)callbackContext.ReadValue<Vector2>().y, 0));
        }
        void OnUndo(InputAction.CallbackContext callbackContext)
        {
            _gameBoard.Rollback();
        }
        #endregion

        #region Public Methods.
        #endregion

        #region Private Methods.
        #endregion
    }
}
