using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	public struct TMP_MeshInfo
	{
		private static readonly Color32 s_DefaultColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private static readonly Vector3 s_DefaultNormal = new Vector3(0f, 0f, -1f);

		private static readonly Vector4 s_DefaultTangent = new Vector4(-1f, 0f, 0f, 1f);

		public Mesh mesh;

		public int vertexCount;

		public Vector3[] vertices;

		public Vector3[] normals;

		public Vector4[] tangents;

		public Vector2[] uvs0;

		public Vector2[] uvs2;

		public Color32[] colors32;

		public int[] triangles;

		public TMP_MeshInfo(Mesh mesh, int size)
		{
			if (mesh == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_MeshInfo..ctor(Mesh, int)).MethodHandle;
				}
				mesh = new Mesh();
			}
			else
			{
				mesh.Clear();
			}
			this.mesh = mesh;
			size = Mathf.Min(size, 0x3FFF);
			int num = size * 4;
			int num2 = size * 6;
			this.vertexCount = 0;
			this.vertices = new Vector3[num];
			this.uvs0 = new Vector2[num];
			this.uvs2 = new Vector2[num];
			this.colors32 = new Color32[num];
			this.normals = new Vector3[num];
			this.tangents = new Vector4[num];
			this.triangles = new int[num2];
			int num3 = 0;
			int num4 = 0;
			while (num4 / 4 < size)
			{
				for (int i = 0; i < 4; i++)
				{
					this.vertices[num4 + i] = Vector3.zero;
					this.uvs0[num4 + i] = Vector2.zero;
					this.uvs2[num4 + i] = Vector2.zero;
					this.colors32[num4 + i] = TMP_MeshInfo.s_DefaultColor;
					this.normals[num4 + i] = TMP_MeshInfo.s_DefaultNormal;
					this.tangents[num4 + i] = TMP_MeshInfo.s_DefaultTangent;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				this.triangles[num3] = num4;
				this.triangles[num3 + 1] = num4 + 1;
				this.triangles[num3 + 2] = num4 + 2;
				this.triangles[num3 + 3] = num4 + 2;
				this.triangles[num3 + 4] = num4 + 3;
				this.triangles[num3 + 5] = num4;
				num4 += 4;
				num3 += 6;
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
			this.mesh.vertices = this.vertices;
			this.mesh.normals = this.normals;
			this.mesh.tangents = this.tangents;
			this.mesh.triangles = this.triangles;
			this.mesh.bounds = new Bounds(Vector3.zero, new Vector3(3840f, 2160f, 0f));
		}

		public TMP_MeshInfo(Mesh mesh, int size, bool isVolumetric)
		{
			if (mesh == null)
			{
				mesh = new Mesh();
			}
			else
			{
				mesh.Clear();
			}
			this.mesh = mesh;
			int num = isVolumetric ? 8 : 4;
			int num2;
			if (!isVolumetric)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_MeshInfo..ctor(Mesh, int, bool)).MethodHandle;
				}
				num2 = 6;
			}
			else
			{
				num2 = 0x24;
			}
			int num3 = num2;
			size = Mathf.Min(size, 0xFFFC / num);
			int num4 = size * num;
			int num5 = size * num3;
			this.vertexCount = 0;
			this.vertices = new Vector3[num4];
			this.uvs0 = new Vector2[num4];
			this.uvs2 = new Vector2[num4];
			this.colors32 = new Color32[num4];
			this.normals = new Vector3[num4];
			this.tangents = new Vector4[num4];
			this.triangles = new int[num5];
			int num6 = 0;
			int num7 = 0;
			while (num6 / num < size)
			{
				for (int i = 0; i < num; i++)
				{
					this.vertices[num6 + i] = Vector3.zero;
					this.uvs0[num6 + i] = Vector2.zero;
					this.uvs2[num6 + i] = Vector2.zero;
					this.colors32[num6 + i] = TMP_MeshInfo.s_DefaultColor;
					this.normals[num6 + i] = TMP_MeshInfo.s_DefaultNormal;
					this.tangents[num6 + i] = TMP_MeshInfo.s_DefaultTangent;
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				this.triangles[num7] = num6;
				this.triangles[num7 + 1] = num6 + 1;
				this.triangles[num7 + 2] = num6 + 2;
				this.triangles[num7 + 3] = num6 + 2;
				this.triangles[num7 + 4] = num6 + 3;
				this.triangles[num7 + 5] = num6;
				if (isVolumetric)
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
					this.triangles[num7 + 6] = num6 + 4;
					this.triangles[num7 + 7] = num6 + 5;
					this.triangles[num7 + 8] = num6 + 1;
					this.triangles[num7 + 9] = num6 + 1;
					this.triangles[num7 + 0xA] = num6;
					this.triangles[num7 + 0xB] = num6 + 4;
					this.triangles[num7 + 0xC] = num6 + 3;
					this.triangles[num7 + 0xD] = num6 + 2;
					this.triangles[num7 + 0xE] = num6 + 6;
					this.triangles[num7 + 0xF] = num6 + 6;
					this.triangles[num7 + 0x10] = num6 + 7;
					this.triangles[num7 + 0x11] = num6 + 3;
					this.triangles[num7 + 0x12] = num6 + 1;
					this.triangles[num7 + 0x13] = num6 + 5;
					this.triangles[num7 + 0x14] = num6 + 6;
					this.triangles[num7 + 0x15] = num6 + 6;
					this.triangles[num7 + 0x16] = num6 + 2;
					this.triangles[num7 + 0x17] = num6 + 1;
					this.triangles[num7 + 0x18] = num6 + 4;
					this.triangles[num7 + 0x19] = num6;
					this.triangles[num7 + 0x1A] = num6 + 3;
					this.triangles[num7 + 0x1B] = num6 + 3;
					this.triangles[num7 + 0x1C] = num6 + 7;
					this.triangles[num7 + 0x1D] = num6 + 4;
					this.triangles[num7 + 0x1E] = num6 + 7;
					this.triangles[num7 + 0x1F] = num6 + 6;
					this.triangles[num7 + 0x20] = num6 + 5;
					this.triangles[num7 + 0x21] = num6 + 5;
					this.triangles[num7 + 0x22] = num6 + 4;
					this.triangles[num7 + 0x23] = num6 + 7;
				}
				num6 += num;
				num7 += num3;
			}
			this.mesh.vertices = this.vertices;
			this.mesh.normals = this.normals;
			this.mesh.tangents = this.tangents;
			this.mesh.triangles = this.triangles;
			this.mesh.bounds = new Bounds(Vector3.zero, new Vector3(3840f, 2160f, 64f));
		}

		public void ResizeMeshInfo(int size)
		{
			size = Mathf.Min(size, 0x3FFF);
			int newSize = size * 4;
			int newSize2 = size * 6;
			int num = this.vertices.Length / 4;
			Array.Resize<Vector3>(ref this.vertices, newSize);
			Array.Resize<Vector3>(ref this.normals, newSize);
			Array.Resize<Vector4>(ref this.tangents, newSize);
			Array.Resize<Vector2>(ref this.uvs0, newSize);
			Array.Resize<Vector2>(ref this.uvs2, newSize);
			Array.Resize<Color32>(ref this.colors32, newSize);
			Array.Resize<int>(ref this.triangles, newSize2);
			if (size <= num)
			{
				this.mesh.triangles = this.triangles;
				this.mesh.vertices = this.vertices;
				this.mesh.normals = this.normals;
				this.mesh.tangents = this.tangents;
				return;
			}
			for (int i = num; i < size; i++)
			{
				int num2 = i * 4;
				int num3 = i * 6;
				this.normals[num2] = TMP_MeshInfo.s_DefaultNormal;
				this.normals[1 + num2] = TMP_MeshInfo.s_DefaultNormal;
				this.normals[2 + num2] = TMP_MeshInfo.s_DefaultNormal;
				this.normals[3 + num2] = TMP_MeshInfo.s_DefaultNormal;
				this.tangents[num2] = TMP_MeshInfo.s_DefaultTangent;
				this.tangents[1 + num2] = TMP_MeshInfo.s_DefaultTangent;
				this.tangents[2 + num2] = TMP_MeshInfo.s_DefaultTangent;
				this.tangents[3 + num2] = TMP_MeshInfo.s_DefaultTangent;
				this.triangles[num3] = num2;
				this.triangles[1 + num3] = 1 + num2;
				this.triangles[2 + num3] = 2 + num2;
				this.triangles[3 + num3] = 2 + num2;
				this.triangles[4 + num3] = 3 + num2;
				this.triangles[5 + num3] = num2;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_MeshInfo.ResizeMeshInfo(int)).MethodHandle;
			}
			this.mesh.vertices = this.vertices;
			this.mesh.normals = this.normals;
			this.mesh.tangents = this.tangents;
			this.mesh.triangles = this.triangles;
		}

		public void ResizeMeshInfo(int size, bool isVolumetric)
		{
			int num;
			if (!isVolumetric)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_MeshInfo.ResizeMeshInfo(int, bool)).MethodHandle;
				}
				num = 4;
			}
			else
			{
				num = 8;
			}
			int num2 = num;
			int num3;
			if (!isVolumetric)
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
				num3 = 6;
			}
			else
			{
				num3 = 0x24;
			}
			int num4 = num3;
			size = Mathf.Min(size, 0xFFFC / num2);
			int newSize = size * num2;
			int newSize2 = size * num4;
			int num5 = this.vertices.Length / num2;
			Array.Resize<Vector3>(ref this.vertices, newSize);
			Array.Resize<Vector3>(ref this.normals, newSize);
			Array.Resize<Vector4>(ref this.tangents, newSize);
			Array.Resize<Vector2>(ref this.uvs0, newSize);
			Array.Resize<Vector2>(ref this.uvs2, newSize);
			Array.Resize<Color32>(ref this.colors32, newSize);
			Array.Resize<int>(ref this.triangles, newSize2);
			if (size <= num5)
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
				this.mesh.triangles = this.triangles;
				this.mesh.vertices = this.vertices;
				this.mesh.normals = this.normals;
				this.mesh.tangents = this.tangents;
				return;
			}
			for (int i = num5; i < size; i++)
			{
				int num6 = i * num2;
				int num7 = i * num4;
				this.normals[num6] = TMP_MeshInfo.s_DefaultNormal;
				this.normals[1 + num6] = TMP_MeshInfo.s_DefaultNormal;
				this.normals[2 + num6] = TMP_MeshInfo.s_DefaultNormal;
				this.normals[3 + num6] = TMP_MeshInfo.s_DefaultNormal;
				this.tangents[num6] = TMP_MeshInfo.s_DefaultTangent;
				this.tangents[1 + num6] = TMP_MeshInfo.s_DefaultTangent;
				this.tangents[2 + num6] = TMP_MeshInfo.s_DefaultTangent;
				this.tangents[3 + num6] = TMP_MeshInfo.s_DefaultTangent;
				if (isVolumetric)
				{
					this.normals[4 + num6] = TMP_MeshInfo.s_DefaultNormal;
					this.normals[5 + num6] = TMP_MeshInfo.s_DefaultNormal;
					this.normals[6 + num6] = TMP_MeshInfo.s_DefaultNormal;
					this.normals[7 + num6] = TMP_MeshInfo.s_DefaultNormal;
					this.tangents[4 + num6] = TMP_MeshInfo.s_DefaultTangent;
					this.tangents[5 + num6] = TMP_MeshInfo.s_DefaultTangent;
					this.tangents[6 + num6] = TMP_MeshInfo.s_DefaultTangent;
					this.tangents[7 + num6] = TMP_MeshInfo.s_DefaultTangent;
				}
				this.triangles[num7] = num6;
				this.triangles[1 + num7] = 1 + num6;
				this.triangles[2 + num7] = 2 + num6;
				this.triangles[3 + num7] = 2 + num6;
				this.triangles[4 + num7] = 3 + num6;
				this.triangles[5 + num7] = num6;
				if (isVolumetric)
				{
					this.triangles[num7 + 6] = num6 + 4;
					this.triangles[num7 + 7] = num6 + 5;
					this.triangles[num7 + 8] = num6 + 1;
					this.triangles[num7 + 9] = num6 + 1;
					this.triangles[num7 + 0xA] = num6;
					this.triangles[num7 + 0xB] = num6 + 4;
					this.triangles[num7 + 0xC] = num6 + 3;
					this.triangles[num7 + 0xD] = num6 + 2;
					this.triangles[num7 + 0xE] = num6 + 6;
					this.triangles[num7 + 0xF] = num6 + 6;
					this.triangles[num7 + 0x10] = num6 + 7;
					this.triangles[num7 + 0x11] = num6 + 3;
					this.triangles[num7 + 0x12] = num6 + 1;
					this.triangles[num7 + 0x13] = num6 + 5;
					this.triangles[num7 + 0x14] = num6 + 6;
					this.triangles[num7 + 0x15] = num6 + 6;
					this.triangles[num7 + 0x16] = num6 + 2;
					this.triangles[num7 + 0x17] = num6 + 1;
					this.triangles[num7 + 0x18] = num6 + 4;
					this.triangles[num7 + 0x19] = num6;
					this.triangles[num7 + 0x1A] = num6 + 3;
					this.triangles[num7 + 0x1B] = num6 + 3;
					this.triangles[num7 + 0x1C] = num6 + 7;
					this.triangles[num7 + 0x1D] = num6 + 4;
					this.triangles[num7 + 0x1E] = num6 + 7;
					this.triangles[num7 + 0x1F] = num6 + 6;
					this.triangles[num7 + 0x20] = num6 + 5;
					this.triangles[num7 + 0x21] = num6 + 5;
					this.triangles[num7 + 0x22] = num6 + 4;
					this.triangles[num7 + 0x23] = num6 + 7;
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			this.mesh.vertices = this.vertices;
			this.mesh.normals = this.normals;
			this.mesh.tangents = this.tangents;
			this.mesh.triangles = this.triangles;
		}

		public void Clear()
		{
			if (this.vertices == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_MeshInfo.Clear()).MethodHandle;
				}
				return;
			}
			Array.Clear(this.vertices, 0, this.vertices.Length);
			this.vertexCount = 0;
			if (this.mesh != null)
			{
				this.mesh.vertices = this.vertices;
			}
		}

		public void Clear(bool uploadChanges)
		{
			if (this.vertices == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_MeshInfo.Clear(bool)).MethodHandle;
				}
				return;
			}
			Array.Clear(this.vertices, 0, this.vertices.Length);
			this.vertexCount = 0;
			if (uploadChanges)
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
				if (this.mesh != null)
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
					this.mesh.vertices = this.vertices;
				}
			}
		}

		public void ClearUnusedVertices()
		{
			int num = this.vertices.Length - this.vertexCount;
			if (num > 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_MeshInfo.ClearUnusedVertices()).MethodHandle;
				}
				Array.Clear(this.vertices, this.vertexCount, num);
			}
		}

		public void ClearUnusedVertices(int startIndex)
		{
			int num = this.vertices.Length - startIndex;
			if (num > 0)
			{
				Array.Clear(this.vertices, startIndex, num);
			}
		}

		public void ClearUnusedVertices(int startIndex, bool updateMesh)
		{
			int num = this.vertices.Length - startIndex;
			if (num > 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_MeshInfo.ClearUnusedVertices(int, bool)).MethodHandle;
				}
				Array.Clear(this.vertices, startIndex, num);
			}
			if (updateMesh && this.mesh != null)
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
				this.mesh.vertices = this.vertices;
			}
		}

		public void SortGeometry(VertexSortingOrder order)
		{
			if (order != VertexSortingOrder.Normal)
			{
				if (order != VertexSortingOrder.Reverse)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_MeshInfo.SortGeometry(VertexSortingOrder)).MethodHandle;
					}
				}
				else
				{
					int num = this.vertexCount / 4;
					for (int i = 0; i < num; i++)
					{
						int num2 = i * 4;
						int num3 = (num - i - 1) * 4;
						if (num2 < num3)
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
							this.SwapVertexData(num2, num3);
						}
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}

		public void SortGeometry(IList<int> sortingOrder)
		{
			int count = sortingOrder.Count;
			if (count * 4 > this.vertices.Length)
			{
				return;
			}
			for (int i = 0; i < count; i++)
			{
				int j;
				for (j = sortingOrder[i]; j < i; j = sortingOrder[j])
				{
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_MeshInfo.SortGeometry(IList<int>)).MethodHandle;
				}
				if (j != i)
				{
					this.SwapVertexData(j * 4, i * 4);
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}

		public void SwapVertexData(int src, int dst)
		{
			Vector3 vector = this.vertices[dst];
			this.vertices[dst] = this.vertices[src];
			this.vertices[src] = vector;
			vector = this.vertices[dst + 1];
			this.vertices[dst + 1] = this.vertices[src + 1];
			this.vertices[src + 1] = vector;
			vector = this.vertices[dst + 2];
			this.vertices[dst + 2] = this.vertices[src + 2];
			this.vertices[src + 2] = vector;
			vector = this.vertices[dst + 3];
			this.vertices[dst + 3] = this.vertices[src + 3];
			this.vertices[src + 3] = vector;
			Vector2 vector2 = this.uvs0[dst];
			this.uvs0[dst] = this.uvs0[src];
			this.uvs0[src] = vector2;
			vector2 = this.uvs0[dst + 1];
			this.uvs0[dst + 1] = this.uvs0[src + 1];
			this.uvs0[src + 1] = vector2;
			vector2 = this.uvs0[dst + 2];
			this.uvs0[dst + 2] = this.uvs0[src + 2];
			this.uvs0[src + 2] = vector2;
			vector2 = this.uvs0[dst + 3];
			this.uvs0[dst + 3] = this.uvs0[src + 3];
			this.uvs0[src + 3] = vector2;
			vector2 = this.uvs2[dst];
			this.uvs2[dst] = this.uvs2[src];
			this.uvs2[src] = vector2;
			vector2 = this.uvs2[dst + 1];
			this.uvs2[dst + 1] = this.uvs2[src + 1];
			this.uvs2[src + 1] = vector2;
			vector2 = this.uvs2[dst + 2];
			this.uvs2[dst + 2] = this.uvs2[src + 2];
			this.uvs2[src + 2] = vector2;
			vector2 = this.uvs2[dst + 3];
			this.uvs2[dst + 3] = this.uvs2[src + 3];
			this.uvs2[src + 3] = vector2;
			Color32 color = this.colors32[dst];
			this.colors32[dst] = this.colors32[src];
			this.colors32[src] = color;
			color = this.colors32[dst + 1];
			this.colors32[dst + 1] = this.colors32[src + 1];
			this.colors32[src + 1] = color;
			color = this.colors32[dst + 2];
			this.colors32[dst + 2] = this.colors32[src + 2];
			this.colors32[src + 2] = color;
			color = this.colors32[dst + 3];
			this.colors32[dst + 3] = this.colors32[src + 3];
			this.colors32[src + 3] = color;
		}
	}
}
