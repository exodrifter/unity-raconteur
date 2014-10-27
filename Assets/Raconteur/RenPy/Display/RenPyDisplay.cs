using UnityEngine;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.Script;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Display
{
	/// <summary>
	/// Interface for a RenPyState and the corresponding Unity gameobjects.
	/// </summary>
	public class RenPyDisplay : MonoBehaviour
	{
		[SerializeField]
		private RenPyScriptAsset m_renPyScript;
		public RenPyScriptAsset Script
		{
			get {
				return m_renPyScript;
			}
		}

		private RenPyState m_state;
		public RenPyState State
		{
			get {
				return m_state;
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

		public RenPyAudioSource m_music;
		public RenPyAudioSource m_sound;
		public RenPyAudioSource m_voice;

		public void Awake()
		{
			// Parse the Ren'Py script
			// TODO: Do this when the asset is imported instead of at run-time
			if (m_renPyScript == null) {
				Debug.LogWarning("RenPy script is null!");
				return;
			}
			m_state = RenPyParser.Parse(ref m_renPyScript);

			// Create the children gameobjects
			Transform parentTransform = this.gameObject.transform.parent;
			GameObject parent = null;
			if (parentTransform != null) {
				parent = parentTransform.gameObject;
			}
			var str = name + " Helpers";
			GameObject helperParent = CreateChildGameObject(parent, str);

			if (m_music == null)
			{
				GameObject go = CreateChildGameObject(helperParent, "Music");
				m_music = go.AddComponent<RenPyAudioSource>();
				m_music.m_state = m_state;
				m_music.m_channel = "music";
			}

			if (m_sound == null)
			{
				GameObject go = CreateChildGameObject(helperParent, "Sounds");
				m_sound = go.AddComponent<RenPyAudioSource>();
				m_sound.m_state = m_state;
				m_sound.m_channel = "sound";
			}

			if (m_voice == null)
			{
				GameObject go = CreateChildGameObject(helperParent, "Voice");
				m_voice = go.AddComponent<RenPyAudioSource>();
				m_voice.m_state = m_state;
				m_voice.m_channel = "voice";
			}
		}

		public void StartDialog()
		{
			StopAllCoroutines();
			running = true;

			m_state.Reset();
			m_state.Execution.NextStatement(m_state);
		}

		public void StopDialog()
		{
			running = false;
		}

		private GameObject CreateChildGameObject(GameObject parent, string name)
		{
			GameObject go = new GameObject();
			go.name = name;
			if (parent != null)
			{
				go.transform.parent = parent.transform;
			}
			return go;
		}
	}
}
