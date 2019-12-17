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
        public GameObject underUpperLayer;

        private GameObject _somethingWrongWindowPrefab;

        private void Start()
        {
            if (PlayerPrefs.HasKey(Constants.UserIdKey))
            {
                SceneManager.LoadSceneAsync("MainScreen");
            }
            
            inputField.Select();
            _somethingWrongWindowPrefab = (GameObject) Resources.Load("Prefabs/SomethingWrongWindow", typeof(GameObject));
        }

        public void Login()
        {
            var userId = Guid.NewGuid().ToString();
            var userName = inputField.text.Trim();

            if (string.IsNullOrEmpty(userName))
                return;
            
            // var loginRequest = new LoginRequest
            // {
            //     UserId = userId,
            //     Name = userName
            // };

            try
            {
                // var url = Constants.ApiUrl + "/api/UserProfile/add";
                // await HttpClient.PostWithRetry(url, loginRequest);
                
                PlayerPrefs.SetString(Constants.UserIdKey, userId);
                PlayerPrefs.Save();
                SceneManager.LoadSceneAsync("MainScreen");
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                Instantiate(_somethingWrongWindowPrefab, underUpperLayer.transform);
            }
        }

        // private class LoginRequest
        // {
        //     public string Name;
        //     
        //     public string UserId;
        // }
    }
}