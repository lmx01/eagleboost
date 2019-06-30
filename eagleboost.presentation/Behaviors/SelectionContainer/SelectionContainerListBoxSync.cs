// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 6:59 PM

namespace eagleboost.presentation.Behaviors.SelectionContainer
{
  using System;
  using System.Windows.Controls;
  using eagleboost.core.Collections;

  /// <summary>
  /// SelectionContainerListBoxSync
  /// </summary>
  public class SelectionContainerListBoxSync : SelectionContainerSync<ListBox>
  {
    #region Overrides
    protected override void OnAttached()
    {
      base.OnAttached();

      InitializeSelection();
    }

    protected override void OnDetaching()
    {
      UnhookSelectionContainer();

      base.OnDetaching();
    }

    protected override void OnSelectionContainerChanged()
    {
      var c = SelectionContainer;
      if (c != null)
      {
        InitializeSelection();
        HookSelectionContainer();
      }
    }

    protected override void OnSelectorSelectionChanged(SelectionChangedEventArgs e)
    {
      UnhookSelectionContainer();

      base.OnSelectorSelectionChanged(e);

      HookSelectionContainer();
    }
    #endregion Overrides

    #region Private Methods
    private void HookSelectionContainer()
    {
      var c = SelectionContainer;
      if (c != null)
      {
        c.ItemsSelected += HandleItemsSelected;
        c.ItemsUnselected += HandleItemsUnselected;
        c.ItemsCleared += HandleItemsCleared;
      }
    }

    private void UnhookSelectionContainer()
    {
      var c = SelectionContainer;
      if (c != null)
      {
        c.ItemsSelected -= HandleItemsSelected;
        c.ItemsUnselected -= HandleItemsUnselected;
        c.ItemsCleared -= HandleItemsCleared;
      }
    }

    private void InitializeSelection()
    {
      var listBox = AssociatedObject;
      var c = SelectionContainer;
      if (listBox == null || c == null)
      {
        return;
      }

      if (listBox.SelectedItems.Count == 0)
      {
        foreach (var i in c.SelectedItems)
        {
          listBox.SelectedItems.Add(i);
        }
      }
    }
    #endregion Private Methods

    #region Event Handlers
    private void HandleItemsCleared(object sender, EventArgs e)
    {
      var listBox = AssociatedObject;
      if (listBox != null)
      {
        listBox.SelectedItems.Clear();
      }
    }

    private void HandleItemsSelected(object sender, ItemsSelectedEventArgs e)
    {
      var listBox = AssociatedObject;
      if (listBox != null)
      {
        foreach (var i in e.Items)
        {
          listBox.SelectedItems.Add(i);
        }
      }
    }

    private void HandleItemsUnselected(object sender, ItemsUnselectedEventArgs e)
    {
      var listBox = AssociatedObject;
      if (listBox != null)
      {
        foreach (var i in e.Items)
        {
          listBox.SelectedItems.Remove(i);
        }
      }
    }
    #endregion Event Handlers
  }
}