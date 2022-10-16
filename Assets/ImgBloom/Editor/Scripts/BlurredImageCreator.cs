using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace ImgBloom.Editor
{
    internal static class BlurredImageCreator
    {
        [MenuItem("Tools/ImgBloom/Generate Blurred Image")]
        internal static void CreateBlurredTexture()
        {
            Texture2D selected = Selection.activeObject as Texture2D;
            if (selected == null)
            {
                Debug.LogWarning($"You need to select a texture.");
                return;
            }
            Texture2D generated = CreateBlurredTexture(selected);
            string path = EditorUtility.SaveFilePanelInProject("Save blurred image", selected.name, "png", "Enter saving file name");
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            byte[] pngData = generated.EncodeToPNG();
            if (pngData == null || pngData.Length == 0)
            {
                Debug.LogError("Failed to get encoded bytes");
                return;
            }

            File.WriteAllBytes(path, pngData);
            AssetDatabase.Refresh();
            
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (textureImporter != null)
            {
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.SaveAndReimport();
            }
        }
        
        internal static Texture2D CreateBlurredTexture(Texture2D source, float sig = 7.5f)
        {
            using TextureReadableScope readableScope = new TextureReadableScope(source);
            sig = Mathf.Max(sig, 0f);
            int w = source.width;
            int h = source.height;
            int wm = (int)(Mathf.Ceil(3.0f * sig) * 2 + 1);
            int rm = (wm - 1) / 2;

            //フィルタ
            float[] msk = new float[wm];

            sig = 2 * sig * sig;
            float div = Mathf.Sqrt(sig * Mathf.PI);

            //フィルタの作成
            for (int x = 0; x < wm; x++)
            {
                int p = (x - rm) * (x - rm);
                msk[x] = Mathf.Exp(-p / sig) / div;
            }

            var src = source.GetPixels(0).Select(x => x.a).ToArray();
            var tmp = new float[src.Length];
            var dst = new Color[src.Length];

            //垂直方向
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    float sum = 0;
                    for (int i = 0; i < wm; i++)
                    {
                        int p = y + i - rm;
                        if (p < 0 || p >= h) continue;
                        sum += msk[i] * src[x + p * w];
                    }
                    tmp[x + y * w] = sum;
                }
            }
            //水平方向
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    float sum = 0;
                    for (int i = 0; i < wm; i++)
                    {
                        int p = x + i - rm;
                        if (p < 0 || p >= w) continue;
                        sum += msk[i] * tmp[p + y * w];
                    }
                    dst[x + y * w] = new Color(1, 1, 1, sum);
                }
            }

            var createTexture = new Texture2D(w, h);
            createTexture.SetPixels(dst);
            createTexture.Apply();

            return createTexture;
        }
    }
}
