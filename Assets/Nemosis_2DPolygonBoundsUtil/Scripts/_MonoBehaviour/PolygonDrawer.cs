using System.Collections.Generic;
using System.Linq;
using _Extensions;
using _Model;
using Nemosis_2DPolygonBoundsUtil.Scripts._Model;
using UnityEngine;

namespace _MonoBehaviour
{
    public class PolygonDrawer : MonoBehaviour
    {
        public bool AddPointOnMouseClick = true;
        
        public List<Vector3> points;

        public List<Vector2> PointsAsMyVector2List()
        {
            return points.Select(p =>
            {
                Vector2 myVector2 = new Vector2();
                myVector2.x = p.x;
                myVector2.y = p.y;
                return myVector2;
            }).ToList();
        }
        
        public HashSet<Vector2> PointsAsMyVector2HashSet()
        {
            return points.Select(p =>
            {
                Vector2 myVector2 = new Vector2();
                myVector2.x = p.x;
                myVector2.y = p.y;
                return myVector2;
            }).ToHashSet();
        }
        
        public MyPolygon MyPolygon()
        {
            return new MyPolygon(PointsAsMyVector2HashSet());
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && AddPointOnMouseClick)
            {
                var mousePos = Input.mousePosition;
                var pointFromMouse = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
                pointFromMouse.z = 0;
                points.Add(pointFromMouse);
            }
        }

        private void OnGUI()
        {
            for (int i = 0; i < points.Count; i++)
            {
                var screenPos = Camera.main.WorldToScreenPoint(points[i]);

                if (GUI.Button(new Rect(screenPos.x, Screen.height - screenPos.y, 30, 30), i.ToString()))
                {
                    
                }
            }
        }

        void OnDrawGizmos()
        {

            if (points == null) return;
            
            for (int i = 0; i < points.Count; i++)
            {
                int next = i < points.Count - 1 ? (i + 1) : 0;

                Vector3 a = points[i];
                Vector3 b = points[next];
            
                this.DrawLine(a,b, Color.black);
            }
            
        }

    }
}