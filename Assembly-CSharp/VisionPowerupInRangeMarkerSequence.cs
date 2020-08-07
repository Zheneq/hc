using System.Collections.Generic;
using UnityEngine;

public class VisionPowerupInRangeMarkerSequence : Sequence
{
	public class IntervalTimer
	{
		private float m_duration;

		private float m_timeTillEnd;

		public IntervalTimer(float duration, float initialDuration)
		{
			m_duration = duration;
			m_timeTillEnd = initialDuration;
		}

		public void ClearTimeTillEnd()
		{
			m_timeTillEnd = 0f;
		}

		public bool TickTimer(float dt)
		{
			bool result = false;
			m_timeTillEnd -= dt;
			if (m_timeTillEnd <= 0f)
			{
				m_timeTillEnd = m_duration;
				result = true;
			}
			return result;
		}
	}

	[Separator("Main Ping VFX originating form center", true)]
	public GameObject m_largePingFxPrefab;

	[Separator("Marker FX to indicate actor is in range", true)]
	public GameObject m_markerFxPrefab;

	[JointPopup("Marker FX attach joint")]
	public JointPopupProperty m_markerFxJoint;

	[Separator("Ping Intervals", true)]
	public float m_largePingDuration = 5f;

	public float m_markerPingDuration = 1f;

	[Separator("Audio Events", "orange")]
	[AudioEvent(false)]
	public string m_audioEventFriendlyPing;

	[AudioEvent(false)]
	public string m_audioEventFriendlyPingSubsequent;

	[AudioEvent(false)]
	public string m_audioEventEnemyPing;

	[AudioEvent(false)]
	public string m_audioEventEnemyPingSubsequent;

	[AudioEvent(false)]
	public string m_audioEventTargetReveal;

	private bool m_canBeVisibleLastUpdate;

	private GameObject m_largePingFxInst;

	private FriendlyEnemyVFXSelector m_largePingFoFSelector;

	private Dictionary<ActorData, AttachedActorVFXInfo> m_actorToMarkerVfx = new Dictionary<ActorData, AttachedActorVFXInfo>();

	private List<ActorData> m_actorsToProcess = new List<ActorData>();

	private int m_updatingTurn = -1;

	private bool m_emittedLargePingThisTurn;

	private bool m_playedMarkerSfxThisTurn;

	private IntervalTimer m_largePingTimer;

