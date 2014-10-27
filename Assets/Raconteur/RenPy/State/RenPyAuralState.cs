using UnityEngine;

namespace DPek.Raconteur.RenPy.State
{
	/// <summary>
	/// Stores the aural (audio) state of a Ren'Py script.
	/// </summary>
	public class RenPyAuralState
	{
		private AudioChannel[] m_channels;

		public RenPyAuralState()
		{
			m_channels = new AudioChannel[7];
			for (int i = 0; i < m_channels.Length; ++i) {
				m_channels[i] = new AudioChannel();
			}
		}

		public AudioChannel GetChannel(int index)
		{
			return m_channels[index];
		}

		public AudioChannel GetChannel(string index)
		{
			if (index == "music") {
				return m_channels[6];
			} else if (index == "sound") {
				return m_channels[0];
			} else if (index == "voice") {
				return m_channels[1]; // TODO: What channel is voice?
			}
			return null;
		}
	}

	public class AudioChannel
	{
		/// <summary>
		/// The audio clip in this channel.
		/// </summary>
		private AudioClip m_audio;
		public AudioClip Audio
		{
			get {
				return m_audio;
			}
			set {
				m_audio = value;
			}
		}

		/// <summary>
		/// The volume of this audio channel.
		/// </summary>
		private float m_volume;
		public float Volume
		{
			get {
				return m_volume;
			}
			set {
				m_volume = value;
			}
		}

		/// <summary>
		/// Whether or not this audio channel is looping.
		/// </summary>
		private bool m_looping;
		public bool Looping
		{
			get {
				return m_looping;
			}
			set {
				m_looping = value;
			}
		}

		/// <summary>
		/// The current transition for this audio channel.
		/// </summary>
		private AudioChannelTransition m_transition;
		public AudioChannelTransition Transition
		{
			get {
				return m_transition;
			}
			set {
				m_transition = value;
			}
		}

		public AudioChannel()
		{
			m_audio = null;
			m_volume = 1;
			m_looping = false;
		}
	}

	public class AudioChannelTransition
	{
		/// <summary>
		/// The audio clip to use when this transition starts, or null to
		/// ignore.
		/// </summary>
		private AudioClip m_startAudio;
		public AudioClip StartAudio
		{
			get {
				return m_startAudio;
			}
			set {
				m_startAudio = value;
			}
		}

		/// <summary>
		/// The audio clip to use when this transition ends, or null to ignore.
		/// </summary>
		private AudioClip m_endAudio;
		public AudioClip EndAudio
		{
			get {
				return m_endAudio;
			}
			set {
				m_endAudio = value;
			}
		}

		/// <summary>
		/// The transition to use when this transition is finished.
		/// </summary>
		private AudioChannelTransition m_nextTransition;
		public AudioChannelTransition NextTransition
		{
			get {
				return m_nextTransition;
			}
			set {
				m_nextTransition = value;
			}
		}

		/// <summary>
		/// The volume that this audio channel is transitioning to.
		/// </summary>
		private float m_fadeTo;
		public float FadeTo
		{
			get {
				return m_fadeTo;
			}
			set {
				m_fadeTo = value;
			}
		}

		/// <summary>
		/// Whether or not to set the audio channel to loop when the transition
		/// is finished.
		/// </summary>
		private bool m_loop;
		public bool Loop
		{
			get {
				return m_loop;
			}
			set {
				m_loop = value;
			}
		}

		/// <summary>
		/// The amount of time the audio channel should take to transition to
		/// the target volume.
		/// </summary>
		private float m_transitionTime;
		public float TransitionTime
		{
			get {
				return m_transitionTime;
			}
			set {
				m_transitionTime = value;
			}
		}

		/// <summary>
		/// The amount of time the audio channel has spent transitioning to the
		/// target volume.
		/// </summary>
		private float m_elapsedTime;
		public float ElapsedTime
		{
			get {
				return m_elapsedTime;
			}
			set {
				m_elapsedTime = value;
			}
		}

		public AudioChannelTransition()
		{
			m_startAudio = null;
			m_endAudio = null;
			m_fadeTo = 1;
			m_loop = false;
			m_transitionTime = 0;
			m_elapsedTime = 0;
		}
	}
}
