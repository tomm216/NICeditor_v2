using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private void SetMenuBarBackgroundColor()
        {
            // システムのメニューの背景色を取得
            Color systemMenuColor = SystemColors.Menu;

            // メニューバーの背景色を設定
            menuStrip1.BackColor = systemMenuColor;
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
    }
}
