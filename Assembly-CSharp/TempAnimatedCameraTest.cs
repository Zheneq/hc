﻿using System;
using UnityEngine;

public class TempAnimatedCameraTest : MonoBehaviour
{
	[Header("-- Specify object to play animation on. Press C to play again")]
	public GameObject m_parentFbx;

	[Header("-- Joint for camera to attach to")]
	public GameObject m_cameraJoint;

	[Header("-- Animation trigger param. (set Can Transition To Self on transition to be repeatable)")]
	public string m_animStartTrigger = "PlayTestAnim";

	private Animator m_animator;

	private bool m_updatingWithAnim;

	private void Start()
	{
		if (this.m_parentFbx != null)
		{
			this.m_animator = this.m_parentFbx.GetComponent<Animator>();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.C) && this.m_animator != null)
		{
			this.m_animator.SetTrigger(this.m_animStartTrigger);
			this.m_updatingWithAnim = true;
		}
	}

	private void LateUpdate()
	{
		if (this.m_cameraJoint != null)
		{
			if (this.m_updatingWithAnim)
			{
				Camera.main.transform.position = this.m_cameraJoint.transform.position;
				Camera.main.transform.rotation = this.m_cameraJoint.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
				Camera.main.fieldOfView = this.GetFieldOfView();
			}
		}
	}

	private float GetFieldOfView()
	{
		float result = Camera.main.fieldOfView;
		if (this.m_cameraJoint != null)
		{
			if (this.m_cameraJoint.transform.localScale.z > 1f)
			{
				result = this.m_cameraJoint.transform.localScale.z;
			}
		}
		return result;
	}
}
