using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.Script;
using DPek.Raconteur.Util;
using System.Collections.Generic;

namespace DPek.Raconteur.RenPy.State
{
	/// <summary>
	/// Stores the state of a Ren'Py script.
	/// </summary>
	public class RenPyState : StoryState
	{
		/// <summary>
		/// The Ren'Py script to store state for.
		/// </summary>
		private RenPyScriptAsset m_script;
		public RenPyScriptAsset Script
		{
			get {
				return m_script;
			}
		}

		/// <summary>
		/// Stores the execution state of the Ren'Py script.
		/// </summary>
		private RenPyExecutionState m_executionState;
		public RenPyExecutionState Execution
		{
			get {
				return m_executionState;
			}
		}

		/// <summary>
		/// Stores the visual state of the Ren'Py script.
		/// </summary>
		private RenPyVisualState m_visualState;
		public RenPyVisualState Visual
		{
			get {
				return m_visualState;
			}
		}

		/// <summary>
		/// Stores the aural (audio) state of the Ren'Py script.
		/// </summary>
		private RenPyAuralState m_auralState;
		public RenPyAuralState Aural
		{
			get {
				return m_auralState;
			}
		}

		/// <summary>
		/// A map of character names to RenPyCharacter objects.
		/// </summary>
		private Dictionary<string, RenPyCharacter> m_characters;

		/// <summary>
		/// A map of image names to image filenames.
		/// </summary>
		private Dictionary<string, string> m_imageFilenames;

		/// <summary>
		/// Creates a new RenPyState for the passed script.
		/// </summary>
		/// <param name="script">
		/// The script to store state for.
		/// </param>
		public RenPyState(ref RenPyScriptAsset script)
		{
			var blocks = RenPyParser.Parse(script.Lines);
			m_script = script;
			m_executionState = new RenPyExecutionState(ref blocks);
			m_visualState = new RenPyVisualState();
			m_auralState = new RenPyAuralState();

			m_characters = new Dictionary<string, RenPyCharacter>();
			m_imageFilenames = new Dictionary<string, string>();
		}

		/// <summary>
		/// Resets the state.
		/// </summary>
		public void Reset()
		{
			m_executionState.Reset();
			m_visualState.Reset();
			m_auralState.Reset();

			m_characters.Clear();
			m_imageFilenames.Clear();
		}

		#region Getters and Setters

		/// <summary>
		/// Adds a RenPyCharacter.
		/// </summary>
		/// <param name="character">
		/// The RenPyCharacter to add.
		/// </param>
		public void AddCharacter(RenPyCharacter character)
		{
			m_characters.Add(character.VarName, character);
		}

		/// <summary>
		/// Adds an image name definition.
		/// </summary>
		/// <param name="imageName">
		/// The image variable name.
		/// </param>
		/// <param name="filename">
		/// The image filename.
		/// </param>
		public void AddImageFilename(string imageName, string filename)
		{
			m_imageFilenames[imageName] = filename;
		}

		/// <summary>
		/// Gets the RenPyCharacter with the specified name.
		/// </summary>
		/// <returns>
		/// The RenPyCharacter with the specified name.
		/// </returns>
		/// <param name="characterVarName">
		/// The name of the RenPyCharacter.
		/// </param>
		public RenPyCharacter GetCharacter(string characterVarName)
		{
			return m_characters[characterVarName];
		}

		/// <summary>
		/// Returns the filename associated with an image name.
		/// </summary>
		/// <param name="imageName">
		/// The image variable name.
		/// </param>
		/// <returns>
		/// The image filename.
		/// </returns>
		internal string GetImageFilename(string imageName)
		{
			return m_imageFilenames[imageName];
		}

		/// <summary>
		/// Gets the value of the specified variable.
		/// </summary>
		/// <returns>
		/// The value of the specified variable.
		/// </returns>
		/// <param name="name">
		/// The name of the variable to get the value of.
		/// </param>
		public override string GetVariable(string name)
		{
			if (Static.Vars.ContainsKey(name))
			{
				return Static.Vars[name];
			}
			else
			{
				throw new UndefinedVariableException(name);
			}
		}

		/// <summary>
		/// Sets the specified variable to the specified value.
		/// </summary>
		/// <param name="name">
		/// The name of the variable to set the value of.
		/// </param>
		/// <param name="value">
		/// The value to set the variable to.
		/// </param>
		public override void SetVariable(string name, string value)
		{
			if (!Static.Vars.ContainsKey(name))
			{
				Static.Vars.Add(name, value);
			}
			else
			{
				Static.Vars[name] = value;
			}
		}

		/// <summary>
		/// Deletes the specified variable.
		/// </summary>
		/// <param name="name">
		/// The name of the variable to delete.
		/// </param>
		public override void DeleteVariable(string name)
		{
			Static.Vars.Remove(name);
		}

		#endregion
	}
}
