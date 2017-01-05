using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GlitchDancer
{
    public static class MeshConverter
    {
        static Mesh[] SelectedMeshAssets {
            get {
                var assets = Selection.GetFiltered(typeof(Mesh), SelectionMode.Deep);
                return assets.Select(x => (Mesh)x).ToArray();
            }
        }

        static bool CheckSkinned(Mesh mesh)
        {
            if (mesh.boneWeights.Length > 0) return true;
            Debug.LogError("The given mesh (" + mesh.name + ") is not skinned.");
            return false;
        }

        [MenuItem("Assets/Glitch Dancer/Convert Mesh", true)]
        static bool ValidateAssets()
        {
            return SelectedMeshAssets.Length > 0;
        }

        [MenuItem("Assets/Glitch Dancer/Convert Mesh")]
        static void ConvertAssets()
        {
            var converted = new List<Object>();

            foreach (var source in SelectedMeshAssets)
            {
                if (!CheckSkinned(source)) continue;

                // Destination file path.
                var dirPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(source));
                var assetPath = AssetDatabase.GenerateUniqueAssetPath(dirPath + "/Converted Model.asset");

                // Create a converted mesh asset.
                var temp = ConvertMesh(source);
                AssetDatabase.CreateAsset(temp, assetPath);

                converted.Add(temp);
            }

            // Save the generated assets.
            AssetDatabase.SaveAssets();

            // Select the generated assets.
            EditorUtility.FocusProjectWindow();
            Selection.objects = converted.ToArray();
        }

        static Mesh ConvertMesh(Mesh source)
        {
            // Input
            var inVertices = source.vertices;
            var inNormals = source.normals;
            var inTangents = source.tangents;
            var inBoneWeights = source.boneWeights;
            var inIndices = source.GetIndices(0);

            // Output
            var outVertices = new List<Vector3>();
            var outNormals = new List<Vector3>();
            var outTangents = new List<Vector4>();
            var outBoneWeights = new List<BoneWeight>();
            var outUV = new List<Vector2>(); // centroid

            // Enumerate all the triangles and belonging vertices.
            for (var i = 0; i < inIndices.Length; i += 3)
            {
                // Simply copy the original vertex attributes.
                // (position, normal, tangent, weight)
                var i1 = inIndices[i + 0];
                var i2 = inIndices[i + 1];
                var i3 = inIndices[i + 2];

                var v1 = inVertices[i1];
                var v2 = inVertices[i2];
                var v3 = inVertices[i3];

                var n1 = inNormals[i1];
                var n2 = inNormals[i2];
                var n3 = inNormals[i3];

                var t1 = inTangents[i1];
                var t2 = inTangents[i2];
                var t3 = inTangents[i3];

                outVertices.Add(v1);
                outVertices.Add(v2);
                outVertices.Add(v3);

                outNormals.Add(n1);
                outNormals.Add(n2);
                outNormals.Add(n3);

                outTangents.Add(t1);
                outTangents.Add(t2);
                outTangents.Add(t3);

                outBoneWeights.Add(inBoneWeights[i1]);
                outBoneWeights.Add(inBoneWeights[i2]);
                outBoneWeights.Add(inBoneWeights[i3]);

                // Calculate the binormal vectors.
                var bn1 = Vector3.Cross(n1, t1) * t1.w;
                var bn2 = Vector3.Cross(n2, t2) * t2.w;
                var bn3 = Vector3.Cross(n3, t3) * t3.w;

                // Calculate the centroid of the triangle.
                var c = (v1 + v2 + v3) / 3;

                // Differences to the centroid.
                var difCV1 = c - v1;
                var difCV2 = c - v2;
                var difCV3 = c - v3;

                // UV = [(C - V) * Tangent, (C - V) * Binormal]
                outUV.Add(new Vector2(Vector3.Dot(difCV1, t1), Vector3.Dot(difCV1, bn1)));
                outUV.Add(new Vector2(Vector3.Dot(difCV2, t2), Vector3.Dot(difCV2, bn2)));
                outUV.Add(new Vector2(Vector3.Dot(difCV3, t3), Vector3.Dot(difCV3, bn3)));
            }

            // Enumerate vertex indices.
            var indices = Enumerable.Range(0, outVertices.Count).ToArray();

            // Make a clone of the source mesh to avoid
            // the SMR internal caching problem - https://goo.gl/mORHCR
            var mesh = Object.Instantiate<Mesh>(source);
            mesh.name = mesh.name.Substring(0, mesh.name.Length - 7);

            // Clear the unused attributes.
            mesh.colors = null;
            mesh.uv2 = null;
            mesh.uv3 = null;
            mesh.uv4 = null;

            // Overwrite the vertices.
            mesh.subMeshCount = 0;
            mesh.SetVertices(outVertices);
            mesh.SetNormals(outNormals);
            mesh.SetTangents(outTangents);
            mesh.SetUVs(0, outUV);
            mesh.bindposes = source.bindposes;
            mesh.boneWeights = outBoneWeights.ToArray();

            // Add point primitives.
            mesh.subMeshCount = 1;
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);

            // Finishing up.
            mesh.UploadMeshData(true);

            return mesh;
        }
    }
}
