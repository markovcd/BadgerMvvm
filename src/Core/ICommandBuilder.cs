namespace BadgerMvvm.Core;

public interface ICommandBuilder
{
  ICommandBuilder WithExceptionHandler(Func<Exception, Task> exceptionHandler);
  
  IAsyncCommand Build();
}

public interface ICommandBuilder<T>
{
  ICommandBuilder<T> WithCanExecute(Func<T, bool> canExecute);
  
  ICommandBuilder<T> WithExceptionHandler(Func<T, Exception, Task> exceptionHandler);
  
  IAsyncCommand<T> Build();
}
