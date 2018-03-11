// Author : Shuo Zhang
// 
// Creation :2018-02-12 21:43

namespace eagleboost.presentation.Extensions
{
  using System.Windows.Controls;

  public static class TextBoxExt
  {
    public static void UpdateBindingSource(this TextBox textBox)
    {
      textBox.UpdateBindingSource(TextBox.TextProperty);
    }
  }
}