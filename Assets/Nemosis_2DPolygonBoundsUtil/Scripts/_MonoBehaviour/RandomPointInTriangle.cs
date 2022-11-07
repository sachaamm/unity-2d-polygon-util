using System.Collections.Generic;
using _Extensions;
using _Model;
using _Utility;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _MonoBehaviour
{
    [RequireComponent(typeof(PolygonDrawer))]
    public class RandomPointInTriangle : MonoBehaviour
    {
        public GameObject prefab;
        public GameObject bigPointPrefab;

        private int attempts = 0;
        const int nbMaxAttempts = 500;

        public float Spread = 1;

        private List<Vector3> outputPoints = new List<Vector3>();

        [SerializeField]
        private PolygonDrawer polygonDrawer;

        void Update()
        {

            attempts++;

            if (attempts == nbMaxAttempts)
            {
                Instantiate(bigPointPrefab, outputPoints.SumAverage(), Quaternion.identity);
            }
        
            if(attempts >= nbMaxAttempts) return;
            
            Vector2 rpInTriangle = MyPolygonUtility.GetRandomPointInTriangle(PointsAsTriangle(),Spread);
            Instantiate(prefab, rpInTriangle, Quaternion.identity);

            outputPoints.Add(rpInTriangle);

        }

        MyTriangle PointsAsTriangle()
        {
            return new MyTriangle(polygonDrawer.points[0], polygonDrawer.points[1], polygonDrawer.points[2]);
        }
        
        void OnDrawGizmos()
        {
            MyTriangle t = PointsAsTriangle();
            
            var aB = t.b - t.a;
            var bC = t.c - t.b;
            var cA = t.a - t.c;
        
            float e = Spread / 2;
        
            var pointBetweenAandB = t.a + aB * (0.5f + Random.Range(-e,e));
            var pointBetweenBandC = t.b + bC * (0.5f + Random.Range(-e,e));
            var pointBetweenCandA = t.c + cA * (0.5f + Random.Range(-e,e));
            
            this.DrawLine(pointBetweenAandB, pointBetweenBandC, Color.red);
            this.DrawLine(pointBetweenBandC, pointBetweenCandA, Color.green);
            this.DrawLine(pointBetweenCandA, pointBetweenAandB, Color.blue);
        }
        
    }
}
