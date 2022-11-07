using _Model;
using UnityEngine;

namespace _Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static void DrawMousePosLine(this MonoBehaviour monoBehaviour)
        {
            var far = new Vector3(9999999, 9999999,0);
            monoBehaviour.DrawLine(monoBehaviour.MousePosToWorldPoint(), far, Color.magenta);
        }

        public static Vector3 MousePosToWorldPoint(this MonoBehaviour monoBehaviour, float z = 0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = z;
            return mousePos;
        }
        
        public static void DrawLine(this MonoBehaviour monoBehaviour, Vector3 a, Vector3 b, Color c)
        {
            Gizmos.color = c;
            Gizmos.DrawLine(a, b);
        }
        
        public static void DrawLine(this MonoBehaviour monoBehaviour, MyPolyLine p, Color c)
        {
            Gizmos.color = c;
            Gizmos.DrawLine(p.a, p.b);
        }
        
        public static void DrawLine(this MonoBehaviour monoBehaviour, Vector2 a, Vector2 b, Color c)
        {
            Gizmos.color = c;
            Gizmos.DrawLine(new Vector3(a.x, a.y, 0), new Vector3(b.x, b.y, 0));
        }
    }
}