using System;
using System.Collections.Generic;
using UnityEngine;

public class SetChildMaterialProperty : MonoBehaviour
{
	private GameObject m_positionObject;

	public string m_positionObjectName;

	public SetChildMaterialProperty.ObjectType m_objectType;

	public Vector3 m_positionOffset;

	private Renderer m_targetRenderer;

	public string m_targetRendererName;

	public SetChildMaterialProperty.PropertyType m_propertyType;

	public string m_materialPropertyName;

	private int m_propertyID;

	private Renderer[] m_renderers;

	private MaterialPropertyBlock m_propBlock;

	private Animator m_animator;

	private bool m_failedToFindTargetRenderer;

	private void Start()
	{
		this.m_propertyID = Shader.PropertyToID(this.m_materialPropertyName);
		this.m_propBlock = new MaterialPropertyBlock();
		if (this.m_positionObject == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SetChildMaterialProperty.Start()).MethodHandle;
			}
			this.m_positionObject = base.gameObject.FindInChildren(this.m_positionObjectName, 0);
		}
		if (this.m_positionObject == null)
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
			Debug.LogError(base.gameObject.name + " SetChildMaterialProperty component did not find position object: " + this.m_positionObjectName);
		}
		if (!this.m_targetRendererName.IsNullOrEmpty())
		{
			GameObject gameObject = base.gameObject.FindInChildren(this.m_targetRendererName, 0);
			if (gameObject)
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
				this.m_targetRenderer = gameObject.GetComponent<Renderer>();
			}
			else
			{
				Debug.LogError(base.name + " SetChildMaterialProperty did not find Target Renderer: " + this.m_targetRendererName);
				this.m_failedToFindTargetRenderer = true;
			}
		}
		this.m_animator = base.GetComponent<Animator>();
	}

	private void LateUpdate()
	{
		if (this.m_renderers == null)
		{
			this.m_renderers = base.GetComponentsInChildren<Renderer>();
		}
		if (this.m_positionObject != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SetChildMaterialProperty.LateUpdate()).MethodHandle;
			}
			Vector3 value = Vector3.zero;
			if (this.m_objectType == SetChildMaterialProperty.ObjectType.LocalPosition)
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
				value = this.m_positionObject.transform.localPosition + this.m_positionOffset;
			}
			else if (this.m_objectType == SetChildMaterialProperty.ObjectType.WorldPosition)
			{
				Transform transform = this.m_positionObject.transform;
				value = transform.TransformPoint(Vector3.Scale(this.m_positionOffset, new Vector3(1f / transform.lossyScale.x, 1f / transform.lossyScale.y, 1f / transform.lossyScale.z)));
			}
			else if (this.m_objectType == SetChildMaterialProperty.ObjectType.LocalScale)
			{
				value = this.m_positionObject.transform.localScale;
			}
			else
			{
				Debug.LogErrorFormat("SetChildMaterialProperty {0} has invalid object type {1} somehow", new object[]
				{
					base.name,
					this.m_objectType
				});
			}
			if (this.m_targetRenderer)
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
				this.Process(this.m_targetRenderer, value);
			}
			else if (!this.m_failedToFindTargetRenderer)
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
				for (int i = 0; i < this.m_renderers.Length; i++)
				{
					this.Process(this.m_renderers[i], value);
				}
			}
		}
	}

	private void Process(Renderer target, Vector3 value)
	{
		this.m_propBlock.Clear();
		target.GetPropertyBlock(this.m_propBlock);
		if (this.m_propertyType == SetChildMaterialProperty.PropertyType.Vector)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SetChildMaterialProperty.Process(Renderer, Vector3)).MethodHandle;
			}
			this.m_propBlock.SetVector(this.m_propertyID, value);
		}
		else if (this.m_propertyType == SetChildMaterialProperty.PropertyType.FloatFromXAxis)
		{
			this.m_propBlock.SetFloat(this.m_propertyID, value.x);
		}
		else if (this.m_propertyType == SetChildMaterialProperty.PropertyType.FloatFromYAxis)
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
			this.m_propBlock.SetFloat(this.m_propertyID, value.y);
		}
		else if (this.m_propertyType == SetChildMaterialProperty.PropertyType.FloatFromZAxis)
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
			this.m_propBlock.SetFloat(this.m_propertyID, value.z);
		}
		else if (this.m_propertyType == SetChildMaterialProperty.PropertyType.ColorRGB)
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
			this.m_propBlock.SetColor(this.m_propertyID, new Color(value.x, value.y, value.z));
		}
		else if (this.m_propertyType == SetChildMaterialProperty.PropertyType.VisibilityFromXAxis)
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
			target.gameObject.SetActive((double)value.x >= 0.1);
		}
		else if (this.m_propertyType == SetChildMaterialProperty.PropertyType.VisibilityFromYAxis)
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
			target.gameObject.SetActive((double)value.y >= 0.1);
		}
		else if (this.m_propertyType == SetChildMaterialProperty.PropertyType.VisibilityFromZAxis)
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
			target.gameObject.SetActive((double)value.z >= 0.1);
		}
		else if (this.m_propertyType == SetChildMaterialProperty.PropertyType.HideIfInRagdoll)
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
			if (this.m_animator != null)
			{
				target.gameObject.SetActiveIfNeeded(this.m_animator.enabled);
			}
		}
		else
		{
			Debug.LogErrorFormat("SetChildMaterialProperty {0} has invalid shader type {1} somehow", new object[]
			{
				base.name,
				this.m_propertyType
			});
		}
		target.SetPropertyBlock(this.m_propBlock);
	}

	public bool UsesPropertyName()
	{
		bool result;
		if (this.m_propertyType != SetChildMaterialProperty.PropertyType.VisibilityFromXAxis && this.m_propertyType != SetChildMaterialProperty.PropertyType.VisibilityFromYAxis)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SetChildMaterialProperty.UsesPropertyName()).MethodHandle;
			}
			result = (this.m_propertyType != SetChildMaterialProperty.PropertyType.VisibilityFromZAxis);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void ReinitRenderersList()
	{
		this.m_renderers = base.GetComponentsInChildren<Renderer>();
	}

	public unsafe bool DiffForSyncCharacterPrefab(SetChildMaterialProperty other, ref List<string> diffDescriptions)
	{
		bool result = true;
		string str = "\tSetChildMaterialProperty: ";
		if (other != null)
		{
			if (!this.m_positionObjectName.Equals(other.m_positionObjectName))
			{
				diffDescriptions.Add(str + "Position Object Name different");
				result = false;
			}
			if (this.m_objectType != other.m_objectType)
			{
				diffDescriptions.Add(str + "Object Type different");
				result = false;
			}
			if (!this.m_targetRendererName.Equals(other.m_targetRendererName))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SetChildMaterialProperty.DiffForSyncCharacterPrefab(SetChildMaterialProperty, List<string>*)).MethodHandle;
				}
				diffDescriptions.Add(str + "Target Renderer Name different");
				result = false;
			}
			if (this.m_propertyType != other.m_propertyType)
			{
				diffDescriptions.Add(str + "Property Type different");
				result = false;
			}
			if (!this.m_materialPropertyName.Equals(other.m_materialPropertyName))
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
				diffDescriptions.Add(str + "Material Property Name different");
				result = false;
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	public enum ObjectType
	{
		LocalPosition,
		WorldPosition,
		LocalScale
	}

	public enum PropertyType
	{
		Vector,
		FloatFromXAxis,
		FloatFromYAxis,
		FloatFromZAxis,
		ColorRGB,
		VisibilityFromXAxis,
		VisibilityFromYAxis,
		VisibilityFromZAxis,
		HideIfInRagdoll
	}
}
