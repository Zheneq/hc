using System;
using System.Globalization;
using System.Reflection;

public class UnknownMethod : MethodBase
{
	public override Type DeclaringType
	{
		get
		{
			return null;
		}
	}

	public override MemberTypes MemberType
	{
		get
		{
			return (MemberTypes)0;
		}
	}

	public override string Name
	{
		get
		{
			return null;
		}
	}

	public override Type ReflectedType
	{
		get
		{
			return null;
		}
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		return null;
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		return null;
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		return false;
	}

	public override MethodAttributes Attributes
	{
		get
		{
			return MethodAttributes.PrivateScope;
		}
	}

	public override RuntimeMethodHandle MethodHandle
	{
		get
		{
			return default(RuntimeMethodHandle);
		}
	}

	public override ParameterInfo[] GetParameters()
	{
		return null;
	}

	public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
	{
		return null;
	}

	public override MethodImplAttributes GetMethodImplementationFlags()
	{
		return MethodImplAttributes.IL;
	}
}
