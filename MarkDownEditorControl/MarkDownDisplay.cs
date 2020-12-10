using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Markdig;

namespace MarkDownHelper
{
    public partial class MarkDownDisplay : UserControl
    {
        public MarkDownDisplay()
        {
            InitializeComponent();
        }

        private bool readOnly;

        private string fileName;

        public string FileName { get => fileName; set { fileName = value; Show(fileName); } }

        public bool ReadOnly { get => readOnly; set { readOnly = value; panel1.Visible = !readOnly; } }

        private void ShowContent(string html)
        {
            // https://weblogs.asp.net/gunnarpeipman/displaying-custom-html-in-webbrowser-control
            webBrowser1.Navigate("about:blank");
            if (webBrowser1.Document != null)
            {
                webBrowser1.Document.Write(string.Empty);
            }

            string newText = html.EnableNewerFeatures().AddGitHubStyle();
            if (!string.IsNullOrEmpty(FileName))
            {
                newText = newText.TranslatePaths(Path.GetDirectoryName(FileName));
            }
            webBrowser1.DocumentText = newText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, "");
            }

            Editor.Edit(fileName);
            Show(fileName);
        }

        private void Show(string fileName)
        {
            if (File.Exists(fileName))
            {
                this.fileName = fileName;

                string readmeText = File.ReadAllText(fileName);

                // Configure the pipeline with all advanced extensions active
                var pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .UseSoftlineBreakAsHardlineBreak()
                    .UseSyntaxHighlighting()
                    .Build();


                string val = Markdig.Markdown.ToHtml(readmeText, pipeline);
                ShowContent(Markdig.Markdown.ToHtml(readmeText, pipeline));
            }
            else
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    ShowContent("<H3>No file selected</H3>");
                }
                else
                {
                    ShowContent(string.Format("<H3>No {0} found</H3>", Path.GetFileName(fileName)));
                }
            }
        }

    }
}
