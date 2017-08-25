using UnityEditor;
using SceneWindowSystem;

public class Sample02Window : SceneWindow<Sample02Window>
{
	[InitializeOnLoadMethod]
	static void Init()
	{
		OnInitializeOnLoadMethod();
	}

	void OnGUI()
	{
		EditorGUILayout.LabelField("Sample02Window");
	}
}
