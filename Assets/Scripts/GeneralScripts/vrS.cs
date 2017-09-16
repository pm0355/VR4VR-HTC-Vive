using UnityEngine;
using System.Collections;

public class vrS : MonoBehaviour {
		
	public static Transform VRHead;
	public static Transform HeadCenter;
	public static Transform VRHandRight;
	public static Transform VRHandLeft;
	public static Transform VRFootRight;
	public static Transform FootRight;
	public static Transform VRFootLeft;
	public static Transform FootLeft;
	public static Transform VRNeck;
	public static Transform VRShoulderLeft;
	public static Transform VRShoulderRight;	
	
	// Use this for initialization
	void Start () 
	{
		VRHead = GameObject.Find("VRHeadNode").transform;
		HeadCenter = VRHead.GetChild(0).transform;
		VRHandRight = GameObject.Find("VRHandRightNode").transform;
		VRHandLeft = GameObject.Find("VRHandLeftNode").transform;
		VRFootRight = GameObject.Find("VRFootRightNode").transform;
		FootRight = VRFootRight.GetChild(0).transform;
		VRFootLeft = GameObject.Find("VRFootLeftNode").transform;
		FootLeft = VRFootLeft.GetChild(0).transform;
		VRNeck = GameObject.Find("VRNeckNode").transform;
		VRShoulderRight = GameObject.Find("VRShoulderRightNode").transform;
		VRShoulderLeft = GameObject.Find("VRShoulderLeftNode").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
