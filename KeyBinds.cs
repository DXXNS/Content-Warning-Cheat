using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace TestMod
{
    public class KeyBindsUpdater : MonoBehaviour
    {
        void Update()
        {
            KeyBinds.OnUpdate();
        }
    }

    public static class KeyBinds
    {
        public static bool[] ListofCheats = { Modules.infHeal,
Modules.infOxy,
Modules.infStam,
Modules.breadCrumbs,
Modules.duplicator,
Modules.ShopLifter,
Modules.teamESP,
Modules.mobESP,
Modules.mobTracer,
Modules.itemESP,
Modules.divingBox,
Modules.killAll,
Modules.infJump,
Modules.preventDeath,
Modules.goodLight,
Modules.playerw,
Modules.espw,
Modules.worldw,
Modules.miscw,
Modules.enemyw,
Modules.keybindw,
Modules.toolTip,
Modules.Watermark,
Modules.customFOV,
Modules.ignoreWebs,
Modules.delRay,
Modules.spawnDropdownOpen,
Modules.dropdownOpenMonster,
Modules.respawn,
Modules.money,
Modules.shopLifter,
Modules.hasLifted,
Modules.dropdownOpen,
Modules.duplicateItems,
Modules.dropdownOpenEnemy,
Modules.dropdownOpenPlayer,
Modules.menuToggle,
Modules.infinitesshockstick,
Modules.antiragdoll,
Modules.add4players,
Modules.infiniteBattery,
Modules.infinitecameratime
};
        // Définir les touches par défaut
        public static Dictionary<string, KeyCode> ModuleKeys = new Dictionary<string, KeyCode>();


        // Remplir le dictionnaire avec les éléments de ListofCheats


        // Indique si nous sommes en train de définir une touche
        public static bool IsSettingKey = false;

        // Le module pour lequel nous définissons une touche
        public static string ModuleBeingSet;
        
        public static Dictionary<string, Action<bool>> ModuleActions = new Dictionary<string, Action<bool>>
{
    { "infHeal", b => Modules.infHeal = b },
    { "infOxy", b => Modules.infOxy = b },
    { "infStam", b => Modules.infStam = b },
    { "breadCrumbs", b => Modules.breadCrumbs = b },
    { "duplicator", b => Modules.duplicator = b },
    { "ShopLifter", b => Modules.ShopLifter = b },
    { "teamESP", b => Modules.teamESP = b },
    { "mobESP", b => Modules.mobESP = b },
    { "mobTracer", b => Modules.mobTracer = b },
    { "itemESP", b => Modules.itemESP = b },
    { "divingBox", b => Modules.divingBox = b },
    { "killAll", b => Modules.killAll = b },
    { "infJump", b => Modules.infJump = b },
    { "preventDeath", b => Modules.preventDeath = b },
    { "goodLight", b => Modules.goodLight = b },
    { "Watermark", b => Modules.Watermark = b },
    { "customFOV", b => Modules.customFOV = b },
    { "ignoreWebs", b => Modules.ignoreWebs = b },
    { "delRay", b => Modules.delRay = b },
    { "respawn", b => Modules.respawn = b },
    { "money", b => Modules.money = b },
    { "shopLifter", b => Modules.shopLifter = b },
    { "duplicateItems", b => Modules.duplicateItems = b },
    { "menuToggle", b => Modules.menuToggle = b },
    { "infinitesshockstick", b => Modules.infinitesshockstick = b },
    { "antiragdoll", b => Modules.antiragdoll = b },
    { "add4players", b => Modules.add4players = b },
    { "infiniteBattery", b => Modules.infiniteBattery = b },
    { "infinitecameratime", b => Modules.infinitecameratime = b }
};
        public static Dictionary<string, bool> ModuleStates = new Dictionary<string, bool>();

        public static void Start()
        {
            // Charger les touches à partir d'un fichier
            if (File.Exists("keys.json"))
            {
                string json = File.ReadAllText("keys.json");
                ModuleKeys = JsonConvert.DeserializeObject<Dictionary<string, KeyCode>>(json);
            }
            else
            {
                // Si le fichier n'existe pas, initialiser les touches à KeyCode.None
                foreach (var moduleName in ModuleActions.Keys)
                {
                    ModuleKeys.Add(moduleName, KeyCode.None);
                }
            }
            if (File.Exists("states.json"))
            {
                string json = File.ReadAllText("states.json");
                ModuleStates = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);

                // Appliquer l'état de chaque cheat
                foreach (var moduleName in ModuleStates.Keys)
                {
                    ModuleActions[moduleName](ModuleStates[moduleName]);
                }
            }
            else
            {
                // Si le fichier n'existe pas, initialiser tous les cheats à désactivé
                foreach (var moduleName in ModuleActions.Keys)
                {
                    ModuleStates.Add(moduleName, false);
                }
            }
        }


        public static void HandleInput()
        {
            if (Event.current.isKey && IsSettingKey)
            {
                ModuleKeys[ModuleBeingSet] = Event.current.keyCode;
                IsSettingKey = false;
                ModuleBeingSet = null;

                // Enregistrer les touches dans un fichier
                string json = JsonConvert.SerializeObject(ModuleKeys);
                File.WriteAllText("keys.json", json);
            }
        }

        public static void OnUpdate()
        {
            foreach (var moduleName in ModuleKeys.Keys)
            {
                if (Input.GetKeyDown(ModuleKeys[moduleName]))
                {
                    // Inverser la valeur du module
                    ModuleActions[moduleName](!Modules.GetModuleByName(moduleName));
                }
            }
            foreach (var moduleName in ModuleKeys.Keys)
            {
                if (Input.GetKeyDown(ModuleKeys[moduleName]))
                {
                    // Inverser la valeur du module
                    bool newState = !ModuleStates[moduleName];
                    ModuleActions[moduleName](newState);
                    ModuleStates[moduleName] = newState;

                    // Enregistrer l'état d'activation des cheats dans un fichier
                    string json = JsonConvert.SerializeObject(ModuleStates);
                    File.WriteAllText("states.json", json);
                }
            }
        }
        
        public static Vector2 scrollPosition;
        public static void DisplayUI()
        {
            // Commencer une zone de défilement
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            foreach (var moduleName in ModuleKeys.Keys)
            {
                if (GUILayout.Button("Set key for " + moduleName))
                {
                    IsSettingKey = true;
                    ModuleBeingSet = moduleName;
                }

                GUILayout.Label("Key for " + moduleName + ": " + ModuleKeys[moduleName]);
            }

            // Finir la zone de défilement
            GUILayout.EndScrollView();
        }
    }
}