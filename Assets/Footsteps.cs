using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour {

	[FMODUnity.EventRef] 
	public string InputFootsteps;
	FMOD.Studio.EventInstance FootstepsEvent;
	FMOD.Studio.ParameterInstance WoodParameter;
	FMOD.Studio.ParameterInstance RockParameter;
	FMOD.Studio.ParameterInstance GrassParameter;
	FMOD.Studio.ParameterInstance SwimParameter;

	bool Moving;
	public float walkingspeed;
	private float WoodValue;
	private float RockValue;
	private float GrassValue;
	private float SwimValue;
	private bool Grounded;
	private Animator m_Anim;

	void Awake ()
	{
		m_Anim = GetComponent<Animator>();

	}



	void Start ()
	{
		FootstepsEvent = FMODUnity.RuntimeManager.CreateInstance(InputFootsteps);
		FootstepsEvent.getParameter ("Wood", out WoodParameter);
		FootstepsEvent.getParameter ("Rock", out RockParameter);
		FootstepsEvent.getParameter ("Grass", out GrassParameter);
		FootstepsEvent.getParameter ("Swim", out SwimParameter);

		InvokeRepeating ("CallFootsteps", 0, walkingspeed);
	}

	void Update () 
	{
		Grounded = m_Anim.GetBool("Ground");

		WoodParameter.setValue (WoodValue);
		RockParameter.setValue (RockValue);
		GrassParameter.setValue (GrassValue);
		SwimParameter.setValue (SwimValue);

		if (Input.GetAxis ("Horizontal") >= 0.01f ||  Input.GetAxis ("Horizontal") <= -0.01f)
		{
			if (Grounded) 
			{
				Moving = true;
			} 
			else if (!Grounded) 
			{
				Moving = false;
			}
		} 
		else if (Input.GetAxis ("Vertical") == 0 || Input.GetAxis ("Horizontal") == 0) 
		{
			Moving = false;
		}
	}

	void CallFootsteps ()
	{
		if (Moving == true) 
		{
			//FootstepsEvent.start();
			FMODUnity.RuntimeManager.PlayOneShot (InputFootsteps);
		} 
		else if (Moving == false) 
		{
			//Debug.Log ("player is moving = false");
		}
	}

	void OnDisable ()
	{
		Moving = false;
	}

	void OnTriggerStay(Collider MaterialCheck)
	{
		//float FadeSpeed = 10f;
//		playerisgrounded = true;

		if (MaterialCheck.CompareTag ("Wood:Material")) 
		{
			WoodValue = 1f;
			RockValue = 0f;
			GrassValue = 0f;
			SwimValue = 0f;
			//WoodValue = Mathf.Lerp (WoodValue, 1f, Time.deltaTime * FadeSpeed);
			//MetalValue = Mathf.Lerp (MetalValue, 0f, Time.deltaTime * FadeSpeed);
			//GrassValue = Mathf.Lerp (GrassValue, 0f, Time.deltaTime * FadeSpeed);
		}
		if (MaterialCheck.CompareTag ("Rock:Material")) 
		{
			WoodValue = 0f;
			RockValue = 1f;
			GrassValue = 0f;
			SwimValue = 0f;
			//WoodValue = Mathf.Lerp (WoodValue, 0f, Time.deltaTime * FadeSpeed);
			//MetalValue = Mathf.Lerp (MetalValue, 1f, Time.deltaTime * FadeSpeed);
			//GrassValue = Mathf.Lerp (GrassValue, 0f, Time.deltaTime * FadeSpeed);
		}
		if (MaterialCheck.CompareTag ("Grass:Material")) 
		{
			WoodValue = 0f;
			RockValue = 0f;
			GrassValue = 1f;
			SwimValue = 0f;
			//WoodValue = Mathf.Lerp (WoodValue, 0f, Time.deltaTime * FadeSpeed);
			//MetalValue = Mathf.Lerp (MetalValue, 0f, Time.deltaTime * FadeSpeed);
			//GrassValue = Mathf.Lerp (GrassValue, 1f, Time.deltaTime * FadeSpeed);
		}
		if (MaterialCheck.CompareTag ("Water:Material")) 
		{
			WoodValue = 0f;
			RockValue = 0f;
			GrassValue = 0f;
			SwimValue = 1f;
			//WoodValue = Mathf.Lerp (WoodValue, 0f, Time.deltaTime * FadeSpeed);
			//MetalValue = Mathf.Lerp (MetalValue, 0f, Time.deltaTime * FadeSpeed);
			//GrassValue = Mathf.Lerp (GrassValue, 1f, Time.deltaTime * FadeSpeed);
		}
	}

//	void OnTriggerExit (Collider MaterialCheck)
//	{
//		Grounded = false;
//	}
}