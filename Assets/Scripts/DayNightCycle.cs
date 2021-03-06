﻿using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {
	public Gradient skyColors;
	public Gradient skyMiddleColors;
	public Gradient skyBottomColors;
	public Gradient lightColors;
	[Range(0.0f, 24.0f)]
	public float timeOfDay = 12f;
	public float timeSpeed = .1f;
	public AnimationCurve sunAnimationCurve;
	public AnimationCurve moonAnimationCurve;
	public AnimationCurve sunHorizontalAnimationCurve;
	public AnimationCurve moonHorizontalAnimationCurve;
	public AnimationCurve nightSkyTransparency;
	public int startShootingStarsAt;
	public int stopShootingStarsAt;
	public float shootingStarProbability;

	GameObject sky;
	Mesh skyMesh;
	Vector3[] skyVertices;
	Color[] skyVertexColors;
	GameObject sun;
	GameObject moon;
	GameObject nightSky;
	Renderer moonRenderer;
	Renderer nightSkyRenderer;
	Light skylight;
	GameObject shootingStartsGO;
	ParticleSystem shootingStars;
	bool canShootStars = false;

	// public static DayNightCycle Instance { get; private set; }

	// Use this for initialization
	void Start () {
		sky = GameObject.Find("SkyGradient");
		if (sky) {
			skyMesh = sky.GetComponent<MeshFilter> ().mesh;
			skyVertices = skyMesh.vertices;
			skyVertexColors = new Color[skyVertices.Length];
		}

		sun = GameObject.Find("sun");

		moon = GameObject.Find("moon");
		if (moon) {
			moonRenderer = moon.GetComponent<Renderer>();
		}

		skylight = GameObject.Find("Skylight").GetComponent<Light>();

		nightSky = GameObject.Find("NightSky");
		if (nightSky) {
			nightSkyRenderer = nightSky.GetComponent<Renderer>();
		}
		shootingStartsGO = GameObject.Find ("ShootingStars");
		if (shootingStartsGO) {
			shootingStars = shootingStartsGO.GetComponent<ParticleSystem> ();
		}
	}

	void Update () {
		IncreaseTime();
		Color currentSkyColor = GetCurrentSkyColor("top");
		Color currentSkyMiddleColor = GetCurrentSkyColor("middle");
		Color currentSkyBottomColor = GetCurrentSkyColor("bottom");
		if (sky) {
			ChangeSkyColor(currentSkyColor, currentSkyMiddleColor, currentSkyBottomColor);
		}
		Color currentLightColor = GetCurrentSkylightColor();
		ChangeSkylightColor(currentLightColor);
		if (sun) {
			UpdateSunPosition();
		}
		if (moon) {
			UpdateMoonPosition();
		}
		if (nightSky) {
			UpdateNighskyVisibility();
		}
		if (shootingStartsGO) {
			UpdateShootingStars ();
		}
	}

	void IncreaseTime() {
		if (timeOfDay > 24) {
			timeOfDay = 0;
		} else {
			timeOfDay += (timeSpeed * Time.deltaTime);
		}
	}
		

	void UpdateShootingStars() {
		if (Mathf.Round (timeOfDay) == startShootingStarsAt) {
			canShootStars = true;
		} else if (Mathf.Round (timeOfDay) == stopShootingStarsAt) {
			canShootStars = false;
		}

		if (canShootStars) {
			float randomValue = Random.value;
			if (randomValue < shootingStarProbability) {
				shootingStars.Emit (1);
			}
		}
	}

	void UpdateNighskyVisibility() {
		float currentPointInCurve = nightSkyTransparency.Evaluate(timeOfDay);
		nightSkyRenderer.material.color = new Color(1, 1, 1, currentPointInCurve);
	}

	void ChangeSkyColor(Color color, Color middleColor, Color bottomColor) {
		skyVertexColors [3] = color;
		skyVertexColors [2] = color;
		skyVertexColors [1] = middleColor;
		skyVertexColors [0] = middleColor;
		skyVertexColors [5] = bottomColor;
		skyVertexColors [4] = bottomColor;
		skyMesh.colors = skyVertexColors;
	}

	void ChangeSkylightColor (Color color) {
		skylight.color = color;
	}

	void UpdateSunPosition() {
		Vector3 sunPosition = sun.transform.position;
		sunPosition.y = sunAnimationCurve.Evaluate(timeOfDay);
		sun.transform.position = sunPosition;
		Vector3 currentLocalPosition = sun.transform.localPosition;
		sun.transform.localPosition = new Vector3(sunHorizontalAnimationCurve.Evaluate(timeOfDay), currentLocalPosition.y, currentLocalPosition.z);
	}

	void UpdateMoonPosition() {
		Vector3 moonPosition = moon.transform.position;
		float currentPointInCurve = moonAnimationCurve.Evaluate(timeOfDay);
		moonPosition.y = currentPointInCurve;
		moon.transform.position = moonPosition;
		Vector3 currentLocalPosition = moon.transform.localPosition;
		moon.transform.localPosition = new Vector3(moonHorizontalAnimationCurve.Evaluate(timeOfDay), currentLocalPosition.y, currentLocalPosition.z);
		moonRenderer.material.color = new Color(255, 255, 255, currentPointInCurve / 3);
	}

	public Color GetCurrentSkyColor(string position) {
		if (position == "top") {
			return skyColors.Evaluate (timeOfDay / 24);
		} else if (position == "middle") {
			return skyMiddleColors.Evaluate (timeOfDay / 24);
		} else {
			return skyBottomColors.Evaluate (timeOfDay / 24);
		}

	}

	public Color GetCurrentSkylightColor() {
		return lightColors.Evaluate(timeOfDay / 24);
	}
}