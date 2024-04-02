using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zorro.Core;

namespace TestMod
{
    public static class Modules
    {
        public static string AinputText = ""; // Text input from the user



        public static bool infHeal = false;
        public static bool infOxy = false;
        public static bool infStam = false;

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
            }






            

        }

        public static void EquipItem(Item item)
        {
            MelonLogger.Msg("Equip item: " + item.name);
            PlayerInventory playerInventory;
            Player.localPlayer.TryGetInventory(out playerInventory);
            playerInventory.TryAddItem(new ItemDescriptor(item, new ItemInstanceData(Guid.NewGuid())));
        }

        public static bool TryGetItemFromID(byte id, out Item item)
        {
            foreach (Item item2 in SingletonAsset<ItemDatabase>.Instance.Objects)
            {
                if (item2.id == id)
                {
                    item = item2;
                    return true;
                }
            }
            item = null;
            return false;
        }
    }
}
