using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestMod
{
    public static class Watermark
    {
        private static readonly System.Collections.Generic.List<string> activeConditions = new System.Collections.Generic.List<string>();
        public static void Call()
        {

            GUIStyle style = new GUIStyle(GUI.skin.label);

            style.fontSize = 14;
            style.alignment = TextAnchor.UpperCenter;
            style.normal.textColor = Color.blue;

            GUI.Label(new Rect(0, 0, Screen.width, 100), "Content Warning by .dxxns", style);
            float fps = 1.0f / Time.deltaTime;
            string fpsText = "FPS: " + Mathf.RoundToInt(fps);

            Rect rect = new Rect(0, 0 + 20, Screen.width, 100); // Adjust height as needed

            GUI.Label(rect, fpsText, style);
            Rect rect1 = new Rect(0, 0 + 40, Screen.width, 100); // Adjust height as needed
            UpdateUI();



        }
        public static void UpdateUI()
        {
            activeConditions.Clear(); // Clear the list before updating it

            // Check conditions and add to the list if true
            if (Modules.infHeal)
                activeConditions.Add("Infinite Heal");
            if (Modules.infOxy)
                activeConditions.Add("Infinite Oxygen");
            if (Modules.infStam)
                activeConditions.Add("Infinite Stamina");

            if (Modules.teamESP)
                activeConditions.Add("Team ESP");
            if (Modules.mobESP)
                activeConditions.Add("Mob ESP");
            if (Modules.mobTracer)
                activeConditions.Add("Mob Tracer");
            if (Modules.divingBox)
                activeConditions.Add("Diving Box");

            if (Modules.killAll)
                activeConditions.Add("Kill All");

            if (Modules.infJump)
                activeConditions.Add("Infinite Jump");
            if (Modules.preventDeath)
                activeConditions.Add("Prevent Death");

            if (Modules.goodLight)
                activeConditions.Add("Good Light");

            // Display the active conditions on the screen
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 14;
            style.alignment = TextAnchor.UpperLeft;
            style.normal.textColor = Color.blue;

            float yPos = 0;
            foreach (string condition in activeConditions)
            {
                Rect rect = new Rect(0, yPos, 200, 20);
                GUI.Label(rect, condition, style);
                yPos += 20;
            }
        }

    }
}
