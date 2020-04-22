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
		int fadeObjectsToRevealCharacters;
		if (!(GameManager.Get() == null))
		{
			if (GameManager.Get().GameConfig != null)
			{
				fadeObjectsToRevealCharacters = ((GameManager.Get().GameConfig.GameType != GameType.Tutorial) ? 1 : 0);
				goto IL_0051;
			}
		}
		fadeObjectsToRevealCharacters = 1;
		goto IL_0051;
		IL_0051:
		m_fadeObjectsToRevealCharacters = ((byte)fadeObjectsToRevealCharacters != 0);
	}

	private void OnDisable()
	{
	}

	public void StopFlyThroughAnimation()
	{
		Object.Destroy(m_cameraBoneInstance);
		m_cameraBoneInstance = null;
		m_cameraBoneTransform = null;
		if (m_fadeObjectsToRevealCharacters)
		{
			return;
		}
		FadeObjectsCameraComponent component = GetComponent<FadeObjectsCameraComponent>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			component.enabled = true;
			return;
		}
	}

	public bool HasAnimatorControllerParamater(Animator animator, string paramName)
	{
		bool result = false;
		AnimatorControllerParameter[] parameters = animator.parameters;
		int num = 0;
		while (true)
		{
			if (num < parameters.Length)
			{
				if (parameters[num].name == paramName)
				{
					result = true;
					break;
				}
				num++;
				continue;
			}
			break;
		}
		return result;
	}

	public void StartFlyThroughAnimation()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (m_cameraBonePrefab != null)
		{
			m_cameraBoneInstance = Object.Instantiate(m_cameraBonePrefab);
			if (activeOwnedActorData != null)
			{
				m_cameraBoneInstance.transform.position = activeOwnedActorData.transform.position;
				Quaternion camRotation = Quaternion.Euler(new Vector3(0f, CameraManager.Get().GetIsometricCamera().GetInitialYAngle()));
				Vector3 vector = CameraManager.Get().GetIsometricCamera().CalcZoomOffsetForActiveAnimatedActor(camRotation);
				m_cameraBoneInstance.transform.position += vector;
			}
			else
			{
				m_cameraBoneInstance.transform.position = CameraManager.Get().CameraPositionBounds.center + CameraManager.Get().GetIsometricCamera().m_maxVertDist * Vector3.up;
			}
			Animator componentInChildren = m_cameraBoneInstance.GetComponentInChildren<Animator>();
			if (HasAnimatorControllerParamater(componentInChildren, SceneManager.GetActiveScene().name.ToLower()))
			{
				if (activeOwnedActorData != null)
				{
					componentInChildren.SetInteger("team", (int)activeOwnedActorData.GetTeam());
				}
				componentInChildren.SetTrigger(SceneManager.GetActiveScene().name.ToLower());
				m_cameraBoneTransform = m_cameraBoneInstance.FindInChildren("camera0").transform;
				componentInChildren.SetBool("tutorial", GameManager.Get().GameConfig.GameType == GameType.Tutorial);
			}
		}
		m_needToHideLoadingScreen = true;
		if (m_fadeObjectsToRevealCharacters)
		{
			return;
		}
		while (true)
		{
			FadeObjectsCameraComponent component = GetComponent<FadeObjectsCameraComponent>();
			if (component != null)
			{
				component.enabled = false;
			}
			return;
		}
	}

	private void LateUpdate()
	{
		if ((bool)m_cameraBoneTransform)
		{
			base.transform.position = m_cameraBoneTransform.position;
			base.transform.rotation = m_cameraBoneTransform.rotation * Quaternion.Euler(0f, 180f, 0f);
			Camera main = Camera.main;
			Vector3 localScale = m_cameraBoneTransform.localScale;
			main.fieldOfView = localScale.z;
		}
		if (!m_needToHideLoadingScreen)
		{
			return;
		}
		if (!(m_cameraBoneTransform != null))
		{
			if (!(m_cameraBonePrefab == null))
			{
				return;
			}
		}
		if (UILoadingScreenPanel.Get() != null)
		{
			UILoadingScreenPanel.Get().SetVisible(false);
		}
		m_needToHideLoadingScreen = false;
	}
}
