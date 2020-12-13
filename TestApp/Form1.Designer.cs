
namespace TestApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.form11 = new ResourceTest.Form1();
            this.editorControl1 = new MarkDownEditor.EditorControl();
            this.viewerControl1 = new MarkDownEditor.ViewerControl();
            this.SuspendLayout();
            // 
            // form11
            // 
            this.form11.Location = new System.Drawing.Point(12, 28);
            this.form11.Name = "form11";
            this.form11.Size = new System.Drawing.Size(345, 64);
            this.form11.TabIndex = 0;
            // 
            // editorControl1
            // 
            this.editorControl1.FileName = "";
            this.editorControl1.Location = new System.Drawing.Point(12, 60);
            this.editorControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.editorControl1.Name = "editorControl1";
            this.editorControl1.Size = new System.Drawing.Size(295, 248);
            this.editorControl1.TabIndex = 1;
            // 
            // viewerControl1
            // 
            this.viewerControl1.FileName = null;
            this.viewerControl1.Location = new System.Drawing.Point(390, 90);
            this.viewerControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.viewerControl1.Name = "viewerControl1";
            this.viewerControl1.Size = new System.Drawing.Size(175, 173);
            this.viewerControl1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.viewerControl1);
            this.Controls.Add(this.editorControl1);
            this.Controls.Add(this.form11);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private ResourceTest.Form1 form11;
        private MarkDownEditor.EditorControl editorControl1;
        private MarkDownEditor.ViewerControl viewerControl1;
    }
}

