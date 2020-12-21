using System.Collections;
using UnityEngine;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class LocationService : MonoBehaviour {

	private void Start() {
		StartCoroutine(Init());
	}

	IEnumerator Init() {
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser) {
			Debug.Log("Location Services are disabled!");
			Utility.AskForPermission(Permission.FineLocation);
			Init();	// retry initialization
		}

		// Start service before querying location
		Input.location.Start();
		Debug.Log("Starting Location Services");

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			Debug.Log("wait for initialization");
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			Debug.LogWarning("Timed out");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			Debug.LogWarning("Unable to determine device location");
			yield break;
		}
	}
}
