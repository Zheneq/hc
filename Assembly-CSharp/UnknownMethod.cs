using System;
using System.Globalization;
using System.Reflection;

public class UnknownMethod : MethodBase
{
	public override Type DeclaringType => null;

	public override MemberTypes MemberType => (MemberTypes)0;

	public override string Name => null;

	public override Type ReflectedType => null;

	public override MethodAttributes Attributes => MethodAttributes.PrivateScope;

	public override RuntimeMethodHandle MethodHandle => default(RuntimeMethodHandle);

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
