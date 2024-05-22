using System;
using TestMod;
using UnityEngine;
using Zorro.Core.CLI;
using MelonLoader;
using System.Linq;
using Steamworks;
public class Duplicator
{
    public static void DuplicateItems()
    {
        if (Modules.duplicator)
        {
            ItemInstance[] items = GameObject.FindObjectsOfType<ItemInstance>();


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

    public static void SpawnItem(string itemName, int amount)
    {
        // Trouver l'item dans la base de données d'items
        var item = ItemDatabase.Instance.lastLoadedItems.FirstOrDefault(i => i.name == itemName);
        if (item != null && amount != 0)
        {
            for (double i = 0; i < amount; i += 1)
            {
                Vector3 debugItemSpawnPos = MainCamera.instance.GetDebugItemSpawnPos();
                Player.localPlayer.RequestCreatePickup(item, new ItemInstanceData(Guid.NewGuid()), debugItemSpawnPos, UnityEngine.Quaternion.identity);
            }
        }

    }
    
}


//Thanks to IcyRelic
public static class PlayerExtensions
{
    public static Photon.Realtime.Player PhotonPlayer(this Player player) => player.refs.view.Owner;
    public static CSteamID GetSteamID(this Player player) => player.refs.view.Owner.GetSteamID();
    public static bool IsValid(this Player player) => !player.ai; //todo figure out way to check if its one of the spammed when joining private
}
public static class PhotonPlayerExtensions
    {
        public static CSteamID GetSteamID(this Photon.Realtime.Player photonPlayer)
        {
            bool success = SteamAvatarHandler.TryGetSteamIDForPlayer(photonPlayer, out CSteamID steamid);
            return steamid;
        }
        public static Player GamePlayer(this Photon.Realtime.Player photonPlayer) => PlayerHandler.instance.players.Find(x => x.PhotonPlayer().ActorNumber == photonPlayer.ActorNumber);
    }


