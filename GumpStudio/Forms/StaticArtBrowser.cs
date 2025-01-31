using GumpStudio.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Ultima;

namespace GumpStudio
{
    public class StaticArtBrowser : Form
    {
        private bool _searchSomething;
        private  Button _cmdCache;
        private  Button _cmdSearch;
        private Label _lblID;
        private Label _lblName;
        private Label _lblSize;
        private Label _lblWait;
        private PictureBox _picCanvas;
        private ToolTip _toolTip;
        private  TextBox _txtSearch;
        private  VScrollBar _vsbScroller;
        protected static Bitmap? BlankCache;
        protected bool BuildingCache;
        protected static GumpCacheEntry[]? Cache;
        private  IContainer? _components;
        protected Size DisplaySize;
        protected Point HoverPos;
        protected int NumX;
        protected int NumY;
        protected static Bitmap[]? RowCache;
        protected int SelectedIndex;
        protected int StartIndex;

        public int ItemID
        {
            get => Cache![SelectedIndex].ID;
            set
            {
                if (Cache == null)
                    return;

                for (int i = 0; i < Cache.Length; i++)
                {
                    if (Cache[i].ID != value)
                        continue;

                    SelectedIndex = i;
                    _lblName.Text = $"{Resources.Name__}{TileData.ItemTable[ItemID].Name}";
                    var staticArt = Art.GetStatic(ItemID);
                    _lblSize.Text = $"{Resources.Size__}{staticArt.Width} x {staticArt.Height}";
                    break;
                }
            }
        }

        public StaticArtBrowser()
        {
            Load += NewStaticArtBrowser_Load;
            Resize += NewStaticArtBrowser_Resize;

            DisplaySize = new Size(45, 45);
            HoverPos = new Point(-1, -1);
            SelectedIndex = 0;
            BuildingCache = false;

            InitializeComponent();
        }

        protected void BuildCache()
        {
            if (BuildingCache)
                return;

            BuildingCache = true;
            _lblWait.Text = Resources.Generating_static_art_cache;
            Show();

            using FileStream? fileStream = null;
            try
            {
                Cache = null;
                _lblWait.Visible = true;
                Application.DoEvents();

                int upperBound = TileData.ItemTable.GetUpperBound(0);

                for (int index = 0; index <= upperBound; index++)
                {
                    if (index % 128 == 0)
                    {
                        double percentage = index / (double)upperBound * 100.0;
                        _lblWait.Text = $"{Resources.Generating_static_art_cache}{percentage:F2}%";
                        Application.DoEvents();
                    }

                    using var bitmap = Art.GetStatic(index);
                    if (bitmap == null)
                        continue;

                    if (Cache == null)
                        Cache = new GumpCacheEntry[1];
                    else
                        Array.Resize(ref Cache, Cache.Length + 1);

                    Cache[Cache.Length - 1] = new GumpCacheEntry
                    {
                        ID = index,
                        Size = bitmap.Size,
                        Name = TileData.ItemTable[index].Name
                    };
                }

                using var fs = new FileStream(Path.Combine(Application.StartupPath, "StaticArt.cache"), FileMode.Create);
                new BinaryFormatter().Serialize(fs, Cache);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Resources.Error_creating_cache}{ex.Message}");
            }
            finally
            {
                _lblWait.Visible = false;
                Application.DoEvents();
                BuildingCache = false;
            }
        }

