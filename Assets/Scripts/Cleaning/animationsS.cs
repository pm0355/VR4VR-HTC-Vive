using UnityEngine;
using System.Collections;

public class animationsS : MonoBehaviour {
	public GameObject[] animations;
	public AudioClip[] voices;
	
	private AnimatableS activeAnimation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CreateAnimation( int _index )
	{
		GameObject tempObj = Instantiate(animations[_index]) as GameObject;
		tempObj.transform.parent = transform;
		
		activeAnimation = tempObj.GetComponentInChildren(typeof(AnimatableS)) as AnimatableS;
	}
	
	public void PlayCurrentAnimation ()
	{
		if(activeAnimation!=null) activeAnimation.PlayAnimation();
	}
	
	public void TalkCurrentAnimation (int _index)
	{
		if(activeAnimation!=null) activeAnimation.PlayTalk(_index);
	}	
	
	public void DestroyAnimations()
	{
		foreach (Transform child in transform) 
		{
			Destroy(child.gameObject);	
		}	
	}
	
	public void PlayVoice(int _index)
	{
		audio.clip = voices[_index];
		audio.Play();
	}

	public bool IsVoicePlaying()
	{
		return audio.isPlaying;
	}
	
	public bool IsRunning()
	{
		return activeAnimation.IsPlaying();
	}
}
