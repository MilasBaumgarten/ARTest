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
			BuildLogger.instance.SetInfo("Location Services are disabled!");
			Utility.AskForPermission(Permission.FineLocation);
			Init();	// retry initialization
		}

		// Start service before querying location
		Input.location.Start();
		BuildLogger.instance.SetInfo("Starting Location Services");

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			BuildLogger.instance.SetInfo("wait for initialization");
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			BuildLogger.instance.SetInfo("Timed out");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			BuildLogger.instance.SetInfo("Unable to determine device location");
			yield break;
		} else if (Input.location.status == LocationServiceStatus.Running) {
			BuildLogger.instance.SetInfo("");
		}
	}
}
