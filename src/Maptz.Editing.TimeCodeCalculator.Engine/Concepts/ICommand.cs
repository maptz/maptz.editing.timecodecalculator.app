using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
namespace Maptz.Editing.TimeCodeCalculator.Engine
{

    public interface ICommand
    {
        string CommandName { get;  }
        Action<string, ICalculatorState> Action { get;  }
        bool IsCommand(string str);
    }
}