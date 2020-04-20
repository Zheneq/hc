﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class UINotificationDisplay : MonoBehaviour
{
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
		this.CheckKillParticipants();
	}

	public void SetAlpha(float newAlpha)
	{
		if (this.currentAlpha == Mathf.Clamp(newAlpha, 0f, 1f))
		{
			return;
		}
		this.currentAlpha = newAlpha;
		this.currentAlpha = Mathf.Clamp(this.currentAlpha, 0f, 1f);
		for (int i = 0; i < this.m_allAlphaImages.Length; i++)
		{
			Color color = this.m_allAlphaImages[i].color;
			color.a = this.currentAlpha;
			this.m_allAlphaImages[i].color = color;
		}
	}

	private void CheckKillParticipants()
	{
		int num = 0;
		for (int i = 0; i < this.characterIconsTransforms.Length; i++)
		{
			if (i < num)
			{
				UIManager.SetGameObjectActive(this.characterIconsTransforms[i], true, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.characterIconsTransforms[i], false, null);
			}
		}
	}

	public void Setup(ActorData actorDied)
	{
		this.m_secondaryCharacterIcon.sprite = actorDied.GetAliveHUDIcon();
		bool flag = false;
		if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
		{
			flag = (actorDied.GetTeam() == GameFlowData.Get().activeOwnedActorData.GetTeam());
		}
		if (flag)
		{
			this.m_glowImage.color = Color.red;
			this.m_background.color = Color.red;
		}
		else
		{
			this.m_glowImage.color = Color.blue;
			this.m_background.color = Color.blue;
		}
		this.CheckKillParticipants();
		this.SetAlpha(1f);
	}

	public enum NoficationType
	{
		Kill,
		FlagDrop,
		FlagPickup,
		CapturePoint,
		MAX
	}
}
