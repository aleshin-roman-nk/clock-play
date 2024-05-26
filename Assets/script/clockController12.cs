
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clockController12 : MonoBehaviour
{
	public Transform hourHand;
	public Transform minuteHand;

	public Transform answerPoint;

	public GameObject rightAnswer;
	public GameObject wrongAnswer;

	public littleQuest[] questTexts;

	public LightManager lightManager;

	public headApproval headApprovalA;

	private string rightTime = "";

	public void Next()
	{
		// Generate a random time in AM/PM format
		string randomTime  = "00:00 PM";

		for (int i = 0; i < 4; i++)
		{
			randomTime = GenerateRandomTime();

			questTexts[i].text.text = randomTime;
		}

		rightTime = questTexts[Random.Range(0, questTexts.Length)].text.text;

		// Set the clock hands to the generated random time
		SetClockHands(rightTime);

		lightManager.SetTimeOfDay(ConvertTimeToMinutes(rightTime));
	}

	public void touchTime(string time)
	{
		if (rightTime.Equals(time))
		{
			headApprovalA.Yes();
			//showRight(true);
			Next();
		}
		else
		{
			headApprovalA.No();
			//showRight(false);
			Next();
		}
	}

	private void showRight(bool isTrue)
	{
		if (isTrue)
		{
			Instantiate(rightAnswer, answerPoint.position, Quaternion.identity);
		}
		else
		{
			Instantiate(wrongAnswer, answerPoint.position, Quaternion.identity);
		}
		
	}

	void Start()
	{
		Next();
	}

	void SetClockHands(string time)
	{
		// Parse the time string
		string[] timeParts = time.Split(' ');
		string[] hourMinuteParts = timeParts[0].Split(':');

		int hours = int.Parse(hourMinuteParts[0]);
		int minutes = int.Parse(hourMinuteParts[1]);
		string amPm = timeParts[1];

		// Convert to 24-hour format
		if (amPm == "PM" && hours != 12)
		{
			hours += 12;
		}
		if (amPm == "AM" && hours == 12)
		{
			hours = 0;
		}

		// Convert hours to 12-hour format if necessary
		hours = hours % 12;

		// Calculate the angles
		float hourAngle = (hours * 30) + (minutes * 0.5f);
		float minuteAngle = minutes * 6;

		// Apply the rotations
		//hourHand.localRotation = Quaternion.Euler(0, 0, -hourAngle);
		//minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteAngle);
		hourHand.localRotation = Quaternion.Euler(0, 0, hourAngle);
		minuteHand.localRotation = Quaternion.Euler(0, 0, minuteAngle);
	}

	string GenerateRandomTime()
	{
		// Generate a random hour between 1 and 12
		int randomHour = Random.Range(1, 13);

		// Generate a random minute between 0 and 59
		//int randomMinute = Random.Range(0, 60);

		// Generate a random minute that is a multiple of 5 between 0 and 55
		int randomMinute = Random.Range(0, 12) * 5;

		// Generate AM or PM randomly
		string amPm = Random.value > 0.5f ? "AM" : "PM";

		// Format the time string
		string randomTime = string.Format("{0:00}:{1:00} {2}", randomHour, randomMinute, amPm);

		return randomTime;
	}

	int ConvertTimeToMinutes(string time)
	{
		// Parse the time string
		string[] timeParts = time.Split(' ');
		string[] hourMinuteParts = timeParts[0].Split(':');

		int hours = int.Parse(hourMinuteParts[0]);
		int minutes = int.Parse(hourMinuteParts[1]);
		string amPm = timeParts[1];

		// Convert to 24-hour format
		if (amPm == "PM" && hours != 12)
		{
			hours += 12;
		}
		if (amPm == "AM" && hours == 12)
		{
			hours = 0;
		}

		// Calculate the total minutes from midnight
		int totalMinutes = (hours * 60) + minutes;
		return totalMinutes;
	}
}
