using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class FBXExtractor
{
    [MenuItem("Tools/FBX/Extract Full Contents")]
    static void ExtractFBXContents()
    {
        Object selected = Selection.activeObject;
        if (selected == null)
        {
            Debug.LogError("No FBX selected.");
            return;
        }

        string fbxPath = AssetDatabase.GetAssetPath(selected);
        GameObject fbxRoot = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);

        if (fbxRoot == null || !fbxPath.EndsWith(".fbx"))
        {
            Debug.LogError("Selected object is not a valid FBX.");
            return;
        }

        string baseDir = Path.GetDirectoryName(fbxPath);
        string assetName = Path.GetFileNameWithoutExtension(fbxPath);

        string modelsFolder = Path.Combine(baseDir, "Models");
        string materialsFolder = Path.Combine(baseDir, "Materials");
        string texturesFolder = Path.Combine(baseDir, "Textures");

        Directory.CreateDirectory(modelsFolder);
        Directory.CreateDirectory(materialsFolder);
        Directory.CreateDirectory(texturesFolder);

        // Track textures we've already copied to prevent duplicates
        Dictionary<string, string> copiedTextures = new Dictionary<string, string>();

        // Extract Materials & Textures
        Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(fbxPath);
        foreach (var asset in subAssets)
        {
            if (asset is Material mat)
            {
                string matPath = Path.Combine(materialsFolder, mat.name + ".mat");
                matPath = AssetDatabase.GenerateUniqueAssetPath(matPath);

                // Clone the material so we don't overwrite the FBX internal one
                Material matCopy = Object.Instantiate(mat);

                Shader shader = matCopy.shader;
                int propertyCount = ShaderUtil.GetPropertyCount(shader);

                for (int i = 0; i < propertyCount; i++)
                {
                    if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                    {
                        string propName = ShaderUtil.GetPropertyName(shader, i);
                        Texture tex = matCopy.GetTexture(propName);

                        if (tex != null)
                        {
                            string texPath = AssetDatabase.GetAssetPath(tex);

                            if (!copiedTextures.TryGetValue(texPath, out string newTexPath))
                            {
                                newTexPath = Path.Combine(texturesFolder, Path.GetFileName(texPath));
                                newTexPath = AssetDatabase.GenerateUniqueAssetPath(newTexPath);
                                AssetDatabase.CopyAsset(texPath, newTexPath);
                                copiedTextures[texPath] = newTexPath;
                            }

                            Texture newTex = AssetDatabase.LoadAssetAtPath<Texture>(copiedTextures[texPath]);
                            matCopy.SetTexture(propName, newTex);
                        }
                    }
                }

                AssetDatabase.CreateAsset(matCopy, matPath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Instantiate FBX into scene (temporarily)
        GameObject tempInstance = Object.Instantiate(fbxRoot);

        // Extract Meshes and Prefabs
        foreach (Transform child in tempInstance.transform)
        {
            GameObject meshObject = child.gameObject;

            // Save Mesh as new asset
            MeshFilter filter = meshObject.GetComponent<MeshFilter>();
            MeshRenderer renderer = meshObject.GetComponent<MeshRenderer>();

            if (filter != null && filter.sharedMesh != null)
            {
                Mesh meshCopy = Object.Instantiate(filter.sharedMesh);
                string meshPath = Path.Combine(modelsFolder, meshObject.name + ".mesh");
                meshPath = AssetDatabase.GenerateUniqueAssetPath(meshPath);
                AssetDatabase.CreateAsset(meshCopy, meshPath);
                filter.sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
            }

            // Reassign extracted materials
            if (renderer != null)
            {
                Material[] mats = renderer.sharedMaterials;
                for (int i = 0; i < mats.Length; i++)
                {
                    string matPath = Path.Combine(materialsFolder, mats[i].name + ".mat");
                    Material extractedMat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
                    if (extractedMat != null)
                    {
                        mats[i] = extractedMat;
                    }
                }
                renderer.sharedMaterials = mats;
            }

            // Save prefab
            string prefabPath = Path.Combine(modelsFolder, meshObject.name + ".prefab");
            prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabPath);
            PrefabUtility.SaveAsPrefabAsset(meshObject, prefabPath);
        }

        Object.DestroyImmediate(tempInstance);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("FBX extraction complete. Models, Materials, and Textures extracted.");
    }
}
