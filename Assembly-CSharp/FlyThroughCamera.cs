using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlyThroughCamera : MonoBehaviour
{
	public GameObject m_cameraBonePrefab;

	private GameObject m_cameraBoneInstance;

	private Transform m_cameraBoneTransform;

	private bool m_needToHideLoadingScreen;

	private bool m_fadeObjectsToRevealCharacters = true;

	private void OnEnable()
	{
		bool fadeObjectsToRevealCharacters;
		if (!(GameManager.Get() == null))
		{
			if (GameManager.Get().GameConfig != null)
			{
				fadeObjectsToRevealCharacters = (GameManager.Get().GameConfig.GameType != GameType.Tutorial);
				goto IL_51;
			}
		}
		fadeObjectsToRevealCharacters = true;
		IL_51:
		this.m_fadeObjectsToRevealCharacters = fadeObjectsToRevealCharacters;
	}

	private void OnDisable()
	{
	}

	public void StopFlyThroughAnimation()
	{
		UnityEngine.Object.Destroy(this.m_cameraBoneInstance);
		this.m_cameraBoneInstance = null;
		this.m_cameraBoneTransform = null;
		if (!this.m_fadeObjectsToRevealCharacters)
		{
			FadeObjectsCameraComponent component = base.GetComponent<FadeObjectsCameraComponent>();
			if (component != null)
			{
				component.enabled = true;
			}
		}
	}

	public bool HasAnimatorControllerParamater(Animator animator, string paramName)
	{
		bool result = false;
		AnimatorControllerParameter[] parameters = animator.parameters;
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].name == paramName)
			{
				result = true;
				return result;
			}
		}
		return result;
	}

	public void StartFlyThroughAnimation()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (this.m_cameraBonePrefab != null)
		{
			this.m_cameraBoneInstance = UnityEngine.Object.Instantiate<GameObject>(this.m_cameraBonePrefab);
			if (activeOwnedActorData != null)
			{
				this.m_cameraBoneInstance.transform.position = activeOwnedActorData.transform.position;
				Quaternion camRotation = Quaternion.Euler(new Vector3(0f, CameraManager.Get().GetIsometricCamera().GetInitialYAngle()));
				Vector3 b = CameraManager.Get().GetIsometricCamera().CalcZoomOffsetForActiveAnimatedActor(camRotation);
				this.m_cameraBoneInstance.transform.position += b;
			}
			else
			{
				this.m_cameraBoneInstance.transform.position = CameraManager.Get().CameraPositionBounds.center + CameraManager.Get().GetIsometricCamera().m_maxVertDist * Vector3.up;
			}
			Animator componentInChildren = this.m_cameraBoneInstance.GetComponentInChildren<Animator>();
			if (this.HasAnimatorControllerParamater(componentInChildren, SceneManager.GetActiveScene().name.ToLower()))
			{
				if (activeOwnedActorData != null)
				{
					componentInChildren.SetInteger("team", (int)activeOwnedActorData.GetTeam());
				}
				componentInChildren.SetTrigger(SceneManager.GetActiveScene().name.ToLower());
				this.m_cameraBoneTransform = this.m_cameraBoneInstance.FindInChildren("camera0", 0).transform;
				componentInChildren.SetBool("tutorial", GameManager.Get().GameConfig.GameType == GameType.Tutorial);
			}
		}
		this.m_needToHideLoadingScreen = true;
		if (!this.m_fadeObjectsToRevealCharacters)
		{
			FadeObjectsCameraComponent component = base.GetComponent<FadeObjectsCameraComponent>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
	}

	private void LateUpdate()
	{
		if (this.m_cameraBoneTransform)
		{
			base.transform.position = this.m_cameraBoneTransform.position;
			base.transform.rotation = this.m_cameraBoneTransform.rotation * Quaternion.Euler(0f, 180f, 0f);
			Camera.main.fieldOfView = this.m_cameraBoneTransform.localScale.z;
		}
		if (this.m_needToHideLoadingScreen)
		{
			if (!(this.m_cameraBoneTransform != null))
			{
				if (!(this.m_cameraBonePrefab == null))
				{
					return;
				}
			}
			if (UILoadingScreenPanel.Get() != null)
			{
				UILoadingScreenPanel.Get().SetVisible(false);
			}
			this.m_needToHideLoadingScreen = false;
		}
	}
}
