// using System.Linq;
// using _Model;
// using Habrador_Computational_Geometry;
// using UnityEngine;
//
// namespace _Extensions
// {
//     public static class HabradorLibExtensions
//     {
//         public static MyVector2[] TriangleMiddlePoints(this Triangle2 triangle2)
//         {
//             Vector2[] points = new Vector2[3];
//             
//             points[0] = (triangle2.p1.ToVector2() + triangle2.p2.ToVector2()) / 2;
//             points[1] = (triangle2.p2.ToVector2() + triangle2.p3.ToVector2()) / 2;
//             points[2] = (triangle2.p3.ToVector2() + triangle2.p1.ToVector2()) / 2;
//
//             return points.Select(s => s.ToMyVector2()).ToArray();
//         }
//
//         public static MyPolyLine ToPolyLine(this Edge2 edge)
//         {
//             return new MyPolyLine(edge.p1.ToVector2(), edge.p2.ToVector2());
//         }
//         
//         // public static Triangle2 ToTriangle2(MyPolyLine a, MyP)
//     }
// }