using _Model;
using UnityEngine;

namespace Nemosis_2DPolygonBoundsUtil.Scripts._Model
{
    public class MyTriangleAreaData
    {
        public MyPolyLine baseEdge;
        public MyPolyLine heightEdge;

        public MyTriangleAreaData(MyPolyLine baseEdge, MyPolyLine heightEdge)
        {
            this.baseEdge = baseEdge;
            this.heightEdge = heightEdge;
        }
    }
}