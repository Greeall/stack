using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour {

	public static background I;

	public List<Color> topColors = new List<Color>();
	public List<Color> bottomColors = new List<Color> ();

	public static Color firstTopColor;
	public static Color firstBottomColor;

	public static Color lastTopColor;
	public static Color lastBottomColor;

	public static int steps = 11;
	public static int currentStep = 0;

	bool startGame = true;

	static Renderer render;

	void Awake() {
		I = this;
	}

	void Start () 
	{	
		firstTopColor = topColors[Random.Range(0, topColors.Count)];
		firstBottomColor = bottomColors[Random.Range(0, bottomColors.Count)];
	
		lastTopColor = topColors[Random.Range(0, topColors.Count)];
		lastBottomColor = bottomColors[Random.Range(0, bottomColors.Count)];

		render = GetComponent<Renderer> ();

		render.material.SetColor ("_GradientTopColor", firstTopColor);
		render.material.SetColor ("_GradientBottomColor", firstBottomColor);
	}

	public static Color CurrentTopColor()
	{
		return Color.Lerp (firstTopColor, lastTopColor, (float)currentStep / steps);
	}

	public static Color CurrentBottomColor()
	{
		return Color.Lerp (firstBottomColor, lastBottomColor, (float)currentStep / steps);
	}

	public static void UpBackground(float y)
	{
		float topLine = render.material.GetFloat ("_TopLine");
		float bottomLine = render.material.GetFloat ("_BottomLine");
		render.material.SetFloat ("_TopLine", topLine + y);
		render.material.SetFloat ("_BottomLine", bottomLine + y);
	}

	public static void ResetBackground()
	{
		render.material.SetFloat ("_TopLine", -63f);
		render.material.SetFloat ("_BottomLine", -80f);
	}

	public static void SelectNextColor()
	{
		firstTopColor = lastTopColor;
		firstBottomColor = lastBottomColor;

		do
			lastBottomColor = I.bottomColors [Random.Range (0, I.bottomColors.Count)];
		while (firstBottomColor == lastBottomColor);

		do
			lastTopColor = I.topColors [Random.Range (0, I.topColors.Count)];
		while (firstTopColor == lastTopColor);
	}

	public static void UpdateBackground()
	{

		if (currentStep >= steps) 
		{
			SelectNextColor ();
			currentStep = 0;
		}

		render.material.SetColor ("_GradientTopColor", CurrentTopColor());
		render.material.SetColor ("_GradientBottomColor", CurrentBottomColor());
		currentStep++;
	}
}
