using System;
using System.Drawing;
using System.Windows.Forms;
using System.Management;

namespace WinFormsApp1
{
    public class NICEditForm : Form
    {

        private TableLayoutPanel tableLayout;
        private Button saveButton;
        private Panel contentPanel;
        private TextBox nameTextBox;
        private TextBox ipTextBox;
        private TextBox subnetMaskTextBox;
        private TextBox defaultGatewayTextBox;
        private TextBox primaryDnsTextBox;
        private TextBox alternateDnsTextBox;

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
            this.Size = new System.Drawing.Size(500, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Arial", 10, FontStyle.Regular);

            tableLayout = new TableLayoutPanel();
            tableLayout.Dock = DockStyle.Fill;
            tableLayout.Padding = new Padding(10);
            tableLayout.ColumnCount = 2;
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // 列1: ラベル
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // 列2: データ

            this.Controls.Add(tableLayout);

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
            // ボタンを含むすべてのコントロールの高さを取得
            int totalHeight = tableLayout.GetPreferredSize(Size.Empty).Height;

            // フォームの高さを調整
            this.Height = totalHeight + 100;
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
