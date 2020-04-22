using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExampleAbility_Flash : Ability
{
	[Separator("Effect on Caster", true)]
	public bool m_applyEffect;

	public StandardActorEffectData m_effectToApply;

	[Separator("Whether require to dash next to another actor", true)]
	public bool m_requireTargetNextToActor;

	[Header("-- Range to consider actor as valid dash-to target")]
	public float m_dashToTargetSelectRange = 8.5f;

	[Header("-- Distance around target to consider as valid squares")]
	public float m_dashToOtherRange = 1.5f;

	public bool m_canDashNextToAllies = true;

	public bool m_canDashNextToEnemies = true;

	[Separator("Sequences", true)]
	public GameObject m_startSquareSequence;

	public GameObject m_endSquareSequence;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "Flash";
		}
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false);
		base.Targeter.ShowArcToShape = false;
		if (m_tags == null)
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
			if (!m_tags.Contains(AbilityTags.UseTeleportUIEffect))
			{
				m_tags.Add(AbilityTags.UseTeleportUIEffect);
			}
			return;
		}
	}

	public override bool IsFlashAbility()
	{
		return true;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		if (m_requireTargetNextToActor)
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
			if (m_dashToTargetSelectRange > 0f)
			{
				return true;
			}
		}
		return base.CanShowTargetableRadiusPreview();
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		if (m_requireTargetNextToActor && m_dashToTargetSelectRange > 0f)
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
					return m_dashToTargetSelectRange;
				}
			}
		}
		return base.GetTargetableRadiusInSquares(caster);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_requireTargetNextToActor)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, m_canDashNextToAllies, m_canDashNextToEnemies);
					float num = m_dashToTargetSelectRange;
					if (num <= 0f)
					{
						num = 50f;
					}
					List<ActorData> actors = AreaEffectUtils.GetActorsInRadius(caster.GetTravelBoardSquareWorldPosition(), num, true, caster, relevantTeams, null);
					actors.Remove(caster);
					if (NetworkClient.active)
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
						TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
					}
					else
					{
						TargeterUtils.RemoveActorsInvisibleToActor(ref actors, caster);
					}
					return actors.Count > 0;
				}
				}
			}
		}
		return base.CustomCanCastValidation(caster);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null)
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
			if (boardSquareSafe.IsBaselineHeight())
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
				if (boardSquareSafe != caster.GetCurrentBoardSquare())
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
					if (m_requireTargetNextToActor)
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
						List<ActorData> actorsVisibleToActor;
						if (NetworkServer.active)
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
							actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(caster);
						}
						else
						{
							actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData);
						}
						List<ActorData> list = actorsVisibleToActor;
						Vector3 coneStart = caster.GetCurrentBoardSquare().ToVector3();
						for (int i = 0; i < list.Count; i++)
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
							ActorData actorData;
							if (!flag)
							{
								actorData = list[i];
								bool flag2 = true;
								if (m_dashToTargetSelectRange > 0f)
								{
									flag2 = AreaEffectUtils.IsSquareInConeByActorRadius(actorData.GetCurrentBoardSquare(), coneStart, 0f, 360f, m_dashToTargetSelectRange, 0f, true, caster);
								}
								if (!flag2)
								{
									continue;
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
								bool flag3 = (!NetworkClient.active) ? actorData.IsActorVisibleToActor(caster) : actorData.IsVisibleToClient();
								bool flag4 = actorData.GetTeam() == caster.GetTeam();
								if (!flag3 || !(actorData != caster))
								{
									continue;
								}
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!flag4)
								{
									if (m_canDashNextToEnemies)
									{
										goto IL_01b8;
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
								if (!flag4 || !m_canDashNextToAllies)
								{
									continue;
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
								goto IL_01b8;
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
							break;
							IL_01b8:
							bool flag5 = AreaEffectUtils.IsSquareInConeByActorRadius(boardSquareSafe, actorData.GetTravelBoardSquareWorldPosition(), 0f, 360f, m_dashToOtherRange, 0f, true, caster);
							flag = (flag || flag5);
						}
					}
					else
					{
						flag = true;
					}
				}
			}
		}
		return flag;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Teleport;
	}

	internal override ActorData.TeleportType GetEvasionTeleportType()
	{
		return ActorData.TeleportType.Evasion_AdjustToVision;
	}
}
