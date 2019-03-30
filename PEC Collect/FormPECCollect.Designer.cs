namespace PEC_Collect
{
    partial class FormPECCollect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPECCollect));
            this.StartButton = new System.Windows.Forms.Button();
            this.EndButton = new System.Windows.Forms.Button();
            this.OutputTextBox = new System.Windows.Forms.TextBox();
            this.LoopsCounter = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.DurationMinutes = new System.Windows.Forms.NumericUpDown();
            this.TimeLeft = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CompletionTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PACheckBox = new System.Windows.Forms.CheckBox();
            this.PauseCheckBox = new System.Windows.Forms.CheckBox();
            this.FocusComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.LoopsCounter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DurationMinutes)).BeginInit();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.ForeColor = System.Drawing.Color.Black;
            this.StartButton.Location = new System.Drawing.Point(415, 189);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(57, 23);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // EndButton
            // 
            this.EndButton.ForeColor = System.Drawing.Color.Black;
            this.EndButton.Location = new System.Drawing.Point(478, 189);
            this.EndButton.Name = "EndButton";
            this.EndButton.Size = new System.Drawing.Size(58, 23);
            this.EndButton.TabIndex = 1;
            this.EndButton.Text = "End";
            this.EndButton.UseVisualStyleBackColor = true;
            this.EndButton.Click += new System.EventHandler(this.EndButton_Click);
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.Location = new System.Drawing.Point(10, 36);
            this.OutputTextBox.Multiline = true;
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.Size = new System.Drawing.Size(526, 147);
            this.OutputTextBox.TabIndex = 2;
            this.OutputTextBox.Text = resources.GetString("OutputTextBox.Text");
            // 
            // LoopsCounter
            // 
            this.LoopsCounter.Location = new System.Drawing.Point(114, 10);
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
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Tracking Log Loops";
            // 
            // DurationMinutes
            // 
            this.DurationMinutes.Location = new System.Drawing.Point(240, 10);
            this.DurationMinutes.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.DurationMinutes.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.DurationMinutes.Name = "DurationMinutes";
            this.DurationMinutes.Size = new System.Drawing.Size(45, 20);
            this.DurationMinutes.TabIndex = 6;
            this.DurationMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DurationMinutes.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // TimeLeft
            // 
            this.TimeLeft.Location = new System.Drawing.Point(279, 192);
            this.TimeLeft.Name = "TimeLeft";
            this.TimeLeft.Size = new System.Drawing.Size(67, 20);
            this.TimeLeft.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(180, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Time Left on Loop";
            // 
            // CompletionTime
            // 
            this.CompletionTime.Location = new System.Drawing.Point(100, 191);
            this.CompletionTime.Name = "CompletionTime";
            this.CompletionTime.Size = new System.Drawing.Size(67, 20);
            this.CompletionTime.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Completion Time";
            // 
            // PACheckBox
            // 
            this.PACheckBox.AutoSize = true;
            this.PACheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PACheckBox.Checked = true;
            this.PACheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PACheckBox.Location = new System.Drawing.Point(490, 12);
            this.PACheckBox.Name = "PACheckBox";
            this.PACheckBox.Size = new System.Drawing.Size(40, 17);
            this.PACheckBox.TabIndex = 14;
            this.PACheckBox.Text = "PA";
            this.PACheckBox.UseVisualStyleBackColor = true;
            // 
            // PauseCheckBox
            // 
            this.PauseCheckBox.AutoSize = true;
            this.PauseCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PauseCheckBox.Location = new System.Drawing.Point(428, 12);
            this.PauseCheckBox.Name = "PauseCheckBox";
            this.PauseCheckBox.Size = new System.Drawing.Size(56, 17);
            this.PauseCheckBox.TabIndex = 15;
            this.PauseCheckBox.Text = "Pause";
            this.PauseCheckBox.UseVisualStyleBackColor = true;
            // 
            // FocusComboBox
            // 
            this.FocusComboBox.FormattingEnabled = true;
            this.FocusComboBox.Items.AddRange(new object[] {
            "@Focus2",
            "@Focus3",
            "None"});
            this.FocusComboBox.Location = new System.Drawing.Point(340, 10);
            this.FocusComboBox.Name = "FocusComboBox";
            this.FocusComboBox.Size = new System.Drawing.Size(82, 21);
            this.FocusComboBox.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(162, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Duration (min)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(298, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Focus";
            // 
            // FormPECCollect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(550, 219);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FocusComboBox);
            this.Controls.Add(this.PauseCheckBox);
            this.Controls.Add(this.PACheckBox);
            this.Controls.Add(this.CompletionTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TimeLeft);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DurationMinutes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LoopsCounter);
            this.Controls.Add(this.OutputTextBox);
            this.Controls.Add(this.EndButton);
            this.Controls.Add(this.StartButton);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Name = "FormPECCollect";
            this.Text = "PEC Collect";
            ((System.ComponentModel.ISupportInitialize)(this.LoopsCounter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DurationMinutes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button EndButton;
        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.NumericUpDown LoopsCounter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown DurationMinutes;
        private System.Windows.Forms.TextBox TimeLeft;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox CompletionTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox PACheckBox;
        private System.Windows.Forms.CheckBox PauseCheckBox;
        private System.Windows.Forms.ComboBox FocusComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
    }
}

