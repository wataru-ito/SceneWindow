using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace SceneWindowSystem
{
	/// <summary>
	/// 対象シーンと連動いて開閉するウィンドウ
	/// </summary>
	public class SceneWindow<T> : EditorWindow, IHasCustomMenu
		where T : EditorWindow
	{
		//----------------------------------------------
		// static function
		//----------------------------------------------

		static T s_instance;

		protected static void OnInitializeOnLoadMethod()
		{
			EditorSceneManager.sceneOpened += (scene, mode) =>
			{
				if (mode == OpenSceneMode.Single)
				{
					if (IsInTargetScene())
					{
						OpenWindow();
					}
					else if (s_instance)
					{
						s_instance.Close();
						s_instance = null;
					}
				}
			};

			// この瞬間だとまだUnityがインスタンス認識していないらしく多重で開いてしまうので１フレーム待つ
			EditorApplication.delayCall += () =>
			{
				if (IsInTargetScene())
				{
					OpenWindow();
				}
			};
		}

		protected static void OpenWindow()
		{
			GetWindow<T>();
		}

		static bool IsInTargetScene()
		{
			var settings = SceneWindowSettings.Create();
			return settings.IsTarget(typeof(T).Name, EditorSceneManager.GetActiveScene().path);
		}


		//----------------------------------------------
		// IHasCustomMenu
		//----------------------------------------------

		public void AddItemsToMenu(GenericMenu menu)
		{
			menu.AddItem(new GUIContent("設定を開く"), false, SceneWindowSettingsInspector.Open);
		}
		              

		//----------------------------------------------
		// unity system function
		//----------------------------------------------

		protected virtual void OnEnable()
		{
			s_instance = this as T;
		}

		protected virtual void OnDestroy()
		{
			if (IsInTargetScene())
			{
				EditorApplication.delayCall += OpenWindow;
			}
		}
	}
}