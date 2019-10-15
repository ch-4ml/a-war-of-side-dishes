namespace chat2
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tsBar = new System.Windows.Forms.ToolStrip();
            this.tsddbtnOption = new System.Windows.Forms.ToolStripDropDownButton();
            this.설정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.닫기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtnConn = new System.Windows.Forms.ToolStripButton();
            this.tsbtnDisconn = new System.Windows.Forms.ToolStripButton();
            this.tsddbtnHelp = new System.Windows.Forms.ToolStripButton();
            this.ssBar = new System.Windows.Forms.StatusStrip();
            this.tsslblTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.rtbText = new System.Windows.Forms.RichTextBox();
            this.plOption = new System.Windows.Forms.Panel();
            this.clientRbtn = new System.Windows.Forms.RadioButton();
            this.serverRbtn = new System.Windows.Forms.RadioButton();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtId = new System.Windows.Forms.TextBox();
            this.plGroup = new System.Windows.Forms.Panel();
            this.btnSend = new System.Windows.Forms.Button();
            this.bmCbx = new System.Windows.Forms.CheckBox();
            this.plMessage = new System.Windows.Forms.Panel();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.plBoard = new chat2.Form1.DoubleBufferPanel();
            this.btnRname = new System.Windows.Forms.Button();
            this.btnBname = new System.Windows.Forms.Button();
            this.btnReset_Reasoning = new System.Windows.Forms.Button();
            this.btnReady_Start_Set = new System.Windows.Forms.Button();
            this.pbRmarker = new System.Windows.Forms.PictureBox();
            this.pbBmarker = new System.Windows.Forms.PictureBox();
            this.pbBwin = new System.Windows.Forms.PictureBox();
            this.pbRwin = new System.Windows.Forms.PictureBox();
            this.tsBar.SuspendLayout();
            this.ssBar.SuspendLayout();
            this.plOption.SuspendLayout();
            this.plGroup.SuspendLayout();
            this.plMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRmarker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBmarker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBwin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRwin)).BeginInit();
            this.SuspendLayout();
            // 
            // tsBar
            // 
            this.tsBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddbtnOption,
            this.tsbtnConn,
            this.tsbtnDisconn,
            this.tsddbtnHelp});
            this.tsBar.Location = new System.Drawing.Point(0, 0);
            this.tsBar.Name = "tsBar";
            this.tsBar.Size = new System.Drawing.Size(1445, 25);
            this.tsBar.TabIndex = 0;
            this.tsBar.Text = "toolStrip1";
            this.tsBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsBar_ItemClicked);
            // 
            // tsddbtnOption
            // 
            this.tsddbtnOption.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsddbtnOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.설정ToolStripMenuItem,
            this.닫기ToolStripMenuItem});
            this.tsddbtnOption.Image = ((System.Drawing.Image)(resources.GetObject("tsddbtnOption.Image")));
            this.tsddbtnOption.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbtnOption.Name = "tsddbtnOption";
            this.tsddbtnOption.Size = new System.Drawing.Size(29, 22);
            this.tsddbtnOption.Text = "1:1 환경설정";
            // 
            // 설정ToolStripMenuItem
            // 
            this.설정ToolStripMenuItem.Name = "설정ToolStripMenuItem";
            this.설정ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.설정ToolStripMenuItem.Text = "설 정";
            this.설정ToolStripMenuItem.Click += new System.EventHandler(this.설정ToolStripMenuItem_Click);
            // 
            // 닫기ToolStripMenuItem
            // 
            this.닫기ToolStripMenuItem.Name = "닫기ToolStripMenuItem";
            this.닫기ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.닫기ToolStripMenuItem.Text = "닫 기";
            // 
            // tsbtnConn
            // 
            this.tsbtnConn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnConn.Enabled = false;
            this.tsbtnConn.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnConn.Image")));
            this.tsbtnConn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnConn.Name = "tsbtnConn";
            this.tsbtnConn.Size = new System.Drawing.Size(23, 22);
            this.tsbtnConn.Text = "연결";
            this.tsbtnConn.Click += new System.EventHandler(this.tsbtnConn_Click);
            // 
            // tsbtnDisconn
            // 
            this.tsbtnDisconn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnDisconn.Enabled = false;
            this.tsbtnDisconn.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnDisconn.Image")));
            this.tsbtnDisconn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnDisconn.Name = "tsbtnDisconn";
            this.tsbtnDisconn.Size = new System.Drawing.Size(23, 22);
            this.tsbtnDisconn.Text = "끊기";
            this.tsbtnDisconn.Click += new System.EventHandler(this.tsbtnDisconn_Click);
            // 
            // tsddbtnHelp
            // 
            this.tsddbtnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsddbtnHelp.Image = ((System.Drawing.Image)(resources.GetObject("tsddbtnHelp.Image")));
            this.tsddbtnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbtnHelp.Name = "tsddbtnHelp";
            this.tsddbtnHelp.Size = new System.Drawing.Size(23, 22);
            this.tsddbtnHelp.Text = "도움말";
            this.tsddbtnHelp.Click += new System.EventHandler(this.tsddbtnHelp_Click);
            // 
            // ssBar
            // 
            this.ssBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslblTime});
            this.ssBar.Location = new System.Drawing.Point(0, 879);
            this.ssBar.Name = "ssBar";
            this.ssBar.Size = new System.Drawing.Size(1445, 22);
            this.ssBar.TabIndex = 1;
            this.ssBar.Text = "statusStrip1";
            // 
            // tsslblTime
            // 
            this.tsslblTime.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tsslblTime.Name = "tsslblTime";
            this.tsslblTime.Size = new System.Drawing.Size(99, 17);
            this.tsslblTime.Text = "메시지 받은 시간";
            // 
            // rtbText
            // 
            this.rtbText.BackColor = System.Drawing.Color.White;
            this.rtbText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbText.Font = new System.Drawing.Font("굴림", 9F);
            this.rtbText.ForeColor = System.Drawing.Color.Pink;
            this.rtbText.Location = new System.Drawing.Point(1069, 155);
            this.rtbText.Name = "rtbText";
            this.rtbText.ReadOnly = true;
            this.rtbText.Size = new System.Drawing.Size(369, 532);
            this.rtbText.TabIndex = 2;
            this.rtbText.TabStop = false;
            this.rtbText.Text = "";
            this.rtbText.TextChanged += new System.EventHandler(this.rtbText_TextChanged);
            this.rtbText.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.rtbText_ControlAdded);
            // 
            // plOption
            // 
            this.plOption.BackColor = System.Drawing.Color.AliceBlue;
            this.plOption.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("plOption.BackgroundImage")));
            this.plOption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plOption.Controls.Add(this.clientRbtn);
            this.plOption.Controls.Add(this.serverRbtn);
            this.plOption.Controls.Add(this.txtIp);
            this.plOption.Controls.Add(this.btnClose);
            this.plOption.Controls.Add(this.btnSave);
            this.plOption.Controls.Add(this.txtPort);
            this.plOption.Controls.Add(this.txtId);
            this.plOption.Font = new System.Drawing.Font("굴림", 15F);
            this.plOption.Location = new System.Drawing.Point(0, 25);
            this.plOption.Name = "plOption";
            this.plOption.Size = new System.Drawing.Size(1445, 873);
            this.plOption.TabIndex = 3;
            this.plOption.Visible = false;
            this.plOption.Paint += new System.Windows.Forms.PaintEventHandler(this.plOption_Paint);
            // 
            // clientRbtn
            // 
            this.clientRbtn.AutoSize = true;
            this.clientRbtn.BackColor = System.Drawing.Color.Goldenrod;
            this.clientRbtn.Checked = true;
            this.clientRbtn.Font = new System.Drawing.Font("굴림", 12F);
            this.clientRbtn.ForeColor = System.Drawing.SystemColors.Control;
            this.clientRbtn.Location = new System.Drawing.Point(1293, 316);
            this.clientRbtn.Name = "clientRbtn";
            this.clientRbtn.Size = new System.Drawing.Size(106, 20);
            this.clientRbtn.TabIndex = 14;
            this.clientRbtn.TabStop = true;
            this.clientRbtn.Text = "클라이언트";
            this.clientRbtn.UseVisualStyleBackColor = false;
            this.clientRbtn.CheckedChanged += new System.EventHandler(this.clientRbtn_CheckedChanged);
            // 
            // serverRbtn
            // 
            this.serverRbtn.AutoSize = true;
            this.serverRbtn.BackColor = System.Drawing.Color.Goldenrod;
            this.serverRbtn.Font = new System.Drawing.Font("굴림", 12F);
            this.serverRbtn.ForeColor = System.Drawing.SystemColors.Control;
            this.serverRbtn.Location = new System.Drawing.Point(1211, 316);
            this.serverRbtn.Name = "serverRbtn";
            this.serverRbtn.Size = new System.Drawing.Size(58, 20);
            this.serverRbtn.TabIndex = 13;
            this.serverRbtn.Text = "서버";
            this.serverRbtn.UseVisualStyleBackColor = false;
            this.serverRbtn.CheckedChanged += new System.EventHandler(this.serverRbtn_CheckedChanged);
            // 
            // txtIp
            // 
            this.txtIp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIp.ForeColor = System.Drawing.SystemColors.Desktop;
            this.txtIp.Location = new System.Drawing.Point(1199, 367);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(211, 30);
            this.txtIp.TabIndex = 1;
            this.txtIp.Text = "192.168.0.25";
            this.txtIp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(1321, 531);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(74, 40);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "닫  기";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(1211, 531);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(74, 40);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "시  작";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtPort
            // 
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPort.Location = new System.Drawing.Point(1199, 467);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(211, 30);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "1234";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPort.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // txtId
            // 
            this.txtId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtId.Location = new System.Drawing.Point(1199, 416);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(211, 30);
            this.txtId.TabIndex = 2;
            this.txtId.Text = "닉네임";
            this.txtId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // plGroup
            // 
            this.plGroup.BackColor = System.Drawing.Color.AntiqueWhite;
            this.plGroup.Controls.Add(this.btnSend);
            this.plGroup.Controls.Add(this.bmCbx);
            this.plGroup.Controls.Add(this.plMessage);
            this.plGroup.Location = new System.Drawing.Point(1069, 688);
            this.plGroup.Name = "plGroup";
            this.plGroup.Size = new System.Drawing.Size(369, 59);
            this.plGroup.TabIndex = 4;
            this.plGroup.Paint += new System.Windows.Forms.PaintEventHandler(this.plGroup_Paint);
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.White;
            this.btnSend.Enabled = false;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("굴림", 9F);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(264, 9);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(78, 42);
            this.btnSend.TabIndex = 1;
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // bmCbx
            // 
            this.bmCbx.AutoSize = true;
            this.bmCbx.Location = new System.Drawing.Point(348, 36);
            this.bmCbx.Name = "bmCbx";
            this.bmCbx.Size = new System.Drawing.Size(15, 14);
            this.bmCbx.TabIndex = 10;
            this.bmCbx.UseVisualStyleBackColor = true;
            // 
            // plMessage
            // 
            this.plMessage.BackColor = System.Drawing.Color.White;
            this.plMessage.Controls.Add(this.txtMessage);
            this.plMessage.Location = new System.Drawing.Point(11, 9);
            this.plMessage.Name = "plMessage";
            this.plMessage.Size = new System.Drawing.Size(247, 42);
            this.plMessage.TabIndex = 0;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.White;
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMessage.Enabled = false;
            this.txtMessage.Font = new System.Drawing.Font("굴림", 9F);
            this.txtMessage.Location = new System.Drawing.Point(9, 14);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(229, 14);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.TextChanged += new System.EventHandler(this.txtMessage_TextChanged);
            this.txtMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMessage_KeyPress);
            // 
            // plBoard
            // 
            this.plBoard.BackColor = System.Drawing.Color.White;
            this.plBoard.Location = new System.Drawing.Point(7, 32);
            this.plBoard.Name = "plBoard";
            this.plBoard.Size = new System.Drawing.Size(840, 840);
            this.plBoard.TabIndex = 5;
            this.plBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plBoard_MouseDown);
            this.plBoard.MouseEnter += new System.EventHandler(this.plBoard_MouseEnter);
            this.plBoard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.plBoard_MouseMove);
            // 
            // btnRname
            // 
            this.btnRname.BackColor = System.Drawing.Color.MistyRose;
            this.btnRname.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRname.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRname.ForeColor = System.Drawing.Color.White;
            this.btnRname.Location = new System.Drawing.Point(1069, 30);
            this.btnRname.Name = "btnRname";
            this.btnRname.Size = new System.Drawing.Size(243, 120);
            this.btnRname.TabIndex = 7;
            this.btnRname.UseVisualStyleBackColor = false;
            // 
            // btnBname
            // 
            this.btnBname.BackColor = System.Drawing.Color.PowderBlue;
            this.btnBname.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBname.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBname.ForeColor = System.Drawing.Color.White;
            this.btnBname.Location = new System.Drawing.Point(1195, 753);
            this.btnBname.Name = "btnBname";
            this.btnBname.Size = new System.Drawing.Size(243, 120);
            this.btnBname.TabIndex = 7;
            this.btnBname.UseVisualStyleBackColor = false;
            // 
            // btnReset_Reasoning
            // 
            this.btnReset_Reasoning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset_Reasoning.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReset_Reasoning.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnReset_Reasoning.Location = new System.Drawing.Point(853, 274);
            this.btnReset_Reasoning.Name = "btnReset_Reasoning";
            this.btnReset_Reasoning.Size = new System.Drawing.Size(210, 175);
            this.btnReset_Reasoning.TabIndex = 11;
            this.btnReset_Reasoning.UseVisualStyleBackColor = true;
            this.btnReset_Reasoning.Click += new System.EventHandler(this.btnReset_Reasoning_Click);
            // 
            // btnReady_Start_Set
            // 
            this.btnReady_Start_Set.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReady_Start_Set.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReady_Start_Set.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnReady_Start_Set.Location = new System.Drawing.Point(853, 456);
            this.btnReady_Start_Set.Name = "btnReady_Start_Set";
            this.btnReady_Start_Set.Size = new System.Drawing.Size(210, 175);
            this.btnReady_Start_Set.TabIndex = 11;
            this.btnReady_Start_Set.UseVisualStyleBackColor = true;
            this.btnReady_Start_Set.Click += new System.EventHandler(this.btnReady_Start_Set_Click);
            // 
            // pbRmarker
            // 
            this.pbRmarker.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbRmarker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbRmarker.Location = new System.Drawing.Point(1318, 30);
            this.pbRmarker.Name = "pbRmarker";
            this.pbRmarker.Size = new System.Drawing.Size(120, 120);
            this.pbRmarker.TabIndex = 12;
            this.pbRmarker.TabStop = false;
            // 
            // pbBmarker
            // 
            this.pbBmarker.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbBmarker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbBmarker.Location = new System.Drawing.Point(1069, 753);
            this.pbBmarker.Name = "pbBmarker";
            this.pbBmarker.Size = new System.Drawing.Size(120, 120);
            this.pbBmarker.TabIndex = 13;
            this.pbBmarker.TabStop = false;
            // 
            // pbBwin
            // 
            this.pbBwin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbBwin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbBwin.Location = new System.Drawing.Point(853, 637);
            this.pbBwin.Name = "pbBwin";
            this.pbBwin.Size = new System.Drawing.Size(210, 235);
            this.pbBwin.TabIndex = 14;
            this.pbBwin.TabStop = false;
            // 
            // pbRwin
            // 
            this.pbRwin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbRwin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbRwin.Location = new System.Drawing.Point(853, 30);
            this.pbRwin.Name = "pbRwin";
            this.pbRwin.Size = new System.Drawing.Size(210, 235);
            this.pbRwin.TabIndex = 15;
            this.pbRwin.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PapayaWhip;
            this.ClientSize = new System.Drawing.Size(1445, 901);
            this.Controls.Add(this.plOption);
            this.Controls.Add(this.pbRwin);
            this.Controls.Add(this.pbBwin);
            this.Controls.Add(this.pbBmarker);
            this.Controls.Add(this.pbRmarker);
            this.Controls.Add(this.btnReady_Start_Set);
            this.Controls.Add(this.btnReset_Reasoning);
            this.Controls.Add(this.btnBname);
            this.Controls.Add(this.btnRname);
            this.Controls.Add(this.plBoard);
            this.Controls.Add(this.plGroup);
            this.Controls.Add(this.rtbText);
            this.Controls.Add(this.ssBar);
            this.Controls.Add(this.tsBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "밥도둑 - 반찬들의 전쟁";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tsBar.ResumeLayout(false);
            this.tsBar.PerformLayout();
            this.ssBar.ResumeLayout(false);
            this.ssBar.PerformLayout();
            this.plOption.ResumeLayout(false);
            this.plOption.PerformLayout();
            this.plGroup.ResumeLayout(false);
            this.plGroup.PerformLayout();
            this.plMessage.ResumeLayout(false);
            this.plMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRmarker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBmarker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBwin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRwin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsBar;
        private System.Windows.Forms.StatusStrip ssBar;
        private System.Windows.Forms.RichTextBox rtbText;
        private System.Windows.Forms.Panel plOption;
        private System.Windows.Forms.Panel plGroup;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.ToolStripDropDownButton tsddbtnOption;
        private System.Windows.Forms.ToolStripMenuItem 설정ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 닫기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tsbtnConn;
        private System.Windows.Forms.ToolStripButton tsbtnDisconn;
        private System.Windows.Forms.ToolStripStatusLabel tsslblTime;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Panel plMessage;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.RadioButton clientRbtn;
        private System.Windows.Forms.RadioButton serverRbtn;
        private DoubleBufferPanel plBoard;
        private System.Windows.Forms.Button btnRname;
        private System.Windows.Forms.Button btnBname;
        private System.Windows.Forms.CheckBox bmCbx;
        private System.Windows.Forms.Button btnReset_Reasoning;
        private System.Windows.Forms.Button btnReady_Start_Set;
        private System.Windows.Forms.ToolStripButton tsddbtnHelp;
        private System.Windows.Forms.PictureBox pbRmarker;
        private System.Windows.Forms.PictureBox pbBmarker;
        private System.Windows.Forms.PictureBox pbBwin;
        private System.Windows.Forms.PictureBox pbRwin;
    }
}

