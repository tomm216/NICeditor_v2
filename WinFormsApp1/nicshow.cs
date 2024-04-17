using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace WinFormsApp1
{
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
            iconPictureBox.Image = global::WinFormsApp1.Properties.Resources.NIC_icon; // プロジェクトのリソースから画像を読み込む
            iconPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            iconPictureBox.Size = new Size(140, 45); // アイコンのサイズを調整
            iconPictureBox.Location = new Point(5, 50);
            this.Controls.Add(iconPictureBox);


            // マウスイベントのハンドラーを追加
            this.MouseEnter += NICCard_MouseEnter;
            this.MouseLeave += NICCard_MouseLeave;

            // 画像の部分にもマウスイベントを追加
            nameLabel.MouseEnter += NICCard_MouseEnter;
            nameLabel.MouseLeave += NICCard_MouseLeave;
            ipLabel.MouseEnter += NICCard_MouseEnter;
            ipLabel.MouseLeave += NICCard_MouseLeave;
            iconPictureBox.MouseEnter += NICCard_MouseEnter;
            iconPictureBox.MouseLeave += NICCard_MouseLeave;
        }

        private void NICCard_MouseEnter(object? sender, EventArgs e)
        {
            this.BackColor = hoverColor;
        }

        private void NICCard_MouseLeave(object? sender, EventArgs e)
        {
            this.BackColor = string.IsNullOrEmpty(ipAddress) ? missingColor : normalColor;
        }
    }

    // Form1クラス
    public partial class Form1 : Form
    {
        private Button refreshButton; // 再取得ボタンのフィールドを追加
        private FlowLayoutPanel flowLayoutPanel; // flowLayoutPanelをクラスのメンバー変数として定義

        public Form1()
        {
            InitializeComponent();
            this.Resize += Form1_Resize; // サイズ変更イベントの設定
            DisplayNICCards(); // NICカードを表示するメソッドを呼び出す
        }

        private void DisplayNICCards()
        {
            // FlowLayoutPanelを作成
            flowLayoutPanel = new FlowLayoutPanel(); // 既存のローカル変数の代わりにメンバー変数を使用
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

            // 再取得ボタンを作成して設定
            refreshButton = new Button();
            refreshButton.Text = "再取得";
            refreshButton.Click += RefreshButton_Click;
            refreshButton.Margin = new Padding(10); // マージンを追加
            refreshButton.Width = 100; // 幅を100ピクセルに設定

            // ボタンの位置を設定
            UpdateRefreshButtonLocation();

            // フォームにボタンを追加
            this.Controls.Add(refreshButton);

            // FormにFlowLayoutPanelを追加
            this.Controls.Add(flowLayoutPanel);

        }

        private void UpdateRefreshButtonLocation()
        {
            // ボタンの位置を再計算
            int buttonX = this.ClientSize.Width - refreshButton.Width - 20; // ボタンの右端を20ピクセルのマージンで調整
            int buttonY = this.ClientSize.Height - refreshButton.Height - 20; // ボタンの下端を20ピクセルのマージンで調整
            refreshButton.Location = new Point(buttonX, buttonY);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // ボタンの位置を再計算
            UpdateRefreshButtonLocation();
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
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            // NIC情報を再取得する
            RefreshNICInfo();
        }

        private void RefreshNICInfo()
        {
            // flowLayoutPanelがnullでないかチェック
            if (flowLayoutPanel != null)
            {
                // フローレイアウトパネルの子コントロール（NICカード）をクリア
                flowLayoutPanel.Controls.Clear();

                // FlowLayoutPanel を再度追加
                this.Controls.Add(flowLayoutPanel);

                // NICカードを再表示
                DisplayNICCards();
            }
            else
            {
                // flowLayoutPanelが初期化されていない場合、DisplayNICCardsを呼び出して初期化する
                DisplayNICCards();
            }
        }


    }


}
