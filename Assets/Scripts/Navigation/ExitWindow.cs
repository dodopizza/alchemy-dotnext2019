using UnityEngine;

namespace Navigation
{
    public class ExitWindow : MonoBehaviour
    {
        public void Exit()
        {
            Debug.Log("Exiting");
            Application.Quit();
        }

        public void CancelExit()
        {
            Destroy(gameObject);
        }
    }
}