using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestScriptEngine.Interfaces
{
    public interface IMathEngine
    {
        string RunCalculation(string wholeInput);
        string ClearingText();
        void DisplayingHelp(bool isDisplayingHelp);
        void ShowHelp(string input);
        bool CheckingHelp();
        void SpeakingText(bool isSpeaking);
        string SpeakToText(string input);
        void UpdateCurrentInputRecord(string input);
    }
}
