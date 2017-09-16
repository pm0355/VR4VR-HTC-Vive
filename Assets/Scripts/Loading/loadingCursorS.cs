using UnityEngine;
using System.Collections;

public class loadingCursorS : MonoBehaviour {

	public Sprite[] visuals;

	public Transform omniCursor;

	public GravityEffect omniM;

	// Use this for initialization
	void Start () 
	{
		Screen.showCursor = false;
		SetSprite(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (omniM.isOmniWanted)
		{
			Vector3 newPos = omniCursor.position;
			newPos.z = -10.0f;
			transform.position = newPos;
		}
		else
		{
			Vector3 newPos = Input.mousePosition;
			newPos.x = (newPos.x - 4224.0f) / 1550.0f * 3.2f;
			newPos.y = (newPos.y - 680.0f) / 400.0f;
			newPos.z = -10.0f;
			transform.position = newPos;
		}

		if (transform.position.x > 2.5f) transform.position = new Vector3(2.5f, transform.position.y, transform.position.z);
		if (transform.position.x < -2.5f) transform.position = new Vector3(-2.5f, transform.position.y, transform.position.z);
		if (transform.position.y > 0.92f) transform.position = new Vector3(transform.position.x, 0.92f, transform.position.z);
		if (transform.position.y < -0.92f) transform.position = new Vector3(transform.position.x, -0.92f, transform.position.z);
	}

	public void SetSprite(bool _isHolding)
	{
		GetComponent<SpriteRenderer>().sprite = visuals[_isHolding ? 1 : 0];
	}
}
