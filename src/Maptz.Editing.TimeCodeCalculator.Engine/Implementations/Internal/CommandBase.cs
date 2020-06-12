using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
namespace Maptz.Editing.TimeCodeCalculator.Engine
{

    internal abstract class CommandBase : ICommand
    {
        /* #region Interface: 'Maptz.Editing.TimeCodeCalculator.Engine.ICommand' Properties */
        public Action<string, ICalculatorState> Action { get; }
        public string CommandName { get; }
        /* #endregion Interface: 'Maptz.Editing.TimeCodeCalculator.Engine.ICommand' Properties */
        /* #region Public Constructors */
        public CommandBase(string commandName, Action<string, ICalculatorState> action)
        {
            this.CommandName = commandName;
            this.Action = action;
        }
        /* #endregion Public Constructors */
        /* #region Interface: 'Maptz.Editing.TimeCodeCalculator.Engine.ICommand' Methods */
        public abstract bool IsCommand(string str);
        /* #endregion Interface: 'Maptz.Editing.TimeCodeCalculator.Engine.ICommand' Methods */
    }
}