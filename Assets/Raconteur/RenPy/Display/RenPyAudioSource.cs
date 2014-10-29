using UnityEngine;

using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Display
{
	public class RenPyAudioSource : MonoBehaviour
	{
		public RenPyState m_state;

		public AudioSource m_source;

		public string m_channel;

		private float startVol;

		void Start()
		{
			m_source = m_source ?? gameObject.AddComponent<AudioSource>();
		}

		void Update()
		{
			if (m_source == null)
			{
				return;
			}

			AudioChannel channel = m_state.Aural.GetChannel(m_channel);
			if (channel == null)
			{
				return;
			}

			if (!m_source.isPlaying) {
				m_source.Play();
			}

			m_source.mute = Static.MuteAudio;
			m_source.loop = channel.Looping;
			m_source.volume = channel.Volume;
			m_source.clip = channel.Audio;

			if (channel.Transition != null) {
				AudioChannelTransition t = channel.Transition;

				// Check if the transition is finished
				if (t.ElapsedTime >= t.TransitionTime) {
					if (t.EndAudio != null) {
						channel.Audio = t.EndAudio;
					}
					channel.Looping = t.Loop;
					channel.Volume = t.FadeTo;
					channel.Transition = t.NextTransition;
				}

				// Check if the transition has started yet
				else if (t.ElapsedTime <= 0) {
					if (t.EndAudio != null) {
						channel.Audio = t.StartAudio;
					}
					t.ElapsedTime = 0;
					startVol = m_source.volume;
				}

				// Play the transition
				else {
					t.ElapsedTime += Time.deltaTime;
					float ratio = t.ElapsedTime / t.TransitionTime;
					channel.Volume = Mathf.Lerp(startVol, t.FadeTo, ratio);
				}
			}
		}
	}
}
