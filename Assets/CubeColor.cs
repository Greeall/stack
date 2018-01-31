using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeColor : MonoBehaviour {
	public List<Color> colors = new List<Color> ();

	public static List<Color> allColors = new List<Color> ();
	public static Color startColor;
	public static Color endColor;
	public static int steps = 6;
	public static int currentStep = 0;
	public static bool firstCube = true;

	public static Color CurrentColor() {


		return Color.Lerp (startColor, endColor, (float)currentStep / steps);
	}

	public void ResetColors() {
		startColor = GetRandomColor ();
		endColor = GetRandomColor (startColor);
		currentStep = 0;
	}

	// Use this for initialization
	void Awake () {
		if (firstCube) {
			allColors = colors;
			startColor = GetRandomColor ();
			endColor = GetRandomColor (startColor);
			firstCube = false;
		}
		if (currentStep >= steps) {
			ChangeColors ();
		}
		SetCurrentColor ();
	}

	void ChangeColors() {
		startColor = endColor;
		endColor = GetRandomColor (startColor);
		currentStep = 0;
	}

	void SetCurrentColor() {
		currentStep++;
		Renderer renderer = gameObject.GetComponent<Renderer> ();
		renderer.material.SetColor("_Color", CurrentColor());
	}

	Color GetRandomColor() {
		return allColors [Random.Range (0, allColors.Count)];
	}

	Color GetRandomColor(Color dontMatch) {
		Color c;
		do {
			c = GetRandomColor ();
		} while(c == dontMatch);
		return c;
	}
}
