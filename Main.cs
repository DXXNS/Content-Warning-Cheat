using MelonLoader;
using System.Collections.Generic;
using UnityEngine;
using TestMod.BreadCrumbs;
using HarmonyLib;
using System.Linq;

namespace TestMod
{
    public static class BuildInfo
    {
        public const string Name = "Content Mod";
        public const string Description = "A mod to Cheat in Content Warning";
        public const string Author = "DXXNS / SnickersIZ / Akira";
        public const string Company = null;
        public const string Version = "0.0.4";
        public const string DownloadLink = null;
    }

    public class TestMod : MelonMod
    {
        private Breadcrumbs breadcrumbsInstance;
        public List<string> itemNames = new List<string>();
        private static GameObject Load;
        public Rect windowRect = new Rect(20, 20, 300, 300);

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (buildIndex != 2) return;

            Load = new GameObject();
            Load.AddComponent<Fullbright>();
            breadcrumbsInstance = new Breadcrumbs();
            var harmony = new HarmonyLib.Harmony("com.Akira.TestMod");
            harmony.UnpatchAll();
            MelonLogger.Msg($"{buildIndex} - {sceneName}");
        }

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

        public override void OnGUI()
        {
            if (Modules.Watermark)
                Watermark.Call();

            GUIStyle customStyle = new GUIStyle(GUI.skin.window);
            customStyle.normal.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 1.0f));
            customStyle.focused.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 1.0f));
            customStyle.onNormal.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 1.0f));
            customStyle.hover.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 1.0f));
            customStyle.normal.textColor = Color.white;
            customStyle.focused.textColor = Color.white;
            customStyle.onNormal.textColor = Color.white;
            customStyle.hover.textColor = Color.white;

            if (Modules.toolTip)
            {
                GUI.Label(new Rect(0, 0, Screen.width, 100), "Menu is on INSERT OR N\nPress DEL to Remove Tooltip");
            }

            breadcrumbsInstance?.OnGUI();
            ESP.Update();

            if (!Modules.menuToggle)
                return;

            windowRect = GUI.Window(0, windowRect, Window, "Content Warning", customStyle);
        }

        public List<string> monsterNames = new List<string>
        {
            "BarnacleBall", "BigSlap", "Bombs", "Dog", "Ear", "EyeGuy", "Flicker", "Ghost", "Jelly", "Knifo",
            "Larva", "Mouthe", "Slurper", "Snatcho", "Spider", "Snail", "Toolkit_Fan", "Toolkit_Hammer",
            "Toolkit_Iron", "Toolkit_Vaccuum", "Toolkit_Whisk", "Toolkit_Wisk", "Weeping"
        };



        public override void OnUpdate()
        {
            if (Input.GetKeyUp(KeyCode.N) || Input.GetKeyUp(KeyCode.Insert))
            {
                Modules.menuToggle = !Modules.menuToggle;
                Cursor.visible = Modules.menuToggle ? true : Modules.OldCursorVisible;
                Cursor.lockState = Modules.menuToggle ? CursorLockMode.None : Modules.OldCursorLockMode;
            }
            if (Input.GetKeyDown(KeyCode.Delete))
                Modules.toolTip = false;

            if (Modules.shopLifter)
            {
                if (!Modules.hasLifted)
                {
                    new HarmonyLib.Harmony("com.Akira.TestMod").PatchAll();
                    Modules.hasLifted = true;
                    MelonLogger.Msg("Patched");
                }
            }
            else if (Modules.hasLifted)
            {
                new HarmonyLib.Harmony("com.Akira.TestMod").UnpatchAll();
                Modules.hasLifted = false;
                MelonLogger.Msg("Unpatched");
            }
            Modules.RunModules();
        }

        private KeyCode breadcrumbsKeybind = KeyCode.B;
        private string moneyInput = "0";
        private void SetAllModulesFalse(ref bool moduleToActivate)
        {
            Modules.worldw = Modules.playerw = Modules.miscw = Modules.espw = Modules.enemyw = false;
            moduleToActivate = true;
        }
        private void Window(int windowID)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Player"))
                SetAllModulesFalse(ref Modules.playerw);
            if (GUILayout.Button("ESP"))
                SetAllModulesFalse(ref Modules.espw);
            if (GUILayout.Button("World"))
                SetAllModulesFalse(ref Modules.worldw);
            if (GUILayout.Button("Misc"))
                SetAllModulesFalse(ref Modules.miscw);
            if (GUILayout.Button("Enemy"))
                SetAllModulesFalse(ref Modules.enemyw);
            GUILayout.EndHorizontal();

            if (Modules.playerw)
            {
                Modules.infHeal = GUILayout.Toggle(Modules.infHeal, "Inf heal");
                Modules.infOxy = GUILayout.Toggle(Modules.infOxy, "Inf Oxy");
                Modules.infStam = GUILayout.Toggle(Modules.infStam, "Inf Stam");
                Modules.infJump = GUILayout.Toggle(Modules.infJump, "Inf Jump");
                Modules.breadCrumbs = GUILayout.Toggle(Modules.breadCrumbs, "BreadCrumbs");
                Modules.preventDeath = GUILayout.Toggle(Modules.preventDeath, "Prevent Death");
                GUILayout.Space(10);
                GUILayout.Label("Custom Speed Modulation: " + Modules.speed);
                Modules.speed = GUILayout.HorizontalSlider(Modules.speed, 2.3f, 10f);
                Modules.respawn = GUILayout.Button("Revive");
            }


            bool itemhasbeeninitialized = false;
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
                Modules.itemESP = GUILayout.Toggle(Modules.itemESP, "Item ESP");
            }

            if (Modules.worldw)
            {
                Modules.killAll = GUILayout.Button("Kill all Mobs");
                if (GUILayout.Button(Modules.selectedItemName))
                {
                    if (!itemhasbeeninitialized)
                    {
                        foreach (var item in ItemDatabase.Instance.lastLoadedItems)
                        {
                            itemNames.Add(item.name);
                            MelonLogger.Msg(item.name);
                        }
                        itemhasbeeninitialized = true;
                    }
                    Modules.dropdownOpen = !Modules.dropdownOpen;
                }

                if (Modules.dropdownOpen)
                {
                    Modules.scrollPosition = GUILayout.BeginScrollView(Modules.scrollPosition);
                    foreach (string itemName in itemNames)
                    {
                        if (GUILayout.Button(itemName))
                        {
                            Modules.selectedItemName = itemName;
                            Modules.dropdownOpen = false;
                        }
                    }
                    GUILayout.EndScrollView();
                }

                if (!string.IsNullOrEmpty(Modules.selectedItemName))
                {
                    if (GUILayout.Button("Spawn Item"))
                    {
                        Duplicator.SpawnItem(Modules.selectedItemName);
                    }
                }

                if (GUILayout.Button(Modules.selectedMonsterName))
                {
                    Modules.dropdownOpenMonster = !Modules.dropdownOpenMonster;
                }

                if (Modules.dropdownOpenMonster)
                {
                    Modules.scrollPositionMonster = GUILayout.BeginScrollView(Modules.scrollPositionMonster);
                    foreach (string monsterName in monsterNames)
                    {
                        if (GUILayout.Button(monsterName))
                        {
                            Modules.selectedMonsterName = monsterName;
                            Modules.dropdownOpenMonster = false;
                        }
                    }
                    GUILayout.EndScrollView();
                }

                if (!string.IsNullOrEmpty(Modules.selectedMonsterName))
                {
                    if (GUILayout.Button("Spawn Monster"))
                    {
                        MonsterSpawner.SpawnMonster(Modules.selectedMonsterName);
                    }
                }
            }

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
                Modules.shopLifter = GUILayout.Toggle(Modules.shopLifter, "Shop Lifter");
                GUILayout.BeginHorizontal();
                GUILayout.Label("Money amount : ");
                Modules.moneyfield = GUILayout.TextField(Modules.moneyfield, GUILayout.Width(100));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Money (Host Only)"))
                {
                    int moneyToAdd;
                    if (int.TryParse(Modules.moneyfield, out moneyToAdd))
                    {
                        Modules.money = true;
                    }
                    else
                    {
                        MelonLogger.Msg("Invalid number");
                    }
                }
                GUILayout.EndHorizontal();

                var itemNames = GameObject.FindObjectsOfType<ItemInstance>()
                .ToList()
                .Select(itemInstance => itemInstance.item.name)
                .Distinct()
                .ToList();

                if (GUILayout.Button(Modules.selectedItemName))
                {
                    Modules.dropdownOpen = !Modules.dropdownOpen;
                }

                if (Modules.dropdownOpen)
                {
                    Modules.scrollPosition = GUILayout.BeginScrollView(Modules.scrollPosition);
                    foreach (string itemName in itemNames)
                    {
                        if (GUILayout.Button(itemName))
                        {
                            Modules.selectedItemName = itemName;
                            Modules.dropdownOpen = false;
                        }
                    }
                    GUILayout.EndScrollView();
                }

                Modules.duplicateItems = GUILayout.Toggle(Modules.duplicator, "Duplicate " + Modules.selectedItemName);
            }

            if (Modules.enemyw)
            {
                var enemyNames = GameObject.FindObjectsOfType<Bot>()
                    .Select(enemy => enemy.name)
                    .Distinct()
                    .ToList();

                if (GUILayout.Button(Modules.selectedEnemyName))
                {
                    Modules.dropdownOpenEnemy = !Modules.dropdownOpenEnemy;
                }

                if (Modules.dropdownOpenEnemy)
                {
                    Modules.scrollPositionEnemy = GUILayout.BeginScrollView(Modules.scrollPositionEnemy);
                    foreach (string enemyName in enemyNames)
                    {
                        if (GUILayout.Button(enemyName))
                        {
                            Modules.selectedEnemyName = enemyName;
                            Modules.dropdownOpenEnemy = false;
                        }
                    }
                    GUILayout.EndScrollView();
                }

                var playerNames = GameObject.FindObjectsOfType<Player>()
                    .Select(player => player.name)
                    .Distinct()
                    .ToList();

                if (GUILayout.Button(Modules.selectedPlayerName))
                {
                    Modules.dropdownOpenPlayer = !Modules.dropdownOpenPlayer;
                }

                if (Modules.dropdownOpenPlayer)
                {
                    Modules.scrollPositionPlayer = GUILayout.BeginScrollView(Modules.scrollPositionPlayer);
                    foreach (string playerName in playerNames)
                    {
                        if (GUILayout.Button(playerName))
                        {
                            Modules.selectedPlayerName = playerName;
                            Modules.dropdownOpenPlayer = false;
                        }
                    }
                    GUILayout.EndScrollView();
                }

                Player selectedPlayer = GameObject.FindObjectsOfType<Player>()
                    .FirstOrDefault(player => player.name == Modules.selectedPlayerName);
                Bot selectedMonster = GameObject.FindObjectsOfType<Bot>()
                    .FirstOrDefault(monster => monster.name == Modules.selectedEnemyName);

                if (GUILayout.Button("AddIgnore"))
                {
                    Enemy.AddIgnore(selectedPlayer, selectedMonster);
                }

                if (GUILayout.Button("DelIgnore"))
                {
                    Enemy.RemoveIgnore(selectedPlayer, selectedMonster);
                }

                if (GUILayout.Button("IgnoreAll"))
                {
                    Enemy.AllIgnore(selectedPlayer);
                }
            }

            GUI.DragWindow();

        }



    }
}
