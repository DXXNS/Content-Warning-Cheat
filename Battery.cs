using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HasSpaceTest;
using UnityEngine;
using Zorro.UI;
using System.Reflection;
using MelonLoader;

namespace TestMod
{
    internal class Battery
    {
        private static float timeSinceLastUpdate = 0.0f;
        private static float updateInterval = 1f; // Update 60 times per second
        private static List<Flashlight> flashlights; // Make flashlights static
        private static List<Defib> defibs; // Add list for Defib objects
        private static List<ShockStick> shockSticks; // Add list for ShockStick objects
        private static List<Flare> flares; // Add list for Flare objects
        private static List<VideoCamera> videoCameras; // Add list for VideoCamera objects
        private static float timeSinceLastUpdate2 = 0.0f;
        private static float updateInterval3 = 1f; // Update 60 times per second

        public static void Update()
        {
            timeSinceLastUpdate += Time.deltaTime;
            

            if (timeSinceLastUpdate >= updateInterval)
            {
                flashlights = GameObject.FindObjectsOfType<Flashlight>().ToList();
                defibs = GameObject.FindObjectsOfType<Defib>().ToList(); // Find all Defib objects
                shockSticks = GameObject.FindObjectsOfType<ShockStick>().ToList(); // Find all ShockStick objects
                flares = GameObject.FindObjectsOfType<Flare>().ToList(); // Find all Flare objects

                timeSinceLastUpdate = 0f;
            }

            RunBattery();
            RunFlareLifeTime();

        }
        public static void Update2()
        {             timeSinceLastUpdate2 += Time.deltaTime;
            if (timeSinceLastUpdate2 >= updateInterval3)
            {
                videoCameras = GameObject.FindObjectsOfType<VideoCamera>().ToList(); // Find all VideoCamera objects
                timeSinceLastUpdate2 = 0f;
            }
            RunVideoCameraTime();

        }
        public static void RunVideoCameraTime()
        {
            if (videoCameras == null)
            {
                //MelonLogger.Error("VideoCamera list is null");
                return;
            }

            foreach (var videoCamera in videoCameras)
            {
                if (videoCamera == null)
                {
                    //MelonLogger.Error("videoCamera is null");
                    continue;
                }

                // Get the 'VideoInfoEntry' field from the VideoCamera
                FieldInfo videoInfoEntryField = videoCamera.GetType().GetField("m_recorderInfoEntry", BindingFlags.NonPublic | BindingFlags.Instance);
                if (videoInfoEntryField == null)
                {
                    //MelonLogger.Error("VideoInfoEntry field not found");
                    continue;
                }

                object videoInfoEntry = videoInfoEntryField.GetValue(videoCamera);
                if (videoInfoEntry == null)
                {
                    //MelonLogger.Error("videoInfoEntry is null");
                    continue;
                }

                // Get the 'timeLeft' field
                FieldInfo timeLeftField = videoInfoEntry.GetType().GetField("timeLeft", BindingFlags.Public | BindingFlags.Instance);
                if (timeLeftField == null)
                {
                    //MelonLogger.Error("timeLeft field not found");
                    continue;
                }

                // Set 'timeLeft' to 100f
                try
                {
                    timeLeftField.SetValue(videoInfoEntry, 100f);
                }
                catch (Exception ex)
                {
                    //MelonLogger.Error($"Failed to set timeLeft: {ex}");
                }
            }
        }

        public static void RunBattery()
        {
            // Run for Flashlight objects
            RunBatteryForType(flashlights, typeof(Flashlight));

            // Run for Defib objects
            RunBatteryForType(defibs, typeof(Defib));

            // Run for ShockStick objects
            RunBatteryForType(shockSticks, typeof(ShockStick));
        }

        private static void RunBatteryForType<T>(List<T> objects, Type type)
        {
            if (objects == null)
            {
                //Debug.LogError($"{type.Name} list is null");
                return;
            }

            foreach (T obj in objects)
            {
                // Get the BatteryEntry field
                FieldInfo batteryEntryField = type.GetField("m_batteryEntry", BindingFlags.NonPublic | BindingFlags.Instance);

                if (batteryEntryField == null)
                {
                    //Debug.LogError("m_batteryEntry field not found in " + type.Name + " class");
                    continue;
                }

                // Get the value of the BatteryEntry field
                BatteryEntry batteryEntry = (BatteryEntry)batteryEntryField.GetValue(obj);

                if (batteryEntry == null)
                {
                    //Debug.LogError("m_batteryEntry field is null in " + type.Name + " object");
                    continue;
                }

                // Now you can use batteryEntry
                batteryEntry.AddCharge(100);
            }
        }

        public static void RunFlareLifeTime()
        {
            if (flares == null)
            {
                //Debug.LogError("Flare list is null");
                return;
            }

            foreach (Flare flare in flares)
            {
                // Get the LifeTimeEntry field
                FieldInfo lifeTimeEntryField = typeof(Flare).GetField("m_lifeTimeEntry", BindingFlags.NonPublic | BindingFlags.Instance);

                if (lifeTimeEntryField == null)
                {
                    //Debug.LogError("m_lifeTimeEntry field not found in Flare class");
                    continue;
                }

                // Get the value of the LifeTimeEntry field
                LifeTimeEntry lifeTimeEntry = (LifeTimeEntry)lifeTimeEntryField.GetValue(flare);

                if (lifeTimeEntry == null)
                {
                    //Debug.LogError("m_lifeTimeEntry field is null in Flare object");
                    continue;
                }

                // Now you can use lifeTimeEntry
                lifeTimeEntry.AddLifeTime(100);
            }
        }
    }
}
