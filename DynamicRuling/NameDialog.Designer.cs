using System;

namespace DynamicRuling
{
    partial class NameDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(Boolean disposing)
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
            label1 = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            button1 = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(22, 25);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(214, 13);
            label1.TabIndex = 0;
            label1.Text = "Name already exists : Choose another name";
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(25, 64);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(177, 20);
            textBox1.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(25, 115);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "Change";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(ButtonOK_Click);
            // 
            // NameDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(273, 172);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            KeyPreview = true;
            Name = "NameDialog";
            Text = "NameDialog";
            KeyDown += new System.Windows.Forms.KeyEventHandler(ThisEntered);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
    }
}