using System;
using System.Reflection;

namespace I2.Loc.SimpleJSON
{
	[DefaultMember("Item")]
	internal class JSONLazyCreator : JSONNode
	{
		private JSONNode \u0015;

		private string \u0016;

		public JSONLazyCreator(JSONNode \u001D)
		{
			this.\u0015 = \u001D;
			this.\u0016 = null;
		}

		public JSONLazyCreator(JSONNode \u001D, string \u000E)
		{
			this.\u0015 = \u001D;
			this.\u0016 = \u000E;
		}

		private void \u000A(JSONNode \u001D)
		{
			if (this.\u0016 == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(JSONLazyCreator.\u000A(JSONNode)).MethodHandle;
				}
				this.\u0015.\u0013(\u001D);
			}
			else
			{
				this.\u0015.\u0013(this.\u0016, \u001D);
			}
			this.\u0015 = null;
		}

		public override JSONNode \u0018
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				JSONArray jsonarray = new JSONArray();
				jsonarray.\u0013(value);
				this.\u000A(jsonarray);
			}
		}

		public override JSONNode symbol_0018_2
		{
			get
			{
				return new JSONLazyCreator(this, \u001D);
			}
			set
			{
				JSONClass jsonclass = new JSONClass();
				jsonclass.\u0013(\u001D, value);
				this.\u000A(jsonclass);
			}
		}

		public override void \u0013(JSONNode \u001D)
		{
			JSONArray jsonarray = new JSONArray();
			jsonarray.\u0013(\u001D);
			this.\u000A(jsonarray);
		}

		public override void \u0013(string \u001D, JSONNode \u000E)
		{
			JSONClass jsonclass = new JSONClass();
			jsonclass.\u0013(\u001D, \u000E);
			this.\u000A(jsonclass);
		}

		public static bool \u000A(JSONLazyCreator \u001D, object \u000E)
		{
			if (\u000E == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(JSONLazyCreator.\u000A(JSONLazyCreator, object)).MethodHandle;
				}
				return true;
			}
			return object.ReferenceEquals(\u001D, \u000E);
		}

		public static bool \u0006(JSONLazyCreator \u001D, object \u000E)
		{
			return !JSONLazyCreator.\u000A(\u001D, \u000E);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(JSONLazyCreator.Equals(object)).MethodHandle;
				}
				return true;
			}
			return object.ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return string.Empty;
		}

		public override string \u0004(string \u001D)
		{
			return string.Empty;
		}

		public override int \u000B
		{
			get
			{
				JSONData u001D = new JSONData(0);
				this.\u000A(u001D);
				return 0;
			}
			set
			{
				JSONData u001D = new JSONData(value);
				this.\u000A(u001D);
			}
		}

		public override float \u0003
		{
			get
			{
				JSONData u001D = new JSONData(0f);
				this.\u000A(u001D);
				return 0f;
			}
			set
			{
				JSONData u001D = new JSONData(value);
				this.\u000A(u001D);
			}
		}

		public override double \u000F
		{
			get
			{
				JSONData u001D = new JSONData(0.0);
				this.\u000A(u001D);
				return 0.0;
			}
			set
			{
				JSONData u001D = new JSONData(value);
				this.\u000A(u001D);
			}
		}

		public override bool \u0017
		{
			get
			{
				JSONData u001D = new JSONData(false);
				this.\u000A(u001D);
				return false;
			}
			set
			{
				JSONData u001D = new JSONData(value);
				this.\u000A(u001D);
			}
		}

		public override JSONArray \u000D
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.\u000A(jsonarray);
				return jsonarray;
			}
		}

		public override JSONClass \u0008
		{
			get
			{
				JSONClass jsonclass = new JSONClass();
				this.\u000A(jsonclass);
				return jsonclass;
			}
		}
	}
}
