# SceneWindow
【Unity】シーンと連動して自動で開閉するウィンドウ

特定のシーンでの編集をアシストするツールに。メニューもスッキリ。

# 使い方

### EditorWindow に SceneWindow.Open 属性をつけるだけ


```C#
using UnityEditor;
using SceneWindowSystem;

// Hogeシーンを開くと自動で開くウィンドウ
[SceneWindow.Open("Hoge")]
public class SampleWindow : EditorWindow
{
	void OnGUI()
	{
		EditorGUILayout.LabelField("SampleWindow");
	}
}

```
