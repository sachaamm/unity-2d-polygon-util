using System;
using _Extensions;
using _Model;
using UnityEngine;

namespace _MonoBehaviour
{
    [ExecuteInEditMode]
    public class TwoLineIntersects : MonoBehaviour
    {
        public GameObject cube;

        public PolygonDrawer polygonDrawer;

        
        
        public Vector2 a;
        public Vector2 b;

        private bool IncludeEdgeIntersectionBounds = false;
        
        private void OnDrawGizmos()
        {
            MyPolyLine myPolyLine = new MyPolyLine(a,b);
            this.DrawLine(myPolyLine, Color.magenta);
        }

        private void OnGUI()
        {
            var pl = polygonDrawer.MyPolygon();
            
            MyPolyLine myPolyLineA = new MyPolyLine(a,b);
            MyPolyLine myPolyLineB = new MyPolyLine(pl.Position(0),pl.Position(1));
            
            bool twoLineIntersects = myPolyLineA.Line2dIntersect(myPolyLineB, IncludeEdgeIntersectionBounds);
            
            string text = twoLineIntersects ? "Intersects" : "Doesnt intersects";

            if (GUI.Button(new Rect(0,0,300,50), text))
            {
                
            }

            IncludeEdgeIntersectionBounds = GUI.Toggle(new Rect(0, 50, 250, 50), IncludeEdgeIntersectionBounds, "IncludeEdgeIntersectionBounds");

        }
    }
}