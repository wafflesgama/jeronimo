using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DetectionController))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        DetectionController controller = (DetectionController)target;
        Handles.color = Color.white;
		Handles.DrawWireArc(controller.transform.position, Vector3.up, Vector3.forward, 360, controller.viewRadius);
		Vector3 viewAngleA = controller.DirFromAngle(-controller.viewAngle / 2, false);
		Vector3 viewAngleB = controller.DirFromAngle(controller.viewAngle / 2, false);

		Handles.DrawLine(controller.transform.position, controller.transform.position + viewAngleA * controller.viewRadius);
		Handles.DrawLine(controller.transform.position, controller.transform.position + viewAngleB * controller.viewRadius);

		Handles.color = Color.red;
		foreach (Transform visibleTarget in controller.visibleTargets)
		{
			Handles.DrawLine(controller.transform.position, visibleTarget.position);
		}
	}
}
