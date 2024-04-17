using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DisplayNICCards(); // NICカードを表示するメソッドを呼び出す
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // フォームのロード時の初期化処理
            // ラベルのテキストを設定
            label1.Text = "NIC一覧表示";

            // フォームのロード時にメニュー項目を追加する
            // ファイルメニューを作成
            ToolStripMenuItem fileMenuItem = new ToolStripMenuItem("ファイル");

            // ファイルメニューにサブメニューを追加
            ToolStripMenuItem newMenuItem = new ToolStripMenuItem("新規作成");
            ToolStripMenuItem openMenuItem = new ToolStripMenuItem("開く");
            ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("保存");
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("終了");

            // サブメニューにイベントハンドラを追加
            newMenuItem.Click += NewMenuItem_Click;
            openMenuItem.Click += OpenMenuItem_Click;
            saveMenuItem.Click += SaveMenuItem_Click;
            exitMenuItem.Click += ExitMenuItem_Click;

            // サブメニューをファイルメニューに追加
            fileMenuItem.DropDownItems.Add(newMenuItem);
            fileMenuItem.DropDownItems.Add(openMenuItem);
            fileMenuItem.DropDownItems.Add(saveMenuItem);
            fileMenuItem.DropDownItems.Add(exitMenuItem);

            // メニューストリップにファイルメニューを追加
            menuStrip1.Items.Add(fileMenuItem);
        }

        private void NewMenuItem_Click(object? sender, EventArgs e)
        {
            // 新規作成メニュー項目がクリックされたときの処理
        }

        private void OpenMenuItem_Click(object? sender, EventArgs e)
        {
            // 開くメニュー項目がクリックされたときの処理
        }

        private void SaveMenuItem_Click(object? sender, EventArgs e)
        {
            // 保存メニュー項目がクリックされたときの処理
        }

        private void ExitMenuItem_Click(object? sender, EventArgs e)
        {
            // 終了メニュー項目がクリックされたときの処理

            // ユーザーに確認用のダイアログボックスを表示し、OKが選択された場合にアプリケーションを終了する
            DialogResult result = MessageBox.Show("アプリケーションを終了しますか？", "終了確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                Application.Exit();
            }
        }


        // NICカード用のカスタムコントロール
        public class NICCard : Panel
        {
            private string nicName;
            private string ipAddress;
            private Color normalColor = Color.LightGray;
            private Color missingColor = Color.Red;
            private Color hoverColor = Color.LightBlue;

            public NICCard(string nicName, string ipAddress)
            {
                this.nicName = nicName;
                this.ipAddress = ipAddress;

                InitializeCard();
            }

            private void InitializeCard()
            {
                this.Size = new Size(150, 120);
                this.BackColor = string.IsNullOrEmpty(ipAddress) ? missingColor : normalColor;

                // NICの情報を表示するラベルを作成
                Label nameLabel = new Label();
                nameLabel.Text = "Name: " + nicName;
                nameLabel.Location = new Point(5, 5);
                this.Controls.Add(nameLabel);

                // NICのIPアドレスを表示するラベルを作成
                Label ipLabel = new Label();
                ipLabel.Text = "IP: " + ipAddress;
                ipLabel.Location = new Point(5, 30);
                ipLabel.AutoSize = true; // 自動サイズ調整を有効にする
                this.Controls.Add(ipLabel);

                // NICのアイコンを表示
                PictureBox iconPictureBox = new PictureBox();
                iconPictureBox.Image = Image.FromFile("C:\\Users\\t0mm\\source\\repos\\WinFormsApp1\\WinFormsApp1\\Resource\\NIC_icon.png"); // 画像ファイルのパスを指定
                iconPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                iconPictureBox.Size = new Size(140, 45); // アイコンのサイズを調整
                iconPictureBox.Location = new Point(5, 50);
                this.Controls.Add(iconPictureBox);

                // カード全体にマウスイベントのハンドラーを追加
                this.MouseEnter += NICCard_MouseEnter;
                this.MouseLeave += NICCard_MouseLeave;
            }

            private void NICCard_MouseEnter(object? sender, EventArgs e)
            {
                // カード全体にマウスが入った時の処理
                this.BackColor = hoverColor;
            }

            private void NICCard_MouseLeave(object? sender, EventArgs e)
            {
                // カード全体からマウスが出た時の処理
                this.BackColor = string.IsNullOrEmpty(ipAddress) ? missingColor : normalColor;
            }
        }

        private void DisplayNICCards()
        {
            // FlowLayoutPanelを作成
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight; // 左から右へのフロー
            flowLayoutPanel.AutoScroll = true; // 自動スクロール有効
            flowLayoutPanel.Dock = DockStyle.Fill; // 親コントロールいっぱいに広がるように設定
            flowLayoutPanel.Padding = new Padding(0, 80, 0, 0); // 上部の余白を追加
            flowLayoutPanel.BackColor = Color.Transparent; // 親コントロールの背景色を透明に設定

            // デバイスに接続されているNICの一覧を取得
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            // NICの一覧を表示
            foreach (NetworkInterface nic in nics)
            {
                // NICの名前を取得
                string nicName = nic.Description;

                // NICのIPアドレスを取得
                string ipAddress = GetIPAddress(nic);

                // NICカードを作成してFlowLayoutPanelに追加
                NICCard nicCard = new NICCard(nicName, ipAddress);
                flowLayoutPanel.Controls.Add(nicCard);
            }

            // FormにFlowLayoutPanelを追加
            this.Controls.Add(flowLayoutPanel);
        }

        private string GetIPAddress(NetworkInterface nic)
        {
            // ネットワークインターフェースからIPアドレスを取得
            var ipProps = nic.GetIPProperties();
            var ipAddresses = ipProps.UnicastAddresses;

            // 最初のIPv4アドレスを取得（IPv6も含まれる場合、適切にフィルタリングする必要があります）
            foreach (var ipAddress in ipAddresses)
            {
                if (ipAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ipAddress.Address.ToString();
                }
            }

            // IPアドレスが見つからなかった場合、空文字列を返す
            return "";
        }

    }
}
