using MelonLoader;
using System.Collections.Generic;
using UnityEngine;
using TestMod.BreadCrumbs;
using HarmonyLib;
using System.Linq;
using Photon.Pun;
using System;
using Photon.Realtime;
using Steamworks;
using System.Collections;
using POpusCodec.Enums;
using Unity.Collections.LowLevel.Unsafe;
using Zorro.Core;

namespace TestMod
{
    public static class BuildInfo
    {
        public const string Name = "Content Mod"; public const string Description = "A mod to Cheat in Content Warning"; public const string Author = "DXXNS / SnickersIZ / Akira"; public const string Company = null; public const string Version = "0.0.4"; public const string DownloadLink = null;
    }

    public class TestMod : MelonMod
    {
        private Breadcrumbs breadcrumbsInstance;
        private static GameObject Load;
        public Rect windowRect = new Rect(20, 20, 400, 400);
        public static List<Player> PlayerControllers = new List<Player>();
        public static List<Room> Rooms = new List<Room>();
        Photon.Realtime.Player[] otherPlayers;
        public SteamAPICall_t hAPICall;
        public static List<string> enemyNames = new List<string>();
        public static List<string> playerNames = new List<string>();
        public static Bot selectedMonster;
        public static Player selectedPlayer;
        public static bool initialized = false;
        public static bool AntiCrash = false;
        public static DivingBell divingBell; // Obtenez une référence à l'instance de DivingBell

        public static Vector3 HeadPos;
        public static bool AlreadyWaiting = false;
        public static GameObject coroutineRunnerObject = null;

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            MelonLogger.Msg($"Scene initialized: {buildIndex} - {sceneName}");
            
            if (coroutineRunnerObject == null)
            {
                GameObject coroutineRunnerObject = new GameObject("CoroutineRunner");
                coroutineRunnerObject.AddComponent<CoroutineRunner>();
                UnityEngine.Object.
                DontDestroyOnLoad(coroutineRunnerObject);
                KeyBinds.Start();
            }
            
