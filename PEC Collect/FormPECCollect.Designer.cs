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
            this.BinningListBox = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.BinSwapCheckbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.LoopsCounter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DurationMinutes)).BeginInit();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.ForeColor = System.Drawing.Color.Black;
            this.StartButton.Location = new System.Drawing.Point(1064, 363);
            this.StartButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(114, 38);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // EndButton
            // 
            this.EndButton.ForeColor = System.Drawing.Color.Black;
            this.EndButton.Location = new System.Drawing.Point(1190, 363);
            this.EndButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.EndButton.Name = "EndButton";
            this.EndButton.Size = new System.Drawing.Size(116, 38);
            this.EndButton.TabIndex = 1;
            this.EndButton.Text = "End";
            this.EndButton.UseVisualStyleBackColor = true;
            this.EndButton.Click += new System.EventHandler(this.EndButton_Click);
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.Location = new System.Drawing.Point(24, 65);
            this.OutputTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.OutputTextBox.Multiline = true;
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.Size = new System.Drawing.Size(1278, 279);
            this.OutputTextBox.TabIndex = 2;
            this.OutputTextBox.Text = resources.GetString("OutputTextBox.Text");
            // 
            // LoopsCounter
            // 
            this.LoopsCounter.Location = new System.Drawing.Point(228, 19);
            this.LoopsCounter.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.LoopsCounter.Name = "LoopsCounter";
            this.LoopsCounter.Size = new System.Drawing.Size(78, 31);
            this.LoopsCounter.TabIndex = 4;
            this.LoopsCounter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.LoopsCounter.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Tracking Log Loops";
            // 
            // DurationMinutes
            // 
            this.DurationMinutes.Location = new System.Drawing.Point(480, 19);
            this.DurationMinutes.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.DurationMinutes.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.DurationMinutes.Name = "DurationMinutes";
            this.DurationMinutes.Size = new System.Drawing.Size(90, 31);
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
            this.TimeLeft.Location = new System.Drawing.Point(558, 369);
            this.TimeLeft.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.TimeLeft.Name = "TimeLeft";
            this.TimeLeft.Size = new System.Drawing.Size(130, 31);
            this.TimeLeft.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(360, 373);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 25);
            this.label4.TabIndex = 10;
            this.label4.Text = "Time Left on Loop";
            // 
            // CompletionTime
            // 
            this.CompletionTime.Location = new System.Drawing.Point(200, 367);
            this.CompletionTime.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.CompletionTime.Name = "CompletionTime";
            this.CompletionTime.Size = new System.Drawing.Size(130, 31);
            this.CompletionTime.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 371);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "Completion Time";
            // 
            // PACheckBox
            // 
            this.PACheckBox.AutoSize = true;
            this.PACheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PACheckBox.Checked = true;
            this.PACheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PACheckBox.Location = new System.Drawing.Point(980, 23);
            this.PACheckBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.PACheckBox.Name = "PACheckBox";
            this.PACheckBox.Size = new System.Drawing.Size(72, 29);
            this.PACheckBox.TabIndex = 14;
            this.PACheckBox.Text = "PA";
            this.PACheckBox.UseVisualStyleBackColor = true;
            // 
            // PauseCheckBox
            // 
            this.PauseCheckBox.AutoSize = true;
            this.PauseCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PauseCheckBox.Location = new System.Drawing.Point(848, 21);
            this.PauseCheckBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.PauseCheckBox.Name = "PauseCheckBox";
            this.PauseCheckBox.Size = new System.Drawing.Size(105, 29);
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
            this.FocusComboBox.Location = new System.Drawing.Point(672, 17);
            this.FocusComboBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.FocusComboBox.Name = "FocusComboBox";
            this.FocusComboBox.Size = new System.Drawing.Size(160, 33);
            this.FocusComboBox.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(324, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 25);
            this.label2.TabIndex = 17;
            this.label2.Text = "Duration (min)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(588, 25);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 25);
            this.label5.TabIndex = 18;
            this.label5.Text = "Focus";
            // 
            // BinningListBox
            // 
            this.BinningListBox.FormattingEnabled = true;
            this.BinningListBox.ItemHeight = 25;
            this.BinningListBox.Items.AddRange(new object[] {
            "1x1",
            "2x2",
            "3x3",
            "4x4"});
            this.BinningListBox.Location = new System.Drawing.Point(1214, 23);
            this.BinningListBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.BinningListBox.Name = "BinningListBox";
            this.BinningListBox.ScrollAlwaysVisible = true;
            this.BinningListBox.Size = new System.Drawing.Size(88, 29);
            this.BinningListBox.TabIndex = 19;
            this.BinningListBox.SelectedIndexChanged += new System.EventHandler(this.BinningListBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1118, 27);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 25);
            this.label6.TabIndex = 20;
            this.label6.Text = "Binning";
            // 
            // BinSwapCheckbox
            // 
            this.BinSwapCheckbox.AutoSize = true;
            this.BinSwapCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BinSwapCheckbox.Location = new System.Drawing.Point(768, 369);
            this.BinSwapCheckbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.BinSwapCheckbox.Name = "BinSwapCheckbox";
            this.BinSwapCheckbox.Size = new System.Drawing.Size(208, 29);
            this.BinSwapCheckbox.TabIndex = 21;
            this.BinSwapCheckbox.Text = "Alternate Binning";
            this.BinSwapCheckbox.UseVisualStyleBackColor = true;
            // 
            // FormPECCollect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(1330, 421);
            this.Controls.Add(this.BinSwapCheckbox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.BinningListBox);
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
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
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
        private System.Windows.Forms.ListBox BinningListBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox BinSwapCheckbox;
    }
}

