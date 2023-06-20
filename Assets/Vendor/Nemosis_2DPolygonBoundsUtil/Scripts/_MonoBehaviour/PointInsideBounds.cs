using _Extensions;
using _Model;
using Nemosis_2DPolygonBoundsUtil.Scripts._Model;
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

            this.DisplayGuiText(insideBounds ? "Inside" : "Outside" );
        }
    }
}