using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
namespace Maptz.Editing.TimeCodeCalculator.Engine
{

    public class Calculator : ICalculator
    {
        /* #region Private Static Fields */
        private static Regex validateIsNumber = new Regex(ISNUMBERREGEXSTRING, RegexOptions.CultureInvariant);
        private static Regex validateTimecode = new Regex(SMPTEREGEXSTRING, RegexOptions.CultureInvariant);
        /* #endregion Private Static Fields */
        /* #region Private Static Methods */
        private static TimeCode GetTimeCode(string timecode, SmpteFrameRate frameRate)
        {
            var tc = new TimeCode(timecode, frameRate);
            return tc;
        }
        private static bool IsNumber(string str) => validateIsNumber.IsMatch(str);
        private static bool IsTimeCode(string str) => validateTimecode.IsMatch(str);

        private static bool IsLooseTimeCode(string str)
        {
            var tcMatchPattern = "^[0-9]+[\\:\\.\\;][0-9]+([\\:\\.\\;][0-9]+)*$";
            Regex validateIsLooseTimeCode = new Regex(tcMatchPattern, RegexOptions.CultureInvariant);

            var isValidMatch =  validateIsLooseTimeCode.IsMatch(str);
            if (!isValidMatch) return false;
            var colonParts = str.Split(new char[] { ':', '.', ';' });
            if (colonParts.Length > 4) return false;
            return true;

        }

        private static TimeCode GetLooseTimeCode(string timecode, SmpteFrameRate frameRate)
        {
            if (IsTimeCode(timecode)) return GetTimeCode(timecode, frameRate);

            //We know there is at least one colon.
            var colonParts = timecode.Split(new char[] { ':', '.', ';' }).Select(p=>int.Parse(p)).ToArray();
            var cpl = colonParts.Length;
            if (cpl < 2 || cpl > 4) throw new InvalidOperationException();

            var newColonParts = new int[4];
            colonParts.CopyTo(newColonParts, 4 - cpl);

            var str = string.Join(":", newColonParts.Select(p =>p.ToString("00")));
            var retval = new TimeCode(str, frameRate);
            return retval;
        }
        /* #endregion Private Static Methods */
        /* #region Private Fields */
        private readonly CalculatorState _state;
        private const string ISNUMBERREGEXSTRING = "^[0-9]+$";
        private const string SMPTEREGEXSTRING = "^(?<Hours>\\d{2}):(?<Minutes>\\d{2}):(?<Seconds>\\d{2})(?::|;)(?<Frames>\\d{2})$";
        /* #endregion Private Fields */
        /* #region Interface: 'Maptz.Editing.TimeCodeCalculator.Engine.ITimeCodeCalculator' Properties */
        ICalculatorState ICalculator.State => this.State;
        /* #endregion Interface: 'Maptz.Editing.TimeCodeCalculator.Engine.ITimeCodeCalculator' Properties */
        /* #region Private Methods */
        private bool IsCommand(string str, out ICommand foundCommand)
        {
            var commands = this.CommandRepository.GetCommands();
            foreach(var command in commands)
            {
                if (command.IsCommand(str))
                {
                    foundCommand = command;
                    return true;
                }
            }
            foundCommand = null;
            return false;
        }
        private void RaiseStateChanged()
        {
            var stateChanged = this.StateChanged;
            if (stateChanged != null) stateChanged(this, new EventArgs());
        }
        /* #endregion Private Methods */
        /* #region Public Delegates */
        public event EventHandler StateChanged;
        /* #endregion Public Delegates */
        /* #region Public Properties */
        public ICommandRepository CommandRepository { get; }
        public ICalculatorState State { get => this._state; }
        /* #endregion Public Properties */
        /* #region Public Constructors */
        public Calculator(ICommandRepository commandRepository)
        {
            this.CommandRepository = commandRepository;
            this._state = new CalculatorState();
            this._state.PropertyChanged += (s, e) =>
            {
                this.RaiseStateChanged();
            };
        }
        /* #endregion Public Constructors */
        /* #region Interface: 'Maptz.Editing.TimeCodeCalculator.Engine.ITimeCodeCalculator' Methods */
        public void ParseInput(string str)
        {
            var trimmed = str.Trim();
            var isTimeCode = IsLooseTimeCode(trimmed);
            var isNumber = IsNumber(trimmed);
            var isOtherCommand = IsCommand(trimmed, out ICommand command);
            if (isTimeCode || isNumber)
            {
                var totalFrames = isTimeCode ? GetLooseTimeCode(trimmed, this.State.FrameRate).TotalFrames : int.Parse(trimmed);
                if (this.State.CommandState == CommandState.ExpectingAddOperand)
                {
                    var newFrames = totalFrames + this.State.Frames;
                    this.State.Frames = newFrames;
                }
                else if (this.State.CommandState == CommandState.ExpectingMinusOperand)
                {
                    var newFrames = totalFrames + this.State.Frames;
                    this.State.Frames = newFrames;
                }
                else
                {
                    var newFrames = totalFrames;
                    this.State.Frames = newFrames;
                }
                this.State.AddToBuffer(this.State.TimeCode.ToString());

            }
            else if (isOtherCommand)
            {
                command.Action(trimmed, this.State);
            }
            else
            {
                this.State.AddToBuffer("????");
            }
        }
        /* #endregion Interface: 'Maptz.Editing.TimeCodeCalculator.Engine.ITimeCodeCalculator' Methods */
    }
}