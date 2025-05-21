using Snakebird.Tile;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Snakebird.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Serialized.
        [SerializeField] SnakebirdHeadInstance _snakebird;
        #endregion

        #region Structures.
        #endregion

        #region Public.
        public static InputAction moveAction;
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
        }
        void OnDisable()
        {
            moveAction.performed -= OnMove;
        }
        void OnMove(InputAction.CallbackContext callbackContext)
        {
            _snakebird?.Move(new Vector3Int((int)callbackContext.ReadValue<Vector2>().x, (int)callbackContext.ReadValue<Vector2>().y, 0));
        }
        #endregion

        #region Public Methods.
        #endregion

        #region Private Methods.
        #endregion
    }
}
