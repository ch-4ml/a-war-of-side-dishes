using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net; // IPAddress
using System.Net.Sockets; //TcpListener 클래스사용
using System.Threading; //스레드 클래스 사용
using System.IO; //파일 클래스 사용

namespace chat2
{
    public partial class Form1 : Form
    {
        public class DoubleBufferPanel : Panel
        {
            public DoubleBufferPanel()
            {
                this.SetStyle(ControlStyles.DoubleBuffer, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.UserPaint, true);
                this.UpdateStyles();
            }
        }

        private TcpListener Server; // TCP 네트워크 클라이언트에서 연결 수신
        private TcpClient client; //TCP 네트워크 서비스에 대한 클라이언트 연결 제공
        private NetworkStream myStream; //네트워크 스트림
        private StreamReader myRead; //스트림 읽기
        private StreamWriter myWrite; //스트림 쓰기
        private Boolean Start = false; //서버 시작
        private Boolean ClientCon = false; //클라이언트 시작
        private int myPort; //포트
        private string myName; //별칭
        private Thread myReader, myServer; //스레드
        private Boolean TextChange = false; //입력 컨트롤의 데이터입력 체크

        private int markersize = 95; // 말의 크기
        private int coinsize = 119; // 목표물의 크기
        private int slinesize = 60; // 직선 그릴 때 간격 계산
        private int clinesize = 120; // 대각선 그릴 때 간격 계산, 화점 간격 계산
        private int point = 120; // 말이 머무르는 지점, 원의 크기
        private int markercount = 1; // 말을 놓을때 종류를 계산해주기 위함
        private int coinX = 0, coinY = 0; // 이동 시 최종 검사를 위한 변수, 모든 이동은 목표물과 가까워져야 하기 때문에 필요
        private int oldX = 0, oldY = 0; // 
        private int wincheck = 0; // 승리 횟수
        private int eWincheck = 0;
        private Pen pen; // 펜 객체
        private Bitmap pImage, oImage, yImage, gImage, sImage, vImage, wImage, cImage, sWin, tWin;
        private Boolean Turn = false; // true : 내 차례, false : 상대방 차례
        private Boolean AmISetter = false; // 말 배치하는 사람
        private Boolean IsReady = false; // 준비 완료 체크 토글
        private Boolean IsSelected = false; // 상대방의 말 선택 완료 체크 토글
        enum SENDINFO { msgentry, msgclear, playername }
        enum PHASE { none, set, select, play, reasoning }
        PHASE phase = PHASE.none;
        enum MARKER { none, pink, orange, yellow, green, skyblue, violet, white, coin, black }
        MARKER marker = MARKER.none;
        MARKER checkmarker = MARKER.none; // 게임이 끝났을 때 승리자가 선택한 말을 보여주기 위함
        MARKER selectedmarker = MARKER.none; // 목표물을 획득하는 말 선택
        MARKER movingmarker = MARKER.none; // 움직임을 위해 선택한 말
        MARKER reasoningmarker = MARKER.none; // 내가 추리한 말
        MARKER rselectedmarker = MARKER.none; // 상대방이 선택한 말
        Boolean[,] Movable = new Boolean[7, 7]; // 말을 움직일 수 있는 곳인지 표시
        MARKER[,] GameBoard = new MARKER[7, 7];

        delegate void TimerEventFiredDelegate();

        public Form1()
        {
            InitializeComponent();

            //-------------------------보드판용 객체-------------------------
            pImage = new Bitmap("../../img/pink.png");
            oImage = new Bitmap("../../img/orange.png");
            yImage = new Bitmap("../../img/yellow.png");
            gImage = new Bitmap("../../img/green.png");
            sImage = new Bitmap("../../img/skyblue.png");
            vImage = new Bitmap("../../img/violet.png");
            wImage = new Bitmap("../../img/white.png");
            cImage = new Bitmap("../../img/coin.png");
            sWin = new Bitmap("../../img/coin2.png");
            tWin = new Bitmap("../../img/coin3.png");
            pen = new Pen(Color.BurlyWood);
            //-------------------------보드판용 객체-------------------------
        }

        void CallBack(Object state)
        {
            BeginInvoke(new TimerEventFiredDelegate(Work));
        }

        private void Work()
        {
            this.rtbText.Clear(); //수행해야할 작업(UI Thread 핸들링 가능)
            MessageView("대화 기록이 삭제되었습니다.");
        }

