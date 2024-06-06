using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playTimer : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI timerText;
	public float timeRemaining = 60f;  // Start with 90 seconds

	public float timeSetting = 60f;

	[SerializeField] private gameSettings gameSettings;
	[SerializeField] private restartPanelController restartGamePanel;
	[SerializeField] private AudioSource _10SecondsLeft;
	[SerializeField] private AudioSource bgMusic;
	[SerializeField] private clockController12 _clockController12;

	[SerializeField] private DamageScreen damageScreen;

	bool is10SecPlay = false;

	bool isGamePlaying = false;

	void Start()
	{
		StartCoroutine(WaitForData());
	}

	IEnumerator WaitForData()
	{
		// Wait until the data is loaded
		while (!gameSettings.isLoaded)
		{
			yield return null;
		}

		var op = gameSettings.Options.options.FirstOrDefault();
		var value = op.values.FirstOrDefault(x => x.title.Equals("Play time"));
		timeSetting = float.Parse(value.value);
		timeRemaining = timeSetting;
		UpdateTimerDisplay();
	}

	// Update is called once per frame
	void Update()
	{
		if(!isGamePlaying) return;

		if (timeRemaining > 0)
		{
			timeRemaining -= Time.deltaTime;  // Decrease the time remaining

			if (timeRemaining < 0) timeRemaining = 0;

			UpdateTimerDisplay();
		}
		else
		{
			EndGame();  // Call the function to end the game
		}

		if(timeRemaining <= 11 && !is10SecPlay)
		{
			is10SecPlay = true;
			_10SecondsLeft.Play();
		}

		if (timeRemaining <= 11)
		{
			if(!damageScreen.isPlaying)
				damageScreen.StartEffect();
		}
	}

	public void StartGame()
	{
		timeRemaining = timeSetting;
		bgMusic.Play();
		isGamePlaying = true;
		is10SecPlay = false;
		_clockController12.StartGame();
	}

	public void ReStartGame()
	{
		timeRemaining = timeSetting;
		bgMusic.Play();
		isGamePlaying = true;
		is10SecPlay = false;
		_clockController12.Restart();
	}

	void UpdateTimerDisplay()
	{
		if (timerText != null)
		{
			timerText.text = Mathf.FloorToInt(timeRemaining).ToString();
		}
	}

	void EndGame()
	{
		// Here you can add whatever you want to happen when the timer runs out
		//SceneManager.LoadScene("EndGame");  // Example: Load an end game scene
		// Alternatively, pause the game or trigger a game over state
		// Time.timeScale = 0;  // Uncomment this to pause the game
		isGamePlaying = false;
		bgMusic.Stop();
		restartGamePanel.EndGame();
	}
}
