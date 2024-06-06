using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class restartPanelController : MonoBehaviour
{
	public translateGesturePress _click;
	public playTimer _playTimer;

	private Animator _animator;
	public AudioSource _startingAudio;
	public AudioSource _endedAudio;

	Image image;

	[SerializeField] private GameObject[] objectsToDisappear;
	// Start is called before the first frame update
	void Start()
	{
		_animator = GetComponent<Animator>();

		_click.onClick += _click_onClick;
		image = gameObject.GetComponent<Image>();
		//StartCoroutine(EnsureComponentsReady());
	}

	private IEnumerator EnsureComponentsReady()
	{
		yield return null; // Wait for one frame

		_animator = GetComponent<Animator>();

		if (_animator != null)
		{
			foreach (var obj in objectsToDisappear)
			{
				obj.SetActive(false);
			}
			StartCoroutine(playFading());
		}
	}

	// restart
	private void _click_onClick(object sender, System.EventArgs e)
	{
		_startingAudio.Play();
		//StartCoroutine(playStarting());
		StartCoroutine(FadeOut());
	}

	public void EndGame()
	{
		foreach (var obj in objectsToDisappear)
		{
			obj.SetActive(false);
		}

		gameObject.SetActive(true);
		_endedAudio.Play();
		StartCoroutine(FadeIn());
	}

	float Delay = 1.5f;
	IEnumerator playStarting()
	{
		foreach (var obj in objectsToDisappear)
		{
			obj.SetActive(false);
		}

		_startingAudio.Play();
		_animator.SetTrigger("start");

		if (Delay != 0) yield return new WaitForSeconds(Delay);

		_playTimer.ReStartGame();
		gameObject.SetActive(false);
	}

	//float Delay = 1.5f;
	IEnumerator playFading()
	{
		_endedAudio.Play();
		_animator.ResetTrigger("end");
		_animator.SetTrigger("end");

		if (Delay != 0) yield return new WaitForSeconds(Delay);

		foreach (var obj in objectsToDisappear)
		{
			obj.SetActive(true);
		}
	}

	float duration = 1.5f;
	private IEnumerator FadeIn()
	{
		yield return null; // Wait for one frame

		image = GetComponent<Image>();

		float startAlpha = 0f; // Starting alpha value
		float endAlpha = 254f / 255f; // Ending alpha value (normalized between 0 and 1)
		float elapsedTime = 0f;

		// Get the current color of the image
		Color color = image.color;
		color.a = startAlpha; // Set initial alpha to startAlpha
		image.color = color;

		// Gradually change the alpha value over time
		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
			image.color = color;
			yield return null;
		}

		// Ensure the alpha is set to the end value after the loop
		color.a = endAlpha;
		image.color = color;

		foreach (var obj in objectsToDisappear)
		{
			obj.SetActive(true);
		}
	}

	float durationOut = 1.5f;
	private IEnumerator FadeOut()
	{
		foreach (var obj in objectsToDisappear)
		{
			obj.SetActive(false);
		}

		float endAlpha = 0f; // Starting alpha value
		float startAlpha = 254f / 255f; // Ending alpha value (normalized between 0 and 1)
		float elapsedTime = 0f;

		// Get the current color of the image
		Color color = image.color;
		color.a = startAlpha; // Set initial alpha to startAlpha
		image.color = color;

		// Gradually change the alpha value over time
		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / durationOut);
			image.color = color;
			yield return null;
		}

		// Ensure the alpha is set to the end value after the loop
		color.a = endAlpha;
		image.color = color;

		_playTimer.ReStartGame();
		gameObject.SetActive(false);
	}
}
