using System.Text;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class PostEffectsCSBase : MonoBehaviour
{
	protected bool supportHDRTextures = true;

	protected bool supportDX11;

	protected bool isSupported = true;

	public Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
	{
		if (!s)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Log.Info(new StringBuilder().Append("Missing shader in ").Append(ToString()).ToString());
					base.enabled = false;
					return null;
				}
			}
		}
		if (s.isSupported && (bool)m2Create)
		{
			if (m2Create.shader == s)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return m2Create;
					}
				}
			}
		}
		if (!s.isSupported)
		{
			NotSupported();
			Log.Info(new StringBuilder().Append("The shader ").Append(s.ToString()).Append(" on effect ").Append(ToString()).Append(" is not supported on this platform!").ToString());
			return null;
		}
		m2Create = new Material(s);
		m2Create.hideFlags = HideFlags.DontSave;
		if ((bool)m2Create)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m2Create;
				}
			}
		}
		return null;
	}

	private Material CreateMaterial(Shader s, Material m2Create)
	{
		if (!s)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Log.Info(new StringBuilder().Append("Missing shader in ").Append(ToString()).ToString());
					return null;
				}
			}
		}
		if ((bool)m2Create && m2Create.shader == s)
		{
			if (s.isSupported)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return m2Create;
					}
				}
			}
		}
		if (!s.isSupported)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		m2Create = new Material(s);
		m2Create.hideFlags = HideFlags.DontSave;
		if ((bool)m2Create)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m2Create;
				}
			}
		}
		return null;
	}

	private void OnEnable()
	{
		isSupported = true;
	}

	public bool CheckSupport()
	{
		return CheckSupport(false);
	}

	public virtual bool CheckResources()
	{
		Log.Warning(new StringBuilder().Append("CheckResources () for ").Append(ToString()).Append(" should be overwritten.").ToString());
		return isSupported;
	}

	private void Start()
	{
		CheckResources();
	}

	public bool CheckSupport(bool needDepth)
	{
		isSupported = true;
		supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
		int num;
		if (SystemInfo.graphicsShaderLevel >= 50)
		{
			num = (SystemInfo.supportsComputeShaders ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		supportDX11 = ((byte)num != 0);
		if (!SystemInfo.supportsImageEffects)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					NotSupported();
					return false;
				}
			}
		}
		if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					NotSupported();
					return false;
				}
			}
		}
		if (needDepth)
		{
			GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
		}
		return true;
	}

	private bool CheckSupport(bool needDepth, bool needHdr)
	{
		if (!CheckSupport(needDepth))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (needHdr)
		{
			if (!supportHDRTextures)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						NotSupported();
						return false;
					}
				}
			}
		}
		return true;
	}

	private bool Dx11Support()
	{
		return supportDX11;
	}

	public void ReportAutoDisable()
	{
		Log.Warning(new StringBuilder().Append("The image effect ").Append(ToString()).Append(" has been disabled as it's not supported on the current platform.").ToString());
	}

	private bool CheckShader(Shader s)
	{
		Log.Info(new StringBuilder().Append("The shader ").Append(s.ToString()).Append(" on effect ").Append(ToString()).Append(" is not part of the Unity 3.2f+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package.").ToString());
		if (!s.isSupported)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					NotSupported();
					return false;
				}
			}
		}
		return false;
	}

	private void NotSupported()
	{
		base.enabled = false;
		isSupported = false;
	}

	private void DrawBorder(RenderTexture dest, Material material)
	{
		RenderTexture.active = dest;
		bool flag = true;
		GL.PushMatrix();
		GL.LoadOrtho();
		for (int i = 0; i < material.passCount; i++)
		{
			material.SetPass(i);
			float y;
			float y2;
			if (flag)
			{
				y = 1f;
				y2 = 0f;
			}
			else
			{
				y = 0f;
				y2 = 1f;
			}
			float x = 0f;
			float x2 = 1f / ((float)dest.width * 1f);
			float y3 = 0f;
			float y4 = 1f;
			GL.Begin(7);
			GL.TexCoord2(0f, y);
			GL.Vertex3(x, y3, 0.1f);
			GL.TexCoord2(1f, y);
			GL.Vertex3(x2, y3, 0.1f);
			GL.TexCoord2(1f, y2);
			GL.Vertex3(x2, y4, 0.1f);
			GL.TexCoord2(0f, y2);
			GL.Vertex3(x, y4, 0.1f);
			x = 1f - 1f / ((float)dest.width * 1f);
			x2 = 1f;
			y3 = 0f;
			y4 = 1f;
			GL.TexCoord2(0f, y);
			GL.Vertex3(x, y3, 0.1f);
			GL.TexCoord2(1f, y);
			GL.Vertex3(x2, y3, 0.1f);
			GL.TexCoord2(1f, y2);
			GL.Vertex3(x2, y4, 0.1f);
			GL.TexCoord2(0f, y2);
			GL.Vertex3(x, y4, 0.1f);
			x = 0f;
			x2 = 1f;
			y3 = 0f;
			y4 = 1f / ((float)dest.height * 1f);
			GL.TexCoord2(0f, y);
			GL.Vertex3(x, y3, 0.1f);
			GL.TexCoord2(1f, y);
			GL.Vertex3(x2, y3, 0.1f);
			GL.TexCoord2(1f, y2);
			GL.Vertex3(x2, y4, 0.1f);
			GL.TexCoord2(0f, y2);
			GL.Vertex3(x, y4, 0.1f);
			x = 0f;
			x2 = 1f;
			y3 = 1f - 1f / ((float)dest.height * 1f);
			y4 = 1f;
			GL.TexCoord2(0f, y);
			GL.Vertex3(x, y3, 0.1f);
			GL.TexCoord2(1f, y);
			GL.Vertex3(x2, y3, 0.1f);
			GL.TexCoord2(1f, y2);
			GL.Vertex3(x2, y4, 0.1f);
			GL.TexCoord2(0f, y2);
			GL.Vertex3(x, y4, 0.1f);
			GL.End();
		}
		GL.PopMatrix();
	}
}
