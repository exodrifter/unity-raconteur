using UnityEngine;
using System.Collections.Generic;

using DPek.Raconteur.RenPy.Script;

namespace DPek.Raconteur.Twine.Parser
{
	/// <summary>
	/// Stores and serializes information about a Twine script.
	/// </summary>
	[System.Serializable]
	public class TwineScriptAsset : ScriptableObject
	{
		public string content;
	}
}
