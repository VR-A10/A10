using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine.Profiling;

public class Shatoo_WorldAssetSize : EditorWindow
{
    private  List<UnityEngine.Object> objects = new List<UnityEngine.Object>();
    private List<double> sizes = new List<double>();
    private Vector2 Scrollbarheight = Vector2.zero;

    private bool onlyTexture = true;

    [MenuItem("Shatoo's Tool/World Asset Size")]
    static void Init()
    {
        Shatoo_WorldAssetSize window = (Shatoo_WorldAssetSize)GetWindow(typeof(Shatoo_WorldAssetSize), false, "World Asset Size");
        window.Show();
    }
    private void OnGUI()
    {
        onlyTexture = GUILayout.Toggle(onlyTexture, "only Texture");
        if (GUILayout.Button("Find"))
        {
            objects.Clear(); sizes.Clear();
            string path = SceneManager.GetActiveScene().path;
            string[] paths = AssetDatabase.GetDependencies(new[] { path });
            foreach (string p in paths)
            {
                UnityEngine.Object res = AssetDatabase.LoadAssetAtPath(p, typeof(UnityEngine.Object));
                if (p.Equals(path))
                {
                    continue;
                }
              
               
                double size = new FileInfo(p).Length;
                Texture2D textureImporter = res as Texture2D;
                if (onlyTexture)
                {
                    if (textureImporter != null)
                    {
                        size = double.Parse((textureImporter.GetRawTextureData().LongLength).ToString());
                        sizes.Add(size);
                        objects.Add(res);
                    }
                    continue;
                }
                if (textureImporter != null)
                {
                    size = double.Parse((textureImporter.GetRawTextureData().LongLength).ToString());
                }
                sizes.Add(size);

                objects.Add(res);
            }
            Debug.Log("Find OK");
            double temp = 0;
            UnityEngine.Object tempo = null;
            for (int i = 0; i < sizes.Count-1; i++)
            {
                for (int j = 0; j < sizes.Count-1-i; j++)
                {
                    if (sizes[j]<sizes[j+1])
                    {
                        temp = sizes[j + 1];
                        sizes[j + 1] = sizes[j];
                        sizes[j] = temp;

                        tempo = objects[j + 1];
                        objects[j + 1] = objects[j];
                        objects[j] = tempo;
                    }
                }
            }

        }
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField("Object\t\t\t\t\t\t\tSize");
        Scrollbarheight = EditorGUILayout.BeginScrollView(Scrollbarheight);
        for (int i = 0; i < objects.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            objects[i] = EditorGUILayout.ObjectField(new GUIContent("Asset"+(i+1)+":"), objects[i], typeof(UnityEngine.Object), true, GUILayout.Width(400));
            if (objects[i]!=null)
            {

                EditorGUILayout.LabelField(sizes[i].ToString());
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}
