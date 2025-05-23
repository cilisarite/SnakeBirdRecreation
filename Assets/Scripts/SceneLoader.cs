using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snakebird
{
    public class SceneLoader : MonoBehaviour
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
        #endregion

        #region Public Methods.
        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
        #endregion

        #region Private Methods.
        #endregion
    }
}