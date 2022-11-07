using System;
using System.Collections.Generic;
using System.Linq;
using _Model;
using _Utility;
using UnityEngine;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;

namespace Nemosis_2DPolygonBoundsUtil.Scripts._Model
{
    public class MyPolygon
    {
        private readonly HashSet<Vector2> _points;

        public MyPolygon(HashSet<Vector2> points)
        {
            this._points = points;
        }
        
        public Vector2 Position(int index)
        {
            return _points.ElementAt(index);
        }
        
        #region Bounds

        private List<MyPolyLineWithIndices> Bounds()
        {
            List<MyPolyLineWithIndices> myPolyLines = new List<MyPolyLineWithIndices>();
            
            for (int i = 0; i < _points.Count; i++)
            {
                var current = _points.ElementAt(i);
                int nextIndex = i == _points.Count - 1 ? 0 : i + 1;
                var next = _points.ElementAt(nextIndex);
                var polyLine = new MyPolyLine(current, next);
                myPolyLines.Add(new MyPolyLineWithIndices(polyLine,i, nextIndex ));
            }

            return myPolyLines;
        }
        
        private bool IsABoundEdge(int indexA, int indexB)
        {
            return indexA == indexB - 1 || (indexA == _points.Count - 1) && indexB == 0;
        }

        public bool PointInsideBounds(Vector2 point)
        {
            var farPoint = new Vector2(999999999, 999999999);

            int nbIntersections = 0;

            MyPolyLine currentToLimits = new MyPolyLine(point, farPoint);

            foreach (var polyLine in Bounds())
            {
                if (MyIntersection.Line2dIntersect(currentToLimits, polyLine.MyPolyLine))
                {
                    nbIntersections++;
                }
            }
            
            return nbIntersections % 2 == 1;
        }

        public Vector2 ClosestPointOnPolygon(Vector2 point)
        {
            float minDistance = Mathf.Infinity;

            Vector2 closestPoint = new Vector2(Mathf.Infinity, Mathf.Infinity);
            
            foreach (var bound in Bounds())
            {
                var closestPointOnLine = MathUtil.ClosestPointOnLine(point, bound.MyPolyLine);
                float distance = Vector2.Distance(point, closestPointOnLine);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = closestPointOnLine;
                }
            }

            return closestPoint;
        }

        #endregion
        
        #region Triangulation
        
        private int _triangulationLeftIndex = 0;
        private int _triangulationRightIndex = 0;
        private int _overflowCount = 0;
        private HashSet<int[]> _myTriangles = new();
        private int _indexTriangulation = 0;

        void FixTriangulationIndexes()
        {
            _myTriangles = new HashSet<int[]>();
            _overflowCount = 0;
            _triangulationLeftIndex = 0;
            _triangulationRightIndex = _points.Count - 1;
        }

        public HashSet<int[]> Triangulate()
        {
            FixTriangulationIndexes();

            while (_indexTriangulation < _points.Count && !TriangulationIsDone())
            {
                ConnectAllPossibleTriangleToCurrentIndex();
            }
            
            return _myTriangles;
        }

        int[] TriangleFace(int a, int b, int c)
        {
            return new[] { a, b, c };
        }
        
        bool ContainsTriangle(int a, int b, int c)
        {
            return _myTriangles.Any(t => t[0] == a && t[1] == b && t[2] == c);
        }

        /// <summary>
        /// Connect All Possible Triangle To Current Index
        /// </summary>
        void ConnectAllPossibleTriangleToCurrentIndex()
        {
            for (int i = 0; i < _points.Count; i++)
            {
                int nextIndex = i == _points.Count - 1 ? 0 : i + 1;
            
                if (_indexTriangulation != i && _indexTriangulation != nextIndex)
                {
                    Profiler.BeginSample("Triangle Face");
                    var face = TriangleFace(_indexTriangulation, i, nextIndex);
                    Profiler.EndSample();
                    Profiler.BeginSample("Triangle Face");
                    Array.Sort(face);
                    Profiler.EndSample();
                    
                    Profiler.BeginSample("if contains and valid add");
                    if (!ContainsTriangle(face[0], face[1], face[2]) && CurrentTriangulationIsValid(_indexTriangulation, i, nextIndex) )
                    {
                        _myTriangles.Add(face);
                    }
                    Profiler.EndSample();
                }
            }

            _indexTriangulation++;

            foreach (MyPolyLineWithIndices bound in Bounds())
            {
                if (!ATriangleContainsBoundIndexes(bound))
                {
                    for (int i = 0; i < _points.Count; i++)
                    {
                        if (CurrentTriangulationIsValid(bound.indexA, bound.indexB, i))
                        {
                            var face = TriangleFace(bound.indexA, bound.indexB, i);
                            Array.Sort(face);
                            _myTriangles.Add(face);
                        }
                    }
                }
            }
        }

