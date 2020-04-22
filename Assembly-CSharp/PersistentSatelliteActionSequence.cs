using System.Collections.Generic;
using UnityEngine;

public class PersistentSatelliteActionSequence : Sequence
{
	public enum SatelliteAction
	{
		Move,
		Attack,
		AltMove
	}

	public SatelliteAction m_action;

	public int m_satelliteIndex;

	[AnimEventPicker]
	public Object m_startActionEvent;

	[AnimEventPicker]
	public Object m_hitEvent;

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
		if (!(m_startActionEvent == null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ProcessAction();
			return;
		}
	}

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (extraSequenceParams is GenericActorListParam)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				GenericActorListParam genericActorListParam = extraSequenceParams as GenericActorListParam;
				m_actorToHitOnMoveEnd.AddRange(genericActorListParam.m_actors);
			}
		}
	}

	private bool Finished()
	{
		return m_hitDestination;
	}

	private void ProcessAction()
	{
		if (m_processedAction)
		{
			return;
		}
		m_processedAction = true;
		SatelliteController component = base.Caster.GetComponent<SatelliteController>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_persistentSatellite = component.GetSatellite(m_satelliteIndex);
			if (!(m_persistentSatellite != null))
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (m_action == SatelliteAction.Attack)
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
					if (base.Target != null)
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
						m_persistentSatellite.TriggerAttack(base.Target.gameObject);
						goto IL_0109;
					}
				}
				if (m_action == SatelliteAction.Move)
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
					m_persistentSatellite.MoveToPosition(base.TargetPos);
				}
				else if (m_action == SatelliteAction.AltMove)
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
					m_persistentSatellite.AltMoveToPosition(base.TargetPos);
				}
				goto IL_0109;
				IL_0109:
				m_lastBoardSquare = Board.Get().GetBoardSquare(m_persistentSatellite.transform.position);
				return;
			}
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (parameter == m_startActionEvent)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					ProcessAction();
					return;
				}
			}
		}
		if (!(parameter == m_hitEvent))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			HandleOnDestinationHits();
			return;
		}
	}

	private void HandleOnDestinationAudio()
	{
		if (m_handledAudioEventOnMoveFinish)
		{
			return;
		}
		if (!string.IsNullOrEmpty(m_audioEventOnMoveFinish))
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
			string audioEventOnMoveFinish = m_audioEventOnMoveFinish;
			object parentGameObject;
			if ((bool)base.Caster)
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
				parentGameObject = base.Caster.gameObject;
			}
			else
			{
				parentGameObject = null;
			}
			AudioManager.PostEvent(audioEventOnMoveFinish, (GameObject)parentGameObject);
		}
		m_handledAudioEventOnMoveFinish = true;
	}

	private void HandleOnDestinationHits()
	{
		if (m_hitDestination)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(0f, base.TargetPos);
			base.Source.OnSequenceHit(this, base.TargetPos, impulseInfo);
			if (m_actorToHitOnMoveEnd != null)
			{
				foreach (ActorData item in m_actorToHitOnMoveEnd)
				{
					base.Source.OnSequenceHit(this, item, null);
				}
			}
			m_hitDestination = true;
			HandleOnDestinationAudio();
			return;
		}
	}

	private void Update()
	{
		if (!m_processedAction)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(m_persistentSatellite != null))
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (m_action != 0)
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
					if (m_action != SatelliteAction.AltMove)
					{
						return;
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
				}
				if (m_hitDestination)
				{
					return;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					if (!m_persistentSatellite.IsMoving())
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
						if (m_hitEvent == null)
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
							HandleOnDestinationHits();
						}
					}
					if (!m_updateFogOfWarOnMovement)
					{
						return;
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						BoardSquare boardSquare = Board.Get().GetBoardSquare(m_persistentSatellite.transform.position);
						if (boardSquare != m_lastBoardSquare)
						{
							base.Caster.GetFogOfWar().MarkForRecalculateVisibility();
							m_lastBoardSquare = boardSquare;
						}
						return;
					}
				}
			}
		}
	}
}
