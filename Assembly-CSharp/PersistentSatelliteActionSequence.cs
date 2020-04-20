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
				this.m_persistentSatellite = component.GetSatellite(this.m_satelliteIndex);
				if (this.m_persistentSatellite != null)
				{
					if (this.m_action == PersistentSatelliteActionSequence.SatelliteAction.Attack)
					{
						if (base.Target != null)
						{
							this.m_persistentSatellite.TriggerAttack(base.Target.gameObject);
							goto IL_109;
						}
					}
					if (this.m_action == PersistentSatelliteActionSequence.SatelliteAction.Move)
					{
						this.m_persistentSatellite.MoveToPosition(base.TargetPos, PersistentSatellite.SatelliteMoveStartType.Normal);
					}
					else if (this.m_action == PersistentSatelliteActionSequence.SatelliteAction.AltMove)
					{
						this.m_persistentSatellite.AltMoveToPosition(base.TargetPos);
					}
					IL_109:
					this.m_lastBoardSquare = Board.Get().GetBoardSquare(this.m_persistentSatellite.transform.position);
				}
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (parameter == this.m_startActionEvent)
		{
			this.ProcessAction();
		}
		else if (parameter == this.m_hitEvent)
		{
			this.HandleOnDestinationHits();
		}
	}

	private void HandleOnDestinationAudio()
	{
		if (!this.m_handledAudioEventOnMoveFinish)
		{
			if (!string.IsNullOrEmpty(this.m_audioEventOnMoveFinish))
			{
				string audioEventOnMoveFinish = this.m_audioEventOnMoveFinish;
				GameObject parentGameObject;
				if (base.Caster)
				{
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
			if (this.m_persistentSatellite != null)
			{
				if (this.m_action != PersistentSatelliteActionSequence.SatelliteAction.Move)
				{
					if (this.m_action != PersistentSatelliteActionSequence.SatelliteAction.AltMove)
					{
						return;
					}
				}
				if (!this.m_hitDestination)
				{
					if (!this.m_persistentSatellite.IsMoving())
					{
						if (this.m_hitEvent == null)
						{
							this.HandleOnDestinationHits();
						}
					}
					if (this.m_updateFogOfWarOnMovement)
					{
						BoardSquare boardSquare = Board.Get().GetBoardSquare(this.m_persistentSatellite.transform.position);
						if (boardSquare != this.m_lastBoardSquare)
						{
							base.Caster.GetFogOfWar().MarkForRecalculateVisibility();
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
