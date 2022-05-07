// ReSharper disable ParameterHidesMember

namespace BadgerMvvm.Core;

public class CommandBuilder : ICommandBuilder
{
  private readonly Func<Task> execute;
  private readonly Func<Exception, Task> exceptionHandler;
  
  private CommandBuilder(Func<Task> execute, Func<Exception, Task> exceptionHandler)
  {
    this.execute = execute;
    this.exceptionHandler = exceptionHandler;
  }

  public static ICommandBuilder<T> WithExecute<T>(Func<T, Task> execute)
  {
    return new CommandBuilderParam<T>(execute, _ => true, (_, _) => Task.CompletedTask);
  }
  
  public static ICommandBuilder WithExecute(Func<Task> execute)
  {
    return new CommandBuilder(execute, _ => Task.CompletedTask);
  }
  
  public ICommandBuilder WithExceptionHandler(Func<Exception, Task> exceptionHandler)
  {
    return new CommandBuilder(execute, exceptionHandler);
  }

  public IAsyncCommand Build()
  {
    return new DelegateCommand(execute, exceptionHandler);
  }

  private class CommandBuilderParam<T> : ICommandBuilder<T>
  {
    private readonly Func<T, Task> execute;
    private readonly Func<T, bool> canExecute;
    private readonly Func<T, Exception, Task> exceptionHandler;

    internal CommandBuilderParam(Func<T, Task> execute, Func<T, bool> canExecute, Func<T, Exception, Task> exceptionHandler)
    {
      this.execute = execute;
      this.canExecute = canExecute;
      this.exceptionHandler = exceptionHandler;
    }

    public IAsyncCommand<T> Build()
    {
      return new DelegateCommand<T>(execute, canExecute, exceptionHandler);
    }
    
    public ICommandBuilder<T> WithCanExecute(Func<T, bool> canExecute)
    {
      return new CommandBuilderParam<T>(execute, canExecute, exceptionHandler);
    }
    
    public ICommandBuilder<T> WithExceptionHandler(Func<T, Exception, Task> exceptionHandler)
    {
      return new CommandBuilderParam<T>(execute, canExecute, exceptionHandler);
    }
  }
}
