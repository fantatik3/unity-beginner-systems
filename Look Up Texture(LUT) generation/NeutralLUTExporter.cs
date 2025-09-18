using UnityEngine;
using System.IO;

public class NeutralLUTExporter : MonoBehaviour
{
    [SerializeField] int lutSize = 64;

    void Start()
    {
        Texture2D lut = GenerateTiled2DLUT(lutSize);
        SaveTextureAsPNG(lut, Application.dataPath + $"/TiledLUT_{lutSize}.png");
        Debug.Log("Tiled 2D LUT generated and saved.");
    }

    Texture2D GenerateTiled2DLUT(int size)
    {
        int grid = Mathf.CeilToInt(Mathf.Sqrt(size)); // e.g. 8 for 64³
        int texWidth = grid * size;
        int texHeight = grid * size;

        Texture2D tex = new Texture2D(texWidth, texHeight, TextureFormat.RGBAFloat, false, true);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Point;

        Color[] pixels = new Color[texWidth * texHeight];

        for (int b = 0; b < size; b++)
        {
            int tileX = b % grid;
            int tileY = b / grid;

            for (int g = 0; g < size; g++)
            {
                for (int r = 0; r < size; r++)
                {
                    float fr = r / (float)(size - 1);
                    float fg = g / (float)(size - 1);
                    float fb = b / (float)(size - 1);

                    int x = tileX * size + r;
                    int y = tileY * size + g;

                    int index = x + y * texWidth;
                    pixels[index] = new Color(fr, fg, fb, 1.0f);
                }
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

    void SaveTextureAsPNG(Texture2D tex, string path)
    {
        byte[] data = tex.EncodeToPNG();
        File.WriteAllBytes(path, data);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}