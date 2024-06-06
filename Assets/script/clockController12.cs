
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class clockController12 : MonoBehaviour
{
	public Transform hourHand;
	public Transform minuteHand;
	public Transform secondHand;

	public Transform answerPoint;

	public littleQuest[] questTexts;

	public LightManager lightManager;

	public headApproval headApprovalA;

	public randomListPlayer randomListPlayerYes;
	public randomListPlayer randomListPlayerNop;

	public randomGameObjectPlayer yesEffectObjects;
	public randomGameObjectPlayer noEffectObjects;

	[SerializeField] private TextMeshProUGUI tryTimes;

	private string rightTime = "";

	// For a button to delay clicking while robot reaction is being animated
	//public bool allowClickSolution => !headApprovalA.isPlaying;
	public bool allowClickSolution {  get; private set; } = true;

	// day animating
	public int currentValue = 900;  // Starting value
	public int targetValue = 800;   // Target value
	public float duration = 2f;     // Duration of the animation in seconds

	private float elapsedTime = 0f; // Track the elapsed time
	private int startValue;         // Starting value at the beginning of the animation
	private bool isAnimating = false; // Is the animation currently running
									  // day animating

	public bool isBusy = false;

	private int currentTry = 3;
	private int tryCapacity = 3;

	private IEnumerator disableButtonsForSeconds(float sec)
	{
		allowClickSolution = false;

		yield return new WaitForSeconds(sec);

		allowClickSolution = true;
	}

	public string Next()
	{
		// Generate a random time in AM/PM format
		string randomTime  = "00:00 PM";

		for (int i = 0; i < 4; i++)
		{
			randomTime = GenerateRandomTime();

			questTexts[i].text.text = randomTime;
		}

		return questTexts[Random.Range(0, questTexts.Length)].text.text;
	}

	private void setFirstDayTime(string t)
	{
		SetClockHands(t);

		currentValue = ConvertTimeToMinutes(t);

		isAnimating = false;
		lightManager.SetTimeOfDay(currentValue);

		elapsedTime = 0f;
	}

	public void touchTime(string time)
	{
		//Debug.Log(headApprovalA.isPlaying);

		StartCoroutine(disableButtonsForSeconds(1.2f));

		if (isAnimating) return;
		if (headApprovalA.isPlaying) return;

		bool goNext = false;

		if (rightTime.Equals(time))
		{
			headApprovalA.Yes();
			if(randomListPlayerYes != null)
				randomListPlayerYes.Play();

			goNext = true;

			showRight(true);
		}
		else
		{
			headApprovalA.No();
			if (randomListPlayerNop != null)
				randomListPlayerNop.Play();

			currentTry--;

			tryTimes.text = currentTry.ToString();

			if (currentTry == 0) goNext = true;

			showRight(false);
			//showRight(true);
		}

		if (goNext)
		{
			isBusy = true;

			currentTry = tryCapacity;
			tryTimes.text = currentTry.ToString();

			rightTime = Next();

			var newTarget = ConvertTimeToMinutes(rightTime);

			if(currentValue > newTarget)
				wayLenght = 1440 - currentValue + newTarget;
			else
				wayLenght = newTarget - currentValue;

			AnimateTo(newTarget);
		}
	}

	public void StartGame()
	{

	}

	public void Restart()
	{
		isBusy = true;

		currentTry = tryCapacity;
		tryTimes.text = currentTry.ToString();

		rightTime = Next();
		//setFirstDayTime(rightTime);

		var newTarget = ConvertTimeToMinutes(rightTime);

		if (currentValue > newTarget)
			wayLenght = 1440 - currentValue + newTarget;
		else
			wayLenght = newTarget - currentValue;

		AnimateTo(newTarget);
	}

	private void showRight(bool isTrue)
	{
		if (isTrue)
		{
			yesEffectObjects.Play();
		}
		else
		{
			noEffectObjects.Play();
		}
		
	}

	void Start()
	{
		yesEffectObjects.wherePoint = answerPoint;
		noEffectObjects.wherePoint = answerPoint;

		tryTimes.text = currentTry.ToString();
		rightTime = Next();
		setFirstDayTime(rightTime);
	}

	private int wayLenght = 0;
	private void Update()
	{
		if (isAnimating)
		{
			// Increment elapsed time by the time passed since last frame
			elapsedTime += Time.deltaTime;

			// Calculate the interpolation factor (0 to 1)
			float t = Mathf.Clamp01(elapsedTime / duration);
			// Calculate the new value
			int newValue = Mathf.RoundToInt(Mathf.Lerp(0, wayLenght, t));

			// Adjust for the circular range
			currentValue = (startValue + newValue) % 1440;

			// Check if the animation is complete
			if (t >= 1f)
			{
				currentValue = targetValue;
				isAnimating = false;
				isBusy = false;

				lightManager.SetTimeOfDay(currentValue);
				SetClockHands(currentValue);
			}
			else
			{
				lightManager.SetTimeOfDay(currentValue);
				SetClockHands(currentValue);
			}
		}

		const float secondSpeed = 6f;
		var rotation = Time.deltaTime * secondSpeed;
		secondHand.transform.Rotate(0, 0, rotation);
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

	void SetClockHands(int timeMinutes)
	{
		// Normalize minutes to be within a 24-hour period
		timeMinutes = timeMinutes % 1440;

		// Calculate hours and remaining minutes
		int hours = timeMinutes / 60;
		int remainingMinutes = timeMinutes % 60;

		// Calculate the minute hand angle
		float minuteAngle = remainingMinutes * 6f;

		// Calculate the hour hand angle
		float hourAngle = (hours * 30f) + (remainingMinutes * 0.5f);


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

	// Method to start the animation to a new target value
	public void AnimateTo(int newTargetValue)
	{
		if (newTargetValue != currentValue)
		{
			targetValue = newTargetValue;
			startValue = currentValue;
			elapsedTime = 0f;
			isAnimating = true;
		}
	}
}