	private IntervalTimer m_markerPingTimer;

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (base.Caster != null)
		{
			List<ActorData> actors = GameFlowData.Get().GetActors();
			for (int i = 0; i < actors.Count; i++)
			{
				ActorData actorData = actors[i];
				if (actorData.GetTeam() != base.Caster.GetTeam())
				{
					m_actorToMarkerVfx[actorData] = null;
					m_actorsToProcess.Add(actorData);
				}
			}
			if (m_largePingFxPrefab != null)
			{
				m_markerFxJoint.Initialize(base.Caster.gameObject);
				m_largePingFxInst = InstantiateFX(m_largePingFxPrefab);
				m_largePingFxInst.transform.parent = m_markerFxJoint.m_jointObject.transform;
				m_largePingFxInst.transform.localPosition = Vector3.zero;
				m_largePingFxInst.transform.localRotation = Quaternion.identity;
				m_largePingFoFSelector = m_largePingFxInst.GetComponent<FriendlyEnemyVFXSelector>();
				if (m_largePingFoFSelector != null)
				{
					m_largePingFoFSelector.Setup(base.Caster.GetTeam());
				}
			}
		}
		m_largePingTimer = new IntervalTimer(m_largePingDuration, 0f);
		m_markerPingTimer = new IntervalTimer(m_markerPingDuration, 0f);
		m_updatingTurn = GameFlowData.Get().CurrentTurn;
	}

	private void OnDisable()
	{
		for (int i = 0; i < m_actorsToProcess.Count; i++)
		{
			ActorData key = m_actorsToProcess[i];
			AttachedActorVFXInfo attachedActorVFXInfo = m_actorToMarkerVfx[key];
			if (attachedActorVFXInfo != null)
			{
				attachedActorVFXInfo.DestroyVfx();
			}
		}
		while (true)
		{
			m_actorsToProcess.Clear();
			m_actorToMarkerVfx.Clear();
			return;
		}
	}

	internal override void OnTurnStart(int currentTurn)
	{
		base.OnTurnStart(currentTurn);
		m_updatingTurn = GameFlowData.Get().CurrentTurn;
		m_emittedLargePingThisTurn = false;
		m_playedMarkerSfxThisTurn = false;
		m_largePingTimer.ClearTimeTillEnd();
		m_markerPingTimer.ClearTimeTillEnd();
	}

	private void Update()
	{
		if (m_markerFxPrefab == null)
		{
			return;
		}
		while (true)
		{
			if (GameFlowData.Get() == null)
			{
				return;
			}
			int num;
			if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
			{
				if (base.Caster != null)
				{
					if (!base.Caster.IsDead())
					{
						if (base.Caster.GetCurrentBoardSquare() != null)
						{
							num = ((GameFlowData.Get().CurrentTurn == m_updatingTurn) ? 1 : 0);
							goto IL_00c4;
						}
					}
				}
			}
			num = 0;
			goto IL_00c4;
			IL_00c4:
			bool flag = (byte)num != 0;
			if (flag)
			{
				bool flag2 = false;
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					if (GameFlowData.Get().activeOwnedActorData.GetTeam() == base.Caster.GetTeam())
					{
						flag2 = true;
					}
				}
				float num2 = base.Caster.GetSightRange();
				ActorAdditionalVisionProviders actorAdditionalVisionProviders = base.Caster.GetActorAdditionalVisionProviders();
				if (actorAdditionalVisionProviders != null)
				{
					SyncListVisionProviderInfo visionProviders = actorAdditionalVisionProviders.GetVisionProviders();
					for (int i = 0; i < visionProviders.Count; i++)
					{
						VisionProviderInfo visionProviderInfo = visionProviders[i];
						if (visionProviderInfo.m_actorIndex != base.Caster.ActorIndex)
						{
							continue;
						}
						VisionProviderInfo visionProviderInfo2 = visionProviders[i];
						if (visionProviderInfo2.m_satelliteIndex >= 0)
						{
							continue;
						}
						VisionProviderInfo visionProviderInfo3 = visionProviders[i];
						if (visionProviderInfo3.m_radius > num2)
						{
							VisionProviderInfo visionProviderInfo4 = visionProviders[i];
							num2 = visionProviderInfo4.m_radius;
						}
					}
				}
				bool flag3 = m_largePingTimer.TickTimer(GameTime.deltaTime);
				bool flag4 = m_markerPingTimer.TickTimer(GameTime.deltaTime);
				bool flag5 = false;
				for (int num3 = 0; num3 < m_actorsToProcess.Count; num3++)
				{
					ActorData actorData = m_actorsToProcess[num3];
					BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
					int num4;
					if (!actorData.IsDead() && currentBoardSquare != null)
					{
						if (!actorData.IsModelAnimatorDisabled())
						{
							num4 = (actorData.IsVisibleToClient() ? 1 : 0);
							goto IL_0294;
						}
					}
					num4 = 0;
					goto IL_0294;
					IL_0294:
					bool flag6 = (byte)num4 != 0;
					if (flag6)
					{
						float num5 = base.Caster.GetCurrentBoardSquare().HorizontalDistanceOnBoardTo(currentBoardSquare);
						if (num5 > num2)
						{
							flag6 = false;
						}
					}
					AttachedActorVFXInfo attachedActorVFXInfo = m_actorToMarkerVfx[actorData];
					if (flag6)
					{
						if (attachedActorVFXInfo == null)
						{
							attachedActorVFXInfo = new AttachedActorVFXInfo(m_markerFxPrefab, actorData, m_markerFxJoint, false, "VisionMarker", AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
							attachedActorVFXInfo.SetCasterTeam(base.Caster.GetTeam());
							m_actorToMarkerVfx[actorData] = attachedActorVFXInfo;
						}
						attachedActorVFXInfo.UpdateVisibility(flag6, !flag2);
						if (flag4)
						{
							attachedActorVFXInfo.RestartEffects();
						}
						flag5 = true;
					}
					else if (attachedActorVFXInfo != null)
					{
						attachedActorVFXInfo.UpdateVisibility(false, false);
					}
				}
				if (m_largePingFxInst != null)
				{
					m_largePingFxInst.SetActiveIfNeeded(true);
					if (IsActorConsideredVisible(base.Caster))
					{
						if (m_largePingFoFSelector != null)
						{
							m_largePingFoFSelector.Setup(base.Caster.GetTeam());
						}
						if (flag3)
						{
							PKFxFX[] componentsInChildren = m_largePingFxInst.GetComponentsInChildren<PKFxFX>();
							foreach (PKFxFX pKFxFX in componentsInChildren)
							{
								pKFxFX.TerminateEffect();
								pKFxFX.StartEffect();
								if (flag2)
								{
									PlayAudioEvent((!m_emittedLargePingThisTurn) ? m_audioEventFriendlyPing : m_audioEventFriendlyPingSubsequent, base.Caster.gameObject);
									continue;
								}
								string audioEvent;
								if (m_emittedLargePingThisTurn)
								{
									audioEvent = m_audioEventEnemyPingSubsequent;
								}
								else
								{
									audioEvent = m_audioEventEnemyPing;
								}
								PlayAudioEvent(audioEvent, base.Caster.gameObject);
							}
							m_emittedLargePingThisTurn = true;
						}
					}
				}
				if (flag5 && !m_playedMarkerSfxThisTurn)
				{
					m_playedMarkerSfxThisTurn = true;
					PlayAudioEvent(m_audioEventTargetReveal, base.Caster.gameObject);
				}
			}
			else if (m_canBeVisibleLastUpdate)
			{
				for (int k = 0; k < m_actorsToProcess.Count; k++)
				{
					ActorData key = m_actorsToProcess[k];
					AttachedActorVFXInfo attachedActorVFXInfo2 = m_actorToMarkerVfx[key];
					if (attachedActorVFXInfo2 != null)
					{
						attachedActorVFXInfo2.UpdateVisibility(false, false);
					}
				}
				if (m_largePingFxInst != null)
				{
					m_largePingFxInst.SetActiveIfNeeded(false);
				}
			}
			m_canBeVisibleLastUpdate = flag;
			return;
		}
	}

	private void PlayAudioEvent(string audioEvent, GameObject sourceObj)
	{
		if (audioEvent.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			AudioManager.PostEvent(audioEvent, sourceObj);
			return;
		}
	}
}
