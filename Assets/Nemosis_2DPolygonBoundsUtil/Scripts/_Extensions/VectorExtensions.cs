using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public static class VectorExtensions
    {
        public static Vector3 SumAverage(this List<Vector3> Points)
        {
            float xa = Points.Average(s => s.x);
            float ya = Points.Average(s => s.y);
            float za = Points.Average(s => s.z);

            return new Vector3(xa, ya, za);
        }

        public static List<Vector2> AsMyVector2List(this List<Vector3> Points)
        {
            return Points.Select(p =>
            {
                return new Vector2(p.x, p.y);
            }).ToList();
        }

        public static List<Vector2> AsMyVector2List(this List<Vector2> Points)
        {
            return Points.Select(p =>
            {
                return new Vector2(p.x, p.y);
            }).ToList();
        }
    }
}