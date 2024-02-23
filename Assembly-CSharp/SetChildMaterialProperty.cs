using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SetChildMaterialProperty : MonoBehaviour
{
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

	private GameObject m_positionObject;

	public string m_positionObjectName;

	public ObjectType m_objectType;

	public Vector3 m_positionOffset;

	private Renderer m_targetRenderer;

	public string m_targetRendererName;

	public PropertyType m_propertyType;

	public string m_materialPropertyName;

	private int m_propertyID;

	private Renderer[] m_renderers;

	private MaterialPropertyBlock m_propBlock;

	private Animator m_animator;

	private bool m_failedToFindTargetRenderer;

	private void Start()
	{
		m_propertyID = Shader.PropertyToID(m_materialPropertyName);
		m_propBlock = new MaterialPropertyBlock();
		if (m_positionObject == null)
		{
			m_positionObject = base.gameObject.FindInChildren(m_positionObjectName);
		}
		if (m_positionObject == null)
		{
			Debug.LogError(new StringBuilder().Append(base.gameObject.name).Append(" SetChildMaterialProperty component did not find position object: ").Append(m_positionObjectName).ToString());
		}
		if (!m_targetRendererName.IsNullOrEmpty())
		{
			GameObject gameObject = base.gameObject.FindInChildren(m_targetRendererName);
			if ((bool)gameObject)
			{
				m_targetRenderer = gameObject.GetComponent<Renderer>();
			}
			else
			{
				Debug.LogError(new StringBuilder().Append(base.name).Append(" SetChildMaterialProperty did not find Target Renderer: ").Append(m_targetRendererName).ToString());
				m_failedToFindTargetRenderer = true;
			}
		}
		m_animator = GetComponent<Animator>();
	}

	private void LateUpdate()
	{
		if (m_renderers == null)
		{
			m_renderers = GetComponentsInChildren<Renderer>();
		}
		if (!(m_positionObject != null))
		{
			return;
		}
		while (true)
		{
			Vector3 value = Vector3.zero;
			if (m_objectType == ObjectType.LocalPosition)
			{
				value = m_positionObject.transform.localPosition + m_positionOffset;
			}
			else if (m_objectType == ObjectType.WorldPosition)
			{
				Transform transform = m_positionObject.transform;
				Vector3 positionOffset = m_positionOffset;
				Vector3 lossyScale = transform.lossyScale;
				float x = 1f / lossyScale.x;
				Vector3 lossyScale2 = transform.lossyScale;
				float y = 1f / lossyScale2.y;
				Vector3 lossyScale3 = transform.lossyScale;
				value = transform.TransformPoint(Vector3.Scale(positionOffset, new Vector3(x, y, 1f / lossyScale3.z)));
			}
			else if (m_objectType == ObjectType.LocalScale)
			{
				value = m_positionObject.transform.localScale;
			}
			else
			{
				Debug.LogErrorFormat("SetChildMaterialProperty {0} has invalid object type {1} somehow", base.name, m_objectType);
			}
			if ((bool)m_targetRenderer)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						Process(m_targetRenderer, value);
						return;
					}
				}
			}
			if (m_failedToFindTargetRenderer)
			{
				return;
			}
			while (true)
			{
				for (int i = 0; i < m_renderers.Length; i++)
				{
					Process(m_renderers[i], value);
				}
				return;
			}
		}
	}

	private void Process(Renderer target, Vector3 value)
	{
		m_propBlock.Clear();
		target.GetPropertyBlock(m_propBlock);
		if (m_propertyType == PropertyType.Vector)
		{
			m_propBlock.SetVector(m_propertyID, value);
		}
		else if (m_propertyType == PropertyType.FloatFromXAxis)
		{
			m_propBlock.SetFloat(m_propertyID, value.x);
		}
		else if (m_propertyType == PropertyType.FloatFromYAxis)
		{
			m_propBlock.SetFloat(m_propertyID, value.y);
		}
		else if (m_propertyType == PropertyType.FloatFromZAxis)
		{
			m_propBlock.SetFloat(m_propertyID, value.z);
		}
		else if (m_propertyType == PropertyType.ColorRGB)
		{
			m_propBlock.SetColor(m_propertyID, new Color(value.x, value.y, value.z));
		}
		else if (m_propertyType == PropertyType.VisibilityFromXAxis)
		{
			target.gameObject.SetActive((double)value.x >= 0.1);
		}
		else if (m_propertyType == PropertyType.VisibilityFromYAxis)
		{
			target.gameObject.SetActive((double)value.y >= 0.1);
		}
		else if (m_propertyType == PropertyType.VisibilityFromZAxis)
		{
			target.gameObject.SetActive((double)value.z >= 0.1);
		}
		else if (m_propertyType == PropertyType.HideIfInRagdoll)
		{
			if (m_animator != null)
			{
				target.gameObject.SetActiveIfNeeded(m_animator.enabled);
			}
		}
		else
		{
			Debug.LogErrorFormat("SetChildMaterialProperty {0} has invalid shader type {1} somehow", base.name, m_propertyType);
		}
		target.SetPropertyBlock(m_propBlock);
	}

	public bool UsesPropertyName()
	{
		int result;
		if (m_propertyType != PropertyType.VisibilityFromXAxis && m_propertyType != PropertyType.VisibilityFromYAxis)
		{
			result = ((m_propertyType != PropertyType.VisibilityFromZAxis) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public void ReinitRenderersList()
	{
		m_renderers = GetComponentsInChildren<Renderer>();
	}

	public bool DiffForSyncCharacterPrefab(SetChildMaterialProperty other, ref List<string> diffDescriptions)
	{
		bool result = true;
		string str = "\tSetChildMaterialProperty: ";
		if (other != null)
		{
			if (!m_positionObjectName.Equals(other.m_positionObjectName))
			{
				diffDescriptions.Add(new StringBuilder().Append(str).Append("Position Object Name different").ToString());
				result = false;
			}
			if (m_objectType != other.m_objectType)
			{
				diffDescriptions.Add(new StringBuilder().Append(str).Append("Object Type different").ToString());
				result = false;
			}
			if (!m_targetRendererName.Equals(other.m_targetRendererName))
			{
				diffDescriptions.Add(new StringBuilder().Append(str).Append("Target Renderer Name different").ToString());
				result = false;
			}
			if (m_propertyType != other.m_propertyType)
			{
				diffDescriptions.Add(new StringBuilder().Append(str).Append("Property Type different").ToString());
				result = false;
			}
			if (!m_materialPropertyName.Equals(other.m_materialPropertyName))
			{
				diffDescriptions.Add(new StringBuilder().Append(str).Append("Material Property Name different").ToString());
				result = false;
			}
		}
		else
		{
			result = false;
		}
		return result;
	}
}
