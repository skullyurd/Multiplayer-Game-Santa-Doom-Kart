using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


#if UNITY_EDITOR
    [CustomEditor(typeof(AutomaticAdjustAnchorToCorners))]
    public class AutomaticAdjustAnchorToCornersOnEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            AutomaticAdjustAnchorToCorners myScript = (AutomaticAdjustAnchorToCorners)target;

            if (GUILayout.Button("Adjust Anchors To Corners"))
            {
                myScript.AdjustAnchorsToCorners();
            }
            if (GUI.changed) EditorUtility.SetDirty(target);
            SceneView.RepaintAll();
        }
    }
#endif