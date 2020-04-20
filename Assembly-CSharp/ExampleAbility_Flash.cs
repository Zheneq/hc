using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Flash";
		}
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
		if (this.m_tags != null)
		{
			if (!this.m_tags.Contains(AbilityTags.UseTeleportUIEffect))
			{
				this.m_tags.Add(AbilityTags.UseTeleportUIEffect);
			}
		}
	}

	public override bool IsFlashAbility()
	{
		return true;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		if (this.m_requireTargetNextToActor)
		{
			if (this.m_dashToTargetSelectRange > 0f)
			{
				return true;
			}
		}
		return base.CanShowTargetableRadiusPreview();
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		if (this.m_requireTargetNextToActor && this.m_dashToTargetSelectRange > 0f)
		{
			return this.m_dashToTargetSelectRange;
		}
		return base.GetTargetableRadiusInSquares(caster);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_requireTargetNextToActor)
		{
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, this.m_canDashNextToAllies, this.m_canDashNextToEnemies);
			float num = this.m_dashToTargetSelectRange;
			if (num <= 0f)
			{
				num = 50f;
			}
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(caster.GetTravelBoardSquareWorldPosition(), num, true, caster, relevantTeams, null, false, default(Vector3));
			actorsInRadius.Remove(caster);
			if (NetworkClient.active)
			{
				TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadius);
			}
			else
			{
				TargeterUtils.RemoveActorsInvisibleToActor(ref actorsInRadius, caster);
			}
			return actorsInRadius.Count > 0;
		}
		return base.CustomCanCastValidation(caster);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				if (boardSquareSafe != caster.GetCurrentBoardSquare())
				{
					if (this.m_requireTargetNextToActor)
					{
						List<ActorData> actorsVisibleToActor;
						if (NetworkServer.active)
						{
							actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(caster, true);
						}
						else
						{
							actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData, true);
						}
						List<ActorData> list = actorsVisibleToActor;
						Vector3 coneStart = caster.GetCurrentBoardSquare().ToVector3();
						int i = 0;
						while (i < list.Count)
						{
							if (flag)
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									goto IL_21C;
								}
							}
							else
							{
								ActorData actorData = list[i];
								bool flag2 = true;
								if (this.m_dashToTargetSelectRange > 0f)
								{
									flag2 = AreaEffectUtils.IsSquareInConeByActorRadius(actorData.GetCurrentBoardSquare(), coneStart, 0f, 360f, this.m_dashToTargetSelectRange, 0f, true, caster, false, default(Vector3));
								}
								if (flag2)
								{
									bool flag3 = (!NetworkClient.active) ? actorData.IsActorVisibleToActor(caster, false) : actorData.IsVisibleToClient();
									bool flag4 = actorData.GetTeam() == caster.GetTeam();
									if (flag3 && actorData != caster)
									{
										if (!flag4)
										{
											if (this.m_canDashNextToEnemies)
											{
												goto IL_1B8;
											}
										}
										if (!flag4 || !this.m_canDashNextToAllies)
										{
											goto IL_1F2;
										}
										IL_1B8:
										bool flag5 = AreaEffectUtils.IsSquareInConeByActorRadius(boardSquareSafe, actorData.GetTravelBoardSquareWorldPosition(), 0f, 360f, this.m_dashToOtherRange, 0f, true, caster, false, default(Vector3));
										flag = (flag || flag5);
									}
								}
								IL_1F2:
								i++;
							}
						}
						IL_21C:;
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
