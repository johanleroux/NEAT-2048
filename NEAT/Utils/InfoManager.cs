using System;
using System.Windows.Forms;

namespace NEAT.Utils
{
    public static class InfoManager
    {
        private static RichTextBox textBox;

        public static void setTextBox(RichTextBox richTextBox) => textBox = richTextBox;

        public static void addLine(String line)
        {
            textBox.AppendText(line + Environment.NewLine);
            // Console.WriteLine(line);

            limitLines();

            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();
        }

        public static void clearLine()
        {
            addLine("");
        }

        public static void limitLines()
        {
            int maxLines = 500;

            if (textBox.Lines.Length > maxLines)
            {
                string[] newLines = new string[maxLines];

                Array.Copy(
                    textBox.Lines,
                    textBox.Lines.Length - maxLines,
                    newLines,
                    0,
                    maxLines
                );

                textBox.Lines = newLines;
            }
        }
    }
}
