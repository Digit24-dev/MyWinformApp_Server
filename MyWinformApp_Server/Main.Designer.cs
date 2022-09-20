
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
            this.Button_Start = new System.Windows.Forms.Button();
            this.Button_Stop = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.TextBox_Status = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ListBox_Users = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Button_Exit = new System.Windows.Forms.Button();
            this.textBox_UserCount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Button_Start
            // 
            this.Button_Start.Location = new System.Drawing.Point(12, 48);
            this.Button_Start.Name = "Button_Start";
            this.Button_Start.Size = new System.Drawing.Size(75, 23);
            this.Button_Start.TabIndex = 0;
            this.Button_Start.Text = "Start";
            this.Button_Start.UseVisualStyleBackColor = true;
            this.Button_Start.Click += new System.EventHandler(this.Button_Start_Click);
            // 
            // Button_Stop
            // 
            this.Button_Stop.Location = new System.Drawing.Point(93, 48);
            this.Button_Stop.Name = "Button_Stop";
            this.Button_Stop.Size = new System.Drawing.Size(75, 24);
            this.Button_Stop.TabIndex = 1;
            this.Button_Stop.Text = "Stop";
            this.Button_Stop.UseVisualStyleBackColor = true;
            this.Button_Stop.Click += new System.EventHandler(this.Button_Stop_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 114);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(434, 324);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // TextBox_Status
            // 
            this.TextBox_Status.Location = new System.Drawing.Point(12, 21);
            this.TextBox_Status.Name = "TextBox_Status";
            this.TextBox_Status.Size = new System.Drawing.Size(156, 21);
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
            this.ListBox_Users.Location = new System.Drawing.Point(488, 45);
            this.ListBox_Users.Name = "ListBox_Users";
            this.ListBox_Users.Size = new System.Drawing.Size(111, 388);
            this.ListBox_Users.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(486, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Users";
            // 
            // Button_Exit
            // 
            this.Button_Exit.Location = new System.Drawing.Point(174, 48);
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(75, 24);
            this.Button_Exit.TabIndex = 8;
            this.Button_Exit.Text = "Exit";
            this.Button_Exit.UseVisualStyleBackColor = true;
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_Click);
            // 
            // textBox_UserCount
            // 
            this.textBox_UserCount.Location = new System.Drawing.Point(346, 21);
            this.textBox_UserCount.Name = "textBox_UserCount";
            this.textBox_UserCount.ReadOnly = true;
            this.textBox_UserCount.Size = new System.Drawing.Size(100, 21);
            this.textBox_UserCount.TabIndex = 9;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 450);
            this.Controls.Add(this.textBox_UserCount);
            this.Controls.Add(this.Button_Exit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ListBox_Users);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBox_Status);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Button_Stop);
            this.Controls.Add(this.Button_Start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Main";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Start;
        private System.Windows.Forms.Button Button_Stop;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox TextBox_Status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox ListBox_Users;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Button_Exit;
        private System.Windows.Forms.TextBox textBox_UserCount;
    }
}

