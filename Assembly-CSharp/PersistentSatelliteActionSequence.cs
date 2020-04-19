using System;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSatelliteActionSequence : Sequence
{
	public PersistentSatelliteActionSequence.SatelliteAction m_action;

	public int m_satelliteIndex;

	[AnimEventPicker]
	public UnityEngine.Object m_startActionEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_hitEvent;

	public bool m_updateFogOfWarOnMovement;

	[Space(10f)]
	[AudioEvent(false)]
	public string m_audioEventOnMoveFinish = string.Empty;

	private bool m_processedAction;

	private bool m_hitDestination;

	private PersistentSatellite m_persistentSatellite;

	private BoardSquare m_lastBoardSquare;

	private bool m_handledAudioEventOnMoveFinish;

	private List<ActorData> m_actorToHitOnMoveEnd = new List<ActorData>();

	public override void FinishSetup()
	{
		if (this.m_startActionEvent == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PersistentSatelliteActionSequence.FinishSetup()).MethodHandle;
			}
			this.ProcessAction();
		}
	}

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (extraSequenceParams is Sequence.GenericActorListParam)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(PersistentSatelliteActionSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				Sequence.GenericActorListParam genericActorListParam = extraSequenceParams as Sequence.GenericActorListParam;
				this.m_actorToHitOnMoveEnd.AddRange(genericActorListParam.m_actors);
			}
		}
	}

	private bool Finished()
	{
		return this.m_hitDestination;
	}

	private void ProcessAction()
	{
		if (!this.m_processedAction)
		{
			this.m_processedAction = true;
			SatelliteController component = base.Caster.GetComponent<SatelliteController>();
			if (component != null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(PersistentSatelliteActionSequence.ProcessAction()).MethodHandle;
				}
				this.m_persistentSatellite = component.GetSatellite(this.m_satelliteIndex);
				if (this.m_persistentSatellite != null)
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
					if (this.m_action == PersistentSatelliteActionSequence.SatelliteAction.Attack)
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
						if (base.Target != null)
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
							this.m_persistentSatellite.TriggerAttack(base.Target.gameObject);
							goto IL_109;
						}
					}
					if (this.m_action == PersistentSatelliteActionSequence.SatelliteAction.Move)
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
						this.m_persistentSatellite.MoveToPosition(base.TargetPos, PersistentSatellite.SatelliteMoveStartType.Normal);
					}
					else if (this.m_action == PersistentSatelliteActionSequence.SatelliteAction.AltMove)
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
						this.m_persistentSatellite.AltMoveToPosition(base.TargetPos);
					}
					IL_109:
					this.m_lastBoardSquare = Board.\u000E().\u000E(this.m_persistentSatellite.transform.position);
				}
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (parameter == this.m_startActionEvent)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PersistentSatelliteActionSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.ProcessAction();
		}
		else if (parameter == this.m_hitEvent)
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
			this.HandleOnDestinationHits();
		}
	}

	private void HandleOnDestinationAudio()
	{
		if (!this.m_handledAudioEventOnMoveFinish)
		{
			if (!string.IsNullOrEmpty(this.m_audioEventOnMoveFinish))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(PersistentSatelliteActionSequence.HandleOnDestinationAudio()).MethodHandle;
				}
				string audioEventOnMoveFinish = this.m_audioEventOnMoveFinish;
				GameObject parentGameObject;
				if (base.Caster)
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
					parentGameObject = base.Caster.gameObject;
				}
				else
				{
					parentGameObject = null;
				}
				AudioManager.PostEvent(audioEventOnMoveFinish, parentGameObject);
			}
			this.m_handledAudioEventOnMoveFinish = true;
		}
	}

	private void HandleOnDestinationHits()
	{
		if (!this.m_hitDestination)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PersistentSatelliteActionSequence.HandleOnDestinationHits()).MethodHandle;
			}
			ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(0f, base.TargetPos);
			base.Source.OnSequenceHit(this, base.TargetPos, impulseInfo);
			if (this.m_actorToHitOnMoveEnd != null)
			{
				foreach (ActorData target in this.m_actorToHitOnMoveEnd)
				{
					base.Source.OnSequenceHit(this, target, null, ActorModelData.RagdollActivation.HealthBased, true);
				}
			}
			this.m_hitDestination = true;
			this.HandleOnDestinationAudio();
		}
	}

	private void Update()
	{
		if (this.m_processedAction)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PersistentSatelliteActionSequence.Update()).MethodHandle;
			}
			if (this.m_persistentSatellite != null)
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
				if (this.m_action != PersistentSatelliteActionSequence.SatelliteAction.Move)
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
					if (this.m_action != PersistentSatelliteActionSequence.SatelliteAction.AltMove)
					{
						return;
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
				if (!this.m_hitDestination)
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
					if (!this.m_persistentSatellite.IsMoving())
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
						if (this.m_hitEvent == null)
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
							this.HandleOnDestinationHits();
						}
					}
					if (this.m_updateFogOfWarOnMovement)
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
						BoardSquare boardSquare = Board.\u000E().\u000E(this.m_persistentSatellite.transform.position);
						if (boardSquare != this.m_lastBoardSquare)
						{
							base.Caster.\u000E().MarkForRecalculateVisibility();
							this.m_lastBoardSquare = boardSquare;
						}
					}
				}
			}
		}
	}

	public enum SatelliteAction
	{
		Move,
		Attack,
		AltMove
	}
}
