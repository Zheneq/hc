using System;
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
		UICharacterSelectWorldObjects.s_instance = this;
		base.Initialize();
	}

	private void OnDestroy()
	{
		if (UICharacterSelectWorldObjects.s_instance == this)
		{
			Log.Info(base.GetType() + " OnDestroy, clearing singleton reference", new object[0]);
			UICharacterSelectWorldObjects.s_instance = null;
		}
	}

	public static UICharacterSelectWorldObjects Get()
	{
		return UICharacterSelectWorldObjects.s_instance;
	}

	protected override void OnVisibleChange()
	{
		if (this.m_LobbyCameraAnimator != null)
		{
			if (base.IsVisible())
			{
				if (!UIPlayCategoryMenu.Get().IsVisible() || UIPlayCategoryMenu.Get().GetGameTypeForSelectedButton() != GameType.Ranked)
				{
					this.PlayCameraAnimation("CamIN");
					goto IL_72;
				}
			}
			this.PlayCameraAnimation("CamOUT");
			IL_72:
			UIManager.SetGameObjectActive(this.m_LobbyBaseAnimator, base.IsVisible(), null);
		}
	}

	public void PlayCameraAnimation(string animName)
	{
		UIAnimationEventManager.Get().PlayAnimation(this.m_LobbyCameraAnimator, animName, null, string.Empty, 0, 0f, true, true, null, null);
	}
}
