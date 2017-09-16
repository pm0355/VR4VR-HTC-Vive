using UnityEngine;
using System.Collections;

public class loadingBoxS : MonoBehaviour {
	
	public bool hasHeavyLabel { get; set; }
	public bool hasUpLabel { get; set; }
	public bool hasFragileLabel { get; set; }
	
	public GameObject HeavyLabelObj;
	public GameObject UpLabelObj;
	public GameObject FragileLabelObj;
	
	private Renderer heavyLabelRenderer;
	private Renderer upLabelRenderer;
	//private Renderer fragileLabelRenderer;
	
	public int order { get; set; }
	public bool physicsActive { get; set; }
	
	private bool isHorizontal;
	
	public Color[] colors;
	public Texture brokenBoxTexture;
	
	private loadingVisualManagerS visualM;
	public AudioClip[] sounds;
	
	void Awake () 
	{
		hasHeavyLabel = false;
		hasUpLabel = false;
		physicsActive = false;
		
		renderer.material.color = colors[0];
	}
	void Start()
	{
		visualM = GameObject.Find("AudioVisuals").transform.GetComponent<loadingVisualManagerS>();
	}
	void Update () 
	{
		
	}
	
	public void UpdateLabel()
	{
		isHorizontal = transform.localScale.x > transform.localScale.y ? true : false;
		
		int labelCount = 0;
		if (hasHeavyLabel) labelCount++;
		if (hasUpLabel) labelCount++;
		if (hasFragileLabel) labelCount++;
		
		
		if (labelCount>1)
		{
			GameObject Object1 = null;
			GameObject Object2 = null;
			if (hasUpLabel)
			{
				Object1 = Instantiate(UpLabelObj, transform.position, Quaternion.identity) as GameObject;
				upLabelRenderer = Object1.transform.GetChild(0).renderer;
				if (hasHeavyLabel)
				{
					Object2 = Instantiate(HeavyLabelObj, transform.position, Quaternion.identity) as GameObject;
					heavyLabelRenderer = Object2.transform.GetChild(0).renderer;
				}
				else
				{
					Object2 = Instantiate(FragileLabelObj, transform.position, Quaternion.identity) as GameObject;
					//fragileLabelRenderer = Object2.transform.GetChild(0).renderer;
				}
			}
			else
			{
				Object1 = Instantiate(HeavyLabelObj, transform.position, Quaternion.identity) as GameObject;
				heavyLabelRenderer = Object1.transform.GetChild(0).renderer;
				Object2 = Instantiate(FragileLabelObj, transform.position, Quaternion.identity) as GameObject;
				//fragileLabelRenderer = Object2.transform.GetChild(0).renderer;
			}
			
			if (isHorizontal)
			{
				float scale = Mathf.Min(transform.lossyScale.x / 2.0f, transform.lossyScale.y);
				scale *= 0.9f;
				if (scale > 0.12f) scale = 0.12f;
				
				Object1.transform.parent = transform;
				Object2.transform.parent = transform;
				
				Object1.transform.localScale = new Vector3(scale / transform.lossyScale.x, scale / transform.lossyScale.y, 1.0f);
				Object2.transform.localScale = new Vector3(scale / transform.lossyScale.x, scale / transform.lossyScale.y, 1.0f);
				
				Object1.transform.localPosition = new Vector3(-0.25f, 0.0f, -0.01f);
				Object2.transform.localPosition = new Vector3(0.25f, 0.0f, -0.01f);
			}
			else
			{
				float scale = Mathf.Min(transform.lossyScale.y / 2.0f, transform.lossyScale.x);
				scale *= 0.9f;
				if (scale > 0.12f) scale = 0.12f;
				
				Object1.transform.parent = transform;
				Object2.transform.parent = transform;
				
				Object1.transform.localScale = new Vector3(scale / transform.lossyScale.x, scale / transform.lossyScale.y, 1.0f);
				Object2.transform.localScale = new Vector3(scale / transform.lossyScale.x, scale / transform.lossyScale.y, 1.0f);

				Object1.transform.localPosition = new Vector3(0.0f, -0.25f, -0.01f);
				Object2.transform.localPosition = new Vector3(0.0f, 0.25f, -0.01f);
			}
			
			if(hasUpLabel)
			{
				Object2.transform.GetChild(0).Rotate(0.0f, 0.0f, Random.Range(0, 4) * 90.0f);
			}
			else
			{
				Object1.transform.GetChild(0).Rotate(0.0f, 0.0f, Random.Range(0, 4) * 90.0f);
				Object2.transform.GetChild(0).Rotate(0.0f, 0.0f, Random.Range(0, 4) * 90.0f);
			}
		}
		else
		{
			if (hasUpLabel)
			{
				GameObject UpObject = Instantiate(UpLabelObj, transform.position, Quaternion.identity) as GameObject;
				upLabelRenderer = UpObject.transform.GetChild(0).renderer;
				
				float scale = isHorizontal ? Mathf.Min(transform.lossyScale.y * 0.9f, 0.12f) : Mathf.Min(transform.lossyScale.x * 0.9f, 0.12f);
				
				UpObject.transform.parent = transform;
				
				UpObject.transform.localScale = new Vector3(scale / transform.lossyScale.x, scale / transform.lossyScale.y, 1.0f);
				UpObject.transform.localPosition = new Vector3(0.0f, 0.0f, -0.01f);
			}
			if (hasHeavyLabel)
			{
				GameObject HeavyObject = Instantiate(HeavyLabelObj, transform.position, Quaternion.identity) as GameObject;
				heavyLabelRenderer = HeavyObject.transform.GetChild(0).renderer;
				
				HeavyObject.transform.GetChild(0).Rotate(0.0f, 0.0f, Random.Range(0, 4) * 90.0f);
				
				float scale = isHorizontal ? Mathf.Min(transform.lossyScale.y * 0.9f, 0.12f) : Mathf.Min(transform.lossyScale.x * 0.9f, 0.12f);
				
				HeavyObject.transform.parent = transform;
				
				HeavyObject.transform.localScale = new Vector3(scale / transform.lossyScale.x, scale / transform.lossyScale.y, 1.0f);
				HeavyObject.transform.localPosition = new Vector3(0.0f, 0.0f, -0.01f);
			}
			if (hasFragileLabel)
			{
				GameObject FragileObject = Instantiate(FragileLabelObj, transform.position, Quaternion.identity) as GameObject;
				//fragileLabelRenderer = FragileObject.transform.GetChild(0).renderer;
				
				FragileObject.transform.GetChild(0).Rotate(0.0f, 0.0f, Random.Range(0, 4) * 90.0f);
				
				float scale = isHorizontal ? Mathf.Min(transform.lossyScale.y * 0.9f, 0.12f) : Mathf.Min(transform.lossyScale.x * 0.9f, 0.12f);
				
				FragileObject.transform.parent = transform;
				
				FragileObject.transform.localScale = new Vector3(scale / transform.lossyScale.x, scale / transform.lossyScale.y, 1.0f);
				FragileObject.transform.localPosition = new Vector3(0.0f, 0.0f, -0.01f);
			}
		}
	}
	
