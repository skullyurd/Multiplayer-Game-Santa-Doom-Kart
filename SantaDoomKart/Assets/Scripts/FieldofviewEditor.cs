using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(FieldOfView_AI))]
public class FieldofviewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView_AI fow = (FieldOfView_AI)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirfromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirfromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
    }
}
#endif
