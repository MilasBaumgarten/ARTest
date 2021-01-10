using UnityEngine;

public class ARPlanet : MonoBehaviour {
	[SerializeField] private GameObject moonPrefab;
	[SerializeField] private float rotationSpeed = 0.2f;
	
	protected void Start() {
		PlanetInfo currentPlanet = PlayerTracker.instance.currentPlanet;
		
		GetComponent<MeshRenderer>().material = currentPlanet.planetMaterial;
		
		if (currentPlanet.specialObjectToSpawn) {
			Instantiate(
				currentPlanet.specialObjectToSpawn,
				transform.position,
				transform.rotation,
				transform
			);
		}
	}

	protected void Update() {
		transform.Rotate(new Vector3(0, rotationSpeed, 0));
	}
}
