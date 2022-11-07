using System;
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
        
        
        private void OnDrawGizmos()
        {
            var points = polygonDrawer.PointsAsMyVector2HashSet();

            var myPolygon = new MyPolygon(points);
            var triangles = myPolygon.Triangulate();
            
            foreach (var t in triangles)
            {
                this.DrawLine(myPolygon.Position(t[0]), myPolygon.Position(t[1]), Color.red);
                this.DrawLine(myPolygon.Position(t[1]), myPolygon.Position(t[2]), Color.green);
                this.DrawLine(myPolygon.Position(t[2]), myPolygon.Position(t[0]), Color.blue);
            }
            
            var areaData = myPolygon.GetAreaData();
            
            foreach (var triangleArea in areaData)
            {
                this.DrawLine(triangleArea.heightEdge, Color.blue);
            }

            var randomPoint = myPolygon.GetRandomPoint();
            cube.transform.position = randomPoint;

        }
    }
}