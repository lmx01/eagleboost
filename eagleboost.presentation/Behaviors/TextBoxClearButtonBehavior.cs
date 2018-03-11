// Author : Shuo Zhang
// 
// Creation :2018-02-12 21:36

namespace eagleboost.presentation.Behaviors
{
  using System.Windows.Controls;
  using System.Windows.Data;
  using eagleboost.presentation.Extensions;

  public class TextBoxClearButtonBehavior : ClearButtonBehavior<TextBox>
  {
    #region Overrides
    protected override void OnButtonClick()
    {
      Control.Text = null;

      var binding = Control.GetBinding(TextBox.TextProperty);
      if (binding != null && binding.UpdateSourceTrigger == UpdateSourceTrigger.Explicit)
      {
        Control.UpdateBindingSource();
      }
    }
    #endregion Overrides
  }
}