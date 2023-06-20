using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using _Extensions;
using Nemosis_2DPolygonBoundsUtil.Scripts._Model;
using UnityEngine;

namespace _MonoBehaviour
{
    public class RandomPointInPolygon : MonoBehaviour
    {
        [SerializeField]
        private PolygonDrawer polygonDrawer;

        public GameObject cube;

        private float deltaTime = 0;
        
        private List<MyTriangleAreaData> _triangleAreaDatas = new List<MyTriangleAreaData>();
        private MyPolygon myPolygon;

        private void Start()
        {
            StartCoroutine(BakePolygonCoroutine());
        }

        private IEnumerator BakePolygonCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                for (int i = 0; i < 100; i++)
                {
                    var points = polygonDrawer.PointsAsMyVector2HashSet();
                    myPolygon = new MyPolygon(points);
                    myPolygon.Bake();

                }
                
                stopwatch.Stop();

                deltaTime = stopwatch.ElapsedMilliseconds;
            }
        }
        
        
        private void OnDrawGizmos()
        {
            if (myPolygon == null) return;
            
            foreach (var t in myPolygon.Faces)
            {
                this.DrawLine(myPolygon.Position(t[0]), myPolygon.Position(t[1]), Color.red);
                this.DrawLine(myPolygon.Position(t[1]), myPolygon.Position(t[2]), Color.green);
                this.DrawLine(myPolygon.Position(t[2]), myPolygon.Position(t[0]), Color.blue);
            }

            var areaData = myPolygon.AreaData;
            
            foreach (MyTriangleAreaData triangleArea in areaData)
            {
                this.DrawLine(triangleArea.heightEdge, Color.blue);
            }

            var randomPoint = myPolygon.GetRandomPoint();
            cube.transform.position = randomPoint;

        }

        private void OnGUI()
        {
            this.DisplayGuiText("Delta Time " + deltaTime + "ms");
        }
    }
}