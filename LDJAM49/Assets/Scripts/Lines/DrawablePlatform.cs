using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace PathCreation.Drawables
{
    public class DrawablePlatform : MonoBehaviour
    {
        LineRenderer lineRenderer;
        Vector3[] linePoints;

        PolygonCollider2D polygonCollider;

        [SerializeField]
        GameObject interactHandle;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private GameObject interactedIndicator;

        [SerializeField] private ColliderToMesh meshMaker;

        VertexPath path;
        private int maxIndex { get { return path.NumPoints; } }

        [SerializeField] private Transform interactPoint;
        [SerializeField] private float lineWidth = 0.2f;
        [SerializeField] private int indexGrace = 5;

        private int currentIndex;

        private bool reachedEnd = false;
        public UnityEvent reachedEndEvent;

        void Awake(){
            lineRenderer = GetComponent<LineRenderer>();
            linePoints = new Vector3[lineRenderer.positionCount];

            polygonCollider = GetComponent<PolygonCollider2D>();
            
            lineRenderer.GetPositions(linePoints);
        }

        void Start(){
            if (interactPoint == null){
                var interHandle = Instantiate(interactHandle, transform);
                interactPoint = interHandle.transform;
            }
            // Debug.Log(linePoints.Length);
            interactPoint.position = linePoints[0];

            // Does not work: CAUSES ERRORS??? UNITY START UP ROUTINE GETS CALLED TWICE???
            //lineRenderer.positionCount = 0;
            currentIndex = 0;

            SetPathVisual(0);
            SetPathCollider(1);
        }

        public void SetPath(VertexPath p){
            path = p;
        }

        public void RemovePath(){
            Debug.Log("Removing");
            currentIndex = 0;

            SetPathVisual(0);
            SetPathCollider(1);

            if (meshMaker != null){
                meshMaker.MakeMesh(polygonCollider);

            }

            SetInteractPoint(0, 0);
        }

        public void Interact(bool interacting){
            if (interacting){
                particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                // interactedIndicator.SetActive(interacting);
            } else {
                particles.Play();
            }
            interactedIndicator.SetActive(interacting);
        }

        public void ShowIndicator(bool show){
            interactedIndicator.SetActive(show);
        }

        public void MakePath(Vector2 hitPoint){
            int index = path.GetClosestPointIndexOnPath(hitPoint);
            if (index < currentIndex){
                return;
            }

            if (index > currentIndex+indexGrace){
                return;
            }

            if (maxIndex - indexGrace <= index && index <= maxIndex){
                index = maxIndex;

                reachedEnd = true;
            }

            currentIndex = index;
            // Debug.Log(index);

            SetPathVisual(index);
            SetPathCollider(index);

            if (meshMaker != null){
                meshMaker.MakeMesh(polygonCollider);

            }

            float t = path.GetClosestTimeOnPath(hitPoint);
            
            SetInteractPoint(t, index);

            if (reachedEnd){
                if (reachedEndEvent != null){
                    reachedEndEvent.Invoke();
                }
                reachedEnd = false;
            }
        }

        public void ErasePath(Vector2 hitPoint){
            int index = path.GetClosestPointIndexOnPath(hitPoint);
            if (index > currentIndex){
                return;
            }

            if (index < currentIndex-indexGrace){
                return;
            }

            if (indexGrace >= index && index >= 0){
                index = 0;

                reachedEnd = true;
            }

            currentIndex = index;

            SetPathVisual(index);
            SetPathCollider(index);

            if (meshMaker != null){
                meshMaker.MakeMesh(polygonCollider);

            }

            float t = path.GetClosestTimeOnPath(hitPoint);
            
            SetInteractPoint(t, Mathf.Max(0, index-1));

            if (reachedEnd){
                if (reachedEndEvent != null){
                    reachedEndEvent.Invoke();
                }
                reachedEnd = false;
            }
        }

        void SetInteractPoint(float t, int index){
            float angle = Vector2.SignedAngle(Vector2.right, path.GetDirection(t));
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            interactPoint.position = linePoints[Mathf.Min(index, maxIndex-1)];
            interactPoint.rotation = rotation;

        }

        void SetPathVisual(int index){
            lineRenderer.positionCount = index;
            for (int i = 0; i < index; i++)
            {
                Vector2 position = linePoints[i];
                lineRenderer.SetPosition(i, position);
            }
        }

        void SetPathCollider(int index){
            Vector3[] positions = path.localPoints;

            int ind = Mathf.Max(1, index);

            Vector3[] posLeft = new Vector3[ind];
            Vector3[] posRight = new Vector3[ind];


            for (int i = 0; i < ind; i++)
            {
                Vector3 localUp = path.up;
                Vector3 localRight = Vector3.Cross (localUp, path.GetTangent (i));

                // Find position to left and right of current path vertex
                Vector3 vertSideA = path.GetPoint (i) - localRight * Mathf.Abs (lineWidth) - transform.position;
                Vector3 vertSideB = path.GetPoint (i) + localRight * Mathf.Abs (lineWidth) - transform.position;

                posLeft[i] = vertSideA;
                posRight[i] = vertSideB;
            }

            Array.Reverse(posRight);

            Vector3[] newPos = new Vector3[posLeft.Length + posRight.Length];
            posLeft.CopyTo(newPos, 0);
            posRight.CopyTo(newPos, ind);

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
