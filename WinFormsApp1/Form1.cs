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
            DisplayNICCards(); // NIC�J�[�h��\�����郁�\�b�h���Ăяo��
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // �t�H�[���̃��[�h���̏���������
            // ���x���̃e�L�X�g��ݒ�
            label1.Text = "NIC�ꗗ�\��";

            // �t�H�[���̃��[�h���Ƀ��j���[���ڂ�ǉ�����
            // �t�@�C�����j���[���쐬
            ToolStripMenuItem fileMenuItem = new ToolStripMenuItem("�t�@�C��");

            // �t�@�C�����j���[�ɃT�u���j���[��ǉ�
            ToolStripMenuItem newMenuItem = new ToolStripMenuItem("�V�K�쐬");
            ToolStripMenuItem openMenuItem = new ToolStripMenuItem("�J��");
            ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("�ۑ�");
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("�I��");

            // �T�u���j���[�ɃC�x���g�n���h����ǉ�
            newMenuItem.Click += NewMenuItem_Click;
            openMenuItem.Click += OpenMenuItem_Click;
            saveMenuItem.Click += SaveMenuItem_Click;
            exitMenuItem.Click += ExitMenuItem_Click;

            // �T�u���j���[���t�@�C�����j���[�ɒǉ�
            fileMenuItem.DropDownItems.Add(newMenuItem);
            fileMenuItem.DropDownItems.Add(openMenuItem);
            fileMenuItem.DropDownItems.Add(saveMenuItem);
            fileMenuItem.DropDownItems.Add(exitMenuItem);

            // ���j���[�X�g���b�v�Ƀt�@�C�����j���[��ǉ�
            menuStrip1.Items.Add(fileMenuItem);
        }

        private void NewMenuItem_Click(object? sender, EventArgs e)
        {
            // �V�K�쐬���j���[���ڂ��N���b�N���ꂽ�Ƃ��̏���
        }

        private void OpenMenuItem_Click(object? sender, EventArgs e)
        {
            // �J�����j���[���ڂ��N���b�N���ꂽ�Ƃ��̏���
        }

        private void SaveMenuItem_Click(object? sender, EventArgs e)
        {
            // �ۑ����j���[���ڂ��N���b�N���ꂽ�Ƃ��̏���
        }

        private void ExitMenuItem_Click(object? sender, EventArgs e)
        {
            // �I�����j���[���ڂ��N���b�N���ꂽ�Ƃ��̏���

            // ���[�U�[�Ɋm�F�p�̃_�C�A���O�{�b�N�X��\�����AOK���I�����ꂽ�ꍇ�ɃA�v���P�[�V�������I������
            DialogResult result = MessageBox.Show("�A�v���P�[�V�������I�����܂����H", "�I���m�F", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                Application.Exit();
            }
        }


        // NIC�J�[�h�p�̃J�X�^���R���g���[��
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

                // NIC�̏���\�����郉�x�����쐬
                Label nameLabel = new Label();
                nameLabel.Text = "Name: " + nicName;
                nameLabel.Location = new Point(5, 5);
                this.Controls.Add(nameLabel);

                // NIC��IP�A�h���X��\�����郉�x�����쐬
                Label ipLabel = new Label();
                ipLabel.Text = "IP: " + ipAddress;
                ipLabel.Location = new Point(5, 30);
                ipLabel.AutoSize = true; // �����T�C�Y������L���ɂ���
                this.Controls.Add(ipLabel);

                // NIC�̃A�C�R����\��
                PictureBox iconPictureBox = new PictureBox();
                iconPictureBox.Image = Image.FromFile("C:\\Users\\t0mm\\source\\repos\\WinFormsApp1\\WinFormsApp1\\Resource\\NIC_icon.png"); // �摜�t�@�C���̃p�X���w��
                iconPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                iconPictureBox.Size = new Size(140, 45); // �A�C�R���̃T�C�Y�𒲐�
                iconPictureBox.Location = new Point(5, 50);
                this.Controls.Add(iconPictureBox);

                // �J�[�h�S�̂Ƀ}�E�X�C�x���g�̃n���h���[��ǉ�
                this.MouseEnter += NICCard_MouseEnter;
                this.MouseLeave += NICCard_MouseLeave;
            }

            private void NICCard_MouseEnter(object? sender, EventArgs e)
            {
                // �J�[�h�S�̂Ƀ}�E�X�����������̏���
                this.BackColor = hoverColor;
            }

            private void NICCard_MouseLeave(object? sender, EventArgs e)
            {
                // �J�[�h�S�̂���}�E�X���o�����̏���
                this.BackColor = string.IsNullOrEmpty(ipAddress) ? missingColor : normalColor;
            }
        }

        private void DisplayNICCards()
        {
            // FlowLayoutPanel���쐬
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight; // ������E�ւ̃t���[
            flowLayoutPanel.AutoScroll = true; // �����X�N���[���L��
            flowLayoutPanel.Dock = DockStyle.Fill; // �e�R���g���[�������ς��ɍL����悤�ɐݒ�
            flowLayoutPanel.Padding = new Padding(0, 80, 0, 0); // �㕔�̗]����ǉ�
            flowLayoutPanel.BackColor = Color.Transparent; // �e�R���g���[���̔w�i�F�𓧖��ɐݒ�

            // �f�o�C�X�ɐڑ�����Ă���NIC�̈ꗗ���擾
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            // NIC�̈ꗗ��\��
            foreach (NetworkInterface nic in nics)
            {
                // NIC�̖��O���擾
                string nicName = nic.Description;

                // NIC��IP�A�h���X���擾
                string ipAddress = GetIPAddress(nic);

                // NIC�J�[�h���쐬����FlowLayoutPanel�ɒǉ�
                NICCard nicCard = new NICCard(nicName, ipAddress);
                flowLayoutPanel.Controls.Add(nicCard);
            }

            // Form��FlowLayoutPanel��ǉ�
            this.Controls.Add(flowLayoutPanel);
        }

        private string GetIPAddress(NetworkInterface nic)
        {
            // �l�b�g���[�N�C���^�[�t�F�[�X����IP�A�h���X���擾
            var ipProps = nic.GetIPProperties();
            var ipAddresses = ipProps.UnicastAddresses;

            // �ŏ���IPv4�A�h���X���擾�iIPv6���܂܂��ꍇ�A�K�؂Ƀt�B���^�����O����K�v������܂��j
            foreach (var ipAddress in ipAddresses)
            {
                if (ipAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ipAddress.Address.ToString();
                }
            }

            // IP�A�h���X��������Ȃ������ꍇ�A�󕶎����Ԃ�
            return "";
        }

    }
}
