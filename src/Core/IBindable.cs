using System.ComponentModel;

namespace BadgerMvvm.Core;

public interface IBindable : INotifyPropertyChanged, INotifyDataErrorInfo
{
  string Name { get; }
  
  IReadOnlyList<string> Errors { get; }
  
  void AddError(string message);
  
  void ClearErrors();

  void Validate();
}

public interface IBindable<T> : IBindable, IEquatable<T>, IEquatable<IBindable<T>>
{
  T? Value { get; set; }

  IDisposable ListenForChange(Action<IBindable<T>> callback);
  
  IDisposable ListenForErrors(Action<IBindable<T>> callback);

  IDisposable AddValidationRule(Func<IBindable<T>, ValidationResult> rule);
  
  IDisposable AddValidationRule(IValidationRule<T> rule);
}
