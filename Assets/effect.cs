using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect : MonoBehaviour {

	public GameObject quad0, quad1, quad2, quad3;
	float quad0CurrentX;
	float quad1CurrentX;
	float quad2CurrentZ;
	float quad3CurrentZ;
	float quad01Size, quad23Size;
	public float delay  = 0;

	public float length = 5f;

	float time = 0.25f;

	void Start () 
	{
		StartCoroutine( Disappearance ());
		Deliquescence ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void Deliquescence()
	{
		CorrectSize ();
		StartCoroutine(Extension ());
	}

	IEnumerator Extension ()
	{
		int steps = 20;
		float SizeY01 = quad01Size;
		float SizeY23 = quad23Size;
		float effectSizeX = transform.localScale.x;
		float effectSizeZ = transform.localScale.z;

		yield return new WaitForSeconds (delay);

		for (int i = 0; i < steps; i++)
		{
			SizeY01 += length / steps * 2f / effectSizeZ;
			SizeY23 += length / steps * 2f / effectSizeX;
			quad0.transform.localScale = new Vector3 (0.05f, SizeY01, 0.2f);
			quad1.transform.localScale = new Vector3 (0.05f, SizeY01, 0.2f);
			quad2.transform.localScale = new Vector3 (0.05f, SizeY23, 0.2f);
			quad3.transform.localScale = new Vector3 (0.05f, SizeY23, 0.2f);

			quad0CurrentX += length / steps;
			quad1CurrentX -= length / steps;
			quad2CurrentZ -= length / steps;
			quad3CurrentZ += length / steps;

			quad0.transform.position = new Vector3 (quad0CurrentX , transform.position.y, Falling_cube.current_z);
			quad1.transform.position = new Vector3 (quad1CurrentX, transform.position.y, Falling_cube.current_z);
			quad2.transform.position = new Vector3 (Falling_cube.current_x, transform.position.y, quad2CurrentZ);
			quad3.transform.position = new Vector3 (Falling_cube.current_x, transform.position.y, quad3CurrentZ);

			yield return new WaitForSeconds (time / steps);
		}
	}

	void CorrectSize()
	{
		float effectSizeX = transform.localScale.x;
		float effectSizeZ = transform.localScale.z;

		quad0CurrentX = transform.position.x + Falling_cube.half_current_scale_x + 0.1f;
		quad1CurrentX = transform.position.x - Falling_cube.half_current_scale_x - 0.1f;
		quad2CurrentZ = transform.position.z - Falling_cube.half_current_scale_z - 0.1f;
		quad3CurrentZ = transform.position.z + Falling_cube.half_current_scale_z + 0.1f;

		quad0.transform.position = new Vector3 (quad0CurrentX, transform.position.y, Falling_cube.current_z);
		quad1.transform.position = new Vector3 (quad1CurrentX, transform.position.y, Falling_cube.current_z);
		quad2.transform.position = new Vector3 (Falling_cube.current_x, transform.position.y, quad2CurrentZ);
		quad3.transform.position = new Vector3 (Falling_cube.current_x, transform.position.y, quad3CurrentZ);

		quad01Size = Falling_cube.half_current_scale_z * 2 / effectSizeZ + 0.09f;
		quad23Size = Falling_cube.half_current_scale_x * 2 / effectSizeX;

		quad0.transform.localScale = new Vector3 (0.05f, quad01Size, 0.2f);
		quad1.transform.localScale = new Vector3 (0.05f, quad01Size, 0.2f);
		quad2.transform.localScale = new Vector3 (0.05f, quad23Size, 0.2f);
		quad3.transform.localScale = new Vector3 (0.05f, quad23Size, 0.2f);
	}

	IEnumerator Disappearance()
	{
		int steps = 20;
		Renderer side0 = quad0.GetComponent<Renderer> ();
		Renderer side1 = quad1.GetComponent<Renderer> ();
		Renderer side2 = quad2.GetComponent<Renderer> ();
		Renderer side3 = quad3.GetComponent<Renderer> ();

		yield return new WaitForSeconds (delay);

		for (int i = 0; i < steps; i++) 
		{
			side0.material.color = new Color (1f, 1f, 1f, side1.material.color.a - 1f / steps);
			side1.material.color = new Color (1f, 1f, 1f, side1.material.color.a - 1f / steps);
			side2.material.color = new Color (1f, 1f, 1f, side1.material.color.a - 1f / steps);
			side3.material.color = new Color (1f, 1f, 1f, side1.material.color.a - 1f / steps);
			yield return new WaitForSeconds (time / steps);
		}
		Destroy (gameObject);
	}
}
