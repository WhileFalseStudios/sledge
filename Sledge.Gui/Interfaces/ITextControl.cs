using System;

namespace Sledge.Gui.Interfaces
{
    public interface ITextControl : IControl
    {
        string Text { get; set; }
        int FontSize { get; }
        bool Bold { get; set; }
        bool Italic { get; set; }

        event EventHandler TextChanged;
    }
}