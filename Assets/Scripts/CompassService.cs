using UnityEngine;

public class CompassService : MonoBehaviour {
    void Start() {
	    Input.compass.enabled = true;
    }

    void Update() {
        Debug.Log("Heading: " + Input.compass.trueHeading);
    }
}
