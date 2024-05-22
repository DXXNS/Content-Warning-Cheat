using System;
using UnityEngine;
using DefaultNamespace;
using MelonLoader;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Linq;
using TestMod;
using static UnityEngine.EventSystems.EventTrigger;
namespace TestMod
{
    public class ESP
    {
        static Color lightBlue = new Color(0.0f, 1.0f, 1.0f); // R = 0, G = 1, B = 1
        public static Color limeColor = new Color(0.0f, 1.0f, 0.0f); // R = 0, G = 1, B = 0

        private static float timeSinceLastUpdate = 0.0f;
        private static float updateInterval = 2f; // Update 60 times per second

        private static Player[] players;
        private static Bot[] bots;
        private static UseDivingBellButton[] divingBells;
        private static ItemInstance[] items;
        public static UnityEngine.Camera cam;
        public static List<Player> PlayerControllers = new List<Player>();

        public static void Update()
        {
            timeSinceLastUpdate += Time.deltaTime;

            if (timeSinceLastUpdate >= updateInterval)
            {
                players = GameObject.FindObjectsOfType<Player>();
                PlayerControllers = Resources.FindObjectsOfTypeAll<Player>().ToList();
                bots = GameObject.FindObjectsOfType<Bot>();
                divingBells = GameObject.FindObjectsOfType<UseDivingBellButton>();
                items = GameObject.FindObjectsOfType<ItemInstance>();
                cam = UnityEngine.Camera.main;


                timeSinceLastUpdate = 0f;
            }
            RunESP();
        }
        public static float distance;
        public static void RunESP()
        {
            try
            {
                if (Modules.teamESP)
                {
                    //Weirdly gets the spiders pos too....
                    foreach (Player player in PlayerControllers)
                    {


                        if (player != null && player.transform != null && !player.ai && cam != null)
                        {
                            Vector3? enemyBottom = null;
                            try
                            {
                                enemyBottom = player.HeadPosition();
                            }
                            catch (NullReferenceException)
                            {
                                continue;
                            }
                            if (enemyBottom == null || enemyBottom.Value == null)
                            {
                                return;
                            }

                            Vector3 w2s = cam.WorldToScreenPoint(enemyBottom.Value);
                            Vector3 enemyTop = enemyBottom.Value;
                            enemyTop.y += 2f;
                            Vector3 worldToScreenBottom = cam.WorldToScreenPoint(enemyBottom.Value);
                            Vector3 worldToScreenTop = cam.WorldToScreenPoint(enemyTop);

                            if (player.IsLocal)
                                continue;



                            if (ESPUtils.IsOnScreen(w2s))
                            {

                                float height = Mathf.Abs(worldToScreenTop.y - worldToScreenBottom.y);
                                float x = w2s.x - height * 0.3f;
                                float y = Screen.height - worldToScreenTop.y;


                                Vector2 namePosition = new Vector2(w2s.x, UnityEngine.Screen.height - w2s.y + 8f);
                                Vector2 hpPosition = new Vector2(x + (height / 2f) + 3f, y + 1f);

                                try
                                {
                                    namePosition -= new Vector2(player.HeadPosition().x - player.HeadPosition().x, 0f);
                                    hpPosition -= new Vector2(player.HeadPosition().x - player.HeadPosition().x, 0f);

                                    float distance = Vector3.Distance(UnityEngine.Camera.main.transform.position, player.HeadPosition());

                                }
                                catch (NullReferenceException)
                                {
                                    return;
                                }

                                int fontSize = Mathf.Clamp(Mathf.RoundToInt(12f / distance), 10, 20);
                                if (player.ai)
                                {
                                    ESPUtils.DrawString(namePosition, player.name.Replace("(Clone)", ""), Color.red, true, fontSize, FontStyle.Bold);
                                }
                                else
                                {
                                    ESPUtils.DrawString(namePosition, player.refs.view.Controller.ToString() + "\n" + "HP: " + player.data.health, Color.green, true, fontSize, FontStyle.Bold);
                                    ESPUtils.DrawHealth(new Vector2(w2s.x, UnityEngine.Screen.height - w2s.y + 22f), player.data.health, 100f, 0.5f, true);

                                }


                            }
                        }
                    }
                    //Trying to get somehow the Enemys Position



                }

                if (Modules.mobESP)
                {
                    foreach (Bot enemy in bots)
                    {
                        if (enemy != null && enemy.transform != null)
                        {
                            //In-Game Position
                            Vector3 pivotPos = enemy.transform.position; //Pivot point NOT at the feet, at the center

                            Vector3 playerHeadPos = pivotPos; //At the head

                            //Screen Position
                            Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);

                            if (w2s_headpos.z > 0f)//
                            {
                                Render.DrawColorString(new Vector2(w2s_headpos.x, (float)Screen.height - w2s_headpos.y + 0.3f), enemy.name, Color.red, 12f);
                                if (Modules.mobTracer)
                                    Render.DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height * 2)), new Vector2(w2s_headpos.x, (float)Screen.height - w2s_headpos.y), Color.red, 2f);
                            }
                        }
                    }
                }


                if (Modules.divingBox)
                {
                    foreach (UseDivingBellButton diving in divingBells)
                    {
                        if (diving != null)
                        {
                            //In-Game Position
                            Vector3 pivotPos = diving.transform.position;
                            Vector3 playerFootPos = pivotPos; //At the feet
                            Vector3 playerHeadPos = pivotPos; //At the head
                            playerHeadPos.y += 0.2f; //At the head

                            //Screen Position
                            Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
                            Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);

                            if (w2s_footpos.z > 0f)//
                            {
                                Render.DrawColorString(new Vector2(w2s_headpos.x, (float)Screen.height - w2s_headpos.y + 1f), "Diving Bell", lightBlue, 12f);
                                DrawBox1(w2s_footpos, w2s_headpos, lightBlue);
                            }
                        }
                    }
                }
                if (Modules.itemESP)
                {
                    foreach (ItemInstance itemInstance in items)
                    {
                        if (itemInstance != null)
                        {
                            Item item = itemInstance.item;
                            Vector3 pivotPos = itemInstance.transform.position;
                            Vector3 itemPos = pivotPos;

                            Vector3 w2s_itempos = Camera.main.WorldToScreenPoint(itemPos);

                            if (w2s_itempos.z > 0f)
                            {
                                Render.DrawColorString(new Vector2(w2s_itempos.x, (float)Screen.height - w2s_itempos.y - 20f), item.name, Color.yellow, 12f);
                                Render.DrawBox(w2s_itempos.x - 10f, (float)Screen.height - w2s_itempos.y - 10f, 20f, 20f, Color.yellow, 2f);
                            }
                        }
                    }
                }
            }
            catch(NullReferenceException)
            {
                return;
            }

        }









            public static void DrawBoxESP(Vector3 footpos, Vector3 headpos, Color color) //Rendering the ESP
        {
            float height = headpos.y - footpos.y - 5f;
            float widthOffset = 2.5f;
            float width = height / widthOffset - 5f;

            //ESP BOX


            Render.DrawBox(footpos.x - (width / 2), (float)Screen.height - footpos.y - height, width, height, color, 2f);

        }
        public static void DrawBox1(Vector3 footpos, Vector3 headpos, Color color) //Rendering the ESP
        {
            float height = headpos.y - footpos.y;
            float widthOffset = 1f;
            float width = height / widthOffset;

            //ESP BOX


            Render.DrawBox(footpos.x - (width / 2), (float)Screen.height - footpos.y - height, width, height, color, 2f);

        }


    }
}
