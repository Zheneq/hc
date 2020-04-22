using System.Collections.Generic;
using UnityEngine;

public class UICharacterSelectWorldObjects : UICharacterWorldObjects
{
	public Animator m_LobbyCameraAnimator;

	public Animator m_LobbyBaseAnimator;

	public List<GameObject> m_objectsToHideForToggleUI;

	private static UICharacterSelectWorldObjects s_instance;

	private void Awake()
	{
		s_instance = this;
		Initialize();
	}

	private void OnDestroy()
	{
		if (!(s_instance == this))
		{
			return;
		}
		while (true)
		{
			Log.Info(string.Concat(GetType(), " OnDestroy, clearing singleton reference"));
			s_instance = null;
			return;
		}
	}

	public static UICharacterSelectWorldObjects Get()
	{
		return s_instance;
	}

	protected override void OnVisibleChange()
	{
		if (!(m_LobbyCameraAnimator != null))
		{
			return;
		}
		while (true)
		{
			if (IsVisible())
			{
				if (!UIPlayCategoryMenu.Get().IsVisible() || UIPlayCategoryMenu.Get().GetGameTypeForSelectedButton() != GameType.Ranked)
				{
					PlayCameraAnimation("CamIN");
					goto IL_0072;
				}
			}
			PlayCameraAnimation("CamOUT");
			goto IL_0072;
			IL_0072:
			UIManager.SetGameObjectActive(m_LobbyBaseAnimator, IsVisible());
			return;
		}
	}

	public void PlayCameraAnimation(string animName)
	{
		UIAnimationEventManager.Get().PlayAnimation(m_LobbyCameraAnimator, animName, null, string.Empty, 0, 0f, true, true);
	}
}
