using System;
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

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		this.m_syncComp = base.Caster.GetComponent<Neko_SyncComponent>();
		this.m_targetSquare = Board.\u000E().\u000E(base.TargetPos);
	}

	protected override void SpawnFX()
	{
		base.SpawnFX();
		if (this.m_discAtStartFxPrefab != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoDiscTetherSequence.SpawnFX()).MethodHandle;
			}
			if (this.m_discAtStartFxInst == null)
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
				this.m_discAtStartFxInst = base.InstantiateFX(this.m_discAtStartFxPrefab);
				this.m_discFofSelector = this.m_discAtStartFxInst.GetComponent<FriendlyEnemyVFXSelector>();
				if (this.m_enlargeDiscFxPrefab != null && this.m_enlargeDiscInst == null)
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
					this.m_enlargeDiscInst = base.InstantiateFX(this.m_enlargeDiscFxPrefab);
					this.m_enlargeDiscFofSelector = this.m_enlargeDiscInst.GetComponent<FriendlyEnemyVFXSelector>();
				}
			}
		}
	}

	protected override void DestroyFx()
	{
		base.DestroyFx();
		if (this.m_discAtStartFxInst != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoDiscTetherSequence.DestroyFx()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_discAtStartFxInst);
			this.m_discAtStartFxInst = null;
			this.m_discFofSelector = null;
		}
		if (this.m_enlargeDiscInst != null)
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
			UnityEngine.Object.Destroy(this.m_enlargeDiscInst);
			this.m_enlargeDiscInst = null;
			this.m_enlargeDiscFofSelector = null;
		}
	}

	internal override void OnTurnStart(int currentTurn)
	{
		base.OnTurnStart(currentTurn);
		if (base.Target != null)
		{
			this.m_startUsingTargetActorSquare = true;
			if (!base.Target.\u000E())
			{
				if (base.Target.\u0012() != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(NekoDiscTetherSequence.OnTurnStart(int)).MethodHandle;
					}
					this.m_fixedEndPos = base.Target.\u0012().ToVector3();
				}
				else if (base.Target.ClientLastKnownPosSquare != null)
				{
					this.m_fixedEndPos = base.Target.ClientLastKnownPosSquare.ToVector3();
				}
			}
			else
			{
				this.m_fixedEndPos = base.Target.LastDeathPosition;
			}
			this.m_fixedEndPos.y = (float)Board.\u000E().BaselineHeight;
		}
	}

	protected override Vector3 GetLineStartPos()
	{
		Vector3 vector = base.GetLineStartPos();
		if (Neko_SyncComponent.HomingDiscStartFromCaster())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoDiscTetherSequence.GetLineStartPos()).MethodHandle;
			}
			if (base.Caster != null)
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
				if (base.Caster.\u000E())
				{
					if (base.Caster.\u0015() != null)
					{
						vector = base.Caster.\u0015().ToVector3();
					}
				}
				else if (base.Caster.\u0012())
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
					vector = this.m_lastStartPos;
				}
				else if (!base.Caster.\u0018())
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
					Vector3 lineEndPos = this.GetLineEndPos();
					Vector3 a = base.Caster.transform.position - lineEndPos;
					a.y = 0f;
					if (a.magnitude > 1E-05f)
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
						a.Normalize();
					}
					else
					{
						a = Vector3.forward;
					}
					vector = lineEndPos + a * this.m_fixedTetherDistForInvisibleCaster;
				}
			}
		}
		this.m_lastStartPos = vector;
		vector.y = (float)Board.\u000E().BaselineHeight + this.m_discAtStartHeightOffset;
		return vector;
	}

	protected override Vector3 GetLineEndPos()
	{
		if (this.m_startUsingTargetActorSquare)
		{
			return this.m_fixedEndPos;
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
		if (this.m_discAtStartFxInst != null)
		{
			Vector3 lastStartPos = this.m_lastStartPos;
			bool flag = true;
			if (Neko_SyncComponent.HomingDiscStartFromCaster())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(NekoDiscTetherSequence.OnUpdate()).MethodHandle;
				}
				if (base.Caster != null)
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
					if (!base.Caster.\u000E())
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
						if (!base.Caster.\u0018())
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
							flag = false;
						}
					}
				}
			}
			lastStartPos.y = (float)Board.\u000E().BaselineHeight + this.m_discAtStartHeightOffset;
			this.m_discAtStartFxInst.transform.position = lastStartPos;
			if (this.m_discFofSelector != null)
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
				this.m_discFofSelector.Setup(base.Caster.\u000E());
			}
			this.m_discAtStartFxInst.SetActiveIfNeeded(flag);
			if (this.m_enlargeDiscInst != null)
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
				bool desiredActive = this.ShouldShowEnlargeDiscFx(flag);
				this.m_enlargeDiscInst.transform.position = lastStartPos;
				if (this.m_enlargeDiscFofSelector != null)
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
					this.m_enlargeDiscFofSelector.Setup(base.Caster.\u000E());
				}
				this.m_enlargeDiscInst.SetActiveIfNeeded(desiredActive);
			}
		}
	}

	public bool ShouldShowEnlargeDiscFx(bool discVisible)
	{
		if (base.AgeInTurns > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoDiscTetherSequence.ShouldShowEnlargeDiscFx(bool)).MethodHandle;
			}
			if (discVisible)
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
				if (this.m_syncComp != null && GameFlowData.Get() != null)
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
					if (Neko_SyncComponent.HomingDiscStartFromCaster())
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
						return this.m_syncComp.m_clientLastDiscBuffTurn == GameFlowData.Get().CurrentTurn;
					}
					return this.m_syncComp.m_clientLastDiscBuffTurn == GameFlowData.Get().CurrentTurn && this.m_syncComp.m_clientDiscBuffTargetSquare == this.m_targetSquare;
				}
			}
		}
		return false;
	}
}
