using System.Collections.Generic;
using System.Linq;
using _Extensions;
using _Model;
using UnityEngine;
using UnityEngine.Profiling;

namespace _MonoBehaviour
{
    public class PolygonTriangulation : MonoBehaviour
    {
        [SerializeField]
        private PolygonDrawer polygonDrawer;

        public bool Draw = true;
        
        public List<int[]> triangles = new List<int[]>();
        public List<int> trianglesFaces = new List<int>();

        private void OnDrawGizmos()
        {
            var points = polygonDrawer.PointsAsMyVector2HashSet();

            var myPolygon = new MyPolygon(points);

            if (Draw)
            {
                foreach (var t in triangles)
                {
                    this.DrawLine(myPolygon.Position(t[0]), myPolygon.Position(t[1]), Color.red);
                    this.DrawLine(myPolygon.Position(t[1]), myPolygon.Position(t[2]), Color.green);
                    this.DrawLine(myPolygon.Position(t[2]), myPolygon.Position(t[0]), Color.blue);
                }
            }
            
            
        }

        void Update()
        {
            if (!Input.anyKey)
            {
                
                Profiler.BeginSample("polygonDrawer.PointsAsMyVector2HashSet");
                var points = polygonDrawer.PointsAsMyVector2HashSet();
                Profiler.EndSample();
                
                var myPolygon = new MyPolygon(points);

                Profiler.BeginSample("myPolygon.Triangles().ToList()");
                triangles = myPolygon.Triangles().ToList();
                Profiler.EndSample();
                
                Profiler.BeginSample("triangles.SelectMany(s => s).ToList()");
                trianglesFaces = triangles.SelectMany(s => s).ToList();
                Profiler.EndSample();
                
            }
            

        }
        
        private void OnGUI()
        {
            string guiText = "Triangulate";

            if (GUI.Button(new Rect(0, 0, 300, 40), guiText))
            {
                var points = polygonDrawer.PointsAsMyVector2HashSet();
                
                var myPolygon = new MyPolygon(points);

                triangles = myPolygon.Triangles().ToList();
                trianglesFaces = triangles.SelectMany(s => s).ToList();
            }
        }
    }
}