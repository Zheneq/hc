using System;
using UnityEngine;

public class IceborgNovaCoreSequence : SimpleAttachedVFXOnTargetSequence
{
	[Separator("Active Version of Nova Core Vfx", true)]
	public GameObject m_activeNovaFxPrefab;

	[Separator("Empowered Vfx (when triggered by Detonate ability)", true)]
	public GameObject m_empoweredFxPrefab;

	private Iceborg_SyncComponent m_syncComp;

	private bool m_switchedToEmpowered;

	private bool m_switchedToActive;

	public override void FinishSetup()
	{
		base.FinishSetup();
		this.m_syncComp = base.Caster.GetComponent<Iceborg_SyncComponent>();
		if (this.m_syncComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgNovaCoreSequence.FinishSetup()).MethodHandle;
			}
			if (Application.isEditor)
			{
				Debug.LogError(base.GetType() + " did not find sync component on caster");
			}
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.m_initialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgNovaCoreSequence.OnUpdate()).MethodHandle;
			}
			if (this.m_syncComp != null)
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
				if (!this.m_switchedToEmpowered)
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
					if (this.m_empoweredFxPrefab != null)
					{
						ActorData target = base.Target;
						AbilityPriority currentAbilityPhase = ServerClientUtils.GetCurrentAbilityPhase();
						if (GameFlowData.Get().CurrentTurn == this.m_syncComp.m_clientDetonateNovaUsedTurn)
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
							if (currentAbilityPhase < AbilityPriority.Evasion)
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
								if (target != null && this.m_syncComp.HasNovaCore(target))
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
									base.SwitchFxTo(this.m_empoweredFxPrefab);
									this.m_switchedToEmpowered = true;
								}
							}
						}
					}
				}
				if (!this.m_switchedToActive)
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
					if (!this.m_switchedToEmpowered)
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
						if (this.m_activeNovaFxPrefab != null)
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
							if (base.AgeInTurns > 0)
							{
								base.SwitchFxTo(this.m_activeNovaFxPrefab);
								this.m_switchedToActive = true;
							}
						}
					}
				}
			}
		}
	}
}
