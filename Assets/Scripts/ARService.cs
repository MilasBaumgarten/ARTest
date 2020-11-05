using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARService: MonoBehaviour {
	
	private void OnEnable() {
		ARSession.stateChanged += OnStateChanged;
	}
	
	private void OnDisable() {
		ARSession.stateChanged -= OnStateChanged;
	}
	
	private void OnStateChanged(ARSessionStateChangedEventArgs args) {
		switch(args.state) {
			case ARSessionState.None:
				BuildLogger.instance.Debug("None", 2);
				break;
			case ARSessionState.CheckingAvailability:
				BuildLogger.instance.Debug("CheckingAvailability", 2);
				break;
			case ARSessionState.Installing:
				BuildLogger.instance.Debug("Installing", 2);
				break;
			case ARSessionState.NeedsInstall:
				BuildLogger.instance.Debug("NeedsInstall", 2);
				break;
			case ARSessionState.Ready:
				BuildLogger.instance.Debug("Ready", 2);
				break;
			case ARSessionState.SessionInitializing:
				BuildLogger.instance.Debug("SessionInitializing", 2);
				break;
			case ARSessionState.SessionTracking:
				BuildLogger.instance.Debug("SessionTracking", 2);
				break;
			case ARSessionState.Unsupported:
				BuildLogger.instance.Debug("Unsupported", 2);
				break;
		}
	}
}