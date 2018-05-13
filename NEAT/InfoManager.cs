using System;
using System.Windows.Forms;

namespace NEAT
{
    public static class InfoManager
    {
        private static RichTextBox textBox;

        public static void setTextBox(RichTextBox richTextBox) => textBox = richTextBox;

        public static void addLine(String line)
        {
            textBox.AppendText(line + Environment.NewLine);
            // Console.WriteLine(line);

            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();
        }

        public static void clearLine()
        {
            addLine("");
        }
    }
}
