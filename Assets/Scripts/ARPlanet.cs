using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPlanet : MonoBehaviour {
	[SerializeField] private GameObject moonPrefab;
	[SerializeField] private float rotationSpeed = 0.2f;
	
	protected void Start() {
		PlanetInfo currentPlanet = PlayerTracker.GetCurrentPlanet();
		
		GetComponent<MeshRenderer>().material = currentPlanet.planetMaterial;
		
		if (currentPlanet.moons > 0) {
			SpawnMoons(currentPlanet.moons);
		}
		
		if (currentPlanet.specialObjectToSpawn) {
			Instantiate(
				currentPlanet.specialObjectToSpawn,
				transform.position,
				transform.rotation,
				transform
			);
		}
		
		PlayerTracker.SetPlanetIsPlaced(true);
	}
	
	private void SpawnMoons(int amount) {
		//Vector3 offsetDirection = new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
		//offsetDirection.Normalize();
		
		//Instantiate(
		//	moonPrefab,
		//	transform.position + offsetDirection * transform.localScale.magnitude + offsetDirection * 1.5f,
		//	transform.rotation,
		//	transform
		//);
	}

	protected void Update() {
		transform.Rotate(new Vector3(0, rotationSpeed, 0));
	}
	
	protected void OnDestroy() {
		PlayerTracker.SetPlanetIsPlaced(false);
	}
	
	protected void OnDisable() {
		PlayerTracker.SetPlanetIsPlaced(false);
	}
}
