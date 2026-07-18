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

    public void SetupRotation(
        ControlpadInputValue clockwiseInputType,
        ControlpadInputSign clockwiseSign,
        ControlpadInputValue antiClockwiseInputType,
        ControlpadInputSign antiClockwiseSign)
    {
        m_rotateClockwise = clockwiseInputType;
        m_rotateClockwiseSign = clockwiseSign;
        m_rotateAntiClockwise = antiClockwiseInputType;
        m_rotateAntiClockwiseSign = antiClockwiseSign;
        if (clockwiseInputType == antiClockwiseInputType && clockwiseSign == antiClockwiseSign)
        {
            Debug.LogError("SetupRotation- Setting up control pad to rotate both directions on same input.  Likely user error in SetupRotation call.");
        }
    }

    public void SetupDepthMovement(
        ControlpadInputValue forwardInputType,
        ControlpadInputSign forwardSign,
        ControlpadInputValue backwardInputType,
        ControlpadInputSign backwardSign)
    {
        m_depthForward = forwardInputType;
        m_depthForwardSign = forwardSign;
        m_depthBackward = backwardInputType;
        m_depthBackwardSign = backwardSign;

        if (forwardInputType == backwardInputType && forwardSign == backwardSign)
        {
            Debug.LogError("SetupDepthMovement- Setting up control pad to move depth both directions on same input.  Likely user error in SetupDepthMovement call.");
        }
    }

    public void SetupPositionMovement(
        ControlpadInputValue positionUp,
        ControlpadInputSign positionUpSign,
        ControlpadInputValue positionDown,
        ControlpadInputSign positionDownSign,
        ControlpadInputValue positionRight,
        ControlpadInputSign positionRightSign,
        ControlpadInputValue positionLeft,
        ControlpadInputSign positionLeftSign)
    {
        m_translationUp = positionUp;
        m_translationUpSign = positionUpSign;
        m_translationDown = positionDown;
        m_translationDownSign = positionDownSign;
        m_translationRight = positionRight;
        m_translationRightSign = positionRightSign;
        m_translationLeft = positionLeft;
        m_translationLeftSign = positionLeftSign;
    }

    public void SetupSpeeds(
        ControlpadAimingSpeed rotationSpeed,
        ControlpadAimingSpeed depthSpeed,
        ControlpadAimingSpeed positionTranslationSpeed)
    {
        m_rotationSpeed = rotationSpeed;
        m_depthSpeed = depthSpeed;
        m_translationSpeed = positionTranslationSpeed;
    }
}