using System;
using System.Collections.Generic;
using UnityEngine;

public class ThiefSmokeBomb : Ability
{
	[Header("-- Bomb Damage")]
	public int m_extraDamageOnCast;

	[Header("-- Bomb Targeting (shape is in Smoke Field Info)")]
	public bool m_penetrateLos;

	public int m_maxAngleWithFirstSegment;

	public float m_maxDistanceWithFirst;

	public float m_minDistanceBetweenBombs = 1f;

	[Header("-- On Cast Hit Effect")]
	public StandardEffectInfo m_bombHitEffectInfo;

	[Header("-- Smoke Field")]
	public GroundEffectField m_smokeFieldInfo;

	[Header("-- Barrier (will make square out of 4 barriers around ground field)")]
	public bool m_addBarriers = true;

	public float m_barrierSquareWidth = 3f;

	public StandardBarrierData m_barrierData;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ThiefSmokeBomb m_abilityMod;

	private StandardEffectInfo m_cachedBombHitEffectInfo;

	private GroundEffectField m_cachedSmokeFieldInfo;

	private StandardBarrierData m_cachedBarrierData;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.Start()).MethodHandle;
			}
			this.m_abilityName = "Smoke Bomb";
		}
		if (this.m_barrierSquareWidth <= 0f)
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
			Debug.LogWarning("Thief Smoke Bomb, Barrier Data has 0 width, setting to 3");
			this.m_barrierSquareWidth = 3f;
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		AbilityAreaShape shape = this.GetSmokeFieldInfo().shape;
		GroundEffectField fieldData = this.GetSmokeFieldInfo();
		if (this.GetExpectedNumberOfTargeters() > 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.Setup()).MethodHandle;
			}
			base.ClearTargeters();
			for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
			{
				AbilityAreaShape shape2 = shape;
				bool penetrateLoS = this.PenetrateLos();
				AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape;
				bool affectsEnemies = true;
				bool affectsAllies;
				if (fieldData.healAmount > 0)
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
					affectsAllies = !fieldData.ignoreNonCasterAllies;
				}
				else
				{
					affectsAllies = false;
				}
				AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, shape2, penetrateLoS, damageOriginType, affectsEnemies, affectsAllies, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
				abilityUtil_Targeter_Shape.m_affectCasterDelegate = delegate(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
				{
					bool result;
					if (fieldData.healAmount > 0)
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
							RuntimeMethodHandle runtimeMethodHandle2 = methodof(ThiefSmokeBomb.<Setup>c__AnonStorey0.<>m__0(ActorData, List<ActorData>, bool)).MethodHandle;
						}
						result = casterInShape;
					}
					else
					{
						result = false;
					}
					return result;
				};
				abilityUtil_Targeter_Shape.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Primary, AbilityTooltipSubject.None);
				base.Targeters.Add(abilityUtil_Targeter_Shape);
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
		else
		{
			AbilityAreaShape shape3 = shape;
			bool penetrateLoS2 = this.PenetrateLos();
			AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType2 = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape;
			bool affectsEnemies2 = true;
			bool affectsAllies2;
			if (fieldData.healAmount > 0)
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
				affectsAllies2 = !fieldData.ignoreNonCasterAllies;
			}
			else
			{
				affectsAllies2 = false;
			}
			base.Targeter = new AbilityUtil_Targeter_Shape(this, shape3, penetrateLoS2, damageOriginType2, affectsEnemies2, affectsAllies2, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible)
			{
				m_affectCasterDelegate = delegate(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
				{
					bool result;
					if (fieldData.healAmount > 0)
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
							RuntimeMethodHandle runtimeMethodHandle2 = methodof(ThiefSmokeBomb.<Setup>c__AnonStorey0.<>m__1(ActorData, List<ActorData>, bool)).MethodHandle;
						}
						result = casterInShape;
					}
					else
					{
						result = false;
					}
					return result;
				}
			};
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Max(1, base.GetNumTargets());
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedBombHitEffectInfo;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.SetCachedFields()).MethodHandle;
			}
			cachedBombHitEffectInfo = this.m_abilityMod.m_bombHitEffectInfoMod.GetModifiedValue(this.m_bombHitEffectInfo);
		}
		else
		{
			cachedBombHitEffectInfo = this.m_bombHitEffectInfo;
		}
		this.m_cachedBombHitEffectInfo = cachedBombHitEffectInfo;
		GroundEffectField cachedSmokeFieldInfo;
		if (this.m_abilityMod)
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
			cachedSmokeFieldInfo = this.m_abilityMod.m_smokeFieldInfoMod.GetModifiedValue(this.m_smokeFieldInfo);
		}
		else
		{
			cachedSmokeFieldInfo = this.m_smokeFieldInfo;
		}
		this.m_cachedSmokeFieldInfo = cachedSmokeFieldInfo;
		StandardBarrierData cachedBarrierData;
		if (this.m_abilityMod)
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
			cachedBarrierData = this.m_abilityMod.m_barrierDataMod.GetModifiedValue(this.m_barrierData);
		}
		else
		{
			cachedBarrierData = this.m_barrierData;
		}
		this.m_cachedBarrierData = cachedBarrierData;
	}

	public int GetExtraDamageOnCast()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.GetExtraDamageOnCast()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageOnCastMod.GetModifiedValue(this.m_extraDamageOnCast);
		}
		else
		{
			result = this.m_extraDamageOnCast;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.PenetrateLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
		}
		else
		{
			result = this.m_penetrateLos;
		}
		return result;
	}

	public int GetMaxAngleWithFirstSegment()
	{
		return (!this.m_abilityMod) ? this.m_maxAngleWithFirstSegment : this.m_abilityMod.m_maxAngleWithFirstSegmentMod.GetModifiedValue(this.m_maxAngleWithFirstSegment);
	}

	public float GetMaxDistanceWithFirst()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.GetMaxDistanceWithFirst()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxDistanceWithFirstMod.GetModifiedValue(this.m_maxDistanceWithFirst);
		}
		else
		{
			result = this.m_maxDistanceWithFirst;
		}
		return result;
	}

	public float GetMinDistanceBetweenBombs()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.GetMinDistanceBetweenBombs()).MethodHandle;
			}
			result = this.m_abilityMod.m_minDistanceBetweenBombsMod.GetModifiedValue(this.m_minDistanceBetweenBombs);
		}
		else
		{
			result = this.m_minDistanceBetweenBombs;
		}
		return result;
	}

	public StandardEffectInfo GetBombHitEffectInfo()
	{
		StandardEffectInfo result;
		if (this.m_cachedBombHitEffectInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.GetBombHitEffectInfo()).MethodHandle;
			}
			result = this.m_cachedBombHitEffectInfo;
		}
		else
		{
			result = this.m_bombHitEffectInfo;
		}
		return result;
	}

	public GroundEffectField GetSmokeFieldInfo()
	{
		GroundEffectField result;
		if (this.m_cachedSmokeFieldInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.GetSmokeFieldInfo()).MethodHandle;
			}
			result = this.m_cachedSmokeFieldInfo;
		}
		else
		{
			result = this.m_smokeFieldInfo;
		}
		return result;
	}

	public bool AddBarriers()
	{
		return (!this.m_abilityMod) ? this.m_addBarriers : this.m_abilityMod.m_addBarriersMod.GetModifiedValue(this.m_addBarriers);
	}

	public float GetBarrierSquareWidth()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.GetBarrierSquareWidth()).MethodHandle;
			}
			result = this.m_abilityMod.m_barrierSquareWidthMod.GetModifiedValue(this.m_barrierSquareWidth);
		}
		else
		{
			result = this.m_barrierSquareWidth;
		}
		return result;
	}

	public StandardBarrierData GetBarrierData()
	{
		StandardBarrierData result;
		if (this.m_cachedBarrierData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.GetBarrierData()).MethodHandle;
			}
			result = this.m_cachedBarrierData;
		}
		else
		{
			result = this.m_barrierData;
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		Board board = Board.Get();
		BoardSquare boardSquareSafe = board.GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (boardSquareSafe.IsBaselineHeight())
			{
				if (targetIndex == 0)
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
					if (boardSquareSafe == caster.GetCurrentBoardSquare())
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							return false;
						}
					}
				}
				Vector3 vector = boardSquareSafe.ToVector3();
				Vector3 firstSegEndPos = (targetIndex <= 0) ? vector : board.GetBoardSquareSafe(currentTargets[0].GridPos).ToVector3();
				AbilityAreaShape shape = this.GetSmokeFieldInfo().shape;
				bool flag = true;
				if (targetIndex > 0)
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
					Vector3 to = vector - caster.GetTravelBoardSquareWorldPosition();
					to.y = 0f;
					bool flag2 = true;
					if (this.GetMaxAngleWithFirstSegment() > 0)
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
						BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[0].GridPos);
						Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(shape, currentTargets[0].FreePos, boardSquareSafe2);
						Vector3 from = centerOfShape - caster.GetTravelBoardSquareWorldPosition();
						from.y = 0f;
						int num = Mathf.RoundToInt(Vector3.Angle(from, to));
						flag2 = (num <= this.GetMaxAngleWithFirstSegment());
					}
					Vector3 centerOfShape2 = AreaEffectUtils.GetCenterOfShape(shape, vector, boardSquareSafe);
					Vector3 vector2 = centerOfShape2 - caster.GetTravelBoardSquareWorldPosition();
					vector2.y = 0f;
					float magnitude = vector2.magnitude;
					bool flag3;
					if (this.GetMaxDistanceWithFirst() > 0f)
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
						flag3 = (magnitude <= this.GetMaxDistanceWithFirst() * board.squareSize);
					}
					else
					{
						flag3 = true;
					}
					bool flag4 = flag3;
					if (flag2)
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
						if (flag4)
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
					}
					flag = false;
				}
				IL_1F2:
				if (flag)
				{
					float shapeCenterMinDistInWorld = 0.71f * board.squareSize;
					float minDistInWorld = this.GetMinDistanceBetweenBombs() * board.squareSize;
					int i = 0;
					while (i < targetIndex)
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
						if (!flag)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								goto IL_28C;
							}
						}
						else
						{
							BoardSquare boardSquareSafe3 = board.GetBoardSquareSafe(currentTargets[i].GridPos);
							Vector3 centerOfShape3 = AreaEffectUtils.GetCenterOfShape(shape, currentTargets[i].FreePos, boardSquareSafe3);
							flag = this.CheckMinDistConstraint(centerOfShape3, boardSquareSafe, shape, shapeCenterMinDistInWorld, minDistInWorld);
							i++;
						}
					}
					IL_28C:
					int expectedNumberOfTargeters = this.GetExpectedNumberOfTargeters();
					if (flag && targetIndex < expectedNumberOfTargeters - 1)
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
						List<AbilityTarget> list = new List<AbilityTarget>();
						for (int j = 0; j < expectedNumberOfTargeters; j++)
						{
							list.Add(target.GetCopy());
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
						for (int k = 0; k < targetIndex; k++)
						{
							list[k].SetPosAndDir(currentTargets[k].GridPos, currentTargets[k].FreePos, Vector3.forward);
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
						list[targetIndex].SetPosAndDir(boardSquareSafe.GetGridPos(), target.FreePos, Vector3.forward);
						float currentRangeInSquares = AbilityUtils.GetCurrentRangeInSquares(this, caster, 0);
						flag = this.CanTargetFutureClicks(caster, firstSegEndPos, targetIndex, list, targetIndex, expectedNumberOfTargeters, currentRangeInSquares);
					}
				}
				return flag;
			}
		}
		return false;
	}

	public bool CanTargetFutureClicks(ActorData caster, Vector3 firstSegEndPos, int lastSelectedTargetIndex, List<AbilityTarget> targetEntries, int numTargetsFromPlayerInput, int numClicks, float abilityMaxRange)
	{
		if (lastSelectedTargetIndex >= numClicks - 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.CanTargetFutureClicks(ActorData, Vector3, int, List<AbilityTarget>, int, int, float)).MethodHandle;
			}
			return true;
		}
		Vector3 vec = firstSegEndPos - caster.GetTravelBoardSquareWorldPosition();
		float coneWidthDegrees = Mathf.Min(360f, 2f * (float)this.GetMaxAngleWithFirstSegment() + 25f);
		int num;
		int num2;
		int num3;
		int num4;
		AreaEffectUtils.GetMaxConeBounds(caster.GetTravelBoardSquareWorldPosition(), VectorUtils.HorizontalAngle_Deg(vec), coneWidthDegrees, abilityMaxRange, 0f, out num, out num2, out num3, out num4);
		Board board = Board.Get();
		AbilityAreaShape shape = this.GetSmokeFieldInfo().shape;
		AbilityData abilityData = caster.GetAbilityData();
		BoardSquare currentBoardSquare = caster.GetCurrentBoardSquare();
		float shapeCenterMinDistInWorld = 0.71f * board.squareSize;
		float minDistInWorld = this.GetMinDistanceBetweenBombs() * board.squareSize;
		bool flag = false;
		int i = num;
		while (i < num2)
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
			if (flag)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					return flag;
				}
			}
			else
			{
				int j = num3;
				while (j < num4)
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
					if (flag)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							goto IL_4E5;
						}
					}
					else
					{
						BoardSquare boardSquare = board.GetBoardSquare(i, j);
						if (boardSquare != null)
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
							if (boardSquare.IsBaselineHeight())
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
								if (currentBoardSquare.\u0013(boardSquare.x, boardSquare.y))
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
									if (abilityData.IsTargetSquareInRangeOfAbilityFromSquare(boardSquare, currentBoardSquare, abilityMaxRange, 0f))
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
										Vector3 vector = boardSquare.ToVector3();
										bool flag2 = true;
										bool flag3 = true;
										int maxAngleWithFirstSegment = this.GetMaxAngleWithFirstSegment();
										if (maxAngleWithFirstSegment > 0)
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
											BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(targetEntries[0].GridPos);
											if (numTargetsFromPlayerInput > 0)
											{
												Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(shape, targetEntries[0].FreePos, boardSquareSafe);
												Vector3 from = centerOfShape - caster.GetTravelBoardSquareWorldPosition();
												Vector3 to = vector - caster.GetTravelBoardSquareWorldPosition();
												int num5 = Mathf.RoundToInt(Vector3.Angle(from, to));
												flag3 = (num5 <= maxAngleWithFirstSegment);
											}
											else
											{
												for (int k = 0; k < 4; k++)
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
													if (!flag2)
													{
														break;
													}
													Vector3 vector2 = boardSquareSafe.ToVector3();
													vector2 += 0.1f * VectorUtils.AngleDegreesToVector(45f + (float)k * 90f);
													Vector3 centerOfShape2 = AreaEffectUtils.GetCenterOfShape(shape, vector2, boardSquareSafe);
													Vector3 from2 = centerOfShape2 - caster.GetTravelBoardSquareWorldPosition();
													Vector3 to2 = vector - caster.GetTravelBoardSquareWorldPosition();
													int num6 = Mathf.RoundToInt(Vector3.Angle(from2, to2));
													bool flag4;
													if (flag3)
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
														flag4 = (num6 <= maxAngleWithFirstSegment);
													}
													else
													{
														flag4 = false;
													}
													flag3 = flag4;
												}
											}
										}
										Vector3 centerOfShape3 = AreaEffectUtils.GetCenterOfShape(shape, vector, boardSquare);
										Vector3 vector3 = centerOfShape3 - caster.GetTravelBoardSquareWorldPosition();
										vector3.y = 0f;
										float magnitude = vector3.magnitude;
										bool flag5;
										if (this.GetMaxDistanceWithFirst() > 0f)
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
											flag5 = (magnitude <= this.GetMaxDistanceWithFirst() * board.squareSize);
										}
										else
										{
											flag5 = true;
										}
										bool flag6 = flag5;
										if (!flag3 || !flag6)
										{
											flag2 = false;
										}
										if (flag2)
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
											int l = 0;
											while (l <= lastSelectedTargetIndex)
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
												if (!flag2)
												{
													for (;;)
													{
														switch (2)
														{
														case 0:
															continue;
														}
														goto IL_461;
													}
												}
												else
												{
													BoardSquare boardSquareSafe2 = board.GetBoardSquareSafe(targetEntries[l].GridPos);
													if (boardSquareSafe2 == boardSquare)
													{
														flag2 = false;
													}
													else if (l < numTargetsFromPlayerInput)
													{
														Vector3 freePos = targetEntries[l].FreePos;
														Vector3 centerOfShape4 = AreaEffectUtils.GetCenterOfShape(shape, freePos, boardSquareSafe2);
														flag2 = this.CheckMinDistConstraint(centerOfShape4, boardSquare, shape, shapeCenterMinDistInWorld, minDistInWorld);
													}
													else
													{
														int m = 0;
														while (m < 4)
														{
															if (!flag2)
															{
																for (;;)
																{
																	switch (7)
																	{
																	case 0:
																		continue;
																	}
																	goto IL_43B;
																}
															}
															else
															{
																Vector3 vector4 = boardSquareSafe2.ToVector3();
																vector4 += 0.1f * VectorUtils.AngleDegreesToVector(45f + (float)m * 90f);
																Vector3 centerOfShape5 = AreaEffectUtils.GetCenterOfShape(shape, vector4, boardSquareSafe2);
																flag2 = this.CheckMinDistConstraint(centerOfShape5, boardSquare, shape, shapeCenterMinDistInWorld, minDistInWorld);
																m++;
															}
														}
													}
													IL_43B:
													l++;
												}
											}
										}
										IL_461:
										if (flag2)
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
											if (lastSelectedTargetIndex < numClicks - 1)
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
												targetEntries[lastSelectedTargetIndex + 1].SetPosAndDir(boardSquare.GetGridPos(), vector, Vector3.forward);
												flag2 = this.CanTargetFutureClicks(caster, firstSegEndPos, lastSelectedTargetIndex + 1, targetEntries, numTargetsFromPlayerInput, numClicks, abilityMaxRange);
											}
										}
										flag = flag2;
									}
								}
							}
						}
						j++;
					}
				}
				IL_4E5:
				i++;
			}
		}
		return flag;
	}

	private bool CheckMinDistConstraint(Vector3 centerOfShapePrev, BoardSquare candidateSquare, AbilityAreaShape fieldShape, float shapeCenterMinDistInWorld, float minDistInWorld)
	{
		bool result = true;
		Vector3 vector = candidateSquare.ToVector3();
		int i = 0;
		while (i < 4)
		{
			Vector3 vector2 = vector;
			vector2 += 0.1f * VectorUtils.AngleDegreesToVector(45f + (float)i * 90f);
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(fieldShape, vector2, candidateSquare);
			Vector3 vector3 = centerOfShape - centerOfShapePrev;
			vector3.y = 0f;
			float magnitude = vector3.magnitude;
			if (magnitude >= shapeCenterMinDistInWorld)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.CheckMinDistConstraint(Vector3, BoardSquare, AbilityAreaShape, float, float)).MethodHandle;
				}
				if (minDistInWorld > 0f)
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
					if (magnitude < minDistInWorld)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							goto IL_A3;
						}
					}
				}
				i++;
				continue;
			}
			IL_A3:
			result = false;
			return result;
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			return result;
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, 1);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetSmokeFieldInfo().healAmount);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetSmokeFieldInfo().healAmount);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeters[0].LastUpdatingGridPos);
		int damageAmount = this.GetSmokeFieldInfo().damageAmount;
		int subsequentDamageAmount = this.GetSmokeFieldInfo().subsequentDamageAmount;
		int i = 0;
		while (i <= currentTargeterIndex)
		{
			if (i <= 0)
			{
				goto IL_B0;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(base.Targeters[i].LastUpdatingGridPos);
			if (!(boardSquareSafe2 == null))
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
				if (!(boardSquareSafe2 == boardSquareSafe))
				{
					goto IL_B0;
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
			IL_D7:
			i++;
			continue;
			IL_B0:
			Ability.AddNameplateValueForOverlap(ref result, base.Targeters[i], targetActor, currentTargeterIndex, damageAmount + this.GetExtraDamageOnCast(), subsequentDamageAmount, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
			goto IL_D7;
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
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefSmokeBomb abilityMod_ThiefSmokeBomb = modAsBase as AbilityMod_ThiefSmokeBomb;
		string name = "ExtraDamageOnCast";
		string empty = string.Empty;
		int val;
		if (abilityMod_ThiefSmokeBomb)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_ThiefSmokeBomb.m_extraDamageOnCastMod.GetModifiedValue(this.m_extraDamageOnCast);
		}
		else
		{
			val = this.m_extraDamageOnCast;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "MaxAngleWithFirstSegment";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ThiefSmokeBomb)
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
			val2 = abilityMod_ThiefSmokeBomb.m_maxAngleWithFirstSegmentMod.GetModifiedValue(this.m_maxAngleWithFirstSegment);
		}
		else
		{
			val2 = this.m_maxAngleWithFirstSegment;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_ThiefSmokeBomb)
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
			effectInfo = abilityMod_ThiefSmokeBomb.m_bombHitEffectInfoMod.GetModifiedValue(this.m_bombHitEffectInfo);
		}
		else
		{
			effectInfo = this.m_bombHitEffectInfo;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "BombHitEffectInfo", this.m_bombHitEffectInfo, true);
		StandardBarrierData standardBarrierData;
		if (abilityMod_ThiefSmokeBomb)
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
			standardBarrierData = abilityMod_ThiefSmokeBomb.m_barrierDataMod.GetModifiedValue(this.m_barrierData);
		}
		else
		{
			standardBarrierData = this.m_barrierData;
		}
		StandardBarrierData standardBarrierData2 = standardBarrierData;
		standardBarrierData2.AddTooltipTokens(tokens, "BarrierData", abilityMod_ThiefSmokeBomb != null, this.m_barrierData);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ThiefSmokeBomb))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSmokeBomb.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ThiefSmokeBomb);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
