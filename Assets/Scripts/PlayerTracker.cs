using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour {

	[Tooltip("Distance from goal coordinates in seconds where the goal will still be counted as hit")]
	[SerializeField] private double goalRadius = 0;
	[SerializeField] private List<PlanetInfo> planets = new List<PlanetInfo>();
	[SerializeField] private GameObject overlay;
	[Space(15)]
	[SerializeField] private Material debugPlanetMaterial;
	
	private static PlanetInfo currentPlanet;
	private static bool planetIsPlaced = false;
	
	public static PlanetInfo GetCurrentPlanet() {
		return currentPlanet;
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
	}

	IEnumerator LocationUpdate() {
		while(true){
			if (Input.location.status == LocationServiceStatus.Running) {
				BuildLogger.instance.Debug("N: "+ Input.location.lastData.latitude, 0.2f);
				BuildLogger.instance.Debug("E: " + Input.location.lastData.longitude, 0.2f);
				BuildLogger.instance.Debug("Accuracy: " + Input.location.lastData.horizontalAccuracy, 0.2f);

				overlay.SetActive(CheckLocationForGoals());
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	public void SetGoalRadius(string goalRadius) {
		int newRadius = 0;
		if (int.TryParse(goalRadius, out newRadius)) {
			this.goalRadius = newRadius * 0.00001;
			BuildLogger.instance.Debug("New Radius: " + this.goalRadius, 5);
		} else {
			BuildLogger.instance.Debug("Could not convert input to int!", 5);
		}
	}

	public void SetGoal() {
		if (Input.location.status == LocationServiceStatus.Running) {
			planets.Add(new PlanetInfo(
				"DebugPlanet", 
				"This is a debug planet.\nThere is no relevant information.",
				Input.location.lastData.latitude, 
				Input.location.lastData.longitude,
				debugPlanetMaterial
			));
			
			BuildLogger.instance.Debug("New goal added at: N" + Input.location.lastData.latitude + " - E" + Input.location.lastData.longitude, 5);
		}
	}
	
	private bool CheckLocationForGoals() {
		foreach(PlanetInfo planet in planets) {
			if (Input.location.lastData.latitude - planet.latitude < goalRadius &&
				Input.location.lastData.longitude - planet.longitude < goalRadius) {
				
				currentPlanet = planet;
				
				if (!planetIsPlaced) {
					BuildLogger.instance.SetInfo("Du hast einen Planeten gefunden.\nFinde eine geeignete Fläche um den Planeten zu sehen.");
				} else {
					BuildLogger.instance.SetInfo(planet.information);
					BuildLogger.instance.SetPlanetName(planet.name);
				}

				return true;
			}
		}
		
		return false;
	}
}