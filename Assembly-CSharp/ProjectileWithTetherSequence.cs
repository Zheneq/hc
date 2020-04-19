using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWithTetherSequence : ArcingProjectileSequence
{
	[Separator("Line FX Settings", "yellow")]
	public GameObject m_linePrefab;

	[Space(10f)]
	public float m_splineMaxHeight;

	[Header("    Line Info")]
	public bool m_lineSpawnOnProjectileImpact;

	[AnimEventPicker]
	public UnityEngine.Object m_lineStartEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_lineRemoveEvent;

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

	public ProjectileWithTetherSequence.AimMode m_onProjHitFxAimMode;

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
		if (this.m_lineStartEvent == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProjectileWithTetherSequence.FinishSetup()).MethodHandle;
			}
			if (!this.m_lineSpawnOnProjectileImpact)
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
				this.SpawnLineFX();
			}
		}
		this.m_markForRemovalAfterImpact = false;
		this.m_doHitsAsProjectileTravels = this.m_projectileDoHitsAsItTravelOverride;
	}

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		this.m_startJointToUse = this.m_lineFxCasterStartJoint;
		if (this.m_startJointOverrides != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProjectileWithTetherSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
			}
			if (this.m_startJointOverrides.Count > 0)
			{
				foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
				{
					if (extraSequenceParams is Sequence.GenericIntParam)
					{
						Sequence.GenericIntParam genericIntParam = extraSequenceParams as Sequence.GenericIntParam;
						if (genericIntParam.m_fieldIdentifier == Sequence.GenericIntParam.FieldIdentifier.Index)
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
							if (genericIntParam.m_value > 0)
							{
								int num = (int)(genericIntParam.m_value - 1);
								if (num < this.m_startJointOverrides.Count)
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
									this.m_startJointToUse = this.m_startJointOverrides[num];
								}
							}
						}
					}
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
	}

	protected override void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		base.SpawnImpactFX(impactPos, impactRot);
		bool flag;
		if (this.m_removeLineIfNoTarget)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProjectileWithTetherSequence.SpawnImpactFX(Vector3, Quaternion)).MethodHandle;
			}
			flag = (base.Targets == null || base.Targets.Length == 0);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (this.m_lineSpawnOnProjectileImpact)
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
			if (!flag2)
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
				this.SpawnLineFX();
			}
		}
		if (flag2)
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
			this.DestroyLine();
		}
		if (this.m_onProjAttachFx == null)
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
			if (this.m_attachedFxOnHitPrefab != null)
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
				if (base.Targets != null)
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
					if (base.Targets.Length > 0)
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
						bool aimAtCaster = this.m_onProjHitFxAimMode != ProjectileWithTetherSequence.AimMode.None;
						bool reverseDir = this.m_onProjHitFxAimMode == ProjectileWithTetherSequence.AimMode.CasterToTarget;
						this.m_onProjAttachFx = Sequence.SpawnAndAttachFx(this, this.m_attachedFxOnHitPrefab, base.Targets[0], this.m_onProjHitJoint, this.m_onProjHitFxAttachToJoint, aimAtCaster, reverseDir);
					}
				}
			}
		}
		if (this.m_lineAttachToHitTarget)
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
			if (base.Targets != null)
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
				if (base.Targets.Length > 0)
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
					this.m_lineOnHitTargetAttachJoint.Initialize(base.Targets[0].gameObject);
				}
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		base.OnAnimationEvent(parameter, sourceObject);
		if (this.m_lineStartEvent == parameter)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProjectileWithTetherSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.SpawnLineFX();
		}
		if (this.m_lineRemoveEvent == parameter)
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
			this.DestroyLine();
		}
	}

	private void SpawnLineFX()
	{
		if (this.m_lineSpawned)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProjectileWithTetherSequence.SpawnLineFX()).MethodHandle;
			}
			return;
		}
		if (!this.m_startJointToUse.IsInitialized())
		{
			GameObject referenceModel = base.GetReferenceModel(base.Caster, Sequence.ReferenceModelType.Actor);
			if (referenceModel != null)
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
				this.m_startJointToUse.Initialize(referenceModel);
			}
		}
		if (this.m_linePrefab != null)
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
			Vector3 position = this.m_startJointToUse.m_jointObject.transform.position;
			Quaternion rotation = default(Quaternion);
			this.m_lineFx = base.InstantiateFX(this.m_linePrefab, position, rotation, true, true);
			if (this.m_lineFx != null)
			{
				this.m_lineFoFComp = this.m_lineFx.GetComponent<FriendlyEnemyVFXSelector>();
			}
		}
		if (!string.IsNullOrEmpty(this.m_lineAudioEvent))
		{
			AudioManager.PostEvent(this.m_lineAudioEvent, base.Caster.gameObject);
		}
		if (this.m_lineDuration > 0f)
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
			this.m_lineDespawnTime = GameTime.time + this.m_lineDuration;
		}
		else
		{
			this.m_lineDespawnTime = -1f;
		}
		this.m_lineSpawned = true;
	}

	private void Update()
	{
		this.OnUpdate();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.m_initialized)
		{
			if (this.m_lineFx != null && base.Caster != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ProjectileWithTetherSequence.OnUpdate()).MethodHandle;
				}
				if (this.m_lineFx != null)
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
					if (this.m_lineFoFComp != null)
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
						this.m_lineFoFComp.Setup(base.Caster.\u000E());
					}
				}
			}
			if (this.m_lineFx != null)
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
				if (this.m_useProjectileStartPosAsStartPoint)
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
					if (this.m_spline != null)
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
						if (this.m_spline.pts.Length > 1)
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
							Vector3 value = this.m_spline.pts[1];
							if (this.m_lineStartPosUseGroundHeight)
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
								value.y = (float)Board.\u000E().BaselineHeight;
							}
							Sequence.SetAttribute(this.m_lineFx, "startPoint", value);
							goto IL_1DA;
						}
					}
				}
				if (this.m_startJointToUse.m_jointObject != null)
				{
					Vector3 vector = this.m_startJointToUse.m_jointObject.transform.position;
					if (this.m_lineStartPosUseGroundHeight)
					{
						vector.y = (float)Board.\u000E().BaselineHeight;
					}
					if (this.m_lineStartYOffsetFromJoint != 0f)
					{
						vector += this.m_lineStartYOffsetFromJoint * this.m_startJointToUse.m_jointObject.transform.up;
					}
					Sequence.SetAttribute(this.m_lineFx, "startPoint", vector);
				}
				IL_1DA:
				Vector3 value2 = Vector3.zero;
				bool flag = true;
				if (this.m_lineAttachToHitTarget)
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
					if (this.m_lineOnHitTargetAttachJoint.IsInitialized())
					{
						value2 = this.m_lineOnHitTargetAttachJoint.m_jointObject.transform.position;
						goto IL_273;
					}
				}
				if (this.m_startJointToUse.m_jointObject != null)
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
					if (this.m_fx != null)
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
						value2 = this.m_fx.transform.position;
						goto IL_273;
					}
				}
				flag = false;
				IL_273:
				if (this.m_lineEndPosUseGroundHeight)
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
					value2.y = (float)Board.\u000E().BaselineHeight;
				}
				if (flag)
				{
					Sequence.SetAttribute(this.m_lineFx, "endPoint", value2);
				}
			}
			if (this.m_lineDespawnTime < GameTime.time)
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
				if (this.m_lineDespawnTime > 0f)
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
					this.DestroyLine();
				}
			}
		}
	}

	protected override void OnSequenceDisable()
	{
		base.OnSequenceDisable();
		this.DestroyLine();
		if (this.m_onProjAttachFx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProjectileWithTetherSequence.OnSequenceDisable()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_onProjAttachFx);
			this.m_onProjAttachFx = null;
		}
	}

	private void DestroyLine()
	{
		if (this.m_lineFx != null)
		{
			UnityEngine.Object.Destroy(this.m_lineFx);
			this.m_lineFx = null;
		}
		if (this.m_onProjAttachFx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProjectileWithTetherSequence.DestroyLine()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_onProjAttachFx);
			this.m_onProjAttachFx = null;
		}
	}

	public void ForceHideLine()
	{
		this.DestroyLine();
	}

	public enum AimMode
	{
		None,
		TargetToCaster,
		CasterToTarget
	}
}
