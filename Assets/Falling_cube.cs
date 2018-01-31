using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class Falling_cube : MonoBehaviour {

	public AudioClip extention;
	public AudioClip knock;
	public AudioClip tunz;

	public GameObject effect;

	public GameObject particle;

	public GameObject camera;

	AudioSource audio;

	static bool new_game;
	public GameObject new_cube;
	public GameObject sliced_cube;

	public float maxSizeOfSide = 4f;

	public static int chainOfDecrease = 5; 
	public static int decrease = 0;

	public static int counter_of_extention = 0;
	public static bool is_direction_z;


	bool is_right_side;
	public float right_side;
	public float left_side = -15;
	public float speed = 100;
	protected bool stop_cube;

	public float put_buffer = 1;

	public static float new_step = 1f;
	public static float current_x = 0f;
	public static float current_z = 0f;
	public static float half_current_scale_x = 2f;
	public static float half_current_scale_z = 2f;

	public static int count = 0;

	// Use this for initialization
	void Start () 
	{
		camera.GetComponent<Camera> ().backgroundColor =  new Color (Cube.rBack, Cube.gBack, Cube.bBack);
		audio = GetComponent<AudioSource> ();

		if (new_game) 
		{
			Move_cube(current_x);
			new_game = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//DIRECTION false = x, true = z;
		float x = transform.position.x;
		float z = transform.position.z;

		if (!stop_cube) 
		{
			if (is_direction_z)
				Move_cube (z);
			else
				Move_cube (x);
		}

		if (Input.GetKeyDown ("space") || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))
		{
			if(is_direction_z)
				Put_cube (z - current_z);
			else
				Put_cube (x - current_x);
		}
	}

	void Move_cube(float position)
	{
		if (!is_direction_z) 
		{
			if (transform.position.x < right_side && !is_right_side)
				transform.Translate (Vector3.right * Time.deltaTime * speed);
			
			if (is_right_side && transform.position.x > left_side)
				transform.Translate (Vector3.left * Time.deltaTime * speed);
		} 
		else 
		{
			if (transform.position.z < right_side && !is_right_side) 
				transform.Translate (new Vector3 (0f, 0f, Time.deltaTime * speed));
		
			if (is_right_side && transform.position.z > left_side)
				transform.Translate (new Vector3 (0f, 0f, -Time.deltaTime * speed));
		}

		if (position > right_side)
			is_right_side = true;

		if (position < left_side)
			is_right_side = false;
	}


	void Put_cube(float difference_coordinate)
	{
		stop_cube = true;
		if (Mathf.Abs(difference_coordinate) < put_buffer)
			Put_all_cube ();
		else if(is_crossing())
			Cut_cube ();
		else 
			End_game ();
	}


	void Put_all_cube()
	{
		count++;
		counter_of_extention++;
		if (counter_of_extention >= 5)
			Expand_cube ();
		else if (counter_of_extention == 4) 
		{
			transform.position = new Vector3 (current_x, transform.position.y, current_z);
			GameObject newEffect = Instantiate (effect, new Vector3 (current_x, transform.position.y, current_z), transform.rotation) as GameObject;
			GameObject newEffect2 = Instantiate (effect, new Vector3 (current_x, transform.position.y, current_z), transform.rotation) as GameObject;
			newEffect2.GetComponent<effect> ().delay = 0.15f;
			GameObject newEffect3 = Instantiate (effect, new Vector3 (current_x, transform.position.y, current_z), transform.rotation) as GameObject;
			newEffect3.GetComponent<effect> ().delay = 0.30f;
		} 
		else if (counter_of_extention == 3)
		{
			transform.position = new Vector3 (current_x, transform.position.y, current_z);
			GameObject newEffect = Instantiate (effect, new Vector3 (current_x, transform.position.y, current_z), transform.rotation) as GameObject;
			GameObject newEffect2 = Instantiate (effect, new Vector3 (current_x, transform.position.y, current_z), transform.rotation) as GameObject;
			newEffect2.GetComponent<effect> ().delay = 0.15f;
		}
		else 
		{
			transform.position = new Vector3 (current_x, transform.position.y, current_z);
			GameObject newEffect = Instantiate (effect, new Vector3 (current_x, transform.position.y, current_z), transform.rotation) as GameObject;
			newEffect.GetComponent<effect> ().length = 0;
		}
		Initiate_cube ();
		Switch_direction (); 

		if (!(counter_of_extention >= 5)) 
		{
			audio.clip = tunz;
			audio.Play ();
		}
	}

	void Expand_cube()
	{
		if (is_direction_z && transform.localScale.z < maxSizeOfSide)
			ExtentionByZCoordinate ();
		else if (is_direction_z && transform.localScale.x < maxSizeOfSide) 
			ExtentionByXCoordinate ();
		else 
			transform.position = new Vector3 (current_x, transform.position.y, current_z);

		if (!is_direction_z && transform.localScale.x < maxSizeOfSide)
			ExtentionByXCoordinate ();
		else if (!is_direction_z && transform.localScale.z < maxSizeOfSide) 
			ExtentionByZCoordinate ();
		else 
			transform.position = new Vector3 (current_x, transform.position.y, current_z);

		audio.clip = extention;
		audio.Play ();

		GameObject pixels = Instantiate (particle, new Vector3(transform.position.x, transform.position.y, transform.position.z), particle.transform.rotation) as GameObject;

		//start particle system
	}

	void ExtentionByZCoordinate()
	{
		float extentionSize = 0.5f;
		if (transform.localScale.z + extentionSize > maxSizeOfSide) 
			extentionSize = 0.5f - ((transform.localScale.z + extentionSize) - maxSizeOfSide);

		StartCoroutine (ExtentionZ(extentionSize));

		half_current_scale_z = (transform.localScale.z + extentionSize) / 2f;
		if (current_z < 0)
			current_z += extentionSize / 2f;
		else
			current_z -= extentionSize / 2f;
	}

	void ExtentionByXCoordinate()
	{
		float extentionSize = 0.5f;
		if (transform.localScale.x + extentionSize > maxSizeOfSide) 
			extentionSize = 0.5f - ((transform.localScale.x + extentionSize) - maxSizeOfSide);

		StartCoroutine (ExtentionX(extentionSize));

		half_current_scale_x = (transform.localScale.x + extentionSize) / 2f;
		if (current_x < 0)
			current_x += extentionSize / 2f;
		else
			current_x -= extentionSize / 2f;
	}

	void End_game()
	{	
		Restart ();
	}

	void Cut_cube()
	{
		counter_of_extention = 0;
		count++;
		float difference = Mathf.Max(Mathf.Abs (current_x - transform.position.x), Mathf.Abs (current_z - transform.position.z));
		if (!is_direction_z) 
		{
			transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x - difference), 1f, transform.localScale.z);
			//задали размер новому кубу

			//задать x 
			if (transform.position.x > current_x)
				transform.position = new Vector3 (current_x + half_current_scale_x - 0.5f * transform.localScale.x, transform.position.y, transform.position.z);
			else
				transform.position = new Vector3 (current_x + -half_current_scale_x + 0.5f * transform.localScale.x, transform.position.y, transform.position.z);
			
			Initiate_sliced_cube (half_current_scale_x);

			half_current_scale_x = 0.5f * transform.localScale.x;
		} 
		else 
		{
			transform.localScale = new Vector3 (transform.localScale.x, new_step, Mathf.Abs(transform.localScale.z - difference));

			if (transform.position.z > current_z)
				transform.position = new Vector3 (transform.position.x, transform.position.y, current_z + half_current_scale_z - 0.5f * transform.localScale.z);
			else
				transform.position = new Vector3 (transform.position.x, transform.position.y, current_z + -half_current_scale_z + 0.5f * transform.localScale.z);

			Initiate_sliced_cube (half_current_scale_z);

			half_current_scale_z = 0.5f * transform.localScale.z;
		}

		audio.clip = knock;
		audio.Play ();

		current_z = transform.position.z;
		current_x = transform.position.x;
		is_right_side = false;
		Initiate_cube ();
		Switch_direction ();
	}
		
	void Initiate_cube()
	{
		GameObject newCube = Instantiate<Object>(new_cube) as GameObject;
		newCube.transform.localPosition = new Vector3(current_x, transform.position.y + new_step, current_z);
		newCube.transform.localScale = new Vector3 (2 * half_current_scale_x, transform.localScale.y, 2f * half_current_scale_z);
		this.enabled = false; //отключение скрипта!!!!
		StartCoroutine(UpCameraSmooth());
	}

	void Initiate_sliced_cube (float half_current_scale)
	{
		GameObject sl_cube = Instantiate (sliced_cube) as GameObject;
		if (!is_direction_z) 
		{
			sl_cube.transform.localScale = new Vector3 (Mathf.Abs(half_current_scale * 2 - transform.localScale.x), new_step, half_current_scale_z * 2);
			if (transform.position.x > current_x)
				sl_cube.transform.position = new Vector3 (current_x + half_current_scale_x + 0.5f * sl_cube.transform.localScale.x, transform.position.y + new_step, transform.position.z);
			else
				sl_cube.transform.position = new Vector3 (current_x - half_current_scale_x - 0.5f * sl_cube.transform.localScale.x, transform.position.y + new_step, transform.position.z);
		} 
		else 
		{
			sl_cube.transform.localScale = new Vector3 (half_current_scale_x * 2, new_step, Mathf.Abs(half_current_scale * 2 - transform.localScale.z));
			if (transform.position.z > current_z)
				sl_cube.transform.position = new Vector3 (current_x, transform.position.y + new_step, current_z + half_current_scale_z + 0.5f * sl_cube.transform.localScale.z);
			else
				sl_cube.transform.position = new Vector3 (current_x, transform.position.y + new_step, current_z - half_current_scale_z - 0.5f * sl_cube.transform.localScale.z);
		}

		sl_cube.GetComponent<Renderer>().material.SetColor("_Color", CubeColor.CurrentColor());

	}

	void UpCamera()
	{
		Vector3 camPos = Camera.allCameras [0].transform.position;
		Camera.allCameras [0].transform.position = new Vector3 (camPos.x, camPos.y + new_step, camPos.z);
	}

	void Switch_direction()
	{
		is_direction_z = !is_direction_z;
	}

	bool is_crossing()
	{
		float x = transform.position.x;
		float z = transform.position.z;
		float width = transform.localScale.x;
		float length = transform.localScale.z;

		if (Mathf.Abs (x - current_x) < width && Mathf.Abs (z - current_z) < length)
			return true;
		else
			return false;
	}

	IEnumerator UpCameraSmooth() {
		background.UpdateBackground ();
		int steps = 20;
		float time = 0.1f;
		Transform cameraTransform = Camera.allCameras [0].transform;
		for (int i = 0; i < steps; i++) 
		{
			background.UpBackground ( new_step / steps);
			cameraTransform.position = new Vector3 (cameraTransform.position.x, cameraTransform.position.y + new_step / steps , cameraTransform.position.z);
			yield return new WaitForSeconds (time/steps);
		}
	}

	IEnumerator ExtentionZ(float extentionSize)
	{
		int steps = 20;
		float bufferCurrentZ = current_z, CurrentZ = current_z;

		for (int i = 0; i < steps; i++) 
		{
			if (CurrentZ < 0) 
			{
				transform.position = new Vector3 (current_x, transform.position.y, bufferCurrentZ + extentionSize / steps / 2f);
				bufferCurrentZ += extentionSize / steps / 2f;
			} 
			else 
			{
				transform.position = new Vector3 (current_x, transform.position.y, bufferCurrentZ - extentionSize / steps / 2f);
				bufferCurrentZ -= extentionSize / steps / 2f;
			}
			
			transform.localScale = new Vector3 (transform.localScale.x, 1f, transform.localScale.z + extentionSize / steps);
			yield return new WaitForSeconds (0.01f);
		}
	}

	IEnumerator ExtentionX(float extentionSize)
	{
		int steps = 20;
		float bufferCurrentX = current_x, CurrentX = current_x;

		for (int i = 0; i < steps; i++) 
		{
			if (CurrentX < 0) 
			{
				transform.position = new Vector3 (bufferCurrentX + extentionSize / steps / 2f, transform.position.y, current_z);
				bufferCurrentX += extentionSize / steps / 2f;
			} 
			else 
			{
				transform.position = new Vector3 (bufferCurrentX - extentionSize / steps / 2f, transform.position.y, current_z);
				bufferCurrentX -= extentionSize / steps / 2f;
			}

			transform.localScale = new Vector3 (transform.localScale.x + extentionSize / steps, 1f, transform.localScale.z);
			yield return new WaitForSeconds (0.01f);
		}
	}
		
	void Restart()
	{
		string name = "Falling_cube(Clone)";
		Destroy(GameObject.Find(name));
		for (int i = 0; i < count; i++) 
		{
			Destroy (GameObject.Find (name+"(Clone)"));
			name += "(Clone)";
		}

		string sliced = "Sliced_cube(Clone)";
		while(GameObject.Find(sliced) != null)
			DestroyImmediate(GameObject.Find(sliced));

		Camera.allCameras [0].transform.position = new Vector3 (-11.8f, 23.89f, -10f);

		count = 0;

		GameObject first_cube = GameObject.Find ("Falling_cube");
		first_cube.transform.localScale = new Vector3(maxSizeOfSide, new_step, maxSizeOfSide);
		first_cube.transform.position = new Vector3 (0f, new_step, 0f);
		first_cube.GetComponent<Falling_cube>().stop_cube = false;
		first_cube.GetComponent<Falling_cube>().enabled = true;
		first_cube.GetComponent<CubeColor> ().ResetColors ();
		first_cube.GetComponent<Renderer>().material.SetColor("_Color", CubeColor.CurrentColor());

		is_direction_z = false;
		is_right_side = false;

		new_step = 1f;
		current_x = 0f;
		current_z = 0f;
		half_current_scale_x = maxSizeOfSide / 2f;
		half_current_scale_z = maxSizeOfSide / 2f;

		new_game = true;

		GameObject.Find("CUBE").GetComponent<Renderer>().material.SetColor("_Color", CubeColor.CurrentColor());
		GameObject.Find ("shadow").GetComponent<Renderer> ().material.SetColor ("_Color", CubeColor.CurrentColor());

		background.ResetBackground ();
	}
}
