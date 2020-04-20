using System;
using System.Collections.Generic;
using AbilityContextNamespace;
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
		this.BaseInit();
		this.m_owner = base.GetComponent<ActorData>();
	}

	private void BaseInit()
	{
		this.m_contextCalcData = new ContextCalcData();
		this.m_commonProperties = new ContextVars();
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
		return ContextVars.GetDebugString(contextName, usage, actorSpecific);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_TargetSelectBase.CanShowTargeterRangePreview(TargetData[])).MethodHandle;
			}
			TargetData targetData2 = targetData[0];
			float num = Mathf.Max(0f, targetData2.m_range - 0.5f);
			if (num > 0f)
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
				if (num < 15f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_TargetSelectBase.SetTargetSelectMod(TargetSelectModBase)).MethodHandle;
			}
			this.m_currentTargetSelectMod = modBase;
			this.OnTargetSelModApplied(modBase);
		}
		else
		{
			Log.Error("Trying to apply null for target select mod", new object[0]);
		}
	}

	public void ClearTargetSelectMod()
	{
		if (this.m_currentTargetSelectMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_TargetSelectBase.ClearTargetSelectMod()).MethodHandle;
			}
			this.m_currentTargetSelectMod = null;
			this.OnTargetSelModRemoved();
		}
	}

	protected virtual void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		Log.Error("Please implement OnTargetSelModApplied in derived class " + base.GetType(), new object[0]);
	}

	protected virtual void OnTargetSelModRemoved()
	{
		Log.Error("Please implement OnTargetSelModRemoved in derived class " + base.GetType(), new object[0]);
	}

	public bool IncludeEnemies()
	{
		bool result;
		if (this.m_currentTargetSelectMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_TargetSelectBase.IncludeEnemies()).MethodHandle;
			}
			result = this.m_currentTargetSelectMod.m_includeEnemiesMod.GetModifiedValue(this.m_includeEnemies);
		}
		else
		{
			result = this.m_includeEnemies;
		}
		return result;
	}

	public bool IncludeAllies()
	{
		bool result;
		if (this.m_currentTargetSelectMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_TargetSelectBase.IncludeAllies()).MethodHandle;
			}
			result = this.m_currentTargetSelectMod.m_includeAlliesMod.GetModifiedValue(this.m_includeAllies);
		}
		else
		{
			result = this.m_includeAllies;
		}
		return result;
	}

	public bool IncludeCaster()
	{
		return (this.m_currentTargetSelectMod == null) ? this.m_includeCaster : this.m_currentTargetSelectMod.m_includeCasterMod.GetModifiedValue(this.m_includeCaster);
	}

	public bool IgnoreLos()
	{
		bool result;
		if (this.m_currentTargetSelectMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_TargetSelectBase.IgnoreLos()).MethodHandle;
			}
			result = this.m_currentTargetSelectMod.m_ignoreLosMod.GetModifiedValue(this.m_ignoreLos);
		}
		else
		{
			result = this.m_ignoreLos;
		}
		return result;
	}

	public TargetData[] GetTargetDataOverride()
	{
		if (this.m_currentTargetSelectMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GenericAbility_TargetSelectBase.GetTargetDataOverride()).MethodHandle;
			}
			if (this.m_currentTargetSelectMod.m_overrideTargetDataOnTargetSelect)
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
				return this.m_currentTargetSelectMod.m_targetDataOverride;
			}
		}
		return this.m_targetDataOverride;
	}

	public bool GetPropFloat(int key, out float value)
	{
		return this.m_commonProperties.FloatVars.TryGetValue(key, out value);
	}

	public bool GetPropInt(int key, out int value)
	{
		return this.m_commonProperties.IntVars.TryGetValue(key, out value);
	}

	public bool GetPropVec3(int key, out Vector3 value)
	{
		return this.m_commonProperties.VectorVars.TryGetValue(key, out value);
	}
}