        //----------------------------------------보 그리기----------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            int mc;
            DrawGameBoard();
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (GameBoard[x, y] != MARKER.none)
                    {
                        mc = (int)GameBoard[x, y];
                        MoveMarker(x, y, ref mc);
                    }
                }
            }
            base.OnPaint(e);
        }

        private void DrawGameBoard() // 게임 판 그리기 
        {
            Graphics g = plBoard.CreateGraphics();
            for (int x = 0; x < 7; x++) // x축 그리기
                g.DrawLine(pen, slinesize, slinesize + x * clinesize, 7 * clinesize - slinesize, slinesize + x * clinesize);
            for (int y = 0; y < 7; y++) // y축 그리기
                g.DrawLine(pen, slinesize + y * clinesize, slinesize, slinesize + y * clinesize, 7 * clinesize - slinesize);
            for (int c = 0; c < 13; c++) // 좌하 > 우상 대각선 그리기
            {
                if (c < 7)
                    g.DrawLine(pen, slinesize + c * clinesize, slinesize, slinesize, slinesize + c * clinesize);
                else
                    g.DrawLine(pen, clinesize * 7 - slinesize, slinesize + (c - 7) * clinesize, slinesize + (c - 7) * clinesize, clinesize * 7 - slinesize);
            }
            for (int c = 0; c < 13; c++) // 좌상 > 우하 대각선 그리기 
            {
                if (c < 7)
                    g.DrawLine(pen, slinesize, slinesize + (6 - c) * clinesize, slinesize + c * clinesize, clinesize * 7 - slinesize);
                else
                    g.DrawLine(pen, slinesize + (c - 6) * clinesize, slinesize, clinesize * 7 - slinesize, slinesize + (12 - c) * clinesize);
            }
            DrawPoint(); // 화점 그리기
        }

        private void DrawPoint() // 화점 그리기
        {
            Graphics g = plBoard.CreateGraphics();
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    Rectangle rex = new Rectangle(slinesize + x * clinesize - point / 2, slinesize + y * clinesize - point / 2, point, point);
                    Bitmap ex = new Bitmap("../../img/rex.png");
                    g.DrawImage(ex, rex);
                }
            }
        }

        private void DrawMarker(int x, int y, ref int markercount)
        {
            Graphics g = plBoard.CreateGraphics();
            if (GameBoard[x, y] != MARKER.none)
            {
                MessageBox.Show("이미 반찬이 있습니다. 다른 위치에 놓으세요.");
                markercount--;
                return;
            }
            Rectangle r = new Rectangle(slinesize + clinesize * x - markersize / 2 - 4, slinesize + clinesize * y - markersize / 2, markersize, markersize);
            Rectangle rPink = new Rectangle(slinesize + clinesize * x - markersize / 2 - 1, slinesize + clinesize * y - markersize / 2 + 10, markersize - 10, markersize - 10);
            Rectangle rSkyblue = new Rectangle(slinesize + clinesize * x - markersize / 2 + 1, slinesize + clinesize * y - markersize / 2 + 5, markersize - 15, markersize - 15);
            Rectangle rCoin = new Rectangle(slinesize + clinesize * x - coinsize / 2 - 4, slinesize + clinesize * y - coinsize / 2 + 1, coinsize, coinsize);
            marker = (MARKER)markercount;
            switch (marker)
            {
                case MARKER.none:
                    break;

                case MARKER.pink:
                    g.DrawImage(pImage, rPink);
                    GameBoard[x, y] = MARKER.pink;
                    break;

                case MARKER.orange:
                    g.DrawImage(oImage, r);
                    GameBoard[x, y] = MARKER.orange;
                    break;

                case MARKER.yellow:
                    g.DrawImage(yImage, r);
                    GameBoard[x, y] = MARKER.yellow;
                    break;

                case MARKER.green:
                    g.DrawImage(gImage, r);
                    GameBoard[x, y] = MARKER.green;
                    break;

                case MARKER.skyblue:
                    g.DrawImage(sImage, rSkyblue);
                    GameBoard[x, y] = MARKER.skyblue;
                    break;

                case MARKER.violet:
                    g.DrawImage(vImage, r);
                    GameBoard[x, y] = MARKER.violet;
                    break;

                case MARKER.white:
                    g.DrawImage(wImage, r);
                    GameBoard[x, y] = MARKER.white;
                    break;

                case MARKER.coin:
                    if (!CheckSet(x, y))
                    {
                        MessageBox.Show("황금접시는 다른 말과 두 칸 이상 떨어져야 합니다.");
                        markercount--;
                        return;
                    }
                    else
                    {
                        g.DrawImage(cImage, rCoin);
                        GameBoard[x, y] = MARKER.coin;
                        break;
                    }
                default: break;
            }
        }

        private void MoveMarker(int x, int y, ref int markercount)
        {
            Graphics g = plBoard.CreateGraphics();
            Rectangle r = new Rectangle(slinesize + clinesize * x - markersize / 2 - 4, slinesize + clinesize * y - markersize / 2, markersize, markersize);
            Rectangle rPink = new Rectangle(slinesize + clinesize * x - markersize / 2 - 1, slinesize + clinesize * y - markersize / 2 + 10, markersize - 10, markersize - 10);
            Rectangle rSkyblue = new Rectangle(slinesize + clinesize * x - markersize / 2 + 1, slinesize + clinesize * y - markersize / 2 + 5, markersize - 15, markersize - 15);
            Rectangle rCoin = new Rectangle(slinesize + clinesize * x - coinsize / 2 - 4, slinesize + clinesize * y - coinsize / 2 + 1, coinsize, coinsize);
            marker = (MARKER)markercount;
            switch (marker)
            {
                case MARKER.none:
                    break;

                case MARKER.pink:
                    g.DrawImage(pImage, rPink);
                    GameBoard[x, y] = MARKER.pink;
                    break;

                case MARKER.orange:
                    g.DrawImage(oImage, r);
                    GameBoard[x, y] = MARKER.orange;
                    break;

                case MARKER.yellow:
                    g.DrawImage(yImage, r);
                    GameBoard[x, y] = MARKER.yellow;
                    break;

                case MARKER.green:
                    g.DrawImage(gImage, r);
                    GameBoard[x, y] = MARKER.green;
                    break;

                case MARKER.skyblue:
                    g.DrawImage(sImage, rSkyblue);
                    GameBoard[x, y] = MARKER.skyblue;
                    break;

                case MARKER.violet:
                    g.DrawImage(vImage, rSkyblue);
                    GameBoard[x, y] = MARKER.violet;
                    break;

                case MARKER.white:
                    g.DrawImage(wImage, r);
                    GameBoard[x, y] = MARKER.white;
                    break;

                case MARKER.coin:
                    g.DrawImage(cImage, rCoin);
                    break;
                default:
                    break;
            }
        }

        private void EraseMarker(int x, int y)
        {
            Graphics g = plBoard.CreateGraphics();
            Rectangle r = new Rectangle(slinesize + clinesize * x - point / 2, slinesize + clinesize * y - point / 2, point, point);
            Bitmap rex = new Bitmap("../../img/rex.png");
            g.DrawImage(rex, r);
            //g.FillEllipse(tBrush, r);
            GameBoard[x, y] = MARKER.none;
        }

        //----------------------------------------보드 그리기----------------------------------------

        private void Form1_Load(object sender, EventArgs e)
        {
            plOption.Visible = true;
        }

        private void lblPort_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void 설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.설정ToolStripMenuItem.Enabled = false;//메뉴비활성화
            this.plOption.Visible = true;//설정을 위한 상자 open
            this.txtId.Focus();//id입력상자로 초점이동
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.clientRbtn.Checked == true)
            {
                if (this.txtIp.Text == "")
                {
                    this.txtIp.Focus();
                }
                else
                {
                    ControlCheck();
                }
            }
            else
            {
                ControlCheck();   //설정을 위한 메서드호출(사용자가 정의한 메서드임)
            }
        }

        private void ControlCheck()
        {
            if (this.txtId.Text == "")//ID가 입력되지 않았으면 ID입력
            {
                this.txtId.Focus();
            }
            else if (this.txtPort.Text == "")//PORT가 입력되지 않았으면 PORT입력
            {
                this.txtPort.Focus();
            }
            else
            {
                try
                {
                    this.plOption.Visible = false;//설정이 완료되었으므로 설정상자를 비활성화
                    this.설정ToolStripMenuItem.Enabled = true;//상위메뉴바 활성화
                    this.tsbtnConn.Enabled = true;//연결메뉴 활성화
                }
                catch
                {//에러메세지 박스 출력
                    MessageBox.Show("설정이 저장되지 않았습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.설정ToolStripMenuItem.Enabled = true; //설정 메뉴 활성화
            this.plOption.Visible = false; //입력창 닫기
            this.txtMessage.Focus();
        }

        private void tsbtnConn_Click(object sender, EventArgs e)
        {
            var addr = new IPAddress(0);//IPAddress 클래스의 개체를 초기화, 매개변수가 0->로컬단말의 아이피 가져옴
            this.myName = this.txtId.Text;
            this.myPort = Convert.ToInt32(this.txtPort.Text);
            if (this.clientRbtn.Checked == true)
            {
                this.btnBname.Text = this.myName; //플레이어 이름 넣기
                this.AmISetter = false; //말 배치하는 사람, 클라이언트는 false로 시작
                this.btnReady_Start_Set.Text = "준비완료";
                this.btnReset_Reasoning.Text = "추리하기";
                if (!(this.ClientCon))
                {
                    ClientConnection(); //ClientConnection() 함수 호출
                }
                else
                {
                    this.txtMessage.Enabled = false;
                    this.btnSend.Enabled = false;
                    Disconnection(); //함수 호출
                }
            }
            else
            {
                this.btnRname.Text = this.myName;
                this.btnReady_Start_Set.Text = "게임시작";
                this.btnReset_Reasoning.Text = "배치 초기화";
                if (!(this.Start))//서버가 시작되지 않은 경우 
                {
                    try
                    {
                        Server = new TcpListener(addr, this.myPort);//ip주소와 포트번호를 인자로 TcpLIstener의 개체 생성
                        Server.Start();//Server 시작

                        this.Start = true;
                        this.txtMessage.Enabled = true;
                        this.btnSend.Enabled = true;
                        this.txtMessage.Focus();//메시지를 쓸 수 있도록 초점을 이동
                        this.tsbtnDisconn.Enabled = true;//연결을 끊을 수 있도록 메뉴 활성화
                        this.tsbtnConn.Enabled = false;//연결시도버튼 비활성화
                                                       //Thread클래스의 생성자를 이용하여 개체생성-> ServerStart()메서드로 클라이언트의 수신과 네트워크 스트림의 값을 수신하는 작업을 새로 생성한 스레스에서 수행 
                        myServer = new Thread(ServerStart);
                        myServer.Start();
                        this.설정ToolStripMenuItem.Enabled = false;
                    }
                    catch
                    {
                        MessageView("서버를 실행할 수 없습니다.");
                    }
                }
                else
                {
                    ServerStop(); //ServerStop() 함수 호출
                }
            }
        }

        private void ServerStart()
        {  //메세지출력
            MessageView("방 만들기 성공!\r\n상대방의 접속을 기다립니다.");

            while (Start)//클라이언트가 접속될 때까지 기다림
            {
                try
                {
                    client = Server.AcceptTcpClient();//보류중인 연결요청을 받아들임
                    MessageView("상대방이 접속했습니다.");
                    myStream = client.GetStream();//데이터를 보내고 받는 데 사용한 NetworkStream을 반환하여 myStream 개체에 대입
                    myRead = new StreamReader(myStream);//읽기 스트림
                    myWrite = new StreamWriter(myStream);//저장 스트림
                    this.ClientCon = true;//클라이언트 연결 허용

                    if (this.ClientCon)
                    {
                        myWrite.WriteLine("S_PlayerName" + "&" + myName);
                        myWrite.Flush();
                    }

                    myReader = new Thread(Receive);//스레드를 이용하여 myReader개체를 생성
                    myReader.Start();
                }
                catch { }
            }
        }

        private void ServerStop() // 서버 모드 종료
        {
            this.Start = false;
            this.txtMessage.Enabled = false;
            this.txtMessage.Clear();
            this.btnSend.Enabled = false;
            this.tsbtnConn.Enabled = true;
            this.tsbtnDisconn.Enabled = false;
            this.ClientCon = false;
            if (!(myRead == null))
            {
                myRead.Close(); //StreamReader 클래스 개체 리소스 해제
            }
            if (!(myWrite == null))
            {
                myWrite.Close(); //StreamWriter 클래스 개체 리소스 해제
            }
            if (!(myStream == null))
            {
                myStream.Close(); //NetworkStream 클래스 개체 리소스 해제
            }
            if (!(client == null))
            {
                client.Close(); //TcpClient 클래스 개체 리소스 해제
            }
            if (!(Server == null))
            {
                Server.Stop(); //TcpListen 클래스 개체 리소스 해제
            }
            if (!(myReader == null))
            {
                myReader.Abort(); //외부 스레드 종료
            }
            if (!(myServer == null))
            {
                myServer.Abort(); //외부 스레드 종료
            }
            MessageView("연결이 끊어졌습니다.");
        }

        private void MessageView(string strText)
        {
            try
            {
                if (bmCbx.Checked == true)
                {
                    this.rtbText.AppendText(strText + "\r\n");//입력한 텍스트와 엔터키추가
                    System.Threading.Timer timer = new System.Threading.Timer(CallBack);
                    timer.Change(1000 * 3, System.Threading.Timeout.Infinite);
                    this.rtbText.AppendText("대화 기록 삭제 메시지를 보냈습니다.\r\n모든 메시지가 3초 후 삭제됩니다.\r\n");
                    bmCbx.Checked = false;
                }
                else
                {
                    this.rtbText.AppendText(strText + "\r\n");//입력한 텍스트와 엔터키추가
                }
                this.rtbText.Focus();//초점이동
                this.rtbText.ScrollToCaret();// 컨트롤의 내용을 현재 캐럿 위치까지 스크롤
                this.txtMessage.Focus();//초점이동
            }
            catch { }
        }

        private void MessageSend(string strText)
        {
            try
            {
                if (bmCbx.Checked == true)
                {
                    this.rtbText.AppendText(strText + "\r\n");//입력한 텍스트와 엔터키추가
                    System.Threading.Timer timer = new System.Threading.Timer(CallBack);
                    timer.Change(1000 * 3, System.Threading.Timeout.Infinite);
                    this.rtbText.AppendText("대화 기록 삭제 메시지를 보냈습니다.\r\n모든 메시지가 3초 후 삭제됩니다.\r\n");
                    bmCbx.Checked = false;
                }
                else
                {
                    this.rtbText.AppendText(strText + "\r\n");//입력한 텍스트와 엔터키추가
                }
                this.rtbText.Focus();//초점이동
                this.rtbText.ScrollToCaret();// 컨트롤의 내용을 현재 캐럿 위치까지 스크롤
                this.txtMessage.Focus();//초점이동
            }
            catch { }
        }

        private void ClientConnection()
        {
            try
            {
                client = new TcpClient(this.txtIp.Text, this.myPort);
                MessageView("서버에 접속 했습니다.");
                myStream = client.GetStream();
                myRead = new StreamReader(myStream);
                myWrite = new StreamWriter(myStream);
                this.ClientCon = true;
                this.tsbtnConn.Enabled = false;
                this.tsbtnDisconn.Enabled = true;
                this.txtMessage.Enabled = true;
                this.btnSend.Enabled = true;
                this.txtMessage.Focus();

                if (this.ClientCon)
                {
                    myWrite.WriteLine("S_PlayerName" + "&" + myName);
                    myWrite.Flush();
                }

                myReader = new Thread(Receive);
                myReader.Start();
            }
            catch
            {
                this.ClientCon = false;
                MessageView("서버에 접속하지 못 했습니다.");
            }
        }

        private void Disconnection()
        {
            this.ClientCon = false;
            this.txtMessage.Enabled = false;
            this.txtMessage.Clear();
            this.btnSend.Enabled = false;
            this.tsbtnConn.Enabled = true;
            this.tsbtnDisconn.Enabled = false;
            try
            {
                if (!(myRead == null))
                {
                    myRead.Close(); //StreamReader 클래스 개체 리소스 해제
                }
                if (!(myWrite == null))
                {
                    myWrite.Close(); //StreamWriter 클래스 개체 리소스 해제
                }
                if (!(myStream == null))
                {
                    myStream.Close(); //NetworkStream 클래스 개체 리소스 해제
                }
                if (!(client == null))
                {
                    client.Close(); //TcpClient 클래스 개체 리소스 해제
                }
                if (!(myReader == null))
                {
                    myReader.Abort(); //외부 스레드 종료
                }
                MessageView("연결이 끊어졌습니다.");
            }
            catch
            {
                return;
            }
        }

        private void plGroup_Paint(object sender, PaintEventArgs e)
        {

        }

        private void rtbText_ControlAdded(object sender, ControlEventArgs e)
        {
        }

        private void btnReset_Reasoning_Click(object sender, EventArgs e)
        {
            switch (btnReset_Reasoning.Text)
            {
                case "배치 초기화":
                    this.markercount = 0;
                    for (int x = 0; x < 7; x++)
                    {
                        for (int y = 0; y < 7; y++)
                            EraseMarker(x, y);
                    }
                    MessageView("말의 배치를 초기화하였습니다.\r\n명란의 위치를 정하세요.");
                    break;

                case "추리하기":
                    if (MessageBox.Show("상대방이 선택한 말을 추리하는 데 실패하면 당신이 패배합니다.\r\n그래도 시도하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        myWrite.WriteLine("S_ReasoningStart" + "&" + myName + "(이)가 당신의 말을 추리합니다.");
                        myWrite.Flush();
                        phase = PHASE.reasoning;
                        MessageView("추리하기를 선택하였습니다.\r\n상대방이 선택했다고 생각되는 말을 클릭하세요.");
                    }
                    break;
                default: break;
            }
        }

        private void btnReady_Start_Set_Click(object sender, EventArgs e)
        {
            switch (btnReady_Start_Set.Text)
            {
                case "준비완료":
                    myWrite.WriteLine("S_IsReady" + "&" + "ImReady" + "&" + myName + "(이)가 준비 완료하였습니다.");//서버에게 보내는 부분
                    myWrite.Flush();
                    btnReady_Start_Set.BackColor = Color.LimeGreen;
                    btnReady_Start_Set.Enabled = true;
                    btnReady_Start_Set.Text = "준비해제";
                    break;
                case "준비해제":
                    myWrite.WriteLine("S_IsReady" + "&" + "ImNotReady" + "&" + myName + "(이)가 준비 해제하였습니다.");//서버에게 보내는 부분
                    myWrite.Flush();
                    btnReady_Start_Set.BackColor = Color.Transparent;
                    btnReady_Start_Set.Enabled = false;
                    btnReady_Start_Set.Text = "준비완료";
                    break;
                case "게임시작":
                    if (IsReady) //클라이언트가 준비 완료 상태인 경우
                    {
                        phase = PHASE.set; // 말 배치 단계로 이동
                        this.btnReset_Reasoning.BackColor = Color.LimeGreen;
                        this.btnReset_Reasoning.Enabled = true;
                        AmISetter = true; // 서버가 말을 배치함
                        this.btnReady_Start_Set.BackColor = Color.Transparent;
                        this.btnReset_Reasoning.Enabled = false;
                        this.btnReady_Start_Set.Text = "배치완료";
                        myWrite.WriteLine("S_SetStart" + "&" + this.myName + "(이)가 말 배치를 시작합니다. 잠시만 기다려주세요." + "&" + phase);
                        myWrite.Flush();
                        MessageView("말 배치를 시작합니다.\r\n명란의 위치를 정하세요.");
                        break;
                    }
                    else break;
                case "배치완료":
                    if (markercount == 9)
                    {
                        MessageView("황금접시를 획득할 말을 클릭하세요.");
                        for (int x = 0; x < 7; x++)
                        {
                            for (int y = 0; y < 7; y++)
                            {
                                if (GameBoard[x, y] != MARKER.none)
                                {
                                    myWrite.WriteLine("S_SetComplete" + "&" + x + "&" + y + "&" + (int)GameBoard[x, y]);
                                    myWrite.Flush();
                                }
                            }
                        }
                        this.btnReady_Start_Set.BackColor = Color.Transparent;
                        this.btnReset_Reasoning.BackColor = Color.Transparent;
                        this.btnReset_Reasoning.Text = "추리하기";
                        phase = PHASE.select;
                        break;
                    }
                    else
                    {
                        MessageBox.Show("아직 배치되지 않은 말이 있습니다.");
                        break;
                    }
                default: break;
            }
        }

        private void plBoard_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            int x, y;
            x = e.X / clinesize;
            y = e.Y / clinesize;

            switch (phase)
            {
                case PHASE.set:
                    if (AmISetter)
                    {
                        if (x < 0 || x >= 7 || y < 0 || y >= 7)
                            return;
                        if (markercount > 8)
                            break;
                        DrawMarker(x, y, ref markercount);

                        switch (markercount)
                        {
                            case 1:
                                MessageView("총각김치의 위치를 정하세요.");
                                break;
                            case 2:
                                MessageView("계란말이의 위치를 정하세요.");
                                break;
                            case 3:
                                MessageView("깻잎의 위치를 정하세요.");
                                break;
                            case 4:
                                MessageView("김의 위치를 정하세요.");
                                break;
                            case 5:
                                MessageView("주꾸미의 위치를 정하세요.");
                                break;
                            case 6:
                                MessageView("고등어의 위치를 정하세요.");
                                break;
                            case 7:
                                MessageView("황금접시의 위치를 정하세요.");
                                break;
                            case 8:
                                MessageView("모든 반찬의 위치를 정했습니다. 배치 완료 버튼을 눌러주세요.");
                                this.btnReady_Start_Set.BackColor = Color.LimeGreen;
                                this.btnReady_Start_Set.Enabled = true;
                                break;
                            default:
                                break;
                        }
                        markercount++;
                    }
                    break;

                case PHASE.select:
                    if (x < 0 || x >= 7 || y < 0 || y >= 7)
                        return;
                    if (selectedmarker != MARKER.none && selectedmarker != MARKER.coin)
                        return;
                    switch (selectedmarker = GameBoard[x, y])
                    {
                        case MARKER.pink:
                            MessageView("명란을 선택하였습니다.");
                            if (Equals(myName, this.btnRname.Text))
                                this.pbRmarker.BackgroundImage = pImage;
                            else
                                this.pbBmarker.BackgroundImage = pImage;
                            break;

                        case MARKER.orange:
                            MessageView("총각김치를 선택하였습니다.");
                            if (Equals(myName, this.btnRname.Text))
                                this.pbRmarker.BackgroundImage = oImage;
                            else
                                this.pbBmarker.BackgroundImage = oImage;
                            break;

                        case MARKER.yellow:
                            MessageView("계란말이를 선택하였습니다.");
                            if (Equals(myName, this.btnRname.Text))
                                this.pbRmarker.BackgroundImage = yImage;
                            else
                                this.pbBmarker.BackgroundImage = yImage;
                            break;

                        case MARKER.green:
                            MessageView("깻잎을 선택하였습니다.");
                            if (Equals(myName, this.btnRname.Text))
                                this.pbRmarker.BackgroundImage = gImage;
                            else
                                this.pbBmarker.BackgroundImage = gImage;
                            break;

                        case MARKER.skyblue:
                            MessageView("김을 선택하였습니다.");
                            if (Equals(myName, this.btnRname.Text))
                                this.pbRmarker.BackgroundImage = sImage;
                            else
                                this.pbBmarker.BackgroundImage = sImage;
                            break;

                        case MARKER.violet:
                            MessageView("주꾸미를 선택하였습니다.");
                            if (Equals(myName, this.btnRname.Text))
                                this.pbRmarker.BackgroundImage = vImage;
                            else
                                this.pbBmarker.BackgroundImage = vImage;
                            break;

                        case MARKER.white:
                            MessageView("고등어를 선택하였습니다.");
                            if (Equals(myName, this.btnRname.Text))
                                this.pbRmarker.BackgroundImage = wImage;
                            else
                                this.pbBmarker.BackgroundImage = wImage;
                            break;

                        case MARKER.coin:
                            MessageView("황금접시는 선택할 수 없습니다.");
                            break;
                    }

                    if (selectedmarker != MARKER.none && selectedmarker != MARKER.coin)
                    {
                        myWrite.WriteLine("S_SelectComplete" + "&" + myName + "(이)가 선택을 마쳤습니다.");
                        myWrite.Flush();
                        Turn = AmISetter ? false : true;
                        if (IsSelected)
                        {
                            MessageView("양쪽 플레이어가 모두 선택을 마쳤습니다.\r\n게임을 시작합니다.");
                            if (Turn)
                                MessageView(myName + "(이)가 먼저 시작합니다.");// 수정 필요
                            this.btnReset_Reasoning.BackColor = Color.LimeGreen;
                            this.btnReset_Reasoning.Enabled = true;
                        }
                        phase = PHASE.play;
                    }
                    break;

                case PHASE.play:
                    if (Turn)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            for (int j = 0; j < 7; j++)
                            {
                                if (GameBoard[i, j] == MARKER.coin)
                                {
                                    coinX = i;
                                    coinY = j;
                                }
                            }
                        }

                        if (movingmarker != MARKER.none) // 움직이기 위해 선택한 말이 있는 경우
                        {
                            if (movingmarker == GameBoard[x, y])
                            {
                                movingmarker = MARKER.none;
                                MessageBox.Show("선택 취소");
                            }
                            else if (GameBoard[x, y] != MARKER.none && GameBoard[x, y] != MARKER.coin)
                            {
                                MessageBox.Show("이미 말이 있습니다. 다른 위치에 놓아야 합니다.");
                            }

                            if (CheckDistance(oldX, oldY, x, y, coinX, coinY) && Movable[x, y]) // 됨
                            {
                                EraseMarker(oldX, oldY);// 기존 말을 지우는 부분
                                MoveMarker(x, y, ref markercount);// 움직이는 부분
                                if (x == coinX && y == coinY) // 목표물에 도달했고
                                {
                                    myWrite.WriteLine("S_MoveComplete" + "&" + x + "&" + y + "&" + markercount + "&" + oldX + "&" + oldY);
                                    myWrite.Flush();
                                    if (movingmarker == selectedmarker) // 도달한 말이 선택한 말이라면 (승리조건 달성)
                                    {
                                        MessageView("-----------" + myName + " 승!-----------");
                                        wincheck++;
                                        WinResult(wincheck);
                                        AmISetter = false;
                                        myWrite.WriteLine("S_GameEnd" + "&" + myName + " 승!" + "&" + (int)selectedmarker + "&" + !AmISetter);
                                        myWrite.Flush();
                                    }
                                    else
                                    {
                                        MessageView("황금접시를 차지한 반찬과 선택한 반찬이 일치하지 않아 패배합니다.");
                                        AmISetter = true;
                                        MessageView("명란의 위치를 정하세요.");
                                        myWrite.WriteLine("S_GameEnd" + "&" + myName + "(이)가 선택하지 않은 반찬이 황금접시를 차지했으므로 패배합니다." + " & " + (int)selectedmarker + "&" + !AmISetter);
                                        myWrite.Flush();
                                    }
                                    movingmarker = MARKER.none;
                                    phase = PHASE.set;
                                    if (AmISetter)
                                    {
                                        this.btnReset_Reasoning.BackColor = Color.LimeGreen;
                                        this.btnReset_Reasoning.Text = "배치 초기화";
                                        this.btnReset_Reasoning.Enabled = true;
                                    }
                                    else
                                    {
                                        this.btnReset_Reasoning.BackColor = Color.Transparent;
                                        this.btnReset_Reasoning.Enabled = false;
                                    }
                                    markercount = 1;
                                    selectedmarker = MARKER.none;
                                    marker = MARKER.none;
                                    Turn = false;
                                    IsSelected = false;
                                    pbRmarker.BackgroundImage = null;
                                    pbBmarker.BackgroundImage = null;
                                    for (int i = 0; i < 7; i++)
                                    {
                                        for (int j = 0; j < 7; j++)
                                        {
                                            EraseMarker(i, j);
                                        }
                                    }
                                    break;
                                }
                                movingmarker = MARKER.none;
                                myWrite.WriteLine("S_MoveComplete" + "&" + x + "&" + y + "&" + markercount + "&" + oldX + "&" + oldY);
                                myWrite.Flush();
                                Turn = false;
                                this.btnReset_Reasoning.BackColor = Color.Transparent;
                                this.btnReset_Reasoning.Enabled = false;
                            }
                            else
                            {
                                if (!CheckDistance(oldX, oldY, x, y, coinX, coinY) && Movable[x, y])
                                {

                                    MessageBox.Show("황금접시와의 거리가 가까워지게 놓아야 합니다.");
                                }
                                else if (CheckDistance(oldX, oldY, x, y, coinX, coinY) && !Movable[x, y])
                                {

                                    MessageBox.Show("이동 불가능한 위치입니다.");
                                }
                                else { }
                            }
                        }
                        else // 움직이기 위해 선택한 말이 없는 경우 (첫 번째 클릭)
                        {
                            movingmarker = GameBoard[x, y] != MARKER.none && GameBoard[x, y] != MARKER.coin ? GameBoard[x, y] : MARKER.none; // 움직일 말을 선택
                            markercount = (int)movingmarker;
                            CheckMove(x, y);
                            oldX = x;
                            oldY = y;
                        }
                    }
                    break;

                case PHASE.reasoning:
                    reasoningmarker = GameBoard[x, y];

                    switch (rselectedmarker)
                    {
                        case MARKER.pink:
                            if (Equals(myName, this.btnRname.Text))
                            {
                                pbBmarker.BackgroundImage = pImage;
                                break;
                            }
                            else
                            {
                                pbRmarker.BackgroundImage = pImage;
                                break;
                            }
                        case MARKER.orange:
                            if (Equals(myName, this.btnRname.Text))
                            {
                                pbBmarker.BackgroundImage = oImage;
                                break;
                            }
                            else
                            {
                                pbRmarker.BackgroundImage = oImage;
                                break;
                            }
                        case MARKER.yellow:
                            if (Equals(myName, this.btnRname.Text))
                            {
                                pbBmarker.BackgroundImage = yImage;
                                break;
                            }
                            else
                            {
                                pbRmarker.BackgroundImage = yImage;
                                break;
                            }
                        case MARKER.green:
                            if (Equals(myName, this.btnRname.Text))
                            {
                                pbBmarker.BackgroundImage = gImage;
                                break;
                            }
                            else
                            {
                                pbRmarker.BackgroundImage = gImage;
                                break;
                            }
                        case MARKER.skyblue:
                            if (Equals(myName, this.btnRname.Text))
                            {
                                pbBmarker.BackgroundImage = sImage;
                                break;
                            }
                            else
                            {
                                pbRmarker.BackgroundImage = sImage;
                                break;
                            }
                        case MARKER.violet:
                            if (Equals(myName, this.btnRname.Text))
                            {
                                pbBmarker.BackgroundImage = vImage;
                                break;
                            }
                            else
                            {
                                pbRmarker.BackgroundImage = vImage;
                                break;
                            }
                        case MARKER.white:
                            if (Equals(myName, this.btnRname.Text))
                            {
                                pbBmarker.BackgroundImage = wImage;
                                break;
                            }
                            else
                            {
                                pbRmarker.BackgroundImage = wImage;
                                break;
                            }
                        default:
                            break;
                    }

                    if (reasoningmarker == rselectedmarker) // 승리조건 달성
                    {
                        MessageView("-----------" + myName + "의 추리 성공!-----------");
                        wincheck++;
                        WinResult(wincheck);
                        AmISetter = false;
                        myWrite.WriteLine("S_GameEnd" + "&" + myName + "의 추리 성공!" + "&" + (int)selectedmarker + "&" + !AmISetter);
                        myWrite.Flush();
                    }
                    else
                    {
                        MessageView("-----------" + myName + "의 추리 실패!-----------");
                        AmISetter = true;
                        myWrite.WriteLine("S_GameEnd" + "&" + myName + "의 추리 실패!" + "&" + (int)selectedmarker + "&" + !AmISetter);
                        myWrite.Flush();
                        this.btnReset_Reasoning.Text = "배치 초기화";
                    }
                    movingmarker = MARKER.none;
                    phase = PHASE.set;
                    if (AmISetter)
                    {
                        this.btnReset_Reasoning.Text = "배치 초기화";
                        this.btnReset_Reasoning.BackColor = Color.LimeGreen;
                        this.btnReset_Reasoning.Enabled = true;
                    }
                    else
                    {
                        this.btnReset_Reasoning.BackColor = Color.Transparent;
                        this.btnReset_Reasoning.Enabled = false;
                    }
                    markercount = 1;
                    selectedmarker = MARKER.none;
                    marker = MARKER.none;
                    Turn = false;
                    pbRmarker.BackgroundImage = null;
                    pbBmarker.BackgroundImage = null;
                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            EraseMarker(i, j);
                        }
                    }
                    break;

                case PHASE.none:
                    break;
            }

        }

        //서버 및 클라이언트 모드에서 myReader 스레드 개체에서 실행되는 메서드로 메시지를 받은 데이터를 화면에 출력하는 작업을 수행
        private void Receive()
        {
            int setcount = 0;
            int mx, my, ex, ey, mindex;

            try
            {
                while (this.ClientCon)//클라이언트의 연결이 종료될 때까지 계속 실행
                {
                    if (myStream.CanRead)//스트림에서 데이터를 읽을 수 있는 경우
                    {
                        var msg = myRead.ReadLine();
                        var Smsg = msg.Split('&');//&를 기준으로 메시지 구분
                        switch (Smsg[0])
                        {
                            case "S_TextChange": //상대방이 메시지를 입력중인 상태를 전달받아 툴스트립 레이블에 입력
                                this.tsslblTime.Text = Smsg[1];
                                break;

                            case "S_MessageClear": //메시지박스의 내용을 모두 삭제하는 메시지를 받은 경우
                                this.rtbText.SelectionColor = Color.Black;
                                MessageSend(Smsg[1] + " : " + Smsg[2]);
                                this.rtbText.SelectionColor = Color.Pink;
                                MessageView("대화 내용 삭제 메시지를 보냈습니다.\r\n모든 메시지가 3초 후 삭제됩니다.\r\n");
                                System.Threading.Timer timer = new System.Threading.Timer(CallBack);
                                timer.Change(1000 * 10, System.Threading.Timeout.Infinite);
                                this.tsslblTime.Text = "마지막으로 받은 시각 :" + Smsg[3];
                                break;

                            case "S_PlayerName": //상대방 폼의 플레이어 이름이 표시되는 버튼에 내 이름을 입력
                                if (!this.clientRbtn.Checked)
                                {
                                    if (Equals(this.btnBname.Text, ""))
                                        this.btnBname.Text = Smsg[1];// 내가 서버라면, 블루팀에 상대방 이름을 넣어줘야 함
                                    break;
                                }
                                else
                                {
                                    if (Equals(this.btnRname.Text, ""))
                                        this.btnRname.Text = Smsg[1]; // 내가 클라이언트라면, 레드팀에 상대방 이름을 넣어줘야 함
                                    break;
                                }

                            case "S_IsReady": //준비 완료 상태를 전달받음
                                if (Equals(Smsg[1], "ImReady"))
                                {
                                    MessageView(Smsg[2]);
                                    IsReady = true; // 클라이언트의 준비 완료
                                    this.btnReady_Start_Set.BackColor = Color.LimeGreen;
                                    this.btnReady_Start_Set.Enabled = true;
                                    break;
                                }
                                else
                                {
                                    MessageView(Smsg[2]);
                                    IsReady = false; // 클라이언트 준비 해제
                                    this.btnReady_Start_Set.BackColor = Color.Transparent;
                                    this.btnReady_Start_Set.Enabled = false;
                                    break;
                                }

                            case "S_SetStart": //서버가 게임 시작(배치 단계) 버튼을 눌렀을 때 게임 시작 신호 전달받음
                                MessageView(Smsg[1]);
                                this.btnReady_Start_Set.BackColor = Color.Transparent;
                                this.btnReady_Start_Set.Enabled = false;
                                this.btnReady_Start_Set.Text = "배치완료"; // 클라이언트도 배치하는 턴이 올 수도 있으니까 써뒀음
                                this.AmISetter = false;
                                break;

                            case "S_SetComplete": //배치 완료 상태 전달 받음
                                mx = Convert.ToInt32(Smsg[1]);
                                my = Convert.ToInt32(Smsg[2]);
                                mindex = Convert.ToInt32(Smsg[3]);
                                DrawMarker(mx, my, ref mindex);
                                phase = PHASE.select;
                                setcount++;
                                if (setcount == 8)
                                    MessageView("황금접시를 획득할 말을 클릭하세요.");
                                break;


                            case "S_SelectComplete":
                                IsSelected = true;
                                MessageView(Smsg[1]);
                                if (phase == PHASE.play)
                                {
                                    MessageView("양쪽 플레이어가 모두 선택을 마쳤습니다.\r\n게임을 시작합니다.");
                                    btnReset_Reasoning.BackColor = Color.LimeGreen;
                                    btnReset_Reasoning.Enabled = true;
                                    if (Turn)
                                        MessageView(myName + "(이)가 먼저 시작합니다.");
                                }
                                break;

                            case "S_MoveComplete":
                                mx = Convert.ToInt32(Smsg[1]);
                                my = Convert.ToInt32(Smsg[2]);
                                mindex = Convert.ToInt32(Smsg[3]);
                                ex = Convert.ToInt32(Smsg[4]);
                                ey = Convert.ToInt32(Smsg[5]);
                                MoveMarker(mx, my, ref mindex);
                                EraseMarker(ex, ey);
                                Turn = true;
                                btnReset_Reasoning.BackColor = Color.LimeGreen;
                                btnReset_Reasoning.Enabled = true;
                                for (int i = 0; i < 7; i++)
                                {
                                    for (int j = 0; j < 7; j++)
                                        Movable[i, j] = false; // 검사 전, 모든 발판을 이동 불가지역으로 만듦
                                }
                                break;

                            case "S_ReasoningStart":
                                MessageView(Smsg[1]);
                                myWrite.WriteLine("S_MySelection" + "&" + (int)selectedmarker);
                                myWrite.Flush();
                                break;

                            case "S_MySelection":
                                rselectedmarker = (MARKER)Convert.ToInt32(Smsg[1]);
                                break;

                            case "S_GameEnd":
                                checkmarker = (MARKER)Convert.ToInt32(Smsg[2]);
                                switch (checkmarker)
                                {
                                    case MARKER.pink:
                                        if (Equals(myName, this.btnRname.Text))
                                        {
                                            pbBmarker.BackgroundImage = pImage;
                                            break;
                                        }
                                        else
                                        {
                                            pbRmarker.BackgroundImage = pImage;
                                            break;
                                        }
                                    case MARKER.orange:
                                        if (Equals(myName, this.btnRname.Text))
                                        {
                                            pbBmarker.BackgroundImage = oImage;
                                            break;
                                        }
                                        else
                                        {
                                            pbRmarker.BackgroundImage = oImage;
                                            break;
                                        }
                                    case MARKER.yellow:
                                        if (Equals(myName, this.btnRname.Text))
                                        {
                                            pbBmarker.BackgroundImage = yImage;
                                            break;
                                        }
                                        else
                                        {
                                            pbRmarker.BackgroundImage = yImage;
                                            break;
                                        }
                                    case MARKER.green:
                                        if (Equals(myName, this.btnRname.Text))
                                        {
                                            pbBmarker.BackgroundImage = gImage;
                                            break;
                                        }
                                        else
                                        {
                                            pbRmarker.BackgroundImage = gImage;
                                            break;
                                        }
                                    case MARKER.skyblue:
                                        if (Equals(myName, this.btnRname.Text))
                                        {
                                            pbBmarker.BackgroundImage = sImage;
                                            break;
                                        }
                                        else
                                        {
                                            pbRmarker.BackgroundImage = sImage;
                                            break;
                                        }
                                    case MARKER.violet:
                                        if (Equals(myName, this.btnRname.Text))
                                        {
                                            pbBmarker.BackgroundImage = vImage;
                                            break;
                                        }
                                        else
                                        {
                                            pbRmarker.BackgroundImage = vImage;
                                            break;
                                        }
                                    case MARKER.white:
                                        if (Equals(myName, this.btnRname.Text))
                                        {
                                            pbBmarker.BackgroundImage = wImage;
                                            break;
                                        }
                                        else
                                        {
                                            pbRmarker.BackgroundImage = wImage;
                                            break;
                                        }
                                    default:
                                        break;
                                }
                                MessageBox.Show(Smsg[1]); // 됨
                                markercount = 1;
                                AmISetter = Convert.ToBoolean(Smsg[3]);
                                if (AmISetter)
                                {
                                    this.btnReset_Reasoning.BackColor = Color.LimeGreen;
                                    this.btnReset_Reasoning.Enabled = true;
                                    this.btnReset_Reasoning.Text = "배치 초기화";
                                    MessageView("명란의 위치를 정하세요.");
                                }
                                else
                                {
                                    wincheck++;
                                    WinResult(wincheck);
                                }
                                phase = PHASE.set;
                                selectedmarker = MARKER.none;
                                marker = MARKER.none;
                                IsSelected = false;
                                pbRmarker.BackgroundImage = null;
                                pbBmarker.BackgroundImage = null;

                                for (int i = 0; i < 7; i++)
                                {
                                    for (int j = 0; j < 7; j++)
                                    {
                                        EraseMarker(i, j);
                                    }
                                }
                                break;

                            case "S_ProgramEnd":
                                MessageBox.Show(Smsg[1] + "(이)가 최종 승리하였습니다.");
                                try
                                {
                                    if (this.client.Connected)//연결중인 상태
                                    {
                                        var dt = Convert.ToString(DateTime.Now);
                                        myWrite.WriteLine(this.myName + "&" + "채팅 APP가 종료되었습니다." + "&" + dt);
                                        myWrite.Flush();
                                    }
                                }
                                catch { }

                                if (clientRbtn.Checked == true) Disconnection();//클라이언트종료
                                else ServerStop();
                                this.설정ToolStripMenuItem.Enabled = true;
                                break;

                            case "S_Winner":
                                eWincheck = Convert.ToInt32(Smsg[1]);
                                EnemyWinResult(eWincheck);
                                break;

                            default:
                                this.rtbText.SelectionColor = Color.Black;
                                MessageView(Smsg[0] + " : " + Smsg[1]);
                                this.rtbText.SelectionColor = Color.Pink;
                                this.tsslblTime.Text = "마지막으로 받은 시각 :" + Smsg[2];
                                break;
                        }
                    }
                }
            }
            catch { }
        }

        private void plBoard_MouseEnter(object sender, EventArgs e)
        {

        }

        private void plBoard_MouseMove(object sender, MouseEventArgs e)
        {

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void tsbtnDisconn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.client.Connected)//연결중인 상태
                {
                    var dt = Convert.ToString(DateTime.Now);
                    myWrite.WriteLine(this.myName + "&" + "채팅 APP가 종료되었습니다." + "&" + dt);
                    myWrite.Flush();
                }
            }
            catch { }

            if (clientRbtn.Checked == true) Disconnection();//클라이언트종료
            else ServerStop();
            this.설정ToolStripMenuItem.Enabled = true;
        }

        private void btnRwin_Click(object sender, EventArgs e)
        {

        }

        private void tsddbtnHelp_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (this.txtMessage.Text == "")
            {
                this.txtMessage.Focus();
            }
            else
            {
                Msg_send();
            }
        }

        private void Msg_send()
        {
            try
            {
                var dt = Convert.ToString(DateTime.Now);
                //아이디 & 메시지 & 날짜시간을 보냄
                if (bmCbx.Checked)
                {
                    myWrite.WriteLine("S_MessageClear" + "&" + this.myName + "&" + this.txtMessage.Text + "&" + dt);
                    myWrite.Flush();
                    //서버의 rtbText에 id와 메시지 출력
                    this.rtbText.SelectionColor = Color.Black;
                    MessageView(this.myName + " : " + this.txtMessage.Text);
                    this.rtbText.SelectionColor = Color.Pink;
                    this.txtMessage.Clear();
                }
                else
                {
                    myWrite.WriteLine(this.myName + "&" + this.txtMessage.Text + "&" + dt);
                    myWrite.Flush();
                    //서버의 rtbText에 id와 메시지 출력
                    this.rtbText.SelectionColor = Color.Black;
                    MessageView(this.myName + " : " + this.txtMessage.Text);
                    this.rtbText.SelectionColor = Color.Pink;
                    this.txtMessage.Clear();
                }
            }
            catch
            {
                MessageView("데이터를 보내는 동안 오류가 발생하였습니다.");
                this.txtMessage.Clear();
            }

        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) //엔터키를 누를 때
            {
                e.Handled = true; //소리 없앰
                if (this.txtMessage.Text == "")
                {
                    this.txtMessage.Focus();
                }
                else
                {
                    Msg_send(); //Msg_send()함수 호출
                }
            }
        }

        private void tsBar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            //입력을 안하고 있던 상황에서 입력을 시작하는 경우
            if (TextChange == false)
            {
                TextChange = true;//입력을 하고 있다고 지정
                //정보전달의 암호: “S_*”
                myWrite.WriteLine("S_TextChange" + "&" + "상대방이 메시지 입력중입니다.");
                myWrite.Flush();
            }
            else if (this.txtMessage.Text == "" && TextChange == true)
            {//메시지를 입력하지 않은 경우
                TextChange = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (clientRbtn.Checked == true) Disconnection();//클라이언트종료
            else ServerStop(); //서버종료
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////
        private void plOption_Paint(object sender, PaintEventArgs e)
        {

        }

        private void serverRbtn_CheckedChanged(object sender, EventArgs e)
        {
            //lblIp.Visible = false;
            txtIp.Visible = false;
        }

        private void clientRbtn_CheckedChanged(object sender, EventArgs e)
        {
            //lblIp.Visible = true;
            txtIp.Visible = true;
        }

        private void rtbText_TextChanged(object sender, EventArgs e)
        {

        }

        private bool CheckSet(int x, int y) //목표물을 놓을 때 인접한 말이 없어야 함
        {
            try
            {
                return GameBoard[x, y - 1] == MARKER.none && // ↑
                    GameBoard[x + 1, y - 1] == MARKER.none &&  // ↗
                    GameBoard[x + 1, y] == MARKER.none && // →
                    GameBoard[x + 1, y + 1] == MARKER.none &&// ↘
                    GameBoard[x, y + 1] == MARKER.none && // ↓
                    GameBoard[x - 1, y + 1] == MARKER.none && // ↙
                    GameBoard[x - 1, y] == MARKER.none && // ←
                    GameBoard[x - 1, y - 1] == MARKER.none // ↖
                    ? true : false;
            }
            catch // 끝 칸 처리
            {
                if (x - 1 < 0 && y - 1 < 0) // (0,0) 지점에 놓는다면
                {
                    if (
                        GameBoard[x + 1, y] == MARKER.none && // →
                        GameBoard[x + 1, y + 1] == MARKER.none &&// ↘
                        GameBoard[x, y + 1] == MARKER.none // ↓
                        )
                        return true;
                    else
                        return false;
                }
                else if (x + 1 > 6 && y - 1 < 0) // (6,0) 지점에 놓는다면
                {
                    if (
                        GameBoard[x, y + 1] == MARKER.none && // ↓
                        GameBoard[x - 1, y + 1] == MARKER.none && // ↙
                        GameBoard[x - 1, y] == MARKER.none // ←
                        )
                        return true;
                    else
                        return false;
                }
                else if (x + 1 > 6 && y + 1 > 6) // (6,6) 지점에 놓는다면
                {
                    if (
                        GameBoard[x - 1, y] == MARKER.none && // ←
                        GameBoard[x - 1, y - 1] == MARKER.none && // ↖
                        GameBoard[x, y - 1] == MARKER.none // ↑
                        )
                        return true;
                    else
                        return false;
                }
                else if (x - 1 < 0 && y + 1 > 6) // (0,6) 지점에 놓는다면
                {
                    if (
                        GameBoard[x, y - 1] == MARKER.none && // ↑
                        GameBoard[x + 1, y - 1] == MARKER.none &&  // ↗
                        GameBoard[x + 1, y] == MARKER.none  // →
                        )
                        return true;
                    else
                        return false;
                }
                else if (y - 1 < 0) // 위쪽 줄에 놓는다면
                {
                    if (
                        GameBoard[x + 1, y] == MARKER.none && // →
                        GameBoard[x + 1, y + 1] == MARKER.none &&// ↘
                        GameBoard[x, y + 1] == MARKER.none && // ↓
                        GameBoard[x - 1, y + 1] == MARKER.none && // ↙
                        GameBoard[x - 1, y] == MARKER.none // ←
                        )
                        return true;
                    else
                        return false;
                }
                else if (x + 1 > 6) // 오른쪽 줄에 놓는다면
                {
                    if (
                        GameBoard[x, y + 1] == MARKER.none && // ↓
                        GameBoard[x - 1, y + 1] == MARKER.none && // ↙
                        GameBoard[x - 1, y] == MARKER.none && // ←
                        GameBoard[x - 1, y - 1] == MARKER.none && // ↖
                        GameBoard[x, y - 1] == MARKER.none // ↑
                        )
                        return true;
                    else
                        return false;
                }
                else if (y + 1 > 6) // 아래쪽 줄에 놓는다면
                {
                    if (
                        GameBoard[x - 1, y] == MARKER.none && // ←
                    GameBoard[x - 1, y - 1] == MARKER.none &&// ↖
                    GameBoard[x, y - 1] == MARKER.none && // ↑
                    GameBoard[x + 1, y - 1] == MARKER.none &&  // ↗
                    GameBoard[x + 1, y] == MARKER.none // →    
                    )
                        return true;
                    else
                        return false;
                }
                else if (x - 1 < 0) //왼쪽 줄에 놓는다면
                {
                    if (
                        GameBoard[x, y - 1] == MARKER.none && // ↑
                        GameBoard[x + 1, y - 1] == MARKER.none &&  // ↗
                        GameBoard[x + 1, y] == MARKER.none && // →
                        GameBoard[x + 1, y + 1] == MARKER.none &&// ↘
                        GameBoard[x, y + 1] == MARKER.none  // ↓
                        )
                        return true;
                    else
                        return false;
                }
                else return false;
            }
        }

        private void CheckMove(int x, int y) //말을 움직이는 경우 계산
        {
            int[] checkX = new int[7]; //0~6
            int[] checkY = new int[7];
            int[] saltableX = new int[7]; //뛰어 넘을 수 있는 말의 X좌표를 저장하는 배열
            int[] saltableY = new int[7];
            int[] skipcheck = new int[7];
            Boolean[] skip = new Boolean[7] { false, false, false, false, false, false, false };
            int c = 0;// 모든 말의 좌표 정보를 담기 위한 인덱스
            int count = 0; // 인접한 말, 뛰어 넘기 가능한 말의 좌표 정보를 담기 위한 인덱스
            int ocount = 0;

            //뛰어넘기가 불가능한 경우(인접한 말이 없는 경우)
            if (CheckSet(x, y))
            {
                CheckNEWS(x, y); // 8방향 true로 바꿔주는 메서드
            }

            else // 뛰어넘기가 가능한 경우
            {
                CheckNEWS(x, y); // 8방향 true로 바꿔주는 메서드
                //for문 추가?
                try
                { //8방향 조사해서 인접한 말의 좌표를 구함
                    if (GameBoard[x, y - 1] != MARKER.none && GameBoard[x, y - 1] != MARKER.coin)
                    {
                        saltableX[count] = x;
                        saltableY[count] = y;
                        count++;
                    }

                    if (GameBoard[x + 1, y - 1] != MARKER.none && GameBoard[x + 1, y - 1] != MARKER.coin)
                    {
                        saltableX[count] = x;
                        saltableY[count] = y;
                        count++;
                    }

                    if (GameBoard[x + 1, y] != MARKER.none && GameBoard[x + 1, y] != MARKER.coin)
                    {
                        saltableX[count] = x;
                        saltableY[count] = y;
                        count++;
                    }

                    if (GameBoard[x + 1, y + 1] != MARKER.none && GameBoard[x + 1, y + 1] != MARKER.coin)
                    {
                        saltableX[count] = x;
                        saltableY[count] = y;
                        count++;
                    }

                    if (GameBoard[x, y + 1] != MARKER.none && GameBoard[x, y + 1] != MARKER.coin)
                    {
                        saltableX[count] = x;
                        saltableY[count] = y;
                        count++;
                    }

                    if (GameBoard[x - 1, y + 1] != MARKER.none && GameBoard[x - 1, y + 1] != MARKER.coin)
                    {
                        saltableX[count] = x;
                        saltableY[count] = y;
                        count++;
                    }

                    if (GameBoard[x - 1, y] != MARKER.none && GameBoard[x - 1, y] != MARKER.coin)
                    {
                        saltableX[count] = x;
                        saltableY[count] = y;
                        count++;
                    }

                    if (GameBoard[x - 1, y - 1] != MARKER.none && GameBoard[x - 1, y - 1] != MARKER.coin)
                    {
                        saltableX[count] = x;
                        saltableY[count] = y;
                        count++;
                    }
                }
                catch
                {
                    if (x - 1 < 0 && y - 1 < 0) // (0,0) 지점의 말을 선택한다면
                    {
                        if (GameBoard[x + 1, y] != MARKER.none && GameBoard[x + 1, y] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x + 1, y + 1] != MARKER.none && GameBoard[x + 1, y + 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x, y + 1] != MARKER.none && GameBoard[x, y + 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                    }
                    else if (x + 1 > 6 && y - 1 < 0) // (6,0) 지점의 말을 선택한다면
                    {
                        if (GameBoard[x, y + 1] != MARKER.none && GameBoard[x, y + 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x - 1, y + 1] != MARKER.none && GameBoard[x - 1, y + 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x - 1, y] != MARKER.none && GameBoard[x - 1, y] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                    }
                    else if (x + 1 > 6 && y + 1 > 6) // (6,6) 지점의 말을 선택한다면
                    {
                        if (GameBoard[x - 1, y] != MARKER.none && GameBoard[x - 1, y] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x - 1, y - 1] != MARKER.none && GameBoard[x - 1, y - 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x, y - 1] != MARKER.none && GameBoard[x, y - 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                    }
                    else if (x - 1 < 0 && y + 1 > 6) // (0,6) 지점의 말을 선택한다면
                    {
                        if (GameBoard[x, y - 1] != MARKER.none && GameBoard[x, y - 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x + 1, y - 1] != MARKER.none && GameBoard[x + 1, y - 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x + 1, y] != MARKER.none && GameBoard[x + 1, y] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                    }
                    else if (y - 1 < 0) // 위쪽 줄의 말을 선택한다면
                    {
                        if (GameBoard[x + 1, y] != MARKER.none && GameBoard[x + 1, y] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x + 1, y + 1] != MARKER.none && GameBoard[x + 1, y + 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x, y + 1] != MARKER.none && GameBoard[x, y + 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x - 1, y + 1] != MARKER.none && GameBoard[x - 1, y + 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x - 1, y] != MARKER.none && GameBoard[x - 1, y] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                    }
                    else if (x + 1 > 6) // 오른쪽 줄의 말을 선택한다면
                    {
                        if (GameBoard[x, y + 1] != MARKER.none && GameBoard[x, y + 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x - 1, y + 1] != MARKER.none && GameBoard[x - 1, y + 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x - 1, y] != MARKER.none && GameBoard[x - 1, y] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x - 1, y - 1] != MARKER.none && GameBoard[x - 1, y - 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x, y - 1] != MARKER.none && GameBoard[x, y - 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                    }
                    else if (y + 1 > 6) // 아래쪽 줄의 말을 선택한다면
                    {
                        if (GameBoard[x - 1, y] != MARKER.none && GameBoard[x - 1, y] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x - 1, y - 1] != MARKER.none && GameBoard[x - 1, y - 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x, y - 1] != MARKER.none && GameBoard[x, y - 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x + 1, y - 1] != MARKER.none && GameBoard[x + 1, y - 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x + 1, y] != MARKER.none && GameBoard[x + 1, y] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                    }
                    else if (x - 1 < 0) //왼쪽 줄의 말을 선택한다면
                    {
                        if (GameBoard[x, y - 1] != MARKER.none && GameBoard[x, y - 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x + 1, y - 1] != MARKER.none && GameBoard[x + 1, y - 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x + 1, y] != MARKER.none && GameBoard[x + 1, y] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x + 1, y + 1] != MARKER.none && GameBoard[x + 1, y + 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                        else if (GameBoard[x, y + 1] != MARKER.none && GameBoard[x, y + 1] != MARKER.coin)
                        {
                            saltableX[count] = x;
                            saltableY[count] = y;
                            count++;
                        }
                    }
                }

                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if (GameBoard[i, j] != MARKER.none && GameBoard[i, j] != MARKER.coin)
                        {
                            Movable[i, j] = false;
                            checkX[c] = i;
                            checkY[c] = j;
                            c++;
                        }
                    }
                }

                for (int i = 0; i < count; i++) // 인접한 말로 부터 이동 가능 체크
                {
                    CheckNEWS(saltableX[i], saltableY[i]); // 인접한 말에서도 8방향 true로 바꿔 준 다음
                } //여태까지 인접한 말의 목록 조사가 끝나면

                for (int i = 0; i < count; i++) //뛰어넘기 검사 시작
                {
                    for (int j = 0; j < 7; j++)
                    {
                        for (int k = 0; k < ocount; k++)
                        {
                            if (skipcheck[k] == j)
                                skip[j] = true;
                        }

                        if (skip[j] == true)
                            continue;
                        else
                        {
                            if (saltableX[i] == checkX[j] && saltableY[i] == checkY[j])
                            {
                                skipcheck[ocount] = j;
                                ocount++;
                                continue;
                            }
                            else if (Math.Sqrt(Math.Pow(Math.Abs(saltableX[i] - checkX[j]), 2) + Math.Pow(Math.Abs(saltableY[i] - checkY[j]), 2)) < 3)
                            {// 인접한 말과 다른 말들과의 거리를 구해서 거리가 3 미만(두 칸 이하)이면
                                CheckNEWS(checkX[j], checkY[j]);// 거리가 3 미만인 다른 말들에서도 8방향 true로 바꿔줌
                                saltableX[count] = checkX[j];
                                saltableY[count] = checkY[j];
                                if (count < 6)
                                    count++;
                                skipcheck[ocount] = j;
                                ocount++;

                            }
                        }
                    }
                }

                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if (GameBoard[i, j] != MARKER.none && GameBoard[i, j] != MARKER.coin)
                        {
                            Movable[i, j] = false;
                        }
                    }
                }
            }
        }

        private Boolean CheckDistance(int oldX, int oldY, int newX, int newY, int coinX, int coinY)
        {

            return (Math.Pow(Math.Abs(coinX - oldX), 2) + Math.Pow(Math.Abs(coinY - oldY), 2) > Math.Pow(Math.Abs(coinX - newX), 2) + Math.Pow(Math.Abs(coinY - newY), 2));
        }

        private void CheckNEWS(int x, int y)
        {
            try
            {
                Movable[x, y - 1] = true;
                Movable[x + 1, y - 1] = true;
                Movable[x + 1, y] = true;
                Movable[x + 1, y + 1] = true;
                Movable[x, y + 1] = true;
                Movable[x - 1, y + 1] = true;
                Movable[x - 1, y] = true;
                Movable[x - 1, y - 1] = true; // 8방향 1칸 이동 가능
            }
            catch // 끝 칸에서 움직임을 시작하는 경우의 처리
            {
                if (x - 1 < 0 && y - 1 < 0) // (0,0) 지점의 말을 선택한다면
                {
                    Movable[x + 1, y] = true;
                    Movable[x + 1, y + 1] = true;
                    Movable[x, y + 1] = true;
                }
                else if (x + 1 > 6 && y - 1 < 0) // (6,0) 지점의 말을 선택한다면
                {
                    Movable[x, y + 1] = true;
                    Movable[x - 1, y + 1] = true;
                    Movable[x - 1, y] = true;
                }
                else if (x + 1 > 6 && y + 1 > 6) // (6,6) 지점의 말을 선택한다면
                {
                    Movable[x - 1, y] = true;
                    Movable[x - 1, y - 1] = true;
                    Movable[x, y - 1] = true;
                }
                else if (x - 1 < 0 && y + 1 > 6) // (0,6) 지점의 말을 선택한다면
                {
                    Movable[x, y - 1] = true;
                    Movable[x + 1, y - 1] = true;
                    Movable[x + 1, y] = true;
                }
                else if (y - 1 < 0) // 위쪽 줄의 말을 선택한다면
                {
                    Movable[x + 1, y] = true;
                    Movable[x + 1, y + 1] = true;
                    Movable[x, y + 1] = true;
                    Movable[x - 1, y + 1] = true;
                    Movable[x - 1, y] = true;
                }
                else if (x + 1 > 6) // 오른쪽 줄의 말을 선택한다면
                {
                    Movable[x, y + 1] = true;
                    Movable[x - 1, y + 1] = true;
                    Movable[x - 1, y] = true;
                    Movable[x - 1, y - 1] = true;
                    Movable[x, y - 1] = true;
                }
                else if (y + 1 > 6) // 아래쪽 줄의 말을 선택한다면
                {
                    Movable[x - 1, y] = true;
                    Movable[x - 1, y - 1] = true;
                    Movable[x, y - 1] = true;
                    Movable[x + 1, y - 1] = true;
                    Movable[x + 1, y] = true;
                }
                else if (x - 1 < 0) //왼쪽 줄의 말을 선택한다면
                {
                    Movable[x, y - 1] = true;
                    Movable[x + 1, y - 1] = true;
                    Movable[x + 1, y] = true;
                    Movable[x + 1, y + 1] = true;
                    Movable[x, y + 1] = true;
                }
            }
        }
        private void EnemyWinResult(int eWincheck)
        {
            switch (eWincheck)
            {
                case 1:
                    if (Equals(myName, this.btnRname.Text))
                    {
                        pbBwin.BackgroundImage = cImage;
                    }
                    else
                    {
                        pbRwin.BackgroundImage = cImage;
                    }
                    break;
                case 2:
                    if (Equals(myName, this.btnRname.Text))
                    {
                        pbBwin.BackgroundImage = sWin;
                    }
                    else
                    {
                        pbRwin.BackgroundImage = sWin;
                    }
                    break;
                case 3:
                    if (Equals(myName, this.btnRname.Text))
                    {
                        pbBwin.BackgroundImage = tWin;
                    }
                    else
                    {
                        pbRwin.BackgroundImage = tWin;
                    }
                    break;
            }
        }

        private void WinResult(int wincheck)
        {
            switch (wincheck)
            {
                case 1:
                    if (Equals(myName, this.btnRname.Text))
                    {
                        pbRwin.BackgroundImage = cImage;
                    }
                    else
                    {
                        pbBwin.BackgroundImage = cImage;
                    }
                    break;
                case 2:
                    if (Equals(myName, this.btnRname.Text))
                    {
                        pbRwin.BackgroundImage = sWin;
                    }
                    else
                    {
                        pbBwin.BackgroundImage = sWin;
                    }
                    break;
                case 3:
                    if (Equals(myName, this.btnRname.Text))
                    {
                        pbRwin.BackgroundImage = tWin;
                    }
                    else
                    {
                        pbBwin.BackgroundImage = tWin;
                    }
                    MessageView(myName + "(이)가 최종 승리하였습니다.");
                    myWrite.WriteLine("S_ProgramEnd" + "&" + myName);
                    myWrite.Flush();
                    break;
                default:
                    break;
            }
            myWrite.WriteLine("S_Winner" + "&" + wincheck);
            myWrite.Flush();
        }
    }
}
