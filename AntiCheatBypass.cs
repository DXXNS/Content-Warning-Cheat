using HarmonyLib;
using Photon.Pun;
using System;
using System.Reflection;
using Lockdown.Patches;
public class Unpatcher
{
    public static void UnpatchInterceptInstantiatePrefix()
    {
        var harmony = new HarmonyLib.Harmony("Lockdown"); // Remplacez par l'ID de votre instance Harmony

        var originalMethod = typeof(PhotonNetwork).GetMethod("NetworkInstantiate", new Type[]
        {
            typeof(InstantiateParameters),
            typeof(bool),
            typeof(bool)
        });

        var prefix = typeof(PhotonNetworkPatch).GetMethod("InterceptInstantiatePrefix");

        harmony.Unpatch(originalMethod, HarmonyPatchType.Prefix, harmony.Id);
        //Send a debug message to the console
        Console.WriteLine("Unpatched InterceptInstantiatePrefix");
    }
}
