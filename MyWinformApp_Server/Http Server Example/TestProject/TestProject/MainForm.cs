using System;
using System.Windows.Forms;

namespace TestProject
{
    /// <summary>
    /// 메인 폼
    /// </summary>
    public partial class MainForm : Form
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 서버
        /// </summary>
        private WebServer server = null;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - MainForm()

        /// <summary>
        /// 생성자
        /// </summary>
        public MainForm()
        {
            InitializeComponent();


            this.server = new WebServer();

            this.server.AddBindingAddress("http://localhost:9999/"); // 포트 번호까지 반드시 설정합니다.

            this.server.RootPath = "c:\\wwwroot";

            this.server.ActionRequested += server_ActionRequested;


            FormClosing += Form_FormClosing;

            this.startButton.Click += startButton_Click;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region 폼을 닫을려는 경우 처리하기 - Form_FormClosing(sender, e)

        /// <summary>
        /// 폼을 닫을려는 경우 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(this.server.IsRunning)
            {
                this.server.Stop();

                this.server = null;
            }
        }

        #endregion
        #region Start 버튼 클릭시 처리하기 - startButton_Click(sender, e)

        /// <summary>
        /// Start 버튼 클릭시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void startButton_Click(object sender, EventArgs e)
        {
            if(this.server.IsRunning)
            {
                this.server.Stop();

                this.startButton.Text = "Start";
            }
            else
            {
                this.server.Start();

                this.startButton.Text = "Stop";
            }
        }

        #endregion
        #region 서버 액션 요청시 처리하기 - server_ActionRequested(sender, e)

        /// <summary>
        /// 서버 액션 요청시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void server_ActionRequested(object sender, ActionRequestedEventArgs e)
        {
            e.Server.WriteDefaultAction(e.Context);
        }

        #endregion
    }
}