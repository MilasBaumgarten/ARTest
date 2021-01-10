using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlanetState {
	NONE,
	FOUND,
	PLACEABLE,
	PLACED
}

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
	[SerializeField] private GameObject planetToPlace;
	private GameObject placedPlanet;

	public PlanetInfo currentPlanet { get; private set; }


	private PlanetState planetState = PlanetState.NONE;
	public PlanetState CurrentPlanetState {
		get { return planetState; }
		set { HandlePlanetStateChange(value); }
	}

	public static PlayerTracker instance { get; private set; }

	private void Start() {
		// singleton
		if (PlayerTracker.instance) {
			Destroy(this);
		} else {
			PlayerTracker.instance = this;
		}

		overlay.SetActive(false);
		StartCoroutine(LocationUpdate());

		goalRadius *= 0.00001;

		// enable compass
		Input.compass.enabled = true;
	}

	private void HandlePlanetStateChange(PlanetState value) {
		Debug.Log("planet state changed from: " + CurrentPlanetState + " to: " + value);

		switch (value) {
			case PlanetState.NONE: {
				infoText.text = "";
				planetNameText.text = "";
				break;
			}
			case PlanetState.FOUND: {
				Debug.Log("Found planet: " + currentPlanet.name);
				planetNameText.text = currentPlanet.name;
				infoText.text = "Du hast einen Planeten gefunden.\nFinde eine geeignete Fläche um den Planeten zu sehen.";
				break;
			}
			case PlanetState.PLACEABLE: {
				planetNameText.text = currentPlanet.name;
				infoText.text = "Du hast einen Planeten und eine geeignete Fläche gefunden.\nDrücke jetzt bitte den Button um den Planeten zu besuchen.";
				break;
			}
			case PlanetState.PLACED: {
				planetNameText.text = currentPlanet.name;
				infoText.text = currentPlanet.information;
				break;
			}
		}

		planetState = value;
	}

	IEnumerator LocationUpdate() {
		while (true) {
			CheckLocationForGoals();
			if (Input.location.status == LocationServiceStatus.Running) {
				if (CurrentPlanetState == PlanetState.NONE || CurrentPlanetState == PlanetState.PLACED) {
					overlay.SetActive(false);
				} else {
					overlay.SetActive(true);
				}

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

	private void CheckLocationForGoals() {
		if (CurrentPlanetState == PlanetState.NONE) {
			PlanetInfo nearestPlanet = new PlanetInfo("", "", 0, 0, null);
			float distanceToNearestPlanet = -1;
			Vector2 playerLocation = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);

			// find neares planet
			foreach (PlanetScriptableObject planet in planetObjects) {
				Vector2 planetLocation = new Vector2(planet.info.latitude, planet.info.longitude);
				float distance = (planetLocation - playerLocation).magnitude;

				if (distance < goalRadius) {
					if (distanceToNearestPlanet < 0 || distance < distanceToNearestPlanet) {
						nearestPlanet = planet.info;
						distanceToNearestPlanet = distance;
					}
				}
			}

			// check if planet was found
			if (nearestPlanet.name.Equals("") && nearestPlanet.information.Equals("")) {
				return;
			} else {
				currentPlanet = nearestPlanet;
				CurrentPlanetState = PlanetState.FOUND;
			}
		}
	}

	public void SpawnPlanet() {
		if (placedPlanet == null) {
			placedPlanet = Instantiate(planetToPlace, ARPlaneTracker.placeableLocation, Quaternion.Euler(0, 0, 0));
		} else {
			placedPlanet.transform.position = ARPlaneTracker.placeableLocation;
		}

		CurrentPlanetState = PlanetState.PLACED;
	}

	public void DespawnPlanet() {
		Debug.Log("Removing planet name and info.");
		CurrentPlanetState = PlanetState.NONE;
		Destroy(placedPlanet);
	}
}