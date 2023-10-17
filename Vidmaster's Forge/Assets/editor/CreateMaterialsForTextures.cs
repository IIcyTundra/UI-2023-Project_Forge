
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public static class CreateMaterialsForTextures
{
    [MenuItem("Assets/Create Complex Material")]
    public static void CreateComplexDiffuseMaterial()
    {
        var selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        var cnt = selectedAsset.Length * 1.0f;
        var idx = 0f;
        List<Texture2D> tx2D = new List<Texture2D>();
        foreach (Object obj in selectedAsset)
        {
            idx++;
            EditorUtility.DisplayProgressBar("Create material", "Create material for: " + obj.name, idx / cnt);

            if (obj is Texture2D)
            {
                tx2D.Add(obj as Texture2D);
            }

        }
        CreateComplexMatFromTx(tx2D, Shader.Find("Standard"));
        EditorUtility.ClearProgressBar();
    }
    static void CreateComplexMatFromTx(List<Texture2D> tx2D, Shader shader)
    {
        Texture2D albedo = null;
        Texture2D metallic = null;
        Texture2D normal = null;
        Texture2D occlusion = null;
        foreach (Texture2D tex in tx2D)
        {
            string n = tex.name.ToLower();
            n = n.Substring(n.IndexOf("_"));
            if (n.Contains("albedo"))
            {
                albedo = tex;
            }
            else if (n.Contains("metallic"))
            {
                metallic = tex;
            }
            else if (n.Contains("normal"))
            {
                normal = tex;
            }
            else if (n.Contains("occlusion"))
            {
                occlusion = tex;
            }
        }
        var path = AssetDatabase.GetAssetPath(tx2D[0]);
        if (File.Exists(path))
        {
            path = Path.GetDirectoryName(path);
        }
        var mat = new Material(shader) { mainTexture = albedo };
        mat.SetTexture("_BumpMap", normal);
        mat.SetTexture("_MetallicGlossMap", metallic);
        mat.SetTexture("_OcclusionMap", occlusion);
        string matTitle = albedo.name.Split('_')[0];
        AssetDatabase.CreateAsset(mat, Path.Combine(path, string.Format("{0}.mat", matTitle)));
    }
}

