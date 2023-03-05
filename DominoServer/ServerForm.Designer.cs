
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
            this.displayTextBox = new System.Windows.Forms.TextBox();
            this.name1 = new System.Windows.Forms.Label();
            this.name2 = new System.Windows.Forms.Label();
            this.twoRB = new System.Windows.Forms.RadioButton();
            this.threeRB = new System.Windows.Forms.RadioButton();
            this.fourRB = new System.Windows.Forms.RadioButton();
            this.playersLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // displayTextBox
            // 
            this.displayTextBox.Location = new System.Drawing.Point(12, 56);
            this.displayTextBox.Multiline = true;
            this.displayTextBox.Name = "displayTextBox";
            this.displayTextBox.ReadOnly = true;
            this.displayTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.displayTextBox.Size = new System.Drawing.Size(734, 382);
            this.displayTextBox.TabIndex = 0;
            // 
            // name1
            // 
            this.name1.AutoSize = true;
            this.name1.Location = new System.Drawing.Point(127, 446);
            this.name1.Name = "name1";
            this.name1.Size = new System.Drawing.Size(0, 20);
            this.name1.TabIndex = 1;
            // 
            // name2
            // 
            this.name2.AutoSize = true;
            this.name2.Location = new System.Drawing.Point(575, 446);
            this.name2.Name = "name2";
            this.name2.Size = new System.Drawing.Size(0, 20);
            this.name2.TabIndex = 2;
            // 
            // twoRB
            // 
            this.twoRB.AutoSize = true;
            this.twoRB.Checked = true;
            this.twoRB.Location = new System.Drawing.Point(240, 10);
            this.twoRB.Name = "twoRB";
            this.twoRB.Size = new System.Drawing.Size(108, 24);
            this.twoRB.TabIndex = 3;
            this.twoRB.TabStop = true;
            this.twoRB.Text = "Два игрока";
            this.twoRB.UseVisualStyleBackColor = true;
            // 
            // threeRB
            // 
            this.threeRB.AutoSize = true;
            this.threeRB.Location = new System.Drawing.Point(354, 10);
            this.threeRB.Name = "threeRB";
            this.threeRB.Size = new System.Drawing.Size(108, 24);
            this.threeRB.TabIndex = 4;
            this.threeRB.Text = "Три игрока";
            this.threeRB.UseVisualStyleBackColor = true;
            // 
            // fourRB
            // 
            this.fourRB.AutoSize = true;
            this.fourRB.Location = new System.Drawing.Point(468, 10);
            this.fourRB.Name = "fourRB";
            this.fourRB.Size = new System.Drawing.Size(134, 24);
            this.fourRB.TabIndex = 5;
            this.fourRB.Text = "Четыре игрока";
            this.fourRB.UseVisualStyleBackColor = true;
            // 
            // playersLabel
            // 
            this.playersLabel.AutoSize = true;
            this.playersLabel.Location = new System.Drawing.Point(12, 14);
            this.playersLabel.Name = "playersLabel";
            this.playersLabel.Size = new System.Drawing.Size(222, 20);
            this.playersLabel.TabIndex = 6;
            this.playersLabel.Text = "Выберите количество игроков";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(608, 8);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(138, 29);
            this.startButton.TabIndex = 7;
            this.startButton.Text = "Начать";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(758, 470);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.playersLabel);
            this.Controls.Add(this.fourRB);
            this.Controls.Add(this.threeRB);
            this.Controls.Add(this.twoRB);
            this.Controls.Add(this.name2);
            this.Controls.Add(this.name1);
            this.Controls.Add(this.displayTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ServerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Domino-Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

