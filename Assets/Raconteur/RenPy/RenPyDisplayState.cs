using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DPek.Raconteur.RenPy.Dialog;
using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.Script;

namespace DPek.Raconteur.RenPy
{
	public class RenPyDisplayState : MonoBehaviour
	{
		[SerializeField]
		private RenPyScriptAsset m_renPyScript;
		public RenPyScriptAsset RenPyScript
		{
			get
			{
				return m_renPyScript;
			}
		}

		private RenPyDialogState m_state;
		public RenPyDialogState State
		{
			get {
				return running ? m_state : null;
			}
		}

		private AudioSource m_music;
		public AudioSource Music
		{
			get {
				return m_music;
			}
		}

		private AudioSource m_sound;
		public AudioSource Sound
		{
			get {
				return m_sound;
			}
		}

		private bool running = false;
		public bool Running
		{
			get
			{
				return running;
			}
		}

		private GameObject m_coroutines;
		private RenPyCoroutine m_corMusic;
		private RenPyCoroutine m_corSound;

		public void Awake()
		{
			// Parse the Ren'Py script
			// TODO: Do this when the asset is imported instead of at run-time
			if (m_renPyScript == null) {
				Debug.LogWarning("RenPy script is null!");
				return;
			}
			m_state = RenPyParser.Parse(ref m_renPyScript);

			// Create the coroutines
			Transform parentTransform = this.gameObject.transform.parent;
			GameObject parent = null;
			if (parentTransform != null) {
				parent = parentTransform.gameObject;
			}
			GameObject coroutineParent = CreateChildGameObject(parent,
			                             name + " Coroutines");

			GameObject go = CreateChildGameObject(coroutineParent, "Music");
			m_music = go.AddComponent<AudioSource>();
			m_music.loop = true;

			go = CreateChildGameObject(coroutineParent, "Sounds");
			m_sound = go.AddComponent<AudioSource>();
			m_sound.loop = false;

			m_coroutines = CreateChildGameObject(coroutineParent, "Coroutines");
			go = CreateChildGameObject(m_coroutines, "Music Coroutine");
			m_corMusic = go.AddComponent<RenPyCoroutine>();
			go = CreateChildGameObject(m_coroutines, "Sound Coroutine");
			m_corSound = go.AddComponent<RenPyCoroutine>();
		}

		public void StartDialog()
		{
			StopAllCoroutines();
			running = true;

			m_state.Reset();
			m_state.NextLine(this);
		}

		public void StopDialog()
		{
			running = false;
			StartCoroutine(FadeOutAudioSource(m_music));
			StartCoroutine(FadeOutAudioSource(m_sound));
		}

		private IEnumerator FadeOutAudioSource(AudioSource source)
		{
			const float time = 2;
			float elapsed = 0;
			while (elapsed < time) {
				yield return new WaitForEndOfFrame();
				elapsed += Time.deltaTime;
				source.volume = 1 - (elapsed / time);
			}

			source.volume = 0;
		}

		private GameObject CreateChildGameObject(GameObject parent, string name)
		{
			GameObject go = new GameObject();
			go.name = name;
			if (parent != null) {
				go.transform.parent = parent.transform;
			}
			return go;
		}

		/// <summary>
		/// Starts a Coroutine for a RenPyPlay line.
		/// </summary>
		/// <param name="category">
		/// The channel to play the audio in.
		/// </param>
		/// <param name="routine">
		/// The Coroutine that will play the audio.
		/// </param>
		/// <returns>
		/// The Coroutine.
		/// </returns>
		public Coroutine BeginPlayCoroutine(string category, IEnumerator routine)
		{
			switch (category) {
				case "music":
					return m_corMusic.BeginCoroutine(routine);
				case "sound":
					return m_corSound.BeginCoroutine(routine);
			}
			return null;
		}
	}
}
