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

            byte randomChoice = (byte)Random.Range(0, 3);

            if (randomChoice == 0)
            {
                return new MyPolyLine(pointBetweenAandB, pointBetweenBandC).RandomPointInPolyLine();
            }
            
            if (randomChoice == 1)
            {
                return new MyPolyLine(pointBetweenBandC, pointBetweenCandA).RandomPointInPolyLine();
            }
            
            if (randomChoice == 2)
            {
                return new MyPolyLine(pointBetweenCandA, pointBetweenAandB).RandomPointInPolyLine();
            }

            return Vector2.negativeInfinity;
        }
    }
}