	public bool CheckPosition()
	{
		Collider2D[] colliders = Physics2D.OverlapAreaAll(collider2D.bounds.min, collider2D.bounds.max);
		if (colliders.Length > 1)
		{
			renderer.material.color = colors[1];
			return false;
		}
		else
		{
			renderer.material.color = colors[0];
			return true;
		}
	}
	public bool CheckLabels()
	{
		bool result = true;
		if(hasUpLabel)
		{
			if (Vector2.Dot(transform.up, Vector2.up) < 0.98f)
			{
				upLabelRenderer.material.color = Color.red;
				result = false;
			}
			else
			{
				upLabelRenderer.material.color = Color.white;
			}
		}
		if (hasHeavyLabel)
		{
			RaycastHit2D[] boxesBelow = Physics2D.BoxCastAll(transform.position, transform.lossyScale * 0.94f, transform.eulerAngles.z, -Vector2.up, 1.0f, 2048);
			
			heavyLabelRenderer.material.color = Color.white;
			foreach (RaycastHit2D box in boxesBelow)
			{
				if (!box.transform.GetComponent<loadingBoxS>().hasHeavyLabel)
				{
					heavyLabelRenderer.material.color = Color.red;
					result = false;
					break;
				}
			}
		}
		return result;
	}
	
	public void ActivatePhysics(bool _isActive)
	{
		if(_isActive)
		{
			rigidbody2D.isKinematic = false;
			transform.GetComponent<BoxCollider2D>().size = new Vector2((transform.localScale.x - 0.01f / transform.root.lossyScale.x) / transform.localScale.x, (transform.localScale.y - 0.01f / transform.root.lossyScale.y) / transform.localScale.y);
			collider2D.isTrigger = false;
			physicsActive = true;
		}
		else
		{
			rigidbody2D.isKinematic = true;
			transform.GetComponent<BoxCollider2D>().size = new Vector2((transform.localScale.x - 0.05f / transform.root.lossyScale.x) / transform.localScale.x, (transform.localScale.y - 0.05f / transform.root.lossyScale.y) / transform.localScale.y);
			
			Vector2 temp = transform.GetComponent<BoxCollider2D>().size;
			if (temp.x < 0.051f) temp.x = 0.051f;
			if (temp.y < 0.051f) temp.y = 0.051f;
			transform.GetComponent<BoxCollider2D>().size = temp;
			
			collider2D.isTrigger = true;
			physicsActive = false;
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (hasFragileLabel && renderer.material.mainTexture != brokenBoxTexture && coll.relativeVelocity.magnitude > 2.0f)
		{
			saverS.failCount++;
			saverS.failTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";

			//PHOTON CODE
			generalManagerS.PhotonProgressUpdate();
			//PHOTON CODE

			//fragileLabelRenderer.material.color = Color.red;
			renderer.material.mainTexture = brokenBoxTexture;
			audio.PlayOneShot(sounds[Random.Range(3, 6)]);
			visualM.visualNo = 4;
			if (visualM.visualOn) visualM.visualStartTime = Time.timeSinceLevelLoad;
			visualM.visualOn = true;
		}
		else
		{
			if (coll.relativeVelocity.magnitude > 0.3f)
			{
				audio.PlayOneShot(sounds[Random.Range(0, 3)], coll.relativeVelocity.magnitude);
			}
		}
		//Debug.Log(coll.relativeVelocity.magnitude);
	}
}
