using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Management;

namespace WinFormsApp1
{
    public class NICEditForm : Form
    {
        private Panel contentPanel;
        private TextBox nameTextBox;
        private TextBox ipTextBox;
        private TextBox subnetMaskTextBox;
        private TextBox defaultGatewayTextBox;
        private TextBox dnsTextBox;
        private Button saveButton;

        public NICEditForm(string nicName, string ipAddress, string subnetMask, string defaultGateway, string[] dnsServers)
        {
            InitializeComponents();
            DisplayNICInfo(nicName, ipAddress, subnetMask, defaultGateway, dnsServers);
        }

        private void InitializeComponents()
        {
            this.Text = "NIC Details";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Arial", 10, FontStyle.Regular);

            contentPanel = new Panel();
            contentPanel.AutoScroll = true;
            contentPanel.Dock = DockStyle.Fill;
            this.Controls.Add(contentPanel);

            Label nameLabel = CreateLabel("NIC Name:", 20);
            nameTextBox = CreateTextBox();
            Label ipLabel = CreateLabel("IP Address:", nameLabel.Bottom + 10);
            ipTextBox = CreateTextBox();
            Label subnetMaskLabel = CreateLabel("Subnet Mask:", ipLabel.Bottom + 10);
            subnetMaskTextBox = CreateTextBox();
            Label defaultGatewayLabel = CreateLabel("Default Gateway:", subnetMaskLabel.Bottom + 10);
            defaultGatewayTextBox = CreateTextBox();
            Label dnsLabel = CreateLabel("DNS Servers:", defaultGatewayLabel.Bottom + 10);
            dnsTextBox = CreateTextBox();

            saveButton = new Button();
            saveButton.Text = "Save";
            saveButton.Font = new Font("Arial", 10, FontStyle.Bold);
            saveButton.Click += SaveButton_Click;

            ArrangeControls(nameLabel, nameTextBox, ipLabel, ipTextBox, subnetMaskLabel, subnetMaskTextBox, defaultGatewayLabel, defaultGatewayTextBox, dnsLabel, dnsTextBox, saveButton);
        }

        private Label CreateLabel(string labelText, int top)
        {
            return new Label
            {
                Text = labelText,
                AutoSize = true,
                ForeColor = Color.FromArgb(64, 64, 64),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(20, top)
            };
        }

        private TextBox CreateTextBox()
        {
            return new TextBox
            {
                Font = new Font("Arial", 10, FontStyle.Regular),
                Size = new Size(250, 25)
            };
        }

        private void ArrangeControls(params Control[] controls)
        {
            int margin = 20;
            for (int i = 0; i < controls.Length; i++)
            {
                controls[i].Location = new Point(margin, margin + i * (controls[i].Height + 10));
                contentPanel.Controls.Add(controls[i]);
            }
        }

        private void DisplayNICInfo(string nicName, string ipAddress, string subnetMask, string defaultGateway, string[] dnsServers)
        {
            nameTextBox.Text = nicName;
            ipTextBox.Text = ipAddress;
            subnetMaskTextBox.Text = subnetMask;
            defaultGatewayTextBox.Text = defaultGateway;
            dnsTextBox.Text = string.Join(", ", dnsServers);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // 新しい設定を取得する
            string newName = nameTextBox.Text;
            string newIp = ipTextBox.Text;
            string newSubnetMask = subnetMaskTextBox.Text;
            string newDefaultGateway = defaultGatewayTextBox.Text;
            string[] newDnsServers = dnsTextBox.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // ネットワーク設定を変更する
            ChangeNetworkSettings(newName, newIp, newSubnetMask, newDefaultGateway, newDnsServers);

            // 成功メッセージを表示する
            MessageBox.Show("Settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ネットワーク設定を変更するメソッド
        private void ChangeNetworkSettings(string nicName, string newIp, string newSubnetMask, string newDefaultGateway, string[] newDnsServers)
        {
            try
            {
                // WMI クエリ文字列を構築する
                string query = $"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Description = '{nicName}'";

                // ManagementObjectSearcher を使用してクエリを実行する
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                foreach (ManagementObject obj in searcher.Get())
                {
                    // IP アドレス、サブネットマスク、デフォルトゲートウェイ、DNS サーバーを変更する
                    ManagementBaseObject newIP = obj.GetMethodParameters("EnableStatic");
                    newIP["IPAddress"] = new string[] { newIp };
                    newIP["SubnetMask"] = new string[] { newSubnetMask };
                    obj.InvokeMethod("EnableStatic", newIP, null);

                    ManagementBaseObject newGateway = obj.GetMethodParameters("SetGateways");
                    newGateway["DefaultIPGateway"] = new string[] { newDefaultGateway };
                    obj.InvokeMethod("SetGateways", newGateway, null);

                    ManagementBaseObject newDNS = obj.GetMethodParameters("SetDNSServerSearchOrder");
                    newDNS["DNSServerSearchOrder"] = newDnsServers;
                    obj.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);

                    // 必要ならば、他のネットワーク設定も変更することができます

                    // ネットワーク設定が正常に変更されたことを示すメッセージを表示する
                    MessageBox.Show("Network settings updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 対象のNICが見つからなかった場合にエラーメッセージを表示する
                MessageBox.Show("Network interface not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // エラーが発生した場合にエラーメッセージを表示する
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
