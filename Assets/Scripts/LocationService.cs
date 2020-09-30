using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class LocationService : MonoBehaviour {
	[SerializeField] private Text debugMessageVisualization;
	[SerializeField] private Camera playerCamera;

	private void Start() {
		StartCoroutine(Init());
	}

	private void Update() {
		// Make sure user is on Android platform
		if (Application.platform == RuntimePlatform.Android) {
			
			// Check if Back was pressed this frame
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Input.location.Stop();
				Application.Quit();
			}
		}
	}

	IEnumerator Init() {
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser) {
			PrintMessage("Location Services are disabled!");
			AskForPermission(Permission.FineLocation);
			Init();	// retry initialization
		}

		// Start service before querying location
		Input.location.Start();
		PrintMessage("Starting Location Services");

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			PrintMessage("wait for initialization");
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			PrintMessage("Timed out");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			PrintMessage("Unable to determine device location");
			yield break;
		}
		else {
			StartCoroutine(LocationUpdate());
		}
	}

	IEnumerator LocationUpdate() {
		while(true){
			ClearLog();
			if (Input.location.status == LocationServiceStatus.Running) {
				PrintMessage("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
				PrintMessage("self: " + playerCamera.transform.position + " " + playerCamera.transform.rotation);
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	void AskForPermission(string permission) {
		#if PLATFORM_ANDROID
		Permission.RequestUserPermission(permission);
		#endif
	}

	void ClearLog() {
		debugMessageVisualization.text = "";
	}

	void PrintMessage(string message) {
		string currentLog = debugMessageVisualization.text;
		currentLog += "\n" + message;
		debugMessageVisualization.text = currentLog;
		Debug.Log(message);
	}
}
