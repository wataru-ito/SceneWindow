using UnityEditor;
using SceneWindowSystem;

public class Sample01Window : SceneWindow<Sample01Window>
{
	[InitializeOnLoadMethod]
	static void Init()
	{
		OnInitializeOnLoadMethod();
	}

	void OnGUI()
	{
		EditorGUILayout.LabelField("Sample01Window");
	}
}
