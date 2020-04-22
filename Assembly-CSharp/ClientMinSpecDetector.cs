using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;

public class ClientMinSpecDetector : MonoBehaviour
{
	[Tooltip("Rough detection of CPU generation, for lack of a better CPU metric within SystemInfo. Includes processors, cores, Hyper Threading")]
	public byte m_minProcessors = 4;

	[Tooltip("TODO Test 3, 2, 1 GB")]
	public short m_min_RAM_MB = 3072;

	[Tooltip("In available not total MB. TODO test at 512MB to get 84% of PC Unity player install base for 2015.")]
	public short m_min_GPU_MB = 512;

	[Tooltip("Use 30 for 3.0, which most of our shaders need, and the vast majority of Unity PC users have")]
	public byte m_minGPUShaderVersion = 30;

	[Tooltip("Required for Fog of War, and lots of other image effects")]
	public bool m_requireImageEffects = true;

	public bool m_requireDepthTextures;

	[Tooltip("6.1.0 = Win 7, 6.0.0 = Win Vista, etc.")]
	public string m_minWindowsVersion = "6.0.6002";

	private const string c_windowsPrefixLower = "windows";

	internal static bool BelowMinSpecDetected
	{
		get;
		private set;
	}

	private void Awake()
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder("Below Min Spec: ");
			int length = stringBuilder.Length;
			if (SystemInfo.processorCount < m_minProcessors)
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
				stringBuilder.Append("CPUs: ");
				stringBuilder.Append(SystemInfo.processorCount);
				stringBuilder.Append("/");
				stringBuilder.Append(m_minProcessors);
				stringBuilder.Append(", ");
			}
			if (SystemInfo.systemMemorySize < m_min_RAM_MB)
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
				stringBuilder.Append("RAM (MB): ");
				stringBuilder.Append(SystemInfo.systemMemorySize);
				stringBuilder.Append("/");
				stringBuilder.Append(m_min_RAM_MB);
				stringBuilder.Append(", ");
			}
			if (SystemInfo.graphicsMemorySize < m_min_GPU_MB)
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
				stringBuilder.Append("VRAM (MB): ");
				stringBuilder.Append(SystemInfo.graphicsMemorySize);
				stringBuilder.Append("/");
				stringBuilder.Append(m_min_GPU_MB);
				stringBuilder.Append(", ");
			}
			if (SystemInfo.graphicsShaderLevel < m_minGPUShaderVersion)
			{
				stringBuilder.Append("Shader Lvl: ");
				stringBuilder.Append(SystemInfo.graphicsShaderLevel);
				stringBuilder.Append("/");
				stringBuilder.Append(m_minGPUShaderVersion);
				stringBuilder.Append(", ");
			}
			if (m_requireImageEffects)
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
				if (!SystemInfo.supportsImageEffects)
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
					stringBuilder.Append("Image effects unsupported, ");
				}
			}
			if (m_requireDepthTextures)
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
				if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
				{
					stringBuilder.Append("Depth textures unsupported, ");
				}
			}
			bool flag;
			if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D9)
			{
				stringBuilder.Append("Old D3D ");
				stringBuilder.Append(SystemInfo.graphicsDeviceVersion);
				stringBuilder.Append(", type: ");
				stringBuilder.Append(SystemInfo.graphicsDeviceType.ToString());
				stringBuilder.Append("/D3D11 or 12");
				stringBuilder.Append(", ");
				flag = false;
			}
			string operatingSystem = SystemInfo.operatingSystem;
			flag = !operatingSystem.ToLower().StartsWith("windows");
			if (!flag)
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
				try
				{
					Regex regex = new Regex("[0-9]+\\.[0-9]+(\\.[0-9]+)*(\\.[0-9]+)*");
					Match match = regex.Match(operatingSystem);
					Version version = new Version(match.Value);
					version = new Version(version.Major, version.Minor, Mathf.Max(0, version.Build), Mathf.Max(0, version.Revision));
					Version version2 = new Version(m_minWindowsVersion);
					version2 = new Version(version2.Major, version2.Minor, Mathf.Max(0, version2.Build), Mathf.Max(0, version2.Revision));
					flag = (version < version2);
				}
				catch
				{
					flag = true;
				}
			}
			if (flag)
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
				stringBuilder.Append("Old OS ");
				stringBuilder.Append(operatingSystem);
				stringBuilder.Append("/");
				stringBuilder.Append("windows");
				stringBuilder.Append(" ");
				stringBuilder.Append(m_minWindowsVersion);
				flag = false;
			}
			if (stringBuilder.Length > length)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						Log.Error(stringBuilder.ToString());
						BelowMinSpecDetected = true;
						return;
					}
				}
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}
}
