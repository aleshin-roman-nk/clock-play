using System.Collections;
using System.Collections.Generic;
using TMPro;
using TouchScript.Gestures;
using UnityEngine;

public class littleQuest : MonoBehaviour
{
	public TextMeshProUGUI text;
	public clockController12 clockController12;
	private PressGesture pressGesture;

	public GameObject clickEffect;

	//private Animator animator;

	//private bool allowClickSolution { get; set; } = true;

	//private IEnumerator disableButtonsForSeconds(float sec)
	//{
	//	allowClickSolution = false;

	//	yield return new WaitForSeconds(sec);

	//	allowClickSolution = true;
	//}

	// Start is called before the first frame update
	void Start()
	{
		//animator = GetComponent<Animator>();

		pressGesture = GetComponent<PressGesture>();
		if (pressGesture != null)
			pressGesture.Pressed += PressGesture_Pressed;
	}

	private void PressGesture_Pressed(object sender, System.EventArgs e)
	{
		if (clockController12.isBusy) return;
		if (!clockController12.allowClickSolution) return;
		//if (!allowClickSolution) return;

		Instantiate(clickEffect, transform.position, Quaternion.identity);

		//animator.ResetTrigger("go");
		//animator.SetTrigger("go");

		clockController12.touchTime(text.text);

		//StartCoroutine(disableButtonsForSeconds(1.2f));
	}
}
