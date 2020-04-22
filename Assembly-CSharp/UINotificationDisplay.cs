using UnityEngine;
using UnityEngine.UI;

public class UINotificationDisplay : MonoBehaviour
{
	public enum NoficationType
	{
		Kill,
		FlagDrop,
		FlagPickup,
		CapturePoint,
		MAX
	}

	public Sprite[] m_notificationIcon = new Sprite[4];

	public Image[] m_allAlphaImages;

	public Image m_iconType;

	public Image m_glowImage;

	public Image m_background;

	public Image[] characterIcons;

	public RectTransform[] characterIconsTransforms;

	public Image m_secondaryCharacterIcon;

	public RectTransform m_secondaryCharacterIransform;

	private float currentAlpha;

	public void Update()
	{
		CheckKillParticipants();
	}

	public void SetAlpha(float newAlpha)
	{
		if (currentAlpha == Mathf.Clamp(newAlpha, 0f, 1f))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		currentAlpha = newAlpha;
		currentAlpha = Mathf.Clamp(currentAlpha, 0f, 1f);
		for (int i = 0; i < m_allAlphaImages.Length; i++)
		{
			Color color = m_allAlphaImages[i].color;
			color.a = currentAlpha;
			m_allAlphaImages[i].color = color;
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void CheckKillParticipants()
	{
		int num = 0;
		for (int i = 0; i < characterIconsTransforms.Length; i++)
		{
			if (i < num)
			{
				UIManager.SetGameObjectActive(characterIconsTransforms[i], true);
			}
			else
			{
				UIManager.SetGameObjectActive(characterIconsTransforms[i], false);
			}
		}
	}

	public void Setup(ActorData actorDied)
	{
		m_secondaryCharacterIcon.sprite = actorDied.GetAliveHUDIcon();
		bool flag = false;
		if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			flag = (actorDied.GetTeam() == GameFlowData.Get().activeOwnedActorData.GetTeam());
		}
		if (flag)
		{
			m_glowImage.color = Color.red;
			m_background.color = Color.red;
		}
		else
		{
			m_glowImage.color = Color.blue;
			m_background.color = Color.blue;
		}
		CheckKillParticipants();
		SetAlpha(1f);
	}
}
