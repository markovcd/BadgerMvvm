namespace BadgerMvvm.Core;

public static class BindableExtensions
{
  public static IDisposable AddValidationRule<T>(this IBindable<T> b, Func<IBindable<T>, ValidationResult> rule)
  {
    return b.AddValidationRule(new DelegateValidationRule<T>(rule));
  }
  
  private sealed class DelegateValidationRule<T> : IValidationRule<T>
  {
    private readonly Func<IBindable<T>, ValidationResult> func;

    public DelegateValidationRule(Func<IBindable<T>, ValidationResult> func)
    {
      this.func = func;
    }
      
    public ValidationResult Validate(IBindable<T> property)
    {
      return func(property);
    }
  }
}