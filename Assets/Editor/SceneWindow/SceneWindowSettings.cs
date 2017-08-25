using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace SceneWindowSystem
{
	/// <summary>
	/// Inspectorに情報表示するためだけにScriptableObjectを継承している。
	/// データ的には全くScriptableObjetである必要はない。
	/// </summary>
	public class SceneWindowSettings : ScriptableObject
	{
		const string kFilePath = "ProjectSettings/SceneWindow.txt";

		// Key:ClassName Value:ScenePaths
		public readonly Dictionary<string, string[]> map = new Dictionary<string, string[]>();


		//----------------------------------------------
		// accessor
		//----------------------------------------------

		public static SceneWindowSettings Create()
		{
			var settings = ScriptableObject.CreateInstance<SceneWindowSettings>();
			settings.Revert();
			return settings;
		}

		public void Save()
		{
			var sb = new StringBuilder();
			foreach (var kvp in map)
			{
				sb.Append(kvp.Key);
				foreach (var sceneName in kvp.Value)
				{
					sb.AppendFormat(",{0}", sceneName);
				}
				sb.AppendLine();
			}
			File.WriteAllText(kFilePath, sb.ToString(), Encoding.UTF8);			
		}

		public void Revert()
		{
			map.Clear();

			if (!File.Exists(kFilePath)) return;

			try
			{
				// Xmlでシリアライズとかの方が早いかな？
				using (var sr = new StreamReader(kFilePath, Encoding.UTF8))
				{
					string line = null;
					while ((line = sr.ReadLine()) != null)
					{
						var elements = line.Split(',');
						var className = elements[0];
						var sceneNames = new string[elements.Length - 1];
						for (int i = 0; i < sceneNames.Length; ++i)
						{
							sceneNames[i] = elements[1 + i];
						}
						map.Add(className, sceneNames);
					}
				}
			}
			catch (Exception)
			{
				map.Clear();
			}
		}

		public bool IsTarget(string className, string scenePath)
		{
			string[] scenePaths;
			return map.TryGetValue(className, out scenePaths) && Array.IndexOf(scenePaths, scenePath) >= 0;
		}
	}
}