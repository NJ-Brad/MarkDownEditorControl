namespace MarkDownEditor
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.editorControl1 = new MarkDownEditor.EditorControl();
            this.SuspendLayout();
            // 
            // editorControl1
            // 
            this.editorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorControl1.FileName = "";
            this.editorControl1.Location = new System.Drawing.Point(0, 0);
            this.editorControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.editorControl1.Name = "editorControl1";
            this.editorControl1.Size = new System.Drawing.Size(1003, 546);
            this.editorControl1.TabIndex = 0;
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 546);
            this.Controls.Add(this.editorControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Editor";
            this.Text = "MarkDown Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Editor_FormClosing);
            this.ResumeLayout(false);

        }

        private void WebBrowser1_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private EditorControl editorControl1;
    }
}