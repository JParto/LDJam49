using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class ColliderToMesh : MonoBehaviour {
    MeshFilter mf; 
    MeshRenderer mr;
    //ParticleSystem ps;

    void Awake(){
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();
        //ps = GetComponent<ParticleSystem>();
    }

    public void MakeMesh (PolygonCollider2D polygon) {
        //Render thing
        int pointCount = 0;
        pointCount = polygon.GetTotalPointCount();
        Mesh mesh = new Mesh();
        Vector2[] points = polygon.points;
        Vector3[] vertices = new Vector3[pointCount];
        Vector2[] uv = new Vector2[pointCount];
        for(int j=0; j<pointCount; j++){
            Vector2 actual = points[j];
            vertices[j] = new Vector3(actual.x, actual.y, 0);
            uv[j] = actual;
        }
        Triangulator tr = new Triangulator(points);
        int [] triangles = tr.Triangulate();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mf.mesh = mesh;
        //Render thing
        
        // Particles??????
        //MakeParticles();
    }


    // void MakeParticles(){
    //     var shape = ps.shape;

    //     shape.meshRenderer = mr;

    // }
}