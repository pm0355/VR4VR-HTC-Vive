using UnityEngine;
using System.Collections;

public class carMovementS : MonoBehaviour {

	public Transform[] frontWheels;
	public Transform[] allWheels;
	public float speedMultiplier;
	public float rotationMultiplier;
	public Renderer carExterior;
	public int materialIndex;
	private Color[] colors;

	// Use this for initialization
	void Start () 
	{
		colors = new Color[6];
		colors[0] = new Color(0.596f, 0.035f, 0.035f);
		colors[1] = new Color(0.396f, 0.8f, 0.165f);
		colors[2] = new Color(0.071f, 0.42f, 0.624f);
		colors[3] = new Color(0.839f, 0.431f, 0.063f);
		colors[4] = new Color(0.133f, 0.125f, 0.125f);
		colors[5] = new Color(0.906f, 0.871f, 0.871f);

		carExterior.materials[materialIndex].color = colors[Random.Range(0, colors.Length)];
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void SetWheels(float _speed, Vector3 _direction)
	{
		foreach (Transform wheel in allWheels)
		{
			wheel.Rotate(_speed * speedMultiplier * Time.deltaTime, 0.0f, 0.0f, Space.Self);
		}
		if(_speed>0.01f)
		{
			foreach (Transform wheel in frontWheels)
			{
				wheel.rotation = Quaternion.LookRotation(_direction, Vector3.up);
				wheel.localEulerAngles = new Vector3(0.0f, wheel.localEulerAngles.y * rotationMultiplier, 0.0f);
			}
		}
	}

	public void RotateRandom()
	{
		transform.localEulerAngles = new Vector3(0.0f, Random.Range(-5.0f, 5.0f));
		foreach (Transform wheel in frontWheels)
		{
			wheel.localEulerAngles = new Vector3(0.0f, Random.Range(-10.0f, 10.0f), 0.0f);
		}		
	}
}
