using System;

[Serializable]
public class AbilityModPropertyBool
{
    public enum ModOp
    {
        Ignore,
        Override
    }

    public bool value;
    public ModOp operation;

    public bool GetModifiedValue(bool input)
    {
        return operation == ModOp.Override ? value : input;
    }

    public void CopyValuesFrom(AbilityModPropertyBool other)
    {
        value = other.value;
        operation = other.operation;
    }
}
