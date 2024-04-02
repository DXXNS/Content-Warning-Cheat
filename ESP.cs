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

namespace TestMod
{
    public class ESP
    {
        static Color lightBlue = new Color(0.0f, 1.0f, 1.0f); // R = 0, G = 1, B = 1

        public static Color limeColor = new Color(0.0f, 1.0f, 0.0f); // R = 0, G = 1, B = 0



        
        public static GameObject[] botTypes;


        public static void EnemyESP()
        {
            if (true)
            {
                foreach (PlayerController enemy in GameObject.FindObjectsOfType<PlayerController>())
                {



                    //In-Game Position
                    Vector3 pivotPos = enemy.transform.position; //Pivot point NOT at the feet, at the center
                    Vector3 playerFootPos; playerFootPos.x = pivotPos.x; playerFootPos.z = pivotPos.z; playerFootPos.y = pivotPos.y; //At the feet- 0.5f
                    Vector3 playerHeadPos; playerHeadPos.x = pivotPos.x; playerHeadPos.z = pivotPos.z; playerHeadPos.y = pivotPos.y; //At the head+ 1.8f

                    //Screen Position
                    Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
                    Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);






                    if (w2s_footpos.z > 0f)//
                    {
                        Render.DrawColorString(new Vector2(w2s_headpos.x, (float)Screen.height - w2s_headpos.y + 0.2f), enemy.name, limeColor, 15f);
                        DrawBoxESP(w2s_footpos, w2s_headpos, limeColor);

                        Render.DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height * 2)), new Vector2(w2s_footpos.x, (float)Screen.height - w2s_footpos.y), limeColor, 1f);




                    }
                }
            }

            foreach (ItemInstance item in GameObject.FindObjectsOfType<ItemInstance>())
            {



                //In-Game Position
                Vector3 pivotPos = item.transform.position; //Pivot point NOT at the feet, at the center
                Vector3 playerFootPos; playerFootPos.x = pivotPos.x; playerFootPos.z = pivotPos.z; playerFootPos.y = pivotPos.y; //At the feet- 0.5f
                Vector3 playerHeadPos; playerHeadPos.x = pivotPos.x; playerHeadPos.z = pivotPos.z; playerHeadPos.y = pivotPos.y; //At the head+ 1.8f

                //Screen Position
                Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
                Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);
                if (w2s_footpos.z > 0f)//
                {
                    Render.DrawColorString(new Vector2(w2s_headpos.x, (float)Screen.height - w2s_headpos.y + 0.2f), item.name, lightBlue, 15f);
                    Render.DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height * 2)), new Vector2(w2s_footpos.x, (float)Screen.height - w2s_footpos.y), lightBlue, 1f);
                }
            }
            foreach (ItemHugger item in GameObject.FindObjectsOfType<ItemHugger>())
            {



                //In-Game Position
                Vector3 pivotPos = item.transform.position; //Pivot point NOT at the feet, at the center
                Vector3 playerFootPos; playerFootPos.x = pivotPos.x; playerFootPos.z = pivotPos.z; playerFootPos.y = pivotPos.y; //At the feet- 0.5f
                Vector3 playerHeadPos; playerHeadPos.x = pivotPos.x; playerHeadPos.z = pivotPos.z; playerHeadPos.y = pivotPos.y; //At the head+ 1.8f

                //Screen Position
                Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
                Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);
                if (w2s_footpos.z > 0f)//
                {
                    Render.DrawColorString(new Vector2(w2s_headpos.x, (float)Screen.height - w2s_headpos.y + 0.2f), item.name, lightBlue, 15f);
                    Render.DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height * 2)), new Vector2(w2s_footpos.x, (float)Screen.height - w2s_footpos.y), lightBlue, 1f);
                }
            }
            foreach (ItemGooBall item in GameObject.FindObjectsOfType<ItemGooBall>())
            {



                //In-Game Position
                Vector3 pivotPos = item.transform.position; //Pivot point NOT at the feet, at the center
                Vector3 playerFootPos; playerFootPos.x = pivotPos.x; playerFootPos.z = pivotPos.z; playerFootPos.y = pivotPos.y; //At the feet- 0.5f
                Vector3 playerHeadPos; playerHeadPos.x = pivotPos.x; playerHeadPos.z = pivotPos.z; playerHeadPos.y = pivotPos.y; //At the head+ 1.8f

                //Screen Position
                Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
                Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);
                if (w2s_footpos.z > 0f)//
                {
                    Render.DrawColorString(new Vector2(w2s_headpos.x, (float)Screen.height - w2s_headpos.y + 0.2f), item.name, lightBlue, 15f);
                    Render.DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height * 2)), new Vector2(w2s_footpos.x, (float)Screen.height - w2s_footpos.y), lightBlue, 1f);
                }
            }
            
        }


        public static void RenderNewESP(Vector3 pivotPos, string name)
        {
            //In-Game Position
            
            Vector3 playerFootPos; playerFootPos.x = pivotPos.x; playerFootPos.z = pivotPos.z; playerFootPos.y = pivotPos.y; //At the feet- 0.5f
            Vector3 playerHeadPos; playerHeadPos.x = pivotPos.x; playerHeadPos.z = pivotPos.z; playerHeadPos.y = pivotPos.y; //At the head+ 1.8f

            //Screen Position
            Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
            Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);
            if (w2s_footpos.z > 0f)//
            {
                Render.DrawColorString(new Vector2(w2s_headpos.x, (float)Screen.height - w2s_headpos.y + 0.2f), name, Color.red, 15f);
                //Render.DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height * 2)), new Vector2(w2s_footpos.x, (float)Screen.height - w2s_footpos.y), Color.red, 1f);
            }

        }






        public static void DrawBoxESP(Vector3 footpos, Vector3 headpos, Color color) //Rendering the ESP
        {
            float height = headpos.y - footpos.y - 5f;
            float widthOffset = 2.5f;
            float width = height / widthOffset - 5f;

            //ESP BOX


            Render.DrawBox(footpos.x - (width / 2), (float)Screen.height - footpos.y - height, width, height, color, 1f);

        }


    }
}
