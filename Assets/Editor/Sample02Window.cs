using UnityEditor;

[SceneWindow.Open("Sample02")]
public class Sample02Window : EditorWindow
{
	void OnGUI()
	{
		EditorGUILayout.LabelField("Sample02Window");
	}
}
