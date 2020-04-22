using UnityEngine;

public class UISeasonsRewardIcon : UIInventoryItem
{
	public Animator m_RewardLevelUp;

	public void PlayLevelUp(string animName)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		AnimatorClipInfo[] currentAnimatorClipInfo = m_RewardLevelUp.GetCurrentAnimatorClipInfo(0);
		if (currentAnimatorClipInfo.Length <= 0)
		{
			return;
		}
		while (true)
		{
			if (currentAnimatorClipInfo[0].clip != null && currentAnimatorClipInfo[0].clip.name != animName)
			{
				while (true)
				{
					m_RewardLevelUp.Play(animName);
					return;
				}
			}
			return;
		}
	}
}
