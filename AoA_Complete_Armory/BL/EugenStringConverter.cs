﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Drawing;

namespace DM.Armory.BL
{
    /// <summary>
    /// Class that translate synthax coloring in description scripts into actual coloring to display on RichTextBoxes
    /// </summary>
    public class EugenStringConverter
    {
        #region Tags
        private static string[] GREEN_TAG = new string[]{ "#styleGreen{", "}" } ;
        private static string START_TAG = "#";
        #endregion

        public static void ConvertGreenText(string eugenStr, ref RichTextBox textBox)
        {
            int i = 0;
            string[] parts =  eugenStr.Split(GREEN_TAG, StringSplitOptions.None);
            if (parts.Length > 1) 
            {
                if (i % 2 == 0 )
                {
                    textBox.AppendText(parts[i].Replace("#styleGreen", string.Empty));
                }
                else
                {
                    AppendTextWithColor(ref textBox, parts[i], Color.Teal);
                }
            }

            textBox.Find(GREEN_TAG[0]);
        }

        public static void AppendTextWithColor(ref RichTextBox box, string text, Color color)
        {
            
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        public static FlowDocument MakeFlowDocument(string eugenstr, System.Windows.Media.Brush specialColor)
        {
            FlowDocument document = new FlowDocument();
            int i = 0;
            string[] parts = eugenstr.Split(GREEN_TAG, StringSplitOptions.None);
            if (parts.Length > 1)
            {
                if (i % 2 == 0)
                {
                    Paragraph normalPara = new Paragraph(new Run(parts[i].Replace("#styleGreen", string.Empty)));
                    document.Blocks.Add(normalPara);
                }
                else
                {
                    Paragraph coloredPara = new Paragraph(new Run(parts[i]));
                    coloredPara.Foreground = specialColor;
                    document.Blocks.Add(coloredPara);
                }
            }
            return document;
        }
    }
}
