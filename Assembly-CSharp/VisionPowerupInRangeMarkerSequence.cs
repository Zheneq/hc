using System;
using System.Collections.Generic;
using UnityEngine;

public class VisionPowerupInRangeMarkerSequence : Sequence
{
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

	private VisionPowerupInRangeMarkerSequence.IntervalTimer m_largePingTimer;

	private VisionPowerupInRangeMarkerSequence.IntervalTimer m_markerPingTimer;

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
					this.m_actorToMarkerVfx[actorData] = null;
					this.m_actorsToProcess.Add(actorData);
				}
			}
			if (this.m_largePingFxPrefab != null)
			{
				this.m_markerFxJoint.Initialize(base.Caster.gameObject);
				this.m_largePingFxInst = base.InstantiateFX(this.m_largePingFxPrefab);
				this.m_largePingFxInst.transform.parent = this.m_markerFxJoint.m_jointObject.transform;
				this.m_largePingFxInst.transform.localPosition = Vector3.zero;
				this.m_largePingFxInst.transform.localRotation = Quaternion.identity;
				this.m_largePingFoFSelector = this.m_largePingFxInst.GetComponent<FriendlyEnemyVFXSelector>();
				if (this.m_largePingFoFSelector != null)
				{
					this.m_largePingFoFSelector.Setup(base.Caster.GetTeam());
				}
			}
		}
		this.m_largePingTimer = new VisionPowerupInRangeMarkerSequence.IntervalTimer(this.m_largePingDuration, 0f);
		this.m_markerPingTimer = new VisionPowerupInRangeMarkerSequence.IntervalTimer(this.m_markerPingDuration, 0f);
		this.m_updatingTurn = GameFlowData.Get().CurrentTurn;
	}

	private void OnDisable()
	{
		for (int i = 0; i < this.m_actorsToProcess.Count; i++)
		{
			ActorData key = this.m_actorsToProcess[i];
			AttachedActorVFXInfo attachedActorVFXInfo = this.m_actorToMarkerVfx[key];
			if (attachedActorVFXInfo != null)
			{
				attachedActorVFXInfo.DestroyVfx();
			}
		}
		this.m_actorsToProcess.Clear();
		this.m_actorToMarkerVfx.Clear();
	}

	internal override void OnTurnStart(int currentTurn)
	{
		base.OnTurnStart(currentTurn);
		this.m_updatingTurn = GameFlowData.Get().CurrentTurn;
		this.m_emittedLargePingThisTurn = false;
		this.m_playedMarkerSfxThisTurn = false;
		this.m_largePingTimer.ClearTimeTillEnd();
		this.m_markerPingTimer.ClearTimeTillEnd();
	}

	private void Update()
	{
		if (!(this.m_markerFxPrefab == null))
		{
			if (!(GameFlowData.Get() == null))
			{
				bool flag;
				if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
				{
					if (base.Caster != null)
					{
						if (!base.Caster.IsDead())
						{
							if (base.Caster.GetCurrentBoardSquare() != null)
							{
								flag = (GameFlowData.Get().CurrentTurn == this.m_updatingTurn);
								goto IL_C4;
							}
						}
					}
				}
				flag = false;
				IL_C4:
				bool flag2 = flag;
				if (flag2)
				{
					bool flag3 = false;
					if (GameFlowData.Get().activeOwnedActorData != null)
					{
						if (GameFlowData.Get().activeOwnedActorData.GetTeam() == base.Caster.GetTeam())
						{
							flag3 = true;
						}
					}
					float num = base.Caster.GetActualSightRange();
					ActorAdditionalVisionProviders actorAdditionalVisionProviders = base.Caster.GetActorAdditionalVisionProviders();
					if (actorAdditionalVisionProviders != null)
					{
						SyncListVisionProviderInfo visionProviders = actorAdditionalVisionProviders.GetVisionProviders();
						for (int i = 0; i < (int)visionProviders.Count; i++)
						{
							if (visionProviders[i].m_actorIndex == base.Caster.ActorIndex)
							{
								if (visionProviders[i].m_satelliteIndex < 0)
								{
									if (visionProviders[i].m_radius > num)
									{
										num = visionProviders[i].m_radius;
									}
								}
							}
						}
					}
					bool flag4 = this.m_largePingTimer.TickTimer(GameTime.deltaTime);
					bool flag5 = this.m_markerPingTimer.TickTimer(GameTime.deltaTime);
					bool flag6 = false;
					int j = 0;
					while (j < this.m_actorsToProcess.Count)
					{
						ActorData actorData = this.m_actorsToProcess[j];
						BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
						if (actorData.IsDead() || !(currentBoardSquare != null))
						{
							goto IL_293;
						}
						if (actorData.IsModelAnimatorDisabled())
						{
							goto IL_293;
						}
						bool flag7 = actorData.IsVisibleToClient();
						IL_294:
						bool flag8 = flag7;
						if (flag8)
						{
							float num2 = base.Caster.GetCurrentBoardSquare().HorizontalDistanceOnBoardTo(currentBoardSquare);
							if (num2 > num)
							{
								flag8 = false;
							}
						}
						AttachedActorVFXInfo attachedActorVFXInfo = this.m_actorToMarkerVfx[actorData];
						if (flag8)
						{
							if (attachedActorVFXInfo == null)
							{
								attachedActorVFXInfo = new AttachedActorVFXInfo(this.m_markerFxPrefab, actorData, this.m_markerFxJoint, false, "VisionMarker", AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
								attachedActorVFXInfo.SetCasterTeam(base.Caster.GetTeam());
								this.m_actorToMarkerVfx[actorData] = attachedActorVFXInfo;
							}
							attachedActorVFXInfo.UpdateVisibility(flag8, !flag3);
							if (flag5)
							{
								attachedActorVFXInfo.RestartEffects();
							}
							flag6 = true;
						}
						else if (attachedActorVFXInfo != null)
						{
							attachedActorVFXInfo.UpdateVisibility(false, false);
						}
						j++;
						continue;
						IL_293:
						flag7 = false;
						goto IL_294;
					}
					if (this.m_largePingFxInst != null)
					{
						this.m_largePingFxInst.SetActiveIfNeeded(true);
						if (base.IsActorConsideredVisible(base.Caster))
						{
							if (this.m_largePingFoFSelector != null)
							{
								this.m_largePingFoFSelector.Setup(base.Caster.GetTeam());
							}
							if (flag4)
							{
								foreach (PKFxFX pkfxFX in this.m_largePingFxInst.GetComponentsInChildren<PKFxFX>())
								{
									pkfxFX.TerminateEffect();
									pkfxFX.StartEffect();
									if (flag3)
									{
										this.PlayAudioEvent((!this.m_emittedLargePingThisTurn) ? this.m_audioEventFriendlyPing : this.m_audioEventFriendlyPingSubsequent, base.Caster.gameObject);
									}
									else
									{
										string audioEvent;
										if (this.m_emittedLargePingThisTurn)
										{
											audioEvent = this.m_audioEventEnemyPingSubsequent;
										}
										else
										{
											audioEvent = this.m_audioEventEnemyPing;
										}
										this.PlayAudioEvent(audioEvent, base.Caster.gameObject);
									}
								}
								this.m_emittedLargePingThisTurn = true;
							}
						}
					}
					if (flag6 && !this.m_playedMarkerSfxThisTurn)
					{
						this.m_playedMarkerSfxThisTurn = true;
						this.PlayAudioEvent(this.m_audioEventTargetReveal, base.Caster.gameObject);
					}
				}
				else if (this.m_canBeVisibleLastUpdate)
				{
					for (int l = 0; l < this.m_actorsToProcess.Count; l++)
					{
						ActorData key = this.m_actorsToProcess[l];
						AttachedActorVFXInfo attachedActorVFXInfo2 = this.m_actorToMarkerVfx[key];
						if (attachedActorVFXInfo2 != null)
						{
							attachedActorVFXInfo2.UpdateVisibility(false, false);
						}
					}
					if (this.m_largePingFxInst != null)
					{
						this.m_largePingFxInst.SetActiveIfNeeded(false);
					}
				}
				this.m_canBeVisibleLastUpdate = flag2;
				return;
			}
		}
	}

	private void PlayAudioEvent(string audioEvent, GameObject sourceObj)
	{
		if (!audioEvent.IsNullOrEmpty())
		{
			AudioManager.PostEvent(audioEvent, sourceObj);
		}
	}

	public class IntervalTimer
	{
		private float m_duration;

		private float m_timeTillEnd;

		public IntervalTimer(float duration, float initialDuration)
		{
			this.m_duration = duration;
			this.m_timeTillEnd = initialDuration;
		}

		public void ClearTimeTillEnd()
		{
			this.m_timeTillEnd = 0f;
		}

		public bool TickTimer(float dt)
		{
			bool result = false;
			this.m_timeTillEnd -= dt;
			if (this.m_timeTillEnd <= 0f)
			{
				this.m_timeTillEnd = this.m_duration;
				result = true;
			}
			return result;
		}
	}
}
