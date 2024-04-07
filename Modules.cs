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
namespace TestMod
{
    public static class Modules
    {
        public static bool infHeal = false, infOxy = false, infStam = false, breadCrumbs = false, duplicator = false, ShopLifter = false;
        public static bool teamESP = false, mobESP = false, mobTracer = false, itemESP = false, divingBox = false;
        public static bool killAll = false;
        public static float speed = 2.3f;
        public static bool infJump = false, preventDeath = false, goodLight = false;
        public static bool playerw = true, espw = false, worldw = false, miscw = false, enemyw = false;
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

        public static void RunModules()
        {

            originalplayer = Player.localPlayer;
            if (Time.time - lastUpdateTime >= updateInterval)
            {
                foreach (Player player in GameObject.FindObjectsOfType<Player>())
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
                    if (breadCrumbs)
                    {
                        Breadcrumbs breadcrumbs2 = GameObject.FindObjectOfType<Breadcrumbs>();
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
                        
                        shocksticks = GameObject.FindObjectsOfType<ShockStickTrigger>();

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
                    if(antiragdoll)
                    {
                        Player.localPlayer.refs.ragdoll.force = 0f;
                        Player.localPlayer.refs.ragdoll.torque = 0f;
                        
                        
                    }
                    if(add4players)
                    { Steamworks.SteamGameServer.SetMaxPlayerCount(8);
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
                }
                lastUpdateTime = Time.time;
            }
            if (speed != 2.3)
            {
                foreach (PlayerController playercon in GameObject.FindObjectsOfType<PlayerController>())
                {
                    playercon.sprintMultiplier = speed;
                }
            }
            if (killAll)
            {
                foreach (BotHandler botHandler in GameObject.FindObjectsOfType<BotHandler>())
                {
                    botHandler.DestroyAll();
                }
                foreach (Bot bot in GameObject.FindObjectsOfType<Bot>())
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
                foreach (Web web in GameObject.FindObjectsOfType<Web>())
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
