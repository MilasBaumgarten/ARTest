using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;

public class PlayerTracker : MonoBehaviour {

	[Tooltip("Distance from goal coordinates in seconds where the goal will still be counted as hit")]
	[SerializeField] private double goalRadius = 10;
	[SerializeField] private double headingOffsetAngle = 45;
	[SerializeField] private List<PlanetScriptableObject> planetObjects = new List<PlanetScriptableObject>();
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

	public static void SetPlanetIsPlaced(bool state) {
		planetIsPlaced = state;

		if (state == false) {
			BuildLogger.instance.SetInfo("");
			BuildLogger.instance.SendMessage("");
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
            BuildLogger.instance.Debug("New Radius: " + this.goalRadius, 5);
        } else {
            BuildLogger.instance.Debug("Could not convert input to int!", 5);
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
			
			if (distance < goalRadius) {
				if (distanceToNearestPlanet < 0 || distance < distanceToNearestPlanet) {
					nearestPlanet = planet.info;
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
				float delta = GetHeadingDelta(nearestPlanet);

				if (delta > headingOffsetAngle) {
					BuildLogger.instance.SetInfo("Du hast einen Planeten gefunden.\nDrehe dich so, dass du ihn sehen kannst. " + delta);
					planetIsInView = false;
				} else {
					BuildLogger.instance.SetInfo("Du hast einen Planeten gefunden.\nFinde eine geeignete Fläche um den Planeten zu sehen. " + delta);
					planetIsInView = true;
				}

			} else {
				planetIsInView = false;
				BuildLogger.instance.SetInfo(nearestPlanet.information);
				BuildLogger.instance.SetPlanetName(nearestPlanet.name);
			}

			return true;
		}
	}

	public void DespawnPlanet() {
		SetPlanetIsPlaced(false);
		Destroy(placeOnPlane.spawnedObject);
    }
	
	private float GetHeadingDelta(PlanetInfo planet) {
		Vector2 planetLocation = new Vector2(planet.latitude, planet.longitude);
		Vector2 playerLocation = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
		Vector2 north = new Vector2(0, 1);
		
		float angle = Mathf.Acos(Vector2.Dot(north, (planetLocation - playerLocation).normalized)) * Mathf.Rad2Deg;
		
		// map angle to [0, 360]
		if (angle < 0) {
			angle += 360;
		}
		
		return Mathf.Abs(Input.compass.trueHeading - angle);
	}
}