using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace LoginScreen
{
    public class LoginManager : MonoBehaviour
    {
        public InputField inputField;
        
        private GameObject _somethingWrongWindowPrefab;

        private void Start()
        {
            if (PlayerPrefs.HasKey(Constants.UserIdKey))
            {
                SceneManager.LoadSceneAsync("MainScreen");
            }
            
            _somethingWrongWindowPrefab = (GameObject) Resources.Load("Prefabs/SomethingWrongWindow", typeof(GameObject));
        }

        public async void Login()
        {
            var userId = Guid.NewGuid().ToString();
            var userName = inputField.text.Trim();

            if (string.IsNullOrEmpty(userName))
                return;
            
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
                    Instantiate(_somethingWrongWindowPrefab);
                }
                
                PlayerPrefs.SetString(Constants.UserIdKey, userId);
                PlayerPrefs.Save();
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