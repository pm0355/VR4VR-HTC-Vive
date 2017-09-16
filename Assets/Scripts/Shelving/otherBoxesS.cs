using UnityEngine;
using System.Collections;

public class otherBoxesS : MonoBehaviour {

	public Renderer[] boxes;

	public void SetBoxTextures(Texture _tex)
	{
		foreach (Renderer item in boxes)
		{
			item.material.mainTexture = _tex;
		}
	}
}
