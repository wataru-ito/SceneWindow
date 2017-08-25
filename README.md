# SceneWindow
【Unity】シーンと連動して自動で開閉するウィンドウ

特定のシーンでの編集をアシストするツールに。メニューもスッキリ。

# 使い方

### EditorWindow側の準備
1. SceneWindowSystem.SceneWindow< T > を継承する
2. [InitializeOnLoadMethod]属性で OnInitializeOnLoadMethod() を呼ぶ
  
### EditorWindowとSceneを関連づけ
1. MENU[Edit/Project Settings/SceneMenu]から設定


```C#
using UnityEditor;
using SceneWindowSystem;

public class SampleWindow : SceneWindow<SampleWindow>
{
	[InitializeOnLoadMethod]
	static void Init()
	{
		OnInitializeOnLoadMethod();
	}

	void OnGUI()
	{
		EditorGUILayout.LabelField("SampleWindow");
	}
}

```
