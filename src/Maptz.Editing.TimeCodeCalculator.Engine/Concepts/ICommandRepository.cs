using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
namespace Maptz.Editing.TimeCodeCalculator.Engine
{



    public interface ICommandRepository
    {
        IEnumerable<ICommand> GetCommands();
    }
}