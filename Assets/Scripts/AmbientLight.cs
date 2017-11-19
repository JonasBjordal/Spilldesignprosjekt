using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientLight : MonoBehaviour
{
  public float cycleTime;
  public float speed = 1.0f;
  public Gradient ambientLight;
  public Gradient fog;
  public Gradient topSky;
  public Gradient botSky;
  public Gradient stars;
  public Gradient sunFlarePower;
  public Gradient moonFlarePower;
  public Gradient lightPower;
  public Transform skyPlane;
  public SpriteRenderer StarSkySR;
  public AnimationCurve rotationCurve;

  public float t = 0.0f;
  private Material sky { get; set; }
  //private Material StarSky { get; set; }
  private Material sunFlare { get; set; }
  private Material moonFlare { get; set; }
  private Light gameLight { get; set; }


  public Color StarColor;

	// Use this for initialization
	void Start ()
  {
    #if UNITY_EDITOR
    if (runInEditMode)
    {
      sky = GetComponentInChildren <MeshRenderer> ().sharedMaterial;
      sunFlare = skyPlane.FindChild ("Sun").FindChild ("Flare").GetComponent <Renderer> ().sharedMaterial;
      moonFlare = skyPlane.FindChild ("Moon").FindChild ("Flare").GetComponent <Renderer> ().sharedMaterial;
		

	  //StarSky = GameObject.FindGameObjectWithTag("Stars").GetComponent <SpriteRenderer> ().sharedMaterial;
	}
    else
    #endif
    {
      sky = GetComponentInChildren <MeshRenderer> ().material;
      sunFlare = skyPlane.FindChild ("Sun").FindChild ("Flare").GetComponent <Renderer> ().material;
      moonFlare = skyPlane.FindChild ("Moon").FindChild ("Flare").GetComponent <Renderer> ().material;
    
	  //StarSky = GameObject.FindGameObjectWithTag("Stars").GetComponent <SpriteRenderer> ().material;	
	}
    gameLight = FindObjectOfType <Light> ();
	}
	
	// Update is called once per frame
	void Update ()
  {
		t += (Time.deltaTime * speed) / (cycleTime * 0.80f); //0.8 for å gjøre cycleTime til sekunder
    RenderSettings.ambientLight = ambientLight.Evaluate (t);
    RenderSettings.fogColor = fog.Evaluate (t);

    sky.SetColor ("_TopColor", topSky.Evaluate (t));
    sky.SetColor ("_BottomColor", botSky.Evaluate (t));
		//print( stars.Evaluate (t));
		//Color32 StarColor = new Color (stars.Evaluate (t));
		//StarSkySR.color = new Color (StarColor); //stars.Evaluate (t)

    skyPlane.localEulerAngles = -Vector3.forward * rotationCurve.Evaluate (t);

    sunFlare.SetFloat ("_Power", sunFlarePower.Evaluate (t).a);
    moonFlare.SetFloat ("_Power", moonFlarePower.Evaluate (t).a);

    if (gameLight != null)
      gameLight.color = lightPower.Evaluate (t);

    if (t > 1.25f)
    {
      t = 0.0f;
    }
	}
}
