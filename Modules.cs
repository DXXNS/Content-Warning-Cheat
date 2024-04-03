using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zorro.Core;

namespace TestMod
{
    public static class Modules
    {
        //Exploits
        public static bool infHeal = false;
        public static bool infOxy = false;
        public static bool infStam = false;


        //ESP
        public static bool teamESP = false;
        public static bool mobESP = false;
        public static bool mobTracer = false;
        public static bool divingBox = false;

        //Monster edit
        public static bool killAll = false;


        //Player
        public static float speed = 2.3f;
        public static bool infJump = false;
        public static bool preventDeath = false;

        //Light
        public static bool goodLight = false;



        //Window pages
        public static bool playerw = true;
        public static bool espw = false;
        public static bool worldw = false;
        public static bool miscw = false;


        public static void RunModules()
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

            }
            if (speed != 2.3)
            {
                foreach (PlayerController playercon in GameObject.FindObjectsOfType<PlayerController>())
                {
                    playercon.sprintMultiplier = speed;
                    //2.3f is standart

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


                //BombItem found in files make a esp
                //Bot weeping is with capcha

            }
        }
    }
}
