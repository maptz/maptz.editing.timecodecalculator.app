using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
namespace Maptz.Editing.TimeCodeCalculator.Engine
{

    internal class CalculatorState : ICalculatorState
    {
        /* #region Private Fields */
        private CommandState _commandState = CommandState.None;
        private SmpteFrameRate _frameRate = SmpteFrameRate.Smpte25;
        private long _frames = 0;
        private TimeCode _timeCode;
        private string name;
        /* #endregion Private Fields */
        /* #region Private Methods */
        private void InvalidateTimeCode()
        {
            this.TimeCode = TimeCode.FromFrames(this.Frames, this.FrameRate);
        }
        /* #endregion Private Methods */
        /* #region Protected Methods */
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        /* #endregion Protected Methods */
        /* #region Public Delegates */
        public event PropertyChangedEventHandler PropertyChanged;
        /* #endregion Public Delegates */
        /* #region Public Fields */
        public string[] _buffer = new string[0];
        /* #endregion Public Fields */
        /* #region Public Properties */
        public string[] Buffer
        {
            get => _buffer;
            set => SetField(ref _buffer, value);
        }
        public SmpteFrameRate FrameRate
        {
            get => _frameRate;
            set
            {
                SetField(ref _frameRate, value);
                this.InvalidateTimeCode();
            }
        }
        public long Frames
        {
            get => _frames;
            set
            {
                SetField(ref _frames, value);
                this.InvalidateTimeCode();
            }
        }
        public TimeCode TimeCode
        {
            get => _timeCode;
            private set => SetField(ref _timeCode, value);
        }
        /* #endregion Public Properties */
        /* #region Interface: 'Maptz.Editing.TimeCodeCalculator.Engine.ITimeCodeCalculatorState' Properties */
        public CommandState CommandState
        {
            get => this._commandState;
            set => SetField(ref _commandState, value);
        }
        /* #endregion Interface: 'Maptz.Editing.TimeCodeCalculator.Engine.ITimeCodeCalculatorState' Properties */
        /* #region Public Constructors */
        public CalculatorState()
        {
            this.InvalidateTimeCode();
        }
        /* #endregion Public Constructors */
        /* #region Interface: 'Maptz.Editing.TimeCodeCalculator.Engine.ITimeCodeCalculatorState' Methods */
        public void AddToBuffer(string str)
        {
            var newBuffer = new string[this.Buffer.Length + 1];
            if (this.Buffer.Length > 0) { this.Buffer.CopyTo(newBuffer, 0); }
            newBuffer[newBuffer.Length - 1] = str;
            this.Buffer = newBuffer;
        }
        /* #endregion Interface: 'Maptz.Editing.TimeCodeCalculator.Engine.ITimeCodeCalculatorState' Methods */
    }
}