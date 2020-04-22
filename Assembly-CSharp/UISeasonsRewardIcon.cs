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
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (currentAnimatorClipInfo[0].clip != null && currentAnimatorClipInfo[0].clip.name != animName)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					m_RewardLevelUp.Play(animName);
					return;
				}
			}
			return;
		}
	}
}
