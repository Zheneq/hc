using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientActionBuffer : MonoBehaviour
{
	private static ClientActionBuffer s_instance;

	private ActionBufferPhase m_actionPhase;

	private AbilityPriority m_abilityPhase;

	private bool m_ignoreOldAbilityPhase;

	public static ClientActionBuffer Get()
	{
		return ClientActionBuffer.s_instance;
	}

	private void Awake()
	{
		ClientActionBuffer.s_instance = this;
		this.m_ignoreOldAbilityPhase = true;
		this.m_actionPhase = ActionBufferPhase.Done;
	}

	private void OnDestroy()
	{
		ClientActionBuffer.s_instance = null;
	}

	public ActionBufferPhase CurrentActionPhase
	{
		get
		{
			return this.m_actionPhase;
		}
		private set
		{
			if (this.m_actionPhase != value)
			{
				this.SetActionPhase(value);
			}
		}
	}

	private void SetActionPhase(ActionBufferPhase value)
	{
		ActionBufferPhase actionPhase = this.m_actionPhase;
		this.m_actionPhase = value;
		if (actionPhase != ActionBufferPhase.Abilities)
		{
			if (actionPhase != ActionBufferPhase.AbilitiesWait)
			{
				if (actionPhase != ActionBufferPhase.Done)
				{
					goto IL_76;
				}
			}
		}
		if (value == ActionBufferPhase.Movement || value == ActionBufferPhase.MovementChase || value == ActionBufferPhase.MovementWait)
		{
			GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedMovement, null);
			AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.MovementPhase);
			this.m_ignoreOldAbilityPhase = true;
			goto IL_E3;
		}
		IL_76:
		if (actionPhase != ActionBufferPhase.Done)
		{
			if (value == ActionBufferPhase.Done)
			{
				GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedDecision, null);
				this.m_ignoreOldAbilityPhase = true;
				goto IL_E3;
			}
		}
		if (value != ActionBufferPhase.Abilities)
		{
			if (value != ActionBufferPhase.AbilitiesWait)
			{
				goto IL_E3;
			}
		}
		if (actionPhase != ActionBufferPhase.AbilitiesWait)
		{
			if (actionPhase != ActionBufferPhase.Abilities)
			{
				this.SetAbilityPhase(this.m_abilityPhase);
			}
		}
		IL_E3:
		if (actionPhase != ActionBufferPhase.Abilities)
		{
			if (actionPhase != ActionBufferPhase.AbilitiesWait)
			{
				if (actionPhase != ActionBufferPhase.MovementWait)
				{
					goto IL_13D;
				}
			}
		}
		if (value == ActionBufferPhase.Done && TheatricsManager.Get() != null)
		{
			TheatricsManager.Get().SetAnimatorParamOnAllActors("DecisionPhase", true);
		}
		IL_13D:
		if (value != ActionBufferPhase.AbilitiesWait)
		{
			if (value != ActionBufferPhase.Movement)
			{
				if (value != ActionBufferPhase.MovementChase)
				{
					goto IL_188;
				}
			}
		}
		if (actionPhase == ActionBufferPhase.Abilities)
		{
			CameraManager.Get().SetTargetForMovementIfNeeded();
		}
		CameraManager.Get().SwitchCameraForMovement();
		IL_188:
		if (value != ActionBufferPhase.AbilitiesWait)
		{
			if (value != ActionBufferPhase.MovementWait)
			{
				return;
			}
		}
		if (GameplayMutators.Get() != null && GameFlowData.Get().HasPotentialGameMutatorVisibilityChanges(false))
		{
			if (FogOfWar.GetClientFog() != null)
			{
				FogOfWar.GetClientFog().UpdateVisibilityOfSquares(true);
			}
		}
	}

	public AbilityPriority AbilityPhase
	{
		get
		{
			return this.m_abilityPhase;
		}
		private set
		{
			if (this.m_abilityPhase != value)
			{
				this.SetAbilityPhase(value);
				SequenceManager.Get().OnAbilityPhaseStart(this.m_abilityPhase);
				ClientResolutionManager.Get().OnAbilityPhaseStart(this.m_abilityPhase);
				List<ActorData> actors = GameFlowData.Get().GetActors();
				using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						if (actorData != null)
						{
							actorData.CurrentlyVisibleForAbilityCast = false;
							actorData.MovedForEvade = false;
						}
					}
				}
				if (ClientAbilityResults.LogMissingSequences)
				{
					Log.Warning(string.Concat(new object[]
					{
						"On Ability Phase Start: <color=magenta>",
						this.m_abilityPhase.ToString(),
						"</color>\n@time = ",
						Time.time
					}), new object[0]);
				}
			}
		}
	}

	private void SetAbilityPhase(AbilityPriority value)
	{
		AbilityPriority abilityPhase = this.m_abilityPhase;
		this.m_abilityPhase = value;
		if (this.m_actionPhase == ActionBufferPhase.Abilities)
		{
			if (value != AbilityPriority.Prep_Defense)
			{
				if (value != AbilityPriority.Prep_Offense)
				{
					goto IL_DB;
				}
			}
			if (!this.m_ignoreOldAbilityPhase)
			{
				if (abilityPhase == AbilityPriority.Prep_Defense || abilityPhase == AbilityPriority.Prep_Offense)
				{
					goto IL_DB;
				}
				if (abilityPhase == AbilityPriority.Evasion)
				{
					goto IL_DB;
				}
				if (abilityPhase == AbilityPriority.DEPRICATED_Combat_Charge)
				{
					goto IL_DB;
				}
				if (abilityPhase == AbilityPriority.Combat_Damage)
				{
					goto IL_DB;
				}
				if (abilityPhase == AbilityPriority.Combat_Final)
				{
					goto IL_DB;
				}
				if (abilityPhase == AbilityPriority.Combat_Knockback)
				{
					goto IL_DB;
				}
			}
			this.m_ignoreOldAbilityPhase = false;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedPrep, null);
			AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.PrepPhase);
			return;
			IL_DB:
			if (value == AbilityPriority.Evasion)
			{
				if (!this.m_ignoreOldAbilityPhase)
				{
					if (abilityPhase == AbilityPriority.Evasion)
					{
						goto IL_135;
					}
				}
				this.m_ignoreOldAbilityPhase = false;
				GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedEvasion, null);
				AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.DashPhase);
				return;
			}
			IL_135:
			if (value != AbilityPriority.DEPRICATED_Combat_Charge)
			{
				if (value != AbilityPriority.Combat_Damage)
				{
					if (value != AbilityPriority.Combat_Final)
					{
						if (value != AbilityPriority.Combat_Knockback)
						{
							return;
						}
					}
				}
			}
			if (!this.m_ignoreOldAbilityPhase)
			{
				if (abilityPhase == AbilityPriority.DEPRICATED_Combat_Charge || abilityPhase == AbilityPriority.Combat_Damage)
				{
					return;
				}
				if (abilityPhase == AbilityPriority.Combat_Final)
				{
					return;
				}
				if (abilityPhase == AbilityPriority.Combat_Knockback)
				{
					return;
				}
			}
			this.m_ignoreOldAbilityPhase = false;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedCombat, null);
			AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.BlastPhase);
		}
	}

	public void SetDataFromShared(ActionBufferPhase actionPhase, AbilityPriority abilityPhase)
	{
		this.CurrentActionPhase = actionPhase;
		this.AbilityPhase = abilityPhase;
	}
}
