using UnityEngine;
using System.Collections.Generic;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.Script;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Display
{
	/// <summary>
	/// Interface for a RenPyView to interact with. Manages running a Ren'Py
	/// script as well as all of the relevant GameObject references created by
	/// the script.
	/// </summary>
	public sealed class RenPyController : MonoBehaviour
	{
		/// <summary>
		/// The script that the display will run.
		/// </summary>
		[SerializeField]
		private RenPyScriptAsset m_renPyScript;
		public RenPyScriptAsset Script
		{
			get {
				return m_renPyScript;
			}
		}

		/// <summary>
		/// The state of the script's execution.
		/// </summary>
		private RenPyState m_state;

		/// <summary>
		/// Whether or not the display is running the script
		/// </summary>
		private bool running = false;
		public bool Running
		{
			get {
				return running && m_state != null && m_state.Execution.Running;
			}
		}

		/// <summary>
		/// A reference to the music channel. Will be deprecated.
		/// </summary>
		public RenPyAudioSource m_music;
		/// <summary>
		/// A reference to the sound channel. Will be deprecated.
		/// </summary>
		public RenPyAudioSource m_sound;
		/// <summary>
		/// A reference to the voice channel. Will be deprecated.
		/// </summary>
		public RenPyAudioSource m_voice;

		/// <summary>
		/// Creates all of the GameObjects that the script will need to run.
		/// </summary>
		public void Awake()
		{
			if (m_renPyScript == null) {
				throw new System.Exception("Script property is not set!");
			}

			m_state = new RenPyState(ref m_renPyScript);

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

		/// <summary>
		/// Starts the script from the beginning.
		/// </summary>
		public void StartDialog()
		{
			StopAllCoroutines();
			running = true;

			if (m_state != null) {
				m_state.Reset();
				m_state.Execution.NextStatement(m_state);
			}
		}

		/// <summary>
		/// Stops the script.
		/// </summary>
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

		#region State Wrapper Methods

		/// <summary>
		/// Gets the speaker of the passed RenPySay statement.
		/// </summary>
		/// <param name="statement">
		/// The statement to get the speaker of.
		/// </param>
		/// <returns>
		/// The RenPyCharacterData of the speaker.
		/// </returns>
		public RenPyCharacterData GetSpeaker(RenPySay statement)
		{
			return statement.GetSpeaker(m_state);
		}

		#endregion

		#region Visual Wrapper Methods

		/// <summary>
		/// Returns the background image.
		/// </summary>
		/// <returns>
		/// The background image.
		/// </returns>
		public RenPyImageData GetBackgroundImage()
		{
			return m_state.Visual.BgImage;
		}

		/// <summary>
		/// Returns a collection of images that are on the screen.
		/// </summary>
		/// <returns>
		/// A collection of images that are on the screen.
		/// </returns>
		public Dictionary<string, RenPyImageData>.ValueCollection GetImages()
		{
			return m_state.Visual.GetImages();
		}

		/// <summary>
		/// Whether or not the dialog window should be drawn. Specifically, this
		/// is true when the current statement is a RenPySay statement or if
		/// the dialog window has been requested and the current statement is
		/// not a menu statement.
		/// </summary>
		/// <returns>
		/// True if the dialog window should be rendered.
		/// </returns>
		public bool ShouldDrawWindow()
		{
			var statement = m_state.Execution.CurrentStatement;
			if(statement == null) {
				return false;
			}
			
			if (statement.Type == RenPyStatementType.SAY) {
				return true;
			}

			if(statement.Type != RenPyStatementType.MENU) {
				return m_state.Visual.WindowRequested;
			}

			return false;
		}

		#endregion

		#region Execution Wrapper Methods

		/// <summary>
		/// Returns the current RenPyStatement.
		/// </summary>
		/// <returns>
		/// The current RenPyStatement
		/// </returns>
		public RenPyStatement GetCurrentStatement()
		{
			return m_state.Execution.CurrentStatement;
		}

		/// <summary>
		/// Executes and returns the next RenPyStatement.
		/// </summary>
		/// <returns>
		/// The next RenPyStatement.
		/// </returns>
		public RenPyStatement NextStatement()
		{
			return m_state.Execution.NextStatement(m_state);
		}

		/// <summary>
		/// Picks a choice in a RenPyMenu.
		/// </summary>
		/// <param name="menu">
		/// The menu to pick a choice in.
		/// </param>
		/// <param name="choice">
		/// The choice to pick.
		/// </param>
		public void PickChoice(RenPyMenu menu, string choice)
		{
			menu.PickChoice(m_state, choice);
		}

		#endregion
	}
}
