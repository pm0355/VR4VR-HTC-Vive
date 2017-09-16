using UnityEngine;
using System.Collections;

public class kinectS : MonoBehaviour {

	public static Transform Head;
	public static Transform HandRight;
	public static Transform HandLeft;
	public static Transform FootRight;
	public static Transform FootLeft;
	public static Transform Neck;
	public static Transform ShoulderLeft;
	public static Transform ShoulderRight;

	// Use this for initialization
	void Start () 
	{
		AvatarController avatar = transform.GetComponent<AvatarController>();

		Head = avatar.Head;
		HandRight = avatar.RightHand;
		HandLeft = avatar.LeftHand;
		FootRight = avatar.RightFoot;
		FootLeft = avatar.LeftFoot;
		Neck = avatar.Neck;
		ShoulderRight = avatar.RightUpperArm;
		ShoulderLeft = avatar.LeftUpperArm;
	}
}
