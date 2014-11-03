using System.Collections.Generic;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.Script;

namespace DPek.Raconteur.RenPy.State
{
	/// <summary>
	/// Stores the state of a Ren'Py script.
	/// </summary>
	public class RenPyState
	{
		/// <summary>
		/// Stores the data of the Ren'Py script.
		/// </summary>
		private RenPyScriptAsset m_data;
		public RenPyScriptAsset Data
		{
			get {
				return m_data;
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

		public RenPyState(ref RenPyScriptAsset data)
		{
			m_data = data;
			m_executionState = new RenPyExecutionState(ref data.Blocks);
			m_visualState = new RenPyVisualState();
			m_auralState = new RenPyAuralState();

			m_characters = new Dictionary<string, RenPyCharacter>();
			m_imageFilenames = new Dictionary<string, string>();
		}

		public void Reset()
		{
			m_executionState.Reset();
			m_visualState.Reset();

			m_characters.Clear();
			m_imageFilenames.Clear();
		}

		#region Getters and Setters

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
		/// The value of the specified variable or an empty string if the
		/// variable does not exist.
		/// </returns>
		/// <param name="name">
		/// The name of the variable to get the value of.
		/// </param>
		public string GetVariable(string name)
		{
			return Static.Vars.ContainsKey(name) ? Static.Vars[name] : "";
		}

		/// <summary>
		/// Sets the specified variable to the specified value.
		/// </summary>
		public void SetVariable(string name, string value)
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

		#endregion
	}
}
