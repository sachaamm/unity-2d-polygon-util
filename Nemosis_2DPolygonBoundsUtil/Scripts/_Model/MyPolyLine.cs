using UnityEngine;

namespace _Model
{
    public class MyPolyLine
    {
        public Vector2 a;
        public Vector2 b;


        public MyPolyLine(Vector2 a, Vector2 b)
        {
            this.a = a;
            this.b = b;
        }

        public Vector2 RandomPointInPolyLine()
        {
            Vector2 diff = b - a;
            return a + diff * Random.value;
        }
        
        public Vector2 MiddlePoint()
        {
            return (a + b) / 2;
        }

        /// <summary>
        /// Are two line 2 intersecting ? From the Habrador Computational Geometry Library (https://github.com/Habrador/Computational-geometry)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="includeEndPoints"></param>
        /// <returns></returns>
        public bool Line2dIntersect(MyPolyLine p, bool includeEndPoints = false)
        {
            //The value we use to avoid floating point precision issues
            //http://sandervanrossen.blogspot.com/2009/12/realtime-csg-part-1.html
            //Unity has a built-in Mathf.Epsilon;
            //But it's better to use our own so we can test different values
            
            //To avoid floating point precision issues we can use a small value
            float epsilon = 0.00001f;

            bool isIntersecting = false;

            float denominator = (p.b.y - p.a.y) * (b.x - a.x) - (p.b.x - p.a.x) * (b.y - a.y);

            //Make sure the denominator is != 0 (or the lines are parallel)
            if (denominator > 0f + epsilon || denominator < 0f - epsilon)
            {
                float u_a = ((p.b.x - p.a.x) * (a.y - p.a.y) - (p.b.y - p.a.y) * (a.x - p.a.x)) / denominator;
                float u_b = ((b.x - a.x) * (a.y - p.a.y) - (b.y - a.y) * (a.x - p.a.x)) / denominator;

                //Are the line segments intersecting if the end points are the same
                if (includeEndPoints)
                {
                    //The only difference between endpoints not included is the =, which will never happen so we have to subtract 0 by epsilon
                    float zero = 0f - epsilon;
                    float one = 1f + epsilon;

                    //Are intersecting if u_a and u_b are between 0 and 1 or exactly 0 or 1
                    if (u_a >= zero && u_a <= one && u_b >= zero && u_b <= one)
                    {
                        isIntersecting = true;
                    }
                }
                else
                {
                    float zero = 0f + epsilon;
                    float one = 1f - epsilon;

                    //Are intersecting if u_a and u_b are between 0 and 1
                    if (u_a > zero && u_a < one && u_b > zero && u_b < one)
                    {
                        isIntersecting = true;
                    }
                }

            }

            return isIntersecting;
        }

    }
}