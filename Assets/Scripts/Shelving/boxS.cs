using UnityEngine;
using System.Collections;

public class boxS : MonoBehaviour {

	public int targetLevelIndex { get; set; }
	public int activeTexture { get; set; }
		
	public int atShelf { get; set; }
	public int atLevel { get; set; }
	public bool atDelivery { get; set; }
	//public bool atTrueDirection { get; set; }

	public bool updateVRNode { get; set; }
	public Transform vrNode;

	public Transform boxModel;
	public Renderer labelObj;
	public Transform referenceDirection { get; set; }

	public bool atWarning = false;

	// Use this for initialization
	void Awake () 
	{
		updateVRNode = true;
		referenceDirection = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (updateVRNode)
		{
			transform.position = vrNode.position;
			transform.rotation = vrNode.rotation;
		}

		//Debug.Log(atShelf + "-" + atLevel + "-" + atDelivery);
	}
	public void FollowVR(bool _isFollowing)
	{
		updateVRNode = _isFollowing;
	}
	public bool CheckDirection()
	{
		float dotP = Vector3.Dot(boxModel.forward, referenceDirection.forward);
		if(dotP > 0.99f) return true;
		else return false;
	}
	public void SetBoxTexture(Texture _tex, int _texIndex)
	{
		boxModel.renderer.material.mainTexture = _tex;
		activeTexture = _texIndex;
	}
	public void SetLabelTexture(Texture _tex,  int _levelIndex)
	{
		labelObj.material.mainTexture = _tex;
		targetLevelIndex = _levelIndex;
	}
	public void ClearBoxTexture()
	{
		boxModel.renderer.material.mainTexture = null;
		labelObj.renderer.material.mainTexture = null;
		activeTexture = -1;
	}
	public void SetBoxWarning(bool _isWarning)
	{
		atWarning = _isWarning;
		if (_isWarning) boxModel.renderer.material.color = Color.red;
		else boxModel.renderer.material.color = Color.white;
	}
	public void RandomRotate(bool _isLast)
	{
		if (!_isLast)
		{
			do
			{
				int randomMultiplier = Random.Range(1, 4);
				boxModel.localRotation = Quaternion.Euler(new Vector3(0.0f, boxModel.localRotation.eulerAngles.y + (randomMultiplier * 90.0f), 0.0f));
			} while (CheckDirection());
		}
		else
		{
			ResetRotate();
		}
	}
	public void ResetRotate()
	{
		boxModel.localRotation = Quaternion.identity;
	}
	public void ShowBox(bool _isShowing)
	{
		boxModel.gameObject.SetActive(_isShowing);
	}
}
