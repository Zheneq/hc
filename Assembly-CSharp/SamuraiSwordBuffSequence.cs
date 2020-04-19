using System;
using UnityEngine;

public class SamuraiSwordBuffSequence : SimpleAttachedVFXSequence
{
	[Separator("Samurai-specific FX Prefab for Decision of the turn buff becomes active", true)]
	public GameObject m_fxPrefabForActiveBuff;

	[AudioEvent(false)]
	public string m_onSwordActivateAudioEvent = string.Empty;

	private Samurai_SyncComponent m_syncComp;

	private bool m_switchedToActiveBuffFx;

	public override void FinishSetup()
	{
		this.m_syncComp = base.Caster.GetComponent<Samurai_SyncComponent>();
		if (this.m_syncComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSwordBuffSequence.FinishSetup()).MethodHandle;
			}
			if (Application.isEditor)
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
				Debug.LogError(base.GetType() + " did not find sync component on caster");
			}
		}
	}

	protected override void OnUpdate()
	{
		if (this.m_initialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSwordBuffSequence.OnUpdate()).MethodHandle;
			}
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
				int num = 0;
				if (this.m_syncComp.m_swordBuffVfxPending)
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
					if (this.m_fx == null && base.AgeInTurns <= 0)
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
						base.SpawnFX(null);
					}
					this.m_syncComp.m_swordBuffVfxPending = false;
				}
				else
				{
					if (!this.m_switchedToActiveBuffFx)
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
						if (this.m_syncComp.IsSelfBuffActive(ref num) && GameFlowData.Get().IsInDecisionState())
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
							if (this.m_fxPrefabForActiveBuff != null)
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
								base.StopFX();
								base.SpawnFX(this.m_fxPrefabForActiveBuff);
								if (!string.IsNullOrEmpty(this.m_onSwordActivateAudioEvent))
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
									GameObject gameObject = null;
									if (base.Caster != null)
									{
										gameObject = base.Caster.gameObject;
									}
									if (gameObject != null)
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
										AudioManager.PostEvent(this.m_onSwordActivateAudioEvent, gameObject);
									}
								}
								this.m_switchedToActiveBuffFx = true;
								goto IL_1C5;
							}
						}
					}
					if (this.m_syncComp.m_swordBuffFinalTurnVfxPending)
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
						if (this.m_switchedToActiveBuffFx)
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
							if (!this.m_syncComp.IsSelfBuffActive(ref num))
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
								if (base.AgeInTurns > 0)
								{
									base.StopFX();
								}
								this.m_syncComp.m_swordBuffFinalTurnVfxPending = false;
							}
						}
					}
				}
			}
		}
		IL_1C5:
		base.OnUpdate();
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
	}
}
