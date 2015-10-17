using UnityEngine;
using System;
using System.Collections.Generic;

namespace Exodrifter.Raconteur.RenPy
{
	/// <summary>
	/// Stores and serializes information about a Ren'Py script.
	/// </summary>
	[Serializable]
	public class RenPyScriptAsset : ScriptableObject,
		ISerializationCallbackReceiver
	{
		public string Title;
		public string Source;

		public Dictionary<string, AudioClip> audio = new Dictionary<string, AudioClip>();
		public Dictionary<string, Texture2D> image = new Dictionary<string, Texture2D>();

		[SerializeField]
		private List<string> audioKeys = new List<string>();
		[SerializeField]
		private List<AudioClip> audioValues = new List<AudioClip>();
		[SerializeField]
		private List<string> imageKeys = new List<string>();
		[SerializeField]
		private List<Texture2D> imageValues = new List<Texture2D>();

		public void OnBeforeSerialize()
		{
			Serialize(audio, audioKeys, audioValues);
			Serialize(image, imageKeys, imageValues);
		}

		public void OnAfterDeserialize()
		{
			Deserialize(audio, audioKeys, audioValues);
			Deserialize(image, imageKeys, imageValues);
		}

		private static void Serialize<K, V>(Dictionary<K, V> dict, List<K> keys, List<V> values)
		{
			keys.Clear();
			values.Clear();

			foreach (var pair in dict) {
				keys.Add(pair.Key);
				values.Add(pair.Value);
			}
		}

		private static void Deserialize<K, V>(Dictionary<K, V> dict, List<K> keys, List<V> values)
		{
			dict.Clear();

			if (keys.Count != values.Count)
				throw new Exception("Number of keys and values don't match!");

			for (int i = 0; i < keys.Count; i++)
				dict.Add(keys[i], values[i]);
		}
	}
}
