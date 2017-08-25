using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace SceneWindowSystem
{
	[CustomEditor(typeof(SceneWindowSettings))]
	public class SceneWindowSettingsInspector : Editor
	{
		readonly GUILayoutOption kButtonWidth = GUILayout.Width(36);

		SceneWindowSettings m_settings;
		string[] m_sceneList;

		List<string> m_remove = new List<string>();
		Dictionary<string, string> m_rename = new Dictionary<string, string>();
		Dictionary<string, string[]> m_editscene = new Dictionary<string, string[]>();

		//----------------------------------------------
		// static function
		//----------------------------------------------

		[MenuItem("Edit/Project Settings/SceneWindow")]
		public static void Open()
		{
			var setting = SceneWindowSettings.Create();
			Selection.activeObject = setting;
		}

		//----------------------------------------------
		// unity system function
		//----------------------------------------------

		void OnEnable()
		{
			m_settings = target as SceneWindowSettings;

			m_sceneList = Array.ConvertAll(
				AssetDatabase.FindAssets("t:scene"),
				i => Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(i)));
		}

		void OnDisable()
		{
			if (m_settings != null)
			{
				m_settings.Save();
			}
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUIUtility.labelWidth = 70;

			StartEditing();

			foreach (var kvp in m_settings.map)
			{
				using (new EditorGUILayout.HorizontalScope())
				using (var check = new EditorGUI.ChangeCheckScope())
				{
					var className = EditorGUILayout.TextField("ClassName", kvp.Key);
					if (check.changed)
					{
						if (m_rename == null)
							m_rename = new Dictionary<string, string>();
						m_rename[kvp.Key] = className;
					}

					if (GUILayout.Button("削除", EditorStyles.miniButton, kButtonWidth))
					{
						m_remove.Add(className);
					}
				}

				using (var check = new EditorGUI.ChangeCheckScope())
				{
					var sceneNames = kvp.Value;

					++EditorGUI.indentLevel;
					for (int i = 0; i < sceneNames.Length; ++i)
					{
						using (new EditorGUILayout.HorizontalScope())
						{
							sceneNames[i] = SceneNamePopup(sceneNames[i]);
							if (GUILayout.Button("削除", EditorStyles.miniButton, kButtonWidth))
							{
								ArrayUtility.RemoveAt(ref sceneNames, i--);
							}
						}
					}
					--EditorGUI.indentLevel;

					using (new EditorGUILayout.HorizontalScope())
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("シーン追加"))
						{
							ArrayUtility.Add(ref sceneNames, string.Empty);
						}
					}

					if (check.changed)
					{
						if (m_editscene == null)
							m_editscene = new Dictionary<string, string[]>();
						m_editscene.Add(kvp.Key, sceneNames);
					}
				}
			}

			if (GUILayout.Button("ウィンドウ追加"))
			{
				m_settings.map.Add("ClassName", new string[0]);
			}

			StopEditting();
		}

		string SceneNamePopup(string sceneName)
		{
			var index = EditorGUILayout.Popup(Array.IndexOf(m_sceneList, sceneName), m_sceneList);
			return index >= 0 ? m_sceneList[index] : string.Empty;
		}


		//----------------------------------------------
		// editing
		//----------------------------------------------

		void StartEditing()
		{
			m_remove.Clear();
			m_rename.Clear();
			m_editscene.Clear();
		}

		void StopEditting()
		{
			if (m_rename.Count > 0)
			{
				foreach (var kvp in m_rename)
				{
					if (m_settings.map.ContainsKey(kvp.Value)) continue;

					var tmp = m_settings.map[kvp.Key];
					m_settings.map.Remove(kvp.Key);
					m_settings.map.Add(kvp.Value, tmp);
				}
				m_rename.Clear();
			}

			if (m_editscene.Count > 0)
			{
				foreach (var kvp in m_editscene)
				{
					m_settings.map[kvp.Key] = kvp.Value;
				}
				m_editscene.Clear();
			}

			if (m_remove.Count > 0)
			{
				foreach (var className in m_remove)
				{
					m_settings.map.Remove(className);
				}
				m_remove.Clear();
			}
		}
	}
}