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
	
	public static BuildLogger instance { get; private set; }

	[SerializeField] private Text debugMessageVisualization;
	[SerializeField] private Text infoText;
	[SerializeField] private Text planetNameText;
	private List<LoggedMessage> messages = new List<LoggedMessage>();

	protected void Awake() {
		if (instance) {
			Destroy(this);
		} else {
			instance = this;
		}
	}
	
	protected void LateUpdate() {
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
		UnityEngine.Debug.Log(displayedMessage);
	}

	public void Debug(string message, float duration) {
		messages.Add(new LoggedMessage(message, duration));
	}
	
	public void SetInfo(string message) {
		infoText.text = message;
	}

	public void SetPlanetName(string name) {
		planetNameText.text = name;
	}
}