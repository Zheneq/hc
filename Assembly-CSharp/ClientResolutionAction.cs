using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientResolutionAction : IComparable
{
	private ResolutionActionType m_type;

	private ClientAbilityResults m_abilityResults;

	private ClientEffectResults m_effectResults;

	private ClientMovementResults m_moveResults;

	public ClientResolutionAction()
	{
		this.m_type = ResolutionActionType.Invalid;
		this.m_abilityResults = null;
		this.m_effectResults = null;
	}

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		ClientResolutionAction clientResolutionAction = obj as ClientResolutionAction;
		if (clientResolutionAction == null)
		{
			throw new ArgumentException("Object is not a ClientResolutionAction");
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.CompareTo(object)).MethodHandle;
		}
		if (this.ReactsToMovement() != clientResolutionAction.ReactsToMovement())
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
			return this.ReactsToMovement().CompareTo(clientResolutionAction.ReactsToMovement());
		}
		if (!this.ReactsToMovement())
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
			if (!clientResolutionAction.ReactsToMovement())
			{
				return 0;
			}
		}
		float moveCost = this.m_moveResults.m_triggeringPath.moveCost;
		float moveCost2 = clientResolutionAction.m_moveResults.m_triggeringPath.moveCost;
		if (moveCost != moveCost2)
		{
			return moveCost.CompareTo(moveCost2);
		}
		bool flag = this.m_moveResults.HasBarrierHitResults();
		bool flag2 = clientResolutionAction.m_moveResults.HasBarrierHitResults();
		if (flag)
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
			if (!flag2)
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
				return -1;
			}
		}
		if (!flag)
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
			if (flag2)
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
				return 1;
			}
		}
		bool flag3 = this.m_moveResults.HasGameModeHitResults();
		bool flag4 = clientResolutionAction.m_moveResults.HasGameModeHitResults();
		if (flag3)
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
			if (!flag4)
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
				return -1;
			}
		}
		if (!flag3)
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
			if (flag4)
			{
				return 1;
			}
		}
		return 0;
	}

	public unsafe static ClientResolutionAction ClientResolutionAction_DeSerializeFromStream(ref IBitStream stream)
	{
		ClientResolutionAction clientResolutionAction = new ClientResolutionAction();
		sbyte b = -1;
		stream.Serialize(ref b);
		ResolutionActionType resolutionActionType = (ResolutionActionType)b;
		clientResolutionAction.m_type = (ResolutionActionType)b;
		if (resolutionActionType == ResolutionActionType.AbilityCast)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.ClientResolutionAction_DeSerializeFromStream(IBitStream*)).MethodHandle;
			}
			clientResolutionAction.m_abilityResults = AbilityResultsUtils.DeSerializeClientAbilityResultsFromStream(ref stream);
		}
		else
		{
			if (resolutionActionType != ResolutionActionType.EffectAnimation)
			{
				if (resolutionActionType != ResolutionActionType.EffectPulse)
				{
					if (resolutionActionType != ResolutionActionType.EffectOnMove && resolutionActionType != ResolutionActionType.BarrierOnMove)
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
						if (resolutionActionType != ResolutionActionType.PowerupOnMove)
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
							if (resolutionActionType != ResolutionActionType.GameModeOnMove)
							{
								return clientResolutionAction;
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
					}
					clientResolutionAction.m_moveResults = AbilityResultsUtils.DeSerializeClientMovementResultsFromStream(ref stream);
					return clientResolutionAction;
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
			clientResolutionAction.m_effectResults = AbilityResultsUtils.DeSerializeClientEffectResultsFromStream(ref stream);
		}
		return clientResolutionAction;
	}

	public ActorData GetCaster()
	{
		if (this.m_abilityResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.GetCaster()).MethodHandle;
			}
			return this.m_abilityResults.GetCaster();
		}
		if (this.m_effectResults != null)
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
			return this.m_effectResults.GetCaster();
		}
		return null;
	}

	public AbilityData.ActionType GetSourceAbilityActionType()
	{
		if (this.m_abilityResults != null)
		{
			return this.m_abilityResults.GetSourceActionType();
		}
		if (this.m_effectResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.GetSourceAbilityActionType()).MethodHandle;
			}
			return this.m_effectResults.GetSourceActionType();
		}
		return AbilityData.ActionType.INVALID_ACTION;
	}

	public bool IsResolutionActionType(ResolutionActionType testType)
	{
		return this.m_type == testType;
	}

	public bool HasReactionHitByCaster(ActorData caster)
	{
		if (this.m_abilityResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.HasReactionHitByCaster(ActorData)).MethodHandle;
			}
			return this.m_abilityResults.HasReactionByCaster(caster);
		}
		if (this.m_effectResults != null)
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
			return this.m_effectResults.HasReactionByCaster(caster);
		}
		return false;
	}

	public unsafe void GetHitResults(out Dictionary<ActorData, ClientActorHitResults> actorHitResList, out Dictionary<Vector3, ClientPositionHitResults> posHitResList)
	{
		actorHitResList = null;
		posHitResList = null;
		if (this.m_abilityResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.GetHitResults(Dictionary<ActorData, ClientActorHitResults>*, Dictionary<Vector3, ClientPositionHitResults>*)).MethodHandle;
			}
			actorHitResList = this.m_abilityResults.GetActorHitResults();
			posHitResList = this.m_abilityResults.GetPosHitResults();
		}
		else if (this.m_effectResults != null)
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
			actorHitResList = this.m_effectResults.GetActorHitResults();
			posHitResList = this.m_effectResults.GetPosHitResults();
		}
	}

	public unsafe void GetReactionHitResultsByCaster(ActorData caster, out Dictionary<ActorData, ClientActorHitResults> actorHitResList, out Dictionary<Vector3, ClientPositionHitResults> posHitResList)
	{
		actorHitResList = null;
		posHitResList = null;
		if (this.m_abilityResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.GetReactionHitResultsByCaster(ActorData, Dictionary<ActorData, ClientActorHitResults>*, Dictionary<Vector3, ClientPositionHitResults>*)).MethodHandle;
			}
			this.m_abilityResults.GetReactionHitResultsByCaster(caster, out actorHitResList, out posHitResList);
		}
		else if (this.m_effectResults != null)
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
			this.m_effectResults.GetReactionHitResultsByCaster(caster, out actorHitResList, out posHitResList);
		}
	}

	public void RunStartSequences()
	{
		if (this.m_abilityResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.RunStartSequences()).MethodHandle;
			}
			this.m_abilityResults.StartSequences();
		}
		if (this.m_effectResults != null)
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
			this.m_effectResults.StartSequences();
		}
	}

	public void Run_OutsideResolution()
	{
		if (this.m_abilityResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.Run_OutsideResolution()).MethodHandle;
			}
			this.m_abilityResults.StartSequences();
		}
		if (this.m_effectResults != null)
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
			this.m_effectResults.StartSequences();
		}
		if (this.m_moveResults != null)
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
			this.m_moveResults.ReactToMovement();
		}
	}

	public bool CompletedAction()
	{
		bool result;
		if (this.m_type == ResolutionActionType.AbilityCast)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.CompletedAction()).MethodHandle;
			}
			result = this.m_abilityResults.DoneHitting();
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
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
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					if (this.m_type != ResolutionActionType.EffectOnMove)
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
						if (this.m_type != ResolutionActionType.BarrierOnMove)
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
							if (this.m_type != ResolutionActionType.PowerupOnMove)
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
								if (this.m_type != ResolutionActionType.GameModeOnMove)
								{
									Debug.LogError("ClientResolutionAction has unknown type: " + (int)this.m_type + ".  Assuming it's complete...");
									return true;
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
					}
					return this.m_moveResults.DoneHitting();
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
			result = this.m_effectResults.DoneHitting();
		}
		return result;
	}

	public void ExecuteUnexecutedClientHitsInAction()
	{
		if (this.m_type == ResolutionActionType.AbilityCast)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.ExecuteUnexecutedClientHitsInAction()).MethodHandle;
			}
			this.m_abilityResults.ExecuteUnexecutedClientHits();
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
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
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					if (this.m_type != ResolutionActionType.EffectOnMove)
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
						if (this.m_type != ResolutionActionType.BarrierOnMove && this.m_type != ResolutionActionType.PowerupOnMove)
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
							if (this.m_type != ResolutionActionType.GameModeOnMove)
							{
								return;
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
					this.m_moveResults.ExecuteUnexecutedClientHits();
					return;
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
			this.m_effectResults.ExecuteUnexecutedClientHits();
		}
	}

	public bool HasUnexecutedHitOnActor(ActorData actor)
	{
		bool result = false;
		if (this.m_type == ResolutionActionType.AbilityCast)
		{
			result = this.m_abilityResults.HasUnexecutedHitOnActor(actor);
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.HasUnexecutedHitOnActor(ActorData)).MethodHandle;
				}
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					if (this.m_type != ResolutionActionType.EffectOnMove)
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
						if (this.m_type != ResolutionActionType.BarrierOnMove && this.m_type != ResolutionActionType.PowerupOnMove)
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
							if (this.m_type != ResolutionActionType.GameModeOnMove)
							{
								return result;
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
					return this.m_moveResults.HasUnexecutedHitOnActor(actor);
				}
			}
			result = this.m_effectResults.HasUnexecutedHitOnActor(actor);
		}
		return result;
	}

	public void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		if (this.m_type == ResolutionActionType.AbilityCast)
		{
			this.m_abilityResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.ExecuteReactionHitsWithExtraFlagsOnActor(ActorData, ActorData, bool, bool)).MethodHandle;
				}
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					if (this.m_type != ResolutionActionType.EffectOnMove)
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
						if (this.m_type != ResolutionActionType.BarrierOnMove)
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
							if (this.m_type != ResolutionActionType.PowerupOnMove)
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
								if (this.m_type != ResolutionActionType.GameModeOnMove)
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
						}
					}
					this.m_moveResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
					return;
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
			this.m_effectResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
		}
	}

	public static bool DoneHitting(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> positionHitResults)
	{
		bool flag = true;
		bool flag2 = true;
		foreach (ClientActorHitResults clientActorHitResults in actorToHitResults.Values)
		{
			if (clientActorHitResults.ExecutedHit)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.DoneHitting(Dictionary<ActorData, ClientActorHitResults>, Dictionary<Vector3, ClientPositionHitResults>)).MethodHandle;
				}
				if (!clientActorHitResults.HasUnexecutedReactionHits())
				{
					continue;
				}
			}
			flag = false;
			break;
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.ValueCollection.Enumerator enumerator2 = positionHitResults.Values.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ClientPositionHitResults clientPositionHitResults = enumerator2.Current;
				if (!clientPositionHitResults.ExecutedHit)
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
					flag2 = false;
					goto IL_C0;
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
		}
		IL_C0:
		return flag && flag2;
	}

	public static bool HasUnexecutedHitOnActor(ActorData targetActor, Dictionary<ActorData, ClientActorHitResults> actorToHitResults)
	{
		bool result = false;
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = actorToHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> keyValuePair = enumerator.Current;
				ClientActorHitResults value = keyValuePair.Value;
				if (!value.ExecutedHit)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.HasUnexecutedHitOnActor(ActorData, Dictionary<ActorData, ClientActorHitResults>)).MethodHandle;
					}
					if (keyValuePair.Key.ActorIndex == targetActor.ActorIndex)
					{
						goto IL_6F;
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
				if (!value.HasUnexecutedReactionOnActor(targetActor))
				{
					continue;
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
				IL_6F:
				return true;
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
		return result;
	}

	public static void ExecuteUnexecutedHits(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> positionHitResults, ActorData caster)
	{
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = actorToHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> keyValuePair = enumerator.Current;
				if (!keyValuePair.Value.ExecutedHit)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.ExecuteUnexecutedHits(Dictionary<ActorData, ClientActorHitResults>, Dictionary<Vector3, ClientPositionHitResults>, ActorData)).MethodHandle;
					}
					ActorData key = keyValuePair.Key;
					if (ClientAbilityResults.LogMissingSequences)
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
						Log.Warning(string.Concat(new string[]
						{
							ClientAbilityResults.s_clientHitResultHeader,
							"Executing unexecuted actor hit on ",
							key.GetDebugName(),
							" from ",
							caster.GetDebugName()
						}), new object[0]);
					}
					keyValuePair.Value.ExecuteActorHit(keyValuePair.Key, caster);
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
		using (Dictionary<Vector3, ClientPositionHitResults>.Enumerator enumerator2 = positionHitResults.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<Vector3, ClientPositionHitResults> keyValuePair2 = enumerator2.Current;
				if (!keyValuePair2.Value.ExecutedHit)
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
					if (ClientAbilityResults.LogMissingSequences)
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
						Log.Warning(string.Concat(new string[]
						{
							ClientAbilityResults.s_clientHitResultHeader,
							"Executing unexecuted position hit on ",
							keyValuePair2.Key.ToString(),
							" from ",
							caster.GetDebugName()
						}), new object[0]);
					}
					keyValuePair2.Value.ExecutePositionHit();
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
		}
	}

	public static void ExecuteReactionHitsWithExtraFlagsOnActorAux(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> keyValuePair in actorToHitResults)
		{
			ActorData key = keyValuePair.Key;
			ClientActorHitResults value = keyValuePair.Value;
			if (!value.ExecutedHit)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.ExecuteReactionHitsWithExtraFlagsOnActorAux(Dictionary<ActorData, ClientActorHitResults>, ActorData, ActorData, bool, bool)).MethodHandle;
				}
				if (key == targetActor)
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
					value.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
				}
			}
		}
	}

	public static bool HasReactionHitByCaster(ActorData caster, Dictionary<ActorData, ClientActorHitResults> actorHitResults)
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> keyValuePair in actorHitResults)
		{
			if (keyValuePair.Value.HasReactionHitByCaster(caster))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.HasReactionHitByCaster(ActorData, Dictionary<ActorData, ClientActorHitResults>)).MethodHandle;
				}
				return true;
			}
		}
		return false;
	}

	public unsafe static void GetReactionHitResultsByCaster(ActorData caster, Dictionary<ActorData, ClientActorHitResults> actorHitResults, out Dictionary<ActorData, ClientActorHitResults> reactionActorHits, out Dictionary<Vector3, ClientPositionHitResults> reactionPosHits)
	{
		reactionActorHits = null;
		reactionPosHits = null;
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = actorHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> keyValuePair = enumerator.Current;
				if (keyValuePair.Value.HasReactionHitByCaster(caster))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.GetReactionHitResultsByCaster(ActorData, Dictionary<ActorData, ClientActorHitResults>, Dictionary<ActorData, ClientActorHitResults>*, Dictionary<Vector3, ClientPositionHitResults>*)).MethodHandle;
					}
					keyValuePair.Value.GetReactionHitResultsByCaster(caster, out reactionActorHits, out reactionPosHits);
					return;
				}
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

	public bool ContainsSequenceSource(SequenceSource sequenceSource)
	{
		return sequenceSource != null && this.ContainsSequenceSourceID(sequenceSource.RootID);
	}

	public bool ContainsSequenceSourceID(uint id)
	{
		bool result;
		if (this.m_type == ResolutionActionType.AbilityCast)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.ContainsSequenceSourceID(uint)).MethodHandle;
			}
			result = this.m_abilityResults.ContainsSequenceSourceID(id);
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
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
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					if (this.m_type != ResolutionActionType.EffectOnMove)
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
						if (this.m_type != ResolutionActionType.BarrierOnMove)
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
							if (this.m_type != ResolutionActionType.PowerupOnMove)
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
								if (this.m_type != ResolutionActionType.GameModeOnMove)
								{
									Debug.LogError("ClientResolutionAction has unknown type: " + (int)this.m_type + ".  Assuming it does not have a given SequenceSource...");
									return false;
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
					}
					return this.m_moveResults.ContainsSequenceSourceID(id);
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
			result = this.m_effectResults.ContainsSequenceSourceID(id);
		}
		return result;
	}

	public bool ReactsToMovement()
	{
		if (this.m_type != ResolutionActionType.EffectOnMove && this.m_type != ResolutionActionType.BarrierOnMove)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.ReactsToMovement()).MethodHandle;
			}
			if (this.m_type != ResolutionActionType.PowerupOnMove)
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
				return this.m_type == ResolutionActionType.GameModeOnMove;
			}
		}
		return true;
	}

	public ActorData GetTriggeringMovementActor()
	{
		ActorData result;
		if (this.m_moveResults == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.GetTriggeringMovementActor()).MethodHandle;
			}
			result = null;
		}
		else
		{
			result = this.m_moveResults.m_triggeringMover;
		}
		return result;
	}

	public void OnActorMoved_ClientResolutionAction(ActorData mover, BoardSquarePathInfo curPath)
	{
		if (this.m_moveResults.TriggerMatchesMovement(mover, curPath))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.OnActorMoved_ClientResolutionAction(ActorData, BoardSquarePathInfo)).MethodHandle;
			}
			this.m_moveResults.ReactToMovement();
		}
	}

	public unsafe void AdjustKnockbackCounts_ClientResolutionAction(ref Dictionary<ActorData, int> outgoingKnockbacks, ref Dictionary<ActorData, int> incomingKnockbacks)
	{
		if (this.m_type == ResolutionActionType.AbilityCast)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.AdjustKnockbackCounts_ClientResolutionAction(Dictionary<ActorData, int>*, Dictionary<ActorData, int>*)).MethodHandle;
			}
			this.m_abilityResults.AdjustKnockbackCounts_ClientAbilityResults(ref outgoingKnockbacks, ref incomingKnockbacks);
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
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
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					return;
				}
			}
			this.m_effectResults.AdjustKnockbackCounts_ClientEffectResults(ref outgoingKnockbacks, ref incomingKnockbacks);
		}
	}

	public string GetDebugDescription()
	{
		string text = this.m_type.ToString() + ": ";
		if (this.m_type == ResolutionActionType.AbilityCast)
		{
			text += this.m_abilityResults.GetDebugDescription();
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectAnimation)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.GetDebugDescription()).MethodHandle;
				}
				if (this.m_type != ResolutionActionType.EffectPulse)
				{
					if (this.m_type != ResolutionActionType.EffectOnMove)
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
						if (this.m_type != ResolutionActionType.BarrierOnMove)
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
							if (this.m_type != ResolutionActionType.PowerupOnMove)
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
								if (this.m_type != ResolutionActionType.GameModeOnMove)
								{
									return text + "??? (invalid results)";
								}
							}
						}
					}
					return text + this.m_moveResults.GetDebugDescription();
				}
			}
			text += this.m_effectResults.GetDebugDescription();
		}
		return text;
	}

	public string GetUnexecutedHitsDebugStr(bool logSequenceDataActors = false)
	{
		string text;
		if (this.m_type == ResolutionActionType.AbilityCast)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.GetUnexecutedHitsDebugStr(bool)).MethodHandle;
			}
			text = this.m_abilityResults.UnexecutedHitsDebugStr();
			if (logSequenceDataActors)
			{
				text = text + "\n" + this.m_abilityResults.GetSequenceStartDataDebugStr() + "\n";
			}
		}
		else if (this.m_type == ResolutionActionType.EffectAnimation || this.m_type == ResolutionActionType.EffectPulse)
		{
			text = this.m_effectResults.UnexecutedHitsDebugStr();
		}
		else
		{
			if (this.m_type != ResolutionActionType.EffectOnMove)
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
				if (this.m_type != ResolutionActionType.BarrierOnMove && this.m_type != ResolutionActionType.PowerupOnMove)
				{
					if (this.m_type != ResolutionActionType.GameModeOnMove)
					{
						return string.Empty;
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
			}
			text = this.m_moveResults.UnexecutedHitsDebugStr();
		}
		return text;
	}

	public static string AssembleUnexecutedHitsDebugStr(Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> positionToHitResults)
	{
		int num = 0;
		string text = string.Empty;
		int num2 = 0;
		string text2 = string.Empty;
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = actorToHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> keyValuePair = enumerator.Current;
				ActorData key = keyValuePair.Key;
				ClientActorHitResults value = keyValuePair.Value;
				if (!value.ExecutedHit)
				{
					num++;
					string text3 = text;
					text = string.Concat(new object[]
					{
						text3,
						"\n\t\t",
						num,
						". ActorHit on ",
						key.GetDebugName()
					});
				}
				else
				{
					num2++;
					string text3 = text2;
					text2 = string.Concat(new object[]
					{
						text3,
						"\n\t\t",
						num2,
						". ActorHit on ",
						key.GetDebugName()
					});
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientResolutionAction.AssembleUnexecutedHitsDebugStr(Dictionary<ActorData, ClientActorHitResults>, Dictionary<Vector3, ClientPositionHitResults>)).MethodHandle;
			}
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.Enumerator enumerator2 = positionToHitResults.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<Vector3, ClientPositionHitResults> keyValuePair2 = enumerator2.Current;
				Vector3 key2 = keyValuePair2.Key;
				ClientPositionHitResults value2 = keyValuePair2.Value;
				if (!value2.ExecutedHit)
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
					num++;
					string text3 = text;
					text = string.Concat(new object[]
					{
						text3,
						"\n\t\t",
						num,
						". PositionHit on ",
						key2.ToString()
					});
				}
				else
				{
					num2++;
					string text3 = text2;
					text2 = string.Concat(new object[]
					{
						text3,
						"\n\t\t",
						num2,
						". PositionHit on ",
						key2.ToString()
					});
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
		string str = "\n\tUnexecuted hits: " + num.ToString() + text;
		if (num2 > 0)
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
			str = str + "\n\tExecuted hits: " + num2.ToString() + text2;
		}
		return str + "\n";
	}
}
