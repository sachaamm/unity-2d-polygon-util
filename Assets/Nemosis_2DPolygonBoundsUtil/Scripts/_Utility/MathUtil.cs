using _Model;
using UnityEngine;

namespace _Utility
{
    public static class MathUtil
    {
        /// <summary>
        /// https://forum.unity.com/threads/how-do-i-find-the-closest-point-on-a-line.340058/
        /// </summary>
        /// <param name="linePnt"></param>
        /// <param name="lineDir"></param>
        /// <param name="pnt"></param>
        /// <returns></returns>
        static Vector2 NearestPointOnLine(Vector2 linePnt, Vector2 lineDir, Vector2 pnt)
        {
            float lineDirMag = lineDir.magnitude;
            lineDir.Normalize();//this needs to be a unit vector
            Vector2 v = pnt - linePnt;
            float d = Vector2.Dot(v, lineDir);
            return linePnt + lineDir * Mathf.Clamp(d,0,lineDirMag);
        }
        //
        // /// <summary>
        // /// USED
        // /// </summary>
        // /// <param name="point"></param>
        // /// <param name="linePointA"></param>
        // /// <param name="linePointB"></param>
        // /// <returns></returns>
        public static Vector2 ClosestPointOnLine(this Vector2 point, MyPolyLine l)
        {
            return NearestPointOnLine(l.a, (l.b - l.a), point);
        }
    }
}