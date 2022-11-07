using _Extensions;
using _Model;
using UnityEngine;

namespace _MonoBehaviour
{
    public class PointInsideBounds : MonoBehaviour
    {
        [SerializeField]
        private PolygonDrawer polygonDrawer;

        private void OnDrawGizmos()
        {
            this.DrawMousePosLine();
        }

        private void OnGUI()
        {
            
            MyPolygon myPolygon = new MyPolygon(polygonDrawer.PointsAsMyVector2HashSet());

            bool insideBounds = myPolygon.PointInsideBounds(this.MousePosToWorldPoint());
            
            if (GUI.Button(new Rect(0, 0, 350, 50), insideBounds ? "Inside" : "Outside" ))
            {
                
            }
        }
    }
}