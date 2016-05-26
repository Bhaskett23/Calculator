using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestScriptEngine.Interfaces;

namespace TestScriptEngine
{
    public partial class CalculatorView : Form
    {
        private Button button1;
        private CheckBox checkBox1;
        private TextBox textBox1;
        private IMathEngine _mathEngine;
        private CheckBox checkBox2;
        bool ran = false;
        SpeechRecognitionEngine reader = new SpeechRecognitionEngine();         
        
        public CalculatorView(IMathEngine mathEngine)
        {
            _mathEngine = mathEngine;
            InitializeComponent();
            Choices colors = new Choices();
            colors.Add(new string[] { "1", "2", "3", "4", "5","6", "7", "8", "9", "0",
                "-", "subtract", "negitive",
                "+", "add", "addition",
                "multiply by", "times by",
                "/", "divided by",
                "to the power of",
                "(", "left parenthesis",
                ")", "right parenthesis",
                "clear", "erase",
                "=", "calculate"});

            GrammarBuilder builder = new GrammarBuilder();
            builder.Append(colors);

            Grammar g = new Grammar(builder);

            reader.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(textBox1_Speak);
            reader.LoadGrammar(g);
            reader.SetInputToDefaultAudioDevice();
            reader.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void InitializeComponent()
        {
            //reader.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(178, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(197, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(13, 41);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(81, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Speak Text";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(187, 42);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(85, 17);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "Display Help";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // CalculatorView
            // 
            this.ClientSize = new System.Drawing.Size(279, 70);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "CalculatorView";
            this.Load += new System.EventHandler(this.CalculatorView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void CalculatorView_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ran = false;
            textBox1.Text = _mathEngine.ClearingText();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _mathEngine.SpeakingText((sender as CheckBox).Checked);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string textToDisplay = (sender as TextBox).Text;
            if (!ran && textToDisplay.Contains("="))
            {
                ran = true;
                textToDisplay = _mathEngine.RunCalculation(textToDisplay);
            }
            else
            {
                _mathEngine.UpdateCurrentInputRecord(textToDisplay);
            }

            if (_mathEngine.CheckingHelp())
            {
               _mathEngine.ShowHelp(textToDisplay); 
            }
            
            textBox1.Text = textToDisplay;
        }

        private void textBox1_Speak(object sender, SpeechRecognizedEventArgs e)
        {
            string textToDisplay = e.Result.Text;
            textBox1.Text = _mathEngine.SpeakToText(textToDisplay);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            _mathEngine.DisplayingHelp((sender as CheckBox).Checked);
        }

    }
}
