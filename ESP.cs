using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine;
using DefaultNamespace;
using System;
using System.Reflection;
using MelonLoader;

namespace TestMod
{
    public class ESP
    {
        static Color lightBlue = new Color(0.0f, 1.0f, 1.0f); // R = 0, G = 1, B = 1

        public static Color limeColor = new Color(0.0f, 1.0f, 0.0f); // R = 0, G = 1, B = 0



        
        public static GameObject[] botTypes;

        private static float timeSinceLastUpdate = 0.0f;
        private static float updateInterval = 1f / 60f; // Mettre à jour 30 fois par seconde

        public static void Update()
        {
            timeSinceLastUpdate += Time.deltaTime;

            if (timeSinceLastUpdate >= updateInterval)
            {
                RunESP();
                timeSinceLastUpdate = 0f;
            }
        }

        public static void RunESP()
        {
            if (Modules.teamESP)
            {
                //Weirdly gets the spiders pos too....
                foreach (Player enemy in GameObject.FindObjectsOfType<Player>())
                {

                    
                    Vector3 pivotPos = enemy.HeadPosition();

                    //In-Game Position
                    Vector3 pivotPos1 = enemy.transform.position; //Pivot point NOT at the feet, at the center
                    Vector3 playerFootPos; playerFootPos.x = pivotPos.x; playerFootPos.z = pivotPos.z; playerFootPos.y = pivotPos.y - 1.8f; //At the feet
                    Vector3 playerHeadPos; playerHeadPos.x = pivotPos.x; playerHeadPos.z = pivotPos.z; playerHeadPos.y = pivotPos.y + 0.5f; //At the head

                    //Screen Position
                    Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
                    Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);






                    if (w2s_footpos.z > 0f && !enemy.IsLocal && enemy.name == "Player(Clone)")//
                    {
                        //Render.DrawColorString(new Vector2(w2s_headpos.x, (float)Screen.height - w2s_headpos.y + 0.3f), "Teammates", limeColor, 12f);
                        DrawBoxESP(w2s_footpos, w2s_headpos, limeColor);

                        Render.DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height * 2)), new Vector2(w2s_footpos.x, (float)Screen.height - w2s_footpos.y), limeColor, 2f);




                    }
                }
                //Trying to get somehow the Enemys Position
               

                
            }

            if (Modules.mobESP)
            {
                foreach (Bot enemy in GameObject.FindObjectsOfType<Bot>())
                {




                    //In-Game Position
                    Vector3 pivotPos = enemy.transform.position; //Pivot point NOT at the feet, at the center

                    Vector3 playerHeadPos; playerHeadPos.x = pivotPos.x; playerHeadPos.z = pivotPos.z; playerHeadPos.y = pivotPos.y; //At the head

                    //Screen Position
                    ;
                    Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);






                    if (w2s_headpos.z > 0f)//
                    {
                        Render.DrawColorString(new Vector2(w2s_headpos.x, (float)Screen.height - w2s_headpos.y + 0.3f), enemy.name, Color.red, 12f);
                        if (Modules.mobTracer)
                        Render.DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height * 2)), new Vector2(w2s_headpos.x, (float)Screen.height - w2s_headpos.y), Color.red, 2f);




                    }
                }
            }


            if (Modules.divingBox)
            {

                foreach (UseDivingBellButton diving in GameObject.FindObjectsOfType<UseDivingBellButton>())
                {




                    //In-Game Position
                    Vector3 pivotPos = diving.transform.position;
                    Vector3 playerFootPos; playerFootPos.x = pivotPos.x; playerFootPos.z = pivotPos.z; playerFootPos.y = pivotPos.y - 0.3f; //At the feet
                    Vector3 playerHeadPos; playerHeadPos.x = pivotPos.x; playerHeadPos.z = pivotPos.z; playerHeadPos.y = pivotPos.y + 0.2f; //At the head

                    //Screen Position
                    Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
                    Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);






                    if (w2s_footpos.z > 0f)//
                    {
                        Render.DrawColorString(new Vector2(w2s_headpos.x, (float)Screen.height - w2s_headpos.y + 1f), "Diving Bell", lightBlue, 12f);
                        DrawBox1(w2s_footpos, w2s_headpos, lightBlue);

                        //Render.DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height * 2)), new Vector2(w2s_footpos.x, (float)Screen.height - w2s_footpos.y), rlimeColo, 2f);




                    }
                }
            }
            if (Modules.itemESP)
            {
                ItemInstance[] items = GameObject.FindObjectsOfType<ItemInstance>();

                foreach (ItemInstance itemInstance in items)
                {
                    Item item = itemInstance.item;
                    Vector3 pivotPos = itemInstance.transform.position;
                    Vector3 itemPos; itemPos.x = pivotPos.x; itemPos.z = pivotPos.z; itemPos.y = pivotPos.y;
                    Vector3 w2s_itempos = Camera.main.WorldToScreenPoint(itemPos);

                    if (w2s_itempos.z > 0f)
                    {
                        Render.DrawColorString(new Vector2(w2s_itempos.x, (float)Screen.height - w2s_itempos.y - 20f), item.name, Color.yellow, 12f);
                        Render.DrawBox(w2s_itempos.x - 10f, (float)Screen.height - w2s_itempos.y - 10f, 20f, 20f, Color.yellow, 2f);
                    }
                }
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
