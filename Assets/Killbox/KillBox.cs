using UnityEngine;

namespace Snakebird
{
    public class KillBox : MonoBehaviour
    {
        #region Serialized.
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
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.rigidbody.gameObject.TryGetComponent<SnakebirdInstance>(out SnakebirdInstance snakebirdInstance))
            {
                snakebirdInstance.Die();
            }
        }
        #endregion

        #region Public Methods.
        #endregion

        #region Private Methods.
        #endregion
    }
}