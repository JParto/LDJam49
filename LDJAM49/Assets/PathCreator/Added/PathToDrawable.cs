using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PathCreation.Drawables;

namespace PathCreation
{
    [ExecuteInEditMode, RequireComponent(typeof(LineRenderer), typeof(PolygonCollider2D), typeof(DrawablePlatform))]
    public class PathToDrawable : MonoBehaviour
    {
        PathCreator path;
        VertexPath p {get {return path.path;}}

        // [SerializeField]
        // GameObject interactHandle;

        LineRenderer lineRenderer;
        PolygonCollider2D polygonCollider;
        DrawablePlatform drawablePlatform;

        [SerializeField] private float lineWidth = 0.2f;
        [SerializeField] private AnimationCurve lineCurve;

        void Awake(){
            path = GetComponent<PathCreator>();
            path.EditorData.bezierOrVertexPathModified += TriggerUpdate;
            path.EditorData.bezierCreated += TriggerUpdate;
            TriggerUpdate();
        }

        void Start(){
            //VertexPath p = path.path;
            //TriggerUpdate();
        }

        public void TriggerUpdate(){
            PathUpdated();
        }

        void PathUpdated(){
            AssignComponents();
            SetPath(p);
        }

        void AssignComponents(){

            if (!gameObject.GetComponent<LineRenderer>()){
                gameObject.AddComponent<LineRenderer>();
            }
            if (!gameObject.GetComponent<PolygonCollider2D>()){
                gameObject.AddComponent<PolygonCollider2D>();
            }
            if (!gameObject.GetComponent<DrawablePlatform>()){
                gameObject.AddComponent<DrawablePlatform>();
            }

            lineRenderer = GetComponent<LineRenderer>();
            polygonCollider = GetComponent<PolygonCollider2D>();
            drawablePlatform = GetComponent<DrawablePlatform>();
        }

        public void SetPath(VertexPath path){
            SetLine(path);
            // SetCollider(path);
            drawablePlatform.SetPath(path);
        }

        void SetLine(VertexPath path){
            lineRenderer.positionCount = path.localPoints.Length;
            // lineRenderer.startWidth = lineWidth*2;
            // lineRenderer.endWidth = lineWidth*2;
            lineRenderer.widthCurve = lineCurve;
            for (int i = 0; i < path.NumPoints; i++)
            {
                lineRenderer.SetPosition(i, path.GetPoint(i));
            }
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
                Vector3 vertSideA = path.GetPoint (i) - localRight * Mathf.Abs (lineWidth);
                Vector3 vertSideB = path.GetPoint (i) + localRight * Mathf.Abs (lineWidth);

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
