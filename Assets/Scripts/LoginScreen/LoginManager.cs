using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace LoginScreen
{
    public class LoginManager : MonoBehaviour
    {
        public InputField inputField;
        
        private void Start()
        {
            if (PlayerPrefs.HasKey(Constants.UserIdKey))
            {
                SceneManager.LoadSceneAsync("MainScreen");
            }
        }

        public async void Login()
        {
            var userId = Guid.NewGuid().ToString();
            var userName = inputField.text;
            
            var loginRequest = new LoginRequest
            {
                UserId = userId,
                Name = userName
            };

            using (var request = HttpClient.CreateApiPostRequest(Constants.ApiUrl + "/api/UserProfile/add", loginRequest))
            {
                request.timeout = Constants.RpcTimeoutSeconds;

                await request.SendWebRequest();
                
                if (!request.isHttpError && !request.isNetworkError)
                {
                    Debug.Log(request.error);
                    // todo: показать окно, что что-то пошло не так
                }
                
                PlayerPrefs.SetString(Constants.UserIdKey, userId);
                SceneManager.LoadSceneAsync("MainScreen");
            }
        }

        private class LoginRequest
        {
            public string Name;
            
            public string UserId;
        }
    }
}