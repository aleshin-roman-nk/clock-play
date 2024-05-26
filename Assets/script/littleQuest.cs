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

	// Start is called before the first frame update
	void Start()
	{
		pressGesture = GetComponent<PressGesture>();

		if (pressGesture != null)
			pressGesture.Pressed += PressGesture_Pressed;
	}

	private void PressGesture_Pressed(object sender, System.EventArgs e)
	{
		clockController12.touchTime(text.text);
	}
}
