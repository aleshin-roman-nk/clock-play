using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startGamePanelController : MonoBehaviour
{
	public translateGesturePress _clickStart;

	public playTimer _playTimer;

	private Animator _goPlayanimator;
	private AudioSource _startingAudio;

	[SerializeField] private GameObject[] objectsToDisappear;

	// Start is called before the first frame update
	void Start()
	{
		_clickStart.onClick += _clickStart_onClick;
		_startingAudio = GetComponent<AudioSource>();
		_goPlayanimator = GetComponent<Animator>();
	}

	private void _clickStart_onClick(object sender, System.EventArgs e)
	{
		StartCoroutine(playStarting());
	}

	float Delay = 1.5f;
	IEnumerator playStarting()
	{
		foreach (var obj in objectsToDisappear)
		{
			obj.SetActive(false);
		}

		_startingAudio.Play();
		_goPlayanimator.SetTrigger("start");

		if (Delay != 0) yield return new WaitForSeconds(Delay);

		_playTimer.StartGame();
		gameObject.SetActive(false);
	}
}
