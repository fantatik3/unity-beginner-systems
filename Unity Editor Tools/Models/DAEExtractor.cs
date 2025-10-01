using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class DAEExtractor
{
    [MenuItem("Tools/DAE/Extract Full Contents")]
    static void ExtractDAEContents()
    {
        Object selected = Selection.activeObject;
        if (selected == null)
        {
            Debug.LogError("No DAE selected.");
            return;
        }

        string daePath = AssetDatabase.GetAssetPath(selected);
        GameObject daeRoot = AssetDatabase.LoadAssetAtPath<GameObject>(daePath);

        if (daeRoot == null || !daePath.ToLower().EndsWith(".dae"))
        {
            Debug.LogError("Selected object is not a valid DAE file.");
            return;
        }

        string baseDir = Path.GetDirectoryName(daePath);
        string assetName = Path.GetFileNameWithoutExtension(daePath);

        string modelsFolder = Path.Combine(baseDir, "Models");
        string materialsFolder = Path.Combine(baseDir, "Materials");
        string texturesFolder = Path.Combine(baseDir, "Textures");

        Directory.CreateDirectory(modelsFolder);
        Directory.CreateDirectory(materialsFolder);
        Directory.CreateDirectory(texturesFolder);

        Dictionary<string, string> copiedTextures = new Dictionary<string, string>();

        // Extract materials and relink textures
        Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(daePath);
        foreach (var asset in subAssets)
        {
            if (asset is Material mat)
            {
                string matPath = Path.Combine(materialsFolder, mat.name + ".mat");
                matPath = AssetDatabase.GenerateUniqueAssetPath(matPath);

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

        // Instantiate into scene temporarily
        GameObject tempInstance = Object.Instantiate(daeRoot);

        // Extract meshes and prefabs
        foreach (Transform child in tempInstance.transform)
        {
            GameObject meshObject = child.gameObject;

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

            if (renderer != null)
            {
                Material[] mats = renderer.sharedMaterials;
                for (int i = 0; i < mats.Length; i++)
                {
                    string matPath = Path.Combine(materialsFolder, mats[i].name + ".mat");
                    Material extractedMat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
                    if (extractedMat != null)
                        mats[i] = extractedMat;
                }
                renderer.sharedMaterials = mats;
            }

            string prefabPath = Path.Combine(modelsFolder, meshObject.name + ".prefab");
            prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabPath);
            PrefabUtility.SaveAsPrefabAsset(meshObject, prefabPath);
        }

        Object.DestroyImmediate(tempInstance);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("DAE extraction complete. Models, Materials, and Textures extracted.");
    }
}
