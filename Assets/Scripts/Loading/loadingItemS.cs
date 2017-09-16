using UnityEngine;
using System.Collections;

public class loadingItemS : MonoBehaviour {
	
	private int numberOfCollisions;	

	public bool isPhysicsOn {get; set; }
	public bool wrongPosition {get; set; }
	public bool wrongLabel {get; set; }
	
	public Color[] colors;

	private bool directionImportant = false;
	private bool weightImportant = false;
	private bool speedImportant = false;
	private bool isHeavy = false;

	public GameObject upLabelObj;
	public GameObject heavyLabelObj;
	public GameObject lightLabelObj;
	public GameObject fragileLabelObj;
	 
	private Vector2 itemSize;

	private bool firstTriggerOccured = false;
	private bool firstDragOccured = false;

	private dragableS myDragable;

	private bool isDamaged = false;
	public Sprite damagedBoxSprite;

	private loadingVisualManagerS visualM;

	public AudioClip[] sounds;

	void Start () 
	{
		Vector2 scale = new Vector2(Random.Range(1.0f, 1.8f), Random.Range(1.0f, 1.4f));

		Vector3 upLabelSize = upLabelObj.transform.localScale;
		Vector3 heavyLabelSize = heavyLabelObj.transform.localScale;
		Vector3 lightLabelSize = lightLabelObj.transform.localScale;
		Vector3 fragileLabelSize = fragileLabelObj.transform.localScale;

		transform.localScale = new Vector3(transform.localScale.x * scale.x, transform.localScale.y * scale.y, 1.0f);

		upLabelObj.transform.localScale = new Vector3(upLabelSize.x / scale.x, upLabelSize.y / scale.y, 1.0f);
		heavyLabelObj.transform.localScale = new Vector3(heavyLabelSize.x / scale.x, heavyLabelSize.y / scale.y, 1.0f);
		lightLabelObj.transform.localScale = new Vector3(lightLabelSize.x / scale.x, lightLabelSize.y / scale.y, 1.0f);
		fragileLabelObj.transform.localScale = new Vector3(fragileLabelSize.x / scale.x, fragileLabelSize.y / scale.y, 1.0f);

		transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.Range(0, 3)*90.0f);

		renderer.material.color = colors[1];
		numberOfCollisions = 1;
		isPhysicsOn = true;
		TurnPhysics(false);

		BoxCollider2D boxCollider = transform.GetComponent<BoxCollider2D>();
		itemSize = new Vector2(boxCollider.size.x * transform.lossyScale.x , boxCollider.size.y * transform.lossyScale.y );

		myDragable = transform.GetComponent<dragableS>();

