using UnityEngine;
using System.Collections;

public class socialBackgroundAudioS : MonoBehaviour
{
	public AudioClip[] audios;
	
	public void PlayAudio(int _index)
	{
		audio.Stop();
		audio.clip = audios[_index];
		audio.Play();
	}
}