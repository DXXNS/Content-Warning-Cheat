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
        public static bool spawnDropdownOpen = false, dropdownOpenMonster = false, respawn = false, money = false, shopLifter = true, hasLifted = true, dropdownOpen = false, duplicateItems = false, dropdownOpenEnemy = false, dropdownOpenPlayer = false;
        public static string moneyfield = "1000";
        public static Vector2 spawnScrollPosition = Vector2.zero, scrollPositionMonster = Vector2.zero, scrollPosition = Vector2.zero, scrollPositionEnemy = Vector2.zero, scrollPositionPlayer = Vector2.zero;
        public static bool menuToggle { get; set; }
        public static bool OldCursorVisible { get; set; }
        public static CursorLockMode OldCursorLockMode { get; set; }
        public static GameObject c_light, c_ray;
        private static float lastUpdateTime = 0f, updateInterval = 1f / 30f;

        public static void RunModules()
        {
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
