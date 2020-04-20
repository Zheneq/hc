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
				if (currentAnimatorClipInfo[0].clip != null && currentAnimatorClipInfo[0].clip.name != animName)
				{
					this.m_RewardLevelUp.Play(animName);
				}
			}
		}
	}
}
