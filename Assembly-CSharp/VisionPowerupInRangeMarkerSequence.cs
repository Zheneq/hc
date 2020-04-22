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
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			List<ActorData> actors = GameFlowData.Get().GetActors();
			for (int i = 0; i < actors.Count; i++)
			{
				ActorData actorData = actors[i];
				if (actorData.GetTeam() != base.Caster.GetTeam())
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					m_actorToMarkerVfx[actorData] = null;
					m_actorsToProcess.Add(actorData);
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_largePingFxPrefab != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				m_markerFxJoint.Initialize(base.Caster.gameObject);
				m_largePingFxInst = InstantiateFX(m_largePingFxPrefab);
				m_largePingFxInst.transform.parent = m_markerFxJoint.m_jointObject.transform;
				m_largePingFxInst.transform.localPosition = Vector3.zero;
				m_largePingFxInst.transform.localRotation = Quaternion.identity;
				m_largePingFoFSelector = m_largePingFxInst.GetComponent<FriendlyEnemyVFXSelector>();
				if (m_largePingFoFSelector != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
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
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				attachedActorVFXInfo.DestroyVfx();
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
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
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameFlowData.Get() == null)
			{
				return;
			}
			int num;
			if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (base.Caster != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!base.Caster.IsDead())
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (base.Caster.GetCurrentBoardSquare() != null)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
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
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag2 = false;
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (GameFlowData.Get().activeOwnedActorData.GetTeam() == base.Caster.GetTeam())
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						flag2 = true;
					}
				}
				float num2 = base.Caster.GetActualSightRange();
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
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						VisionProviderInfo visionProviderInfo2 = visionProviders[i];
						if (visionProviderInfo2.m_satelliteIndex >= 0)
						{
							continue;
						}
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						VisionProviderInfo visionProviderInfo3 = visionProviders[i];
						if (visionProviderInfo3.m_radius > num2)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							VisionProviderInfo visionProviderInfo4 = visionProviders[i];
							num2 = visionProviderInfo4.m_radius;
						}
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
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
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!actorData.IsModelAnimatorDisabled())
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
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
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						float num5 = base.Caster.GetCurrentBoardSquare().HorizontalDistanceOnBoardTo(currentBoardSquare);
						if (num5 > num2)
						{
							flag6 = false;
						}
					}
					AttachedActorVFXInfo attachedActorVFXInfo = m_actorToMarkerVfx[actorData];
					if (flag6)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (attachedActorVFXInfo == null)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							attachedActorVFXInfo = new AttachedActorVFXInfo(m_markerFxPrefab, actorData, m_markerFxJoint, false, "VisionMarker", AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
							attachedActorVFXInfo.SetCasterTeam(base.Caster.GetTeam());
							m_actorToMarkerVfx[actorData] = attachedActorVFXInfo;
						}
						attachedActorVFXInfo.UpdateVisibility(flag6, !flag2);
						if (flag4)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							attachedActorVFXInfo.RestartEffects();
						}
						flag5 = true;
					}
					else if (attachedActorVFXInfo != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						attachedActorVFXInfo.UpdateVisibility(false, false);
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_largePingFxInst != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					m_largePingFxInst.SetActiveIfNeeded(true);
					if (IsActorConsideredVisible(base.Caster))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_largePingFoFSelector != null)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							m_largePingFoFSelector.Setup(base.Caster.GetTeam());
						}
						if (flag3)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							PKFxFX[] componentsInChildren = m_largePingFxInst.GetComponentsInChildren<PKFxFX>();
							foreach (PKFxFX pKFxFX in componentsInChildren)
							{
								pKFxFX.TerminateEffect();
								pKFxFX.StartEffect();
								if (flag2)
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									PlayAudioEvent((!m_emittedLargePingThisTurn) ? m_audioEventFriendlyPing : m_audioEventFriendlyPingSubsequent, base.Caster.gameObject);
									continue;
								}
								string audioEvent;
								if (m_emittedLargePingThisTurn)
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									audioEvent = m_audioEventEnemyPingSubsequent;
								}
								else
								{
									audioEvent = m_audioEventEnemyPing;
								}
								PlayAudioEvent(audioEvent, base.Caster.gameObject);
							}
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
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
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int k = 0; k < m_actorsToProcess.Count; k++)
				{
					ActorData key = m_actorsToProcess[k];
					AttachedActorVFXInfo attachedActorVFXInfo2 = m_actorToMarkerVfx[key];
					if (attachedActorVFXInfo2 != null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						attachedActorVFXInfo2.UpdateVisibility(false, false);
					}
				}
				if (m_largePingFxInst != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
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
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AudioManager.PostEvent(audioEvent, sourceObj);
			return;
		}
	}
}
