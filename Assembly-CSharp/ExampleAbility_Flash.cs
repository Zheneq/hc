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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExampleAbility_Flash.Start()).MethodHandle;
			}
			this.m_abilityName = "Flash";
		}
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
		if (this.m_tags != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExampleAbility_Flash.CanShowTargetableRadiusPreview()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExampleAbility_Flash.GetTargetableRadiusInSquares(ActorData)).MethodHandle;
			}
			return this.m_dashToTargetSelectRange;
		}
		return base.GetTargetableRadiusInSquares(caster);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_requireTargetNextToActor)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExampleAbility_Flash.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, this.m_canDashNextToAllies, this.m_canDashNextToEnemies);
			float num = this.m_dashToTargetSelectRange;
			if (num <= 0f)
			{
				num = 50f;
			}
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(caster.\u0016(), num, true, caster, relevantTeams, null, false, default(Vector3));
			actorsInRadius.Remove(caster);
			if (NetworkClient.active)
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
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (boardSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExampleAbility_Flash.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (boardSquare.\u0016())
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
				if (boardSquare != caster.\u0012())
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
					if (this.m_requireTargetNextToActor)
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
						List<ActorData> actorsVisibleToActor;
						if (NetworkServer.active)
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
							actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(caster, true);
						}
						else
						{
							actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData, true);
						}
						List<ActorData> list = actorsVisibleToActor;
						Vector3 coneStart = caster.\u0012().ToVector3();
						int i = 0;
						while (i < list.Count)
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
									flag2 = AreaEffectUtils.IsSquareInConeByActorRadius(actorData.\u0012(), coneStart, 0f, 360f, this.m_dashToTargetSelectRange, 0f, true, caster, false, default(Vector3));
								}
								if (flag2)
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
									bool flag3 = (!NetworkClient.active) ? actorData.\u000E(caster, false) : actorData.\u0018();
									bool flag4 = actorData.\u000E() == caster.\u000E();
									if (flag3 && actorData != caster)
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
										if (!flag4)
										{
											if (this.m_canDashNextToEnemies)
											{
												goto IL_1B8;
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
										if (!flag4 || !this.m_canDashNextToAllies)
										{
											goto IL_1F2;
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
										IL_1B8:
										bool flag5 = AreaEffectUtils.IsSquareInConeByActorRadius(boardSquare, actorData.\u0016(), 0f, 360f, this.m_dashToOtherRange, 0f, true, caster, false, default(Vector3));
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
