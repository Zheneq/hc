using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SensitiveDataAttribute : Attribute
{
}
