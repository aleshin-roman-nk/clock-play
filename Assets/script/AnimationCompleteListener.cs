using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCompleteListener : StateMachineBehaviour
{
	public bool isPlaying { get; private set; }

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// You can add code here if needed when the state starts
		isPlaying = false;
		//Debug.Log("animation stopped");
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// You can add code here if needed while the state is being evaluated
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// This is where you handle the end of the animation
		isPlaying = true;
		// Perform any other actions you need here

		//Debug.Log("animation started");
	}
}
