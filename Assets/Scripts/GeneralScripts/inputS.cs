using UnityEngine;
using System.Collections;

public enum UISkill
{
	Cleaning,
	Loading,
	Money_Management,
	Shelving,
	Sorting,
	Ordering,
	Cautiousness,
	Socialness,
}
public enum UICleaningSubTask
{
	Vacuuming,
	Mopping,
	Litter_Collection
}
public enum UILoadingSubTask
{
	Identical_Boxes,
	Labeled_Boxes,
	Timer
}
public enum UIMoneyManagementSubTask
{
	ST0,
	ST1,
	ST2
}
public enum UIShelvingSubTask
{
	ST0,
	ST1,
	ST2
}
public enum UISortingSubTask
{
	ST0,
	ST1,
	ST2
}
public enum UIOrderingSubTask
{
	ST0,
	ST1,
	ST2
}
public enum UICautiousnessSubTask
{
	ST0,
	ST1,
	ST2
}
public enum UISocialnessSubTask
{
	ST0,
	ST1,
	ST2
}
public enum UILevel
{
	Tutorial,
	Train_without_Distracters,
	Train_with_Distracters
}
public enum UIDistracter
{
	Loud_Vacuum_Sound,
	Solid_Object_Vacuumed,
	Object_Shatters,
	Announcement,
	Lightning,
	D5,
	D6,
	D7,
	D8,
	D9,
	D10,
	D11
}



public class inputS : MonoBehaviour {

	public static UISkill skill;

	public static UICleaningSubTask cleaningSubTask;
	public static UILoadingSubTask loadingSubTask;
	public static UIMoneyManagementSubTask moneyManagementSubTask;
	public static UIShelvingSubTask shelvingSubTask;
	public static UISortingSubTask sortingSubTask;
	public static UIOrderingSubTask orderingSubTask;
	public static UICautiousnessSubTask cautiousnessSubTask;
	public static UISocialnessSubTask socialnessSubTask;

	public static UILevel level;

	public static UIDistracter distracter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
