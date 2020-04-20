using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVFXAtTargetPosSequence : Sequence
{
	[Separator("FX To Spawn", true)]
	public GameObject m_fxPrefab;

	[Separator("Height", true)]
	public float m_yOffset;

	public bool m_useGroundHeight;

	private GameObject m_fx;

	private FriendlyEnemyVFXSelector m_fxFoFSelectComp;

	[Separator("Start delay time (ignored if has Start Event)", true)]
	public float m_startDelayTime;

	private float m_timeToSpawnVfx = -1f;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[Separator("Anim Events -- ( start / stop )", "orange")]
	public UnityEngine.Object m_startEvent;

	private bool m_ignoreStartEvent;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before stopping the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_stopEvent;

	[Separator("Gameplay Hits", true)]
	public bool m_callOnHitForGameplay;

	public float m_hitDelayTime;

	private float m_timeToHit = -1f;

	private bool m_sequenceHitCalled;

	[Separator("Audio Event -- ( on FX spawn )", "orange")]
	[AudioEvent(false)]
	public string m_audioEvent;

	[Separator("Phase-Based Timing", true)]
	public Sequence.PhaseTimingParameters m_phaseTimingParameters;

	[Separator("Special case handler for additional VFX at target position", true)]
	public AdditionalVfxContainerBase m_additionalFxAtTargetPos;

	private int m_initialTimerControllerValue;

	private int m_timeControllerValueNow = -0x64;

	private Dictionary<string, float> m_fxAttributes = new Dictionary<string, float>();

	private Vector3 m_fxSpawnPosition;

	internal override Vector3 GetSequencePos()
	{
		if (this.m_fx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVFXAtTargetPosSequence.GetSequencePos()).MethodHandle;
			}
			return this.m_fx.transform.position;
		}
		return Vector3.zero;
	}

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		this.m_fxSpawnPosition = base.TargetPos;
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			base.OverridePhaseTimingParams(this.m_phaseTimingParameters, extraSequenceParams);
			SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam ignoreStartEventExtraParam = extraSequenceParams as SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam;
			if (ignoreStartEventExtraParam != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVFXAtTargetPosSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				this.m_ignoreStartEvent = ignoreStartEventExtraParam.ignoreStartEvent;
			}
			if (extraSequenceParams is SimpleVFXAtTargetPosSequence.PositionOverrideParam)
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
				SimpleVFXAtTargetPosSequence.PositionOverrideParam positionOverrideParam = extraSequenceParams as SimpleVFXAtTargetPosSequence.PositionOverrideParam;
				this.m_fxSpawnPosition = positionOverrideParam.m_positionOverride;
			}
			if (extraSequenceParams is Sequence.FxAttributeParam)
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
				Sequence.FxAttributeParam fxAttributeParam = extraSequenceParams as Sequence.FxAttributeParam;
				if (fxAttributeParam != null)
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
					if (fxAttributeParam.m_paramNameCode != Sequence.FxAttributeParam.ParamNameCode.None)
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
						string attributeName = fxAttributeParam.GetAttributeName();
						float paramValue = fxAttributeParam.m_paramValue;
						if (fxAttributeParam.m_paramTarget == Sequence.FxAttributeParam.ParamTarget.MainVfx)
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
							if (!this.m_fxAttributes.ContainsKey(attributeName))
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
								this.m_fxAttributes.Add(attributeName, paramValue);
							}
						}
					}
				}
			}
		}
		if (this.m_additionalFxAtTargetPos != null)
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
			this.m_additionalFxAtTargetPos.Initialize(this);
		}
	}

	public override void FinishSetup()
	{
		if (!(this.m_startEvent == null))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVFXAtTargetPosSequence.FinishSetup()).MethodHandle;
			}
			if (!this.m_ignoreStartEvent)
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
				if (!ClientGameManager.Get().IsFastForward)
				{
					return;
				}
			}
		}
		if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
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
			if (this.m_startDelayTime <= 0f)
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
				this.SpawnFX();
			}
			else
			{
				this.m_timeToSpawnVfx = GameTime.time + this.m_startDelayTime;
			}
		}
	}

	internal override void OnTurnStart(int currentTurn)
	{
		this.m_phaseTimingParameters.OnTurnStart(currentTurn);
	}

	internal override void OnAbilityPhaseStart(AbilityPriority abilityPhase)
	{
		this.m_phaseTimingParameters.OnAbilityPhaseStart(abilityPhase);
	}

	internal override void SetTimerController(int value)
	{
		if (this.m_fx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVFXAtTargetPosSequence.SetTimerController(int)).MethodHandle;
			}
			if (value != this.m_timeControllerValueNow)
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
				this.m_timeControllerValueNow = value;
				if (value >= 4)
				{
					Sequence.SetAttribute(this.m_fx, "timerControl01", 0);
					Sequence.SetAttribute(this.m_fx, "timerControl02", 0);
					Sequence.SetAttribute(this.m_fx, "timerControl03", 0);
					Sequence.SetAttribute(this.m_fx, "timerControl04", 0);
				}
				else if (value == 3)
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
					Sequence.SetAttribute(this.m_fx, "timerControl01", 0);
					Sequence.SetAttribute(this.m_fx, "timerControl02", 0);
					Sequence.SetAttribute(this.m_fx, "timerControl03", 0);
					Sequence.SetAttribute(this.m_fx, "timerControl04", 1);
				}
				else if (value == 2)
				{
					Sequence.SetAttribute(this.m_fx, "timerControl01", 0);
					Sequence.SetAttribute(this.m_fx, "timerControl02", 0);
					Sequence.SetAttribute(this.m_fx, "timerControl03", 1);
					Sequence.SetAttribute(this.m_fx, "timerControl04", 1);
				}
				else if (value == 1)
				{
					Sequence.SetAttribute(this.m_fx, "timerControl01", 0);
					Sequence.SetAttribute(this.m_fx, "timerControl02", 1);
					Sequence.SetAttribute(this.m_fx, "timerControl03", 1);
					Sequence.SetAttribute(this.m_fx, "timerControl04", 1);
				}
				else if (value <= 0)
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
					Sequence.SetAttribute(this.m_fx, "timerControl01", 1);
					Sequence.SetAttribute(this.m_fx, "timerControl02", 1);
					Sequence.SetAttribute(this.m_fx, "timerControl03", 1);
					Sequence.SetAttribute(this.m_fx, "timerControl04", 1);
				}
			}
		}
		else
		{
			this.m_initialTimerControllerValue = value;
		}
	}

	private void SpawnFX()
	{
		if (this.m_fxPrefab)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVFXAtTargetPosSequence.SpawnFX()).MethodHandle;
			}
			Vector3 fxSpawnPosition = this.m_fxSpawnPosition;
			if (this.m_useGroundHeight)
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
				fxSpawnPosition.y = (float)Board.Get().BaselineHeight;
			}
			fxSpawnPosition.y += this.m_yOffset;
			Quaternion targetRotation = base.TargetRotation;
			this.m_fx = base.InstantiateFX(this.m_fxPrefab, fxSpawnPosition, targetRotation, true, true);
			this.SetTimerController(this.m_initialTimerControllerValue);
			this.m_fxFoFSelectComp = this.m_fx.GetComponent<FriendlyEnemyVFXSelector>();
			if (this.m_fxFoFSelectComp != null)
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
				if (base.Caster != null)
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
					this.m_fxFoFSelectComp.Setup(base.Caster.GetTeam());
				}
			}
			if (!this.m_sequenceHitCalled && this.m_callOnHitForGameplay)
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
				if (this.m_hitDelayTime > 0f && this.m_timeToHit < 0f)
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
					this.m_timeToHit = GameTime.time + this.m_hitDelayTime;
				}
				else if (this.m_hitDelayTime <= 0f)
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
					base.CallHitSequenceOnTargets(base.TargetPos, 1f, null, true);
					this.m_sequenceHitCalled = true;
				}
			}
			if (this.m_fx != null && this.m_fxAttributes != null)
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
				using (Dictionary<string, float>.Enumerator enumerator = this.m_fxAttributes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, float> keyValuePair = enumerator.Current;
						Sequence.SetAttribute(this.m_fx, keyValuePair.Key, keyValuePair.Value);
					}
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			if (this.m_fx != null && this.m_additionalFxAtTargetPos != null)
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
				this.m_additionalFxAtTargetPos.SpawnFX(this.m_fx.transform.position, this.m_fx.transform.rotation, this);
			}
		}
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			GameObject gameObject = null;
			if (this.m_fx != null)
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
				gameObject = this.m_fx;
			}
			else if (base.Caster != null)
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
				gameObject = base.Caster.gameObject;
			}
			if (gameObject != null)
			{
				AudioManager.PostEvent(this.m_audioEvent, gameObject);
			}
		}
	}

	private void StopFX()
	{
		if (this.m_fx != null)
		{
			this.m_fx.SetActive(false);
		}
		if (this.m_additionalFxAtTargetPos)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVFXAtTargetPosSequence.StopFX()).MethodHandle;
			}
			this.m_additionalFxAtTargetPos.SetAsInactive();
		}
	}

	private void Update()
	{
		this.OnUpdate();
	}

	protected virtual void OnUpdate()
	{
		if (this.m_initialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVFXAtTargetPosSequence.OnUpdate()).MethodHandle;
			}
			if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
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
				if (this.m_timeToSpawnVfx > 0f)
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
					if (GameTime.time >= this.m_timeToSpawnVfx)
					{
						this.m_timeToSpawnVfx = -1f;
						this.SpawnFX();
					}
				}
				if (this.m_callOnHitForGameplay)
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
					if (!this.m_sequenceHitCalled)
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
						if (this.m_initialized)
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
							if (!(this.m_fxPrefab == null))
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
								if (this.m_timeToHit <= 0f)
								{
									goto IL_113;
								}
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (GameTime.time < this.m_timeToHit)
								{
									goto IL_113;
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
							}
							base.CallHitSequenceOnTargets(base.TargetPos, 1f, null, true);
							this.m_sequenceHitCalled = true;
						}
					}
				}
				IL_113:
				if (this.m_fx != null)
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
					if (this.m_fxFoFSelectComp != null)
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
						if (base.Caster != null)
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
							this.m_fxFoFSelectComp.Setup(base.Caster.GetTeam());
						}
					}
				}
				base.ProcessSequenceVisibility();
				if (this.m_additionalFxAtTargetPos != null)
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
					this.m_additionalFxAtTargetPos.OnUpdate(base.LastDesiredVisible(), base.Caster);
				}
			}
			else
			{
				base.SetSequenceVisibility(false);
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_phaseTimingParameters.ShouldSequenceBeActive())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVFXAtTargetPosSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			if (this.m_startEvent == parameter)
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
				this.SpawnFX();
			}
			else if (this.m_stopEvent == parameter)
			{
				this.StopFX();
			}
		}
	}

	private void OnDisable()
	{
		if (this.m_fx)
		{
			UnityEngine.Object.Destroy(this.m_fx.gameObject);
			this.m_fx = null;
		}
		if (this.m_additionalFxAtTargetPos != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVFXAtTargetPosSequence.OnDisable()).MethodHandle;
			}
			this.m_additionalFxAtTargetPos.DestroyFX();
		}
	}

	public override string GetVisibilityDescription()
	{
		return string.Empty;
	}

	public override string GetSequenceSpecificDescription()
	{
		string text = string.Empty;
		if (this.m_fxPrefab == null)
		{
			text += "<color=yellow>WARNING: </color>No VFX Prefab for field [Fx Prefab]\n\n";
		}
		if (this.m_callOnHitForGameplay)
		{
			text += "<color=cyan>Can do Gameplay Hits</color>\n";
			if (this.m_hitDelayTime > 0f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVFXAtTargetPosSequence.GetSequenceSpecificDescription()).MethodHandle;
				}
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"Gameplay Hit happens ",
					this.m_hitDelayTime,
					" second(s) after VFX start.\n\n"
				});
			}
		}
		else
		{
			text += "Ignoring Gameplay Hits\n";
		}
		if (this.m_startEvent != null)
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
			if (this.m_startDelayTime > 0f)
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
				text += "<color=yellow>WARNING: </color>Start Delay Time is ignored, will use StartEvent.\n\n";
			}
		}
		else if (this.m_startDelayTime > 0f)
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
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"Starts ",
				this.m_startDelayTime,
				" second(s) after sequence spawn."
			});
		}
		return text;
	}

	public class IgnoreStartEventExtraParam : Sequence.IExtraSequenceParams
	{
		public bool ignoreStartEvent;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.ignoreStartEvent);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.ignoreStartEvent);
		}
	}

	public class PositionOverrideParam : Sequence.IExtraSequenceParams
	{
		public Vector3 m_positionOverride;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.m_positionOverride);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.m_positionOverride);
		}
	}
}
