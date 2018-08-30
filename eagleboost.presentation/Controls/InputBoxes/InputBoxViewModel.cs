// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-29 4:48 PM

namespace eagleboost.presentation.Controls.InputBoxes
{
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Contracts;
  using eagleboost.presentation.Interactivity;

  public class InputBoxViewModel : ViewController, IHeader
  {
    #region Declarations
    private string _text;
    #endregion Declarations

    #region ctors
    static InputBoxViewModel()
    {
      Invalidate(v => v.OkCommand).By(v => ((InputBoxViewModel) v).Text);
    }

    public InputBoxViewModel()
    {
      Header = "Input";
      Description = "Please enter";
      InputLabel = "Input:";
    }
    #endregion ctors

    #region Public Properties
    public string Header { get; set; }

    public string Description { get; set; }

    public string InputLabel { get; set; }

    public string Text
    {
      get { return _text; }
      set { SetValue(ref _text, value); }
    }
    #endregion Public Properties

    #region Overrides
    protected override bool CanHandleOk
    {
      get { return Text.HasValue(); }
    }
    #endregion Overrides
  }
}