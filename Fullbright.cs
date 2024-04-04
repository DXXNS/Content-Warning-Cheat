using JetBrains.Annotations;
using MelonLoader;
using UnityEngine;
using UnityEngine.UIElements;

namespace TestMod
{
    public class Fullbright : MonoBehaviour
    {
        public KeyCode lightSpawnKey = KeyCode.O; // Change this to any key you want
        public Color lightColor = Color.white; // Customize the default light color
        public float lightIntensity = 100.5f; // Customize the default light intensity
        public float lightRange = 200.0f; // Customize the default light range
        public float spotAngle = 150.0f; // Customize the default spot angle
        public GameObject ligt;
        public void Update()
        {
            Camera activeCamera = Camera.main;
            // Check if the specified key is pressed
            
            if (ligt != null)
            {
                ligt.transform.position = activeCamera.transform.position;
                ligt.transform.rotation = activeCamera.transform.rotation;
            }

            if (Input.GetKeyDown(lightSpawnKey))
            {
                // Get the active camera
                // You may need a more robust method to find the active camera if you have multiple cameras in the scene

                // Check if there is an active camera
                if (activeCamera != null)
                {
                    if (ligt == null)
                    {
                        ligt = new GameObject("SpotLight");
                        ligt.transform.position = activeCamera.transform.position;

                        // Attach the light component to the new GameObject
                        Light newLight = ligt.AddComponent<Light>();
                        newLight.type = LightType.Spot; // Set light type to spot
                        newLight.color = lightColor; // Set light color
                        newLight.intensity = lightIntensity; // Set light intensity
                        newLight.range = lightRange; // Set light range
                        newLight.spotAngle = spotAngle; // Set spot angle

                        MelonLogger.Msg("Light created");
                    }
                    else
                        MelonLogger.Msg("Light Already active");
                }
                else
                {
                    MelonLogger.Msg("No active camera found!");
                }
            }

        }
        public Rect windowRect = new Rect(20, 20, 200, 200);
        public void OnGUI()
        {
            Render.DrawString(new Vector2(200, 20), "Fullbright");
            
        }

        
    }
}