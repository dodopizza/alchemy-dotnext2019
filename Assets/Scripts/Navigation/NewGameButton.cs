using MainScreen;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Navigation
{
    public class NewGameButton : MonoBehaviour
    {
        public void EraseData(string sceneName)
        {
            GameManager.Instance.EraseData();
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}