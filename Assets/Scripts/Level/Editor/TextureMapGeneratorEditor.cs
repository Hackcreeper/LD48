using UnityEditor;
using UnityEngine;

namespace Level.Editor
{
    [CustomEditor(typeof(TextureMapGenerator))]
    public class TextureMapGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            GUILayout.Space(20);
            if (!GUILayout.Button("Generate texture map"))
            {
                return;
            }
            
            ((TextureMapGenerator)target).Generate();
                
            foreach (var tile in ((TextureMapGenerator) target).tiles)
            {
                EditorUtility.SetDirty(tile);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}