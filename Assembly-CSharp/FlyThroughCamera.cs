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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_cameraBoneInstance = Object.Instantiate(m_cameraBonePrefab);
			if (activeOwnedActorData != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (activeOwnedActorData != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
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
			switch (5)
			{
			case 0:
				continue;
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!(m_cameraBonePrefab == null))
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (UILoadingScreenPanel.Get() != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			UILoadingScreenPanel.Get().SetVisible(false);
		}
		m_needToHideLoadingScreen = false;
	}
}
