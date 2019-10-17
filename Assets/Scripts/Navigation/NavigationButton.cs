using UnityEngine;
using UnityEngine.SceneManagement;

namespace Navigation 
{
    public class NavigationButton : MonoBehaviour
    {
        public void GoToScene(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}