		visualM = GameObject.Find("AudioVisuals").transform.GetComponent<loadingVisualManagerS>();
	}
	void Update () 
	{
//		Debug.Log(transform.gameObject.name +" "+ numberOfCollisions);

		if(numberOfCollisions==0 && !myDragable.draging) TurnPhysics(true);
		if(myDragable.draging) TurnPhysics(false);

		if(!firstDragOccured && myDragable.draging) firstDragOccured = true;
	}
	public void CreateLabels()
	{
		weightImportant = true;

		float test = Random.value;
		if(test < 0.5f)
		{
			isHeavy = true;
			heavyLabelObj.SetActive(true);
		}
		else
		{
			isHeavy = false;
			lightLabelObj.SetActive(true);
		}

		test = Random.value;
		if(test < 0.3f)
		{
			directionImportant = true;
			upLabelObj.SetActive(true);
		}
		else if(test< 0.6f)
		{
			speedImportant = true;
			fragileLabelObj.SetActive(true);
		}
	}
	
	public void TurnPhysics(bool _isOn)
	{
		if(_isOn && !isPhysicsOn)
		{
			rigidbody2D.isKinematic = false;
			collider2D.isTrigger = false;
			isPhysicsOn = true;
		}
		else if(!_isOn && isPhysicsOn)
		{
			rigidbody2D.isKinematic = true;
			collider2D.isTrigger = true;
			isPhysicsOn = false;
		}
	}

	public void CheckPosition()
	{
		if(firstDragOccured)
		{
			if(numberOfCollisions == 0) 
			{
				wrongPosition = false;
				renderer.material.color = colors[1];
			}
			else
			{
				wrongPosition = true;
				renderer.material.color = colors[0];
			}
		}
		else
		{
			wrongPosition = true;
			renderer.material.color = colors[1];
		}
	}
	public void CheckLabels()
	{
		wrongLabel = false;
		CheckWeight ();
		CheckDirection();
	}
	void CheckWeight()
	{
		if(weightImportant)
		{
			if(isHeavy)
			{
				heavyLabelObj.renderer.material.color = Color.white;
				RaycastHit2D[] boxesBelow = Physics2D.BoxCastAll( (Vector2)transform.position, itemSize * 0.9f, transform.eulerAngles.z, -Vector2.up, 2.0f);
				foreach (RaycastHit2D box in boxesBelow) 
				{
					loadingItemS item = box.transform.GetComponent<loadingItemS>();
					//if(item && item.gameObject!=gameObject)
					if(item && item!=this)
					{
						if(box.transform.GetComponent<loadingItemS>().isHeavy == false)
						{
							heavyLabelObj.renderer.material.color = colors[0];
							wrongLabel = true;
							break;
						}
					}
				}
			}
			else
			{
				lightLabelObj.renderer.material.color = Color.white;
				RaycastHit2D[] boxesBelow = Physics2D.BoxCastAll( (Vector2)transform.position, itemSize * 0.9f, transform.eulerAngles.z, Vector2.up, 2.0f);
				foreach (RaycastHit2D box in boxesBelow) 
				{
					loadingItemS item = box.transform.GetComponent<loadingItemS>();
					//if(item && item.gameObject!=gameObject)
					if(item && item!=this)
					{
						if(box.transform.GetComponent<loadingItemS>().isHeavy == true)
						{
							lightLabelObj.renderer.material.color = colors[0];
							wrongLabel = true;
							break;
						}
					}
				}
			}
		}
	}
	void CheckDirection()
	{
		if(directionImportant)
		{
			upLabelObj.renderer.material.color = Color.white;
			if (Vector3.Dot(transform.up, Vector3.up) < 0.95f ) 
			{
				upLabelObj.renderer.material.color = colors[0];
				wrongLabel = true;
			}
		}
	}

	public void ClearWarnings()
	{
		renderer.material.color = colors[1];
		heavyLabelObj.renderer.material.color = Color.white;
		lightLabelObj.renderer.material.color = Color.white;
		upLabelObj.renderer.material.color = Color.white;
	}

	void OnTriggerEnter2D(Collider2D other) 
	{ 
		if(firstTriggerOccured) 
		{
			numberOfCollisions++; 
		}
		else
		{
			firstTriggerOccured = true;
		}
	}
	void OnTriggerExit2D(Collider2D other) { if(numberOfCollisions>0)numberOfCollisions--; }

	void OnCollisionEnter2D(Collision2D coll) 
	{
		if(speedImportant && !isDamaged)
		{
		//	Debug.Log(coll.relativeVelocity.magnitude);
			if(coll.relativeVelocity.magnitude>2.0f) 
			{
				saverS.failCount++;
				saverS.failTimes += ((int)(Time.timeSinceLevelLoad - timerS.levelStartTime)).ToString("#") + "-";

				//PHOTON CODE
				generalManagerS.PhotonProgressUpdate();
				//PHOTON CODE

				transform.GetComponent<SpriteRenderer>().sprite = damagedBoxSprite;
				isDamaged = true;
				visualM.visualNo = 4;
				visualM.visualOn = true;

				audio.PlayOneShot(sounds[Random.Range(3,6)]);				
			}
		}
		else
		{
			if(coll.relativeVelocity.magnitude>0.5f) 
			{
				audio.PlayOneShot(sounds[Random.Range(0,3)]);
			}
		}
	}
}