            if (!AntiCrash)
            {
                AntiCrash = true;
                MelonLogger.Msg("Sleeping for 5 seconds to allow the game to load in OnSceneWasInitialized");
                //CoroutineRunner.Instance.StartMyCoroutine(ToggleBooleans());
                AntiCrash = false;
            }
            Load = new GameObject();
            Load.AddComponent<Fullbright>();
            breadcrumbsInstance = new Breadcrumbs();
            

        }
        public static IEnumerator ToggleBooleans()
        {
            // Sauvegarde l'état original des booléens
            bool originalInfHeal = Modules.infHeal;
            bool originalInfOxy = Modules.infOxy;
            bool originalInfStam = Modules.infStam;
            bool originalBreadCrumbs = Modules.breadCrumbs;
            bool originalDuplicator = Modules.duplicator;
            bool originalShopLifter = Modules.ShopLifter;
            bool originalTeamESP =  Modules.teamESP;
            bool originalMobESP = Modules.mobESP;
            bool originalMobTracer = Modules.mobTracer;
            bool originalItemESP = Modules.itemESP;
            bool originalDivingBox = Modules.divingBox;
            bool originalKillAll = Modules.killAll;
            bool originalInfJump = Modules.infJump;
            bool originalPreventDeath = Modules.preventDeath;
            bool originalGoodLight = Modules.goodLight;
            bool originalPlayerw = Modules.playerw;
            bool originalEspw = Modules.espw;
            bool originalWorldw = Modules.worldw;
            bool originalMiscw = Modules.miscw;
            bool originalEnemyw = Modules.enemyw;
            bool originalToolTip = Modules.toolTip;
            bool originalWatermark = Modules.Watermark;
            bool originalCustomFOV = Modules.customFOV;
            bool originalIgnoreWebs = Modules.ignoreWebs;
            bool originalDelRay = Modules.delRay;
            bool originalSpawnDropdownOpen = Modules.spawnDropdownOpen;
            bool originalDropdownOpenMonster = Modules.dropdownOpenMonster;
            bool originalRespawn = Modules.respawn;
            bool originalMoney = Modules.money;
            bool originalHasLifted = Modules.hasLifted;
            bool originalDropdownOpen = Modules.dropdownOpen;
            bool originalDuplicateItems = Modules.duplicateItems;
            bool originalDropdownOpenEnemy = Modules.dropdownOpenEnemy;
            bool originalDropdownOpenPlayer = Modules.dropdownOpenPlayer;
            bool originalMenuToggle = Modules.menuToggle;
            bool originalOldCursorVisible = Modules.OldCursorVisible;
            bool originalInfinitesshockstick = Modules.infinitesshockstick;
            bool originalAntiragdoll = Modules.antiragdoll;
            bool originalAdd4players = Modules.add4players;
            bool originalInfiniteBattery = Modules.infiniteBattery;
            bool originalInfinitecameratime = Modules.infinitecameratime;

            // Inverse les valeurs des booléens
            Modules.infHeal = false;
            Modules.infOxy = false;
            Modules.infStam = false;
            Modules.breadCrumbs = false;
            Modules.duplicator = false;
            Modules.ShopLifter = false;
            Modules.teamESP = false;
            Modules.mobESP = false;
            Modules.mobTracer = false;
            Modules.itemESP = false;
            Modules.divingBox = false;
            Modules.killAll = false;
            Modules.infJump = false;
            Modules.preventDeath = false;
            Modules.goodLight = false;
            Modules.toolTip = false;
            Modules.customFOV = false;
            Modules.ignoreWebs = false;
            Modules.delRay = false;
            Modules.spawnDropdownOpen = false;
            Modules.dropdownOpenMonster = false;
            Modules.respawn = false;
            Modules.money = false;
            Modules.shopLifter = false;
            Modules.hasLifted = false;
            Modules.dropdownOpen = false;
            Modules.duplicateItems = false;
            Modules.dropdownOpenEnemy = false;
            Modules.dropdownOpenPlayer = false;
            Modules.OldCursorVisible = false;
            Modules.infinitesshockstick = false;
            Modules.antiragdoll = false;
            Modules.add4players = false;
            Modules.infiniteBattery = false;

            Modules.infinitecameratime = false;

            // Attend 10 secondes
            yield return new WaitForSeconds(5);

            // Restaure les valeurs originales des booléens
            Modules.infHeal = originalInfHeal;
            Modules.infOxy = originalInfOxy;
            Modules.infStam = originalInfStam;
            Modules.breadCrumbs = originalBreadCrumbs;
            Modules.duplicator = originalDuplicator;
            Modules.ShopLifter = originalShopLifter;
            Modules.teamESP = originalTeamESP;
            Modules.mobESP = originalMobESP;
            Modules.mobTracer = originalMobTracer;
            Modules.itemESP = originalItemESP;
            Modules.divingBox = originalDivingBox;
            Modules.killAll = originalKillAll;
            Modules.infJump = originalInfJump;
            Modules.preventDeath = originalPreventDeath;
            Modules.goodLight = originalGoodLight;
            Modules.playerw = originalPlayerw;
            Modules.espw = originalEspw;
            Modules.worldw = originalWorldw;
            Modules.miscw = originalMiscw;
            Modules.enemyw = originalEnemyw;
            Modules.toolTip = originalToolTip;
            Modules.Watermark = originalWatermark;
            Modules.customFOV = originalCustomFOV;
            Modules.ignoreWebs = originalIgnoreWebs;
            Modules.delRay = originalDelRay;
            Modules.spawnDropdownOpen = originalSpawnDropdownOpen;
            Modules.dropdownOpenMonster = originalDropdownOpenMonster;
            Modules.respawn = originalRespawn;
            Modules.money = originalMoney;
            Modules.shopLifter = originalShopLifter;
            Modules.hasLifted = originalHasLifted;
            Modules.dropdownOpen = originalDropdownOpen;
            Modules.duplicateItems = originalDuplicateItems;
            Modules.dropdownOpenEnemy = originalDropdownOpenEnemy;
            Modules.dropdownOpenPlayer = originalDropdownOpenPlayer;
            Modules.menuToggle = originalMenuToggle;
            Modules.OldCursorVisible = originalOldCursorVisible;
            Modules.infinitesshockstick = originalInfinitesshockstick;
            Modules.antiragdoll = originalAntiragdoll;
            Modules.add4players = originalAdd4players;
            Modules.infiniteBattery = originalInfiniteBattery;
            Modules.infinitecameratime = originalInfinitecameratime;
            AlreadyWaiting = false;
        }

