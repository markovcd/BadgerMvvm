namespace BadgerMvvm.Core
{
  public interface IAsyncCommand
  {
    bool IsBusy { get; }
    
    bool CanExecute { get; set; }
    
    Task Execute();
  }
  
  public interface IAsyncCommand<in T>
  {
    bool IsBusy { get; }

    bool CanExecute(T parameter);
    
    Task Execute(T parameter);
  }
}
