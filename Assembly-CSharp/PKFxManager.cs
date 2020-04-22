using AOT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public static class PKFxManager
{
	public enum E_AvailableCamEvents
	{
		BeforeImageEffectsOpaque = 12,
		BeforeImageEffects = 18
	}

	[XmlRoot("PKFxGlobalConf")]
	public class PKFxConf
	{
		public bool enableFileLog = true;

		public bool enableEffectsKill;

		public bool enablePackFxInPersistentDataPath;

		public bool useOrthographicProjection;

		public E_AvailableCamEvents globalEventSetting = E_AvailableCamEvents.BeforeImageEffectsOpaque;

		public void Save()
		{
			string path = m_PackPath + "/PKconfig.cfg";
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PKFxConf));
			if (!Directory.Exists(Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
			FileStream fileStream = new FileStream(path, FileMode.Create);
			using (StreamWriter textWriter = new StreamWriter(fileStream, Encoding.ASCII))
			{
				xmlSerializer.Serialize(textWriter, this);
			}
			fileStream.Close();
		}
	}

	public enum GfxDeviceRenderer
	{
		kGfxRendererOpenGL,
		kGfxRendererD3D9,
		kGfxRendererD3D11,
		kGfxRendererGCM,
		kGfxRendererNull,
		kGfxRendererHollywood,
		kGfxRendererXenon,
		kGfxRendererOpenGLES,
		kGfxRendererOpenGLES20Mobile,
		kGfxRendererMolehill,
		kGfxRendererOpenGLES20Desktop
	}

	public enum GfxDeviceEventType
	{
		kGfxDeviceEventInitialize,
		kGfxDeviceEventShutdown,
		kGfxDeviceEventBeforeReset,
		kGfxDeviceEventAfterReset
	}

	public enum BaseType
	{
		Int = 22,
		Int2 = 23,
		Int3 = 24,
		Int4 = 25,
		Float = 28,
		Float2 = 29,
		Float3 = 30,
		Float4 = 0x1F
	}

	public enum DepthGrabFormat
	{
		Depth16Bits = 0x10,
		Depth24Bits = 24
	}

	public enum CamFlags
	{
		UseDepthGrabberTexture = 1,
		ScreenResolutionChanged
	}

	public struct CamDesc
	{
		public Matrix4x4 ViewMatrix;

		public Matrix4x4 ProjectionMatrix;

		public float DT;

		public int RenderPass;

		public float NearClip;

		public float FarClip;

		public IntPtr DepthRT;

		public int DepthBpp;

		public float LODBias;

		public int Flags;
	}

	public struct FxDesc
	{
		public string FxPath;

		public Matrix4x4 Transforms;
	}

	public struct ShaderDesc
	{
		public string ShaderPath;

		public string ShaderGroup;

		public int Api;

		public int VertexType;

		public int PixelType;
	}

	public enum EAttrDescFlag : byte
	{
		HasMin = 1,
		HasMax = 2,
		HasDesc = 4
	}

	public struct S_AttributeDesc
	{
		public int Type;

		public char MinMaxFlag;

		public IntPtr Name;

		public IntPtr Description;

		public float DefaultValue0;

		public float DefaultValue1;

		public float DefaultValue2;

		public float DefaultValue3;

		public float MinValue0;

		public float MinValue1;

		public float MinValue2;

		public float MinValue3;

		public float MaxValue0;

		public float MaxValue1;

		public float MaxValue2;

		public float MaxValue3;
	}

	public enum ESamplerType
	{
		SamplerShape,
		SamplerCurve,
		SamplerImage,
		SamplerText,
		SamplerUnsupported
	}

	public enum ETexcoordMode
	{
		Clamp,
		Wrap
	}

	public struct S_SamplerDesc
	{
		public int Type;

		public IntPtr Name;

		public IntPtr Description;
	}

	public struct S_ShaderConstantDesc
	{
		public int Type;

		public IntPtr Name;
	}

	public struct S_Stats
	{
		public float UpdateTime;

		public float RenderTime;

		public int TotalMemoryFootprint;

		public int TotalParticleMemory;

		public int UnusedParticleMemory;
	}

	public struct S_SoundDescriptor
	{
		public int ChannelGroup;

		public IntPtr Path;

		public IntPtr EventStart;

		public IntPtr EventStop;

		public Vector3 WorldPosition;

		public float Volume;

		public float StartTimeOffsetInSeconds;

		public float PlayTimeInSeconds;

		public int UserData;
	}

	[Serializable]
	public class SamplerDesc
	{
		public int Type;

		public string Name;

		public string Description;

		public SamplerDesc(SamplerDesc desc)
		{
			Type = desc.Type;
			Name = desc.Name;
		}

		public SamplerDesc(S_SamplerDesc desc)
		{
			Type = desc.Type;
			Name = Marshal.PtrToStringAnsi(desc.Name);
			Description = Marshal.PtrToStringAnsi(desc.Description);
		}

		public SamplerDesc(string name, int type)
		{
			Type = type;
			Name = name;
		}
	}

	public enum EShapeType
	{
		BoxShape,
		SphereShape,
		CylinderShape,
		CapsuleShape,
		MeshShape,
		SkinnedMeshShape
	}

	public enum EMeshChannels
	{
		Channel_Position = 1,
		Channel_Normal = 2,
		Channel_Tangent = 4,
		Channel_Velocity = 8,
		Channel_UV = 0x10,
		Channel_VertexColor = 0x20
	}

	public enum EImageFormat
	{
		Invalid = 0,
		BGR8 = 3,
		BGRA8 = 4,
		DXT1 = 8,
		DXT3 = 10,
		DXT5 = 12,
		RGB8_ETC1 = 0x10,
		RGB8_ETC2 = 17,
		RGBA8_ETC2 = 18,
		RGB8A1_ETC2 = 19,
		RGB4_PVRTC1 = 20,
		RGB2_PVRTC1 = 21,
		RGBA4_PVRTC1 = 22,
		RGBA2_PVRTC1 = 23
	}

	public class SamplerDescShapeBox
	{
		public Vector3 Center;

		public Vector3 Dimensions;

		public Vector3 EulerOrientation;

		public SamplerDescShapeBox()
		{
			Center = new Vector3(0f, 0f, 0f);
			Dimensions = new Vector3(1f, 1f, 1f);
			EulerOrientation = new Vector3(0f, 0f, 0f);
		}

		public SamplerDescShapeBox(Vector3 center, Vector3 dimension, Vector3 euler)
		{
			Center = center;
			Dimensions = dimension;
			EulerOrientation = euler;
		}
	}

	public class SamplerDescShapeSphere
	{
		public Vector3 Center;

		public float InnerRadius;

		public float Radius;

		public Vector3 EulerOrientation;

		public SamplerDescShapeSphere()
		{
			Center = new Vector3(0f, 0f, 0f);
			InnerRadius = 1f;
			Radius = 1f;
			EulerOrientation = new Vector3(0f, 0f, 0f);
		}

		public SamplerDescShapeSphere(Vector3 center, float radius, float innerRadius, Vector3 euler)
		{
			Center = center;
			InnerRadius = innerRadius;
			Radius = radius;
			EulerOrientation = euler;
		}
	}

	public class SamplerDescShapeCylinder
	{
		public Vector3 Center;

		public float InnerRadius;

		public float Radius;

		public float Height;

		public Vector3 EulerOrientation;

		public SamplerDescShapeCylinder()
		{
			Center = new Vector3(0f, 0f, 0f);
			InnerRadius = 1f;
			Radius = 1f;
			Height = 1f;
			EulerOrientation = new Vector3(0f, 0f, 0f);
		}

		public SamplerDescShapeCylinder(Vector3 center, float radius, float innerRadius, float height, Vector3 euler)
		{
			Center = center;
			InnerRadius = innerRadius;
			Radius = radius;
			Height = height;
			EulerOrientation = euler;
		}
	}

	public class SamplerDescShapeCapsule
	{
		public Vector3 Center;

		public float InnerRadius;

		public float Radius;

		public float Height;

		public Vector3 EulerOrientation;

		public SamplerDescShapeCapsule()
		{
			Center = new Vector3(0f, 0f, 0f);
			InnerRadius = 1f;
			Radius = 1f;
			Height = 1f;
			EulerOrientation = new Vector3(0f, 0f, 0f);
		}

		public SamplerDescShapeCapsule(Vector3 center, float radius, float innerRadius, float height, Vector3 euler)
		{
			Center = center;
			InnerRadius = innerRadius;
			Radius = radius;
			Height = height;
			EulerOrientation = euler;
		}
	}

	public class SamplerDescShapeMesh
	{
		public Vector3 Center;

		public Vector3 Dimensions;

		public Vector3 EulerOrientation;

		public Mesh Mesh;

		public int SamplingChannels;

		public SamplerDescShapeMesh()
		{
			Center = new Vector3(0f, 0f, 0f);
			Dimensions = new Vector3(1f, 1f, 1f);
			EulerOrientation = new Vector3(0f, 0f, 0f);
			SamplingChannels |= 1;
		}

		public SamplerDescShapeMesh(Vector3 center, Vector3 dimension, Vector3 euler, Mesh mesh, int samplingChannels)
		{
			Center = center;
			Dimensions = dimension;
			EulerOrientation = euler;
			Mesh = mesh;
			SamplingChannels = samplingChannels;
		}
	}

	public class SamplerDescShapeMeshFilter
	{
		public Vector3 Center;

		public Vector3 Dimensions;

		public Vector3 EulerOrientation;

		public MeshFilter MeshFilter;

		public int SamplingChannels;

		public SamplerDescShapeMeshFilter()
		{
			Center = new Vector3(0f, 0f, 0f);
			Dimensions = new Vector3(1f, 1f, 1f);
			EulerOrientation = new Vector3(0f, 0f, 0f);
			SamplingChannels |= 1;
		}

		public SamplerDescShapeMeshFilter(Vector3 center, Vector3 dimension, Vector3 euler, MeshFilter mesh, int samplingChannels)
		{
			Center = center;
			Dimensions = dimension;
			EulerOrientation = euler;
			MeshFilter = mesh;
			SamplingChannels = samplingChannels;
		}
	}

	public class SamplerDescShapeSkinnedMesh
	{
		public Vector3 Center;

		public Vector3 Dimensions;

		public Vector3 EulerOrientation;

		public SkinnedMeshRenderer SkinnedMesh;

		public int SamplingChannels;

		public SamplerDescShapeSkinnedMesh()
		{
			Center = new Vector3(0f, 0f, 0f);
			Dimensions = new Vector3(1f, 1f, 1f);
			EulerOrientation = new Vector3(0f, 0f, 0f);
			SamplingChannels |= 1;
		}

		public SamplerDescShapeSkinnedMesh(Vector3 center, Vector3 dimension, Vector3 euler, SkinnedMeshRenderer skinnedMesh, int samplingChannels)
		{
			Center = center;
			Dimensions = dimension;
			EulerOrientation = euler;
			SkinnedMesh = skinnedMesh;
			SamplingChannels = samplingChannels;
		}
	}

	[Serializable]
	public class Sampler
	{
		public class SkinnedMeshData
		{
			public float[] m_SkeletonDataBuffer;

			public Matrix4x4[] m_Bindposes;

			public void InitData(SkinnedMeshRenderer skinnedMeshRenderer)
			{
				m_SkeletonDataBuffer = new float[skinnedMeshRenderer.bones.Length * 16];
				m_Bindposes = skinnedMeshRenderer.sharedMesh.bindposes;
			}
		}

		public SamplerDesc m_Descriptor;

		public int m_ShapeType;

		public int m_EditorShapeType;

		public MeshFilter m_MeshFilter;

		public Mesh m_Mesh;

		public SkinnedMeshRenderer m_SkinnedMeshRenderer;

		public int m_MeshHashCode;

		public int m_SamplingChannels;

		public Vector3 m_ShapeCenter = Vector3.zero;

		public Vector3 m_Dimensions = Vector3.one;

		public Vector3 m_EulerOrientation = Vector3.zero;

		public Texture2D m_Texture;

		public bool m_TextureChanged;

		public ETexcoordMode m_TextureTexcoordMode;

		public AnimationCurve[] m_CurvesArray;

		public float[] m_CurvesTimeKeys;

		public string m_Text = string.Empty;

		public SkinnedMeshData m_SkinnedMeshData;

		public Sampler(SamplerDesc dsc)
		{
			m_Descriptor = new SamplerDesc(dsc);
			m_ShapeType = -1;
			m_EditorShapeType = -1;
		}

		public Sampler(string name, SamplerDescShapeBox dsc)
		{
			m_Descriptor = new SamplerDesc(name, 0);
			m_ShapeCenter = dsc.Center;
			m_Dimensions = dsc.Dimensions;
			m_EulerOrientation = dsc.EulerOrientation;
			m_ShapeType = 0;
			m_EditorShapeType = 0;
		}

		public Sampler(string name, SamplerDescShapeSphere dsc)
		{
			m_Descriptor = new SamplerDesc(name, 0);
			m_ShapeCenter = dsc.Center;
			m_Dimensions = new Vector3(dsc.Radius, dsc.InnerRadius);
			m_Dimensions.y = Mathf.Min(m_Dimensions.x, m_Dimensions.y);
			m_Dimensions.x = Mathf.Max(m_Dimensions.x, m_Dimensions.y);
			m_EulerOrientation = dsc.EulerOrientation;
			m_ShapeType = 1;
			m_EditorShapeType = 1;
		}

		public Sampler(string name, SamplerDescShapeCylinder dsc)
		{
			m_Descriptor = new SamplerDesc(name, 0);
			m_ShapeCenter = dsc.Center;
			m_Dimensions = new Vector3(dsc.Radius, dsc.InnerRadius, dsc.Height);
			m_Dimensions.y = Mathf.Min(m_Dimensions.x, m_Dimensions.y);
			m_Dimensions.x = Mathf.Max(m_Dimensions.x, m_Dimensions.y);
			m_EulerOrientation = dsc.EulerOrientation;
			m_ShapeType = 2;
			m_EditorShapeType = 2;
		}

		public Sampler(string name, SamplerDescShapeCapsule dsc)
		{
			m_Descriptor = new SamplerDesc(name, 0);
			m_ShapeCenter = dsc.Center;
			m_Dimensions = new Vector3(dsc.Radius, dsc.InnerRadius, dsc.Height);
			m_Dimensions.y = Mathf.Min(m_Dimensions.x, m_Dimensions.y);
			m_Dimensions.x = Mathf.Max(m_Dimensions.x, m_Dimensions.y);
			m_EulerOrientation = dsc.EulerOrientation;
			m_ShapeType = 3;
			m_EditorShapeType = 3;
		}

		public Sampler(string name, SamplerDescShapeMesh dsc)
		{
			m_Descriptor = new SamplerDesc(name, 0);
			m_ShapeCenter = dsc.Center;
			m_Dimensions = dsc.Dimensions;
			m_EulerOrientation = dsc.EulerOrientation;
			m_Mesh = dsc.Mesh;
			m_MeshFilter = null;
			m_SkinnedMeshRenderer = null;
			if (m_Mesh != null)
			{
				m_MeshHashCode = m_Mesh.name.GetHashCode();
			}
			else
			{
				m_MeshHashCode = 0;
			}
			m_SkinnedMeshRenderer = null;
			m_SamplingChannels = dsc.SamplingChannels;
			m_ShapeType = 4;
			m_EditorShapeType = 4;
		}

		public Sampler(string name, SamplerDescShapeMeshFilter dsc)
		{
			m_Descriptor = new SamplerDesc(name, 0);
			m_ShapeCenter = dsc.Center;
			m_Dimensions = dsc.Dimensions;
			m_EulerOrientation = dsc.EulerOrientation;
			m_MeshFilter = dsc.MeshFilter;
			m_Mesh = m_MeshFilter.sharedMesh;
			m_SkinnedMeshRenderer = null;
			if (m_Mesh != null)
			{
				m_MeshHashCode = m_Mesh.name.GetHashCode();
			}
			else
			{
				m_MeshHashCode = 0;
			}
			m_SamplingChannels = dsc.SamplingChannels;
			m_ShapeType = 4;
			m_EditorShapeType = 5;
		}

		public Sampler(string name, SamplerDescShapeSkinnedMesh dsc)
		{
			m_Descriptor = new SamplerDesc(name, 0);
			m_ShapeCenter = dsc.Center;
			m_Dimensions = dsc.Dimensions;
			m_EulerOrientation = dsc.EulerOrientation;
			m_SkinnedMeshRenderer = dsc.SkinnedMesh;
			m_Mesh = dsc.SkinnedMesh.sharedMesh;
			m_MeshFilter = null;
			if (m_Mesh != null)
			{
				m_MeshHashCode = m_Mesh.name.GetHashCode();
			}
			else
			{
				m_MeshHashCode = 0;
			}
			m_SamplingChannels = dsc.SamplingChannels;
			m_ShapeType = 5;
			m_EditorShapeType = 6;
		}

		public Sampler(string name, AnimationCurve[] curvesArray)
		{
			m_Descriptor = new SamplerDesc(name, 1);
			m_CurvesArray = curvesArray;
			if (m_CurvesArray.Length != 0)
			{
				int num = 0;
				m_CurvesTimeKeys = new float[m_CurvesArray[0].keys.Length];
				Keyframe[] keys = m_CurvesArray[0].keys;
				foreach (Keyframe keyframe in keys)
				{
					m_CurvesTimeKeys[num++] = keyframe.time;
				}
			}
			m_ShapeType = -1;
			m_EditorShapeType = -1;
		}

		public Sampler(string name, Texture2D texture, ETexcoordMode texcoordMode)
		{
			m_Descriptor = new SamplerDesc(name, 2);
			m_Texture = texture;
			m_TextureChanged = true;
			m_TextureTexcoordMode = texcoordMode;
			m_ShapeType = -1;
			m_EditorShapeType = -1;
		}

		public Sampler(string name, string text)
		{
			m_Descriptor = new SamplerDesc(name, 3);
			m_Text = text;
			m_ShapeType = -1;
			m_EditorShapeType = -1;
		}

		public void Copy(Sampler other)
		{
			m_ShapeType = other.m_ShapeType;
			m_EditorShapeType = other.m_EditorShapeType;
			m_Mesh = other.m_Mesh;
			m_SkinnedMeshRenderer = other.m_SkinnedMeshRenderer;
			m_MeshHashCode = other.m_MeshHashCode;
			m_SamplingChannels = other.m_SamplingChannels;
			m_ShapeCenter = other.m_ShapeCenter;
			m_Dimensions = other.m_Dimensions;
			m_EulerOrientation = other.m_EulerOrientation;
			m_Texture = other.m_Texture;
			m_TextureChanged = other.m_TextureChanged;
			m_TextureTexcoordMode = other.m_TextureTexcoordMode;
			m_CurvesArray = other.m_CurvesArray;
			m_CurvesTimeKeys = other.m_CurvesTimeKeys;
			m_Text = other.m_Text;
			m_SkinnedMeshData = other.m_SkinnedMeshData;
		}
	}

	[Serializable]
	public class AttributeDesc
	{
		public BaseType Type;

		public int MinMaxFlag;

		public string Name;

		public string Description;

		public float DefaultValue0;

		public float DefaultValue1;

		public float DefaultValue2;

		public float DefaultValue3;

		public float MinValue0;

		public float MinValue1;

		public float MinValue2;

		public float MinValue3;

		public float MaxValue0;

		public float MaxValue1;

		public float MaxValue2;

		public float MaxValue3;

		public AttributeDesc(S_AttributeDesc desc)
		{
			Type = (BaseType)desc.Type;
			MinMaxFlag = desc.MinMaxFlag;
			Name = Marshal.PtrToStringAnsi(desc.Name);
			Description = Marshal.PtrToStringAnsi(desc.Description);
			DefaultValue0 = desc.DefaultValue0;
			DefaultValue1 = desc.DefaultValue1;
			DefaultValue2 = desc.DefaultValue2;
			DefaultValue3 = desc.DefaultValue3;
			MinValue0 = desc.MinValue0;
			MinValue1 = desc.MinValue1;
			MinValue2 = desc.MinValue2;
			MinValue3 = desc.MinValue3;
			MaxValue0 = desc.MaxValue0;
			MaxValue1 = desc.MaxValue1;
			MaxValue2 = desc.MaxValue2;
			MaxValue3 = desc.MaxValue3;
		}

		public AttributeDesc(BaseType type, IntPtr name)
		{
			Type = type;
			Name = Marshal.PtrToStringAnsi(name);
		}

		public AttributeDesc(BaseType type, string name)
		{
			Type = type;
			Name = name;
		}
	}

	[Serializable]
	public class Attribute
	{
		public AttributeDesc m_Descriptor;

		public float m_Value0;

		public float m_Value1;

		public float m_Value2;

		public float m_Value3;

		public float ValueFloat
		{
			get
			{
				return m_Value0;
			}
			set
			{
				m_Value0 = value;
			}
		}

		public Vector2 ValueFloat2
		{
			get
			{
				return new Vector2(m_Value0, m_Value1);
			}
			set
			{
				m_Value0 = value.x;
				m_Value1 = value.y;
			}
		}

		public Vector3 ValueFloat3
		{
			get
			{
				return new Vector3(m_Value0, m_Value1, m_Value2);
			}
			set
			{
				m_Value0 = value.x;
				m_Value1 = value.y;
				m_Value2 = value.z;
			}
		}

		public Vector4 ValueFloat4
		{
			get
			{
				return new Vector4(m_Value0, m_Value1, m_Value2, m_Value3);
			}
			set
			{
				m_Value0 = value.x;
				m_Value1 = value.y;
				m_Value2 = value.z;
				m_Value3 = value.w;
			}
		}

		public int ValueInt
		{
			get
			{
				return ftoi(m_Value0);
			}
			set
			{
				m_Value0 = itof(value);
			}
		}

		public int[] ValueInt2
		{
			get
			{
				return new int[2]
				{
					ftoi(m_Value0),
					ftoi(m_Value1)
				};
			}
			set
			{
				m_Value0 = itof(value[0]);
				m_Value1 = itof(value[1]);
			}
		}

		public int[] ValueInt3
		{
			get
			{
				return new int[3]
				{
					ftoi(m_Value0),
					ftoi(m_Value1),
					ftoi(m_Value2)
				};
			}
			set
			{
				m_Value0 = itof(value[0]);
				m_Value1 = itof(value[1]);
				m_Value2 = itof(value[2]);
			}
		}

		public int[] ValueInt4
		{
			get
			{
				return new int[4]
				{
					ftoi(m_Value0),
					ftoi(m_Value1),
					ftoi(m_Value2),
					ftoi(m_Value3)
				};
			}
			set
			{
				m_Value0 = itof(value[0]);
				m_Value1 = itof(value[1]);
				m_Value2 = itof(value[2]);
				m_Value3 = itof(value[3]);
			}
		}

		public Attribute(S_AttributeDesc desc)
		{
			m_Descriptor = new AttributeDesc(desc);
			m_Value0 = desc.DefaultValue0;
			m_Value1 = desc.DefaultValue1;
			m_Value2 = desc.DefaultValue2;
			m_Value3 = desc.DefaultValue3;
		}

		public Attribute(AttributeDesc desc)
		{
			m_Descriptor = desc;
			m_Value0 = desc.DefaultValue0;
			m_Value1 = desc.DefaultValue1;
			m_Value2 = desc.DefaultValue2;
			m_Value3 = desc.DefaultValue3;
		}

		public Attribute(string name, float val)
		{
			m_Descriptor = new AttributeDesc(BaseType.Float, name);
			ValueFloat = val;
		}

		public Attribute(string name, Vector2 val)
		{
			m_Descriptor = new AttributeDesc(BaseType.Float2, name);
			ValueFloat2 = val;
		}

		public Attribute(string name, Vector3 val)
		{
			m_Descriptor = new AttributeDesc(BaseType.Float3, name);
			ValueFloat3 = val;
		}

		public Attribute(string name, Vector4 val)
		{
			m_Descriptor = new AttributeDesc(BaseType.Float4, name);
			ValueFloat4 = val;
		}

		public Attribute(string name, int val)
		{
			m_Descriptor = new AttributeDesc(BaseType.Int, name);
			ValueInt = val;
		}

		public Attribute(string name, int[] val)
		{
			if (val.Length >= 1)
			{
				m_Descriptor = new AttributeDesc((BaseType)(22 + val.Length - 1), name);
				m_Value0 = itof(val[0]);
			}
			if (val.Length >= 2)
			{
				m_Value1 = itof(val[1]);
			}
			if (val.Length >= 3)
			{
				m_Value2 = itof(val[2]);
			}
			if (val.Length < 4)
			{
				return;
			}
			while (true)
			{
				m_Value3 = itof(val[3]);
				return;
			}
		}
	}

	[Serializable]
	public class ShaderConstantDesc
	{
		public BaseType Type;

		public string Name;

		public int MinMaxFlag;

		public string Description;

		public ShaderConstantDesc(S_ShaderConstantDesc desc)
		{
			Type = (BaseType)desc.Type;
			Name = Marshal.PtrToStringAnsi(desc.Name);
			MinMaxFlag = 0;
		}

		public ShaderConstantDesc(BaseType type, IntPtr name)
		{
			Type = type;
			Name = Marshal.PtrToStringAnsi(name);
			MinMaxFlag = 0;
		}

		public ShaderConstantDesc(BaseType type, string name)
		{
			Type = type;
			Name = name;
			MinMaxFlag = 0;
		}
	}

	[Serializable]
	public class ShaderConstant
	{
		public ShaderConstantDesc m_Descriptor;

		public float m_Value0;

		public float m_Value1;

		public float m_Value2;

		public float m_Value3;

		public float ValueFloat
		{
			get
			{
				return m_Value0;
			}
			set
			{
				m_Value0 = value;
			}
		}

		public Vector2 ValueFloat2
		{
			get
			{
				return new Vector2(m_Value0, m_Value1);
			}
			set
			{
				m_Value0 = value.x;
				m_Value1 = value.y;
			}
		}

		public Vector3 ValueFloat3
		{
			get
			{
				return new Vector3(m_Value0, m_Value1, m_Value2);
			}
			set
			{
				m_Value0 = value.x;
				m_Value1 = value.y;
				m_Value2 = value.z;
			}
		}

		public Vector4 ValueFloat4
		{
			get
			{
				return new Vector4(m_Value0, m_Value1, m_Value2, m_Value3);
			}
			set
			{
				m_Value0 = value.x;
				m_Value1 = value.y;
				m_Value2 = value.z;
				m_Value3 = value.w;
			}
		}

		public ShaderConstant(S_ShaderConstantDesc desc)
		{
			m_Descriptor = new ShaderConstantDesc(desc);
			m_Value0 = 0f;
			m_Value1 = 0f;
			m_Value2 = 0f;
			m_Value3 = 0f;
		}

		public ShaderConstant(ShaderConstantDesc desc)
		{
			m_Descriptor = desc;
			m_Value0 = 0f;
			m_Value1 = 0f;
			m_Value2 = 0f;
			m_Value3 = 0f;
		}

		public ShaderConstant(string name, float val)
		{
			m_Descriptor = new ShaderConstantDesc(BaseType.Float, name);
			ValueFloat = val;
		}

		public ShaderConstant(string name, Vector2 val)
		{
			m_Descriptor = new ShaderConstantDesc(BaseType.Float2, name);
			ValueFloat2 = val;
		}

		public ShaderConstant(string name, Vector3 val)
		{
			m_Descriptor = new ShaderConstantDesc(BaseType.Float3, name);
			ValueFloat3 = val;
		}

		public ShaderConstant(string name, Vector4 val)
		{
			m_Descriptor = new ShaderConstantDesc(BaseType.Float4, name);
			ValueFloat4 = val;
		}
	}

	[Serializable]
	public class SoundDescriptor
	{
		public int ChannelGroup;

		public string Path;

		public string EventStart;

		public string EventStop;

		public Vector3 WorldPosition;

		public float Volume;

		public float StartTimeOffsetInSeconds;

		public float PlayTimeInSeconds;

		public int UserData;

		public SoundDescriptor(S_SoundDescriptor desc)
		{
			ChannelGroup = desc.ChannelGroup;
			Path = Marshal.PtrToStringAnsi(desc.Path);
			EventStart = Marshal.PtrToStringAnsi(desc.EventStart);
			EventStop = Marshal.PtrToStringAnsi(desc.EventStop);
			WorldPosition = desc.WorldPosition;
			Volume = desc.Volume;
			StartTimeOffsetInSeconds = desc.StartTimeOffsetInSeconds;
			PlayTimeInSeconds = desc.PlayTimeInSeconds;
			UserData = desc.UserData;
		}
	}

	private delegate void FxCallback(int guid);

	private delegate void FxHotReloadCallback(int guid, int newGuid);

	public delegate IntPtr AudioCallback(IntPtr channelName, IntPtr nbSamples);

	internal static HashSet<string> m_preloadedPKFXPaths;

	public const uint POPCORN_MAGIC_NUMBER = 1526595584u;

	public const uint PK_DESC_NAME_MAX_LEN = 64u;

	public const uint PK_DESC_DESC_MAX_LEN = 128u;

	private const string kPopcornPluginName = "PK-UnityPlugin";

	private const string m_UnityVersion = "Unity 5.2 and up";

	public const string m_PluginVersion = "2.9p6 for Unity 5.2 and up";

	public static string m_PackPath;

	public static string m_CurrentVersionString;

	public static bool m_PackCopied;

	public static bool m_PackLoaded;

	public static PKFxConf m_GlobalConf;

	public static string m_LogFilePath;

	public static bool m_IsStarted;

	private static float[] m_Samples;

	private static GCHandle m_SamplesHandle;

	private static bool m_HasSpawnerIDs;

	private static bool m_HasFileLogging;

	private static bool m_IsUsingOrthographicProjection;

	static PKFxManager()
	{
		m_preloadedPKFXPaths = new HashSet<string>();
		m_PackPath = Application.streamingAssetsPath;
		m_CurrentVersionString = string.Empty;
		m_PackCopied = false;
		m_PackLoaded = false;
		m_LogFilePath = Path.GetFullPath(Path.Combine(Application.dataPath, "../popcorn.htm"));
		m_IsStarted = false;
		m_HasSpawnerIDs = false;
		m_HasFileLogging = false;
		m_IsUsingOrthographicProjection = false;
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(PKFxConf));
		if (Application.platform == RuntimePlatform.Android)
		{
			m_PackPath = Application.persistentDataPath;
			IEnumerator<object> enumerator = AndroidRetrieveConfFile().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						goto end_IL_0097;
					}
				}
				end_IL_0097:;
			}
			finally
			{
				if (enumerator != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							enumerator.Dispose();
							goto end_IL_00c1;
						}
					}
				}
				end_IL_00c1:;
			}
		}
		string text = m_PackPath + "/PKconfig.cfg";
		if (File.Exists(text))
		{
			FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read);
			StreamReader textReader = new StreamReader(fileStream, Encoding.ASCII);
			m_GlobalConf = (xmlSerializer.Deserialize(textReader) as PKFxConf);
			fileStream.Close();
		}
		else
		{
			Debug.LogWarning("[PKFX] Can't find conf file : " + text);
			m_GlobalConf = new PKFxConf();
			m_GlobalConf.Save();
		}
		EnableFileLoggingIFN(m_GlobalConf.enableFileLog);
		SetupPackInPersistantDataPathIFN(m_GlobalConf.enablePackFxInPersistentDataPath);
	}

	internal static void UnloadAll()
	{
		using (HashSet<string>.Enumerator enumerator = m_preloadedPKFXPaths.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				UnloadEffect(current);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					goto end_IL_000d;
				}
			}
			end_IL_000d:;
		}
		m_preloadedPKFXPaths.Clear();
	}

	[DllImport("PK-UnityPlugin")]
	public static extern IntPtr GetRuntimeVersion();

	[DllImport("PK-UnityPlugin")]
	public static extern void GetStats(ref S_Stats stats);

	[DllImport("PK-UnityPlugin")]
	public static extern int LoadFx(FxDesc path);

	[DllImport("PK-UnityPlugin")]
	public static extern bool StopFx(int guid);

	[DllImport("PK-UnityPlugin")]
	public static extern bool TerminateFx(int guid);

	[DllImport("PK-UnityPlugin")]
	public static extern bool KillFx(int guid);

	[DllImport("PK-UnityPlugin")]
	public static extern bool IsFxAlive(int guid);

	[DllImport("PK-UnityPlugin")]
	public static extern void PreLoadFxIFN(string path);

	[DllImport("PK-UnityPlugin")]
	public static extern uint LoadShader(ShaderDesc desc);

	[DllImport("PK-UnityPlugin")]
	public static extern void UnloadShader(string path);

	[DllImport("PK-UnityPlugin")]
	public static extern IntPtr GetDefaultShaderString(int api, int type);

	[DllImport("PK-UnityPlugin")]
	public static extern int ShaderConstantsCount(string path, int api);

	[DllImport("PK-UnityPlugin")]
	public static extern bool ShaderFillConstantDesc(string path, int constantId, ref S_ShaderConstantDesc desc);

	[DllImport("PK-UnityPlugin")]
	public static extern bool ShaderSetConstant(uint shaderId, int constantCount, [In] IntPtr attributes);

	[DllImport("PK-UnityPlugin")]
	public static extern bool LoadPack(string path);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectSetTransforms(int guid, Matrix4x4 tranforms);

	[DllImport("PK-UnityPlugin")]
	public static extern int EffectSamplersCountFromFx(string fxName);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectFillSamplerDescFromFx(string fxName, int samplerID, ref S_SamplerDesc desc);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectSetSamplers(int guid, int samplerCount, [In] IntPtr samplers);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectUpdateSamplerSkinning(int guid, int samplerId, [In] IntPtr samplers, float dt);

	[DllImport("PK-UnityPlugin")]
	public static extern int EffectAttributesCount(int guid);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectFillAttributeDesc(int fxGUID, int attrID, ref S_AttributeDesc desc);

	[DllImport("PK-UnityPlugin")]
	public static extern int EffectAttributesCountFromFx(string fxName);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectFillAttributeDescFromFx(string fxName, int attrID, ref S_AttributeDesc desc);

	[DllImport("PK-UnityPlugin")]
	public static extern bool EffectSetAttributes(int guid, int attributeCount, [In] IntPtr attributes);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetDelegateOnFxStopped(IntPtr delegatePtr);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetDelegateOnFxHotReloaded(IntPtr delegatePtr);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetDelegateOnAudioSpectrumData(IntPtr delegatePtr);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetDelegateOnAudioWaveformData(IntPtr delegatePtr);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetDelegateOnStartSound(IntPtr delegatePtr);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetReversedZBuffer(bool zBufferReversed);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetupColorSpace(bool isSRGB);

	[DllImport("PK-UnityPlugin")]
	public static extern bool UnloadEffect(string path);

	[DllImport("PK-UnityPlugin")]
	public static extern void LogicalUpdate(float dt);

	[DllImport("PK-UnityPlugin")]
	public static extern void UpdateParticles(CamDesc desc);

	[DllImport("PK-UnityPlugin")]
	public static extern void UpdateCamDesc(int camID, CamDesc desc, bool update);

	[DllImport("PK-UnityPlugin")]
	public static extern void Reset();

	[DllImport("PK-UnityPlugin")]
	public static extern void DeepReset();

	[DllImport("PK-UnityPlugin")]
	public static extern bool LoadPkmmAsSceneMesh(string pkmmVirtualPath);

	[DllImport("PK-UnityPlugin")]
	public static extern void SceneMeshClear();

	[DllImport("PK-UnityPlugin")]
	public static extern bool SceneMeshAddRawMesh(int indicesCount, int[] indices, int verticesCount, Vector3[] vertices, Vector3[] normals, Matrix4x4 MeshMatrix);

	[DllImport("PK-UnityPlugin")]
	public static extern int SceneMeshBuild(string outputPkmmVirtualPath);

	[DllImport("PK-UnityPlugin")]
	public static extern void UnitySetGraphicsDevice(IntPtr device, int deviceType, int eventType);

	[DllImport("PK-UnityPlugin")]
	public static extern void UnityRenderEvent(int camID);

	[DllImport("PK-UnityPlugin")]
	public static extern void EnableSpawnerIDs(bool enable);

	[DllImport("PK-UnityPlugin")]
	public static extern void WriteProfileReport(string path);

	[DllImport("PK-UnityPlugin")]
	public static extern void ProfilerSetEnable(bool enable);

	[DllImport("PK-UnityPlugin")]
	public static extern IntPtr GetRenderEventFunc();

	[DllImport("PK-UnityPlugin")]
	public static extern IntPtr GetGLConstantsCountEvent();

	[DllImport("PK-UnityPlugin")]
	public static extern void GLConstantsCountEvent(int eventId);

	[DllImport("PK-UnityPlugin")]
	public static extern void SetUseOrthographicProjection(bool enable);

	[DllImport("PK-UnityPlugin")]
	public static extern void TransformAllParticles(Matrix4x4 transform);

	private static int ftoi(float fff)
	{
		return BitConverter.ToInt32(BitConverter.GetBytes(fff), 0);
	}

	private static float itof(int i)
	{
		return BitConverter.ToSingle(BitConverter.GetBytes(i), 0);
	}

	public static bool SceneMeshAddMesh(Mesh mesh, Matrix4x4 localToWorldMatrix)
	{
		int subMeshCount = mesh.subMeshCount;
		if (subMeshCount <= 0)
		{
			Debug.LogError("[PKFX] Mesh doesn't have sub meshes");
			return false;
		}
		int vertexCount = mesh.vertexCount;
		if (mesh.subMeshCount > 1)
		{
			Debug.LogWarning("[PKFX] Mesh has more than 1 submesh: non opti");
		}
		for (int num = 0; num < mesh.subMeshCount; num++)
		{
			int indicesCount = mesh.GetIndices(num).Length;
			Debug.Log("[PKFX] Mesh (" + (num + 1) + "/" + subMeshCount + ") idx:" + indicesCount + " v:" + vertexCount + " v:" + mesh.vertices.Length + " n:" + mesh.normals.Length + " uv:" + mesh.uv.Length);
			if (mesh.vertices.Length == vertexCount)
			{
				if (mesh.normals.Length == vertexCount)
				{
					goto IL_017a;
				}
			}
			Debug.LogError("[PKFX] Invalid mesh");
			goto IL_017a;
			IL_017a:
			if (!SceneMeshAddRawMesh(indicesCount, mesh.GetIndices(num), vertexCount, mesh.vertices, mesh.normals, localToWorldMatrix))
			{
				Debug.LogError("[PKFX] Fail to load raw mesh");
			}
		}
		while (true)
		{
			return true;
		}
	}

	public static void Render(short cameraID)
	{
		if (cameraID < 0)
		{
			Debug.LogError("[PKFX] PKFxManager: invalid cameraID for rendering " + cameraID);
		}
	}

	private static IEnumerable<object> AndroidRetrieveConfFile()
	{
		WWW www = new WWW(Path.Combine(Application.streamingAssetsPath, "PKconfig.cfg"));
		while (!www.isDone)
		{
			yield return www;
		}
		while (true)
		{
			File.WriteAllBytes(m_PackPath + "/PKconfig.cfg", www.bytes);
			www.Dispose();
			yield break;
		}
	}

	public static void Startup()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			UnitySetGraphicsDevice(IntPtr.Zero, 8, 0);
		}
		if (SystemInfo.usesReversedZBuffer)
		{
			SetReversedZBuffer(true);
		}
		SetupColorSpace(QualitySettings.activeColorSpace == ColorSpace.Linear);
		EnableKillIndividualEffect(m_GlobalConf.enableEffectsKill);
		SetUseOrthographicProjection(m_GlobalConf.useOrthographicProjection);
		m_IsUsingOrthographicProjection = m_GlobalConf.useOrthographicProjection;
		SetDelegateOnAudioSpectrumData(Marshal.GetFunctionPointerForDelegate(new Func<IntPtr, IntPtr, IntPtr>(OnAudioSpectrumData)));
		SetDelegateOnAudioWaveformData(Marshal.GetFunctionPointerForDelegate(new Func<IntPtr, IntPtr, IntPtr>(OnAudioWaveformData)));
		SetDelegateOnFxStopped(Marshal.GetFunctionPointerForDelegate(new Action<int>(OnFxStopped)));
		SetDelegateOnFxHotReloaded(Marshal.GetFunctionPointerForDelegate(new Action<int, int>(OnFxHotReloaded)));
		SetDelegateOnStartSound(Marshal.GetFunctionPointerForDelegate(new Action<IntPtr>(PKFxSoundManager.OnStartSound)));
		m_Samples = new float[1024];
		m_SamplesHandle = GCHandle.Alloc(m_Samples, GCHandleType.Pinned);
		m_CurrentVersionString = Marshal.PtrToStringAnsi(GetRuntimeVersion());
		m_IsStarted = true;
	}

	public static bool TryLoadPackRelative()
	{
		string text = Application.dataPath;
		string packPath = m_PackPath;
		if (string.IsNullOrEmpty(text))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (string.IsNullOrEmpty(packPath))
		{
			return false;
		}
		int num = text.LastIndexOf("/");
		if (num > 0)
		{
			text = text.Substring(0, num);
		}
		Uri uri = new Uri(text + "/");
		Uri uri2 = new Uri(packPath + "/");
		if (uri.Scheme != uri2.Scheme)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		Uri uri3 = uri.MakeRelativeUri(uri2);
		string str = Uri.UnescapeDataString(uri3.ToString());
		return LoadPack(str + "PackFx");
	}

	[MonoPInvokeCallback(typeof(FxCallback))]
	public static void OnFxStopped(int guid)
	{
		if (!PKFxFX.m_ListEffects.TryGetValue(guid, out PKFxFX value))
		{
			return;
		}
		while (true)
		{
			if (value.m_OnFxStopped != null)
			{
				value.m_OnFxStopped(value);
			}
			value.OnFxStopPlaying();
			return;
		}
	}

	[MonoPInvokeCallback(typeof(FxHotReloadCallback))]
	public static void OnFxHotReloaded(int guid, int newGuid)
	{
		if (!PKFxFX.m_ListEffects.TryGetValue(guid, out PKFxFX value))
		{
			return;
		}
		while (true)
		{
			value.OnFxHotReloaded(newGuid);
			return;
		}
	}

	[MonoPInvokeCallback(typeof(AudioCallback))]
	public static IntPtr OnAudioSpectrumData(IntPtr channelName, IntPtr nbSamples)
	{
		AudioListener.GetSpectrumData(m_Samples, 0, FFTWindow.Rectangular);
		m_Samples[1023] = m_Samples[1022];
		return m_SamplesHandle.AddrOfPinnedObject();
	}

	[MonoPInvokeCallback(typeof(AudioCallback))]
	public static IntPtr OnAudioWaveformData(IntPtr channelName, IntPtr nbSamples)
	{
		AudioListener.GetOutputData(m_Samples, 0);
		return m_SamplesHandle.AddrOfPinnedObject();
	}

	public static List<AttributeDesc> ListEffectAttributesFromGUID(int FxGUID)
	{
		List<AttributeDesc> list = new List<AttributeDesc>();
		int num = EffectAttributesCount(FxGUID);
		for (int i = 0; i < num; i++)
		{
			S_AttributeDesc desc = default(S_AttributeDesc);
			desc.Name = Marshal.AllocHGlobal(64);
			desc.Description = Marshal.AllocHGlobal(128);
			if (EffectFillAttributeDesc(FxGUID, i, ref desc))
			{
				list.Add(new AttributeDesc(desc));
			}
			Marshal.FreeHGlobal(desc.Name);
			Marshal.FreeHGlobal(desc.Description);
		}
		return list;
	}

	public static List<AttributeDesc> ListEffectAttributesFromFx(string name)
	{
		List<AttributeDesc> list = new List<AttributeDesc>();
		int num = EffectAttributesCountFromFx(name);
		for (int i = 0; i < num; i++)
		{
			S_AttributeDesc desc = default(S_AttributeDesc);
			desc.Name = Marshal.AllocHGlobal(64);
			desc.Description = Marshal.AllocHGlobal(128);
			if (EffectFillAttributeDescFromFx(name, i, ref desc))
			{
				list.Add(new AttributeDesc(desc));
			}
			Marshal.FreeHGlobal(desc.Name);
			Marshal.FreeHGlobal(desc.Description);
		}
		while (true)
		{
			return list;
		}
	}

	public static List<SamplerDesc> ListEffectSamplersFromFx(string name)
	{
		List<SamplerDesc> list = new List<SamplerDesc>();
		int num = EffectSamplersCountFromFx(name);
		for (int i = 0; i < num; i++)
		{
			S_SamplerDesc desc = default(S_SamplerDesc);
			desc.Name = Marshal.AllocHGlobal(64);
			desc.Description = Marshal.AllocHGlobal(128);
			if (EffectFillSamplerDescFromFx(name, i, ref desc))
			{
				list.Add(new SamplerDesc(desc));
			}
			Marshal.FreeHGlobal(desc.Name);
			Marshal.FreeHGlobal(desc.Description);
		}
		while (true)
		{
			return list;
		}
	}

	public static List<ShaderConstantDesc> ListShaderConstantsFromName(string name, int count)
	{
		List<ShaderConstantDesc> list = new List<ShaderConstantDesc>();
		for (int i = 0; i < count; i++)
		{
			S_ShaderConstantDesc desc = default(S_ShaderConstantDesc);
			if (ShaderFillConstantDesc(name, i, ref desc))
			{
				list.Add(new ShaderConstantDesc(desc));
			}
		}
		while (true)
		{
			return list;
		}
	}

	public static int CreateEffect(string path, Transform t)
	{
		return CreateEffect(path, t.localToWorldMatrix);
	}

	public static int CreateEffect(string path, Vector3 position, Quaternion rotation, Vector3 scale)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.SetTRS(position, rotation, scale);
		return CreateEffect(path, identity);
	}

	public static int CreateEffect(string path, Matrix4x4 m)
	{
		FxDesc path2 = default(FxDesc);
		path2.Transforms = m;
		path2.FxPath = path;
		return LoadFx(path2);
	}

	public static bool UpdateTransformEffect(int FxGUID, Transform t)
	{
		Matrix4x4 localToWorldMatrix = t.localToWorldMatrix;
		return EffectSetTransforms(FxGUID, localToWorldMatrix);
	}

	private static void EnableFileLoggingIFN(bool enable)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				try
				{
					m_HasFileLogging = enable;
					if (enable)
					{
						if (!File.Exists(m_LogFilePath))
						{
							FileStream fileStream = File.Create(m_LogFilePath);
							fileStream.Close();
						}
					}
					if (!enable)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								if (File.Exists(m_LogFilePath))
								{
									File.Delete(m_LogFilePath);
								}
								return;
							}
						}
					}
				}
				catch
				{
					Debug.LogError("[PKFX] Setting up file logging failed.");
				}
				return;
			}
		}
		m_HasFileLogging = false;
	}

	public static bool FileLoggingEnabled()
	{
		return m_HasFileLogging;
	}

	private static void SetupPackInPersistantDataPathIFN(bool enable)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (enable)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					List<string> list = new List<string>();
					list.AddRange(Directory.GetFiles(Application.streamingAssetsPath + "/PackFx", "*", SearchOption.AllDirectories));
					for (int i = 0; i < list.Count; i++)
					{
						list[i] = list[i].Replace("\\", "/");
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
						{
							list.Sort();
							using (List<string>.Enumerator enumerator = list.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									string current = enumerator.Current;
									if (Path.GetExtension(current) != ".meta")
									{
										string text = current.Substring(Application.streamingAssetsPath.Length);
										FileInfo fileInfo = new FileInfo(current);
										FileInfo fileInfo2 = new FileInfo(Application.persistentDataPath + text);
										if (!fileInfo2.Exists)
										{
											Debug.Log("Copy " + Application.persistentDataPath + text);
											if (!Directory.Exists(fileInfo2.Directory.FullName))
											{
												Directory.CreateDirectory(fileInfo2.Directory.FullName);
											}
											File.Copy(current, fileInfo2.FullName);
										}
										else if (fileInfo.LastWriteTime > fileInfo2.LastWriteTime)
										{
											Debug.Log("Overwriting " + Application.persistentDataPath + text);
											File.Copy(current, fileInfo2.FullName, true);
										}
									}
								}
							}
							m_PackPath = Application.persistentDataPath;
							return;
						}
						}
					}
				}
				}
			}
		}
		m_PackPath = Application.streamingAssetsPath;
	}

	public static bool PackInPersistantDataPathEnabled()
	{
		return m_PackPath == Application.persistentDataPath;
	}

	public static bool IsUsingOrthographicProjection()
	{
		return m_IsUsingOrthographicProjection;
	}

	private static void EnableKillIndividualEffect(bool enable)
	{
		m_HasSpawnerIDs = enable;
		EnableSpawnerIDs(enable);
	}

	public static bool KillIndividualEffectEnabled()
	{
		return m_HasSpawnerIDs;
	}

	public static string GetDefaultShader(int api, int type)
	{
		return Marshal.PtrToStringAnsi(GetDefaultShaderString(api, type));
	}
}
