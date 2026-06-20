using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Nameplate popup that appears when the actor is gaining or losing a status.
 */
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
		if (m_parent != null)
		{
			m_parent.NotifyStatusAnimationDone(this, gainedStatus);
		}
	}

	public void Update()
	{
		if (m_parent == null)
		{
			Destroy(gameObject);
		}
	}
}
