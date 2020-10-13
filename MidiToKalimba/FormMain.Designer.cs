namespace MidiToKalimba
{
    partial class FormMain
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
            this.btnConvert = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tbFilePath = new System.Windows.Forms.TextBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.lblTooHighNotes = new System.Windows.Forms.Label();
            this.lblTooLowNotes = new System.Windows.Forms.Label();
            this.lblUnplayableCounter = new System.Windows.Forms.Label();
            this.tbConvertedMidi = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudBaseOctave = new System.Windows.Forms.NumericUpDown();
            this.nudSpeed = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseOctave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConvert
            // 
            this.btnConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConvert.Location = new System.Drawing.Point(713, 600);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(138, 52);
            this.btnConvert.TabIndex = 0;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Midi files|*.mid";
            // 
            // tbFilePath
            // 
            this.tbFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilePath.Location = new System.Drawing.Point(61, 86);
            this.tbFilePath.Name = "tbFilePath";
            this.tbFilePath.Size = new System.Drawing.Size(640, 26);
            this.tbFilePath.TabIndex = 1;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFile.Location = new System.Drawing.Point(713, 73);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(138, 52);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // lblTooHighNotes
            // 
            this.lblTooHighNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTooHighNotes.AutoSize = true;
            this.lblTooHighNotes.Location = new System.Drawing.Point(13, 648);
            this.lblTooHighNotes.Name = "lblTooHighNotes";
            this.lblTooHighNotes.Size = new System.Drawing.Size(131, 20);
            this.lblTooHighNotes.TabIndex = 3;
            this.lblTooHighNotes.Text = "Too High Notes: /";
            // 
            // lblTooLowNotes
            // 
            this.lblTooLowNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTooLowNotes.AutoSize = true;
            this.lblTooLowNotes.Location = new System.Drawing.Point(13, 616);
            this.lblTooLowNotes.Name = "lblTooLowNotes";
            this.lblTooLowNotes.Size = new System.Drawing.Size(127, 20);
            this.lblTooLowNotes.TabIndex = 4;
            this.lblTooLowNotes.Text = "Too Low Notes: /";
            // 
            // lblUnplayableCounter
            // 
            this.lblUnplayableCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblUnplayableCounter.AutoSize = true;
            this.lblUnplayableCounter.Location = new System.Drawing.Point(13, 582);
            this.lblUnplayableCounter.Name = "lblUnplayableCounter";
            this.lblUnplayableCounter.Size = new System.Drawing.Size(146, 20);
            this.lblUnplayableCounter.TabIndex = 5;
            this.lblUnplayableCounter.Text = "Unplayable Notes: /";
            // 
            // tbConvertedMidi
            // 
            this.tbConvertedMidi.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConvertedMidi.Location = new System.Drawing.Point(12, 142);
            this.tbConvertedMidi.Multiline = true;
            this.tbConvertedMidi.Name = "tbConvertedMidi";
            this.tbConvertedMidi.Size = new System.Drawing.Size(838, 423);
            this.tbConvertedMidi.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(435, 632);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Base Octave: ";
            // 
            // nudBaseOctave
            // 
            this.nudBaseOctave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudBaseOctave.Location = new System.Drawing.Point(566, 630);
            this.nudBaseOctave.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudBaseOctave.Name = "nudBaseOctave";
            this.nudBaseOctave.Size = new System.Drawing.Size(120, 26);
            this.nudBaseOctave.TabIndex = 9;
            this.nudBaseOctave.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // nudSpeed
            // 
            this.nudSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudSpeed.DecimalPlaces = 2;
            this.nudSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudSpeed.Location = new System.Drawing.Point(566, 590);
            this.nudSpeed.Name = "nudSpeed";
            this.nudSpeed.Size = new System.Drawing.Size(120, 26);
            this.nudSpeed.TabIndex = 11;
            this.nudSpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(435, 592);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Playback Speed: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(644, 59);
            this.label4.TabIndex = 12;
            this.label4.Text = "Midi to Kalimba Converter";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Path:";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 680);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudSpeed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudBaseOctave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbConvertedMidi);
            this.Controls.Add(this.lblUnplayableCounter);
            this.Controls.Add(this.lblTooLowNotes);
            this.Controls.Add(this.lblTooHighNotes);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.tbFilePath);
            this.Controls.Add(this.btnConvert);
            this.Name = "FormMain";
            this.Text = "Midi to Kalimba Converter 0.1";
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseOctave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox tbFilePath;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Label lblTooHighNotes;
        private System.Windows.Forms.Label lblTooLowNotes;
        private System.Windows.Forms.Label lblUnplayableCounter;
        private System.Windows.Forms.TextBox tbConvertedMidi;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudBaseOctave;
        private System.Windows.Forms.NumericUpDown nudSpeed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

