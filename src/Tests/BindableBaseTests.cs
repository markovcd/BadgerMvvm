#pragma warning disable CS8618

using System.Collections.Generic;
using BadgerMvvm.Core;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BadgerMvvm.Tests;

[TestClass]
public class BindableBaseTests
{
  private sealed class TestViewModel : BindableBase<TestViewModel>
  {
    public IBindable<string> Title { get; init; }

    public IBindable<int> Count { get; init; }

    public TestViewModel()
    {
      RegisterProperties();
    }
  }

  [TestMethod]
  public void RegisterProperties_CreatesEveryBindableProperty()
  {
    var viewModel = new TestViewModel();

    viewModel.Title.Should().NotBeNull();
    viewModel.Title.Name.Should().Be("Title");
    viewModel.Count.Should().NotBeNull();
    viewModel.Count.Name.Should().Be("Count");
  }

  [TestMethod]
  public void SettingValue_RaisesPropertyChangedOnViewModelWithPropertyName()
  {
    var viewModel = new TestViewModel();
    var raised = new List<string?>();
    viewModel.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

    viewModel.Title.Value = "abc";
    viewModel.Count.Value = 3;

    raised.Should().Equal("Title", "Count");
  }

  [TestMethod]
  public void SettingValue_RaisesPropertyChangedOnTheProperty()
  {
    var viewModel = new TestViewModel();
    var raised = 0;
    viewModel.Title.PropertyChanged += (_, _) => raised++;

    viewModel.Title.Value = "abc";

    raised.Should().Be(1);
  }

  [TestMethod]
  public void SettingValue_NotifiesListeners()
  {
    var viewModel = new TestViewModel();
    var values = new List<string?>();
    viewModel.Title.ListenForChange(p => values.Add(p.Value));

    viewModel.Title.Value = "abc";
    viewModel.Title.Value = "def";

    values.Should().Equal("abc", "def");
  }

  [TestMethod]
  public void SettingTheSameValue_DoesNotNotify()
  {
    var viewModel = new TestViewModel();
    viewModel.Title.Value = "abc";
    var raised = 0;
    viewModel.PropertyChanged += (_, _) => raised++;

    viewModel.Title.Value = "abc";

    raised.Should().Be(0);
  }

  [TestMethod]
  public void StoppingListening_StopsNotifications()
  {
    var viewModel = new TestViewModel();
    var calls = 0;
    var subscription = viewModel.Title.ListenForChange(_ => calls++);

    viewModel.Title.Value = "abc";
    subscription.Dispose();
    viewModel.Title.Value = "def";

    calls.Should().Be(1);
  }
}
