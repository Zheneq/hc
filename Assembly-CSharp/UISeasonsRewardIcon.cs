using System;
using UnityEngine;

public class UISeasonsRewardIcon : UIInventoryItem
{
	public Animator m_RewardLevelUp;

	public void PlayLevelUp(string animName)
	{
		if (base.gameObject.activeInHierarchy)
		{
			AnimatorClipInfo[] currentAnimatorClipInfo = this.m_RewardLevelUp.GetCurrentAnimatorClipInfo(0);
			if (currentAnimatorClipInfo.Length > 0)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsRewardIcon.PlayLevelUp(string)).MethodHandle;
				}
				if (currentAnimatorClipInfo[0].clip != null && currentAnimatorClipInfo[0].clip.name != animName)
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
					this.m_RewardLevelUp.Play(animName);
				}
			}
		}
	}
}
