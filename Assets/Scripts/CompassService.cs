using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassService : MonoBehaviour {
    void Start() {
	    Input.compass.enabled = true;
    }

    void Update() {
	    BuildLogger.instance.Debug("Heading: " + Input.compass.trueHeading, 0.0f);
    }
    
}
