using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class clockController : MonoBehaviour
{
	[SerializeField] private TextMeshPro timeText;

	public Transform hourHand;
	public Transform minuteHand;

	// Set the time in HH:MM format
	public string timeString = "14:10";

	void Start()
	{
		SetClockHands(timeString);
	}

	void SetClockHands(string time)
	{
		// Parse the time string
		string[] timeParts = time.Split(':');
		int hours = int.Parse(timeParts[0]);
		int minutes = int.Parse(timeParts[1]);

		// Convert hours to 12-hour format if necessary
		hours = hours % 12;

		// Calculate the angles
		float hourAngle = (hours * 30) + (minutes * 0.5f);
		float minuteAngle = minutes * 6;

		// Apply the rotations
		hourHand.localRotation = Quaternion.Euler(0, 0, -hourAngle);
		minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteAngle);

		timeText.text = time;
	}
}
