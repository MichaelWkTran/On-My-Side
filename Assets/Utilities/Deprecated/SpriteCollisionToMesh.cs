//#if UNITY_EDITOR
//using System.Collections.Generic;
//using UnityEngine;

//public class SpriteCollisionToMesh : MonoBehaviour
//{
//    public MeshCollider m_assignCollider; //The mesh collider from which the generated mesh collider will be assigned to
//    public PolygonCollider2D m_sourcePolygonCollider; //The poly collider used to generate the mesh
//    public float m_thickness; //Thickness of the generated mesh

//    public void GenerateMesh()
//    {
//        Mesh generatedMesh = m_sourcePolygonCollider.CreateMesh(false, false);
//        List<Vector3> newVertices;
//        List<int> newTriangles;

//        //Add Back and Front Verticies
//        {
//            List<Vector3> backVertices = new List<Vector3>(generatedMesh.vertices);
//            List<Vector3> frontVertices = new List<Vector3>(backVertices);

//            //Shift Back Verticies
//            for (int i = 0; i < backVertices.Count; i++)
//            {
//                Vector3 data = backVertices[i];
//                data.y -= m_thickness * 0.5f;
//                backVertices[i] = data;
//            }

//            //Shift Front Verticies
//            for (int i = 0; i < frontVertices.Count; i++)
//            {
//                Vector3 data = frontVertices[i];
//                data.y -= m_thickness * 0.5f;
//                frontVertices[i] = data;
//            }

//            newVertices = new List<Vector3>(backVertices);
//            newVertices.AddRange(frontVertices);
//        }

//        //Add Front and Back Triangles
//        {
//            List<int> backTriangles = new List<int>(generatedMesh.triangles);
//            List<int> frontTriangles = new List<int>(generatedMesh.triangles);

//            //Adjust front triangles
//            for (int i = 0; i < frontTriangles.Count; i++) frontTriangles[i] += frontTriangles.Count;

//            newTriangles = new List<int>(backTriangles);
//            newTriangles.AddRange(frontTriangles);
//        }

//        //Add edges triangles
//        for (int i = 0; i < generatedMesh.vertices.Length; i++)
//        {
//            newTriangles.Add(i);
//            newTriangles.Add(i);
//            newTriangles.Add(0);

//            newTriangles.Add(0);
//            newTriangles.Add(0);
//            newTriangles.Add(0);
//        }

//        //Finish Mesh
//        generatedMesh.vertices = newVertices.ToArray();
//        generatedMesh.triangles = newTriangles.ToArray();

//        //Assign Mesh
//        m_assignCollider.sharedMesh = generatedMesh;
//    }

//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}

//#endif