using UnityEngine;
using UnityEngine.Rendering;

public class CopyMainCameraDepth : MonoBehaviour
{
	public Camera m_Camera1;

	public Camera m_Camera2;

	public Shader m_DepthGrabShader;

	public Shader m_DepthWriteShader;

	private RenderTexture m_CamDepth;

	private CommandBuffer m_CmdDepthWrite;

	private CommandBuffer m_CmdDepthGrab;

	private Material m_DepthWriteMat;

	private Material m_DepthGrabMat;

	private void Start()
	{
		m_DepthGrabMat = new Material(m_DepthGrabShader);
		m_DepthWriteMat = new Material(m_DepthWriteShader);
		m_CmdDepthGrab = new CommandBuffer();
		m_CmdDepthWrite = new CommandBuffer();
	}

	private void Update()
	{
		m_Camera1.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, m_CmdDepthGrab);
		m_Camera2.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, m_CmdDepthWrite);
		if (m_CamDepth == null)
		{
			m_CamDepth = new RenderTexture(m_Camera1.pixelWidth, m_Camera1.pixelHeight, 32, RenderTextureFormat.Depth);
		}
		if (!m_CamDepth.IsCreated())
		{
			m_CamDepth.Create();
		}
		m_CmdDepthGrab.Clear();
		m_CmdDepthGrab.name = "Grab depth";
		m_CmdDepthGrab.Blit(m_CamDepth, m_CamDepth, m_DepthGrabMat);
		m_Camera1.AddCommandBuffer(CameraEvent.AfterForwardOpaque, m_CmdDepthGrab);
		m_CmdDepthWrite.Clear();
		m_CmdDepthWrite.name = "Write depth";
		m_CmdDepthWrite.Blit(m_CamDepth, BuiltinRenderTextureType.CurrentActive, m_DepthWriteMat);
		m_Camera2.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, m_CmdDepthWrite);
	}
}
