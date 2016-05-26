using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using TestScriptEngine.Interfaces;

namespace TestScriptEngine
{
    public class Calculator
    {
        public static void Main()
        {
            IMathEngine mathEngine = new MathEngine();
            CalculatorView view = new CalculatorView(mathEngine);

            view.ShowDialog();            
        }     
    }

}
