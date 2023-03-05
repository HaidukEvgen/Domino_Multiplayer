
namespace DominoClient
{
    partial class MainForm
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
            this.hostNameTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.connectionDoneLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.makeTurnButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // hostNameTB
            // 
            this.hostNameTB.Location = new System.Drawing.Point(33, 91);
            this.hostNameTB.Name = "hostNameTB";
            this.hostNameTB.Size = new System.Drawing.Size(199, 27);
            this.hostNameTB.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Введите host name сервера";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(33, 124);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(199, 29);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Подключиться";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(359, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(199, 27);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "DESKTOP-GLBQFFP";
            // 
            // connectionDoneLabel
            // 
            this.connectionDoneLabel.AutoSize = true;
            this.connectionDoneLabel.BackColor = System.Drawing.SystemColors.Desktop;
            this.connectionDoneLabel.ForeColor = System.Drawing.Color.Lime;
            this.connectionDoneLabel.Location = new System.Drawing.Point(33, 156);
            this.connectionDoneLabel.Name = "connectionDoneLabel";
            this.connectionDoneLabel.Size = new System.Drawing.Size(190, 20);
            this.connectionDoneLabel.TabIndex = 4;
            this.connectionDoneLabel.Text = "Подключение выполнено";
            this.connectionDoneLabel.Visible = false;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(33, 14);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(137, 20);
            this.nameLabel.TabIndex = 6;
            this.nameLabel.Text = "Введите ваше имя";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(33, 37);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.PlaceholderText = "Игрок ";
            this.nameTextBox.Size = new System.Drawing.Size(199, 27);
            this.nameTextBox.TabIndex = 5;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.statusLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.statusLabel.Location = new System.Drawing.Point(33, 220);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(193, 20);
            this.statusLabel.TabIndex = 7;
            this.statusLabel.Text = "Ожидается подключение...";
            // 
            // makeTurnButton
            // 
            this.makeTurnButton.Enabled = false;
            this.makeTurnButton.Location = new System.Drawing.Point(33, 256);
            this.makeTurnButton.Name = "makeTurnButton";
            this.makeTurnButton.Size = new System.Drawing.Size(199, 29);
            this.makeTurnButton.TabIndex = 8;
            this.makeTurnButton.Text = "Походить";
            this.makeTurnButton.UseVisualStyleBackColor = true;
            this.makeTurnButton.Click += new System.EventHandler(this.makeTurnButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.makeTurnButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.connectionDoneLabel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hostNameTB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Domino-Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox hostNameTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label connectionDoneLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button makeTurnButton;
    }
}