        private void InitializeComponent()
        {
            _components = new Container();

            _picCanvas = new PictureBox();
            _vsbScroller = new VScrollBar();
            _txtSearch = new TextBox();
            _cmdSearch = new Button();
            _lblName = new Label();
            _lblSize = new Label();
            _cmdCache = new Button();
            _lblWait = new Label();
            _toolTip = new ToolTip(_components);
            _lblID = new Label();

            ((ISupportInitialize)(_picCanvas)).BeginInit();
            SuspendLayout();

            // picCanvas
            _picCanvas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            _picCanvas.Location = new Point(0, 0);
            _picCanvas.Name = "_picCanvas";
            _picCanvas.Size = new Size(488, 396);
            _picCanvas.TabIndex = 0;
            _picCanvas.TabStop = false;
            _picCanvas.Paint += picCanvas_Paint;
            _picCanvas.DoubleClick += picCanvas_DoubleClick;
            _picCanvas.MouseLeave += picCanvas_MouseLeave;
            _picCanvas.MouseMove += picCanvas_MouseMove;
            _picCanvas.MouseUp += picCanvas_MouseUp;

            // vsbScroller
            _vsbScroller.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            _vsbScroller.Location = new Point(488, 0);
            _vsbScroller.Name = "_vsbScroller";
            _vsbScroller.Size = new Size(17, 396);
            _vsbScroller.TabIndex = 3;
            _vsbScroller.Scroll += vsbScroller_Scroll;

            // txtSearch
            _txtSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _txtSearch.Location = new Point(56, 403);
            _txtSearch.Name = "_txtSearch";
            _txtSearch.Size = new Size(100, 20);
            _txtSearch.TabIndex = 4;

            // cmdSearch
            _cmdSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _cmdSearch.Location = new Point(160, 403);
            _cmdSearch.Name = "_cmdSearch";
            _cmdSearch.Size = new Size(50, 20);
            _cmdSearch.TabIndex = 5;
            _cmdSearch.Text = "Search";
            _cmdSearch.Click += cmdSearch_Click;

            // lblName
            _lblName.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _lblName.AutoSize = true;
            _lblName.Location = new Point(216, 405);
            _lblName.Name = "_lblName";
            _lblName.Size = new Size(38, 13);
            _lblName.TabIndex = 6;
            _lblName.Text = "Name:";

            // lblSize
            _lblSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _lblSize.AutoSize = true;
            _lblSize.Location = new Point(408, 405);
            _lblSize.Name = "_lblSize";
            _lblSize.Size = new Size(30, 13);
            _lblSize.TabIndex = 7;
            _lblSize.Text = "Size:";

            // cmdCache
            _cmdCache.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _cmdCache.FlatStyle = FlatStyle.Flat;
            _cmdCache.Location = new Point(0, 402);
            _cmdCache.Name = "_cmdCache";
            _cmdCache.Size = new Size(50, 23);
            _cmdCache.TabIndex = 9;
            _cmdCache.Text = "Cache";
            _toolTip.SetToolTip(_cmdCache, "Rebuild Art Cache");
            _cmdCache.Click += cmdCache_Click;

            // lblWait
            _lblWait.BackColor = Color.Transparent;
            _lblWait.BorderStyle = BorderStyle.Fixed3D;
            _lblWait.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _lblWait.Location = new Point(164, 159);
            _lblWait.Name = "_lblWait";
            _lblWait.Size = new Size(184, 104);
            _lblWait.TabIndex = 10;
            _lblWait.Text = "Please Wait, Generating Static Art Cache...";
            _lblWait.TextAlign = ContentAlignment.MiddleCenter;
            _lblWait.Visible = false;

            // lblID
            _lblID.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _lblID.AutoSize = true;
            _lblID.Location = new Point(216, 429);
            _lblID.Name = "_lblID";
            _lblID.Size = new Size(21, 13);
            _lblID.TabIndex = 11;
            _lblID.Text = "ID:";

            // StaticArtBrowser
            AutoScaleBaseSize = new Size(5, 13);
            ClientSize = new Size(505, 451);
            Controls.AddRange(new Control[]
            {
                _lblID,
                _lblWait,
                _cmdCache,
                _lblSize,
                _lblName,
                _cmdSearch,
                _txtSearch,
                _vsbScroller,
                _picCanvas
            });

            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            MaximumSize = new Size(521, 3000);
            MinimumSize = new Size(521, 200);
            Name = "StaticArtBrowser";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Static Art Browser";

            ((ISupportInitialize)(_picCanvas)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void cmdCache_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                Resources.Rebuild_longtime,
                Resources.Question,
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.OK)
                return;

            BuildCache();
            RowCache = new Bitmap[(int)Math.Ceiling(Cache!.Length / (double)NumX) + 2];
            ItemID = 0;
            _picCanvas.Invalidate();
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            int foundIndex = -1;
            int currentIndex = SelectedIndex == -1 ? 0 : SelectedIndex;

            while (foundIndex == -1 && currentIndex < Cache!.Length - 1)
            {
                currentIndex++;               
                if (Cache[currentIndex].Name.IndexOf(_txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        foundIndex = currentIndex;
            }

            if (foundIndex != -1)
                ItemID = Cache[foundIndex].ID;
            else if (currentIndex > 0 && !_searchSomething)
            {
                SelectedIndex = 0;
                _searchSomething = true;
                cmdSearch_Click(sender, e);
            }

            _vsbScroller.Value = SelectedIndex / NumX;
            vsbScroller_Scroll(_vsbScroller, new ScrollEventArgs(ScrollEventType.ThumbPosition, _vsbScroller.Value));
            _searchSomething = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _components?.Dispose();
            base.Dispose(disposing);
        }

        protected void DrawGrid(Graphics g)
        {
            // Draw vertical lines
            for (int i = 0; i <= NumX; i++)
                g.DrawLine(Pens.Black, i * DisplaySize.Width, 0, i * DisplaySize.Width, (NumY + 1) * DisplaySize.Height);

            // Draw horizontal lines
            for (int i = 0; i <= NumY + 1; i++)
                g.DrawLine(Pens.Black, 0, i * DisplaySize.Height, NumX * DisplaySize.Width, i * DisplaySize.Height);
        }

        protected void DrawHover(Graphics g)
        {
            var (x, y) = (HoverPos.X, HoverPos.Y);
            int index = StartIndex + x + y * NumX;

            if (index >= Cache!.Length)
                return;

            int id = Cache[index].ID;
            using var bitmap = Art.GetStatic(id);

            var point = new Point
            {
                X = (int)(x * DisplaySize.Width + DisplaySize.Width / 2.0 - bitmap.Width / 2.0 - 3),
                Y = (int)(y * DisplaySize.Height + DisplaySize.Height / 2.0 - bitmap.Height / 2.0 - 3)
            };

            // Adjust position to keep within bounds
            point.X = Math.Max(0, Math.Min(point.X, _picCanvas.Width - bitmap.Width - 3));
            point.Y = Math.Max(0, Math.Min(point.Y, _picCanvas.Height - bitmap.Height - 3));

            var rect = new Rectangle(point, bitmap.Size);
            using var shadowBrush = new SolidBrush(Color.FromArgb(127, Color.Black));

            g.FillRectangle(shadowBrush, point.X + 5, point.Y + 5, rect.Width, rect.Height);
            g.FillRectangle(Brushes.White, rect);
            g.DrawRectangle(Pens.Black, rect);
            g.DrawImage(bitmap, point);

            _lblName.Text = $"{Resources.Name__}{TileData.ItemTable[id].Name}";
            _lblSize.Text = $"{Resources.Size__}{bitmap.Width} x {bitmap.Height}";
            _lblID.Text = $"ID: {id} - hex:{id:X}";
        }

        protected Bitmap GetRowImage(int row)
        {
            if (row >= RowCache!.Length)
            {
                if (BlankCache != null)
                    return BlankCache;

                BlankCache = new Bitmap(NumX * DisplaySize.Width, DisplaySize.Height, PixelFormat.Format16bppRgb565);
                using var g2 = Graphics.FromImage(BlankCache);
                g2.Clear(Color.Gray);
                return BlankCache;
            }

            if (RowCache[row] != null)
                return RowCache[row];

            var rowImage = new Bitmap(NumX * DisplaySize.Width, DisplaySize.Height, PixelFormat.Format16bppRgb565);
            using var g = Graphics.FromImage(rowImage);
            g.Clear(Color.Gray);

            var originalClip = g.Clip;
            var bounds = new Rectangle(0, 0, NumX * DisplaySize.Width, NumY * DisplaySize.Height);
            using var boundsRegion = new Region(bounds);
            g.Clip = boundsRegion;

            for (int x = 0; x < NumX; x++)
            {
                int index = row * NumX + x;
                if (index >= Cache!.Length)
                    continue;

                using var staticArt = Art.GetStatic(Cache[index].ID);
                var cellBounds = new Rectangle(x * DisplaySize.Width, 0, DisplaySize.Width, DisplaySize.Height);
                using var cellRegion = new Region(cellBounds);
                g.Clip = cellRegion;
                g.FillRectangle(Brushes.White, x * DisplaySize.Width, 0, DisplaySize.Width, DisplaySize.Height);
                g.DrawImage(staticArt, x * DisplaySize.Width + 1, 0);
            }

            g.Clip = originalClip;
            RowCache[row] = rowImage;
            return rowImage;
        }

        private void NewStaticArtBrowser_Load(object sender, EventArgs e)
        {
            if (Cache == null)
            {
                string cachePath = Path.Combine(Application.StartupPath, "StaticArt.cache");
                if (!File.Exists(cachePath))
                {
                    BuildCache();
                }
                else
                {
                    try
                    {
                        using var fs = new FileStream(cachePath, FileMode.Open);
                        Cache = (GumpCacheEntry[])new BinaryFormatter().Deserialize(fs);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error reading cache file:\n{ex.Message}");
                    }
                }
            }

            _picCanvas.Width = ClientSize.Width - _vsbScroller.Width;
            Show();

            _vsbScroller.Maximum = (int)Math.Ceiling(Cache!.Length / (double)NumX) + 1;
            _vsbScroller.LargeChange = NumY - 1;

            RowCache ??= new Bitmap[(int)Math.Ceiling(Cache.Length / (double)NumX) + 2];

            _vsbScroller.Value = SelectedIndex / NumX;
            vsbScroller_Scroll(_vsbScroller, null);

            UpdateLabels();
        }

        private void NewStaticArtBrowser_Resize(object sender, EventArgs e)
        {
            const int defaultNumX = 11;
            int newNumY = _picCanvas.Height / DisplaySize.Height;

            if (defaultNumX == NumX && newNumY == NumY)
                return;

            NumX = defaultNumX;
            NumY = newNumY;

            if (Cache == null)
                return;

            _vsbScroller.Maximum = (int)Math.Ceiling(Cache.Length / (double)NumX) + 1;
            _vsbScroller.LargeChange = NumY - 1;
            _picCanvas.Invalidate();
        }

        private void picCanvas_DoubleClick(object sender, EventArgs e)
        {
            if (BuildingCache)
                return;

            DialogResult = DialogResult.OK;
        }

        private void picCanvas_MouseLeave(object sender, EventArgs e)
        {
            HoverPos = new Point(-1, -1);
            _picCanvas.Invalidate();

            var selectedItem = Cache![SelectedIndex];
            _lblName.Text = $"{Resources.Name__}{TileData.ItemTable[selectedItem.ID].Name}";
            _lblSize.Text = $"{Resources.Size__}{selectedItem.Size.Width} x {selectedItem.Size.Height}";
            _lblID.Text = $"ID: {selectedItem.ID} (0x{selectedItem.ID:X})";
        }

        private void picCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var newHoverPos = new Point(e.X / DisplaySize.Width, e.Y / DisplaySize.Height);

            if (newHoverPos.X >= NumX || (newHoverPos.X == HoverPos.X && newHoverPos.Y == HoverPos.Y))
                return;

            HoverPos = newHoverPos;
            _picCanvas.Invalidate();
        }

        private void picCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            int index = e.X / DisplaySize.Width + e.Y / DisplaySize.Height * NumX + StartIndex;

            if (index >= Cache!.Length)
                return;

            ItemID = Cache[index].ID;
            _picCanvas.Invalidate();
        }

        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Render(e.Graphics);

