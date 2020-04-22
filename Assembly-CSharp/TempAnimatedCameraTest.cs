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
		if (m_parentFbx != null)
		{
			m_animator = m_parentFbx.GetComponent<Animator>();
		}
	}

	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.C) || !(m_animator != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_animator.SetTrigger(m_animStartTrigger);
			m_updatingWithAnim = true;
			return;
		}
	}

	private void LateUpdate()
	{
		if (!(m_cameraJoint != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_updatingWithAnim)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					Camera.main.transform.position = m_cameraJoint.transform.position;
					Camera.main.transform.rotation = m_cameraJoint.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
					Camera.main.fieldOfView = GetFieldOfView();
					return;
				}
			}
			return;
		}
	}

	private float GetFieldOfView()
	{
		float result = Camera.main.fieldOfView;
		if (m_cameraJoint != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Vector3 localScale = m_cameraJoint.transform.localScale;
			if (localScale.z > 1f)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 localScale2 = m_cameraJoint.transform.localScale;
				result = localScale2.z;
			}
		}
		return result;
	}
}
