using System.Collections.Generic;

using DPek.Raconteur.RenPy.Script;

namespace DPek.Raconteur.RenPy.Dialog
{
	/// <summary>
	/// Stores the state of a Ren'Py script.
	/// </summary>
	public class RenPyDialogState
	{
		#region Member Variables

		/// <summary>
		/// The index of the current line in the Ren'Py script.
		/// </summary>
		private int m_index;

		/// <summary>
		/// The Ren'Py lines of this dialog.
		/// </summary>
		private readonly RenPyLine[] m_lines;

		/// <summary>
		/// A map of character names to RenPyCharacter objects.
		/// </summary>
		private Dictionary<string, RenPyCharacter> m_characters;

		/// <summary>
		/// A map of image names to image filenames.
		/// </summary>
		private Dictionary<string, string> m_imageFilenames;

		/// <summary>
		/// A list of images that should be visible as a result of show and hide 
		/// commands.
		/// </summary>
		private Dictionary<string, RenPyDialogImage> m_images;

		/// <summary>
		/// A map of label names to the line index that label is on.
		/// </summary>
		private Dictionary<string, int> m_labels;

		/// <summary>
		/// Returns the current line of the Ren'Py dialog. Returns null if the
		/// current line index is invalid.
		/// </summary>
		public RenPyLine CurrentLine
		{
			get {
				if (m_index >= m_lines.Length || m_index < 0) {
					return null;
				}
				return m_lines[m_index];
			}
		}

		#endregion

		/// <summary>
		/// Creates a new RenPyDialogState with the passed script.
		/// </summary>
		/// <param name="lines">
		/// The Ren'Py script as an array of RenPyLine objects.
		/// <param>
		public RenPyDialogState(RenPyLine[] lines)
		{
			this.m_lines = lines;
			Reset();
		}

		/// <summary>
		/// Moves the state to the next line in the Ren'Py script.
		/// </summary>
		/// <param name="display">
		/// The display that owns this RenPyDialogState.
		/// </param>
		/// <param name="execute">
		/// Whether or not to execute the next line.
		/// </param>
		// TODO: Get rid of the "execute" param (used as a hack for RenPyIf.cs)
		public void NextLine(RenPyDisplayState display, bool execute = true)
		{
			m_index++;
			if (execute && CurrentLine != null) {
				Static.Log(CurrentLine.ToString());
				CurrentLine.Execute(display);
			}
		}

		/// <summary>
		/// Resets the state of the RenPyDialogState.
		/// </summary>
		public void Reset()
		{
			this.m_characters = new Dictionary<string, RenPyCharacter>();
			this.m_images = new Dictionary<string,RenPyDialogImage>();
			this.m_imageFilenames = new Dictionary<string, string>();

			this.m_labels = new Dictionary<string, int>();
			for (int i = 0; i < m_lines.Length; i++) {
				RenPyLabel label = m_lines[i] as RenPyLabel;
				if (label == null) {
					continue;
				} else {
					m_labels.Add(label.Name, i);
				}
			}

			m_index = -1;
		}

		/// <summary>
		/// Goes to the specified label in the Ren'Py script.
		/// </summary>
		/// <returns>
		/// True if the label exists.
		/// </returns>
		/// <param name="display">
		/// The display that owns this RenPyDialogState.
		/// <param>
		/// <param name="label">
		/// The label to jump to.
		/// </param>
		public bool GoToLabel(RenPyDisplayState display, string label)
		{
			if (m_labels.ContainsKey(label)) {
				m_index = m_labels[label];
				Static.Log(CurrentLine.ToString());
				CurrentLine.Execute(display);
				return true;
			}
			return false;
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
		/// Returns a list of images that should be visible as a result of show
		/// and hide commands.
		/// </summary>
		/// <returns>
		/// A list of images that should be visible as a result of show and hide
		/// commands.
		/// </returns>
		public Dictionary<string, RenPyDialogImage>.ValueCollection GetImages()
		{
			return m_images.Values;
		}

		/// <summary>
		/// Adds the image with the specified variable.
		/// </summary>
		/// <param name="imageName">
		/// The image variable name.
		/// </param>
		public void AddImage(string imageName, ref RenPyDialogImage image)
		{
			string tag = imageName.Split(' ')[0];
			m_images[tag] = image;
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
		/// The value of the specified variable.
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
			if (!Static.Vars.ContainsKey(name)) {
				Static.Vars.Add(name, value);
			} else {
				Static.Vars[name] = value;
			}
		}

		#endregion
	}
}
