using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestMod.BreadCrumbs
{
    public class Breadcrumbs : MonoBehaviour
    {
        private Dictionary<Vector3, long> breadcrumbs = new Dictionary<Vector3, long>();
        private long lastBreadcrumbTime = 0;
        private bool wasBreadcrumbsEnabled = false;
        private string breadcrumbsKeybind = "b";
        private float keyPressTime = 0f;

        public void Update()
        {
            if (!gameObject.activeInHierarchy || !this.enabled)
            {
                return;
            }

            if (wasBreadcrumbsEnabled && !Modules.breadCrumbs)
            {
                breadcrumbs.Clear();
            }

            if (Input.GetKeyUp(StringToKeyCode(breadcrumbsKeybind)) && Time.time - keyPressTime > 0.5f)
            {
                Modules.breadCrumbs = !Modules.breadCrumbs;
                keyPressTime = Time.time;
                if (!Modules.breadCrumbs)
                {
                    breadcrumbs.Clear();
                }
            }

            wasBreadcrumbsEnabled = Modules.breadCrumbs;

            if (!Modules.breadCrumbs)
                return;

            long currentTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

            if (currentTime - lastBreadcrumbTime >= 1000)
            {
                lastBreadcrumbTime = currentTime;

                foreach (Player player in GameObject.FindObjectsOfType<Player>())
                {
                    if (player != null && player.data != null && !player.ai)
                    {
                        Vector3 playerPosition = player.data.groundPos;
                        breadcrumbs[playerPosition] = currentTime;
                    }
                }
            }
        }

        private KeyCode StringToKeyCode(string key)
        {
            return (KeyCode)System.Enum.Parse(typeof(KeyCode), key.ToUpper());
        }

        public void OnGUI()
        {
            if (!Modules.breadCrumbs || breadcrumbs.Count == 0)
                return;

            foreach (KeyValuePair<Vector3, long> breadcrumb in breadcrumbs)
            {
                Vector3 viewportPosition = Camera.main.WorldToViewportPoint(breadcrumb.Key);

                if (viewportPosition.z > 0 && viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1)
                {
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(breadcrumb.Key);
                    GUI.DrawTexture(new Rect(screenPosition.x, Screen.height - screenPosition.y, 5, 5), Texture2D.whiteTexture);

                    long elapsedTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds() - breadcrumb.Value;
                    int elapsedSeconds = (int)(elapsedTime / 1000);

                    GUI.Label(new Rect(screenPosition.x, Screen.height - screenPosition.y - 20, 100, 20), elapsedSeconds.ToString());
                }
            }
        }
    }
}
