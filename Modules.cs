using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zorro.Core;
using TestMod.BreadCrumbs;
using Harmony;
using Steamworks;
using Photon.Realtime;
using Steamworks;
using System.Reflection;
namespace TestMod
{
    public static class Modules
    {
        public static bool infHeal = false, infOxy = false, infStam = false, breadCrumbs = false, duplicator = false, ShopLifter = false;
        public static bool teamESP = false, mobESP = false, mobTracer = false, itemESP = false, divingBox = false;
        public static bool killAll = false;
        public static float speed = 2.3f;
        public static bool infJump = false, preventDeath = false, goodLight = false;
        public static bool playerw = true, espw = false, worldw = false, miscw = false, enemyw = false, keybindw = false;
        public static bool toolTip = true, Watermark = false, customFOV = false, ignoreWebs = false, delRay = false;
        public static float cusFOVv = 60f;
        public static string selectedSpawnItemName = "Select Item", selectedMonsterName = "Select Monster", selectedItemName = "Select Item", selectedEnemyName = "Select Enemy", selectedPlayerName = "Select Player";
        public static bool spawnDropdownOpen = false, dropdownOpenMonster = false, respawn = false, money = false, shopLifter = false, hasLifted = false, dropdownOpen = false, duplicateItems = false, dropdownOpenEnemy = false, dropdownOpenPlayer = false;
        public static string moneyfield = "1000";
        public static Vector2 spawnScrollPosition = Vector2.zero, scrollPositionMonster = Vector2.zero, scrollPosition = Vector2.zero, scrollPositionEnemy = Vector2.zero, scrollPositionPlayer = Vector2.zero;
        public static bool menuToggle { get; set; }
        public static bool OldCursorVisible { get; set; }
        public static CursorLockMode OldCursorLockMode { get; set; }
        public static GameObject c_light, c_ray;
        private static float lastUpdateTime = 0f, updateInterval = 1f / 30f;
        public static bool infinitesshockstick = false;
        private static ShockStickTrigger[] shocksticks;
        public static Player originalplayer;
        public static CSteamID originalSteamId;
        public static Photon.Realtime.Player RealPhotonPlayer;
        public static CSteamID actual;
        public static bool antiragdoll = false;
        public static bool add4players = false;
        public static bool infiniteBattery = false;
        private static float timeSinceLastUpdate = 0.0f;
        private static float updateInterval2 = 1f; // Update 60 times per second
        private static float updateInterval3 = 5f;
        private static float timeSinceLastUpdate2 = 0.0f;
        public static bool infinitecameratime;
        public static VideoCamera[] videoCameras;
        private static Breadcrumbs breadcrumbs2;
        private static Player[] players;
        private static PlayerController[] playerControllers;
        private static ShockStickTrigger[] shocksticks2;
        private static PlayerHandler playerHandler;
        private static BotHandler[] botHandlers;
        private static Bot[] bots;
        private static Web[] webs;
        private static float updatetime3 = 0.0f;
        public static bool GetModuleByName(string moduleName)
        {
            switch (moduleName)
            {
                case "infHeal":
                    return infHeal;
                case "infOxy":
                    return infOxy;
                case "infStam":
                    return infStam;
                case "breadCrumbs":
                    return breadCrumbs;
                case "duplicator":
                    return duplicator;
                case "ShopLifter":
                    return ShopLifter;
                case "teamESP":
                    return teamESP;
                case "mobESP":
                    return mobESP;
                case "mobTracer":
                    return mobTracer;
                case "itemESP":
                    return itemESP;
                case "divingBox":
                    return divingBox;
                case "killAll":
                    return killAll;
                case "infJump":
                    return infJump;
                case "preventDeath":
                    return preventDeath;
                case "goodLight":
                    return goodLight;
                case "playerw":
                    return playerw;
                case "espw":
                    return espw;
                case "worldw":
                    return worldw;
                case "miscw":
                    return miscw;
                case "enemyw":
                    return enemyw;
                case "keybindw":
                    return keybindw;
                case "toolTip":
                    return toolTip;
                case "Watermark":
                    return Watermark;
                case "customFOV":
                    return customFOV;
                case "ignoreWebs":
                    return ignoreWebs;
                case "delRay":
                    return delRay;
                case "spawnDropdownOpen":
                    return spawnDropdownOpen;
                case "dropdownOpenMonster":
                    return dropdownOpenMonster;
                case "respawn":
                    return respawn;
                case "money":
                    return money;
                case "shopLifter":
                    return shopLifter;
                case "hasLifted":
                    return hasLifted;
                case "dropdownOpen":
                    return dropdownOpen;
                case "duplicateItems":
                    return duplicateItems;
                case "dropdownOpenEnemy":
                    return dropdownOpenEnemy;
                case "dropdownOpenPlayer":
                    return dropdownOpenPlayer;
                case "menuToggle":
                    return menuToggle;
                case "infinitesshockstick":
                    return infinitesshockstick;
                case "antiragdoll":
                    return antiragdoll;
                case "add4players":
                    return add4players;
                case "infiniteBattery":
                    return infiniteBattery;
                case "infinitecameratime":
                    return infinitecameratime;
                default:
                    throw new ArgumentException("Invalid module name: " + moduleName);
            }
        }
        public static void RunModules()
        {

            originalplayer = Player.localPlayer;
            if (timeSinceLastUpdate >= updateInterval2)
            {
                shocksticks = GameObject.FindObjectsOfType<ShockStickTrigger>();
                breadcrumbs2 = GameObject.FindObjectOfType<Breadcrumbs>();
                players = GameObject.FindObjectsOfType<Player>();
                timeSinceLastUpdate = 0f;
            }
            timeSinceLastUpdate += Time.deltaTime;
            if (players == null || players.Length == 0)
            {
                players = GameObject.FindObjectsOfType<Player>();
            }

            foreach (Player player in players)
            {
                if (infHeal)
                    player.data.health = 100f;
                if (infOxy)
                    player.data.remainingOxygen = 500f;
                if (infStam)
                    player.data.currentStamina = 100f;
                if (preventDeath)
                    player.data.dead = false;
                if (infJump)
                {
                    player.data.sinceGrounded = 0.4f;
                    player.data.sinceJump = 0.7f;
                }
                
            }
                if (breadCrumbs)
                {
                    //
                    if (breadcrumbs2 != null)
                    {
                        breadcrumbs2.Update();
                    }
                    else
                    {
                        MelonLogger.Msg("Impossible de trouver l'object Breadcrumbs, création d'une nouvelle instance.");
                        GameObject newGameObject = new GameObject("Breadcrumbs");
                        breadcrumbs2 = newGameObject.AddComponent<Breadcrumbs>();
                        breadcrumbs2.Update();
                    }
                }
                if (infinitesshockstick)
                {



                    foreach (ShockStickTrigger shockstick in shocksticks)
                    {
                        foreach (Player player1 in TestMod.PlayerControllers)
                        {
                            if (player1 == null)
                            {
                                //MelonLogger.Msg("Player is null");
                            }
                            else if (player1.refs == null)
                            {
                                //MelonLogger.Msg("Player.refs is null");
                            }
                            else if (player1.refs.view == null)
                            {
                                //MelonLogger.Msg("Player.refs.view is null");
                            }
                            else if (player1.refs.view.Controller == null)
                            {
                                //MelonLogger.Msg("Player.refs.view.Controller is null");
                            }
                            else
                            {
                                if (!player1.ai)
                                {
                                    if (player1.refs.view.Controller.ToString() == originalplayer.refs.view.Controller.ToString())
                                    {
                                        // Add originalplayer to the ignored players list if it's not already there
                                        if (!shockstick.ignoredPlayers.Contains(player1))
                                        {
                                            shockstick.ignoredPlayers.Add(player1);
                                            MelonLogger.Msg("Added " + player1.refs.view.Controller.ToString() + " to the ignored list");
                                        }
                                    }
                                    else
                                    {
                                        // Remove other players from the ignored players list
                                        if (shockstick.ignoredPlayers.Contains(player1))
                                        {
                                            shockstick.ignoredPlayers.Remove(player1);
                                            MelonLogger.Msg("Removed " + player1.refs.view.Controller.ToString() + " from the ignored list");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (antiragdoll)
                {
                    Player.localPlayer.refs.ragdoll.force = 0f;
                    Player.localPlayer.refs.ragdoll.torque = 0f;


                }
                if (infiniteBattery)
                {
                    Battery.Update();
                }
                if (add4players)
                {
                    Steamworks.SteamGameServer.SetMaxPlayerCount(8);
                    for (int i = 0; i < 5; i++)
                    {
                        // Créez une nouvelle instance de Player
                        Player fakePlayer = new Player();

                        // Définissez les propriétés du joueur fictif
                        // Note : Vous devrez adapter ce code à la structure exacte de votre classe Player
                        fakePlayer.name = "FakePlayer" + i;
                        fakePlayer.data.health = 100f;
                        fakePlayer.data.remainingOxygen = 500f;
                        fakePlayer.data.currentStamina = 100f;

                        PlayerHandler playerHandler = GameObject.FindObjectOfType<PlayerHandler>();

                        // Assurez-vous que PlayerHandler existe
                        if (playerHandler != null)
                        {
                            // Ajoutez le joueur fictif à la liste des joueurs du jeu
                            playerHandler.AddPlayer(fakePlayer);
                        }
                        else
                        {
                            MelonLogger.Msg("Impossible de trouver PlayerHandler");
                        }
                    }
                }
            
            lastUpdateTime = Time.time;
            updatetime3 += Time.deltaTime;

            if (speed != 2.3)
            {
                if (updatetime3 >= updateInterval3)
                {
                    playerControllers = GameObject.FindObjectsOfType<PlayerController>();
                    updatetime3 = 0f;
                }
                    
                if (playerControllers == null || playerControllers.Length == 0)
                {
                    playerControllers = GameObject.FindObjectsOfType<PlayerController>();
                }

                foreach (PlayerController playercon in playerControllers)
                {
                    playercon.sprintMultiplier = speed;
                }
            }
            if (killAll)
            {
                if (botHandlers == null || botHandlers.Length == 0)
                {
                    botHandlers = GameObject.FindObjectsOfType<BotHandler>();
                }

                foreach (BotHandler botHandler in botHandlers)
                {
                    botHandler.DestroyAll();
                }

                if (bots == null || bots.Length == 0)
                {
                    bots = GameObject.FindObjectsOfType<Bot>();
                }

                foreach (Bot bot in bots)
                {
                    bot.Destroy();
                }

                killAll = false;
            }

            if (goodLight)
            {
                c_light = new UnityEngine.GameObject();
                c_light.AddComponent<Fullbright>();
                goodLight = false;
            }

            if (delRay)
            {
                c_ray = new UnityEngine.GameObject();
                c_ray.AddComponent<DeleteRay>();
                delRay = false;
            }

            if (customFOV)
            {
                SetFOV(cusFOVv, Camera.main);
            }

            if (ignoreWebs)
            {
                if (webs == null || webs.Length == 0)
                {
                    webs = GameObject.FindObjectsOfType<Web>();
                }

                foreach (Web web in webs)
                {
                    web.wholeBodyFactor = 0f;
                    web.distanceFactor = 0f;
                    web.drag = 0f;
                    web.force = 0f;
                }
            }

            if (duplicator)
            {
                Duplicator.DuplicateItems();
                duplicator = !duplicator;
            }

            if (duplicateItems)
            {
                Duplicator.DuplicateItems();
            }

            if (money)
            {
                Money.AddMoneyToPlayer(int.Parse(moneyfield));
                money = !money;
            }

            if (infinitecameratime)
            {
                Battery.Update2();
            }

            if (respawn)
            {
                Player.localPlayer.CallRevive();
                respawn = !respawn;
                MelonLogger.Msg("Respawned");
            }
        }

        public static void SetFOV(float newFOV, Camera cam)
        {
            newFOV = Mathf.Clamp(newFOV, 1f, 179f);
            cam.fieldOfView = newFOV;
        }
    }
}
