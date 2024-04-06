using System;
using TestMod;
using UnityEngine;
using Zorro.Core.CLI;
using MelonLoader;
using System.Linq;
public class Duplicator
{
    public static void DuplicateItems()
    {
        if (Modules.duplicator)
        {
            ItemInstance[] items = GameObject.FindObjectsOfType<ItemInstance>();

            MelonLogger.Msg("Found " + items.Length + " items."); // Log the number of found items

            foreach (ItemInstance itemInstance in items)
            {
                Item item = itemInstance.item;
                Vector3 playerPos = Player.localPlayer.data.groundPos; // Get the player's position inside the loop
                playerPos.y += 1f; // Increase the vertical position to avoid items appearing in the ground

                // Duplicate the item at the player's position
                Player.localPlayer.RequestCreatePickup(item, new ItemInstanceData(Guid.NewGuid()), playerPos, Quaternion.identity);
                MelonLogger.Msg("Item duplicated: " + item.name + " at player's position: " + playerPos); // Log the name of the duplicated item and the player's position
            }
        }
        if (Modules.duplicateItems)
        {
            // Find the selected item
            ItemInstance selectedItemInstance = GameObject.FindObjectsOfType<ItemInstance>()
                .FirstOrDefault(itemInstance => itemInstance.item.name == Modules.selectedItemName);

            if (selectedItemInstance != null)
            {
                Item item = selectedItemInstance.item;
                Vector3 playerPos = Player.localPlayer.data.groundPos; // Get the player's position
                playerPos.y += 1f; // Increase the vertical position to avoid items appearing in the ground

                // Duplicate the item at the player's position
                Player.localPlayer.RequestCreatePickup(item, new ItemInstanceData(Guid.NewGuid()), playerPos, Quaternion.identity);
                MelonLogger.Msg("Item duplicated: " + item.name + " at player's position: " + playerPos); // Log the name of the duplicated item and the player's position
            }
            else
            {
                MelonLogger.Msg("Item not found: " + Modules.selectedItemName); // Log a message if the selected item is not found
            }

            Modules.duplicateItems = false; // Reset the duplicateItems flag
        }

    }

    public static void SpawnItem(string itemName)
    {
        // Trouver l'item dans la base de données d'items
        var item = ItemDatabase.Instance.lastLoadedItems.FirstOrDefault(i => i.name == itemName);

        if (item != null)
        {
            Debug.Log("Spawn item: " + item.name);
            Vector3 debugItemSpawnPos = MainCamera.instance.GetDebugItemSpawnPos();
            Player.localPlayer.RequestCreatePickup(item, new ItemInstanceData(Guid.NewGuid()), debugItemSpawnPos, UnityEngine.Quaternion.identity);
        }
        else
        {
            Debug.Log("Item not found: " + itemName);
        }
    }

}
