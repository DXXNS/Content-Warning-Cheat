using MelonLoader;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TestMod
{
    public static class BuildInfo
    {
        public const string Name = "TestMod"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "Mod for Testing"; // Description for the Mod.  (Set as null if none)
        public const string Author = "TestAuthor"; // Author of the Mod.  (MUST BE SET)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class TestMod : MelonMod
    {


        public override void OnSceneWasLoaded(int buildindex, string sceneName) // Runs when a Scene has Loaded and is passed the Scene's Build Index and Name.
        {
            MelonLogger.Msg("OnSceneWasLoaded: " + buildindex.ToString() + " | " + sceneName);
        }

        public override void OnSceneWasInitialized(int buildindex, string sceneName) // Runs when a Scene has Initialized and is passed the Scene's Build Index and Name.
        {
            MelonLogger.Msg("OnSceneWasInitialized: " + buildindex.ToString() + " | " + sceneName);
            /*foreach (string persistentID in new ItemDatabase().lastLoadedPersistentIDs)
            {
                MelonLogger.Msg(persistentID);
            }*/
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName) {
            MelonLogger.Msg("OnSceneWasUnloaded: " + buildIndex.ToString() + " | " + sceneName);
        }
        public Rect windowRect = new Rect(20, 20, 200, 200);

        private Texture2D MakeTex(int width, int height, Color color)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = color;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        public bool gay = false;
        public override void OnGUI()
        {
            //set window style
            GUIStyle customStyle = new GUIStyle(GUI.skin.window);

            // Set background color to a dark color (you can adjust these values)
            customStyle.normal.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 1.0f)); // Adjust alpha as needed
            customStyle.focused.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 1.0f)); // Adjust alpha as needed
            customStyle.onNormal.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 1.0f)); // Adjust alpha as needed
            customStyle.hover.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 1.0f)); // Adjust alpha as needed

            // Set text color to a light color
            customStyle.normal.textColor = Color.white; // Adjust text color as needed
            customStyle.focused.textColor = Color.white; // Adjust text color as needed
            customStyle.onNormal.textColor = Color.white; // Adjust text color as needed
            customStyle.hover.textColor = Color.white; // Adjust text color as needed

            if (gay)
                ESP.EnemyESP();


            windowRect = GUI.Window(0, windowRect, Window, "Content Kindergarden", customStyle);
        }

        public override void OnUpdate()
        {
            Modules.RunModules();
        }
        public string schtring = "";
        
        private void Window(int windowID)
        {
            Modules.infHeal = GUILayout.Toggle(Modules.infHeal, "Inf heal");
            Modules.infOxy = GUILayout.Toggle(Modules.infOxy, "Inf Oxy");
            Modules.infStam = GUILayout.Toggle(Modules.infStam, "Inf Stam");
            gay = GUILayout.Toggle(gay, "sex");
            
            GUI.DragWindow();
        }

       

    }
}