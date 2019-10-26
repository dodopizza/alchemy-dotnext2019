using UnityEngine;

namespace Navigation
{
    public class UrlButton : MonoBehaviour
    {
        public void GoToPage(string pageName)
        {
            Application.OpenURL(pageName);
        }
    }
}