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
		m_syncComp = base.Caster.GetComponent<Samurai_SyncComponent>();
		if (!(m_syncComp == null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (Application.isEditor)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					Debug.LogError(string.Concat(GetType(), " did not find sync component on caster"));
					return;
				}
			}
			return;
		}
	}

	protected override void OnUpdate()
	{
		if (m_initialized)
		{
			while (true)
			{
				switch (4)
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
			if (m_syncComp != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				int damageIncrease = 0;
				if (m_syncComp.m_swordBuffVfxPending)
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
					if (m_fx == null && base.AgeInTurns <= 0)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						SpawnFX();
					}
					m_syncComp.m_swordBuffVfxPending = false;
				}
				else
				{
					if (!m_switchedToActiveBuffFx)
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
						if (m_syncComp.IsSelfBuffActive(ref damageIncrease) && GameFlowData.Get().IsInDecisionState())
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
							if (m_fxPrefabForActiveBuff != null)
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
								StopFX();
								SpawnFX(m_fxPrefabForActiveBuff);
								if (!string.IsNullOrEmpty(m_onSwordActivateAudioEvent))
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
									GameObject gameObject = null;
									if (base.Caster != null)
									{
										gameObject = base.Caster.gameObject;
									}
									if (gameObject != null)
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
										AudioManager.PostEvent(m_onSwordActivateAudioEvent, gameObject);
									}
								}
								m_switchedToActiveBuffFx = true;
								goto IL_01c5;
							}
						}
					}
					if (m_syncComp.m_swordBuffFinalTurnVfxPending)
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
						if (m_switchedToActiveBuffFx)
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
							if (!m_syncComp.IsSelfBuffActive(ref damageIncrease))
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
								if (base.AgeInTurns > 0)
								{
									StopFX();
								}
								m_syncComp.m_swordBuffFinalTurnVfxPending = false;
							}
						}
					}
				}
			}
		}
		goto IL_01c5;
		IL_01c5:
		base.OnUpdate();
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
	}
}
