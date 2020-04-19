using System;
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
		this.m_DepthGrabMat = new Material(this.m_DepthGrabShader);
		this.m_DepthWriteMat = new Material(this.m_DepthWriteShader);
		this.m_CmdDepthGrab = new CommandBuffer();
		this.m_CmdDepthWrite = new CommandBuffer();
	}

	private void Update()
	{
		this.m_Camera1.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, this.m_CmdDepthGrab);
		this.m_Camera2.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, this.m_CmdDepthWrite);
		if (this.m_CamDepth == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CopyMainCameraDepth.Update()).MethodHandle;
			}
			this.m_CamDepth = new RenderTexture(this.m_Camera1.pixelWidth, this.m_Camera1.pixelHeight, 0x20, RenderTextureFormat.Depth);
		}
		if (!this.m_CamDepth.IsCreated())
		{
			this.m_CamDepth.Create();
		}
		this.m_CmdDepthGrab.Clear();
		this.m_CmdDepthGrab.name = "Grab depth";
		this.m_CmdDepthGrab.Blit(this.m_CamDepth, this.m_CamDepth, this.m_DepthGrabMat);
		this.m_Camera1.AddCommandBuffer(CameraEvent.AfterForwardOpaque, this.m_CmdDepthGrab);
		this.m_CmdDepthWrite.Clear();
		this.m_CmdDepthWrite.name = "Write depth";
		this.m_CmdDepthWrite.Blit(this.m_CamDepth, BuiltinRenderTextureType.CurrentActive, this.m_DepthWriteMat);
		this.m_Camera2.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, this.m_CmdDepthWrite);
	}
}
