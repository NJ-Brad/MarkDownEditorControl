using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Markdig;
using Microsoft.Web.WebView2.WinForms;
using PropertyEditor;

namespace MarkDownEditor
{
    public partial class EditorForm : Form
    {
        public EditorForm()
        {
            InitializeComponent();
        }

        public string FileName {
            get {
                return this.fileName;
            }
            set {
                this.fileName = value;
            }
        }

        private static string CreateLinkText()
        {
            string rtnVal = string.Empty;

            PropertyForm pf = new PropertyForm();
            LinkDefinition ld = new LinkDefinition();

            if (pf.ShowDialog(ld) == DialogResult.OK)
            {
                if ((string.IsNullOrEmpty(ld.Display)) && (!string.IsNullOrEmpty(ld.LinkText)))
                {
                    ld.Display = ld.LinkText;
                }

                if (string.IsNullOrEmpty(ld.LinkText))
                {
                    return string.Empty;
                }

                if (!string.IsNullOrEmpty(ld.Tooltip))
                {
                    ld.LinkText = string.Format("{0} \"{1}\"", ld.LinkText, ld.Tooltip);
                }

                //rtnVal = string.Format("[{0}]({1})", ld.Display, ld.LinkText);
                rtnVal = $"[{ld.Display}]({ld.LinkText})";
            }

            return rtnVal;
        }

        private static string CreateImageText()
        {
            string rtnVal = string.Empty;

            PropertyForm pf = new PropertyForm();
            ImageDefinition id = new ImageDefinition();

            if (pf.ShowDialog(id) == DialogResult.OK)
            {
                if ((string.IsNullOrEmpty(id.Display)) && (!string.IsNullOrEmpty(id.ImageUrl)))
                {
                    id.Display = id.ImageUrl;
                }

                if (string.IsNullOrEmpty(id.ImageUrl))
                {
                    return string.Empty;
                }

                if (!string.IsNullOrEmpty(id.Tooltip))
                {
                    id.ImageUrl = string.Format("{0} \"{1}\"", id.ImageUrl, id.Tooltip);
                }

                rtnVal = $"![{id.Display}]({id.ImageUrl})";
            }

            return rtnVal;
        }


        private static string CreateDefinitionText()
        {
            string rtnVal = string.Empty;

            PropertyForm pf = new PropertyForm();
            DefinitionDefinition dd = new DefinitionDefinition();

            if (pf.ShowDialog(dd) == DialogResult.OK)
            {
                //rtnVal = string.Format("<dt>{0}</dt>\n<dd>{1}</dd>\n", dd.Term, dd.Meaning);
                rtnVal = $"<dt>{dd.Term}</dt>\n<dd>{dd.Meaning}</dd>\n";
            }

            return rtnVal;
        }

        public static string CreateLanguageText()
        {
            string rtnVal = string.Empty;

            PropertyForm pf = new PropertyForm();
            CodeDefinition cd = new CodeDefinition();

            if (pf.ShowDialog(cd) == DialogResult.OK)
            {
                rtnVal = $"```{cd.Language}\n";
            }

            return rtnVal;
        }


        public static void Edit(string fileName)
        {
            EditorForm ed = new EditorForm();
            ed.FileName = fileName;
            ed.ShowDialog();
        }

        Operations operations = new Operations();

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Show();
        }

        private new void Show()
        {
            base.Show();
//            ShowText(richTextBox1.Text, webBrowser1);
        }

        //        private void ShowText(string rawText, WebBrowser control)
        private void ShowText(string rawText, WebView2 control)
        {
            string readmeText = rawText;

            // https://talk.commonmark.org/t/markdig-markdown-processor-for-net/2106

            //https://github.com/lunet-io/markdig
            // https://marketplace.visualstudio.com/items?itemName=MadsKristensen.MarkdownEditor
            // http://markdownpad.com/
            // https://guides.github.com/pdfs/markdown-cheatsheet-online.pdf
            // https://confluence.atlassian.com/bitbucketserver/markdown-syntax-guide-776639995.html
            // https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet#tables

            // https://github.github.com/github-flavored-markdown/sample_content.html

            // Configure the pipeline with all advanced extensions active
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseSoftlineBreakAsHardlineBreak()
                .UseSyntaxHighlighting()
                .Build();

            //string val = Markdig.Markdown.ToHtml(readmeText.FixLineFeeds(), pipeline);
//            ShowContent(control, Markdig.Markdown.ToHtml(readmeText.FixLineFeeds().TranslatePaths(Path.GetDirectoryName(fileName).GenerateToc()), pipeline));
            ShowContent(control, Markdig.Markdown.ToHtml(readmeText.TranslatePaths(Path.GetDirectoryName(fileName)).GenerateToc(), pipeline));
        }

        private void Show(string fileName, WebView2 control)
        {
            if (File.Exists(fileName))
            {
                string readmeText = File.ReadAllText(fileName);

                // Configure the pipeline with all advanced extensions active
                var pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .UseSoftlineBreakAsHardlineBreak()
                    .UseSyntaxHighlighting()
                    .Build();

                string val = Markdig.Markdown.ToHtml(readmeText, pipeline);
                ShowContent(control, Markdig.Markdown.ToHtml(readmeText.GenerateToc(), pipeline));
            }
            else
            {
                ShowContent(control, string.Format("<H3>No {0} found</H3>", Path.GetFileName(fileName)));
            }
        }

        private void ShowContent(WebView2 control, string html)
        {
            // https://weblogs.asp.net/gunnarpeipman/displaying-custom-html-in-webbrowser-control
            control.NavigateToString("about:blank");
            //if (control.Document != null)
            //{
            //    control.Document.Write(string.Empty);
            //}
            //control.DocumentText = html.EnableNewerFeatures().AddGitHubStyle().TranslatePaths(Path.GetDirectoryName(fileName));

            control.NavigateToString(html.EnableNewerFeatures().AddGitHubStyle().TranslatePaths(Path.GetDirectoryName(fileName)));
        }

        string fileName = string.Empty;

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Markdown Files (*.md)|*.md|All Files (*.*)|*.*";
            ofd.CheckFileExists = true;
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadFileToEdit(ofd.FileName);
            }
        }
 
        private void LoadFileToEdit(string fileName)
        {
            //this.fileName = fileName;
            //richTextBox1.Text = File.ReadAllText(fileName);
            //Show();
            //dirty = false;
        }

        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (dirty)
            //{
            //    switch (MessageBox.Show("Changes have not been saved.  Save now?", "Warning", MessageBoxButtons.YesNoCancel))
            //    {
            //        case System.Windows.Forms.DialogResult.Yes:
            //            File.WriteAllText(fileName, richTextBox1.Text);
            //            break;
            //        case System.Windows.Forms.DialogResult.No:
            //            break;
            //        case System.Windows.Forms.DialogResult.Cancel:
            //            e.Cancel = true;
            //            break;
            //    }
            //}
        }

        bool dirty = false;

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            dirty = true;
        }

    }
}
