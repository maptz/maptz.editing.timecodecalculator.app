using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
namespace Maptz.Editing.TimeCodeCalculator.Engine
{

    public class CommandRepository : ICommandRepository
    {
        public const string AddCommandName = "Add";
        public const string ClearCommandName = "Clear";
        public const string MinusCommandName = "Minus";

        private Func<string, bool> GetMatchFunc(string str)
        {
            return new Func<string, bool>(s =>
            {
                return str == s;
            });
        }

        public IEnumerable<ICommand> GetCommands()
        {
            var retval = new List<ICommand>();
            var addCommand = new SimpleCommand(AddCommandName, (str, state) =>
            {
                state.CommandState = CommandState.ExpectingAddOperand;
                state.AddToBuffer("+");
            }, GetMatchFunc("+"));

            retval.Add(addCommand);

            var minusCommand = new SimpleCommand(AddCommandName, (str, state) =>
            {
                state.CommandState = CommandState.ExpectingMinusOperand;
                state.AddToBuffer("+");
            }
            , GetMatchFunc("-"));
            retval.Add(minusCommand);


            var isSetFrameRateCommand = new Func<string, bool>(s =>
            {
                return IsSetFrameRateCommand(s, out SmpteFrameRate frameRate);
            });
            var setFrameRateCommand = new SimpleCommand(AddCommandName, (str, state) =>
            {
                IsSetFrameRateCommand(str, out SmpteFrameRate newFrameRate);
                if (newFrameRate == SmpteFrameRate.Unknown)
                {
                    state.AddToBuffer("Unknown Framerate");
                }
                else
                {
                    state.FrameRate = newFrameRate;
                }
                state.AddToBuffer(state.TimeCode.ToString());

            }, isSetFrameRateCommand);
            retval.Add(setFrameRateCommand);

            return retval;
        }

        private static bool IsSetFrameRateCommand(string str, out SmpteFrameRate frameRate)
        {

            var prefix = "SetFrameRate ";
            if (!str.StartsWith(prefix))
            {
                frameRate = SmpteFrameRate.Unknown;
                return false;
            }

            var remainder = str.Length > prefix.Length ? str.Substring(prefix.Length).Trim() : string.Empty;

            var lookupDictionary = new Dictionary<string, SmpteFrameRate>();
            lookupDictionary.Add("Smpte2398", SmpteFrameRate.Smpte2398);
            lookupDictionary.Add("Smpte24", SmpteFrameRate.Smpte24);
            lookupDictionary.Add("Smpte25", SmpteFrameRate.Smpte25);
            lookupDictionary.Add("Smpte2997Drop", SmpteFrameRate.Smpte2997Drop);
            lookupDictionary.Add("Smpte2997NonDrop", SmpteFrameRate.Smpte2997NonDrop);
            lookupDictionary.Add("Smpte30", SmpteFrameRate.Smpte30);

            if (lookupDictionary.ContainsKey(remainder))
            {
                frameRate = lookupDictionary[remainder];
                return true;
            }
            frameRate = SmpteFrameRate.Unknown;
            return true;


        }
    }
}