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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientActionBuffer.set_CurrentActionPhase(ActionBufferPhase)).MethodHandle;
				}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientActionBuffer.SetActionPhase(ActionBufferPhase)).MethodHandle;
			}
			if (actionPhase != ActionBufferPhase.AbilitiesWait)
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
				if (actionPhase != ActionBufferPhase.Done)
				{
					goto IL_76;
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
			for (;;)
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedDecision, null);
				this.m_ignoreOldAbilityPhase = true;
				goto IL_E3;
			}
		}
		if (value != ActionBufferPhase.Abilities)
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
			if (value != ActionBufferPhase.AbilitiesWait)
			{
				goto IL_E3;
			}
		}
		if (actionPhase != ActionBufferPhase.AbilitiesWait)
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
			if (actionPhase != ActionBufferPhase.Abilities)
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
				this.SetAbilityPhase(this.m_abilityPhase);
			}
		}
		IL_E3:
		if (actionPhase != ActionBufferPhase.Abilities)
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
			if (actionPhase != ActionBufferPhase.AbilitiesWait)
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
				if (actionPhase != ActionBufferPhase.MovementWait)
				{
					goto IL_13D;
				}
				for (;;)
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
			for (;;)
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
		IL_13D:
		if (value != ActionBufferPhase.AbilitiesWait)
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
			if (value != ActionBufferPhase.Movement)
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
				if (value != ActionBufferPhase.MovementChase)
				{
					goto IL_188;
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
			}
		}
		if (actionPhase == ActionBufferPhase.Abilities)
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
			CameraManager.Get().SetTargetForMovementIfNeeded();
		}
		CameraManager.Get().SwitchCameraForMovement();
		IL_188:
		if (value != ActionBufferPhase.AbilitiesWait)
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
			if (value != ActionBufferPhase.MovementWait)
			{
				return;
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
		if (GameplayMutators.Get() != null && GameFlowData.Get().HasPotentialGameMutatorVisibilityChanges(false))
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
			if (FogOfWar.GetClientFog() != null)
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
								RuntimeMethodHandle runtimeMethodHandle = methodof(ClientActionBuffer.set_AbilityPhase(AbilityPriority)).MethodHandle;
							}
							actorData.CurrentlyVisibleForAbilityCast = false;
							actorData.MovedForEvade = false;
						}
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
				if (ClientAbilityResults.\u001D)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientActionBuffer.SetAbilityPhase(AbilityPriority)).MethodHandle;
			}
			if (value != AbilityPriority.Prep_Defense)
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
				if (value != AbilityPriority.Prep_Offense)
				{
					goto IL_DB;
				}
			}
			if (!this.m_ignoreOldAbilityPhase)
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
				if (abilityPhase == AbilityPriority.Prep_Defense || abilityPhase == AbilityPriority.Prep_Offense)
				{
					goto IL_DB;
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
				if (abilityPhase == AbilityPriority.Evasion)
				{
					goto IL_DB;
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
				if (abilityPhase == AbilityPriority.DEPRICATED_Combat_Charge)
				{
					goto IL_DB;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (abilityPhase == AbilityPriority.Combat_Damage)
				{
					goto IL_DB;
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
				if (abilityPhase == AbilityPriority.Combat_Final)
				{
					goto IL_DB;
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
				if (abilityPhase == AbilityPriority.Combat_Knockback)
				{
					goto IL_DB;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_ignoreOldAbilityPhase = false;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedPrep, null);
			AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.PrepPhase);
			return;
			IL_DB:
			if (value == AbilityPriority.Evasion)
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
				if (!this.m_ignoreOldAbilityPhase)
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
					if (abilityPhase == AbilityPriority.Evasion)
					{
						goto IL_135;
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
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
				for (;;)
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
					for (;;)
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
						for (;;)
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
				}
			}
			if (!this.m_ignoreOldAbilityPhase)
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
				if (abilityPhase == AbilityPriority.DEPRICATED_Combat_Charge || abilityPhase == AbilityPriority.Combat_Damage)
				{
					return;
				}
				for (;;)
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
				for (;;)
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
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
