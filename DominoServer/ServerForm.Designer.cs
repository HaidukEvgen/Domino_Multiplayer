
namespace DominoServer
{
    partial class ServerForm
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
            displayTextBox = new System.Windows.Forms.TextBox();
            name1 = new System.Windows.Forms.Label();
            name2 = new System.Windows.Forms.Label();
            twoRB = new System.Windows.Forms.RadioButton();
            threeRB = new System.Windows.Forms.RadioButton();
            fourRB = new System.Windows.Forms.RadioButton();
            playersLabel = new System.Windows.Forms.Label();
            startButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            pointsAimUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)pointsAimUpDown).BeginInit();
            SuspendLayout();
            // 
            // displayTextBox
            // 
            displayTextBox.Location = new System.Drawing.Point(12, 76);
            displayTextBox.Multiline = true;
            displayTextBox.Name = "displayTextBox";
            displayTextBox.ReadOnly = true;
            displayTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            displayTextBox.Size = new System.Drawing.Size(734, 382);
            displayTextBox.TabIndex = 0;
            // 
            // name1
            // 
            name1.AutoSize = true;
            name1.Location = new System.Drawing.Point(127, 446);
            name1.Name = "name1";
            name1.Size = new System.Drawing.Size(0, 18);
            name1.TabIndex = 1;
            // 
            // name2
            // 
            name2.AutoSize = true;
            name2.Location = new System.Drawing.Point(575, 446);
            name2.Name = "name2";
            name2.Size = new System.Drawing.Size(0, 18);
            name2.TabIndex = 2;
            // 
            // twoRB
            // 
            twoRB.AutoSize = true;
            twoRB.Checked = true;
            twoRB.Location = new System.Drawing.Point(256, 10);
            twoRB.Name = "twoRB";
            twoRB.Size = new System.Drawing.Size(116, 22);
            twoRB.TabIndex = 3;
            twoRB.TabStop = true;
            twoRB.Text = "Два игрока";
            twoRB.UseVisualStyleBackColor = true;
            // 
            // threeRB
            // 
            threeRB.AutoSize = true;
            threeRB.Location = new System.Drawing.Point(371, 11);
            threeRB.Name = "threeRB";
            threeRB.Size = new System.Drawing.Size(111, 22);
            threeRB.TabIndex = 4;
            threeRB.Text = "Три игрока";
            threeRB.UseVisualStyleBackColor = true;
            // 
            // fourRB
            // 
            fourRB.AutoSize = true;
            fourRB.Location = new System.Drawing.Point(481, 12);
            fourRB.Name = "fourRB";
            fourRB.Size = new System.Drawing.Size(145, 22);
            fourRB.TabIndex = 5;
            fourRB.Text = "Четыре игрока";
            fourRB.UseVisualStyleBackColor = true;
            // 
            // playersLabel
            // 
            playersLabel.AutoSize = true;
            playersLabel.Location = new System.Drawing.Point(12, 12);
            playersLabel.Name = "playersLabel";
            playersLabel.Size = new System.Drawing.Size(244, 18);
            playersLabel.TabIndex = 6;
            playersLabel.Text = "Выберите количество игроков";
            // 
            // startButton
            // 
            startButton.Location = new System.Drawing.Point(626, 8);
            startButton.Name = "startButton";
            startButton.Size = new System.Drawing.Size(120, 29);
            startButton.TabIndex = 7;
            startButton.Text = "Начать";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += StartButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 43);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(174, 18);
            label1.TabIndex = 8;
            label1.Text = "Выберите цель очков";
            // 
            // pointsAimUpDown
            // 
            pointsAimUpDown.Location = new System.Drawing.Point(188, 41);
            pointsAimUpDown.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            pointsAimUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            pointsAimUpDown.Name = "pointsAimUpDown";
            pointsAimUpDown.Size = new System.Drawing.Size(62, 26);
            pointsAimUpDown.TabIndex = 9;
            pointsAimUpDown.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.Color.DarkGray;
            ClientSize = new System.Drawing.Size(758, 470);
            Controls.Add(pointsAimUpDown);
            Controls.Add(label1);
            Controls.Add(startButton);
            Controls.Add(playersLabel);
            Controls.Add(fourRB);
            Controls.Add(threeRB);
            Controls.Add(twoRB);
            Controls.Add(name2);
            Controls.Add(name1);
            Controls.Add(displayTextBox);
            Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "ServerForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Domino-Server";
            FormClosing += ServerForm_FormClosing;
            Load += ServerForm_Load;
            ((System.ComponentModel.ISupportInitialize)pointsAimUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox displayTextBox;
        private System.Windows.Forms.Label name1;
        private System.Windows.Forms.Label name2;
        private System.Windows.Forms.RadioButton twoRB;
        private System.Windows.Forms.RadioButton threeRB;
        private System.Windows.Forms.RadioButton fourRB;
        private System.Windows.Forms.Label playersLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown pointsAimUpDown;
    }
}

