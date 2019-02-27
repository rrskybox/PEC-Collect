namespace PEC_Collect
{
    partial class FormPECDataCollection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPECDataCollection));
            this.StartButton = new System.Windows.Forms.Button();
            this.EndButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.AtFocus3Checkbox = new System.Windows.Forms.CheckBox();
            this.LoopsCounter = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.LoopsCounter)).BeginInit();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.ForeColor = System.Drawing.Color.Black;
            this.StartButton.Location = new System.Drawing.Point(313, 139);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // EndButton
            // 
            this.EndButton.ForeColor = System.Drawing.Color.Black;
            this.EndButton.Location = new System.Drawing.Point(403, 139);
            this.EndButton.Name = "EndButton";
            this.EndButton.Size = new System.Drawing.Size(75, 23);
            this.EndButton.TabIndex = 1;
            this.EndButton.Text = "End";
            this.EndButton.UseVisualStyleBackColor = true;
            this.EndButton.Click += new System.EventHandler(this.EndButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(466, 121);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // AtFocus3Checkbox
            // 
            this.AtFocus3Checkbox.AutoSize = true;
            this.AtFocus3Checkbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AtFocus3Checkbox.Location = new System.Drawing.Point(147, 143);
            this.AtFocus3Checkbox.Name = "AtFocus3Checkbox";
            this.AtFocus3Checkbox.Size = new System.Drawing.Size(94, 17);
            this.AtFocus3Checkbox.TabIndex = 3;
            this.AtFocus3Checkbox.Text = "Use @Focus3";
            this.AtFocus3Checkbox.UseVisualStyleBackColor = true;
            // 
            // LoopsCounter
            // 
            this.LoopsCounter.Location = new System.Drawing.Point(99, 142);
            this.LoopsCounter.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LoopsCounter.Name = "LoopsCounter";
            this.LoopsCounter.Size = new System.Drawing.Size(39, 20);
            this.LoopsCounter.TabIndex = 4;
            this.LoopsCounter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.LoopsCounter.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Tracking Loops";
            // 
            // FormPECDataCollection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(488, 170);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LoopsCounter);
            this.Controls.Add(this.AtFocus3Checkbox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.EndButton);
            this.Controls.Add(this.StartButton);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Name = "FormPECDataCollection";
            this.Text = "PEC Collect";
            ((System.ComponentModel.ISupportInitialize)(this.LoopsCounter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button EndButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox AtFocus3Checkbox;
        private System.Windows.Forms.NumericUpDown LoopsCounter;
        private System.Windows.Forms.Label label1;
    }
}

