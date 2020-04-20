using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_LaserChargeWithReverseCones : GenericAbility_TargetSelectBase
{
	[Separator("Targeting Properties", true)]
	public float m_laserRange = 5f;

	public float m_laserWidth = 1f;

	[Header("Cone Properties")]
	public ConeTargetingInfo m_coneInfo;

	[Space(10f)]
	public int m_coneCount = 3;

	public float m_coneStartOffset;

	public float m_perConeHorizontalOffset;

	public float m_angleInBetween = 10f;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	public GameObject m_coneSequencePrefab;

	private const string c_directChargeHit = "DirectChargeHit";

	public static ContextNameKeyPair s_cvarDirectChargeHit = new ContextNameKeyPair("DirectChargeHit");

	private TargetSelectMod_LaserChargeWithReverseCones m_targetSelMod;

	private ConeTargetingInfo m_cachedConeInfo;

	[CompilerGenerated]
	private static AbilityUtil_Targeter_LaserChargeReverseCones.ConeLosCheckerDelegate f__mg_cache0;

	public override string GetUsageForEditor()
	{
		return base.GetContextUsageStr(TargetSelect_LaserChargeWithReverseCones.s_cvarDirectChargeHit.GetName(), "whether this is a direct charge hit or not (if not, it's a cone hit)", true);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add("DirectChargeHit");
	}

	public override void Initialize()
	{
		this.SetCachedFields();
		ConeTargetingInfo coneInfo = this.GetConeInfo();
		coneInfo.m_affectsAllies = base.IncludeAllies();
		coneInfo.m_affectsEnemies = base.IncludeEnemies();
		coneInfo.m_affectsCaster = base.IncludeCaster();
		coneInfo.m_penetrateLos = base.IgnoreLos();
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		AbilityUtil_Targeter_LaserChargeReverseCones abilityUtil_Targeter_LaserChargeReverseCones = new AbilityUtil_Targeter_LaserChargeReverseCones(ability, this.GetLaserWidth(), this.GetLaserRange(), this.GetConeInfo(), this.GetConeCount(), this.GetConeStartOffset(), this.GetPerConeHorizontalOffset(), this.GetAngleInBetween(), new AbilityUtil_Targeter_LaserChargeReverseCones.GetConeInfoDelegate(this.GetConeOrigins), new AbilityUtil_Targeter_LaserChargeReverseCones.GetConeInfoDelegate(this.GetConeDirections));
		AbilityUtil_Targeter_LaserChargeReverseCones abilityUtil_Targeter_LaserChargeReverseCones2 = abilityUtil_Targeter_LaserChargeReverseCones;
		
		abilityUtil_Targeter_LaserChargeReverseCones2.m_coneLosCheckDelegate = new AbilityUtil_Targeter_LaserChargeReverseCones.ConeLosCheckerDelegate(TargetSelect_LaserChargeWithReverseCones.CustomLosForCone);
		list.Add(abilityUtil_Targeter_LaserChargeReverseCones);
		return list;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		this.m_targetSelMod = (modBase as TargetSelectMod_LaserChargeWithReverseCones);
	}

	protected override void OnTargetSelModRemoved()
	{
		this.m_targetSelMod = null;
	}

	private void SetCachedFields()
	{
		ConeTargetingInfo cachedConeInfo;
		if (this.m_targetSelMod != null)
		{
			cachedConeInfo = this.m_targetSelMod.m_coneInfoMod.GetModifiedValue(this.m_coneInfo);
		}
		else
		{
			cachedConeInfo = this.m_coneInfo;
		}
		this.m_cachedConeInfo = cachedConeInfo;
	}

	public float GetLaserRange()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
		}
		else
		{
			result = this.m_laserRange;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
		}
		else
		{
			result = this.m_laserWidth;
		}
		return result;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		ConeTargetingInfo result;
		if (this.m_cachedConeInfo != null)
		{
			result = this.m_cachedConeInfo;
		}
		else
		{
			result = this.m_coneInfo;
		}
		return result;
	}

	public int GetConeCount()
	{
		return (this.m_targetSelMod == null) ? this.m_coneCount : this.m_targetSelMod.m_coneCountMod.GetModifiedValue(this.m_coneCount);
	}

	public float GetConeStartOffset()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_coneStartOffsetMod.GetModifiedValue(this.m_coneStartOffset);
		}
		else
		{
			result = this.m_coneStartOffset;
		}
		return result;
	}

	public float GetPerConeHorizontalOffset()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_perConeHorizontalOffsetMod.GetModifiedValue(this.m_perConeHorizontalOffset);
		}
		else
		{
			result = this.m_perConeHorizontalOffset;
		}
		return result;
	}

	public float GetAngleInBetween()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_angleInBetweenMod.GetModifiedValue(this.m_angleInBetween);
		}
		else
		{
			result = this.m_angleInBetween;
		}
		return result;
	}

	public override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	protected List<Vector3> GetConeOrigins(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		List<Vector3> coneDirections = this.GetConeDirections(currentTarget, targeterFreePos, caster);
		Vector3 vector = -currentTarget.AimDirection;
		vector.Normalize();
		Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
		float d = this.GetConeStartOffset() * Board.SquareSizeStatic;
		Vector3 a = targeterFreePos + d * vector;
		for (int i = 0; i < coneDirections.Count; i++)
		{
			float d2 = this.GetPerConeHorizontalOffset() * (float)(i - coneDirections.Count / 2);
			Vector3 vector2 = a + normalized * d2;
			vector2 -= this.GetConeInfo().m_radiusInSquares * Board.SquareSizeStatic * coneDirections[i];
			list.Add(vector2);
		}
		return list;
	}

	public virtual List<Vector3> GetConeDirections(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		int coneCount = this.GetConeCount();
		float angleInBetween = this.GetAngleInBetween();
		float num = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
		float num2 = num + 0.5f * (float)(coneCount - 1) * angleInBetween;
		for (int i = 0; i < coneCount; i++)
		{
			list.Add(-VectorUtils.AngleDegreesToVector(num2 - (float)i * angleInBetween));
		}
		return list;
	}

	public static bool CustomLosForCone(ActorData actor, ActorData caster, Vector3 chargeEndPos, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		BoardSquare currentBoardSquare = actor.GetCurrentBoardSquare();
		bool result = false;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(chargeEndPos);
		if (boardSquare != null)
		{
			result = AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquare, currentBoardSquare, caster, true, nonActorTargetInfo);
		}
		return result;
	}
}
