using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour {

	[Tooltip("Distance from goal coordinates in seconds where the goal will still be counted as hit")]
	[SerializeField] private double goalRadius = 0;
	[SerializeField] private List<Goal> goals = new List<Goal>();
	[SerializeField] private BuildLogger logger;
	[SerializeField] private GameObject overlay;

	private void Start() {
		overlay.SetActive(false);
		StartCoroutine(LocationUpdate());
	}

	IEnumerator LocationUpdate() {
		while(true){
			logger.ClearLog();
			if (Input.location.status == LocationServiceStatus.Running) {
				logger.PrintMessage("N: "+ Input.location.lastData.latitude);
				logger.PrintMessage("E: " + Input.location.lastData.longitude);
				logger.PrintMessage("Accuracy: " + Input.location.lastData.horizontalAccuracy);
				//logger.PrintMessage("Timestamp: " + Utility.UnixTimeStampToDateTime(Input.location.lastData.timestamp).TimeOfDay);

				overlay.SetActive(CheckLocationForGoals());
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	public void SetGoalRadius(string goalRadius) {
		int newRadius = 0;
		if (int.TryParse(goalRadius, out newRadius)) {
			this.goalRadius = newRadius * 0.00001;
			logger.PrintMessage("New Radius: " + this.goalRadius);
		} else {
			logger.PrintMessage("Could not convert input to int!");
		}
	}

	public void SetGoal() {
		if (Input.location.status == LocationServiceStatus.Running) {
			goals.Add(new Goal(Input.location.lastData.latitude, Input.location.lastData.longitude));
			logger.PrintMessage("New goal added at: N" + Input.location.lastData.latitude + " - E" + Input.location.lastData.longitude);
		}
	}
	
	private bool CheckLocationForGoals() {
		foreach(Goal goal in goals) {
			if (Input.location.lastData.latitude - goal.latitude < goalRadius &&
				Input.location.lastData.longitude - goal.longitude < goalRadius) {
				logger.PrintMessage("Goal found!");
				return true;
				}
		}
		
		return false;
	}
}