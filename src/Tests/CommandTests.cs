using System;
using System.Threading.Tasks;
using BadgerMvvm.Core;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BadgerMvvm.Tests;

[TestClass]
public class CommandTests
{
  [TestMethod]
  public async Task CommandBuilder_BuildsCanExecute()
  {
    TestParam? canExecuteParam = null;
    var expectedParam = new TestParam("ssfdtgdf");

    bool CanExecute(TestParam param) 
    {
      canExecuteParam = param;
      return true;
    }

    var command = CommandBuilder.WithExecute<TestParam>(_ => {})
      .WithCanExecute(CanExecute)
      .Build();
    
    await command.Execute(expectedParam);
    
    canExecuteParam.Should().Be(expectedParam);
  }
  
  [TestMethod]
  public async Task CommandBuilder_BuildsAsyncCanExecute()
  {
    TestParam? canExecuteParam = null;
    var expectedParam = new TestParam("ssfdtgdf");

    bool CanExecute(TestParam param) 
    {
      canExecuteParam = param;
      return true;
    }

    var command = CommandBuilder.WithExecute<TestParam>(_ => Task.CompletedTask)
      .WithCanExecute(CanExecute)
      .Build();
    
    await command.Execute(expectedParam);
    
    canExecuteParam.Should().Be(expectedParam);
  }
  
  [TestMethod]
  public async Task CommandBuilder_BuildsExceptionHandler()
  {
    var exceptionHandled = false;
    var exception = new Exception();

    void ExceptionHandler(Exception e) => exceptionHandled = e == exception;

    var command = CommandBuilder.WithExecute(() => throw exception)
      .WithExceptionHandler(ExceptionHandler)
      .Build();
    
    await command.Execute();
    
    exceptionHandled.Should().BeTrue();
  }
  
  [TestMethod]
  public async Task CommandBuilder_BuildsParamExceptionHandler()
  {
    var exceptionHandled = false;
    var exception = new Exception();

    void ExceptionHandler<T>(T param, Exception e) => exceptionHandled = e == exception;

    var command = CommandBuilder.WithExecute<TestParam>(_ => throw exception)
      .WithExceptionHandler(ExceptionHandler)
      .Build();
    
    await command.Execute(new TestParam("xxx"));
    
    exceptionHandled.Should().BeTrue();

  }
  
  [TestMethod]
  public async Task CommandBuilder_BuildsAsyncExceptionHandler()
  {
    var exceptionHandled = false;
    var exception = new Exception();

    Task ExceptionHandler(Exception e) 
    {
      exceptionHandled = e == exception;
      return Task.CompletedTask;
    }

    var command = CommandBuilder.WithExecute(() => Task.FromException(exception))
      .WithExceptionHandler(ExceptionHandler)
      .Build();
    
    await command.Execute();
    
    exceptionHandled.Should().BeTrue();
  }
  
  [TestMethod]
  public async Task CommandBuilder_BuildsParamAsyncExceptionHandler()
  {
    var exceptionHandled = false;
    var exception = new Exception();

    Task ExceptionHandler<T>(T param, Exception e)
    {
      exceptionHandled = e == exception;
      return Task.CompletedTask;
    }

    var command = CommandBuilder.WithExecute<TestParam>(_ => Task.FromException(exception))
      .WithExceptionHandler(ExceptionHandler)
      .Build();
    
    await command.Execute(new TestParam(string.Empty));
    
    exceptionHandled.Should().BeTrue();
  }
  
  [TestMethod]
  public async Task CommandBuilder_ExecutesWithExecuteLambda()
  {
    var executed = false;

    void Execute() => executed = true;
    
    var command = CommandBuilder.WithExecute(Execute).Build();
    
    await command.Execute();
    
    executed.Should().BeTrue();
  }
  
  [TestMethod]
  public async Task CommandBuilder_ExecutesWithParamExecuteLambda()
  {
    TestParam? executeParam = null;
    var expectedParam = new TestParam("cxdsff");

    void Execute(TestParam param) => executeParam = param;
    
    var command = CommandBuilder.WithExecute<TestParam>(Execute).Build();
    
    await command.Execute(expectedParam);
    
    executeParam.Should().Be(expectedParam);
  }
  
  [TestMethod]
  public async Task CommandBuilder_ExecutesWithAsyncExecuteLambda()
  {
    var executed = false;

    Task Execute() 
    {
      executed = true;
      return Task.CompletedTask;
    }
    
    var command = CommandBuilder.WithExecute(Execute).Build();
    
    await command.Execute();
    
    executed.Should().BeTrue();
  }
  
  [TestMethod]
  public async Task CommandBuilder_ExecutesWithParamAsyncExecuteLambda()
  {
    TestParam? executeParam = null;
    var expectedParam = new TestParam("cxdsff");

    Task Execute(TestParam param)
    {
      executeParam = param;
      return Task.CompletedTask;
    }
    
    var command = CommandBuilder.WithExecute<TestParam>(Execute).Build();
    
    await command.Execute(expectedParam);
    
    executeParam.Should().Be(expectedParam);
  }

  private record TestParam(string Value);
}
