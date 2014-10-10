using UnityEngine;
using System;
using System.Collections.Generic;

namespace DPek.Raconteur.RenPy.Parser
{
	/// <summary>
	/// Stores information about a Ren'Py script.
	/// </summary>
	[Serializable]
	public class RenPyScriptAsset : ScriptableObject
	{
		public string Title;
		public string[] Lines;

		public string[] audioKeys;
		public AudioClip[] audioValues;

		public string[] imageKeys;
		public Texture2D[] imageValues;

		public bool HasAudioClip(string str)
		{
			foreach (string key in audioKeys) {
				if (key == str) {
					return true;
				}
			}
			return false;
		}

		public AudioClip GetAudioClip(string str)
		{
			for (int i = 0; i < audioKeys.Length; i++) {
				if (audioKeys[i] == str) {
					return audioValues[i];
				}
			}

			return null;
		}

		public bool HasImage(string str)
		{
			foreach (string key in imageKeys) {
				if (key == str) {
					return true;
				}
			}
			return false;
		}

		public Texture2D GetImage(string str)
		{
			for (int i = 0; i < imageKeys.Length; i++) {
				if (imageKeys[i] == str) {
					return imageValues[i];
				}
			}

			return null;
		}
	}
}
