using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

#nullable disable

namespace DataMahasiswa
{
    public static class UIHelper
    {
        public static GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);

            path.CloseFigure();
            return path;
        }

        public static void DrawRoundedBorder(Graphics g, Rectangle rect, int radius, Color color)
        {
            if (rect.Width <= 2 || rect.Height <= 2) return;

            rect.Width -= 1;
            rect.Height -= 1;

            using (GraphicsPath path = RoundedRect(rect, radius))
            using (Pen pen = new Pen(color, 1))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawPath(pen, path);
            }
        }
    }

    public class MainForm : Form
    {
        private class SmoothButton : Button
        {
            private Color normalColor = Color.FromArgb(24, 68, 110);
            private Color hoverColor = Color.FromArgb(36, 92, 145);
            private Color borderColor = Color.Transparent;
            private bool outline;

            [DefaultValue(typeof(Color), "24, 68, 110")]
            public Color NormalColor
            {
                get => normalColor;
                set { normalColor = value; Invalidate(); }
            }

            [DefaultValue(typeof(Color), "36, 92, 145")]
            public Color HoverColor
            {
                get => hoverColor;
                set { hoverColor = value; Invalidate(); }
            }

            [DefaultValue(typeof(Color), "Transparent")]
            public Color BorderColor
            {
                get => borderColor;
                set { borderColor = value; Invalidate(); }
            }

            [DefaultValue(false)]
            public bool Outline
            {
                get => outline;
                set { outline = value; Invalidate(); }
            }

            private bool hovering;

            public SmoothButton()
            {
                SetStyle(
                    ControlStyles.UserPaint |
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw,
                    true);

                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0;
                BackColor = Color.Transparent;
                Cursor = Cursors.Hand;
                TabStop = false;
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                hovering = true;
                Invalidate();
                base.OnMouseEnter(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                hovering = false;
                Invalidate();
                base.OnMouseLeave(e);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                Color background = Parent == null ? Color.Transparent : Parent.BackColor;
                e.Graphics.Clear(background);

                Rectangle rect = new Rectangle(1, 1, Math.Max(1, Width - 3), Math.Max(1, Height - 3));
                int radius = Math.Min(10, Math.Max(5, Height / 4));

                using (GraphicsPath path = UIHelper.RoundedRect(rect, radius))
                {
                    Color fill = hovering ? HoverColor : NormalColor;
                    using (SolidBrush brush = new SolidBrush(fill))
                    {
                        e.Graphics.FillPath(brush, path);
                    }

                    if (Outline)
                    {
                        using (Pen pen = new Pen(BorderColor, 1.2f))
                        {
                            e.Graphics.DrawPath(pen, path);
                        }
                    }
                }

                TextRenderer.DrawText(
                    e.Graphics,
                    Text,
                    Font,
                    rect,
                    ForeColor,
                    TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.NoPadding |
                    TextFormatFlags.EndEllipsis);
            }
        }

        private readonly List<Mahasiswa> daftarMahasiswa = new List<Mahasiswa>();

        private readonly Color Bg = Color.FromArgb(10, 20, 34);
        private readonly Color Toolbar = Color.FromArgb(13, 29, 48);
        private readonly Color Card = Color.FromArgb(18, 40, 64);
        private readonly Color Card2 = Color.FromArgb(22, 50, 78);
        private readonly Color Border = Color.FromArgb(40, 80, 115);
        private readonly Color Cyan = Color.FromArgb(0, 214, 255);
        private readonly Color Blue = Color.FromArgb(55, 126, 255);
        private readonly Color Green = Color.FromArgb(25, 198, 111);
        private readonly Color Yellow = Color.FromArgb(255, 199, 30);
        private readonly Color Red = Color.FromArgb(239, 75, 75);
        private readonly Color White = Color.FromArgb(242, 247, 252);
        private readonly Color Muted = Color.FromArgb(154, 181, 207);

        private Panel toolbar = null!;
        private Panel contentPanel = null!;
        private Label lblStatus = null!;

        private Label lblTotalMahasiswa = null!;
        private Label lblTotalProdi = null!;
        private Label lblRataIPK = null!;

        private DataGridView dashboardTable = null!;
        private DataGridView dataTable = null!;
        private TextBox txtSearch = null!;

        public MainForm()
        {
            InitializeWindow();
            BuildToolbar();
            BuildContentPanel();

            SeedDummyData();
            ShowDashboard();
        }

        private void SeedDummyData()
        {
            daftarMahasiswa.Clear();
            daftarMahasiswa.Add(new Mahasiswa("5025241038", "Danish Faiq Ibad Yuadi", "Teknik Informatika", 3.85));
            daftarMahasiswa.Add(new Mahasiswa("5025241004", "Budi Santoso", "Teknik Informatika", 3.42));
            daftarMahasiswa.Add(new Mahasiswa("5025241048", "Candra Wijaya", "Teknik Informatika", 3.78));
            daftarMahasiswa.Add(new Mahasiswa("5025241100", "Liem, Alfred Haryanto", "Teknik Informatika", 3.92));
            daftarMahasiswa.Add(new Mahasiswa("5025241012", "Rian Pratama", "Sistem Informasi", 3.60));
        }

        private void InitializeWindow()
        {
            Text = "Sistem Data Mahasiswa";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1500, 900);
            MinimumSize = new Size(1100, 700);
            WindowState = FormWindowState.Maximized;

            BackColor = Bg;
            ForeColor = White;
            FormBorderStyle = FormBorderStyle.Sizable;
            AutoScaleMode = AutoScaleMode.Font;

            Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            DoubleBuffered = true;
        }

        private void BuildToolbar()
        {
            toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 76,
                BackColor = Toolbar
            };
            toolbar.Paint += PaintToolbar;
            Controls.Add(toolbar);

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(20, 0, 20, 0)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            toolbar.Controls.Add(layout);

            FlowLayoutPanel leftPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 14, 0, 0)
            };

            Label logo = new Label
            {
                Text = "🎓",
                Font = new Font("Segoe UI Emoji", 22F),
                ForeColor = Cyan,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(46, 46),
                Margin = new Padding(0, 0, 10, 0)
            };
            leftPanel.Controls.Add(logo);

            FlowLayoutPanel brandPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 25, 0)
            };

            Label brand = new Label
            {
                Text = "DATA MAHASISWA",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = White,
                AutoSize = true,
                Margin = new Padding(0, 2, 0, 0)
            };
            brandPanel.Controls.Add(brand);

            Label brandSub = new Label
            {
                Text = "Student Management System",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Muted,
                AutoSize = true,
                Margin = new Padding(0)
            };
            brandPanel.Controls.Add(brandSub);
            leftPanel.Controls.Add(brandPanel);

            Button btnDashboard = CreateNavButton("⌂  Dashboard", true);
            btnDashboard.Size = new Size(125, 44);
            btnDashboard.Margin = new Padding(0, 2, 6, 0);
            btnDashboard.Click += (s, e) => ShowDashboard();
            leftPanel.Controls.Add(btnDashboard);

            Button btnTambah = CreateNavButton("+  Tambah", false);
            btnTambah.Size = new Size(115, 44);
            btnTambah.Margin = new Padding(0, 2, 6, 0);
            btnTambah.Click += (s, e) => ShowTambahMahasiswa();
            leftPanel.Controls.Add(btnTambah);

            Button btnData = CreateNavButton("▤  Data", false);
            btnData.Size = new Size(105, 44);
            btnData.Margin = new Padding(0, 2, 6, 0);
            btnData.Click += (s, e) => ShowDataMahasiswa();
            leftPanel.Controls.Add(btnData);

            Button btnCari = CreateNavButton("⌕  Cari", false);
            btnCari.Size = new Size(100, 44);
            btnCari.Margin = new Padding(0, 2, 6, 0);
            btnCari.Click += (s, e) =>
            {
                ShowDataMahasiswa();
                txtSearch?.Focus();
            };
            leftPanel.Controls.Add(btnCari);
            layout.Controls.Add(leftPanel, 0, 0);

            FlowLayoutPanel rightPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 16, 0, 0)
            };

            Button btnExit = CreateOutlineButton("↪  Keluar", Red);
            btnExit.Size = new Size(100, 42);
            btnExit.Margin = new Padding(6, 0, 0, 0);
            btnExit.Click += (s, e) =>
            {
                if (MessageBox.Show("Yakin ingin keluar dari aplikasi?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Close();
            };
            rightPanel.Controls.Add(btnExit);

            Button btnRefresh = CreateOutlineButton("⟳  Refresh", Cyan);
            btnRefresh.Size = new Size(105, 42);
            btnRefresh.Margin = new Padding(6, 0, 0, 0);
            btnRefresh.Click += (s, e) =>
            {
                RefreshDashboard();
                if (dataTable != null) LoadDataTable("");
            };
            rightPanel.Controls.Add(btnRefresh);

            lblStatus = new Label
            {
                Text = "●  SYSTEM ONLINE",
                ForeColor = Green,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 14, 16, 0)
            };
            rightPanel.Controls.Add(lblStatus);
            layout.Controls.Add(rightPanel, 1, 0);
        }

        private void BuildContentPanel()
        {
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Bg,
                Padding = new Padding(28, 22, 28, 22),
                AutoScroll = true
            };
            Controls.Add(contentPanel);
            contentPanel.BringToFront();
        }

        private void ShowDashboard()
        {
            ClearContent();

            TableLayoutPanel page = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Bg,
                ColumnCount = 1,
                RowCount = 5
            };
            page.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            page.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            page.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            page.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            page.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentPanel.Controls.Add(page);

            Panel header = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            Panel accent = new Panel { BackColor = Cyan, Size = new Size(5, 62), Location = new Point(0, 6) };
            header.Controls.Add(accent);

            Label title = new Label
            {
                Text = "Sistem Data Mahasiswa",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = White,
                AutoSize = true,
                Location = new Point(22, 4)
            };
            header.Controls.Add(title);

            Label subtitle = new Label
            {
                Text = "Kelola, cari, dan pantau data mahasiswa dengan mudah.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Muted,
                AutoSize = true,
                Location = new Point(24, 48)
            };
            header.Controls.Add(subtitle);
            page.Controls.Add(header, 0, 0);

            TableLayoutPanel stats = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 6, 0, 8)
            };
            stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            page.Controls.Add(stats, 0, 1);

            Panel card1 = CreateStatCard("TOTAL MAHASISWA", "0", "👥", Cyan, "Mahasiswa terdaftar");
            Panel card2 = CreateStatCard("PROGRAM STUDI", "0", "▣", Blue, "Program studi tersedia");
            Panel card3 = CreateStatCard("RATA-RATA IPK", "0.00", "★", Yellow, "Rata-rata keseluruhan");

            stats.Controls.Add(WrapCard(card1, 6), 0, 0);
            stats.Controls.Add(WrapCard(card2, 6), 1, 0);
            stats.Controls.Add(WrapCard(card3, 6), 2, 0);

            lblTotalMahasiswa = FindLabel(card1, "STAT_VALUE");
            lblTotalProdi = FindLabel(card2, "STAT_VALUE");
            lblRataIPK = FindLabel(card3, "STAT_VALUE");

            TableLayoutPanel action = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(0, 6, 0, 6)
            };
            action.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            action.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            action.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            action.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F));
            page.Controls.Add(action, 0, 2);

            Panel search = CreateSearchBox("Cari NIM, nama, atau program studi...");
            search.Dock = DockStyle.Fill;
            search.Margin = new Padding(0, 0, 12, 0);
            action.Controls.Add(search, 0, 0);

            Button btnLihat = CreateSolidButton("◉  Lihat Semua", Blue);
            btnLihat.Dock = DockStyle.Fill;
            btnLihat.Margin = new Padding(0, 0, 10, 0);
            btnLihat.Click += (s, e) => ShowDataMahasiswa();
            action.Controls.Add(btnLihat, 1, 0);

            Button btnTambah = CreateSolidButton("+  Tambah", Green);
            btnTambah.Dock = DockStyle.Fill;
            btnTambah.Margin = new Padding(0);
            btnTambah.Click += (s, e) => ShowTambahMahasiswa();
            action.Controls.Add(btnTambah, 2, 0);

            TextBox dashboardSearch = FindTextBox(search);
            if (dashboardSearch != null)
            {
                dashboardSearch.TextChanged += (s, e) =>
                {
                    string keyword = dashboardSearch.Text;
                    if (keyword == Convert.ToString(dashboardSearch.Tag)) keyword = "";
                    LoadDashboardTable(keyword);
                };
            }

            Panel section = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            Label icon = new Label
            {
                Text = "▤",
                Font = new Font("Segoe UI Symbol", 16F, FontStyle.Bold),
                ForeColor = Cyan,
                AutoSize = true,
                Location = new Point(0, 10)
            };
            section.Controls.Add(icon);

            Label sectionTitle = new Label
            {
                Text = "Data Mahasiswa",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = White,
                AutoSize = true,
                Location = new Point(28, 13)
            };
            section.Controls.Add(sectionTitle);
            page.Controls.Add(section, 0, 3);

            Panel tableHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 4, 0, 4)
            };
            page.Controls.Add(tableHost, 0, 4);

            dashboardTable = CreateStudentTable();
            dashboardTable.Dock = DockStyle.Fill;
            dashboardTable.CellContentClick += DashboardTable_CellContentClick;
            tableHost.Controls.Add(dashboardTable);

            RefreshDashboard();
        }

        private Panel CreateStatCard(string title, string value, string icon, Color accentColor, string description)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Card,
                Tag = "STAT_CARD"
            };
            panel.Paint += (sender, e) =>
            {
                UIHelper.DrawRoundedBorder(e.Graphics, panel.ClientRectangle, 10, Border);
                using (SolidBrush brush = new SolidBrush(accentColor))
                {
                    e.Graphics.FillRectangle(brush, 0, 0, 5, panel.Height);
                }
            };

            TableLayoutPanel inner = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(16, 8, 16, 8)
            };
            inner.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
            inner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panel.Controls.Add(inner);

            Label iconLabel = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI Emoji", 22F),
                ForeColor = accentColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            inner.Controls.Add(iconLabel, 0, 0);

            TableLayoutPanel textStack = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 3
            };
            textStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            textStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            textStack.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Muted,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            textStack.Controls.Add(titleLabel, 0, 0);

            Label valueLabel = new Label
            {
                Name = "STAT_VALUE",
                Text = value,
                Font = new Font("Segoe UI", 21F, FontStyle.Bold),
                ForeColor = White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            textStack.Controls.Add(valueLabel, 0, 1);

            Label descLabel = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Muted,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft
            };
            textStack.Controls.Add(descLabel, 0, 2);

            inner.Controls.Add(textStack, 1, 0);
            return panel;
        }

        private void ShowDataMahasiswa()
        {
            ClearContent();

            TableLayoutPanel page = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Bg,
                ColumnCount = 1,
                RowCount = 3
            };
            page.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            page.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            page.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentPanel.Controls.Add(page);

            Panel header = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            Label title = new Label
            {
                Text = "Data Mahasiswa",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = White,
                AutoSize = true,
                Location = new Point(0, 4)
            };
            header.Controls.Add(title);

            Label subtitle = new Label
            {
                Text = "Kelola seluruh data mahasiswa dalam satu halaman.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Muted,
                AutoSize = true,
                Location = new Point(2, 48)
            };
            header.Controls.Add(subtitle);
            page.Controls.Add(header, 0, 0);

            TableLayoutPanel action = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(0, 6, 0, 6)
            };
            action.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            action.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 185F));
            page.Controls.Add(action, 0, 1);

            Panel search = CreateSearchBox("Cari NIM, nama, atau program studi...");
            search.Dock = DockStyle.Fill;
            search.Margin = new Padding(0, 0, 12, 0);
            action.Controls.Add(search, 0, 0);

            Button add = CreateSolidButton("+  Tambah Mahasiswa", Green);
            add.Dock = DockStyle.Fill;
            add.Margin = new Padding(0);
            add.Click += (s, e) => ShowTambahMahasiswa();
            action.Controls.Add(add, 1, 0);

            txtSearch = FindTextBox(search);
            if (txtSearch != null)
            {
                txtSearch.TextChanged += (s, e) =>
                {
                    string keyword = txtSearch.Text;
                    if (keyword == Convert.ToString(txtSearch.Tag)) keyword = "";
                    LoadDataTable(keyword);
                };
            }

            Panel tableHost = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 4, 0, 4)
            };
            page.Controls.Add(tableHost, 0, 2);

            dataTable = CreateStudentTable();
            dataTable.Dock = DockStyle.Fill;
            dataTable.CellContentClick += DataTable_CellContentClick;
            tableHost.Controls.Add(dataTable);

            LoadDataTable("");
        }

        private void ShowTambahMahasiswa()
        {
            ClearContent();

            TableLayoutPanel page = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Bg,
                ColumnCount = 1,
                RowCount = 3
            };
            page.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            page.RowStyles.Add(new RowStyle(SizeType.Absolute, 420F));
            page.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentPanel.Controls.Add(page);

            Panel header = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            Label title = new Label
            {
                Text = "Tambah Mahasiswa",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = White,
                AutoSize = true,
                Location = new Point(0, 4)
            };
            header.Controls.Add(title);

            Label subtitle = new Label
            {
                Text = "Masukkan informasi mahasiswa baru dengan lengkap.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Muted,
                AutoSize = true,
                Location = new Point(2, 48)
            };
            header.Controls.Add(subtitle);
            page.Controls.Add(header, 0, 0);

            Panel formCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Card,
                Padding = new Padding(30, 24, 30, 24),
                Margin = new Padding(0, 0, 0, 10)
            };
            formCard.Paint += (sender, e) => UIHelper.DrawRoundedBorder(e.Graphics, formCard.ClientRectangle, 10, Border);
            page.Controls.Add(formCard, 0, 1);

            TableLayoutPanel formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 200,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = Color.Transparent
            };
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            formCard.Controls.Add(formLayout);

            Panel nimPanel = CreateFormField("NIM", out TextBox txtNim);
            formLayout.Controls.Add(nimPanel, 0, 0);

            Panel namaPanel = CreateFormField("Nama Lengkap", out TextBox txtNama);
            formLayout.Controls.Add(namaPanel, 1, 0);

            Panel prodiPanel = CreateFormField("Program Studi", out TextBox txtProdi);
            formLayout.Controls.Add(prodiPanel, 0, 1);

            Panel ipkPanel = CreateFormField("IPK", out TextBox txtIpk);
            formLayout.Controls.Add(ipkPanel, 1, 1);

            Label info = new Label
            {
                Text = "ℹ  IPK harus berada pada rentang 0.00 sampai 4.00.",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Muted,
                AutoSize = true,
                Location = new Point(30, 215)
            };
            formCard.Controls.Add(info);

            FlowLayoutPanel buttons = new FlowLayoutPanel
            {
                Location = new Point(30, 260),
                Size = new Size(700, 55),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };
            formCard.Controls.Add(buttons);

            Button btnSimpan = CreateSolidButton("✓  Simpan Data", Green);
            btnSimpan.Size = new Size(155, 46);
            btnSimpan.Margin = new Padding(0, 0, 10, 0);
            btnSimpan.Click += (s, e) => TambahMahasiswa(txtNim.Text.Trim(), txtNama.Text.Trim(), txtProdi.Text.Trim(), txtIpk.Text.Trim());
            buttons.Controls.Add(btnSimpan);

            Button btnReset = CreateOutlineButton("↺  Reset", Blue);
            btnReset.Size = new Size(120, 46);
            btnReset.Margin = new Padding(0, 0, 10, 0);
            btnReset.Click += (s, e) =>
            {
                txtNim.Clear();
                txtNama.Clear();
                txtProdi.Clear();
                txtIpk.Clear();
                txtNim.Focus();
            };
            buttons.Controls.Add(btnReset);

            Button btnKembali = CreateOutlineButton("←  Kembali", Cyan);
            btnKembali.Size = new Size(130, 46);
            btnKembali.Margin = new Padding(0);
            btnKembali.Click += (s, e) => ShowDataMahasiswa();
            buttons.Controls.Add(btnKembali);
        }

        private Panel CreateFormField(string labelText, out TextBox textbox)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 6, 20, 6),
                BackColor = Color.Transparent
            };

            Label label = CreateFormLabel(labelText);
            label.Dock = DockStyle.Top;
            label.Height = 24;
            panel.Controls.Add(label);

            textbox = CreateInput();
            textbox.Dock = DockStyle.Top;
            textbox.Height = 38;
            panel.Controls.Add(textbox);

            return panel;
        }

        private DataGridView CreateStudentTable()
        {
            DataGridView grid = new DataGridView
            {
                BackgroundColor = Card,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(28, 59, 87),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                AutoGenerateColumns = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 48,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ScrollBars = ScrollBars.Both
            };
            grid.RowTemplate.Height = 48;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(16, 48, 78);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Cyan;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            grid.DefaultCellStyle.BackColor = Card;
            grid.DefaultCellStyle.ForeColor = White;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(26, 67, 102);
            grid.DefaultCellStyle.SelectionForeColor = White;
            grid.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(11, 29, 49);

            AddTextColumn(grid, "No", "No", 45, 40);
            AddTextColumn(grid, "NIM", "NIM", 110, 90);
            AddTextColumn(grid, "Nama", "Nama", 180, 180);
            AddTextColumn(grid, "Program Studi", "Prodi", 200, 200);
            AddTextColumn(grid, "IPK", "IPK", 70, 60);

            AddButtonColumn(grid, "Aksi", "Edit", 90, "✎  Edit", Yellow);
            AddButtonColumn(grid, " ", "Hapus", 95, "▢  Hapus", Red);

            // Mendaftarkan event agar warna selang-seling tidak menimpa warna tombol
            grid.CellFormatting += DataGridView_CellFormatting;

            return grid;
        }

        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid == null || e.RowIndex < 0) return;

            if (grid.Columns[e.ColumnIndex].Name == "Edit")
            {
                e.CellStyle.BackColor = Yellow;
                e.CellStyle.SelectionBackColor = Yellow;
                e.CellStyle.ForeColor = Color.FromArgb(30, 30, 30);
                e.CellStyle.SelectionForeColor = Color.FromArgb(30, 30, 30);
            }
            else if (grid.Columns[e.ColumnIndex].Name == "Hapus")
            {
                e.CellStyle.BackColor = Red;
                e.CellStyle.SelectionBackColor = Red;
                e.CellStyle.ForeColor = White;
                e.CellStyle.SelectionForeColor = White;
            }
        }

        private void AddTextColumn(DataGridView grid, string header, string property, int width, int fillWeight)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn
            {
                HeaderText = header,
                Name = property,
                DataPropertyName = property,
                Width = width,
                MinimumWidth = Math.Min(width, 60),
                SortMode = DataGridViewColumnSortMode.NotSortable,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = fillWeight
            };
            grid.Columns.Add(col);
        }

        private void AddButtonColumn(DataGridView grid, string header, string name, int width, string text, Color color)
        {
            DataGridViewButtonColumn col = new DataGridViewButtonColumn
            {
                HeaderText = header,
                Name = name,
                Width = width,
                MinimumWidth = Math.Min(width, 80),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = width,
                Text = text,
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            DataGridViewCellStyle style = new DataGridViewCellStyle
            {
                BackColor = color,
                ForeColor = color == Yellow ? Color.FromArgb(30, 30, 30) : White,
                SelectionBackColor = color,
                SelectionForeColor = color == Yellow ? Color.FromArgb(30, 30, 30) : White,
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Padding = new Padding(0)
            };
            col.DefaultCellStyle = style;
            grid.Columns.Add(col);
        }

        private void LoadDataTable(string keyword)
        {
            if (dataTable == null) return;

            IEnumerable<Mahasiswa> data = daftarMahasiswa;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string key = keyword.Trim().ToLower();
                data = data.Where(m =>
                    (m.NIM ?? "").ToLower().Contains(key) ||
                    (m.Nama ?? "").ToLower().Contains(key) ||
                    (m.Prodi ?? "").ToLower().Contains(key));
            }

            dataTable.Rows.Clear();
            int no = 1;

            foreach (Mahasiswa m in data)
            {
                int row = dataTable.Rows.Add();
                dataTable.Rows[row].Cells["No"].Value = no++;
                dataTable.Rows[row].Cells["NIM"].Value = m.NIM;
                dataTable.Rows[row].Cells["Nama"].Value = m.Nama;
                dataTable.Rows[row].Cells["Prodi"].Value = m.Prodi;
                dataTable.Rows[row].Cells["IPK"].Value = m.IPK.ToString("0.00");
            }

            Label footer = FindLabelByName("DATA_FOOTER");
            if (footer != null)
            {
                footer.Text = $"Menampilkan {data.Count()} dari {daftarMahasiswa.Count} data mahasiswa";
            }
        }

        private void LoadDashboardTable(string keyword = "")
        {
            if (dashboardTable == null) return;

            dashboardTable.Rows.Clear();
            IEnumerable<Mahasiswa> source = daftarMahasiswa;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string key = keyword.Trim().ToLower();
                source = source.Where(m =>
                    (m.NIM ?? "").ToLower().Contains(key) ||
                    (m.Nama ?? "").ToLower().Contains(key) ||
                    (m.Prodi ?? "").ToLower().Contains(key));
            }

            List<Mahasiswa> data = source.OrderByDescending(m => m.IPK).Take(5).ToList();
            int no = 1;

            foreach (Mahasiswa m in data)
            {
                int row = dashboardTable.Rows.Add();
                dashboardTable.Rows[row].Cells["No"].Value = no++;
                dashboardTable.Rows[row].Cells["NIM"].Value = m.NIM;
                dashboardTable.Rows[row].Cells["Nama"].Value = m.Nama;
                dashboardTable.Rows[row].Cells["Prodi"].Value = m.Prodi;
                dashboardTable.Rows[row].Cells["IPK"].Value = m.IPK.ToString("0.00");
            }

            Label footer = FindLabelByName("DASHBOARD_FOOTER");
            if (footer != null)
            {
                footer.Text = $"Menampilkan {data.Count} mahasiswa dengan IPK tertinggi";
            }
        }

        private void RefreshDashboard()
        {
            if (lblTotalMahasiswa != null)
                lblTotalMahasiswa.Text = daftarMahasiswa.Count.ToString();

            if (lblTotalProdi != null)
            {
                lblTotalProdi.Text = daftarMahasiswa
                    .Select(m => m.Prodi)
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count()
                    .ToString();
            }

            if (lblRataIPK != null)
            {
                double avg = daftarMahasiswa.Count == 0 ? 0 : daftarMahasiswa.Average(m => m.IPK);
                lblRataIPK.Text = avg.ToString("0.00");
            }

            LoadDashboardTable("");
        }

        private void TambahMahasiswa(string nim, string nama, string prodi, string ipkText)
        {
            if (string.IsNullOrWhiteSpace(nim) || string.IsNullOrWhiteSpace(nama) ||
                string.IsNullOrWhiteSpace(prodi) || string.IsNullOrWhiteSpace(ipkText))
            {
                MessageBox.Show("Semua data harus diisi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (daftarMahasiswa.Any(m => string.Equals(m.NIM, nim, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("NIM tersebut sudah terdaftar.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(ipkText.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double ipk) || ipk < 0 || ipk > 4)
            {
                MessageBox.Show("IPK harus berupa angka rentang 0.00 - 4.00.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            daftarMahasiswa.Add(new Mahasiswa(nim, nama, prodi, ipk));
            MessageBox.Show("Data mahasiswa berhasil ditambahkan.", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowDataMahasiswa();
        }

        private void DataTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == dataTable.Columns["Hapus"].Index) HapusBaris(dataTable, e.RowIndex);
            else if (e.ColumnIndex == dataTable.Columns["Edit"].Index) EditBaris(dataTable, e.RowIndex);
        }

        private void DashboardTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == dashboardTable.Columns["Hapus"].Index) HapusBaris(dashboardTable, e.RowIndex);
            else if (e.ColumnIndex == dashboardTable.Columns["Edit"].Index) EditBaris(dashboardTable, e.RowIndex);
        }

        private void HapusBaris(DataGridView grid, int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= grid.Rows.Count) return;

            string nim = Convert.ToString(grid.Rows[rowIndex].Cells["NIM"].Value);
            Mahasiswa item = daftarMahasiswa.FirstOrDefault(m => m.NIM == nim);
            if (item == null) return;

            if (MessageBox.Show($"Hapus data mahasiswa dengan NIM {nim}?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                daftarMahasiswa.Remove(item);
                if (dataTable != null) LoadDataTable(txtSearch?.Text ?? "");
                RefreshDashboard();
            }
        }

        private void EditBaris(DataGridView grid, int rowIndex)
        {
            if (rowIndex < 0) return;

            string nim = Convert.ToString(grid.Rows[rowIndex].Cells["NIM"].Value);
            Mahasiswa item = daftarMahasiswa.FirstOrDefault(m => m.NIM == nim);
            if (item != null) ShowEditDialog(item);
        }

        private void ShowEditDialog(Mahasiswa item)
        {
            Form dialog = new Form
            {
                Text = "Edit Data Mahasiswa",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(580, 480),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Bg,
                ForeColor = White,
                Font = new Font("Segoe UI", 10F),
                Padding = new Padding(28)
            };

            Label title = new Label
            {
                Text = "Edit Data Mahasiswa",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = White,
                AutoSize = true,
                Location = new Point(28, 22)
            };
            dialog.Controls.Add(title);

            Label sub = new Label
            {
                Text = "Perbarui informasi mahasiswa di bawah ini.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Muted,
                AutoSize = true,
                Location = new Point(30, 56)
            };
            dialog.Controls.Add(sub);

            Label lNim = CreateFormLabel("NIM");
            lNim.Location = new Point(28, 92);
            dialog.Controls.Add(lNim);

            TextBox nim = CreateInput();
            nim.Text = item.NIM;
            nim.Location = new Point(28, 118);
            nim.Size = new Size(240, 38);
            nim.ReadOnly = true;
            nim.BackColor = Color.FromArgb(10, 26, 44);
            nim.ForeColor = Muted;
            dialog.Controls.Add(nim);

            Label lNama = CreateFormLabel("Nama Lengkap");
            lNama.Location = new Point(290, 92);
            dialog.Controls.Add(lNama);

            TextBox nama = CreateInput();
            nama.Text = item.Nama;
            nama.Location = new Point(290, 118);
            nama.Size = new Size(240, 38);
            dialog.Controls.Add(nama);

            Label lProdi = CreateFormLabel("Program Studi");
            lProdi.Location = new Point(28, 168);
            dialog.Controls.Add(lProdi);

            TextBox prodi = CreateInput();
            prodi.Text = item.Prodi;
            prodi.Location = new Point(28, 194);
            prodi.Size = new Size(502, 38);
            dialog.Controls.Add(prodi);

            Label lIpk = CreateFormLabel("IPK");
            lIpk.Location = new Point(28, 244);
            dialog.Controls.Add(lIpk);

            TextBox ipk = CreateInput();
            ipk.Text = item.IPK.ToString("0.00");
            ipk.Location = new Point(28, 270);
            ipk.Size = new Size(240, 38);
            dialog.Controls.Add(ipk);

            Button save = CreateSolidButton("✓  Simpan", Green);
            save.Location = new Point(28, 340);
            save.Size = new Size(140, 46);
            save.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(nama.Text) || string.IsNullOrWhiteSpace(prodi.Text) ||
                    !double.TryParse(ipk.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double val) || val < 0 || val > 4)
                {
                    MessageBox.Show("Periksa kembali Nama, Program Studi, dan IPK.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                item.Nama = nama.Text.Trim();
                item.Prodi = prodi.Text.Trim();
                item.IPK = val;
                dialog.DialogResult = DialogResult.OK;
                dialog.Close();
            };
            dialog.Controls.Add(save);

            Button cancel = CreateOutlineButton("Batal", Red);
            cancel.Location = new Point(180, 340);
            cancel.Size = new Size(130, 46);
            cancel.Click += (s, e) => dialog.Close();
            dialog.Controls.Add(cancel);

            dialog.ShowDialog(this);
            if (dataTable != null) LoadDataTable(txtSearch?.Text ?? "");
            RefreshDashboard();
        }

        private Panel CreateSearchBox(string placeholder)
        {
            Panel panel = new Panel
            {
                BackColor = Card,
                Height = 48,
                Padding = new Padding(14, 3, 12, 3)
            };
            panel.Paint += (sender, e) => UIHelper.DrawRoundedBorder(e.Graphics, panel.ClientRectangle, 8, Border);

            Label icon = new Label
            {
                Text = "⌕",
                Font = new Font("Segoe UI Symbol", 16F),
                ForeColor = Cyan,
                Dock = DockStyle.Left,
                Width = 30,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panel.Controls.Add(icon);

            TextBox box = new TextBox
            {
                Name = "SEARCH_BOX",
                BorderStyle = BorderStyle.None,
                BackColor = Card,
                ForeColor = Muted,
                Font = new Font("Segoe UI", 10F),
                Dock = DockStyle.Fill,
                Tag = placeholder,
                Text = placeholder
            };

            box.GotFocus += (s, e) =>
            {
                if (box.Text == Convert.ToString(box.Tag))
                {
                    box.Text = "";
                    box.ForeColor = White;
                }
            };

            box.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(box.Text))
                {
                    box.Text = Convert.ToString(box.Tag);
                    box.ForeColor = Muted;
                }
            };

            panel.Controls.Add(box);
            return panel;
        }

        private TextBox FindTextBox(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox tb) return tb;
            }
            return null;
        }

        private Button CreateNavButton(string text, bool active)
        {
            return new SmoothButton
            {
                Text = text,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = active ? White : Color.FromArgb(177, 202, 224),
                NormalColor = active ? Color.FromArgb(24, 68, 110) : Toolbar,
                HoverColor = Color.FromArgb(36, 92, 145),
                BorderColor = active ? Color.FromArgb(62, 105, 145) : Color.Transparent,
                Outline = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private Button CreateSolidButton(string text, Color color)
        {
            return new SmoothButton
            {
                Text = text,
                ForeColor = color == Yellow ? Color.FromArgb(25, 30, 35) : White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                NormalColor = color,
                HoverColor = ControlPaint.Light(color, 0.12F),
                Outline = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private Button CreateOutlineButton(string text, Color color)
        {
            return new SmoothButton
            {
                Text = text,
                ForeColor = color,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                NormalColor = Toolbar,
                HoverColor = Color.FromArgb(24, 52, 80),
                BorderColor = color,
                Outline = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private TextBox CreateInput()
        {
            return new TextBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Card2,
                ForeColor = White,
                Font = new Font("Segoe UI", 10F),
                Margin = new Padding(0)
            };
        }

        private Label CreateFormLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = White,
                AutoSize = true
            };
        }

        private Panel WrapCard(Control child, int margin)
        {
            Panel wrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(margin)
            };
            wrapper.Controls.Add(child);
            return wrapper;
        }

        private void ClearContent()
        {
            contentPanel.Controls.Clear();
            lblTotalMahasiswa = null;
            lblTotalProdi = null;
            lblRataIPK = null;
            dashboardTable = null;
            dataTable = null;
            txtSearch = null;
        }

        private Label FindLabel(Control parent, string name)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Label label && c.Name == name) return label;
                Label found = FindLabel(c, name);
                if (found != null) return found;
            }
            return null;
        }

        private Label FindLabelByName(string name)
        {
            return FindControlRecursive(contentPanel, name) as Label;
        }

        private Control FindControlRecursive(Control parent, string name)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Name == name) return c;
                Control result = FindControlRecursive(c, name);
                if (result != null) return result;
            }
            return null;
        }

        private void PaintToolbar(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.FromArgb(40, 80, 115), 1))
            {
                e.Graphics.DrawLine(pen, 0, toolbar.Height - 1, toolbar.Width, toolbar.Height - 1);
            }
        }
    }
}