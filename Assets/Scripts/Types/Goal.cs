using System;

[Serializable]
public struct Goal {
	public Goal(float latitude, float longitude) {
		this.latitude = latitude;
		this.longitude = longitude;
	}

	public float latitude;
	public float longitude;
}