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
        public const string Version = "0.0.3"; 
        public const string DownloadLink = null;
    }

    public class TestMod : MelonMod
    {

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {

           
            
            
            //Open for debug purposes
            //MelonLogger.Msg(buildIndex + " - "+ sceneName);
        }

        
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

        
        public override void OnGUI()
        {
            if (Modules.Watermark)
                Watermark.Call();

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

            //Implementation of the tooltip
            if (Modules.toolTip)
            {
                GUI.Label(new Rect(0, 0, Screen.width, 100), "Menu is on INSERT\nPress DEL to Remove Tooltip");
            }

            ESP.RunESP();

            

            if (!Modules.menuToggle)
                return;

            windowRect = GUI.Window(0, windowRect, Window, "Content Warning", customStyle);
        }

        public override void OnUpdate()
        {
            
            //Hide and show for the menu
            if (Input.GetKeyUp(KeyCode.Insert))
            {
                Modules.menuToggle = !Modules.menuToggle;

                // so it doesn't make your cursor disappear in escape menus
                if (Modules.menuToggle)
                {
                    Modules.OldCursorVisible = Cursor.visible;
                    Modules.OldCursorLockMode = Cursor.lockState;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.visible = Modules.OldCursorVisible;
                    Cursor.lockState = Modules.OldCursorLockMode;
                }
            }
            if (Input.GetKeyDown(KeyCode.Delete))
                Modules.toolTip = false;



            Modules.RunModules();

            
        }
        
        //Window
        private void Window(int windowID)
        {
            //4 Categorys
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


            //Window player
            if (Modules.playerw)
            {
                Modules.infHeal = GUILayout.Toggle(Modules.infHeal, "Inf heal");
                Modules.infOxy = GUILayout.Toggle(Modules.infOxy, "Inf Oxy");
                Modules.infStam = GUILayout.Toggle(Modules.infStam, "Inf Stam");
                Modules.infJump = GUILayout.Toggle(Modules.infJump, "Inf Jump");
                Modules.preventDeath = GUILayout.Toggle(Modules.preventDeath, "Prevent Death");
                GUILayout.Space(10);
                GUILayout.Label("Custom Speed Modulation: "+ Modules.speed);
                Modules.speed = GUILayout.HorizontalSlider(Modules.speed, 2.3f, 10f);
            }
            //window esp
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
            //window world
            if (Modules.worldw)
            {
                Modules.killAll = GUILayout.Button("Kill all Mobs");
            }
            //window misc
            if (Modules.miscw)
            {
                Modules.Watermark = GUILayout.Toggle(Modules.Watermark, "Watermark & Active Modules");
                Modules.goodLight = GUILayout.Button("Add own light to scene");
                Modules.customFOV = GUILayout.Toggle(Modules.customFOV, "Custom FOV");
                if (Modules.customFOV)
                {
                    GUILayout.Label("FOV: " + Modules.cusFOVv);
                    Modules.cusFOVv = GUILayout.HorizontalSlider(Modules.cusFOVv, 1f, 179f);
                }
                Modules.delRay = GUILayout.Button("Add Delete Ray");
            }
            
            

            GUI.DragWindow();
        }

       

    }
}