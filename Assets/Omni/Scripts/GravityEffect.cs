using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;


public class GravityEffect: HapticClassScript {
	private GenericFunctionsClass myGenericFunctionsClassScript;
	private ConstantForceEffect myConstantForceEffect;
	
	public bool button1Activated {get;set;}
	public bool button2Activated {get;set;}
	public bool button1Deactivated {get;set;}
	public bool button2Deactivated {get;set;}
	private bool button1WasActive = false;
	private bool button2WasActive = false;

	public bool isOmniWanted {get;set;}
	public bool isOmniControlActive {get;set;}

	void Awake()
	{
		myGenericFunctionsClassScript = transform.GetComponent<GenericFunctionsClass>();
		myConstantForceEffect = transform.GetComponent<ConstantForceEffect>();
	}

	void Start()
	{
		isOmniWanted = true;
		isOmniControlActive = true;
		StartOmni ();
	}

	public void StartOmni()
	{
		isOmniControlActive = true;
		if(PluginImport.InitHapticDevice())
		{
			//Debug.Log("OpenGL Context Launched");
			//Debug.Log("Haptic Device Launched");
			
			myGenericFunctionsClassScript.SetHapticWorkSpace();
			myGenericFunctionsClassScript.GetHapticWorkSpace();
			
			//Update Workspace as function of camera
			PluginImport.UpdateWorkspace(myHapticCamera.transform.rotation.eulerAngles.y);
			
			//Set Mode of Interaction
			/*
			 * Mode = 0 Contact
			 * Mode = 1 Manipulation - So objects will have a mass when handling them
			 * Mode = 2 Custom Effect - So the haptic device simulate vibration and tangential forces as power tools
			 * Mode = 3 Puncture - So the haptic device is a needle that puncture inside a geometry
			 */
			PluginImport.SetMode(ModeIndex);
			//Show a text descrition of the mode
			myGenericFunctionsClassScript.IndicateMode();

			//Set the touchable face(s)
			PluginImport.SetTouchableFace(ConverterClass.ConvertStringToByteToIntPtr(TouchableFace));

			Debug.Log("Haptic Device is launched");
		}
		else
			Debug.Log("Haptic Device cannot be launched");


			myGenericFunctionsClassScript.SetEnvironmentConstantForce();
		
		
		/***************************************************************/
		//Setup the Haptic Geometry in the OpenGL context
		/***************************************************************/
		myGenericFunctionsClassScript.SetHapticGeometry();

		//Get the Number of Haptic Object
		//Debug.Log ("Total Number of Haptic Objects: " + PluginImport.GetHapticObjectCount());

		/***************************************************************/
		//Launch the Haptic Event for all different haptic objects
		/***************************************************************/
		PluginImport.LaunchHapticEvent();
	}
	
	public void StopOmni()
	{
		isOmniControlActive = false;
		if (PluginImport.HapticCleanUp())
		{
			Debug.Log("Haptic Context CleanUp");
			Debug.Log("Desactivate Device");
			Debug.Log("OpenGL Context CleanUp");
		}
	}

	void Update()
	{
		try 
		{
			if(isOmniControlActive)
			{
				PluginImport.UpdateWorkspace(myHapticCamera.transform.rotation.eulerAngles.y);
				myGenericFunctionsClassScript.UpdateGraphicalWorkspace();
				
				PluginImport.RenderHaptic ();
				
				myGenericFunctionsClassScript.GetProxyValues();
				
				myGenericFunctionsClassScript.GetTouchedObject();
				
				//Debug.Log ("Button 1: " + PluginImport.GetButton1State());
				//Debug.Log ("Button 2: " + PluginImport.GetButton2State());
				
				bool button1isActive = PluginImport.GetButton1State ();
				if(!button1WasActive && button1isActive) button1Activated = true;
				else button1Activated = false;
				if(button1WasActive && !button1isActive) button1Deactivated = true;
				else button1Deactivated = false;
				button1WasActive = button1isActive;
				
				bool button2isActive = PluginImport.GetButton2State ();
				if(!button2WasActive && button2isActive) button2Activated = true;
				else button2Activated = false;
				if(button2WasActive && !button2isActive) button2Deactivated = true;
				else button2Deactivated = false;
				button2WasActive = button2isActive;		
			}

			if(isOmniControlActive && Input.GetMouseButtonDown(0))
			{
				isOmniWanted = false;
				StopOmni();
			}
			if(!isOmniControlActive && Input.GetMouseButtonDown(1))
			{
				isOmniWanted = true;
				StartOmni();
			}

		} 
		catch (Exception ex) 
		{
			Debug.Log("Failed to update omni" + ex.Message);
			isOmniControlActive = false;
			PluginImport.HapticCleanUp();
		}

	}

	public void ApplyGravity(float _magnitude)
	{
		try 
		{
			myConstantForceEffect.magnitude=_magnitude;
			myGenericFunctionsClassScript.SetEnvironmentConstantForce();			
		} 
		catch (Exception ex) 
		{
			Debug.Log("Failed to apply force" + ex.Message);
			isOmniControlActive = false;
			PluginImport.HapticCleanUp();
		}

	}

	void OnDisable()
	{
		StopOmni ();
	}
}
