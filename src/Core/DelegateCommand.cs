namespace BadgerMvvm.Core;

public class DelegateCommand : CommandBase
{
  private readonly Func<Task> execute;
  private readonly Func<Exception, Task> handleException;

  public DelegateCommand(Func<Task> execute, Func<Exception, Task> handleException)
  {
    this.execute = execute;
    this.handleException = handleException;
  }
  
  public DelegateCommand(Func<Task> execute)
    : this(execute, _ => Task.CompletedTask)
  {
  }
  
  public override async Task Execute()
  {
    await execute();
  }

  protected override async Task HandleException(Exception exception)
  {
    await handleException(exception);
  }
}

public class DelegateCommand<T> : CommandBase<T>
{
  private readonly Func<T, Task> execute;
  private readonly Func<T, bool> canExecute;
  private readonly Func<T, Exception, Task> handleException;

  public DelegateCommand(Func<T, Task> execute, Func<T, bool> canExecute, Func<T, Exception, Task> handleException)
  {
    this.execute = execute;
    this.canExecute = canExecute;
    this.handleException = handleException;
  }
  
  public DelegateCommand(Func<T, Task> execute, Func<T, bool> canExecute)
    : this(execute, canExecute, (_, _) => Task.CompletedTask)
  {
  }
  
  public DelegateCommand(Func<T, Task> execute, Func<T, Exception, Task> handleException)
    : this(execute, _ => true, handleException)
  {
  }
  
  public DelegateCommand(Func<T, Task> execute)
    : this(execute, _ => true)
  {
  }
  
  public override async Task Execute(T parameter)
  {
    await execute(parameter);
  }

  public override bool CanExecute(T parameter)
  {
    return canExecute(parameter);
  }

  protected override async Task HandleException(T parameter, Exception exception)
  {
    await handleException(parameter, exception);
  }
}
