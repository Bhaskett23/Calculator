using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestScriptEngine.Interfaces;

namespace TestScriptEngine
{
    public class MathEngine : IMathEngine
    {
        char[] numbers = {'0', '1', '2', '3', '4', '5', '6','7', '8', '9'};
        char[] function = {'*', '+', '-', '/',};
        
        static bool displayHelp = false;
        static string currentDisplay;
        static bool speaking = false;
        List<string> leftParen = new List<string>{"(", "left parenthesis"};
        List<string> rightParen = new List<string> { ")", "right parenthesis" };
        SpeechSynthesizer synth = new SpeechSynthesizer();

        public string ClearingText()
        {
            currentDisplay = "";
            return currentDisplay;
        }

        public string RunCalculation(string wholeInput)
        {
            //--creates the script reader to run the equation
            MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControl();
            try
            {
                sc.Language = "VBScript";
                string expression = wholeInput.Replace("=", "");

                object result = sc.Eval(expression);
                string resultString = result.ToString();
                string resultToReturn = expression + "=" + resultString;
                if (speaking)
                {
                    SpeakAnswer(resultToReturn);
                }

                return resultToReturn;
            }
            catch (Exception)
            {
                string message = "the inputed equation was not valid please input and try again";
                ShowOrSpeak(message);
            }

            return wholeInput.Replace("=", "");
        }
        
        public void DisplayingHelp(bool isDisplayingHelp)
        {
            displayHelp = isDisplayingHelp;
        }

        public void ShowHelp(string input)
        {
            int size = input.Count()-1;
            if (size > 0)
            {
                char lastInput = input[size];
                if (input.Count() != 0)
                {
                    if (displayHelp)
                    {
                        string message;
                        if (numbers.Contains(lastInput))
                        {
                            message = "another number is expected or function";
                            ShowOrSpeak(message);
                        }
                        else if(function.Contains(lastInput))
                        {
                            message = "a number is expected after this function";
                            ShowOrSpeak(message);
                        }
                        else if (lastInput == '(')
                        {
                            message = "a number is expected after this also please remember to close";
                            ShowOrSpeak(message);
                        }
                    }
                }
            }         
        }

        public bool CheckingHelp()
        {
            return displayHelp;
        }
        
        public void SpeakingText(bool isSpeaking)
        {
            speaking = isSpeaking;
        }

        public string SpeakToText(string input)
        {
            if (input == "to the power of")
            {
                currentDisplay += "^";
            }
            else if (input == "=" || input == "calculate")
            {
                currentDisplay += "=";
            }
            else if (input == "clear" || input == "erase")
            {
                currentDisplay = ClearingText();
            }
            else if (input == "multiply by" || input == "times by")
            {
                currentDisplay += "*";
            }
            else if (input == "/" || input == "divided by")
            {
                currentDisplay += "/";
            }
            else if (input == "+" || input == "add" || input == "addition")
            {
                currentDisplay += "+";
            }
            else if (input == "-" || input == "subtract" || input == "negitive")
            {
                currentDisplay += "-";
            }
            else if (input == "(" || input == "left parenthesis")
            {
                currentDisplay += "(";
            }
            else if (input == ")" || input == "right parenthesis")
            {
                currentDisplay += ")";                
            }
            else
            {
                currentDisplay += input;
            }
            
            return currentDisplay;
        }
        
        public void UpdateCurrentInputRecord(string input)
        {
            currentDisplay = input;
        }

        private void ShowOrSpeak(string message)
        {
            if (!speaking)
            {
                MessageBox.Show(message);
            }
            else
            {
                synth.Speak(message);
            }
        }

        private string ConvertToEnglish(string message)
        {
            if (message.Contains("*"))
            {
               message = message.Replace("*", " multiplied by ");
            }
            if (message.Contains("/"))
            {
              message = message.Replace("/", " divided by ");
            }
            if (message.Contains("^"))
            {
                message = message.Replace("^", " to the power of ");
            }
            if (message.Contains("-"))
            {
                message = message.Replace("-", " minus ");
            }

            return message;
        }

        private void SpeakAnswer(string message)
        {
            message = ConvertToEnglish(message);
            synth.Speak(message);
        }
    }
}
