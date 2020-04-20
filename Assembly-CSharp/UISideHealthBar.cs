﻿using System;
using TMPro;
using UnityEngine;

public class UISideHealthBar : MonoBehaviour
{
	public TextMeshProUGUI m_healthText;

	public TextMeshProUGUI m_pendingHealthText;

	public TextMeshProUGUI m_energyText;

	public TextMeshProUGUI m_shieldText;

	[Range(0f, 1f)]
	public float m_energyPercent;

	public ImageFilledSloped m_energyImage;

	[Range(0f, 1f)]
	public float m_healthPercent;

	public ImageFilledSloped m_healthImage;

	[Range(0f, 1f)]
	public float m_shieldPercent;

	public ImageFilledSloped m_shieldBarImage;

	[Range(0f, 1f)]
	public float m_pendingHPPercent;

	public ImageFilledSloped m_pendingHPImage;

	public Color m_activeActorColor = new Color(0f, 1f, 0.43f, 1f);

	public Color m_confirmedActorColor = new Color(0.6f, 0.6f, 0.6f, 1f);

	private float m_lastEnergyPercent = -1f;

	private float m_lastShieldPercent = -1f;

	private float m_lastHealthPercent = -1f;

	private float m_lastPendingHealthPercent = -1f;

	private ActorData m_actor;

	private bool m_usingCurrentActiveActorColor;

	public void SetActor(ActorData actor)
	{
		this.m_actor = actor;
	}

	public void SetHealthBarColorForActiveActor(bool activeNow)
	{
		if (activeNow != this.m_usingCurrentActiveActorColor)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideHealthBar.SetHealthBarColorForActiveActor(bool)).MethodHandle;
			}
			this.m_usingCurrentActiveActorColor = activeNow;
			if (this.m_healthImage != null)
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
				if (activeNow)
				{
					this.m_healthImage.color = this.m_activeActorColor;
				}
				else
				{
					this.m_healthImage.color = Color.white;
				}
			}
		}
	}

	private void UpdatePendingHealthBar()
	{
		if (this.m_lastPendingHealthPercent == this.m_pendingHPPercent)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideHealthBar.UpdatePendingHealthBar()).MethodHandle;
			}
			return;
		}
		if (this.m_healthPercent > 0f)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_pendingHPImage.fillAmount = this.m_pendingHPPercent;
			this.m_pendingHPImage.gameObject.SetActive(true);
		}
		else
		{
			this.m_pendingHPImage.gameObject.SetActive(false);
		}
		this.m_lastPendingHealthPercent = this.m_pendingHPPercent;
	}

	private void UpdateHealthBar()
	{
		if (!this.m_usingCurrentActiveActorColor)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideHealthBar.UpdateHealthBar()).MethodHandle;
			}
			if (this.m_actor != null)
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
				TurnStateEnum currentState = this.m_actor.GetActorTurnSM().CurrentState;
				if (currentState != TurnStateEnum.CONFIRMED)
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
					if (currentState != TurnStateEnum.WAITING)
					{
						this.m_healthImage.color = Color.white;
						goto IL_8C;
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				this.m_healthImage.color = this.m_confirmedActorColor;
			}
		}
		IL_8C:
		if (this.m_lastHealthPercent == this.m_healthPercent)
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
			return;
		}
		if (this.m_healthPercent > 0f)
		{
			this.m_healthImage.fillAmount = this.m_healthPercent;
			this.m_healthImage.gameObject.SetActive(true);
		}
		else
		{
			this.m_healthImage.gameObject.SetActive(false);
		}
		this.m_lastHealthPercent = this.m_healthPercent;
	}

	private void UpdateShieldBar()
	{
		if (this.m_lastShieldPercent == this.m_shieldPercent)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideHealthBar.UpdateShieldBar()).MethodHandle;
			}
			return;
		}
		if (this.m_shieldPercent > 0f)
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
			this.m_shieldBarImage.fillAmount = this.m_shieldPercent;
			this.m_shieldBarImage.gameObject.SetActive(true);
		}
		else
		{
			this.m_shieldBarImage.gameObject.SetActive(false);
		}
		this.m_lastShieldPercent = this.m_shieldPercent;
	}

	private void UpdateEnergyBar()
	{
		if (this.m_lastEnergyPercent == this.m_energyPercent)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideHealthBar.UpdateEnergyBar()).MethodHandle;
			}
			return;
		}
		if (this.m_energyPercent > 0f)
		{
			this.m_energyImage.fillAmount = this.m_energyPercent;
			this.m_energyImage.gameObject.SetActive(true);
		}
		else
		{
			this.m_energyImage.gameObject.SetActive(false);
		}
		this.m_lastEnergyPercent = this.m_energyPercent;
	}

	private void Update()
	{
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideHealthBar.Update()).MethodHandle;
			}
			ActorData actor = this.m_actor;
			if (actor != null)
			{
				int hitPointsAfterResolution = actor.GetHitPointsAfterResolution();
				int maxHitPoints = actor.GetMaxHitPoints();
				int energyToDisplay = actor.GetEnergyToDisplay();
				int actualMaxTechPoints = actor.GetActualMaxTechPoints();
				int num = actor.\u0004();
				int clientUnappliedHoTTotal_ToDisplay_zq = actor.GetClientUnappliedHoTTotal_ToDisplay_zq();
				if (clientUnappliedHoTTotal_ToDisplay_zq > 0)
				{
					this.m_pendingHealthText.text = "+" + clientUnappliedHoTTotal_ToDisplay_zq.ToString();
				}
				else
				{
					this.m_pendingHealthText.text = string.Empty;
				}
				if (num > 0)
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
					this.m_shieldText.text = "+" + num.ToString();
				}
				else
				{
					this.m_shieldText.text = string.Empty;
				}
				this.m_healthText.text = hitPointsAfterResolution.ToString();
				this.m_energyText.text = energyToDisplay.ToString();
				this.m_healthPercent = (float)hitPointsAfterResolution / (float)(maxHitPoints + num);
				this.m_shieldPercent = (float)(hitPointsAfterResolution + num) / (float)(maxHitPoints + num);
				this.m_pendingHPPercent = (float)(hitPointsAfterResolution + num + clientUnappliedHoTTotal_ToDisplay_zq) / (float)(maxHitPoints + num);
				this.m_energyPercent = (float)energyToDisplay / (float)actualMaxTechPoints;
			}
		}
		this.UpdateEnergyBar();
		this.UpdateHealthBar();
		this.UpdateShieldBar();
		this.UpdatePendingHealthBar();
	}

	private void OnEnable()
	{
		this.m_lastEnergyPercent = 1f;
		this.Update();
	}
}
