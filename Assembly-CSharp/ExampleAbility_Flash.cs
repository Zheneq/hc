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
					List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, m_canDashNextToAllies, m_canDashNextToEnemies);
					float num = m_dashToTargetSelectRange;
					if (num <= 0f)
					{
						num = 50f;
					}
					List<ActorData> actors = AreaEffectUtils.GetActorsInRadius(caster.GetFreePos(), num, true, caster, relevantTeams, null);
					actors.Remove(caster);
					if (NetworkClient.active)
					{
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
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.IsValidForGameplay())
			{
				if (boardSquareSafe != caster.GetCurrentBoardSquare())
				{
					if (m_requireTargetNextToActor)
					{
						List<ActorData> actorsVisibleToActor;
						if (NetworkServer.active)
						{
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
								bool flag3 = (!NetworkClient.active) ? actorData.IsActorVisibleToActor(caster) : actorData.IsActorVisibleToClient();
								bool flag4 = actorData.GetTeam() == caster.GetTeam();
								if (!flag3 || !(actorData != caster))
								{
									continue;
								}
								if (!flag4)
								{
									if (m_canDashNextToEnemies)
									{
										goto IL_01b8;
									}
								}
								if (!flag4 || !m_canDashNextToAllies)
								{
									continue;
								}
								goto IL_01b8;
							}
							break;
							IL_01b8:
							bool flag5 = AreaEffectUtils.IsSquareInConeByActorRadius(boardSquareSafe, actorData.GetFreePos(), 0f, 360f, m_dashToOtherRange, 0f, true, caster);
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
