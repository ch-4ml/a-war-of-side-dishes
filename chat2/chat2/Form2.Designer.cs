namespace chat2
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(0, -2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(972, 567);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(32, 482);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(357, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "8. 황금접시 3개를 먼저 차지하는 사람이 게임의 승리자가 됩니다.";
            this.label11.Click += new System.EventHandler(this.label1_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(32, 329);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(495, 36);
            this.label8.TabIndex = 0;
            this.label8.Text = "5-3. 상대방이 선택했을 거라고 생각하는 반찬을 추리합니다. 이 때, 기회는 단 한 번 뿐이며\r\n\r\n       추리에 성공하면 그 라운드는 바" +
    "로 승리하고, 실패하면 그 라운드는 바로 패배합니다.";
            this.label8.Click += new System.EventHandler(this.label1_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(32, 439);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(427, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "7. 자신이 선택한 반찬으로 황금접시를 차지하면 그 라운드의 승리자가 됩니다.";
            this.label10.Click += new System.EventHandler(this.label1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(31, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(595, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "1. 반찬을 배치하는 역할은 기본을 방장으로 하고, 한 라운드가 끝난 뒤에는 그 라운드의 패배자가 수행합니다.";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(32, 396);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(535, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "6. 이동을 마쳤을 때 반찬의 위치는 이동하기 전 반찬의 위치보다 황금접시에 가까워져야만 합니다.";
            this.label9.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(31, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(655, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "2. 7가지 반찬을 무작위로 접시 위에 놓을 수 있지만, 마지막에 놓는 황금접시와는 반찬 모두가 2칸 이상 떨어져야 합니다.";
            this.label2.Click += new System.EventHandler(this.label1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(32, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(451, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "3. 반찬의 배치가 끝나면 양쪽 플레이어들은 황금접시를 차지할 반찬을 선택합니다.";
            this.label3.Click += new System.EventHandler(this.label1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(32, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(519, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "4. 양쪽 플레이어 모두 반찬 선택을 마치면, 반찬을 배치하지 않은 사람이 먼저 차례를 얻습니다.";
            this.label4.Click += new System.EventHandler(this.label1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(32, 286);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(591, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "5-2. 인접한 다른 반찬을 뛰어넘어, 그 말의 주위에 착지할 수 있습니다. 이 때, 여러 번 뛰어넘을 수 있습니다.";
            this.label7.Click += new System.EventHandler(this.label1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(32, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(291, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "5. 자신의 차례에는 세 가지 행동을 취할 수 있습니다.";
            this.label5.Click += new System.EventHandler(this.label1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(31, 243);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(679, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "5-1. 이동시키고 싶은 반찬 하나를 선택하고, 선을 따라 빈 칸으로 한 칸 이동합니다. 이 때, 대각선으로도 이동할 수 있습니다.";
            this.label6.Click += new System.EventHandler(this.label1_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(972, 563);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.Text = "게임 방법";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form2_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}