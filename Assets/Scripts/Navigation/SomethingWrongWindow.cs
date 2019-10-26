using MainScreen;
using UnityEngine;

namespace Navigation
{
    public class SomethingWrongWindow : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.CheckAndLockInput();
        }
        
        public void Ok()
        {
            GameManager.Instance.UnlockInput();
            Destroy(gameObject);
        }
    }
}