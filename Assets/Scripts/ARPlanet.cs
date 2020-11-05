using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPlanet : MonoBehaviour {
	protected void Start() {
		PlanetInfo currentPlanet = PlayerTracker.GetCurrentPlanet();
		
		GetComponent<MeshRenderer>().material = currentPlanet.planetMaterial;
		
		PlayerTracker.SetPlanetIsPlaced(true);
	}
	
	protected void OnDestroy() {
		PlayerTracker.SetPlanetIsPlaced(false);
	}
	
	protected void OnDisable() {
		PlayerTracker.SetPlanetIsPlaced(false);
	}
}
