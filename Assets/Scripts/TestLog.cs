using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
	    StartCoroutine(LongerMessage());
	    StartCoroutine(EvenLongerMessage());
    }

    // Update is called once per frame
    void Update()
    {
	    BuildLogger.instance.Debug("Debug Message 1", 0.0f);
	    BuildLogger.instance.Debug("Debug Message 2", 0.0f);
    }
    
	IEnumerator LongerMessage() {
		while(true){
			BuildLogger.instance.Debug("Longer Message", 0.2f);
			yield return new WaitForSeconds(0.2f);
		}
	}
	
	IEnumerator EvenLongerMessage() {
		while(true){
			BuildLogger.instance.Debug("Even Longer Message\nThis one also goes over multiple rows\nThis is the last row.", 0.5f);
			yield return new WaitForSeconds(0.5f);
		}
	}
}
