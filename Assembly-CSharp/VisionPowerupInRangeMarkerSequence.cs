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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(VisionPowerupInRangeMarkerSequence.FinishSetup()).MethodHandle;
			}
			List<ActorData> actors = GameFlowData.Get().GetActors();
			for (int i = 0; i < actors.Count; i++)
			{
				ActorData actorData = actors[i];
				if (actorData.\u000E() != base.Caster.\u000E())
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
					this.m_actorToMarkerVfx[actorData] = null;
					this.m_actorsToProcess.Add(actorData);
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_largePingFxPrefab != null)
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
				this.m_markerFxJoint.Initialize(base.Caster.gameObject);
				this.m_largePingFxInst = base.InstantiateFX(this.m_largePingFxPrefab);
				this.m_largePingFxInst.transform.parent = this.m_markerFxJoint.m_jointObject.transform;
				this.m_largePingFxInst.transform.localPosition = Vector3.zero;
				this.m_largePingFxInst.transform.localRotation = Quaternion.identity;
				this.m_largePingFoFSelector = this.m_largePingFxInst.GetComponent<FriendlyEnemyVFXSelector>();
				if (this.m_largePingFoFSelector != null)
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
					this.m_largePingFoFSelector.Setup(base.Caster.\u000E());
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(VisionPowerupInRangeMarkerSequence.OnDisable()).MethodHandle;
				}
				attachedActorVFXInfo.DestroyVfx();
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(VisionPowerupInRangeMarkerSequence.Update()).MethodHandle;
			}
			if (!(GameFlowData.Get() == null))
			{
				bool flag;
				if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
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
					if (base.Caster != null)
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
						if (!base.Caster.\u000E())
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
							if (base.Caster.\u0012() != null)
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
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					bool flag3 = false;
					if (GameFlowData.Get().activeOwnedActorData != null)
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
						if (GameFlowData.Get().activeOwnedActorData.\u000E() == base.Caster.\u000E())
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
							flag3 = true;
						}
					}
					float num = base.Caster.\u0015();
					ActorAdditionalVisionProviders actorAdditionalVisionProviders = base.Caster.\u000E();
					if (actorAdditionalVisionProviders != null)
					{
						SyncListVisionProviderInfo visionProviders = actorAdditionalVisionProviders.GetVisionProviders();
						for (int i = 0; i < (int)visionProviders.Count; i++)
						{
							if (visionProviders[i].m_actorIndex == base.Caster.ActorIndex)
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
								if (visionProviders[i].m_satelliteIndex < 0)
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
									if (visionProviders[i].m_radius > num)
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
										num = visionProviders[i].m_radius;
									}
								}
							}
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
					bool flag4 = this.m_largePingTimer.TickTimer(GameTime.deltaTime);
					bool flag5 = this.m_markerPingTimer.TickTimer(GameTime.deltaTime);
					bool flag6 = false;
					int j = 0;
					while (j < this.m_actorsToProcess.Count)
					{
						ActorData actorData = this.m_actorsToProcess[j];
						BoardSquare boardSquare = actorData.\u0012();
						if (actorData.\u000E() || !(boardSquare != null))
						{
							goto IL_293;
						}
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (actorData.\u0012())
						{
							goto IL_293;
						}
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						bool flag7 = actorData.\u0018();
						IL_294:
						bool flag8 = flag7;
						if (flag8)
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
							float num2 = base.Caster.\u0012().HorizontalDistanceOnBoardTo(boardSquare);
							if (num2 > num)
							{
								flag8 = false;
							}
						}
						AttachedActorVFXInfo attachedActorVFXInfo = this.m_actorToMarkerVfx[actorData];
						if (flag8)
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
							if (attachedActorVFXInfo == null)
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
								attachedActorVFXInfo = new AttachedActorVFXInfo(this.m_markerFxPrefab, actorData, this.m_markerFxJoint, false, "VisionMarker", AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
								attachedActorVFXInfo.SetCasterTeam(base.Caster.\u000E());
								this.m_actorToMarkerVfx[actorData] = attachedActorVFXInfo;
							}
							attachedActorVFXInfo.UpdateVisibility(flag8, !flag3);
							if (flag5)
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
								attachedActorVFXInfo.RestartEffects();
							}
							flag6 = true;
						}
						else if (attachedActorVFXInfo != null)
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
							attachedActorVFXInfo.UpdateVisibility(false, false);
						}
						j++;
						continue;
						IL_293:
						flag7 = false;
						goto IL_294;
					}
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_largePingFxInst != null)
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
						this.m_largePingFxInst.SetActiveIfNeeded(true);
						if (base.IsActorConsideredVisible(base.Caster))
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
							if (this.m_largePingFoFSelector != null)
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
								this.m_largePingFoFSelector.Setup(base.Caster.\u000E());
							}
							if (flag4)
							{
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								foreach (PKFxFX pkfxFX in this.m_largePingFxInst.GetComponentsInChildren<PKFxFX>())
								{
									pkfxFX.TerminateEffect();
									pkfxFX.StartEffect();
									if (flag3)
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
										this.PlayAudioEvent((!this.m_emittedLargePingThisTurn) ? this.m_audioEventFriendlyPing : this.m_audioEventFriendlyPingSubsequent, base.Caster.gameObject);
									}
									else
									{
										string audioEvent;
										if (this.m_emittedLargePingThisTurn)
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
											audioEvent = this.m_audioEventEnemyPingSubsequent;
										}
										else
										{
											audioEvent = this.m_audioEventEnemyPing;
										}
										this.PlayAudioEvent(audioEvent, base.Caster.gameObject);
									}
								}
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
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
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					for (int l = 0; l < this.m_actorsToProcess.Count; l++)
					{
						ActorData key = this.m_actorsToProcess[l];
						AttachedActorVFXInfo attachedActorVFXInfo2 = this.m_actorToMarkerVfx[key];
						if (attachedActorVFXInfo2 != null)
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
							attachedActorVFXInfo2.UpdateVisibility(false, false);
						}
					}
					if (this.m_largePingFxInst != null)
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(VisionPowerupInRangeMarkerSequence.PlayAudioEvent(string, GameObject)).MethodHandle;
			}
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
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(VisionPowerupInRangeMarkerSequence.IntervalTimer.TickTimer(float)).MethodHandle;
				}
				this.m_timeTillEnd = this.m_duration;
				result = true;
			}
			return result;
		}
	}
}
