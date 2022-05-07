namespace BadgerMvvm.Core;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class CoupledWithAttribute : Attribute
{
  public CoupledWithAttribute(string propertyName)
  {
    PropertyName = propertyName;
  }

  internal string PropertyName { get; }
}
