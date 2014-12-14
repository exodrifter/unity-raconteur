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
			AudioChannel channel = m_state.Aural.GetChannel(m_channel);
			if (channel == null) {
				return;
			}

			// Set source properties
			m_source.mute = Static.MuteAudio;
			m_source.volume = channel.Volume;
			m_source.loop = false;

			// Loop the audio if the queue is empty and looping is requested
			if (channel.Queue.Count == 0) {
				m_source.loop = channel.Looping;
			}
			// Play the next audio clip in the queue if we stopped playing audio
			if(!m_source.isPlaying) {
				m_source.clip = channel.NextClip();
				if(m_source.clip != null) {
					m_source.Play();
				}
			}

			// Check for transitions
			// TODO: This is really hacky and won't work for small non-zero
			// transition times. Fix transitions so they respect changes to the
			// current audio clip and queue list.
			while (channel.Transition != null) {
				AudioChannelTransition t = channel.Transition;

				// Check if the transition has started yet
				if (t.ElapsedTime <= 0) {
					if (t.EndAudio != null) {
						channel.Clip = t.StartAudio;
					}
					t.ElapsedTime = 0;
					startVol = m_source.volume;
				}

				// Play the transition
				t.ElapsedTime += Time.deltaTime;
				float ratio = t.ElapsedTime / t.TransitionTime;
				channel.Volume = Mathf.Lerp(startVol, t.FadeTo, ratio);
				
				// Check if the transition is finished
				if (t.ElapsedTime >= t.TransitionTime) {
					if (t.EndAudio != null) {
						channel.Clip = t.EndAudio;
					}
					channel.Looping = t.Loop;
					channel.Volume = t.FadeTo;
					channel.Transition = t.NextTransition;
				} else {
					break;
				}
			}
		}
	}
}
