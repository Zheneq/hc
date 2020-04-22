using System.Collections.Generic;
using UnityEngine;

public class ClientActionBuffer : MonoBehaviour
{
	private static ClientActionBuffer s_instance;

	private ActionBufferPhase m_actionPhase;

	private AbilityPriority m_abilityPhase;

	private bool m_ignoreOldAbilityPhase;

	public ActionBufferPhase CurrentActionPhase
	{
		get
		{
			return m_actionPhase;
		}
		private set
		{
			if (m_actionPhase == value)
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
				SetActionPhase(value);
				return;
			}
		}
	}

	public AbilityPriority AbilityPhase
	{
		get
		{
			return m_abilityPhase;
		}
		private set
		{
			if (m_abilityPhase == value)
			{
				return;
			}
			SetAbilityPhase(value);
			SequenceManager.Get().OnAbilityPhaseStart(m_abilityPhase);
			ClientResolutionManager.Get().OnAbilityPhaseStart(m_abilityPhase);
			List<ActorData> actors = GameFlowData.Get().GetActors();
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (current != null)
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						current.CurrentlyVisibleForAbilityCast = false;
						current.MovedForEvade = false;
					}
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (!ClientAbilityResults.LogMissingSequences)
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
				Log.Warning("On Ability Phase Start: <color=magenta>" + m_abilityPhase.ToString() + "</color>\n@time = " + Time.time);
				return;
			}
		}
	}

	public static ClientActionBuffer Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		m_ignoreOldAbilityPhase = true;
		m_actionPhase = ActionBufferPhase.Done;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void SetActionPhase(ActionBufferPhase value)
	{
		ActionBufferPhase actionPhase = m_actionPhase;
		m_actionPhase = value;
		if (actionPhase != 0)
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
			if (actionPhase != ActionBufferPhase.AbilitiesWait)
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
				if (actionPhase != ActionBufferPhase.Done)
				{
					goto IL_0076;
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
			}
		}
		if (value != ActionBufferPhase.Movement && value != ActionBufferPhase.MovementChase && value != ActionBufferPhase.MovementWait)
		{
			goto IL_0076;
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedMovement, null);
		AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.MovementPhase);
		m_ignoreOldAbilityPhase = true;
		goto IL_00e3;
		IL_0076:
		if (actionPhase != ActionBufferPhase.Done)
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
			if (value == ActionBufferPhase.Done)
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
				GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedDecision, null);
				m_ignoreOldAbilityPhase = true;
				goto IL_00e3;
			}
		}
		if (value != 0)
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
			if (value != ActionBufferPhase.AbilitiesWait)
			{
				goto IL_00e3;
			}
		}
		if (actionPhase != ActionBufferPhase.AbilitiesWait)
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
			if (actionPhase != 0)
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
				SetAbilityPhase(m_abilityPhase);
			}
		}
		goto IL_00e3;
		IL_0188:
		if (value != ActionBufferPhase.AbilitiesWait)
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
			if (value != ActionBufferPhase.MovementWait)
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (!(GameplayMutators.Get() != null) || !GameFlowData.Get().HasPotentialGameMutatorVisibilityChanges(false))
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
			if (FogOfWar.GetClientFog() != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					FogOfWar.GetClientFog().UpdateVisibilityOfSquares();
					return;
				}
			}
			return;
		}
		IL_00e3:
		if (actionPhase != 0)
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
			if (actionPhase != ActionBufferPhase.AbilitiesWait)
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
				if (actionPhase != ActionBufferPhase.MovementWait)
				{
					goto IL_013d;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (value == ActionBufferPhase.Done && TheatricsManager.Get() != null)
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
			TheatricsManager.Get().SetAnimatorParamOnAllActors("DecisionPhase", true);
		}
		goto IL_013d;
		IL_013d:
		if (value != ActionBufferPhase.AbilitiesWait)
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
			if (value != ActionBufferPhase.Movement)
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
				if (value != ActionBufferPhase.MovementChase)
				{
					goto IL_0188;
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
			}
		}
		if (actionPhase == ActionBufferPhase.Abilities)
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
			CameraManager.Get().SetTargetForMovementIfNeeded();
		}
		CameraManager.Get().SwitchCameraForMovement();
		goto IL_0188;
	}

	private void SetAbilityPhase(AbilityPriority value)
	{
		AbilityPriority abilityPhase = m_abilityPhase;
		m_abilityPhase = value;
		if (m_actionPhase != 0)
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
			if (value != 0)
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
				if (value != AbilityPriority.Prep_Offense)
				{
					goto IL_00db;
				}
			}
			if (m_ignoreOldAbilityPhase)
			{
				goto IL_00af;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (abilityPhase != 0 && abilityPhase != AbilityPriority.Prep_Offense)
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
				if (abilityPhase != AbilityPriority.Evasion)
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
					if (abilityPhase != AbilityPriority.DEPRICATED_Combat_Charge)
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
						if (abilityPhase != AbilityPriority.Combat_Damage)
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
							if (abilityPhase != AbilityPriority.Combat_Final)
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
								if (abilityPhase != AbilityPriority.Combat_Knockback)
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
									goto IL_00af;
								}
							}
						}
					}
				}
			}
			goto IL_00db;
			IL_00db:
			if (value == AbilityPriority.Evasion)
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
				if (!m_ignoreOldAbilityPhase)
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
					if (abilityPhase == AbilityPriority.Evasion)
					{
						goto IL_0135;
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
				}
				m_ignoreOldAbilityPhase = false;
				GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedEvasion, null);
				AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.DashPhase);
				return;
			}
			goto IL_0135;
			IL_0135:
			if (value != AbilityPriority.DEPRICATED_Combat_Charge)
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
				if (value != AbilityPriority.Combat_Damage)
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
					if (value != AbilityPriority.Combat_Final)
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
						if (value != AbilityPriority.Combat_Knockback)
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
				}
			}
			if (!m_ignoreOldAbilityPhase)
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
				if (abilityPhase == AbilityPriority.DEPRICATED_Combat_Charge || abilityPhase == AbilityPriority.Combat_Damage)
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
					break;
				}
				if (abilityPhase == AbilityPriority.Combat_Final)
				{
					return;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (abilityPhase == AbilityPriority.Combat_Knockback)
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
					break;
				}
			}
			m_ignoreOldAbilityPhase = false;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedCombat, null);
			AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.BlastPhase);
			return;
			IL_00af:
			m_ignoreOldAbilityPhase = false;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedPrep, null);
			AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.PrepPhase);
			return;
		}
	}

	public void SetDataFromShared(ActionBufferPhase actionPhase, AbilityPriority abilityPhase)
	{
		CurrentActionPhase = actionPhase;
		AbilityPhase = abilityPhase;
	}
}
