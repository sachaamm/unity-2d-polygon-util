using System.Collections.Generic;
using _Model;
using DefaultNamespace;
using UnityEngine;

namespace _Utility
{
    public static class MyPolygonUtility
    {
        /// <summary>
        /// Get A random point in triangle projection
        /// </summary>
        /// <param name="triangle"></param>
        /// <param name="spread"> 0 to 1</param>
        /// <returns></returns>
        public static Vector2 GetRandomPointInTriangle(MyTriangle triangle, float spread)
        {
            Vector2 aB = triangle.b - triangle.a;
            Vector2 bC = triangle.c - triangle.b;
            Vector2 cA = triangle.a - triangle.c;

            float e = spread / 2;
        
            Vector2 pointBetweenAandB = triangle.a + aB * (0.5f + Random.Range(-e,e));
            Vector2 pointBetweenBandC = triangle.b + bC * (0.5f + Random.Range(-e,e));
            Vector2 pointBetweenCandA = triangle.c + cA * (0.5f + Random.Range(-e,e));

            List<MyPolyLine> candidates = new List<MyPolyLine>
            {
                new (pointBetweenAandB, pointBetweenBandC),
                new (pointBetweenBandC, pointBetweenCandA),
                new (pointBetweenCandA, pointBetweenAandB)
            };

            MyPolyLine innerLineRandomlySelected = candidates.RandomItem();
        
            return innerLineRandomlySelected.RandomPointInPolyLine();
        }
    }
}