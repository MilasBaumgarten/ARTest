using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActiveState : MonoBehaviour {
	[SerializeField] private MonoBehaviour target;
    
	public void Switch() {
		target.enabled = !target.enabled;
	}
}
