using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Markdig;
using Microsoft.Web.WebView2.WinForms;

namespace MarkDownHelper
{
    public delegate string OperationDelegate(string s);  

    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();
// http://stackoverflow.com/questions/4823468/comments-in-markdown
// (empty line)
// [comment]: # (This actually is the most platform independent comment)

            // https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet
            // I didn't implement the Youtube video option
            operations.Add("H1", "# ");
            operations.Add("H2", "## ");
            operations.Add("H3", "### ");
            operations.Add("H4", "#### ");
            operations.Add("H5", "##### ");
            operations.Add("H6", "###### ");
            operations.Add("Alt-H1", string.Empty, t => { return "\r\n" + new string('=', t.Length); });
            operations.Add("Alt-H2", string.Empty, t => { return "\r\n" + new string('-', t.Length); });
            operations.Add("Italic", "_", "_");
            operations.Add("Bold", "**", "**");
            operations.Add("Strike", "~~", "~~");
            operations.Add("HR", "\r\n----");
            operations.Add("Link", t => { return CreateLinkText(); }, string.Empty);
            operations.Add("Ordered List", "1. ");
            operations.Add("Unordered List", "+ ");
            operations.Add("Image", t => { return CreateImageText(); }, string.Empty);
            //operations.Add("Code", t => { return CodeForm.CreateLanguageText(); }, "\n```");
            operations.Add("Code", t => { return CreateLanguageText(); }, "\n```");
            operations.Add("Quote", t => { StringBuilder sb = new StringBuilder("\n" + t); sb.Replace("\n", "\n> "); return sb.ToString().TrimStart().TrimEnd(new char[]{'\n', '>', ' '}); }, string.Empty);
            operations["Quote"].KeepOriginal = false;
            operations.Add("Dictionary", "<dl>\n", "</dl>");
            //operations.Add("Definition", t => { return DefinitionForm.CreateDefinitionText(); }, string.Empty);
            operations.Add("Definition", t => { return CreateDefinitionText(); }, string.Empty);
            operations.Add("Header", t => { return TableForm.CreateTableText(); }, string.Empty);
            operations.Add("Row", string.Empty, t => { int cols = t.Occurrences('|') + 1; if (t.StartsWith("|")) cols--; if (t.TrimEnd().EndsWith("|")) cols--;
            StringBuilder sb = new StringBuilder("\n|"); for (int i = 0; i < cols; i++) { sb.Append("value|"); }
                return sb.ToString();
            });
            operations.Add("Task", "- [ ]", string.Empty);
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
            Editor ed = new Editor();
            ed.FileName = fileName;
            ed.ShowDialog();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (!string.IsNullOrEmpty(fileName))
            {
                openToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
                LoadFileToEdit(fileName);
            }

            webBrowser1.EnsureCoreWebView2Async();
        }

        Operations operations = new Operations();

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Show();
        }

        private new void Show()
        {
            base.Show();
            ShowText(richTextBox1.Text, webBrowser1);
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

        private void Modify(RichTextBox control, string prefix)
        {
            Modify(control, prefix, string.Empty, true);
        }

        private void Modify(RichTextBox control, Operation op)
        {
            string currentText = control.SelectedText.TrimEnd();

            string prefix = op.Prefix;
            string suffix = op.Suffix;

            if (op.PrefixDelegate != null)
            {
                prefix = op.PrefixDelegate(currentText);
            }

            if (op.SuffixDelegate != null)
            {
                suffix = op.SuffixDelegate(currentText);
            }

            Modify(richTextBox1, prefix, suffix, op.KeepOriginal);
        }

        private void Modify(RichTextBox control, string prefix, string suffix, bool keepOriginal)
        {
            string currentText = control.SelectedText.TrimEnd();
            bool replaceEOL = !control.SelectedText.Equals(control.SelectedText.TrimEnd());
            string newText = string.Format("{0}{1}{2}{3}", prefix, keepOriginal ? currentText : string.Empty, suffix, replaceEOL ? "\r\n" : string.Empty);
            control.SelectedText = newText;
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            string buttonText = GetButtonText(sender);

            if (operations.ContainsKey(buttonText))
            {
                Operation op = operations[buttonText];

                Modify(richTextBox1, op);
                Show();
            }
        }

        private string GetButtonText(object sender)
        {
            string rtnval = string.Empty;

            if (sender is ToolStripDropDownItem)
            {
                rtnval = (sender as ToolStripDropDownItem).Text;
            }
            if(sender is ToolStripButton)
            {
                rtnval = (sender as ToolStripButton).Text;
            }

            return rtnval;
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
            this.fileName = fileName;
            richTextBox1.Text = File.ReadAllText(fileName);
            Show();
            dirty = false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                File.WriteAllText(fileName, richTextBox1.Text);
            }
            dirty = false;
        }

        private void sampleTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Markdown Files (*.md)|*.md|All Files (*.*)|*.*";
            sfd.OverwritePrompt = true;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fileName = sfd.FileName;
                File.WriteAllText(fileName, richTextBox1.Text);
            }
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {

        }

        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dirty)
            {
                switch (MessageBox.Show("Changes have not been saved.  Save now?", "Warning", MessageBoxButtons.YesNoCancel))
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        File.WriteAllText(fileName, richTextBox1.Text);
                        break;
                    case System.Windows.Forms.DialogResult.No:
                        break;
                    case System.Windows.Forms.DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        bool dirty = false;

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            dirty = true;
        }

        private void gitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = @"[https://guides.github.com/features/wikis/](https://guides.github.com/features/wikis/)
