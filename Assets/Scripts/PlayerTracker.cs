﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation.Samples;

public class PlayerTracker : MonoBehaviour {

	[Tooltip("Distance from goal coordinates in seconds where the goal will still be counted as hit")]
	[SerializeField] private double goalRadius = 10;
	[SerializeField] private double headingOffsetAngle = 45;
	[SerializeField] private List<PlanetScriptableObject> planetObjects = new List<PlanetScriptableObject>();
	[Space(10)]
	[SerializeField] private Text infoText;
	[SerializeField] private Text planetNameText;
	[SerializeField] private GameObject overlay;
	[Space(15)]
	[SerializeField] private Material debugPlanetMaterial;

	[Space(10)]
	[SerializeField] private PlaceOnPlane placeOnPlane;
	
	private static PlanetInfo currentPlanet;
	private static bool planetIsPlaced = false;
	private static bool planetIsInView = false;

	public static PlanetInfo GetCurrentPlanet() {
		return currentPlanet;
	}
	
	public static bool GetPlanetIsInView() {
		return planetIsInView;
	}

	public void SetPlanetIsPlaced(bool state) {
		planetIsPlaced = state;

		if (state == false) {
			Debug.Log("Removing planet name and info.");
            infoText.text = "";
			planetNameText.text = "";
		}
	}

	private void Start() {

		overlay.SetActive(false);
		StartCoroutine(LocationUpdate());
		
		goalRadius *= 0.00001;
		
		// enable compass
		Input.compass.enabled = true;
	}

	IEnumerator LocationUpdate() {
		while(true){
			if (Input.location.status == LocationServiceStatus.Running) {
				overlay.SetActive(CheckLocationForGoals());
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	public void SetGoalRadius(string goalRadius) {
        if (int.TryParse(goalRadius, out int newRadius)) {
            this.goalRadius = newRadius * 0.00001;
			Debug.Log("New Radius: " + this.goalRadius);
        } else {
			Debug.LogWarning("Could not convert input to int!");
        }
    }
	
	private bool CheckLocationForGoals() {
		PlanetInfo nearestPlanet = new PlanetInfo("", "", 0, 0, null);
		float distanceToNearestPlanet = -1;
		Vector2 playerLocation = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);

		// find neares planet
		foreach (PlanetScriptableObject planet in planetObjects) {
			Vector2 planetLocation = new Vector2(planet.info.latitude, planet.info.longitude);
			float distance = (planetLocation - playerLocation).magnitude;

			Debug.Log("Distance: " + distance);
			
			if (distance < goalRadius) {
				if (distanceToNearestPlanet < 0 || distance < distanceToNearestPlanet) {
					nearestPlanet = planet.info;
					distanceToNearestPlanet = distance;
                }
			}
		}

		// check if planet was found
		if (nearestPlanet.name.Equals("") && nearestPlanet.information.Equals("")) {
			planetIsInView = false;
			return false;
		} else {
			currentPlanet = nearestPlanet;

			if (!planetIsPlaced) {

				Debug.Log("Found planet:" + nearestPlanet.name);
				planetNameText.text = nearestPlanet.name;

				}

			} else {
				planetIsInView = false;
				infoText.text = nearestPlanet.information;
				planetNameText.text = nearestPlanet.name;
			}

			return true;
		}
	}

	public void DespawnPlanet() {
		SetPlanetIsPlaced(false);
		Destroy(placeOnPlane.spawnedObject);
    }

	public static void PlanetWasSpawned() {
		planetIsInView = true;
    }
	}
}