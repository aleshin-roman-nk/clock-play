using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headApproval : MonoBehaviour
{
	private Animator mAnimator;

	private AnimationCompleteListener completeListener;

	public bool isPlaying
	{
		get
		{
			if(completeListener != null) return completeListener.isPlaying;
			return false;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		mAnimator = GetComponent<Animator>();

		completeListener = mAnimator.GetBehaviour<AnimationCompleteListener>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void No()
	{
		mAnimator.ResetTrigger("no");
		mAnimator.SetTrigger("no");

	}

	public void Yes()
	{
		mAnimator.ResetTrigger("yes");
		mAnimator.SetTrigger("yes");
	}
}
