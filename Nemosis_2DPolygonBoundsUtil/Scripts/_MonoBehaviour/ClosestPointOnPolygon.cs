using _Extensions;
using _Model;
using UnityEngine;

namespace _MonoBehaviour
{
    public class ClosestPointOnPolygon : MonoBehaviour
    {
        public PolygonDrawer polygonDrawer;

        public GameObject cube;
        
        void Update()
        {
            var points = polygonDrawer.PointsAsMyVector2List();

            MyPolygon myPolygon = polygonDrawer.MyPolygon();
            var closestPointOnPolygon = myPolygon.ClosestPointOnPolygon(this.MousePosToWorldPoint());
            
            MyPolyLine myPolyLine = new MyPolyLine(points[0], points[1]);
        
            cube.transform.position = closestPointOnPolygon;
        }
        
        private void OnDrawGizmos()
        {
            this.DrawMousePosLine();
        }
    }
}