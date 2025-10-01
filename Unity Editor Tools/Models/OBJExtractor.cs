using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class ObjExtractor
{
    [MenuItem("Tools/OBJ/Extract Full Contents")]
    static void ExtractObjContents()
    {
        Object selected = Selection.activeObject;
        if (selected == null)
        {
            Debug.LogError("No .obj file selected.");
            return;
        }

        string objPath = AssetDatabase.GetAssetPath(selected);
        if (!objPath.EndsWith(".obj"))
        {
            Debug.LogError("Selected file is not a .obj model.");
            return;
        }

        GameObject objRoot = AssetDatabase.LoadAssetAtPath<GameObject>(objPath);
        if (objRoot == null)
        {
            Debug.LogError("Failed to load .obj as GameObject.");
            return;
        }

        string baseFolder = Path.GetDirectoryName(objPath);
        string assetName = Path.GetFileNameWithoutExtension(objPath);

        string modelsFolder = Path.Combine(baseFolder, "Models");
        string materialsFolder = Path.Combine(baseFolder, "Materials");
        string texturesFolder = Path.Combine(baseFolder, "Textures");

        Directory.CreateDirectory(modelsFolder);
        Directory.CreateDirectory(materialsFolder);
        Directory.CreateDirectory(texturesFolder);

        Dictionary<string, string> copiedTextures = new();

        // Instantiate OBJ model in scene
        GameObject instance = Object.Instantiate(objRoot);
        instance.name = assetName;

        foreach (Transform child in instance.transform)
        {
            GameObject model = child.gameObject;

            // Save mesh
            MeshFilter meshFilter = model.GetComponent<MeshFilter>();
            if (meshFilter && meshFilter.sharedMesh)
            {
                Mesh meshCopy = Object.Instantiate(meshFilter.sharedMesh);
                string meshPath = Path.Combine(modelsFolder, model.name + ".mesh");
                meshPath = AssetDatabase.GenerateUniqueAssetPath(meshPath);
                AssetDatabase.CreateAsset(meshCopy, meshPath);
                meshFilter.sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
            }

            // Extract materials
            MeshRenderer renderer = model.GetComponent<MeshRenderer>();
            if (renderer && renderer.sharedMaterials != null)
            {
                Material[] updatedMats = new Material[renderer.sharedMaterials.Length];

                for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    Material mat = renderer.sharedMaterials[i];
                    if (mat == null) continue;

                    string matPath = Path.Combine(materialsFolder, mat.name + ".mat");
                    matPath = AssetDatabase.GenerateUniqueAssetPath(matPath);

                    Material matCopy = Object.Instantiate(mat);

                    if (matCopy.mainTexture != null)
                    {
                        string texPath = AssetDatabase.GetAssetPath(matCopy.mainTexture);

                        if (!copiedTextures.TryGetValue(texPath, out string newTexPath))
                        {
                            newTexPath = Path.Combine(texturesFolder, Path.GetFileName(texPath));
                            newTexPath = AssetDatabase.GenerateUniqueAssetPath(newTexPath);
                            AssetDatabase.CopyAsset(texPath, newTexPath);
                            copiedTextures[texPath] = newTexPath;
                        }

                        Texture newTex = AssetDatabase.LoadAssetAtPath<Texture>(copiedTextures[texPath]);
                        matCopy.mainTexture = newTex;
                    }

                    AssetDatabase.CreateAsset(matCopy, matPath);
                    updatedMats[i] = AssetDatabase.LoadAssetAtPath<Material>(matPath);
                }

                renderer.sharedMaterials = updatedMats;
            }

            // Save prefab
            string prefabPath = Path.Combine(modelsFolder, model.name + ".prefab");
            prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabPath);
            PrefabUtility.SaveAsPrefabAsset(model, prefabPath);
        }

        Object.DestroyImmediate(instance);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("OBJ extraction complete. Check Models/, Materials/, and Textures/ folders.");
    }
}
