using System;
using System.Drawing;
using System.Windows.Forms;
using System.Management;

namespace WinFormsApp1
{
    public class NICEditForm : Form
    {
        private TableLayoutPanel? tableLayout; // Null 許容フィールドとして宣言
        private Label titleLabel; // NIC設定履歴タイトル用のラベル

        private Button? saveButton; // Null 許容フィールドとして宣言
        private TextBox? nameTextBox; // Null 許容フィールドとして宣言
        private TextBox? ipTextBox; // Null 許容フィールドとして宣言
        private TextBox? subnetMaskTextBox; // Null 許容フィールドとして宣言
        private TextBox? defaultGatewayTextBox; // Null 許容フィールドとして宣言
        private TextBox? primaryDnsTextBox; // Null 許容フィールドとして宣言
        private TextBox? alternateDnsTextBox; // Null 許容フィールドとして宣言

        private Panel? historyPanel; // Null 許容フィールドとして宣言
        private ListBox? historyListBox; // Null 許容フィールドとして宣言

        public NICEditForm(string nicName, string ipAddress, string subnetMask, string defaultGateway, string[] dnsServers)
        {
            InitializeComponents();
            DisplayNICInfo(nicName, ipAddress, subnetMask, defaultGateway, dnsServers.Length > 0 ? dnsServers[0] : "", dnsServers.Length > 1 ? dnsServers[1] : "");
            this.Resize += NICEditForm_Resize; // リサイズイベントを有効化
        }

        private void NICEditForm_Resize(object sender, EventArgs e)
        {
            AdjustPanelSize();
        }

        private void InitializeComponents()
        {
            this.Text = "NIC 設定変更";
            this.Size = new System.Drawing.Size(500, 500); // サイズを適切に調整してください
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Arial", 10, FontStyle.Regular);

            tableLayout = new TableLayoutPanel();
            tableLayout.Dock = DockStyle.Fill;
            tableLayout.Padding = new Padding(10);
            tableLayout.ColumnCount = 2;
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // 列1: ラベル
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // 列2: データ

            this.Controls.Add(tableLayout);

            // NIC設定変更のタイトル
            Label titleLabel = new Label();
            titleLabel.Text = "NIC 設定変更";
            titleLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            titleLabel.Dock = DockStyle.Top;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.AutoSize = true;
            tableLayout.Controls.Add(titleLabel);
            tableLayout.SetColumnSpan(titleLabel, 2);

            // ラベルとテキストボックスを追加
            nameTextBox = CreateTextBox();
            ipTextBox = CreateTextBox();
            subnetMaskTextBox = CreateTextBox();
            defaultGatewayTextBox = CreateTextBox();
            primaryDnsTextBox = CreateTextBox();
            alternateDnsTextBox = CreateTextBox();

            AddRowWithTextBox("NIC 名:", nameTextBox);
            AddRowWithTextBox("IP アドレス:", ipTextBox);
            AddRowWithTextBox("サブネット マスク:", subnetMaskTextBox);
            AddRowWithTextBox("デフォルト ゲートウェイ:", defaultGatewayTextBox);
            AddRowWithTextBox("プライマリ DNS サーバー:", primaryDnsTextBox);
            AddRowWithTextBox("代替 DNS サーバー:", alternateDnsTextBox);

            saveButton = new Button();
            saveButton.Text = "保存";
            saveButton.Font = new Font("Arial", 10, FontStyle.Bold);
            saveButton.Click += SaveButton_Click;

            tableLayout.Controls.Add(saveButton);
            tableLayout.SetColumnSpan(saveButton, 2);

            // NIC変更履歴一覧のパネルとリストボックスを追加
            historyPanel = new Panel();
            historyPanel.Dock = DockStyle.Bottom;
            historyPanel.Height = 150; // 適切なサイズに調整してください

            Label historyLabel = new Label();
            historyLabel.Text = "NIC 変更履歴一覧";
            historyLabel.Dock = DockStyle.Top;
            historyLabel.TextAlign = ContentAlignment.MiddleCenter;
            historyLabel.Font = new Font("Arial", 10, FontStyle.Bold);

            historyListBox = new ListBox();
            historyListBox.Dock = DockStyle.Fill;
            historyPanel.Controls.Add(historyLabel);
            historyPanel.Controls.Add(historyListBox);

            this.Controls.Add(historyPanel);

            // NIC変更履歴一覧の下に罫線を引くためのパネルを作成
            Panel borderPanel = new Panel();
            borderPanel.Height = 1;
            borderPanel.Dock = DockStyle.Bottom;
            borderPanel.BackColor = SystemColors.ControlDark; // 罫線の色を設定

            this.Controls.Add(borderPanel);

            // Null 参照の可能性があるもののチェックを追加
            if (tableLayout != null)
            {
                AdjustPanelSize();
            }
        }



