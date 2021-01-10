using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneTracker : MonoBehaviour {
	[SerializeField] private ARPlaneManager manager;

	public static Vector3 placeableLocation { get; private set; }

	void Update() {
		if (PlayerTracker.instance.CurrentPlanetState != PlanetState.FOUND) {
			return;
		}

		ARPlane largestPlane = null;

		foreach (ARPlane plane in manager.trackables) {
			float size = 0;

			// check for tracking quality
			if (plane.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) {
				float planeExtends = plane.extents.x * plane.extents.y;
				if (planeExtends > size) {
					size = planeExtends;
					largestPlane = plane;
				}
			}
		}

		if (largestPlane) {
			PlayerTracker.instance.CurrentPlanetState = PlanetState.PLACEABLE;
			placeableLocation = largestPlane.center;
		}
	}
}
