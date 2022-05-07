using System.Windows.Input;

namespace BadgerMvvm.Core;

public abstract class CommandBase : BindableBaseSimple, ICommand, IAsyncCommand
{
  private readonly object syncRoot = new();
  private bool isBusy;
  private bool canExecute = true;

  public event EventHandler? CanExecuteChanged;
    
  public bool IsBusy
  {
    get => isBusy;
    private set
    {
      lock (syncRoot)
      {
        if (!SetProperty(ref isBusy, value))
          throw new InvalidOperationException();
      }
        
      RaiseCanExecuteChanged();
    }
  }

  public bool CanExecute
  {
    get => canExecute;
    set
    {
      if (SetProperty(ref canExecute, value)) 
        RaiseCanExecuteChanged();
    }
  }
  
  public async Task Execute()
  {
    if (IsBusy || !CanExecute) return;
    
    try
    {
      IsBusy = true;
      await ExecuteInternal();
    }
    catch (Exception e)
    {
      await HandleException(e);
    }
    finally
    {
      IsBusy = false;
    }
  }
  
  protected abstract Task ExecuteInternal();

  protected void RaiseCanExecuteChanged()
  {
    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
  }
  
  protected virtual Task HandleException(Exception exception)
  {
    return Task.CompletedTask;
  }
  
  void ICommand.Execute(object? parameter)
  {
    _ = Execute();
  }

  bool ICommand.CanExecute(object? parameter)
  {
    return !IsBusy && CanExecute;
  }
}

public abstract class CommandBase<T> : BindableBaseSimple, ICommand, IAsyncCommand<T>
{
  private readonly object syncRoot = new();
  private bool isBusy;

  public event EventHandler? CanExecuteChanged;
    
  public bool IsBusy
  {
    get => isBusy;
    private set
    {
      lock (syncRoot)
      {
        if (!SetProperty(ref isBusy, value))
          throw new InvalidOperationException();
      }
        
      RaiseCanExecuteChanged();
    }
  }
  
  public async Task Execute(T parameter)
  {
    if (IsBusy || !CanExecute(parameter)) return;
    
    try
    {
      IsBusy = true;
      await ExecuteInternal(parameter);
    }
    catch (Exception e)
    {
      await HandleException(parameter, e);
    }
    finally
    {
      IsBusy = false;
    }
  }
  
  public virtual bool CanExecute(T parameter)
  {
    return true;
  }
  
  public void RaiseCanExecuteChanged()
  {
    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
  }
  
  protected virtual Task HandleException(T parameter, Exception exception)
  {
    return Task.CompletedTask;
  }
  
  protected abstract Task ExecuteInternal(T parameter);
  
  void ICommand.Execute(object? parameter)
  {
    _ = Execute(parameter is T t ? t : default!);
  }

  bool ICommand.CanExecute(object? parameter)
  {
    return !IsBusy && CanExecute(parameter is T t ? t : default!);
  }
}
