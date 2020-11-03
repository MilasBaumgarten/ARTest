using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

class LoggedMessage {
	public string text {get; private set;}
	public float duration {get;  private set;}
	
	public LoggedMessage(string text, float duration) {
		this.text = text;
		this.duration = duration;
	}
	
	public void SubtractTime(float time) {
		duration -= time;
	}
}

public class BuildLogger : MonoBehaviour {

	[SerializeField] private Text debugMessageVisualization;
	private List<LoggedMessage> messages = new List<LoggedMessage>();
	
	protected void Update() {
		string displayedMessage = "";
		
		// remove time from all messages, combine display message
		for (int i = messages.Count - 1; i >= 0; i--) {
			
			if (messages[i].duration < 0) {
				messages.RemoveAt(i);
			} else {
				displayedMessage += messages[i].text + "\n";
				messages[i].SubtractTime(Time.deltaTime);
			}
		}
		
		// visualize
		debugMessageVisualization.text = displayedMessage;
		Debug.Log(displayedMessage);
	}

	public void PrintMessage(string message, float duration) {
		messages.Add(new LoggedMessage(message, duration));
	}
}