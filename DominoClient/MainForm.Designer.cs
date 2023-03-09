
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
            this.gameNumsButton = new System.Windows.Forms.Button();
            this.SendDominoesButton = new System.Windows.Forms.Button();
            this.MyDominoesLabel = new System.Windows.Forms.Label();
            this.firstDominoNum = new System.Windows.Forms.NumericUpDown();
            this.secondDominoNum = new System.Windows.Forms.NumericUpDown();
            this.placeDominoButton = new System.Windows.Forms.Button();
            this.leftCheckBox = new System.Windows.Forms.CheckBox();
            this.numsLabel = new System.Windows.Forms.Label();
            this.bazarPanel = new System.Windows.Forms.Panel();
            this.bazarButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.firstDominoNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondDominoNum)).BeginInit();
            this.bazarPanel.SuspendLayout();
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
            this.textBox1.Location = new System.Drawing.Point(424, 14);
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
            // gameNumsButton
            // 
            this.gameNumsButton.Enabled = false;
            this.gameNumsButton.Location = new System.Drawing.Point(33, 256);
            this.gameNumsButton.Name = "gameNumsButton";
            this.gameNumsButton.Size = new System.Drawing.Size(199, 29);
            this.gameNumsButton.TabIndex = 8;
            this.gameNumsButton.Text = "Крайние числа";
            this.gameNumsButton.UseVisualStyleBackColor = true;
            this.gameNumsButton.Click += new System.EventHandler(this.gameNumsButton_Click);
            // 
            // SendDominoesButton
            // 
            this.SendDominoesButton.Enabled = false;
            this.SendDominoesButton.Location = new System.Drawing.Point(33, 291);
            this.SendDominoesButton.Name = "SendDominoesButton";
            this.SendDominoesButton.Size = new System.Drawing.Size(199, 29);
            this.SendDominoesButton.TabIndex = 9;
            this.SendDominoesButton.Text = "Мои домино";
            this.SendDominoesButton.UseVisualStyleBackColor = true;
            this.SendDominoesButton.Click += new System.EventHandler(this.SendDominoesButton_Click);
            // 
            // MyDominoesLabel
            // 
            this.MyDominoesLabel.AutoSize = true;
            this.MyDominoesLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.MyDominoesLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.MyDominoesLabel.Location = new System.Drawing.Point(249, 295);
            this.MyDominoesLabel.Name = "MyDominoesLabel";
            this.MyDominoesLabel.Size = new System.Drawing.Size(0, 20);
            this.MyDominoesLabel.TabIndex = 10;
            // 
            // firstDominoNum
            // 
            this.firstDominoNum.Location = new System.Drawing.Point(33, 326);
            this.firstDominoNum.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.firstDominoNum.Name = "firstDominoNum";
            this.firstDominoNum.Size = new System.Drawing.Size(35, 27);
            this.firstDominoNum.TabIndex = 11;
            // 
            // secondDominoNum
            // 
            this.secondDominoNum.Location = new System.Drawing.Point(74, 326);
            this.secondDominoNum.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.secondDominoNum.Name = "secondDominoNum";
            this.secondDominoNum.Size = new System.Drawing.Size(35, 27);
            this.secondDominoNum.TabIndex = 12;
            // 
            // placeDominoButton
            // 
            this.placeDominoButton.Location = new System.Drawing.Point(33, 359);
            this.placeDominoButton.Name = "placeDominoButton";
            this.placeDominoButton.Size = new System.Drawing.Size(199, 29);
            this.placeDominoButton.TabIndex = 13;
            this.placeDominoButton.Text = "Положить домино";
            this.placeDominoButton.UseVisualStyleBackColor = true;
            this.placeDominoButton.Click += new System.EventHandler(this.placeDominoButton_Click);
            // 
            // leftCheckBox
            // 
            this.leftCheckBox.AutoSize = true;
            this.leftCheckBox.Enabled = false;
            this.leftCheckBox.Location = new System.Drawing.Point(122, 329);
            this.leftCheckBox.Name = "leftCheckBox";
            this.leftCheckBox.Size = new System.Drawing.Size(145, 24);
            this.leftCheckBox.TabIndex = 14;
            this.leftCheckBox.Text = "Положить слева";
            this.leftCheckBox.UseVisualStyleBackColor = true;
            // 
            // numsLabel
            // 
            this.numsLabel.AutoSize = true;
            this.numsLabel.Location = new System.Drawing.Point(249, 260);
            this.numsLabel.Name = "numsLabel";
            this.numsLabel.Size = new System.Drawing.Size(0, 20);
            this.numsLabel.TabIndex = 15;
            // 
            // bazarPanel
            // 
            this.bazarPanel.BackColor = System.Drawing.Color.OldLace;
            this.bazarPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bazarPanel.Controls.Add(this.bazarButton);
            this.bazarPanel.Controls.Add(this.label2);
            this.bazarPanel.Location = new System.Drawing.Point(258, 107);
            this.bazarPanel.Name = "bazarPanel";
            this.bazarPanel.Size = new System.Drawing.Size(250, 133);
            this.bazarPanel.TabIndex = 16;
            this.bazarPanel.Visible = false;
            // 
            // bazarButton
            // 
            this.bazarButton.Location = new System.Drawing.Point(64, 48);
            this.bazarButton.Name = "bazarButton";
            this.bazarButton.Size = new System.Drawing.Size(123, 29);
            this.bazarButton.TabIndex = 1;
            this.bazarButton.Text = "Взять домино";
            this.bazarButton.UseVisualStyleBackColor = true;
            this.bazarButton.Click += new System.EventHandler(this.bazarButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(99, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Базар";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bazarPanel);
            this.Controls.Add(this.numsLabel);
            this.Controls.Add(this.leftCheckBox);
            this.Controls.Add(this.placeDominoButton);
            this.Controls.Add(this.secondDominoNum);
            this.Controls.Add(this.firstDominoNum);
            this.Controls.Add(this.MyDominoesLabel);
            this.Controls.Add(this.SendDominoesButton);
            this.Controls.Add(this.gameNumsButton);
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
            ((System.ComponentModel.ISupportInitialize)(this.firstDominoNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondDominoNum)).EndInit();
            this.bazarPanel.ResumeLayout(false);
            this.bazarPanel.PerformLayout();
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
        private System.Windows.Forms.Button gameNumsButton;
        private System.Windows.Forms.Button SendDominoesButton;
        private System.Windows.Forms.Label MyDominoesLabel;
        private System.Windows.Forms.NumericUpDown firstDominoNum;
        private System.Windows.Forms.NumericUpDown secondDominoNum;
        private System.Windows.Forms.Button placeDominoButton;
        private System.Windows.Forms.CheckBox leftCheckBox;
        private System.Windows.Forms.Label numsLabel;
        private System.Windows.Forms.Panel bazarPanel;
        private System.Windows.Forms.Button bazarButton;
        private System.Windows.Forms.Label label2;
    }
}

