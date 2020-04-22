using UnityEngine;

public class NekoDiscTetherSequence : LineSequence
{
	[Separator("Disc Fx Prefab at line start", true)]
	public GameObject m_discAtStartFxPrefab;

	public float m_discAtStartHeightOffset = 1f;

	[Header("-- Fixed distance for tether when caster is not visible")]
	public float m_fixedTetherDistForInvisibleCaster = 6f;

	[Separator("For additional Fx for enlarged discs", true)]
	public GameObject m_enlargeDiscFxPrefab;

	private bool m_startUsingTargetActorSquare;

	private Vector3 m_fixedEndPos;

	private Vector3 m_lastStartPos;

	private GameObject m_discAtStartFxInst;

	private FriendlyEnemyVFXSelector m_discFofSelector;

	private GameObject m_enlargeDiscInst;

	private FriendlyEnemyVFXSelector m_enlargeDiscFofSelector;

	private Neko_SyncComponent m_syncComp;

	private BoardSquare m_targetSquare;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		m_syncComp = base.Caster.GetComponent<Neko_SyncComponent>();
		m_targetSquare = Board.Get().GetBoardSquare(base.TargetPos);
	}

	protected override void SpawnFX()
	{
		base.SpawnFX();
		if (!(m_discAtStartFxPrefab != null))
		{
			return;
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
			if (!(m_discAtStartFxInst == null))
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
				m_discAtStartFxInst = InstantiateFX(m_discAtStartFxPrefab);
				m_discFofSelector = m_discAtStartFxInst.GetComponent<FriendlyEnemyVFXSelector>();
				if (m_enlargeDiscFxPrefab != null && m_enlargeDiscInst == null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						m_enlargeDiscInst = InstantiateFX(m_enlargeDiscFxPrefab);
						m_enlargeDiscFofSelector = m_enlargeDiscInst.GetComponent<FriendlyEnemyVFXSelector>();
						return;
					}
				}
				return;
			}
		}
	}

	protected override void DestroyFx()
	{
		base.DestroyFx();
		if (m_discAtStartFxInst != null)
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
			Object.Destroy(m_discAtStartFxInst);
			m_discAtStartFxInst = null;
			m_discFofSelector = null;
		}
		if (!(m_enlargeDiscInst != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			Object.Destroy(m_enlargeDiscInst);
			m_enlargeDiscInst = null;
			m_enlargeDiscFofSelector = null;
			return;
		}
	}

	internal override void OnTurnStart(int currentTurn)
	{
		base.OnTurnStart(currentTurn);
		if (!(base.Target != null))
		{
			return;
		}
		m_startUsingTargetActorSquare = true;
		if (!base.Target.IsDead())
		{
			if (base.Target.GetCurrentBoardSquare() != null)
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
				m_fixedEndPos = base.Target.GetCurrentBoardSquare().ToVector3();
			}
			else if (base.Target.ClientLastKnownPosSquare != null)
			{
				m_fixedEndPos = base.Target.ClientLastKnownPosSquare.ToVector3();
			}
		}
		else
		{
			m_fixedEndPos = base.Target.LastDeathPosition;
		}
		m_fixedEndPos.y = Board.Get().BaselineHeight;
	}

	protected override Vector3 GetLineStartPos()
	{
		Vector3 vector = base.GetLineStartPos();
		if (Neko_SyncComponent.HomingDiscStartFromCaster())
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
			if (base.Caster != null)
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
				if (base.Caster.IsDead())
				{
					if (base.Caster.GetMostResetDeathSquare() != null)
					{
						vector = base.Caster.GetMostResetDeathSquare().ToVector3();
					}
				}
				else if (base.Caster.IsModelAnimatorDisabled())
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
					vector = m_lastStartPos;
				}
				else if (!base.Caster.IsVisibleToClient())
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
					Vector3 lineEndPos = GetLineEndPos();
					Vector3 a = base.Caster.transform.position - lineEndPos;
					a.y = 0f;
					if (a.magnitude > 1E-05f)
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
						a.Normalize();
					}
					else
					{
						a = Vector3.forward;
					}
					vector = lineEndPos + a * m_fixedTetherDistForInvisibleCaster;
				}
			}
		}
		m_lastStartPos = vector;
		vector.y = (float)Board.Get().BaselineHeight + m_discAtStartHeightOffset;
		return vector;
	}

	protected override Vector3 GetLineEndPos()
	{
		if (m_startUsingTargetActorSquare)
		{
			return m_fixedEndPos;
		}
		return base.GetLineEndPos();
	}

	protected override bool ShouldHideForCaster()
	{
		return false;
	}

	protected override bool ShouldHideForTarget()
	{
		return false;
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (!(m_discAtStartFxInst != null))
		{
			return;
		}
		Vector3 lastStartPos = m_lastStartPos;
		bool flag = true;
		if (Neko_SyncComponent.HomingDiscStartFromCaster())
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
			if (base.Caster != null)
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
				if (!base.Caster.IsDead())
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
					if (!base.Caster.IsVisibleToClient())
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
						flag = false;
					}
				}
			}
		}
		lastStartPos.y = (float)Board.Get().BaselineHeight + m_discAtStartHeightOffset;
		m_discAtStartFxInst.transform.position = lastStartPos;
		if (m_discFofSelector != null)
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
			m_discFofSelector.Setup(base.Caster.GetTeam());
		}
		m_discAtStartFxInst.SetActiveIfNeeded(flag);
		if (!(m_enlargeDiscInst != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			bool desiredActive = ShouldShowEnlargeDiscFx(flag);
			m_enlargeDiscInst.transform.position = lastStartPos;
			if (m_enlargeDiscFofSelector != null)
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
				m_enlargeDiscFofSelector.Setup(base.Caster.GetTeam());
			}
			m_enlargeDiscInst.SetActiveIfNeeded(desiredActive);
			return;
		}
	}

	public bool ShouldShowEnlargeDiscFx(bool discVisible)
	{
		if (base.AgeInTurns > 0)
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
			if (discVisible)
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
				if (m_syncComp != null && GameFlowData.Get() != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							if (Neko_SyncComponent.HomingDiscStartFromCaster())
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										return m_syncComp.m_clientLastDiscBuffTurn == GameFlowData.Get().CurrentTurn;
									}
								}
							}
							return m_syncComp.m_clientLastDiscBuffTurn == GameFlowData.Get().CurrentTurn && m_syncComp.m_clientDiscBuffTargetSquare == m_targetSquare;
						}
					}
				}
			}
		}
		return false;
	}
}
