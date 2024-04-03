using MelonLoader;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TestMod
{
    public static class BuildInfo
    {
        public const string Name = "Content Mod"; 
        public const string Description = "A mod to Cheat in Content Warning"; 
        public const string Author = "DXXNS / SnickersIZ"; 
        public const string Company = null; 
        public const string Version = "1.0.0"; 
        public const string DownloadLink = null;
    }

    public class TestMod : MelonMod
    {

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {

            if (buildIndex == 2)
            {
                TestMod.Load = new UnityEngine.GameObject();
                TestMod.Load.AddComponent<Fullbright>();
            }
            
            

            MelonLogger.Msg(buildIndex + " - "+ sceneName);
        }
        private static GameObject Load;
        //Window Pos and size
        public Rect windowRect = new Rect(20, 20, 300, 300);


        //Menu Background
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

        //test var lol

        public bool gay = false;
        public override void OnGUI()
        {
            //set window style
            GUIStyle customStyle = new GUIStyle(GUI.skin.window);

            //  Set background color to a dark color (you can adjust these values)
            customStyle.normal.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 1.0f)); // Adjust alpha as needed
            customStyle.focused.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 1.0f)); // Adjust alpha as needed
            customStyle.onNormal.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 1.0f)); // Adjust alpha as needed
            customStyle.hover.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 1.0f)); // Adjust alpha as needed

            // Set text color to a light color
            customStyle.normal.textColor = Color.white; // Adjust text color as needed
            customStyle.focused.textColor = Color.white; // Adjust text color as needed
            customStyle.onNormal.textColor = Color.white; // Adjust text color as needed
            customStyle.hover.textColor = Color.white; // Adjust text color as needed

            
            ESP.EnemyESP();

            windowRect = GUI.Window(0, windowRect, Window, "Content Kindergarden", customStyle);
        }

        public override void OnUpdate()
        {
            Modules.RunModules();

            
        }
        
        
        private void Window(int windowID)
        {
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("Player"))
            {
                Modules.worldw=false;
                Modules.playerw=true;
                Modules.miscw=false;
                Modules.espw = false;
            }
            if (GUILayout.Button("ESP"))
            {
                Modules.worldw = false;
                Modules.playerw = false;
                Modules.miscw = false;
                Modules.espw = true;
            }
            if (GUILayout.Button("World"))
            {
                Modules.worldw = true;
                Modules.playerw = false;
                Modules.miscw = false;
                Modules.espw = false;
            }
            if (GUILayout.Button("Misc"))
            {
                Modules.worldw = false;
                Modules.playerw = false;
                Modules.miscw = true;
                Modules.espw = false;
            }
            GUILayout.EndHorizontal();

            if (Modules.playerw)
            {
                Modules.infHeal = GUILayout.Toggle(Modules.infHeal, "Inf heal");
                Modules.infOxy = GUILayout.Toggle(Modules.infOxy, "Inf Oxy");
                Modules.infStam = GUILayout.Toggle(Modules.infStam, "Inf Stam");
                Modules.infJump = GUILayout.Toggle(Modules.infJump, "Inf Jump");
                Modules.preventDeath = GUILayout.Toggle(Modules.preventDeath, "Prevent Death");
                GUILayout.Space(10);
                GUILayout.Label("Custom Speed Moduleation: "+ Modules.speed);
                Modules.speed = GUILayout.HorizontalSlider(Modules.speed, 2.3f, 10f);
            }
            if (Modules.espw)
            {
                Modules.teamESP = GUILayout.Toggle(Modules.teamESP, "Team ESP");
                Modules.divingBox = GUILayout.Toggle(Modules.divingBox, "Diving Bell ESP");
                Modules.mobESP = GUILayout.Toggle(Modules.mobESP, "Mob ESP");
                if (Modules.mobESP)
                {
                    GUILayout.BeginHorizontal();
                    Modules.mobTracer = GUILayout.Toggle(Modules.mobTracer, "Mob Tracer");
                    GUILayout.EndHorizontal();
                }
            }
            if (Modules.worldw)
            {
                GUILayout.Label("Kill all mobs coming soon");
            }
            if (Modules.miscw)
            {
                GUILayout.Label("leave emty for later");
            }
            
            

            GUI.DragWindow();
        }

       

    }
}