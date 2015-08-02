#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
namespace DPek.Raconteur.Test
{
	[InitializeOnLoad]
	public class RaconteurTests : EditorWindow
	{
		private enum Status { PASS, FAIL, RUNNING, UNKNOWN };

		private static LinkedList<MethodInfo> methods;
		private static Dictionary<MethodInfo, Status> status;
		private static Dictionary<MethodInfo, string> message;
		private static Dictionary<string, bool> toggleGroup;

		void Awake()
		{
			GetTests();
		}

		void OnGUI()
		{
			if (GUILayout.Button("Refresh Tests"))
			{
				GetTests();
			}

			if (GUILayout.Button("Run Tests"))
			{
				RunTests();
			}

			var boldtext = new GUIStyle(GUI.skin.label);
			boldtext.fontStyle = FontStyle.Bold;
			EditorGUILayout.LabelField("Tests", boldtext);

			if (null == status || 0 == status.Count)
			{
				EditorGUILayout.LabelField("No tests loaded");
				return;
			}

			var unknownStyle = new GUIStyle(GUI.skin.label);
			var failStyle = new GUIStyle(GUI.skin.label);
			failStyle.normal.textColor = Color.red;
			var passStyle = new GUIStyle(GUI.skin.label);
			passStyle.normal.textColor = Color.green;
			var runningStyle = new GUIStyle(GUI.skin.label);
			runningStyle.normal.textColor = Color.yellow;

			string classname = null;
			foreach (var method in methods)
			{
				if (classname != method.DeclaringType.Name)
				{
					classname = method.DeclaringType.Name;
					toggleGroup[classname] = EditorGUILayout.Foldout(toggleGroup[classname], classname);
				}

				if (!toggleGroup[classname])
				{
					continue;
				}

				String str = "";
				GUIStyle style = null;
				switch (status[method])
				{
					case Status.FAIL:
						style = failStyle;
						str = "[FAIL] ";
						break;
					case Status.PASS:
						style = passStyle;
						str = "[PASS] ";
						break;
					case Status.RUNNING:
						style = runningStyle;
						str = "[RUNNING] ";
						break;
					default:
						style = unknownStyle;
						str = "[UNKNOWN] ";
						break;
				}

				str += method.Name;

				if (null != message && message.ContainsKey(method))
				{
					str += ": " + message[method];
				}
				EditorGUILayout.LabelField(str, style);
			}
		}

		private Type[] GetTypesInNamespace(string nameSpace)
		{
			var types = Assembly.GetExecutingAssembly().GetTypes();
			return types.Where(t => t.Namespace == nameSpace).ToArray();
		}

		private void GetTests()
		{
			methods = methods ?? new LinkedList<MethodInfo>();
			methods.Clear();
			status = status ?? new Dictionary<MethodInfo, Status>();
			status.Clear();
			toggleGroup = toggleGroup ?? new Dictionary<string, bool>();

			var bindingFlags = BindingFlags.Public;
			bindingFlags |= BindingFlags.Static;
			bindingFlags |= BindingFlags.DeclaredOnly;

			Type[] typelist = GetTypesInNamespace("DPek.Raconteur.Test");
			for (int i = 0; i < typelist.Length; i++)
			{
				foreach (var method in typelist[i].GetMethods(bindingFlags))
				{
					if (0 == method.GetParameters().Length)
					{
						methods.AddLast(method);
						status.Add(method, Status.UNKNOWN);

						if(!toggleGroup.ContainsKey(method.DeclaringType.Name))
						{
							toggleGroup[method.DeclaringType.Name] = false;
						}
					}
				}
			}
		}

		private void RunTests()
		{
			if (null == methods || methods.Count == 0) { return; }

			message = message ?? new Dictionary<MethodInfo, string>();
			message.Clear();

			foreach (var method in methods)
			{
				try
				{
					status[method] = Status.RUNNING;
					object ret = method.Invoke(null, null);

					if (ret is bool && true == (ret as bool?))
					{
						status[method] = Status.PASS;
					}
					else
					{
						status[method] = Status.FAIL;
						message[method] = "Returned false";
					}
				}
				catch (Exception e)
				{
					status[method] = Status.FAIL;
					message[method] = e.InnerException.Message;
				}
			}
		}
	}
}
#endif
