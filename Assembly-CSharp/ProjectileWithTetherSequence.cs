using System.Collections.Generic;
using UnityEngine;

public class ProjectileWithTetherSequence : ArcingProjectileSequence
{
	public enum AimMode
	{
		None,
		TargetToCaster,
		CasterToTarget
	}

	[Separator("Line FX Settings", "yellow")]
	public GameObject m_linePrefab;

	[Space(10f)]
	public float m_splineMaxHeight;

	[Header("    Line Info")]
	public bool m_lineSpawnOnProjectileImpact;

	[AnimEventPicker]
	public Object m_lineStartEvent;

	[AnimEventPicker]
	public Object m_lineRemoveEvent;

	public float m_lineDuration = -1f;

	[AudioEvent(false)]
	public string m_lineAudioEvent;

	[Header("-- (only check if don't want to use start joint) Whether to use projectile's starting position as start position for line")]
	public bool m_useProjectileStartPosAsStartPoint;

	public bool m_lineStartPosUseGroundHeight;

	public bool m_lineEndPosUseGroundHeight;

	[Space(15f, order = 0)]
	[JointPopup("Line Start Joint Caster", order = 1)]
	public JointPopupProperty m_lineFxCasterStartJoint;

	[JointPopup("Start Joint Overrides")]
	public List<JointPopupProperty> m_startJointOverrides;

	public float m_lineStartYOffsetFromJoint;

	[JointPopup("Line Hit Target Joint", order = 1)]
	[Space(15f, order = 0)]
	public JointPopupProperty m_lineOnHitTargetAttachJoint;

	public bool m_lineAttachToHitTarget = true;

	[Header("-- On Projectile Hit Stuff --")]
	public GameObject m_attachedFxOnHitPrefab;

	public bool m_onProjHitFxAttachToJoint = true;

	public bool m_projectileDoHitsAsItTravelOverride;

	public AimMode m_onProjHitFxAimMode;

	[JointPopup("On Projectile Hit Attach Joint")]
	public JointPopupProperty m_onProjHitJoint;

	[Header("-- For when there are no targets --")]
	public bool m_removeLineIfNoTarget = true;

	private GameObject m_lineFx;

	private FriendlyEnemyVFXSelector m_lineFoFComp;

	private float m_lineDespawnTime;

	private bool m_lineSpawned;

	private GameObject m_onProjAttachFx;

