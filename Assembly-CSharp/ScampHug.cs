using System;
using System.Collections.Generic;
using UnityEngine;

public class ScampHug : Ability
{
	[Separator("Knockback Targeting", true)]
	public ScampHug.TargetingMode m_targetingMode;

	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;

	public float m_knockbackWidth = 1f;

	public float m_knockbackMaxRange = 10f;

	[Separator("On Hit", true)]
	public int m_knockbackDirectHitDamage = 0x14;

	public int m_knockbackAoeDamage = 0xA;

	public StandardEffectInfo m_knockbackDirectHitEffect;

	public StandardEffectInfo m_knockbackAoeHitEffect;

	[Header("-- Knockback distance and type")]
	public float m_enemyKnockbackDist;

	public KnockbackType m_enemyKnockbackType = KnockbackType.AwayFromSource;

	[Separator("Evasion Phase Params", true)]
	public int m_evadeCooldown = 2;

	public TargetData[] m_evadeTargetData;

	[Header("-- Evasion Phase Animation --")]
	public ActorModelData.ActionAnimationType m_evadeAnimType = ActorModelData.ActionAnimationType.Ability5;

	[Separator("Sequences", true)]
	public GameObject m_knockbackCastSequencePrefab;

	public GameObject m_evasionCastSequencePrefab;

