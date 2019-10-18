using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoginScreen
{
    public class LoginButton : MonoBehaviour
    {
        public void Login()
        {
            SceneManager.LoadSceneAsync("MainScreen");
        }
    }
}