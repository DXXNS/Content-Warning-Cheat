using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMod
{
    internal class Enemy
    {
        public static void AddIgnore(Player player, Bot monster)
        {
            if (player == null) return;
            monster.ignoredPlayers.Add(player);
        }
        public static void RemoveIgnore(Player player, Bot monster) 
        { 
            if (player == null) return;
            monster.ignoredPlayers.Remove(player);
        }
        public static void AllIgnore(Player player)
        {
            if (player == null) return;
            Bot[] monsters = UnityEngine.Object.FindObjectsOfType<Bot>();
            foreach (Bot monster in monsters)
            {
                monster.ignoredPlayers.Add(player);
            }
        }
    }
}
