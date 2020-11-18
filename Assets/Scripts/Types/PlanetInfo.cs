using UnityEngine;
using System;

[Serializable]
public struct PlanetInfo {
	public string name;
	[TextArea]
	public string information;
	[Space(10)]
	public float latitude;
	public float longitude;
	[Space(10)]
	public Material planetMaterial;
	public int moons;
	public GameObject specialObjectToSpawn;
	
	public PlanetInfo(string name, string information, float latitude, float longitude, Material planetMaterial) {
		this.name = name;
		this.information = information;
		this.latitude = latitude;
		this.longitude = longitude;
		this.planetMaterial = planetMaterial;
		this.moons = 0;
		this.specialObjectToSpawn = null;
	}
}
