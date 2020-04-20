using System;
using UnityEngine;

public class ControlpadAimingConfig
{
	public ControlpadInputValue m_rotateClockwise;

	public ControlpadInputSign m_rotateClockwiseSign;

	public ControlpadInputValue m_rotateAntiClockwise;

	public ControlpadInputSign m_rotateAntiClockwiseSign;

	public ControlpadAimingSpeed m_rotationSpeed;

	public ControlpadInputValue m_depthForward;

	public ControlpadInputSign m_depthForwardSign;

	public ControlpadInputValue m_depthBackward;

	public ControlpadInputSign m_depthBackwardSign;

	public ControlpadAimingSpeed m_depthSpeed;

	public ControlpadInputValue m_translationUp;

	public ControlpadInputSign m_translationUpSign;

	public ControlpadInputValue m_translationDown;

	public ControlpadInputSign m_translationDownSign;

	public ControlpadInputValue m_translationRight;

	public ControlpadInputSign m_translationRightSign;

	public ControlpadInputValue m_translationLeft;

	public ControlpadInputSign m_translationLeftSign;

	public ControlpadAimingSpeed m_translationSpeed;

	public void SetupRotation(ControlpadInputValue clockwiseInputType, ControlpadInputSign clockwiseSign, ControlpadInputValue antiClockwiseInputType, ControlpadInputSign antiClockwiseSign)
	{
		this.m_rotateClockwise = clockwiseInputType;
		this.m_rotateClockwiseSign = clockwiseSign;
		this.m_rotateAntiClockwise = antiClockwiseInputType;
		this.m_rotateAntiClockwiseSign = antiClockwiseSign;
		if (clockwiseInputType == antiClockwiseInputType)
		{
			if (clockwiseSign == antiClockwiseSign)
			{
				Debug.LogError("SetupRotation- Setting up control pad to rotate both directions on same input.  Likely user error in SetupRotation call.");
			}
		}
	}

	public void SetupDepthMovement(ControlpadInputValue forwardInputType, ControlpadInputSign forwardSign, ControlpadInputValue backwardInputType, ControlpadInputSign backwardSign)
	{
		this.m_depthForward = forwardInputType;
		this.m_depthForwardSign = forwardSign;
		this.m_depthBackward = backwardInputType;
		this.m_depthBackwardSign = backwardSign;
		if (forwardInputType == backwardInputType)
		{
			if (forwardSign == backwardSign)
			{
				Debug.LogError("SetupDepthMovement- Setting up control pad to move depth both directions on same input.  Likely user error in SetupDepthMovement call.");
			}
		}
	}

	public void SetupPositionMovement(ControlpadInputValue positionUp, ControlpadInputSign positionUpSign, ControlpadInputValue positionDown, ControlpadInputSign positionDownSign, ControlpadInputValue positionRight, ControlpadInputSign positionRightSign, ControlpadInputValue positionLeft, ControlpadInputSign positionLeftSign)
	{
		this.m_translationUp = positionUp;
		this.m_translationUpSign = positionUpSign;
		this.m_translationDown = positionDown;
		this.m_translationDownSign = positionDownSign;
		this.m_translationRight = positionRight;
		this.m_translationRightSign = positionRightSign;
		this.m_translationLeft = positionLeft;
		this.m_translationLeftSign = positionLeftSign;
	}

	public void SetupSpeeds(ControlpadAimingSpeed rotationSpeed, ControlpadAimingSpeed depthSpeed, ControlpadAimingSpeed positionTranslationSpeed)
	{
		this.m_rotationSpeed = rotationSpeed;
		this.m_depthSpeed = depthSpeed;
		this.m_translationSpeed = positionTranslationSpeed;
	}
}
