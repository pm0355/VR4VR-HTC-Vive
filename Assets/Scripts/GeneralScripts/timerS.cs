using UnityEngine;
using System.Collections;

public static class timerS {
	
	public static float stateStartTime = 0.0f;
	public static float levelStartTime = 0.0f;
	
	public static float indicationTime = 30.0f;
	public static float pictographTime = 60.0f;
	public static float failTime = 300.0f;
	
	public static float lastActivitiyTime = 0.0f;
	//public static float lastIndicationTime = 0.0f;
	public static float lastPictographTime = 0.0f;
	
	public static void initializeTimes(float _indicationT, float _pictographT, float _failT)
	{
		indicationTime = _indicationT;
		pictographTime = _pictographT;
		failTime = _failT;
	}
	
	public static void setStateStartTime() 
	{ 
		stateStartTime = Time.timeSinceLevelLoad;
		setLastActivitiyTime();
		//setLastIndicationTime();
		setLastPictographTime();
	}

	public static void setLevelStartTime()
	{
		levelStartTime = Time.timeSinceLevelLoad;
	}
	
	public static void setLastActivitiyTime() { lastActivitiyTime = Time.timeSinceLevelLoad; }
	//public static void setLastIndicationTime() { lastIndicationTime = Time.timeSinceLevelLoad; }
	public static void setLastPictographTime() { lastPictographTime = Time.timeSinceLevelLoad; }
}
