
namespace DominoClient
{
    partial class RegisterForm
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
            nameLabel = new System.Windows.Forms.Label();
            usernameTextBox = new System.Windows.Forms.TextBox();
            startButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            hostNameTB = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            passwordTextBox = new System.Windows.Forms.TextBox();
            registerButton = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(147, 133);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(282, 23);
            nameLabel.TabIndex = 23;
            nameLabel.Text = "Введите имя пользователя";
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new System.Drawing.Point(147, 159);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new System.Drawing.Size(299, 32);
            usernameTextBox.TabIndex = 22;
            usernameTextBox.Text = "haiduk_evgen";
            // 
            // startButton
            // 
            startButton.Location = new System.Drawing.Point(146, 262);
            startButton.Name = "startButton";
            startButton.Size = new System.Drawing.Size(300, 33);
            startButton.TabIndex = 20;
            startButton.Text = "Подключиться";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += StartButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(147, 65);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(299, 23);
            label1.TabIndex = 19;
            label1.Text = "Введите host name сервера";
            // 
            // hostNameTB
            // 
            hostNameTB.Location = new System.Drawing.Point(147, 91);
            hostNameTB.Name = "hostNameTB";
            hostNameTB.Size = new System.Drawing.Size(299, 32);
            hostNameTB.TabIndex = 18;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(147, 198);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(173, 23);
            label2.TabIndex = 26;
            label2.Text = "Введите пароль";
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new System.Drawing.Point(147, 224);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '●';
            passwordTextBox.Size = new System.Drawing.Size(299, 32);
            passwordTextBox.TabIndex = 25;
            passwordTextBox.Text = "password";
            // 
            // registerButton
            // 
            registerButton.Location = new System.Drawing.Point(146, 297);
            registerButton.Name = "registerButton";
            registerButton.Size = new System.Drawing.Size(300, 33);
            registerButton.TabIndex = 27;
            registerButton.Text = "Зарегистрироваться";
            registerButton.UseVisualStyleBackColor = true;
            registerButton.Click += RegisterButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.MenuBackground_0_8x;
            pictureBox1.Location = new System.Drawing.Point(-30, -2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(644, 390);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 28;
            pictureBox1.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Century Gothic", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label3.Location = new System.Drawing.Point(181, 18);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(241, 36);
            label3.TabIndex = 29;
            label3.Text = "Домино клиент";
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(572, 356);
            Controls.Add(label3);
            Controls.Add(registerButton);
            Controls.Add(label2);
            Controls.Add(passwordTextBox);
            Controls.Add(nameLabel);
            Controls.Add(usernameTextBox);
            Controls.Add(startButton);
            Controls.Add(label1);
            Controls.Add(hostNameTB);
            Controls.Add(pictureBox1);
            Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "RegisterForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Регистрация клиента домино";
            Load += RegisterForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox hostNameTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
    }
}