        bool ATriangleContainsBoundIndexes(MyPolyLineWithIndices bound)
        {
            for (int i = 0; i < _myTriangles.Count; i++)
            {
                var t = _myTriangles.ElementAt(i);
                if (t.Contains(bound.indexA) && t.Contains(bound.indexB)) return true;
            }
            return false;
        }
        
        bool TriangulationIndexesInBounds()
        {
            return _triangulationLeftIndex < _points.Count && _triangulationRightIndex > 0 && _overflowCount <= _points.Count * 2;
        }
        
        bool CurrentTriangulationIsValid(int indexA, int indexB, int indexC)
        {
            Profiler.BeginSample("if TriangulationIndexesInBounds");
            if (!TriangulationIndexesInBounds()) return false;
            Profiler.EndSample();
            
            Profiler.BeginSample("if isIntersectingWithAnotherTriangleEdge");
            bool isIntersectingWithAnotherTriangleEdge = IsIntersectingWithAnotherTriangleEdge(indexA, indexB, indexC);
            Profiler.EndSample();
            
            Profiler.BeginSample("if edgesOfTriangleAreOutOfPolygonBounds");
            bool edgesOfTriangleAreOutOfPolygonBounds = EdgesOfTriangleAreOutOfPolygonBounds(indexA, indexB, indexC);
            Profiler.EndSample();
            // TODO check triangle exists
            bool valid = !isIntersectingWithAnotherTriangleEdge && !edgesOfTriangleAreOutOfPolygonBounds;
            
            return valid;
        }

        private bool IsIntersectingWithAnotherTriangleEdge(int indexA, int indexB, int indexC)
        {
            if (!TriangulationIndexesInBounds()) return false;

            MyPolyLine edgeA = new MyPolyLine(_points.ElementAt(indexA), _points.ElementAt(indexB));
            MyPolyLine edgeB = new MyPolyLine(_points.ElementAt(indexB), _points.ElementAt(indexC));
            MyPolyLine edgeC = new MyPolyLine(_points.ElementAt(indexC), _points.ElementAt(indexA));

            foreach (var boundLine in Bounds())
            {
                bool intersectionOnA = edgeA.Line2dIntersect(boundLine.MyPolyLine);// && IsBoundEdge(indexA, indexB);
                bool intersectionOnB = edgeB.Line2dIntersect(boundLine.MyPolyLine);// && IsBoundEdge(indexB, indexC);
                bool intersectionOnC = edgeC.Line2dIntersect(boundLine.MyPolyLine);// && IsBoundEdge(indexC, indexA);
                
                if (intersectionOnA || 
                    intersectionOnB ||
                    intersectionOnC)
                {
                    return true;
                }
            }

            foreach (var t in _myTriangles)
            {
                var tEdgeA = new MyPolyLine(Position(t[0]), Position(t[1]));
                var tEdgeB = new MyPolyLine(Position(t[1]), Position(t[2]));
                var tEdgeC = new MyPolyLine(Position(t[2]), Position(t[0]));
                
                bool iaa = edgeA.Line2dIntersect(tEdgeA);
                bool iab = edgeA.Line2dIntersect(tEdgeB);
                bool iac = edgeA.Line2dIntersect(tEdgeC);
                
                bool iba = edgeB.Line2dIntersect(tEdgeA);
                bool ibb = edgeB.Line2dIntersect(tEdgeB);
                bool ibc = edgeB.Line2dIntersect(tEdgeC);

                bool ica = edgeC.Line2dIntersect(tEdgeA);
                bool icb = edgeC.Line2dIntersect(tEdgeB);
                bool icc = edgeC.Line2dIntersect(tEdgeC);

                if (iaa || iab || iac || iba || ibb || ibc || ica || icb || icc)
                {
                    return true;
                }
            }
            
            return false;
        }

        private bool EdgesOfTriangleAreOutOfPolygonBounds(int indexA, int indexB, int indexC)
        {
            MyPolyLine edgeA = new MyPolyLine(_points.ElementAt(indexA), _points.ElementAt(indexB));
            MyPolyLine edgeB = new MyPolyLine(_points.ElementAt(indexB), _points.ElementAt(indexC));
            MyPolyLine edgeC = new MyPolyLine(_points.ElementAt(indexC), _points.ElementAt(indexA));
            
            var middlePointA = edgeA.MiddlePoint();
            var middlePointB = edgeB.MiddlePoint();
            var middlePointC = edgeC.MiddlePoint();

            bool firstEdgeIsABoundEdge = IsABoundEdge(indexA, indexB); // IsABoundEdge(indexA, indexB); // IsABoundEdge(edgeA);
            bool secondEdgeIsABoundEdge = IsABoundEdge(indexB, indexC); // IsABoundEdge(indexB, indexC); // IsABoundEdge(edgeB);
            bool thirdEdgeIsABoundEdge = IsABoundEdge(indexC, indexA); // IsABoundEdge(indexC, indexA); // IsABoundEdge(edgeC);

            bool anEdgeIsOutsideBound = 
                (!firstEdgeIsABoundEdge && !PointInsideBounds(middlePointA)) || 
                (!secondEdgeIsABoundEdge && !PointInsideBounds(middlePointB)) ||
                (!thirdEdgeIsABoundEdge && !PointInsideBounds(middlePointC));

            return anEdgeIsOutsideBound;
        }

