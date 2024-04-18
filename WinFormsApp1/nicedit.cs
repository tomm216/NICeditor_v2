using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public class NICEditForm : Form
    {
        private Label nameLabel;
        private Label ipLabel;
        private Label subnetMaskLabel;
        private Label defaultGatewayLabel;
        private Label dnsLabel;

        public NICEditForm(string nicName, string ipAddress, string subnetMask, string defaultGateway, string[] dnsServers)
        {
            InitializeComponents();
            DisplayNICInfo(nicName, ipAddress, subnetMask, defaultGateway, dnsServers);
        }

        private void InitializeComponents()
        {
            this.Text = "NIC Details";
            this.Size = new System.Drawing.Size(400, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Arial", 10, FontStyle.Regular);

            nameLabel = CreateLabel("NIC Name:");
            ipLabel = CreateLabel("IP Address:");
            subnetMaskLabel = CreateLabel("Subnet Mask:");
            defaultGatewayLabel = CreateLabel("Default Gateway:");
            dnsLabel = CreateLabel("DNS Servers:");

            ArrangeLabels();
        }

        private Label CreateLabel(string labelText)
        {
            return new Label
            {
                Text = labelText,
                AutoSize = true,
                ForeColor = Color.FromArgb(64, 64, 64),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
        }

        private void ArrangeLabels()
        {
            int margin = 20;
            int labelWidth = 120;
            int labelHeight = 20;

            nameLabel.Location = new Point(margin, margin);
            ipLabel.Location = new Point(margin, nameLabel.Bottom + margin);
            subnetMaskLabel.Location = new Point(margin, ipLabel.Bottom + margin);
            defaultGatewayLabel.Location = new Point(margin, subnetMaskLabel.Bottom + margin);
            dnsLabel.Location = new Point(margin, defaultGatewayLabel.Bottom + margin);

            Controls.AddRange(new[] { nameLabel, ipLabel, subnetMaskLabel, defaultGatewayLabel, dnsLabel });
        }

        private void DisplayNICInfo(string nicName, string ipAddress, string subnetMask, string defaultGateway, string[] dnsServers)
        {
            nameLabel.Text += " " + nicName;
            ipLabel.Text += " " + ipAddress;
            subnetMaskLabel.Text += " " + subnetMask;
            defaultGatewayLabel.Text += " " + defaultGateway;
            dnsLabel.Text += " " + string.Join(", ", dnsServers);
        }
    }
}
