using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace PathCreation
{
    public class VertexPathLine : MonoBehaviour
    {
        LineRenderer line;
        PolygonCollider2D polygonCollider;
        [SerializeField] private float colWidth;

        void Awake(){
            line = GetComponent<LineRenderer>();
            polygonCollider = GetComponent<PolygonCollider2D>();
        }

        public void SetPath(VertexPath path){
            SetLine(path);
            SetCollider(path);
        }

        void SetLine(VertexPath path){
            line.positionCount = path.localPoints.Length;
            line.SetPositions(path.localPoints);
        }

        void SetCollider(VertexPath path){
            Vector3[] positions = path.localPoints;

            Vector3[] posLeft = new Vector3[path.NumPoints];
            Vector3[] posRight = new Vector3[path.NumPoints];

            for (int i = 0; i < path.NumPoints; i++)
            {
                Vector3 localUp = path.up;
                Vector3 localRight = Vector3.Cross (localUp, path.GetTangent (i));

                // Find position to left and right of current path vertex
                Vector3 vertSideA = path.GetPoint (i) - localRight * Mathf.Abs (colWidth);
                Vector3 vertSideB = path.GetPoint (i) + localRight * Mathf.Abs (colWidth);

                posLeft[i] = vertSideA;
                posRight[i] = vertSideB;
            }

            Array.Reverse(posRight);

            Vector3[] newPos = new Vector3[path.NumPoints*2];
            posLeft.CopyTo(newPos, 0);
            posRight.CopyTo(newPos, path.NumPoints);

            var points = ToVector2(newPos);

            polygonCollider.SetPath(0, points);

        }

        Vector2[] ToVector2(Vector3[] points){
            Vector2[] newPoints = new Vector2[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                newPoints[i] = new Vector2(points[i].x, points[i].y);
            }

            return newPoints;
        }
    }
}
