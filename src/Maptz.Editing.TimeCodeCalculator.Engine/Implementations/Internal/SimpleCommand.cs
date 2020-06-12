using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
namespace Maptz.Editing.TimeCodeCalculator.Engine
{

    internal class SimpleCommand : CommandBase
    {
        /* #region Private Fields */
        private Func<string, bool> _isCommandMethod;
        /* #endregion Private Fields */
        /* #region Public Constructors */
        public SimpleCommand(string commandName, Action<string, ICalculatorState> action, Func<string, bool> isCommand) : base(commandName, action)
        {
            this._isCommandMethod = isCommand;
        }
        /* #endregion Public Constructors */
        /* #region Public Methods */
        public override bool IsCommand(string str)
        {
            return this._isCommandMethod(str);
        }
        /* #endregion Public Methods */
    }
}