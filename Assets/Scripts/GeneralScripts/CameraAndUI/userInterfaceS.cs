using UnityEngine;
using System.Collections;

public static class userInterfaceS
{
	static Vector2[] ViewPortCenters = new Vector2[]{new Vector2(960,540), new Vector2(2432, 384), new Vector2(4224, 400)};
	static Vector2[] ViewPortTopLefts = new Vector2[]{new Vector2(0,0), new Vector2(1920, 0), new Vector2(3074, 0)};
	static Vector2[] ViewPortDimentions = new Vector2[]{new Vector2(1920,1080), new Vector2(1024, 768), new Vector2(2300, 800)};
	
	public static void drawTex(Vector2 _relativePos, Vector2 _relativeDim, Texture _tex)
	{
		for(int v=0; v<3; v++)
		{
			GUI.DrawTexture(new Rect(ViewPortTopLefts[v].x + ViewPortDimentions[v].x * _relativePos.x, 
			                         ViewPortTopLefts[v].y + ViewPortDimentions[v].y * _relativePos.y, 
			                         ViewPortDimentions[v].x*_relativeDim.x, 
			                         ViewPortDimentions[v].y*_relativeDim.y), 
			                _tex, ScaleMode.ScaleToFit, true, 0);
		}	
	}
	public static void drawTexCenter(Vector2 _relativeDim, Texture _tex)
	{
		for(int v=0; v<3; v++)
		{
			GUI.DrawTexture(new Rect(ViewPortCenters[v].x - ViewPortDimentions[v].x * _relativeDim.x * 0.5f, 
			                         ViewPortCenters[v].y - ViewPortDimentions[v].y * _relativeDim.y * 0.5f, 
			                         ViewPortDimentions[v].x*_relativeDim.x, 
			                         ViewPortDimentions[v].y*_relativeDim.y), 
			                _tex, ScaleMode.ScaleToFit, true, 0);
		}	
	}
	public static void drawText(Vector2 _relativePos, Vector2 _relativeDim, string _text, GUIStyle _style)
	{
		for(int v=0; v<3; v++)
		{
			GUI.Label(new Rect(	ViewPortTopLefts[v].x + ViewPortDimentions[v].x * _relativePos.x, 
			                    ViewPortTopLefts[v].y + ViewPortDimentions[v].y * _relativePos.y, 
			                    ViewPortDimentions[v].x*_relativeDim.x, 
			                    ViewPortDimentions[v].y*_relativeDim.y), 
			                _text, _style);
		}	
	}
	public static void drawTextCenter(Vector2 _relativeDim, string _text, GUIStyle _style)
	{
		for(int v=0; v<3; v++)
		{
			GUI.Label(new Rect(	ViewPortCenters[v].x - ViewPortDimentions[v].x * _relativeDim.x * 0.5f, 
			                    ViewPortCenters[v].y - ViewPortDimentions[v].y * _relativeDim.y * 0.5f, 
			                    ViewPortDimentions[v].x*_relativeDim.x, 
			                    ViewPortDimentions[v].y*_relativeDim.y), 
			                _text, _style);
		}	
	}	
	public static void drawTexEverywhere(Texture _tex, float _alpha)
	{
		Color temp = GUI.color;
		GUI.color = new Color(temp.r, temp.g, temp.b, _alpha);
		int tempI = GUI.depth;
		GUI.depth = -1000;
		for(int v=0; v<3; v++)
		{
			GUI.DrawTexture(new Rect(	ViewPortCenters[v].x - ViewPortDimentions[v].x * 0.5f, 
			                    		ViewPortCenters[v].y - ViewPortDimentions[v].y * 0.5f, 
			                    		ViewPortDimentions[v].x, 
			                    		ViewPortDimentions[v].y),
									_tex, ScaleMode.ScaleAndCrop, true);	
		}	
		GUI.color = temp;
		GUI.depth = tempI;
	}	
}
