using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARService: MonoBehaviour {
	[SerializeField] private BuildLogger logger;
	
	private void OnEnable() {
		ARSession.stateChanged += OnStateChanged;
	}
	
	private void OnDisable() {
		ARSession.stateChanged -= OnStateChanged;
	}
	
	private void OnStateChanged(ARSessionStateChangedEventArgs args) {
		switch(args.state) {
			case ARSessionState.None:
				logger.PrintMessage("None", 2);
				break;
			case ARSessionState.CheckingAvailability:
				logger.PrintMessage("CheckingAvailability", 2);
				break;
			case ARSessionState.Installing:
				logger.PrintMessage("Installing", 2);
				break;
			case ARSessionState.NeedsInstall:
				logger.PrintMessage("NeedsInstall", 2);
				break;
			case ARSessionState.Ready:
				logger.PrintMessage("Ready", 2);
				break;
			case ARSessionState.SessionInitializing:
				logger.PrintMessage("SessionInitializing", 2);
				break;
			case ARSessionState.SessionTracking:
				logger.PrintMessage("SessionTracking", 2);
				break;
			case ARSessionState.Unsupported:
				logger.PrintMessage("Unsupported", 2);
				break;
		}
	}
}