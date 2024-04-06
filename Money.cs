using HarmonyLib;
using Photon.Pun;
using System;
using System.Reflection;

namespace TestMod
{
    [HarmonyPatch(typeof(ShoppingCart), "AddItemToCart")]
    public static class AddItemToCartPatch
    {
        public static bool Prefix(ShoppingCart __instance, ShopItem itemToAdd)
        {
            __instance.Cart.Add(itemToAdd);
            return false;
        }
    }
    [HarmonyPatch(typeof(RoomStatsHolder), "CanAfford")]
    public static class CanAffordPatch
    {
        public static void Postfix(ref bool __result)
        {
            __result = true;
        }
    }
    public static class Money
    {
        public static void AddMoneyToPlayer(int money)
        {
            RoomStatsHolder roomStatsHolder = SurfaceNetworkHandler.RoomStats;
            roomStatsHolder.AddMoney(money);
        }
    }
    [HarmonyPatch(typeof(ShopHandler), "OnAddToCartItemClicked")]
    public static class OnAddToCartItemClickedPatch
    {
        public static bool Prefix(ShopHandler __instance, byte itemID)
        {
            ShopItem shopItem = default(ShopItem);
            var m_ShoppingCart = (ShoppingCart)__instance.GetType().GetField("m_ShoppingCart", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
            var m_PhotonView = (PhotonView)__instance.GetType().GetField("m_PhotonView", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);

            if (__instance.TryGetShopItem(itemID, ref shopItem))
            {
                int cost = m_ShoppingCart.CartValue + shopItem.Price;
            }

            m_PhotonView.RPC("RPCA_AddItemToCart", RpcTarget.All, new object[] { itemID });
            __instance.addSFX.Play(__instance.transform.position, false, 1f, null);
            return false;
        }
    }
    [HarmonyPatch(typeof(ShopHandler), "OnOrderCartClicked")]
    public static class OnOrderCartClickedPatch
    {
        public static bool Prefix(ShopHandler __instance)
        {
            var m_ShoppingCart = (ShoppingCart)__instance.GetType().GetField("m_ShoppingCart", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
            var m_PhotonView = (PhotonView)__instance.GetType().GetField("m_PhotonView", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);

            var buyItemMethod = __instance.GetType().GetMethod("BuyItem", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(ShoppingCart) }, null);
            if (buyItemMethod == null)
            {
                return false;
            }

            try
            {
                buyItemMethod.Invoke(__instance, new object[] { m_ShoppingCart });
            }
            catch (Exception)
            {
                return false;
            }

            __instance.purchaseSFX.Play(__instance.transform.position, false, 1f, null);
            m_PhotonView.RPC("RPCM_RequestShopAction", RpcTarget.MasterClient, new object[] { 4 });
            return false;
        }
    }
}
