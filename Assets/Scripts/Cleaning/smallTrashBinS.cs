using UnityEngine;
using System.Collections;

public class smallTrashBinS : MonoBehaviour {

	public Renderer model;
	public Color[] colors;

	// Use this for initialization
	void Start () {
		model.material.color = colors[Random.Range(0, colors.Length)];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