	private Scamp_SyncComponent m_syncComp;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampHug.Start()).MethodHandle;
			}
			this.m_abilityName = "ScampHug";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_syncComp = base.GetComponent<Scamp_SyncComponent>();
		base.Targeter = new AbilityUtil_Targeter_ScampHug(this, this.m_syncComp, this.m_targetingMode, this.m_knockbackWidth, this.m_knockbackMaxRange, this.m_aoeShape, true, this.m_enemyKnockbackDist, this.m_enemyKnockbackType);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_knockbackDirectHitDamage);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_knockbackAoeDamage);
		return result;
	}

	public bool IsInSuit()
	{
		bool result;
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampHug.IsInSuit()).MethodHandle;
			}
			result = this.m_syncComp.m_suitWasActiveOnTurnStart;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (this.IsInSuit())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampHug.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (this.m_targetingMode == ScampHug.TargetingMode.Laser)
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
				return true;
			}
		}
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (boardSquare != null)
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
			if (boardSquare != caster.\u0012() && boardSquare.\u0016())
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
				int num;
				return KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquare, caster.\u0012(), false, out num);
			}
		}
		return false;
	}

	public override AbilityPriority GetRunPriority()
	{
		if (this.IsInSuit())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampHug.GetRunPriority()).MethodHandle;
			}
			return AbilityPriority.Combat_Knockback;
		}
		return AbilityPriority.Evasion;
	}

	public override TargetData[] GetTargetData()
	{
		if (this.IsInSuit())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampHug.GetTargetData()).MethodHandle;
			}
			return base.GetTargetData();
		}
		return this.m_evadeTargetData;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		if (this.IsInSuit())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampHug.GetMovementType()).MethodHandle;
			}
			return ActorData.MovementType.None;
		}
		return ActorData.MovementType.Charge;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (this.IsInSuit())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampHug.GetActionAnimType()).MethodHandle;
			}
			return base.GetActionAnimType();
		}
		return this.m_evadeAnimType;
	}

	public override int GetBaseCooldown()
	{
		if (!this.IsInSuit())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampHug.GetBaseCooldown()).MethodHandle;
			}
			return this.m_evadeCooldown;
		}
		return base.GetBaseCooldown();
	}

	public unsafe static void GetHitActorsAndKnockbackDestinationStatic(AbilityTarget currentTarget, ActorData caster, ScampHug.TargetingMode targetingMode, bool includeInvisibles, float knockbackWidth, float knockbackRange, AbilityAreaShape aoeShape, out ActorData firstHitActor, out List<ActorData> aoeHitActors, out BoardSquare knockbackDestSquare)
	{
		firstHitActor = null;
		aoeHitActors = new List<ActorData>();
		knockbackDestSquare = null;
		if (targetingMode == ScampHug.TargetingMode.Laser)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampHug.GetHitActorsAndKnockbackDestinationStatic(AbilityTarget, ActorData, ScampHug.TargetingMode, bool, float, float, AbilityAreaShape, ActorData*, List<ActorData>*, BoardSquare*)).MethodHandle;
			}
			Vector3 vector = caster.\u0015();
			Vector3 vector2 = VectorUtils.GetLaserEndPoint(vector, currentTarget.AimDirection, knockbackRange * Board.\u000E().squareSize, false, caster, null, true);
			BoardSquare lastValidBoardSquareInLine = KnockbackUtils.GetLastValidBoardSquareInLine(vector, vector2, false, false, float.MaxValue);
			Vector3 vector3 = lastValidBoardSquareInLine.ToVector3() - vector;
			vector3.y = 0f;
			float distance = Mathf.Max(0.1f, vector3.magnitude / Board.SquareSizeStatic);
			BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildKnockbackPath(caster, KnockbackType.ForwardAlongAimDir, currentTarget.AimDirection, Vector3.zero, distance);
			BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo.GetPathEndpoint();
			Vector3 lhs = boardSquarePathInfo2.square.ToVector3() - vector;
			lhs.y = 0f;
			float d = Vector3.Dot(lhs, currentTarget.AimDirection) + 0.5f;
			vector2 = vector + d * currentTarget.AimDirection;
			float magnitude = (vector2 - vector).magnitude;
			Vector3 vector4;
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(vector, currentTarget.AimDirection, magnitude / Board.\u000E().squareSize, knockbackWidth, caster, caster.\u0015(), true, 1, true, includeInvisibles, out vector4, null, null, false, true);
			if (actorsInLaser.Count > 0)
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
				firstHitActor = actorsInLaser[0];
				BoardSquare boardSquare = firstHitActor.\u0012();
				if (boardSquare != null)
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
					aoeHitActors = AreaEffectUtils.GetActorsInShape(aoeShape, boardSquare.ToVector3(), boardSquare, false, caster, caster.\u0012(), null);
					if (!includeInvisibles)
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
						TargeterUtils.RemoveActorsInvisibleToClient(ref aoeHitActors);
					}
					aoeHitActors.Remove(firstHitActor);
					vector2 = boardSquare.ToVector3();
				}
				Vector3 lhs2 = vector2 - vector;
				lhs2.y = 0f;
				float num = Vector3.Dot(lhs2, currentTarget.AimDirection);
				while (boardSquarePathInfo2 != null)
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
					if (boardSquarePathInfo2.prev == null)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							goto IL_29F;
						}
					}
					else
					{
						Vector3 vector5 = boardSquarePathInfo2.square.ToVector3() - vector;
						vector5.y = 0f;
						if (vector5.magnitude <= num + 0.71f)
						{
							break;
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
						boardSquarePathInfo2.prev.next = null;
						boardSquarePathInfo2 = boardSquarePathInfo2.prev;
					}
				}
			}
			IL_29F:
			knockbackDestSquare = boardSquarePathInfo2.square;
		}
		else
		{
			BoardSquare boardSquare2 = Board.\u000E().\u000E(currentTarget.GridPos);
			if (boardSquare2 != null)
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
				aoeHitActors = AreaEffectUtils.GetActorsInShape(aoeShape, boardSquare2.ToVector3(), boardSquare2, false, caster, caster.\u0012(), null);
				if (!includeInvisibles)
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
					TargeterUtils.RemoveActorsInvisibleToClient(ref aoeHitActors);
				}
			}
			knockbackDestSquare = boardSquare2;
		}
	}

	public enum TargetingMode
	{
		Laser,
		OnSquare
	}
}