# Project name:
Your project’s name is the first thing people will see upon scrolling down to your README, and is included upon creation of your README file.

# Description:
A description of your project follows. A good description is clear, short, and to the point. Describe the importance of your project, and what it does.

# Table of Contents:
Optionally, include a table of contents in order to allow other people to quickly naviagte especially long or detailed READMEs.

# Installation:
Installation is the next section in an effective README. Tell other users how to install your project locally. Optionally, include a gif to make the process even more clear for other people.

# Usage:
The next section is usage, in which you instruct other people on how to use your project after they’ve installed it. This would also be a good place to include screenshots of your project in action.

# Contributing:
Larger projects often have sections on contributing to their project, in which contribution instructions are outlined. Sometimes, this is a separate file. If you have specific contribution preferences, explain them so that other developers know how to best contribute to your work. To learn more about how to help others contribute, check out the guide for (setting guidelines for repository contributors)[https://help.github.com/articles/setting-guidelines-for-repository-contributors/].

# Credits:
Include a section for credits in order to highlight and link to the authors of your project.

# License:
Finally, include a section for the license of your project. For more information on choosing a license, check out GitHub’s licensing guide!";

        }

        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {   //click event
                //MessageBox.Show("you got it!");
                ContextMenuStrip contextMenu = new System.Windows.Forms.ContextMenuStrip();
                ToolStripMenuItem menuItem = new ToolStripMenuItem("Cut");
                menuItem.Click += new EventHandler(CutAction);
                contextMenu.Items.Add(menuItem);
                menuItem = new ToolStripMenuItem("Copy");
                menuItem.Click += new EventHandler(CopyAction);
                contextMenu.Items.Add(menuItem);
                menuItem = new ToolStripMenuItem("Paste");
                menuItem.Click += new EventHandler(PasteAction);
                contextMenu.Items.Add(menuItem);

                richTextBox1.ContextMenuStrip = contextMenu;
            }
        }
        void CutAction(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        //void CopyAction(object sender, EventArgs e)
        //{
        //    Graphics objGraphics;
        //    Clipboard.SetData(DataFormats.Rtf, richTextBox1.SelectedRtf);
        //    Clipboard.Clear();
        //}

        //void PasteAction(object sender, EventArgs e)
        //{
        //    if (Clipboard.ContainsText(TextDataFormat.Rtf))
        //    {
        //        richTextBox1.SelectedRtf
        //            = Clipboard.GetData(DataFormats.Rtf).ToString();
        //    }
        //}

        void CopyAction(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox1.SelectedText);
        }

        void PasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                richTextBox1.Text
                    += Clipboard.GetText(TextDataFormat.Text).ToString();
            }
        }
    }

    public class Operations : Dictionary<string, Operation> {
        public void Add(string trigger, string prefix)
        {
            Add(trigger, prefix, string.Empty);
        }

        public void Add(string trigger, string prefix, string suffix) {
            base.Add(trigger, new Operation(trigger, prefix, suffix));
        }

        public void Add(string trigger, OperationDelegate prefix, string suffix)
        {
            base.Add(trigger, new Operation(trigger, prefix, suffix));
        }

        public void Add(string trigger, string prefix, OperationDelegate suffix)
        {
            base.Add(trigger, new Operation(trigger, prefix, suffix));
        }

        public void Add(string trigger, OperationDelegate prefix, OperationDelegate suffix)
        {
            base.Add(trigger, new Operation(trigger, prefix, suffix));
        }
    }

    public class Operation
    {
        string trigger = string.Empty;
        string prefix = string.Empty;
        string suffix = string.Empty;
        OperationDelegate prefixDelegate = null;
        OperationDelegate suffixDelegate = null;
        bool keepOriginal = true;

        public Operation(string trigger, string prefix)
        {
            this.trigger = trigger;
            this.prefix = prefix;
        }

        public Operation(string trigger, OperationDelegate prefix)
        {
            this.trigger = trigger;
            this.prefixDelegate = prefix;
        }

        public Operation(string trigger, string prefix, string suffix)
        {
            this.trigger = trigger;
            this.prefix = prefix;
            this.suffix = suffix;
        }

        public Operation(string trigger, OperationDelegate prefix, string suffix)
        {
            this.trigger = trigger;
            this.prefixDelegate = prefix;
            this.suffix = suffix;
        }

        public Operation(string trigger, string prefix, OperationDelegate suffix)
        {
            this.trigger = trigger;
            this.prefix = prefix;
            this.suffixDelegate = suffix;
        }

        public Operation(string trigger, OperationDelegate prefix, OperationDelegate suffix)
        {
            this.trigger = trigger;
            this.prefixDelegate = prefix;
            this.suffixDelegate = suffix;
        }

        public bool KeepOriginal {
            get {
                return this.keepOriginal;
            }
            set {
                this.keepOriginal = value;
            }
        }

        public OperationDelegate PrefixDelegate {
            get {
                return this.prefixDelegate;
            }
            set {
                this.prefixDelegate = value;
            }
        }

        public OperationDelegate SuffixDelegate {
            get {
                return this.suffixDelegate;
            }
            set {
                this.suffixDelegate = value;
            }
        }

        public string Trigger {
            get {
                return this.trigger;
            }
            set {
                this.trigger = value;
            }
        }

        public string Prefix {
            get {
                return this.prefix;
            }
            set {
                this.prefix = value;
            }
        }

        public string Suffix {
            get {
                return this.suffix;
            }
            set {
                this.suffix = value;
            }
        }
    }
}
