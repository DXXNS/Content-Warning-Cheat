
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TestMod
{
    public class ESPUtils : MonoBehaviour
    {
        static ESPUtils()
        {
            whiteTexture = Texture2D.whiteTexture;
            drawingTex = new Texture2D(1, 1);

            drawMaterial = new Material(Shader.Find("Hidden/Internal-Colored"))
            {
                hideFlags = (HideFlags)61
            };

            drawMaterial.SetInt("_SrcBlend", 5);
            drawMaterial.SetInt("_DstBlend", 10);
            drawMaterial.SetInt("_Cull", 0);
            drawMaterial.SetInt("_ZWrite", 0);
        }

        public static Color GetHealthColour(float health, float maxHealth)
        {
            Color result = Color.green;

            float percentage = health / maxHealth;

            if (percentage >= 0.75f)
            {
                result = Color.green;
            }
            else
            {
                result = Color.yellow;
            }

            if (percentage <= 0.25f)
            {
                result = Color.red;
            }

            return result;
        }
        public static GameObject[] GetGameObjectsByName(string name)
        {
            GameObject[] allObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject[] parentObjects = allObjects.Where(obj => obj.name.Contains(name)).ToArray();

            if (parentObjects.Length > 0)
            {
                return parentObjects;
            }
            else
            {
                Debug.LogError("The specified game objects name was not found in the scene.");
            }

            return null;
        }
        public static GameObject GetGameObjectComponentByName(string gameObjectName, string componentName)
        {
            GameObject gameObject = GameObject.Find(gameObjectName);
            if (gameObject != null)
            {
                Component component = gameObject.GetComponent(componentName);
                if (component != null)
                {
                    return component.gameObject;
                }
                else
                {
                    Debug.LogWarning("The specified component name was not found on the game object.");
                }
            }
            else
            {
                Debug.LogError("The specified game object name was not found in the scene.");
            }

            return null;
        }

        public static void DrawCircle(Color Col, Vector2 Center, float Radius)
        {
            GL.PushMatrix();

            if (!drawMaterial.SetPass(0))
            {
                GL.PopMatrix();
                return;
            }

            GL.Begin(1);
            GL.Color(Col);

            for (float num = 0f; num < 6.28318548f; num += 0.05f)
            {
                GL.Vertex(new Vector3(Mathf.Cos(num) * Radius + Center.x, Mathf.Sin(num) * Radius + Center.y));
                GL.Vertex(new Vector3(Mathf.Cos(num + 0.05f) * Radius + Center.x, Mathf.Sin(num + 0.05f) * Radius + Center.y));
            }

            GL.End();
            GL.PopMatrix();
        }
        public static Vector3 WorldToScreenPoint(Vector3 wp)
        {
            Vector4 vector = Camera.current.projectionMatrix * Camera.current.worldToCameraMatrix * new Vector4(wp.x, wp.y, wp.z, 1f);
            if (vector.w < 0.1f)
            {
                return Vector3.zero;
            }
            float num = 1f / vector.w;
            vector.x *= num;
            vector.y *= num;
            Vector2 vector2 = new Vector2(0.5f * (float)Camera.current.pixelWidth, 0.5f * (float)Camera.current.pixelHeight);
            vector2.x += 0.5f * vector.x * (float)Camera.current.pixelWidth + 0.5f;
            vector2.y -= 0.5f * vector.y * (float)Camera.current.pixelHeight + 0.5f;
            return new Vector3(vector2.x, vector2.y, wp.z);
        }
        public static void DrawESPLine(Vector3 point, Color color, float thickness)
        {
            Vector3 pointInScreen = Camera.main.WorldToScreenPoint(point);

            DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)), new Vector2(pointInScreen.x, (float)Screen.height - pointInScreen.y), color, thickness);
        }
        private static Color __color;
        internal static void BoxRect(Rect rect, Color color)
        {
            if (color != __color)
            {
                drawingTex.SetPixel(0, 0, color);
                drawingTex.Apply();
                __color = color;
            }

            GUI.DrawTexture(rect, drawingTex);
        }
        internal static void DrawHealth(Vector2 pos, float health, float maxHealth, float size = 1.0f, bool center = false)
        {
            if (center)
            {
                pos -= new Vector2(26f * size, 0f);
            }
            pos += new Vector2(0f, 18f * size);
            BoxRect(new Rect(pos.x, pos.y, 52f * size, 5f * size), Color.black);
            pos += new Vector2(1f * size, 1f * size);

            // Ensure health is within the bounds of 0 to maxHealth
            health = Mathf.Clamp(health, 0f, maxHealth);

            Color color = Color.green;
            if (health <= 50f)
            {
                color = Color.yellow;
            }
            if (health <= 25f)
            {
                color = Color.red;
            }

            // Calculate the percentage of health relative to maxHealth
            float healthPercentage = health / maxHealth;

            // Draw the health bar based on the percentage
            BoxRect(new Rect(pos.x, pos.y, 0.5f * healthPercentage * 100f * size, 3f * size), color);
        }
        public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
        {
            Color oldColour = GUI.color;

            var rad2deg = 360 / (Math.PI * 2);

            Vector2 d = end - start;

            float a = (float)rad2deg * Mathf.Atan(d.y / d.x);

            if (d.x < 0)
                a += 180;

            int width2 = (int)Mathf.Ceil(width / 2);

            GUIUtility.RotateAroundPivot(a, start);

            GUI.color = color;

            GUI.DrawTexture(new Rect(start.x, start.y - width2, d.magnitude, width), Texture2D.whiteTexture, ScaleMode.StretchToFill);

            GUIUtility.RotateAroundPivot(-a, start);

            GUI.color = oldColour;
        }

        public static void OutlineBox(Vector2 pos, Vector2 size, Color colour)
        {
            Color oldColour = GUI.color;
            GUI.color = colour;

            GUI.DrawTexture(new Rect(pos.x, pos.y, 1, size.y), whiteTexture);
            GUI.DrawTexture(new Rect(pos.x + size.x, pos.y, 1, size.y), whiteTexture);
            GUI.DrawTexture(new Rect(pos.x, pos.y, size.x, 1), whiteTexture);
            GUI.DrawTexture(new Rect(pos.x, pos.y + size.y, size.x, 1), whiteTexture);

            GUI.color = oldColour;
        }

        public static bool IsOnScreen(Vector3 position)
        {
            return position.y > 0.01f && position.y < Screen.height - 5f && position.z > 0.01f;
        }

        public static void CornerBox(Vector2 Head, float Width, float Height, float thickness, Color color, bool outline)
        {
            int num = (int)(Width / 4f);
            int num2 = num;

            if (outline)
            {
                RectFilled(Head.x - Width / 2f - 1f, Head.y - 1f, num + 2, 3f, Color.black);
                RectFilled(Head.x - Width / 2f - 1f, Head.y - 1f, 3f, num2 + 2, Color.black);
                RectFilled(Head.x + Width / 2f - num - 1f, Head.y - 1f, num + 2, 3f, Color.black);
                RectFilled(Head.x + Width / 2f - 1f, Head.y - 1f, 3f, num2 + 2, Color.black);
                RectFilled(Head.x - Width / 2f - 1f, Head.y + Height - 4f, num + 2, 3f, Color.black);
                RectFilled(Head.x - Width / 2f - 1f, Head.y + Height - num2 - 4f, 3f, num2 + 2, Color.black);
                RectFilled(Head.x + Width / 2f - num - 1f, Head.y + Height - 4f, num + 2, 3f, Color.black);
                RectFilled(Head.x + Width / 2f - 1f, Head.y + Height - num2 - 4f, 3f, num2 + 3, Color.black);
            }

            RectFilled(Head.x - Width / 2f, Head.y, num, 1f, color);
            RectFilled(Head.x - Width / 2f, Head.y, 1f, num2, color);
            RectFilled(Head.x + Width / 2f - num, Head.y, num, 1f, color);
            RectFilled(Head.x + Width / 2f, Head.y, 1f, num2, color);
            RectFilled(Head.x - Width / 2f, Head.y + Height - 3f, num, 1f, color);
            RectFilled(Head.x - Width / 2f, Head.y + Height - num2 - 3f, 1f, num2, color);
            RectFilled(Head.x + Width / 2f - num, Head.y + Height - 3f, num, 1f, color);
            RectFilled(Head.x + Width / 2f, Head.y + Height - num2 - 3f, 1f, num2 + 1, color);
        }

        public static void RectFilled(float x, float y, float width, float height, Color color)
        {
            if (color != lastTexColour)
            {
                drawingTex.SetPixel(0, 0, color);
                drawingTex.Apply();

                lastTexColour = color;
            }

            GUI.DrawTexture(new Rect(x, y, width, height), drawingTex);
        }
        public static void DrawBones(Transform bone1, Transform bone2, Color c)
        {
            if (!Camera.main) //fix the crash maybe
            {
                return;
            }

            if (!bone1 || !bone2)
            {
                return;
            }

            Vector3 w1 = Camera.main.WorldToScreenPoint(bone1.position);
            Vector3 w2 = Camera.main.WorldToScreenPoint(bone2.position);
            if (w1.z > 0.0f && w2.z > 0.0f)
            {
                DrawLine(new Vector2(w1.x, Screen.height - w1.y), new Vector2(w2.x, Screen.height - w2.y), c, 2f);
            }
        }

        public static void DrawAllBones(List<Transform> b, Color c)
        {
            DrawBones(b[0], b[1], c); //head, neck
            DrawBones(b[1], b[2], c); //neck, spine
            DrawBones(b[2], b[3], c); //spine, hips

            DrawBones(b[1], b[4], c); //neck, left shoulder
            DrawBones(b[4], b[5], c); //left shoulder, left upper arm
            DrawBones(b[5], b[6], c); //left upper arm, left lower arm
            DrawBones(b[6], b[7], c); //left lower arm, left hand

            DrawBones(b[1], b[8], c); //neck, right shoulder
            DrawBones(b[8], b[9], c); //right shoulder, right upper arm 
            DrawBones(b[9], b[10], c); //right upper arm, right lower arm
            DrawBones(b[10], b[11], c); //right lower arm, right hand

            DrawBones(b[3], b[12], c); //hips, left upper leg
            DrawBones(b[12], b[13], c); //left upper leg, left lower leg
            DrawBones(b[13], b[14], c); //left lower leg, left foot

            DrawBones(b[3], b[15], c); //hips, right upper leg
            DrawBones(b[15], b[16], c); //right upper leg, right lower leg
            DrawBones(b[16], b[17], c); //right lower leg, right foot
        }
        public static void Draw3DBox(Bounds b, Color color)
        {
            Vector3[] pts = new Vector3[8];
            pts[0] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
            pts[1] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
            pts[2] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
            pts[3] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
            pts[4] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
            pts[5] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
            pts[6] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
            pts[7] = Camera.main.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));

            for (int i = 0; i < pts.Length; i++) pts[i].y = Screen.height - pts[i].y;

            GL.PushMatrix();
            GL.Begin(1);
            drawMaterial.SetPass(0);
            GL.End();
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Begin(1);
            drawMaterial.SetPass(0);
            GL.Color(color);
            GL.Vertex3(pts[0].x, pts[0].y, 0f);
            GL.Vertex3(pts[1].x, pts[1].y, 0f);
            GL.Vertex3(pts[1].x, pts[1].y, 0f);
            GL.Vertex3(pts[5].x, pts[5].y, 0f);
            GL.Vertex3(pts[5].x, pts[5].y, 0f);
            GL.Vertex3(pts[4].x, pts[4].y, 0f);
            GL.Vertex3(pts[4].x, pts[4].y, 0f);
            GL.Vertex3(pts[0].x, pts[0].y, 0f);
            GL.Vertex3(pts[2].x, pts[2].y, 0f);
            GL.Vertex3(pts[3].x, pts[3].y, 0f);
            GL.Vertex3(pts[3].x, pts[3].y, 0f);
            GL.Vertex3(pts[7].x, pts[7].y, 0f);
            GL.Vertex3(pts[7].x, pts[7].y, 0f);
            GL.Vertex3(pts[6].x, pts[6].y, 0f);
            GL.Vertex3(pts[6].x, pts[6].y, 0f);
            GL.Vertex3(pts[2].x, pts[2].y, 0f);
            GL.Vertex3(pts[2].x, pts[2].y, 0f);
            GL.Vertex3(pts[0].x, pts[0].y, 0f);
            GL.Vertex3(pts[3].x, pts[3].y, 0f);
            GL.Vertex3(pts[1].x, pts[1].y, 0f);
            GL.Vertex3(pts[7].x, pts[7].y, 0f);
            GL.Vertex3(pts[5].x, pts[5].y, 0f);
            GL.Vertex3(pts[6].x, pts[6].y, 0f);
            GL.Vertex3(pts[4].x, pts[4].y, 0f);

            GL.End();
            GL.PopMatrix();

        }
        public static void DrawString(Vector2 pos, string text, Color color, bool center = true, int size = 12, FontStyle fontStyle = FontStyle.Bold, int depth = 1)
        {
            __style.fontSize = size;
            __style.richText = true;
            __style.normal.textColor = color;
            __style.fontStyle = fontStyle;

            __outlineStyle.fontSize = size;
            __outlineStyle.richText = true;
            __outlineStyle.normal.textColor = new Color(0f, 0f, 0f, 1f);
            __outlineStyle.fontStyle = fontStyle;

            GUIContent content = new GUIContent(text);
            GUIContent content2 = new GUIContent(text);
            if (center)
            {
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                pos.x -= __style.CalcSize(content).x / 2f;
            }
            switch (depth)
            {
                case 0:
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), content, __style);
                    break;
                case 1:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), content, __style);
                    break;
                case 2:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x - 1f, pos.y - 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), content, __style);
                    break;
                case 3:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x - 1f, pos.y - 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y - 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y + 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), content, __style);
                    break;
            }
        }

        private static Texture2D drawingTex;
        private static Texture2D whiteTexture;

        private static Color lastTexColour;

        private static Material drawMaterial;

        private static GUIStyle __style = new GUIStyle();
        private static GUIStyle __outlineStyle = new GUIStyle();
    }
}