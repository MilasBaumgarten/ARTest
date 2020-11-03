using System.Collections;
using UnityEngine;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class LocationService : MonoBehaviour {
	[SerializeField] private BuildLogger logger;

	private void Start() {
		StartCoroutine(Init());
	}

	IEnumerator Init() {
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser) {
			logger.PrintMessage("Location Services are disabled!", 0);
			Utility.AskForPermission(Permission.FineLocation);
			Init();	// retry initialization
		}

		// Start service before querying location
		Input.location.Start();
		logger.PrintMessage("Starting Location Services", 1);

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			logger.PrintMessage("wait for initialization", 1);
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			logger.PrintMessage("Timed out", 10);
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			logger.PrintMessage("Unable to determine device location", 10);
			yield break;
		}
	}
}
