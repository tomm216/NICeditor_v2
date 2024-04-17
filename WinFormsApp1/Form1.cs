using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private void SetMenuBarBackgroundColor()
        {
            // �V�X�e���̃��j���[�̔w�i�F���擾
            Color systemMenuColor = SystemColors.Menu;

            // ���j���[�o�[�̔w�i�F��ݒ�
            menuStrip1.BackColor = systemMenuColor;
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
    }
}
