using _Extensions;
using _Model;
using _Utility;
using UnityEngine;

namespace _MonoBehaviour
{
    public class ClosestPointOnLine : MonoBehaviour
    {
        public GameObject cube;

        public PolygonDrawer polygonDrawer;
        
        void Update()
        {
            var points = polygonDrawer.PointsAsMyVector2List();
            MyPolyLine myPolyLine = new MyPolyLine(points[0], points[1]);
            var closestPointOnLine = MathUtil.ClosestPointOnLine(this.MousePosToWorldPoint(), myPolyLine);

            cube.transform.position = closestPointOnLine;
        }
        
        private void OnDrawGizmos()
        {
            this.DrawMousePosLine();
        }
    }
}