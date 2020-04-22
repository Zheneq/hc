using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINameplateStatus : MonoBehaviour
{
	public Image m_StatusIcon;

	public TextMeshProUGUI m_StatusText;

	public Animator m_animController;

	public UINameplateItem m_parent;

	private bool gainedStatus;

	public void DisplayAsNegativeStatus(UINameplateItem parent)
	{
		m_animController.Play("GainedNegativeStatus");
		gainedStatus = true;
		m_parent = parent;
	}

	public void DisplayAsPositiveStatus(UINameplateItem parent)
	{
		m_animController.Play("GainedPositiveStatus");
		gainedStatus = true;
		m_parent = parent;
	}

	public void DisplayAsLostStatus(UINameplateItem parent)
	{
		m_animController.Play("LostStatus");
		gainedStatus = false;
		m_parent = parent;
	}

	public void AnimDone()
	{
		if (!(m_parent != null))
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
			m_parent.NotifyStatusAnimationDone(this, gainedStatus);
			return;
		}
	}

	public void Update()
	{
		if (m_parent == null)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
