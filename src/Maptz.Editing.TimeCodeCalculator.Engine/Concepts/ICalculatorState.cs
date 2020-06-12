using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
namespace Maptz.Editing.TimeCodeCalculator.Engine
{

    
    public interface ICalculatorState : INotifyPropertyChanged
    {
        string[] Buffer { get; }
        CommandState CommandState { get; set; }
        SmpteFrameRate FrameRate { get; set; }
        long Frames { get; set; }
        TimeCode TimeCode { get; }
        void AddToBuffer(string str);
    }
}