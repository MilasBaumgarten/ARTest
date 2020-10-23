using UnityEngine;
using UnityEngine.UI;

public class BuildLogger : MonoBehaviour {

	[SerializeField] private Text debugMessageVisualization;

	public void ClearLog() {
		debugMessageVisualization.text = "";
	}

	public void PrintMessage(string message) {
		string currentLog = debugMessageVisualization.text;
		currentLog += "\n" + message;
		debugMessageVisualization.text = currentLog;
		Debug.Log(message);
	}
}