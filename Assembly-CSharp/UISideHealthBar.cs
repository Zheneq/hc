using System.Text;
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
		m_actor = actor;
	}

	public void SetHealthBarColorForActiveActor(bool activeNow)
	{
		if (activeNow == m_usingCurrentActiveActorColor)
		{
			return;
		}
		while (true)
		{
			m_usingCurrentActiveActorColor = activeNow;
			if (!(m_healthImage != null))
			{
				return;
			}
			while (true)
			{
				if (activeNow)
				{
					m_healthImage.color = m_activeActorColor;
				}
				else
				{
					m_healthImage.color = Color.white;
				}
				return;
			}
		}
	}

	private void UpdatePendingHealthBar()
	{
		if (m_lastPendingHealthPercent == m_pendingHPPercent)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_healthPercent > 0f)
		{
			m_pendingHPImage.fillAmount = m_pendingHPPercent;
			m_pendingHPImage.gameObject.SetActive(true);
		}
		else
		{
			m_pendingHPImage.gameObject.SetActive(false);
		}
		m_lastPendingHealthPercent = m_pendingHPPercent;
	}

	private void UpdateHealthBar()
	{
		if (!m_usingCurrentActiveActorColor)
		{
			if (m_actor != null)
			{
				TurnStateEnum currentState = m_actor.GetActorTurnSM().CurrentState;
				if (currentState != TurnStateEnum.CONFIRMED)
				{
					if (currentState != TurnStateEnum.WAITING)
					{
						m_healthImage.color = Color.white;
						goto IL_008c;
					}
				}
				m_healthImage.color = m_confirmedActorColor;
			}
		}
		goto IL_008c;
		IL_008c:
		if (m_lastHealthPercent == m_healthPercent)
		{
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		if (m_healthPercent > 0f)
		{
			m_healthImage.fillAmount = m_healthPercent;
			m_healthImage.gameObject.SetActive(true);
		}
		else
		{
			m_healthImage.gameObject.SetActive(false);
		}
		m_lastHealthPercent = m_healthPercent;
	}

	private void UpdateShieldBar()
	{
		if (m_lastShieldPercent == m_shieldPercent)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_shieldPercent > 0f)
		{
			m_shieldBarImage.fillAmount = m_shieldPercent;
			m_shieldBarImage.gameObject.SetActive(true);
		}
		else
		{
			m_shieldBarImage.gameObject.SetActive(false);
		}
		m_lastShieldPercent = m_shieldPercent;
	}

	private void UpdateEnergyBar()
	{
		if (m_lastEnergyPercent == m_energyPercent)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_energyPercent > 0f)
		{
			m_energyImage.fillAmount = m_energyPercent;
			m_energyImage.gameObject.SetActive(true);
		}
		else
		{
			m_energyImage.gameObject.SetActive(false);
		}
		m_lastEnergyPercent = m_energyPercent;
	}

	private void Update()
	{
		if (GameFlowData.Get() != null)
		{
			ActorData actor = m_actor;
			if (actor != null)
			{
				int hitPointsAfterResolution = actor.GetHitPointsToDisplay();
				int maxHitPoints = actor.GetMaxHitPoints();
				int energyToDisplay = actor.GetTechPointsToDisplay();
				int actualMaxTechPoints = actor.GetMaxTechPoints();
				int num = actor.GetShieldPoints();
				int clientUnappliedHoTTotal_ToDisplay_zq = actor.GetHoTTotalToDisplay();
				if (clientUnappliedHoTTotal_ToDisplay_zq > 0)
				{
					m_pendingHealthText.text = new StringBuilder().Append("+").Append(clientUnappliedHoTTotal_ToDisplay_zq).ToString();
				}
				else
				{
					m_pendingHealthText.text = string.Empty;
				}
				if (num > 0)
				{
					m_shieldText.text = new StringBuilder().Append("+").Append(num).ToString();
				}
				else
				{
					m_shieldText.text = string.Empty;
				}
				m_healthText.text = hitPointsAfterResolution.ToString();
				m_energyText.text = energyToDisplay.ToString();
				m_healthPercent = (float)hitPointsAfterResolution / (float)(maxHitPoints + num);
				m_shieldPercent = (float)(hitPointsAfterResolution + num) / (float)(maxHitPoints + num);
				m_pendingHPPercent = (float)(hitPointsAfterResolution + num + clientUnappliedHoTTotal_ToDisplay_zq) / (float)(maxHitPoints + num);
				m_energyPercent = (float)energyToDisplay / (float)actualMaxTechPoints;
			}
		}
		UpdateEnergyBar();
		UpdateHealthBar();
		UpdateShieldBar();
		UpdatePendingHealthBar();
	}

	private void OnEnable()
	{
		m_lastEnergyPercent = 1f;
		Update();
	}
}
