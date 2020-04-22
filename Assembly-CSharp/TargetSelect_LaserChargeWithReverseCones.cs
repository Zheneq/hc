using AbilityContextNamespace;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
	private static AbilityUtil_Targeter_LaserChargeReverseCones.ConeLosCheckerDelegate _003C_003Ef__mg_0024cache0;

	public override string GetUsageForEditor()
	{
		return GetContextUsageStr(s_cvarDirectChargeHit.GetName(), "whether this is a direct charge hit or not (if not, it's a cone hit)");
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add("DirectChargeHit");
	}

	public override void Initialize()
	{
		SetCachedFields();
		ConeTargetingInfo coneInfo = GetConeInfo();
		coneInfo.m_affectsAllies = IncludeAllies();
		coneInfo.m_affectsEnemies = IncludeEnemies();
		coneInfo.m_affectsCaster = IncludeCaster();
		coneInfo.m_penetrateLos = IgnoreLos();
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		AbilityUtil_Targeter_LaserChargeReverseCones abilityUtil_Targeter_LaserChargeReverseCones = new AbilityUtil_Targeter_LaserChargeReverseCones(ability, GetLaserWidth(), GetLaserRange(), GetConeInfo(), GetConeCount(), GetConeStartOffset(), GetPerConeHorizontalOffset(), GetAngleInBetween(), GetConeOrigins, GetConeDirections);
		abilityUtil_Targeter_LaserChargeReverseCones.m_coneLosCheckDelegate = CustomLosForCone;
		list.Add(abilityUtil_Targeter_LaserChargeReverseCones);
		return list;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		m_targetSelMod = (modBase as TargetSelectMod_LaserChargeWithReverseCones);
	}

	protected override void OnTargetSelModRemoved()
	{
		m_targetSelMod = null;
	}

	private void SetCachedFields()
	{
		ConeTargetingInfo cachedConeInfo;
		if (m_targetSelMod != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			cachedConeInfo = m_targetSelMod.m_coneInfoMod.GetModifiedValue(m_coneInfo);
		}
		else
		{
			cachedConeInfo = m_coneInfo;
		}
		m_cachedConeInfo = cachedConeInfo;
	}

	public float GetLaserRange()
	{
		float result;
		if (m_targetSelMod != null)
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
			result = m_targetSelMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
		}
		else
		{
			result = m_laserRange;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		float result;
		if (m_targetSelMod != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_targetSelMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
		}
		return result;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		ConeTargetingInfo result;
		if (m_cachedConeInfo != null)
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
			result = m_cachedConeInfo;
		}
		else
		{
			result = m_coneInfo;
		}
		return result;
	}

	public int GetConeCount()
	{
		return (m_targetSelMod == null) ? m_coneCount : m_targetSelMod.m_coneCountMod.GetModifiedValue(m_coneCount);
	}

	public float GetConeStartOffset()
	{
		float result;
		if (m_targetSelMod != null)
		{
			while (true)
			{
				switch (6)
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
			result = m_targetSelMod.m_coneStartOffsetMod.GetModifiedValue(m_coneStartOffset);
		}
		else
		{
			result = m_coneStartOffset;
		}
		return result;
	}

	public float GetPerConeHorizontalOffset()
	{
		float result;
		if (m_targetSelMod != null)
		{
			while (true)
			{
				switch (6)
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
			result = m_targetSelMod.m_perConeHorizontalOffsetMod.GetModifiedValue(m_perConeHorizontalOffset);
		}
		else
		{
			result = m_perConeHorizontalOffset;
		}
		return result;
	}

	public float GetAngleInBetween()
	{
		float result;
		if (m_targetSelMod != null)
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
			result = m_targetSelMod.m_angleInBetweenMod.GetModifiedValue(m_angleInBetween);
		}
		else
		{
			result = m_angleInBetween;
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
		List<Vector3> coneDirections = GetConeDirections(currentTarget, targeterFreePos, caster);
		Vector3 vector = -currentTarget.AimDirection;
		vector.Normalize();
		Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
		float d = GetConeStartOffset() * Board.SquareSizeStatic;
		Vector3 a = targeterFreePos + d * vector;
		for (int i = 0; i < coneDirections.Count; i++)
		{
			float d2 = GetPerConeHorizontalOffset() * (float)(i - coneDirections.Count / 2);
			Vector3 item = a + normalized * d2;
			item -= GetConeInfo().m_radiusInSquares * Board.SquareSizeStatic * coneDirections[i];
			list.Add(item);
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	public virtual List<Vector3> GetConeDirections(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		int coneCount = GetConeCount();
		float angleInBetween = GetAngleInBetween();
		float num = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
		float num2 = num + 0.5f * (float)(coneCount - 1) * angleInBetween;
		for (int i = 0; i < coneCount; i++)
		{
			list.Add(-VectorUtils.AngleDegreesToVector(num2 - (float)i * angleInBetween));
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	public static bool CustomLosForCone(ActorData actor, ActorData caster, Vector3 chargeEndPos, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		BoardSquare currentBoardSquare = actor.GetCurrentBoardSquare();
		bool result = false;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(chargeEndPos);
		if (boardSquare != null)
		{
			while (true)
			{
				switch (6)
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
			result = AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquare, currentBoardSquare, caster, true, nonActorTargetInfo);
		}
		return result;
	}
}
