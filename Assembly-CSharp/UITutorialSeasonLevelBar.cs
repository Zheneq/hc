using System;
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
		this.m_filled = filled;
		if (this.m_animator == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialSeasonLevelBar.SetFilled(bool)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_fillContainer, filled, null);
		}
		else
		{
			if (!this.m_animator.isInitialized)
			{
				return false;
			}
			Animator animator = this.m_animator;
			string animationBaseString = this.m_animationBaseString;
			string str;
			if (filled)
			{
				for (;;)
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
			animator.Play(animationBaseString + str);
		}
		return true;
	}

	public bool AnimateFill()
	{
		return this.SetFilled(this.m_filled);
	}

	public void SetReward(int level, RewardUtils.RewardData reward)
	{
		this.m_reward = reward;
		if (this.m_tooltipHoverObj != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialSeasonLevelBar.SetReward(int, RewardUtils.RewardData)).MethodHandle;
			}
			if (reward != null)
			{
				for (;;)
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
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_tooltipHoverObj.Setup(TooltipType.InventoryItem, delegate(UITooltipBase tooltip)
					{
						if (this.m_reward == null)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!true)
							{
								RuntimeMethodHandle runtimeMethodHandle2 = methodof(UITutorialSeasonLevelBar.<SetReward>m__0(UITooltipBase)).MethodHandle;
							}
							return false;
						}
						(tooltip as UIInventoryItemTooltip).Setup(this.m_reward.InventoryTemplate);
						return true;
					}, null);
				}
				else
				{
					this.m_tooltipHoverObj.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
					{
						if (this.m_reward == null)
						{
							return false;
						}
						string name = this.m_reward.Name;
						(tooltip as UISimpleTooltip).Setup(name);
						return true;
					}, null);
				}
			}
		}
		if (this.m_levelText != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_levelText.text = level.ToString();
		}
		UIManager.SetGameObjectActive(this.m_rewardMarker, reward != null, null);
		if (this.m_rewardContainer != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (reward == null || reward.SpritePath.IsNullOrEmpty())
			{
				UIManager.SetGameObjectActive(this.m_rewardContainer, false, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_rewardContainer, true, null);
				this.m_rewardIcon.sprite = Resources.Load<Sprite>(reward.SpritePath);
			}
		}
	}
}
