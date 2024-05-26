using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headApproval : MonoBehaviour
{
	private Animator mAnimator;

	// Start is called before the first frame update
	void Start()
	{
		mAnimator = GetComponent<Animator>();
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
