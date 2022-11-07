using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace _Model
{
    public class MyTriangle
    {
        public Vector2 a, b, c;
        
        public MyTriangle(Vector2 a, Vector2 b, Vector2 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
        
        public Vector2 GetRandomPoint()
        {
            Vector2 aB = b - a;
            Vector2 bC = c - b;
            Vector2 cA = a - c;

            Vector2 pointBetweenAandB = a + aB * Random.value;
            Vector2 pointBetweenBandC = b + bC * Random.value;
            Vector2 pointBetweenCandA = c + cA * Random.value;

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