        bool TriangulationIsDone()
        {
            return _myTriangles.SelectMany(s => s).Distinct().ToList().Count == _points.Count;
        }
        
        #endregion
        
        #region Polygon Area

        public List<MyTriangleAreaData> GetAreaData()
        {
            List<MyTriangleAreaData> data = new List<MyTriangleAreaData>();
     
            foreach (var t in _myTriangles)
            {
                var pointA = Position(t[0]);
                var pointB = Position(t[1]);
                var pointC = Position(t[2]);

                var edgeA = new MyPolyLine(pointA, pointB);
                var edgeB = new MyPolyLine(pointB, pointC);
                var edgeC = new MyPolyLine(pointC, pointA);

                float magA = edgeA.Magnitude();
                float magB = edgeB.Magnitude();
                float magC = edgeC.Magnitude();

                MyPolyLine baseEdge = null;
                int indexPointNotInBaseEdge = -1;
                Vector2 baseEdgeNormalize = Vector2.down;

                // Get Triangle Base 
                if (magA >= magB && magA >= magC)
                {
                    baseEdge = edgeA;
                    indexPointNotInBaseEdge = t[2];
                    baseEdgeNormalize = edgeA.Normalized();
                }
                
                if (magB >= magA && magB >= magC)
                {
                    baseEdge = edgeB;
                    indexPointNotInBaseEdge = t[0];
                    baseEdgeNormalize = edgeB.Normalized();
                }
                
                if (magC >= magA && magC >= magB)
                {
                    baseEdge = edgeC;
                    indexPointNotInBaseEdge = t[1];
                    baseEdgeNormalize = edgeC.Normalized();
                }

                Vector2 baseEdgeRightAngleDirection = new Vector2(baseEdgeNormalize.y, -baseEdgeNormalize.x);

                var baseOppositePoint = Position(indexPointNotInBaseEdge);
                
                MyPolyLine rightAnglePolyline = new MyPolyLine(baseOppositePoint, baseOppositePoint + baseEdgeRightAngleDirection * 9999999);

                Vector2 intersectionWithBase = baseEdge.GetLineLineIntersectionPoint(rightAnglePolyline);
                
                // Get Triangle Height
                MyPolyLine heightEdge = new MyPolyLine(intersectionWithBase, baseOppositePoint);
                
                data.Add(new MyTriangleAreaData(baseEdge, heightEdge));

            }
            
            return data;
        }

        
        public Vector2 GetRandomPoint()
        {
            double polygonArea = 0;

            List<float> areaRatioInPolygons = new List<float>();

            foreach (var triangleAreaData in GetAreaData())
            {
                double triangleArea =
                    (triangleAreaData.baseEdge.Magnitude() * triangleAreaData.heightEdge.Magnitude()) / 2;

                polygonArea += triangleArea;
            }
            
            foreach (var triangleAreaData in GetAreaData())
            {
                double triangleArea =
                    (triangleAreaData.baseEdge.Magnitude() * triangleAreaData.heightEdge.Magnitude()) / 2;

                float areaRatio = (float)(triangleArea / polygonArea);
                areaRatioInPolygons.Add(areaRatio);
            }

            int selectRandomTriangle = 0;

            float random = Random.value;
         
            float ratioSum = 0;
            int i = 0;

            foreach (var ratio in areaRatioInPolygons)
            {
                
                float nextRatioValue = i < areaRatioInPolygons.Count - 1 ? ratioSum + areaRatioInPolygons[i + 1] : 1;

                if (random >= ratioSum && random < nextRatioValue)
                {
                    selectRandomTriangle = i;
                }
                ratioSum += ratio;
                
                i++;
            }
            
            var randomFace = _myTriangles.ElementAt(selectRandomTriangle);
            
            MyTriangle myTriangle =
                new MyTriangle(Position(randomFace[0]), Position(randomFace[1]), Position(randomFace[2]));
            
            return myTriangle.GetRandomPoint();

        }
        
        #endregion
    }
}