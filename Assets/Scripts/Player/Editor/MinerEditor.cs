using Level;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player.Editor
{
    [CustomEditor(typeof(Miner))]
    public class MinerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            GUILayout.Space(20);
            if (GUILayout.Button("Generate check spheres"))
            {
                ((Miner)target).GenerateChecks();
            }
        }
    }
}