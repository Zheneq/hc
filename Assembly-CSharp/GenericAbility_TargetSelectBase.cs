using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericAbility_TargetSelectBase : MonoBehaviour
{
	[TextArea(1, 5)]
	public string m_notes;

	[Separator("Target Data Override, override ability's base Target Data (before mod)", true)]
	public bool m_useTargetDataOverride;

	public TargetData[] m_targetDataOverride;

	[Separator("Targeting - Team Filters, LOS. (Overrides fields in ConeTargetingInfo and LaserTargetingInfo)", true)]
	public bool m_includeEnemies = true;

	public bool m_includeAllies;

	public bool m_includeCaster;

	public bool m_ignoreLos;

	internal ContextCalcData m_contextCalcData;

	internal ContextVars m_commonProperties;

	protected ActorData m_owner;

	private TargetSelectModBase m_currentTargetSelectMod;

	private void Awake()
	{
		BaseInit();
		m_owner = GetComponent<ActorData>();
	}

	private void BaseInit()
	{
		m_contextCalcData = new ContextCalcData();
		m_commonProperties = new ContextVars();
	}

	public virtual void Initialize()
	{
	}

	public virtual List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		return null;
	}

	public virtual bool HandleCanCastValidation(Ability ability, ActorData caster)
	{
		return true;
	}

	public virtual bool HandleCustomTargetValidation(Ability ability, ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return true;
	}

	public virtual string GetUsageForEditor()
	{
		return string.Empty;
	}

	public string GetContextUsageStr(string contextName, string usage, bool actorSpecific = true)
	{
		return ContextVars.GetContextUsageStr(contextName, usage, actorSpecific);
	}

	public virtual void ListContextNamesForEditor(List<string> names)
	{
	}

	public virtual ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.None;
	}

	public virtual bool CanShowTargeterRangePreview(TargetData[] targetData)
	{
		if (targetData.Length > 0)
		{
			TargetData targetData2 = targetData[0];
			float num = Mathf.Max(0f, targetData2.m_range - 0.5f);
			if (num > 0f)
			{
				if (num < 15f)
				{
					if (targetData2.m_targetingParadigm != Ability.TargetingParadigm.Direction)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public virtual float GetTargeterRangePreviewRadius(Ability ability, ActorData caster)
	{
		return AbilityUtils.GetCurrentRangeInSquares(ability, caster, 0);
	}

	public void SetTargetSelectMod(TargetSelectModBase modBase)
	{
		if (modBase != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_currentTargetSelectMod = modBase;
					OnTargetSelModApplied(modBase);
					return;
				}
			}
		}
		Log.Error("Trying to apply null for target select mod");
	}

	public void ClearTargetSelectMod()
	{
		if (m_currentTargetSelectMod == null)
		{
			return;
		}
		while (true)
		{
			m_currentTargetSelectMod = null;
			OnTargetSelModRemoved();
			return;
		}
	}

	protected virtual void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		Log.Error("Please implement OnTargetSelModApplied in derived class " + GetType());
	}

	protected virtual void OnTargetSelModRemoved()
	{
		Log.Error("Please implement OnTargetSelModRemoved in derived class " + GetType());
	}

	public bool IncludeEnemies()
	{
		bool result;
		if (m_currentTargetSelectMod != null)
		{
			result = m_currentTargetSelectMod.m_includeEnemiesMod.GetModifiedValue(m_includeEnemies);
		}
		else
		{
			result = m_includeEnemies;
		}
		return result;
	}

	public bool IncludeAllies()
	{
		bool result;
		if (m_currentTargetSelectMod != null)
		{
			result = m_currentTargetSelectMod.m_includeAlliesMod.GetModifiedValue(m_includeAllies);
		}
		else
		{
			result = m_includeAllies;
		}
		return result;
	}

	public bool IncludeCaster()
	{
		return (m_currentTargetSelectMod == null) ? m_includeCaster : m_currentTargetSelectMod.m_includeCasterMod.GetModifiedValue(m_includeCaster);
	}

	public bool IgnoreLos()
	{
		bool result;
		if (m_currentTargetSelectMod != null)
		{
			result = m_currentTargetSelectMod.m_ignoreLosMod.GetModifiedValue(m_ignoreLos);
		}
		else
		{
			result = m_ignoreLos;
		}
		return result;
	}

	public TargetData[] GetTargetDataOverride()
	{
		if (m_currentTargetSelectMod != null)
		{
			if (m_currentTargetSelectMod.m_overrideTargetDataOnTargetSelect)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return m_currentTargetSelectMod.m_targetDataOverride;
					}
				}
			}
		}
		return m_targetDataOverride;
	}

	public bool GetPropFloat(int key, out float value)
	{
		return m_commonProperties.m_floatVars.TryGetValue(key, out value);
	}

	public bool GetPropInt(int key, out int value)
	{
		return m_commonProperties.m_intVars.TryGetValue(key, out value);
	}

	public bool GetPropVec3(int key, out Vector3 value)
	{
		return m_commonProperties.m_vec3Vars.TryGetValue(key, out value);
	}
}