        private void AddRowWithTextBox(string label, TextBox textBox)
        {
            Label labelCtrl = new Label();
            labelCtrl.Text = label;
            labelCtrl.AutoSize = true;
            labelCtrl.TextAlign = ContentAlignment.MiddleLeft;

            tableLayout.Controls.Add(labelCtrl);

            tableLayout.Controls.Add(textBox);
        }

        private void DisplayNICInfo(string nicName, string ipAddress, string subnetMask, string defaultGateway, string primaryDnsServer, string alternateDnsServer)
        {
            // ラベルに表示する内容を更新
            nameTextBox.Text = nicName;
            ipTextBox.Text = ipAddress;
            subnetMaskTextBox.Text = subnetMask;
            defaultGatewayTextBox.Text = defaultGateway;
            primaryDnsTextBox.Text = primaryDnsServer;
            alternateDnsTextBox.Text = alternateDnsServer;
        }

        private void AdjustPanelSize()
        {
            // Null 参照の可能性があるもののチェックを追加
            if (tableLayout != null)
            {
                // ボタンを含むすべてのコントロールの高さを取得
                int totalHeight = tableLayout.GetPreferredSize(Size.Empty).Height;

                // フォームの高さを調整
                this.Height = totalHeight + 200; // パネルの高さも考慮して調整してください

                // テーブルレイアウトパネルの高さを調整
                tableLayout.Height = totalHeight;

                // ラベルの幅を計算
                int labelWidth = (int)(tableLayout.Width * 0.3); // ラベルの幅はテーブルレイアウトパネルの幅の30%とします

                // ラベルの幅を適用
                foreach (Control control in tableLayout.Controls)
                {
                    if (control is Label)
                    {
                        control.Width = labelWidth;
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string newName = nameTextBox.Text;
            string newIp = ipTextBox.Text;
            string newSubnetMask = subnetMaskTextBox.Text;
            string newDefaultGateway = defaultGatewayTextBox.Text;
            string newPrimaryDnsServer = primaryDnsTextBox.Text;
            string newAlternateDnsServer = alternateDnsTextBox.Text;

            ChangeNetworkSettings(newName, newIp, newSubnetMask, newDefaultGateway, newPrimaryDnsServer, newAlternateDnsServer);
        }

        private void ChangeNetworkSettings(string nicName, string newIp, string newSubnetMask, string newDefaultGateway, string newPrimaryDnsServer, string newAlternateDnsServer)
        {
            string query = $"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Description = '{nicName}'";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject obj in searcher.Get())
            {
                ManagementBaseObject newIP = obj.GetMethodParameters("EnableStatic");
                newIP["IPAddress"] = new string[] { newIp };
                newIP["SubnetMask"] = new string[] { newSubnetMask };
                obj.InvokeMethod("EnableStatic", newIP, null);

                ManagementBaseObject newGateway = obj.GetMethodParameters("SetGateways");
                newGateway["DefaultIPGateway"] = new string[] { newDefaultGateway };
                obj.InvokeMethod("SetGateways", newGateway, null);

                ManagementBaseObject newDNS = obj.GetMethodParameters("SetDNSServerSearchOrder");
                newDNS["DNSServerSearchOrder"] = new string[] { newPrimaryDnsServer, newAlternateDnsServer };
                obj.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);

                // リストボックスに履歴を追加
                historyListBox.Items.Add($"NIC: {nicName} - IP: {newIp}, Subnet Mask: {newSubnetMask}, Gateway: {newDefaultGateway}, DNS: {newPrimaryDnsServer}, {newAlternateDnsServer}");

                MessageBox.Show("ネットワーク設定が正常に更新されました。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show("ネットワークインターフェースが見つかりませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private TextBox CreateTextBox()
        {
            TextBox textBox = new TextBox();
            textBox.Font = new Font("Arial", 10, FontStyle.Regular);
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            return textBox;
        }
    }
}
