namespace BadgerMvvm.Core;

public interface ICommandBuilder
{
  ICommandBuilder WithExceptionHandler(Func<Exception, Task> exceptionHandler);
  
  ICommandBuilder WithExceptionHandler(Action<Exception> exceptionHandler);
  
  IAsyncCommand Build();
}

public interface ICommandBuilder<T>
{
  ICommandBuilder<T> WithCanExecute(Func<T, bool> canExecute);

  ICommandBuilder<T> WithExceptionHandler(Func<T, Exception, Task> exceptionHandler);
  
  ICommandBuilder<T> WithExceptionHandler(Action<T, Exception> exceptionHandler);
  
  IAsyncCommand<T> Build();
}