        private Texture2D MakeTex(int width, int height, Color color)
        {
            var pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = color;
            }
            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        public override void OnGUI()
        {

            if (Modules.Watermark)
                Watermark.Call();

            var customStyle = new GUIStyle(GUI.skin.window);
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

        float natNextUpdateTime = 0f;
        
        public override void OnUpdate()
        {
            KeyBinds.OnUpdate();
            DivingBell activeDivingBell = GameObject.FindObjectsOfType<DivingBell>()
    .FirstOrDefault(db => db.isActiveAndEnabled);
            if (Player.localPlayer != null)
            {
                HeadPos = Player.localPlayer.HeadPosition();
                if (activeDivingBell != null)
                {
                    GameObject divingBellGameObject = activeDivingBell.gameObject;
                    Vector3 position = divingBellGameObject.transform.position;
                    if(HeadPos.x >= position.x - 4f && HeadPos.x <= position.x + 4f && HeadPos.z >= position.z - 5f && HeadPos.z <= position.z + 5f)
                    {
                        if (!AlreadyWaiting)
                        {
                            AlreadyWaiting = true;
                            CoroutineRunner.Instance.StartMyCoroutine(ToggleBooleans());
                        }
                    }
                    else
                    {
                        // Print the position of the player and the position of the Diving Bell
                        /*MelonLogger.Msg($"Player position: {HeadPos}");
                        MelonLogger.Msg($"Diving Bell position: {position}");*/
                    }

                }
                else
                {
                    /*MelonLogger.Msg("Diving Bell is null (Tell Akira to delete that)");*/
                }
            }
            else
            {                 /*MelonLogger.Msg("Player.localPlayer is null (Tell Akira to delete that)");*/
            }

            natNextUpdateTime += Time.deltaTime;

            if (natNextUpdateTime >= 3f)
            {
                PlayerControllers = Resources.FindObjectsOfTypeAll<Player>().ToList();
                Rooms = Resources.FindObjectsOfTypeAll<Room>().ToList();
                enemyNames = GameObject.FindObjectsOfType<Bot>()
                        .Select(enemy => enemy.name)
                        .Distinct()
                        .ToList();
                playerNames = GameObject.FindObjectsOfType<Player>()
                        .Select(player => player.name)
                        .Distinct()
                        .ToList();
                otherPlayers = PhotonNetwork.PlayerListOthers;
                natNextUpdateTime = 0f;
            }
            if (Input.GetKeyUp(KeyCode.N) || Input.GetKeyUp(KeyCode.Insert))
            {
                Modules.menuToggle = !Modules.menuToggle;
                Cursor.visible = Modules.menuToggle;
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
        private void SetAllModulesFalse(ref bool moduleToActivate)
        {
            Modules.worldw = Modules.playerw = Modules.miscw = Modules.espw = Modules.enemyw = false; Modules.keybindw = false;
            moduleToActivate = true;
        }
        bool itemhasbeeninitialized = false;
       public List<string> monsterNames = new List<string>
{
    "BarnacleBall",    // spawns
    "BigSlap",         // spawns
    "Bombs",           // spawns
    "Dog",             // spawns
    "Ear",             // spawns
    "EyeGuy",          // spawns
    "Flicker",         // spawns
    "Ghost",           // spawns
    "Jello",           // spawns
    "Knifo",           // spawns
    "Larva",           // spawns
    "Mouthe",          // spawns
    "Slurper",         // spawns
    "Snatcho",         // spawns
    "Spider",          // spawns
    "Zombe",          // spawns
    "Toolkit_Fan",     // spawns
    "Toolkit_Hammer",  // spawns
    "Toolkit_Iron",    // spawns
    "Toolkit_Vaccuum", // spawns
    "Toolkit_Whisk",   // spawns
    "Toolkit_Wisk",    // spawns
    "Weeping", // spawns


};

        public List<string> itemNames = new List<string>();
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
            if (GUILayout.Button("Keybinds"))
                SetAllModulesFalse(ref Modules.keybindw);
            GUILayout.EndHorizontal();

            if (Modules.playerw)
            {
                Modules.infHeal = GUILayout.Toggle(Modules.infHeal, "Infinite heal");
                Modules.infOxy = GUILayout.Toggle(Modules.infOxy, "Infinite Oxy");
                Modules.infStam = GUILayout.Toggle(Modules.infStam, "Infinite Stam");
                Modules.infJump = GUILayout.Toggle(Modules.infJump, "Infinite Jump");
                Modules.breadCrumbs = GUILayout.Toggle(Modules.breadCrumbs, "BreadCrumbs");
                Modules.preventDeath = GUILayout.Toggle(Modules.preventDeath, "Prevent Death");
                GUILayout.Space(10);
                GUILayout.Label("Custom Speed Modulation: " + Modules.speed);
                Modules.speed = GUILayout.HorizontalSlider(Modules.speed, 2.3f, 10f);
                Modules.respawn = GUILayout.Button("Revive");
                Modules.infinitesshockstick = GUILayout.Toggle(Modules.infinitesshockstick, "Infinite Shock Stick");
                Modules.infiniteBattery = GUILayout.Toggle(Modules.infiniteBattery, "Infinite Battery");
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
                GUILayout.BeginVertical(GUI.skin.box);
                
                if (GUILayout.Button("Request Lobby List"))
                {
                    hAPICall = SteamMatchmaking.RequestLobbyList();
                    Debug.Log("Requested Lobby List");
                }

                if (GUILayout.Button("Random Join"))
                {
                    MainMenuHandler.Instance.JoinRandom();

                }

                /*if (GUILayout.Button("OnCreatedRoom"))
                {
                    MainMenuHandler.Instance.OnCreatedRoom();

                }*/
                GUILayout.EndVertical();

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
                Modules.infinitecameratime = GUILayout.Toggle(Modules.infinitecameratime, "Infinite Camera Time");
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
                /*Modules.add4players = GUILayout.Button("Add 4 Players");*/
            }

            if (Modules.enemyw)
            {
                try
                {

                    if (GUILayout.Button(Modules.selectedEnemyName))
                    {
                        Modules.dropdownOpenEnemy = !Modules.dropdownOpenEnemy;
                    }

                    if (Modules.dropdownOpenEnemy && enemyNames.Count > 0)
                    {
                        Modules.scrollPositionEnemy = GUILayout.BeginScrollView(Modules.scrollPositionEnemy);
                        foreach (string enemyName in enemyNames)
                        {
                            if (GUILayout.Button(enemyName))
                            {
                                Modules.selectedEnemyName = enemyName;
                                Modules.dropdownOpenEnemy = false;
                                selectedMonster = GameObject.FindObjectsOfType<Bot>()
                        .FirstOrDefault(monster => monster.name == Modules.selectedEnemyName);
                            }
                        }
                        GUILayout.EndScrollView();
                    }


                    if (GUILayout.Button(Modules.selectedPlayerName))
                    {
                        Modules.dropdownOpenPlayer = !Modules.dropdownOpenPlayer;
                    }

                    if (Modules.dropdownOpenPlayer)
                    {
                        Modules.scrollPositionPlayer = GUILayout.BeginScrollView(Modules.scrollPositionPlayer);
                        foreach (Player player in PlayerControllers)
                        {
                            if (player == null)
                            {
                                //MelonLogger.Msg("Player is null");
                            }
                            else if (player.refs == null)
                            {
                                //MelonLogger.Msg("Player.refs is null");
                            }
                            else if (player.refs.view == null)
                            {
                                //MelonLogger.Msg("Player.refs.view is null");
                            }
                            else if (player.refs.view.Controller == null)
                            {
                                //MelonLogger.Msg("Player.refs.view.Controller is null");
                            }
                            else
                            {
                                if (!player.ai)
                                {
                                    string playerName = player.refs.view.Controller.ToString();
                                    if (!string.IsNullOrEmpty(playerName) && GUILayout.Button(playerName))
                                    {
                                        Modules.selectedPlayerName = playerName;
                                        Modules.dropdownOpenPlayer = false;
                                        selectedPlayer = PlayerControllers
                    .FirstOrDefault(player1 => player.refs != null && player.refs.view != null && player.refs.view.Controller != null && player.refs.view.Controller.ToString() == Modules.selectedPlayerName);

                                    }
                                }
                            }
                        }
                        GUILayout.EndScrollView();
                    }
                    else
                    {
                        // Ajout d'un log lorsque le menu déroulant n'est pas ouvert
                    }



                    

                    if (selectedPlayer != null && selectedMonster != null)
                    {
                        
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
                }
                catch (Exception ex)
                {
                    // Log the exception to the console
                    MelonLogger.Error($"An error occurred in the enemy module: {ex}");
                }
            }
            if (Modules.keybindw)
            {
                KeyBinds.DisplayUI();
                KeyBinds.HandleInput();
            }


            GUI.DragWindow();

        }



    }

}
public class CoroutineRunner : MonoBehaviour
{
    public static CoroutineRunner Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StartMyCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}