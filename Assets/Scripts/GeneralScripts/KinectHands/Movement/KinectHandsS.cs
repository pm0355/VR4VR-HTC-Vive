using UnityEngine;
using System.Collections;

public class KinectHandsS : MonoBehaviour {
	
	public Transform rightHand;
	public Transform leftHand;	
	
	public float armLenght;
	public float translationMultiplier;
	public float rotateMultiplier;
	public float sideMultiplier;
	
	public float handOffsetToCenterMultiplier;
	
	public float depth;
	public float screenWidthToHeight;
	
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		/*OLD WORKING CODE
		Vector3 sholdersLtoR = kinectS.ShoulderRight.position - kinectS.ShoulderLeft.position;
		Vector3 rotateEffect = Vector3.Cross(sholdersLtoR.normalized, Vector3.up) * rotateMultiplier;
		
		Vector3 sideEffect = (kinectS.ShoulderRight.position + kinectS.ShoulderLeft.position) * 0.5f * sideMultiplier;
		
		float handOffsetToCenter = (kinectS.HandRight.position.x - kinectS.HandLeft.position.x) * handOffsetToCenterMultiplier;

		Vector3 temp = Vector3.zero;
		
		temp.x = Mathf.Clamp( (kinectS.HandRight.position.x - kinectS.Neck.position.x) / armLenght + rotateEffect.x + sideEffect.x - handOffsetToCenter , -screenWidthToHeight, screenWidthToHeight);
		temp.y = Mathf.Clamp(( kinectS.HandRight.position.y - kinectS.ShoulderRight.position.y) / armLenght , -1.0f, 1.0f);
		temp.z = depth;
		rightHand.transform.position = temp;
		
		temp.x = Mathf.Clamp( (kinectS.HandLeft.position.x - kinectS.Neck.position.x) / armLenght + rotateEffect.x + sideEffect.x + handOffsetToCenter , -screenWidthToHeight, screenWidthToHeight);
		temp.y = Mathf.Clamp(( kinectS.HandLeft.position.y - kinectS.ShoulderLeft.position.y) / armLenght , -1.0f, 1.0f);
		temp.z = depth;
		leftHand.transform.position = temp;	
		 */

		Vector3 sholdersLtoR = kinectS.ShoulderRight.position - kinectS.ShoulderLeft.position;

		Vector3 rotateEffect = Vector3.Cross(sholdersLtoR.normalized, Vector3.up) * rotateMultiplier;
		Vector3 sideEffect = (kinectS.ShoulderRight.position + kinectS.ShoulderLeft.position) * 0.5f * sideMultiplier;
		float handOffsetToCenter = (kinectS.HandRight.position.x - kinectS.HandLeft.position.x) * handOffsetToCenterMultiplier;

		Vector3 temp = Vector3.zero;

		temp.x = Mathf.Clamp((kinectS.HandRight.position - kinectS.ShoulderRight.position).normalized.x * translationMultiplier + rotateEffect.x + sideEffect.x - handOffsetToCenter, -screenWidthToHeight, screenWidthToHeight);
		temp.y = Mathf.Clamp((kinectS.HandRight.position - kinectS.ShoulderRight.position).normalized.y * translationMultiplier, -1.0f, 1.0f);
		temp.z = depth;
		rightHand.transform.position = temp;

		temp.x = Mathf.Clamp((kinectS.HandLeft.position - kinectS.ShoulderLeft.position).normalized.x * translationMultiplier + rotateEffect.x + sideEffect.x + handOffsetToCenter, -screenWidthToHeight, screenWidthToHeight);
		temp.y = Mathf.Clamp((kinectS.HandLeft.position - kinectS.ShoulderLeft.position).normalized.y * translationMultiplier, -1.0f, 1.0f);
		temp.z = depth;
		leftHand.transform.position = temp;

		/*Same code with middleVR kinect data

		Vector3 sholdersLtoR = vrS.VRShoulderRight.position - vrS.VRShoulderLeft.position;
		Vector3 rotateEffect = Vector3.Cross(sholdersLtoR.normalized, Vector3.up) * rotateMultiplier;

		Vector3 sideEffect = (vrS.VRShoulderRight.position + vrS.VRShoulderLeft.position) * 0.5f * sideMultiplier;

		float handOffsetToCenter = (vrS.VRHandRight.position.x - vrS.VRHandLeft.position.x) * handOffsetToCenterMultiplier;

		Vector3 temp = Vector3.zero;

		temp.x = Mathf.Clamp((vrS.VRHandRight.position - vrS.VRShoulderRight.position).x * translationMultiplier + rotateEffect.x + sideEffect.x - handOffsetToCenter, -screenWidthToHeight, screenWidthToHeight);
		temp.y = Mathf.Clamp((vrS.VRHandRight.position - vrS.VRShoulderRight.position).y * translationMultiplier, -1.0f, 1.0f);
		temp.z = depth;
		rightHand.transform.position = temp;

		temp.x = Mathf.Clamp((vrS.VRHandLeft.position - vrS.VRShoulderLeft.position).x * translationMultiplier + rotateEffect.x + sideEffect.x + handOffsetToCenter, -screenWidthToHeight, screenWidthToHeight);
		temp.y = Mathf.Clamp((vrS.VRHandLeft.position - vrS.VRShoulderLeft.position).y * translationMultiplier, -1.0f, 1.0f);
		temp.z = depth;
		leftHand.transform.position = temp;	
		*/
		
	}

	public void ShowHands(bool _show)
	{
		if(_show)
		{
			rightHand.GetComponentInChildren<Renderer>().enabled = true;
			leftHand.GetComponentInChildren<Renderer>().enabled = true;
		}
		else
		{
			rightHand.GetComponentInChildren<Renderer>().enabled = false;
			leftHand.GetComponentInChildren<Renderer>().enabled = false;
		}
	}
}