	private JointPopupProperty m_startJointToUse;

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (m_lineStartEvent == null)
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
			if (!m_lineSpawnOnProjectileImpact)
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
				SpawnLineFX();
			}
		}
		m_markForRemovalAfterImpact = false;
		m_doHitsAsProjectileTravels = m_projectileDoHitsAsItTravelOverride;
	}

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		m_startJointToUse = m_lineFxCasterStartJoint;
		if (m_startJointOverrides == null)
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
			if (m_startJointOverrides.Count <= 0)
			{
				return;
			}
			foreach (IExtraSequenceParams extraSequenceParams in extraParams)
			{
				if (!(extraSequenceParams is GenericIntParam))
				{
					continue;
				}
				GenericIntParam genericIntParam = extraSequenceParams as GenericIntParam;
				if (genericIntParam.m_fieldIdentifier != GenericIntParam.FieldIdentifier.Index)
				{
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (genericIntParam.m_value <= 0)
				{
					continue;
				}
				int num = genericIntParam.m_value - 1;
				if (num < m_startJointOverrides.Count)
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
					m_startJointToUse = m_startJointOverrides[num];
				}
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	protected override void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		base.SpawnImpactFX(impactPos, impactRot);
		int num;
		if (m_removeLineIfNoTarget)
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
			num = ((base.Targets == null || base.Targets.Length == 0) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		if (m_lineSpawnOnProjectileImpact)
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
			if (!flag)
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
				SpawnLineFX();
			}
		}
		if (flag)
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
			DestroyLine();
		}
		if (m_onProjAttachFx == null)
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
			if (m_attachedFxOnHitPrefab != null)
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
				if (base.Targets != null)
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
					if (base.Targets.Length > 0)
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
						bool aimAtCaster = m_onProjHitFxAimMode != AimMode.None;
						bool reverseDir = m_onProjHitFxAimMode == AimMode.CasterToTarget;
						m_onProjAttachFx = Sequence.SpawnAndAttachFx(this, m_attachedFxOnHitPrefab, base.Targets[0], m_onProjHitJoint, m_onProjHitFxAttachToJoint, aimAtCaster, reverseDir);
					}
				}
			}
		}
		if (!m_lineAttachToHitTarget)
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
			if (base.Targets == null)
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
				if (base.Targets.Length > 0)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						m_lineOnHitTargetAttachJoint.Initialize(base.Targets[0].gameObject);
						return;
					}
				}
				return;
			}
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		base.OnAnimationEvent(parameter, sourceObject);
		if (m_lineStartEvent == parameter)
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
			SpawnLineFX();
		}
		if (!(m_lineRemoveEvent == parameter))
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
			DestroyLine();
			return;
		}
	}

	private void SpawnLineFX()
	{
		if (m_lineSpawned)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (!m_startJointToUse.IsInitialized())
		{
			GameObject referenceModel = GetReferenceModel(base.Caster, ReferenceModelType.Actor);
			if (referenceModel != null)
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
				m_startJointToUse.Initialize(referenceModel);
			}
		}
		if (m_linePrefab != null)
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
			Vector3 position = m_startJointToUse.m_jointObject.transform.position;
			m_lineFx = InstantiateFX(m_linePrefab, position, default(Quaternion));
			if (m_lineFx != null)
			{
				m_lineFoFComp = m_lineFx.GetComponent<FriendlyEnemyVFXSelector>();
			}
		}
		if (!string.IsNullOrEmpty(m_lineAudioEvent))
		{
			AudioManager.PostEvent(m_lineAudioEvent, base.Caster.gameObject);
		}
		if (m_lineDuration > 0f)
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
			m_lineDespawnTime = GameTime.time + m_lineDuration;
		}
		else
		{
			m_lineDespawnTime = -1f;
		}
		m_lineSpawned = true;
	}

	private void Update()
	{
		OnUpdate();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (!m_initialized)
		{
			return;
		}
		if (m_lineFx != null && base.Caster != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_lineFx != null)
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
				if (m_lineFoFComp != null)
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
					m_lineFoFComp.Setup(base.Caster.GetTeam());
				}
			}
		}
		if (m_lineFx != null)
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
			if (m_useProjectileStartPosAsStartPoint)
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
				if (m_spline != null)
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
					if (m_spline.pts.Length > 1)
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
						Vector3 value = m_spline.pts[1];
						if (m_lineStartPosUseGroundHeight)
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
							value.y = Board.Get().BaselineHeight;
						}
						Sequence.SetAttribute(m_lineFx, "startPoint", value);
						goto IL_01da;
					}
				}
			}
			if (m_startJointToUse.m_jointObject != null)
			{
				Vector3 position = m_startJointToUse.m_jointObject.transform.position;
				if (m_lineStartPosUseGroundHeight)
				{
					position.y = Board.Get().BaselineHeight;
				}
				if (m_lineStartYOffsetFromJoint != 0f)
				{
					position += m_lineStartYOffsetFromJoint * m_startJointToUse.m_jointObject.transform.up;
				}
				Sequence.SetAttribute(m_lineFx, "startPoint", position);
			}
			goto IL_01da;
		}
		goto IL_02af;
		IL_0273:
		Vector3 value2;
		if (m_lineEndPosUseGroundHeight)
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
			value2.y = Board.Get().BaselineHeight;
		}
		bool flag;
		if (flag)
		{
			Sequence.SetAttribute(m_lineFx, "endPoint", value2);
		}
		goto IL_02af;
		IL_01da:
		value2 = Vector3.zero;
		flag = true;
		if (m_lineAttachToHitTarget)
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
			if (m_lineOnHitTargetAttachJoint.IsInitialized())
			{
				value2 = m_lineOnHitTargetAttachJoint.m_jointObject.transform.position;
				goto IL_0273;
			}
		}
		if (m_startJointToUse.m_jointObject != null)
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
			if (m_fx != null)
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
				value2 = m_fx.transform.position;
				goto IL_0273;
			}
		}
		flag = false;
		goto IL_0273;
		IL_02af:
		if (!(m_lineDespawnTime < GameTime.time))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (m_lineDespawnTime > 0f)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					DestroyLine();
					return;
				}
			}
			return;
		}
	}

	protected override void OnSequenceDisable()
	{
		base.OnSequenceDisable();
		DestroyLine();
		if (!(m_onProjAttachFx != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Object.Destroy(m_onProjAttachFx);
			m_onProjAttachFx = null;
			return;
		}
	}

	private void DestroyLine()
	{
		if (m_lineFx != null)
		{
			Object.Destroy(m_lineFx);
			m_lineFx = null;
		}
		if (!(m_onProjAttachFx != null))
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
			Object.Destroy(m_onProjAttachFx);
			m_onProjAttachFx = null;
			return;
		}
	}

	public void ForceHideLine()
	{
		DestroyLine();
	}
}
