using GumpStudio;
using GumpStudio.Elements;
using GumpStudio.Forms;
using GumpStudio.Plugins;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace GumpStudioCore.Plugins
{
    public class SnapToGrid : BasePlugin
    {
        protected GridConfiguration Config;
        private DesignerForm _designer;
        private SnapToGridExtender _extender;

        public override string Name => GetPluginInfo().PluginName;

        public MenuItem ShowGridMenu { get; set; }

        public SnapToGrid()
        {
            Size gridSize = new Size(10, 10);

            Config = new GridConfiguration(gridSize, Color.LightGray, true);
        }

        private void DoConfigGridMenu(object Sender, EventArgs E)
        {
            _designer.picCanvas.Refresh();
        }

        private void DoToggleGridMenu(object Sender, EventArgs E)
        {
            Config.ShowGrid = !Config.ShowGrid;
            ShowGridMenu.Checked = Config.ShowGrid;
            _designer.picCanvas.Refresh();
        }

        public override PluginInfo GetPluginInfo()
        {
            PluginInfo val = new PluginInfo
            {
                AuthorEmail = "buffner@tkpups.com",
                AuthorName = "Bradley Uffner",
                Description = "Allows elements to be snapped to a grid.",
                PluginName = "SnapToGrid",
                Version = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor
            };

            return val;
        }

        private void HookKeyDown(object sender, ref KeyEventArgs e)
        {
            if (_designer.ActiveElement == null || sender != _designer.CanvasFocus || e.Modifiers.HasFlag(Keys.Shift) || !Config.ShowGrid)
            {
                return;
            }

            bool modified = false;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    {
                        foreach (object obj in _designer.ElementStack.GetSelectedElements())
                        {
                            BaseElement element = (BaseElement)obj;
                            element.Y = element.Y - Config.GridSize.Height;
                            element.Y = _extender.SnapYToGrid(element.Y);
                        }

                        modified = true;
                        _designer.CreateUndoPoint();

                        break;
                    }

                case Keys.Down:
                    {
                        foreach (object obj in _designer.ElementStack.GetSelectedElements())
                        {
                            BaseElement element = (BaseElement)obj;
                            element.Y = element.Y + Config.GridSize.Height;
                            element.Y = _extender.SnapYToGrid(element.Y);
                        }
                        modified = true;
                        _designer.CreateUndoPoint();
                        break;
                    }

                case Keys.Left:
                    {
                        foreach (object obj in _designer.ElementStack.GetSelectedElements())
                        {
                            BaseElement element = (BaseElement)obj;
                            element.X = element.X - Config.GridSize.Width;
                            element.X = _extender.SnapXToGrid(element.X);
                        }
                        modified = true;
                        _designer.CreateUndoPoint();
                        break;
                    }

                case Keys.Right:
                    {
                        foreach (object obj in _designer.ElementStack.GetSelectedElements())
                        {
                            BaseElement element = (BaseElement)obj;
                            element.X = element.X + Config.GridSize.Width;
                            element.X = _extender.SnapXToGrid(element.X);
                        }
                        modified = true;
                        _designer.CreateUndoPoint();
                        break;
                    }
            }

            if (modified)
            {
                e.Handled = true;
                _designer.picCanvas.Invalidate();
            }
        }

        public override void InitializeElementExtenders(BaseElement Element)
        {
            Element.AddExtender(_extender);
        }

        public override void Load(DesignerForm frmDesigner)
        {
            base.Load(frmDesigner);
            _designer = frmDesigner;

            LoadConfig();

            if (_extender == null)
            {
                _extender = new SnapToGridExtender(_designer);
            }

            _extender.Config = Config;

            MenuItem menuItem = new MenuItem("Snap To Grid", ToggleSnapToGrid);
            menuItem.Checked = Config.ShowGrid;

            _designer.mnuPlugins.MenuItems.Add(menuItem);
            _designer.HookPreRender += RenderGrid;
            _designer.HookKeyDown += HookKeyDown;
        }

        protected void LoadConfig()
        {
            if (!File.Exists(_designer.AppPath + "\\Plugins\\SnapToGrid.config"))
            {
                return;
            }

            FileStream fileStream = new FileStream(_designer.AppPath + "\\Plugins\\SnapToGrid.config", FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Config = (GridConfiguration)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
        }

        public override void MouseMoveHook(ref MouseMoveHookEventArgs e)
        {
            if (!Config.ShowGrid)
            {
                return;
            }

            if (e.MoveMode == MoveModeType.Move && !e.Keys.HasFlag(Keys.Shift))
            {
                e.MouseLocation = _extender.SnapToGrid(e.MouseLocation);
            }
        }

        private void RenderGrid(Bitmap Target)
        {
            Rectangle rect = new Rectangle(0, 0, Target.Width, Target.Height);
            BitmapData bitmapData = Target.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            checked
            {
                if (Config.ShowGrid)
                {
                    int num = Target.Width - 1;
                    int width = _extender.Config.GridSize.Width;
                    int num2 = num;
                    int num3 = 0;

                    while (true)
                    {
                        int num4 = (width >> 31) ^ num3;
                        int num5 = (width >> 31) ^ num2;

                        if (num4 > num5)
                        {
                            break;
                        }

                        int num6 = Target.Height - 1;
                        int height = _extender.Config.GridSize.Height;
                        int num7 = num6;
                        int num8 = 0;

                        while (true)
                        {
                            int num9 = (height >> 31) ^ num8;
                            num5 = (height >> 31) ^ num7;

                            if (num9 > num5)
                            {
                                break;
                            }

                            int num10 = bitmapData.Stride * num8 + 4 * num3;
                            Marshal.WriteByte(bitmapData.Scan0, num10, Config.GridColor.R);
                            Marshal.WriteByte(bitmapData.Scan0, num10 + 1, Config.GridColor.G);
                            Marshal.WriteByte(bitmapData.Scan0, num10 + 2, Config.GridColor.B);
                            Marshal.WriteByte(bitmapData.Scan0, num10 + 3, byte.MaxValue);
                            num8 += height;
                        }

                        num3 += width;
                    }
                }

                Target.UnlockBits(bitmapData);
            }
        }

        protected void SaveConfig()
        {
            FileStream fileStream = new FileStream(_designer.AppPath + "\\Plugins\\SnapToGrid.config", FileMode.Create);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, Config);
            fileStream.Close();
        }

        private void ToggleSnapToGrid(object sender, EventArgs e)
        {
            Config.ShowGrid = !Config.ShowGrid;
            ((MenuItem)sender).Checked = Config.ShowGrid;
            _designer.picCanvas.Invalidate();
        }
    }
}