                if (!HoverPos.Equals(new Point(-1, -1)))
                    DrawHover(e.Graphics);
            }
            catch (Exception)
            {
                // Log or handle the exception as needed
            }
        }

        public void Render(Graphics g)
        {
            if (Cache == null || RowCache == null)
                return;

            g.Clear(Color.Gray);
            DrawGrid(g);

            var clip = g.Clip;
            int rowNum = StartIndex / NumX;
            bool selectedFound = false;
            Rectangle selectedRect = Rectangle.Empty;

            for (int y = 0; y <= NumY; y++)
            {
                g.DrawImage(GetRowImage(y + rowNum), 0, y * DisplaySize.Height);

                if (!selectedFound && y + rowNum == SelectedIndex / NumX)
                {
                    selectedFound = true;
                    selectedRect = new Rectangle(
                        SelectedIndex % NumX * DisplaySize.Width,
                        y * DisplaySize.Height,
                        DisplaySize.Width,
                        DisplaySize.Height
                    );
                }
            }

            DrawGrid(g);

            if (selectedFound)
            {
                using var region = new Region(selectedRect);
                selectedRect.Inflate(5, 5);

                using var highlightBrush = new SolidBrush(Color.FromArgb(127, Color.Blue));
                g.FillRectangle(highlightBrush, selectedRect);
                g.DrawRectangle(Pens.Blue, selectedRect);

                selectedRect.Inflate(-5, -5);
                g.Clip = region;
                g.DrawImage(Art.GetStatic(Cache[SelectedIndex].ID), selectedRect.Location);
                g.Clip = clip;
            }
        }

        private void vsbScroller_Scroll(object sender, ScrollEventArgs? e)
        {
            StartIndex = _vsbScroller.Value * NumX;
            _picCanvas.Invalidate();
        }

        private void UpdateLabels()
        {
            var selectedItem = Cache![SelectedIndex];
            _lblName.Text = $"{Resources.Name__}{TileData.ItemTable[selectedItem.ID].Name}";
            _lblSize.Text = $"{Resources.Size__}{selectedItem.Size.Width} x {selectedItem.Size.Height}";
        }
    }
}