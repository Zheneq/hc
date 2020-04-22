using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITutorialSeasonLevelBar : MonoBehaviour
{
	public RectTransform m_fillContainer;

	public RectTransform m_rewardMarker;

	[Header("Optional")]
	public RectTransform m_rewardContainer;

	public TextMeshProUGUI m_levelText;

	public Image m_rewardIcon;

	public Animator m_animator;

	public string m_animationBaseString;

	public UITooltipHoverObject m_tooltipHoverObj;

	private bool m_filled;

	private RewardUtils.RewardData m_reward;

	public bool SetFilled(bool filled)
	{
		m_filled = filled;
		if (m_animator == null)
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
			UIManager.SetGameObjectActive(m_fillContainer, filled);
		}
		else
		{
			if (!m_animator.isInitialized)
			{
				return false;
			}
			Animator animator = m_animator;
			string animationBaseString = m_animationBaseString;
			object str;
			if (filled)
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
				str = "DefaultIN";
			}
			else
			{
				str = "DisabledIDLE";
			}
			animator.Play(animationBaseString + (string)str);
		}
		return true;
	}

	public bool AnimateFill()
	{
		return SetFilled(m_filled);
	}

	public void SetReward(int level, RewardUtils.RewardData reward)
	{
		m_reward = reward;
		if (m_tooltipHoverObj != null)
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
			if (reward != null)
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
				if (reward.InventoryTemplate != null)
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
					m_tooltipHoverObj.Setup(TooltipType.InventoryItem, delegate(UITooltipBase tooltip)
					{
						if (m_reward == null)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									if (1 == 0)
									{
										/*OpCode not supported: LdMemberToken*/;
									}
									return false;
								}
							}
						}
						(tooltip as UIInventoryItemTooltip).Setup(m_reward.InventoryTemplate);
						return true;
					});
				}
				else
				{
					m_tooltipHoverObj.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
					{
						if (m_reward == null)
						{
							return false;
						}
						string name = m_reward.Name;
						(tooltip as UISimpleTooltip).Setup(name);
						return true;
					});
				}
			}
		}
		if (m_levelText != null)
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
			m_levelText.text = level.ToString();
		}
		UIManager.SetGameObjectActive(m_rewardMarker, reward != null);
		if (!(m_rewardContainer != null))
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
			if (reward == null || reward.SpritePath.IsNullOrEmpty())
			{
				UIManager.SetGameObjectActive(m_rewardContainer, false);
				return;
			}
			UIManager.SetGameObjectActive(m_rewardContainer, true);
			m_rewardIcon.sprite = Resources.Load<Sprite>(reward.SpritePath);
			return;
		}
	}
}
