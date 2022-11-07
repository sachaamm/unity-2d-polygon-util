using UnityEngine;

namespace _Extensions
{
    public static class GameObjectExtensions
    {
        public static Vector3 Pos(this GameObject gameObject)
        {
            return gameObject.transform.position;
        }
    }
}