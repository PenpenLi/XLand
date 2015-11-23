using UnityEngine;
using System.Collections;
using UnityEditor;

public class NewBehaviourScript{

	[MenuItem("test/test")]
	public static void Start(){

		Mesh mesh = new Mesh ();

		Vector3[] vertices = new Vector3[6];


		vertices [0] = new Vector3 (-1, 0, 0);
		vertices [1] = new Vector3 (-0.5f, 0, -0.5f * Mathf.Sqrt(3));
		vertices [2] = new Vector3 (0.5f, 0, -0.5f * Mathf.Sqrt(3));
		vertices [3] = new Vector3 (1, 0, 0);
		vertices [4] = new Vector3 (0.5f, 0, 0.5f * Mathf.Sqrt(3));
		vertices [5] = new Vector3 (-0.5f, 0, 0.5f * Mathf.Sqrt(3));

		int[] triangles = new int[12];

		triangles [0] = 0;
		triangles [1] = 2;
		triangles [2] = 1;
		triangles [3] = 0;
		triangles [4] = 3;
		triangles [5] = 2;
		triangles [6] = 0;
		triangles [7] = 4;
		triangles [8] = 3;
		triangles [9] = 0;
		triangles [10] = 5;
		triangles [11] = 4;

		mesh.vertices = vertices;

		mesh.triangles = triangles;

		AssetDatabase.CreateAsset (mesh, "Assets/a.asset");
	}
}
