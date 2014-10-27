using UnityEngine;
using System.Collections.Generic;

using DPek.Raconteur.RenPy.Util;

namespace DPek.Raconteur.RenPy.State
{
	/// <summary>
	/// Stores the visual state of a Ren'Py script.
	/// </summary>
	public class RenPyVisualState
	{
		/// <summary>
		/// The current background image or null if there is none.
		/// </summary>
		public RenPyImageData m_bgImage;

		/// <summary>
		/// A list of images that should be visible as a result of show and hide
		/// commands.
		/// </summary>
		private Dictionary<string, RenPyImageData> m_images;

		public RenPyVisualState()
		{
			m_images = new Dictionary<string, RenPyImageData>();
			m_bgImage = null;
		}

		/// <summary>
		/// Resets the state of the RenPyDisplayState.
		/// </summary>
		public void Reset()
		{
			m_images.Clear();
			m_bgImage = null;
		}

		/// <summary>
		/// Returns the current background image or null if there is none.
		/// </summary>
		/// <returns>
		/// The current background image or null if there is none.
		/// </returns>
		public RenPyImageData GetBackgroundImage()
		{
			return m_bgImage;
		}

		/// <summary>
		/// Sets the current background image to the passed image.
		/// </summary>
		/// <param name="image">
		/// The image to use as the background image.
		/// </param>
		public void SetBackgroundImage(RenPyImageData image)
		{
			m_bgImage = image;
		}

		/// <summary>
		/// Returns a collection of images that should be visible as a result of
		/// show and hide commands.
		/// </summary>
		/// <returns>
		/// A collection of images that should be visible as a result of show
		/// and hide commands.
		/// </returns>
		public Dictionary<string, RenPyImageData>.ValueCollection GetImages()
		{
			return m_images.Values;
		}

		/// <summary>
		/// Adds an image with the specified image name.
		/// </summary>
		/// <param name="imageName">
		/// The image name.
		/// </param>
		public void AddImage(string imageName, ref RenPyImageData image)
		{
			string tag = imageName.Split(' ')[0];
			m_images[tag] = image;
		}

		/// <summary>
		/// Removes the image with the specified image name.
		/// </summary>
		/// <param name="imageName">
		/// The image name.
		/// </param>
		public void RemoveImage(string imageName)
		{
			string tag = imageName.Split(' ')[0];
			m_images.Remove(tag);
		}

		/// <summary>
		/// Removes all images from the state.
		/// </summary>
		public void RemoveAllImages()
		{
			m_images.Clear();
		}
	}

	public class RenPyImageData
	{
		/// <summary>
		/// The texture for this image.
		/// </summary>
		private Texture2D m_texture;
		public Texture2D Texture
		{
			get
			{
				return m_texture;
			}
			set
			{
				m_texture = value;
			}
		}

		/// <summary>
		/// The alignment of this image.
		/// </summary>
		private RenPyAlignment m_alignment;
		public RenPyAlignment Alignment
		{
			get
			{
				return m_alignment;
			}
			set
			{
				m_alignment = value;
			}
		}

		public RenPyImageData(ref Texture2D texture, RenPyAlignment alignment)
		{
			m_texture = texture;
			m_alignment = alignment;
		}
	}
}
