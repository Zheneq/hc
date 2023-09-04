using UnityEngine;

public class NekoDiscTetherSequence : LineSequence
{
	[Separator("Disc Fx Prefab at line start")]
	public GameObject m_discAtStartFxPrefab;
	public float m_discAtStartHeightOffset = 1f;
	[Header("-- Fixed distance for tether when caster is not visible")]
	public float m_fixedTetherDistForInvisibleCaster = 6f;
	[Separator("For additional Fx for enlarged discs")]
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
		m_syncComp = Caster.GetComponent<Neko_SyncComponent>();
		m_targetSquare = Board.Get().GetSquareFromVec3(TargetPos);
	}

	protected override void SpawnFX()
	{
		base.SpawnFX();
		if (m_discAtStartFxPrefab != null && m_discAtStartFxInst == null)
		{
			m_discAtStartFxInst = InstantiateFX(m_discAtStartFxPrefab);
			m_discFofSelector = m_discAtStartFxInst.GetComponent<FriendlyEnemyVFXSelector>();
			if (m_enlargeDiscFxPrefab != null && m_enlargeDiscInst == null)
			{
				m_enlargeDiscInst = InstantiateFX(m_enlargeDiscFxPrefab);
				m_enlargeDiscFofSelector = m_enlargeDiscInst.GetComponent<FriendlyEnemyVFXSelector>();
			}
		}
	}

	protected override void DestroyFx()
	{
		base.DestroyFx();
		if (m_discAtStartFxInst != null)
		{
			Destroy(m_discAtStartFxInst);
			m_discAtStartFxInst = null;
			m_discFofSelector = null;
		}
		if (m_enlargeDiscInst != null)
		{
			Destroy(m_enlargeDiscInst);
			m_enlargeDiscInst = null;
			m_enlargeDiscFofSelector = null;
		}
	}

	internal override void OnTurnStart(int currentTurn)
	{
		base.OnTurnStart(currentTurn);
		if (Target == null)
		{
			return;
		}
		m_startUsingTargetActorSquare = true;
		if (!Target.IsDead())
		{
			if (Target.GetCurrentBoardSquare() != null)
			{
				m_fixedEndPos = Target.GetCurrentBoardSquare().ToVector3();
			}
			else if (Target.ClientLastKnownPosSquare != null)
			{
				m_fixedEndPos = Target.ClientLastKnownPosSquare.ToVector3();
			}
		}
		else
		{
			m_fixedEndPos = Target.LastDeathPosition;
		}
		m_fixedEndPos.y = Board.Get().BaselineHeight;
	}

	protected override Vector3 GetLineStartPos()
	{
		Vector3 vector = base.GetLineStartPos();
		if (Neko_SyncComponent.HomingDiscStartFromCaster() && Caster != null)
		{
			if (Caster.IsDead())
			{
				if (Caster.GetMostRecentDeathSquare() != null)
				{
					vector = Caster.GetMostRecentDeathSquare().ToVector3();
				}
			}
			else if (Caster.IsInRagdoll())
			{
				vector = m_lastStartPos;
			}
			else if (!Caster.IsActorVisibleToClient())
			{
				Vector3 lineEndPos = GetLineEndPos();
				Vector3 a = Caster.transform.position - lineEndPos;
				a.y = 0f;
				if (a.magnitude > 1E-05f)
				{
					a.Normalize();
				}
				else
				{
					a = Vector3.forward;
				}
				vector = lineEndPos + a * m_fixedTetherDistForInvisibleCaster;
			}
		}
		m_lastStartPos = vector;
		vector.y = Board.Get().BaselineHeight + m_discAtStartHeightOffset;
		return vector;
	}

	protected override Vector3 GetLineEndPos()
	{
		return m_startUsingTargetActorSquare
			? m_fixedEndPos
			: base.GetLineEndPos();
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
		if (m_discAtStartFxInst == null)
		{
			return;
		}
		Vector3 lastStartPos = m_lastStartPos;
		bool showFx = !Neko_SyncComponent.HomingDiscStartFromCaster()
		              || Caster == null
		              || Caster.IsDead()
		              || Caster.IsActorVisibleToClient();
		lastStartPos.y = Board.Get().BaselineHeight + m_discAtStartHeightOffset;
		m_discAtStartFxInst.transform.position = lastStartPos;
		if (m_discFofSelector != null)
		{
			m_discFofSelector.Setup(Caster.GetTeam());
		}
		m_discAtStartFxInst.SetActiveIfNeeded(showFx);
		if (m_enlargeDiscInst != null)
		{
			bool desiredActive = ShouldShowEnlargeDiscFx(showFx);
			m_enlargeDiscInst.transform.position = lastStartPos;
			if (m_enlargeDiscFofSelector != null)
			{
				m_enlargeDiscFofSelector.Setup(Caster.GetTeam());
			}
			m_enlargeDiscInst.SetActiveIfNeeded(desiredActive);
		}
	}

	public bool ShouldShowEnlargeDiscFx(bool discVisible)
	{
		return AgeInTurns > 0
		       && discVisible
		       && m_syncComp != null
		       && GameFlowData.Get() != null
		       && m_syncComp.m_clientLastDiscBuffTurn == GameFlowData.Get().CurrentTurn
		       && (Neko_SyncComponent.HomingDiscStartFromCaster() ||
		           m_syncComp.m_clientDiscBuffTargetSquare == m_targetSquare);
	}
}
