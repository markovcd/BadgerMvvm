namespace BadgerMvvm.Core;

public interface IValidationRule<T>
{
  ValidationResult Validate(IBindable<T> property);
}
