
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class ShockStickTriggerh
{
    private static ShockStickTrigger[] shocksticks;
    public void RemoveAllIgnoredPlayers()
    {
        shocksticks = GameObject.FindObjectsOfType<ShockStickTrigger>();
        foreach (ShockStickTrigger shockstick in shocksticks)
        {
            shockstick.ignoredPlayers.Clear();
        }
        
    }
}
