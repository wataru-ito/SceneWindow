using UnityEditor;

[SceneWindow.Open("Sample01")]
public class Sample01Window : EditorWindow
{
	void OnGUI()
	{
		EditorGUILayout.LabelField("Sample01Window");
	}
}
