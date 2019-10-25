using MainScreen;
using UnityEngine;

namespace Navigation
{
    public class ExitWindow : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.CheckAndLockInput();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelExit();
            }
        }

        public void Exit()
        {
            Debug.Log("Exiting");
            Application.Quit();
        }

        public void CancelExit()
        {
            GameManager.Instance.UnlockInput();
            Destroy(gameObject);
        }
    }
}