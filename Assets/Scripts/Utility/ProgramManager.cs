using UnityEngine;

public class ProgramManager : MonoBehaviour {
	public static ProgramManager instance;

	private void Start() {
		if (instance) {
			GameObject.Destroy(gameObject);
		} else {
			instance = this;
		}
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
}