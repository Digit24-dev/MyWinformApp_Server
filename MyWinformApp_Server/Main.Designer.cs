﻿
namespace MyWinformApp_Server
{
    partial class Main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.Button_UpdateUserCount = new System.Windows.Forms.Button();
            this.Button_DB_test = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.TextBox_Status = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ListBox_Users = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Button_Exit = new System.Windows.Forms.Button();
            this.textBox_UserCount = new System.Windows.Forms.TextBox();
            this.label = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Button_UpdateUserCount
            // 
            this.Button_UpdateUserCount.Location = new System.Drawing.Point(543, 12);
            this.Button_UpdateUserCount.Name = "Button_UpdateUserCount";
            this.Button_UpdateUserCount.Size = new System.Drawing.Size(100, 23);
            this.Button_UpdateUserCount.TabIndex = 0;
            this.Button_UpdateUserCount.Text = "Update Counts";
            this.Button_UpdateUserCount.UseVisualStyleBackColor = true;
            this.Button_UpdateUserCount.Click += new System.EventHandler(this.Button_Start_Click);
            // 
            // Button_DB_test
            // 
            this.Button_DB_test.Location = new System.Drawing.Point(93, 87);
            this.Button_DB_test.Name = "Button_DB_test";
            this.Button_DB_test.Size = new System.Drawing.Size(75, 24);
            this.Button_DB_test.TabIndex = 1;
            this.Button_DB_test.Text = "DB Test";
            this.Button_DB_test.UseVisualStyleBackColor = true;
            this.Button_DB_test.Click += new System.EventHandler(this.Button_Stop_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 114);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(631, 324);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // TextBox_Status
            // 
            this.TextBox_Status.Location = new System.Drawing.Point(12, 21);
            this.TextBox_Status.Name = "TextBox_Status";
            this.TextBox_Status.Size = new System.Drawing.Size(318, 21);
            this.TextBox_Status.TabIndex = 3;
            this.TextBox_Status.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox_Status_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Log";
            // 
            // ListBox_Users
            // 
            this.ListBox_Users.FormattingEnabled = true;
            this.ListBox_Users.ItemHeight = 12;
            this.ListBox_Users.Location = new System.Drawing.Point(661, 41);
            this.ListBox_Users.Name = "ListBox_Users";
            this.ListBox_Users.Size = new System.Drawing.Size(111, 388);
            this.ListBox_Users.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(659, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Users";
            // 
            // Button_Exit
            // 
            this.Button_Exit.Location = new System.Drawing.Point(255, 87);
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(75, 24);
            this.Button_Exit.TabIndex = 8;
            this.Button_Exit.Text = "Exit";
            this.Button_Exit.UseVisualStyleBackColor = true;
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_Click);
            // 
            // textBox_UserCount
            // 
            this.textBox_UserCount.Location = new System.Drawing.Point(543, 41);
            this.textBox_UserCount.Name = "textBox_UserCount";
            this.textBox_UserCount.ReadOnly = true;
            this.textBox_UserCount.Size = new System.Drawing.Size(100, 21);
            this.textBox_UserCount.TabIndex = 9;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(469, 45);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(68, 12);
            this.label.TabIndex = 10;
            this.label.Text = "User Count";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(174, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 441);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label);
            this.Controls.Add(this.textBox_UserCount);
            this.Controls.Add(this.Button_Exit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ListBox_Users);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBox_Status);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Button_DB_test);
            this.Controls.Add(this.Button_UpdateUserCount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Main";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_UpdateUserCount;
        private System.Windows.Forms.Button Button_DB_test;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox TextBox_Status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox ListBox_Users;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Button_Exit;
        private System.Windows.Forms.TextBox textBox_UserCount;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Button button1;
    }
}

