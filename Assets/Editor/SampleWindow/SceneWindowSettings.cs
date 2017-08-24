using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SceneWindowSystem
{
	public class SceneWindowSettings
	{
		const string kFilePath = "ProjectSettings/SceneWindow.txt";

		public readonly Dictionary<string, string[]> map = new Dictionary<string, string[]>();


		//----------------------------------------------
		// accessor
		//----------------------------------------------

		public static SceneWindowSettings Create()
		{
			var settings = new SceneWindowSettings();
			if (File.Exists(kFilePath))
			{
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
							settings.map.Add(className, sceneNames);
						}
					}
				}
				catch(Exception)
				{
					settings.map.Clear();
				}
			}
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

		public bool IsTarget(string className, string sceneName)
		{
			string[] sceneNames;
			return map.TryGetValue(className, out sceneNames) && Array.IndexOf(sceneNames, sceneName) >= 0;
		}
	}
}