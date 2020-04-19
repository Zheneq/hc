using System;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	public class JSONData : JSONNode
	{
		private string \u0012;

		public JSONData(string \u001D)
		{
			this.\u0012 = \u001D;
		}

		public JSONData(float \u001D)
		{
			this.\u0003 = \u001D;
		}

		public JSONData(double \u001D)
		{
			this.\u000F = \u001D;
		}

		public JSONData(bool \u001D)
		{
			this.\u0017 = \u001D;
		}

		public JSONData(int \u001D)
		{
			this.\u000B = \u001D;
		}

		public override string \u0009
		{
			get
			{
				return this.\u0012;
			}
			set
			{
				this.\u0012 = value;
			}
		}

		public override string ToString()
		{
			return "\"" + JSONNode.\u000A(this.\u0012) + "\"";
		}

		public override string \u0004(string \u001D)
		{
			return "\"" + JSONNode.\u000A(this.\u0012) + "\"";
		}

		public override void \u0002(BinaryWriter \u001D)
		{
			JSONData jsondata = new JSONData(string.Empty);
			jsondata.\u000B = this.\u000B;
			if (jsondata.\u0012 == this.\u0012)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(JSONData.\u0002(BinaryWriter)).MethodHandle;
				}
				\u001D.Write(4);
				\u001D.Write(this.\u000B);
				return;
			}
			jsondata.\u0003 = this.\u0003;
			if (jsondata.\u0012 == this.\u0012)
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
				\u001D.Write(7);
				\u001D.Write(this.\u0003);
				return;
			}
			jsondata.\u000F = this.\u000F;
			if (jsondata.\u0012 == this.\u0012)
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
				\u001D.Write(5);
				\u001D.Write(this.\u000F);
				return;
			}
			jsondata.\u0017 = this.\u0017;
			if (jsondata.\u0012 == this.\u0012)
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
				\u001D.Write(6);
				\u001D.Write(this.\u0017);
				return;
			}
			\u001D.Write(3);
			\u001D.Write(this.\u0012);
		}
	}
}
