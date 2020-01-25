using FrameLessWindow.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FrameLessWindow
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:ToolsLibrary"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:ToolsLibrary;assembly=ToolsLibrary"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class FrameLessWindow : Window
    {
        private bool IsMaxSized = false;
        private bool IsMaxSizeing = false;
        private bool IsCanMove = false;
        private bool IsCaptured = false;
        private Point Start;
        private int LastTiime = 0;

        private Thickness Original;
        private double Original_ShadowSize;
        private Thickness Original_Padding;

        public WindowCommands CloseCommand{ get; set; }
        public WindowCommands MiniCommand { get; set; }
        public WindowCommands FullCommand { get; set; }
        public WindowCommands ColorBorderCommand { get; set; }

        static FrameLessWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FrameLessWindow), new FrameworkPropertyMetadata(typeof(FrameLessWindow)));
        }

        public FrameLessWindow() {
            CloseCommand = new WindowCommands();
            MiniCommand = new WindowCommands();
            FullCommand = new WindowCommands();
            ColorBorderCommand = new WindowCommands();
            CloseCommand.Actor = new Action<object>(CloseWindow);
            MiniCommand.Actor = new Action<object>(MiniSize);
            FullCommand.Actor = new Action<object>(FullSize);
            ColorBorderCommand.Actor = new Action<object>(ColorBorder);

            MouseMove += FrameLessWindow_MouseMove;
            MouseUp += FrameLessWindow_MouseUp;
            MouseDown += FrameLessWindow_MouseDown;
            MouseDoubleClick += FrameLessWindow_MouseDoubleClick;
            
            DataContext = this;
            
        }
        #region Command响应函数
        private void ColorBorder(object parameter)
        {
            DynamicBorderColor = !DynamicBorderColor;
        }
        private void CloseWindow(object parameter)
        {
            this.Close();
        }
        private void MiniSize(object parameter)
        {
            
            WindowState = WindowState.Minimized;
        }
        private void FullSize(object parameter)
        {
            if (ResizeMode == ResizeMode.NoResize) return;
            IsMaxSizeing = true;
            if (IsMaxSized)
            {
                ReStorySize();
                ShadowSize = Original_ShadowSize;
                Padding = Original_Padding;
                IsMaxSized = false;
            }
            else
            {
                Original_ShadowSize = ShadowSize;
                Original_Padding = Padding;

                
                Padding = new Thickness(0, 0, 0, 0);
                ShadowSize = 0;

                UpdateSizeLog();
                Left = SystemParameters.WorkArea.Left;
                Top = SystemParameters.WorkArea.Top;
                Height = SystemParameters.WorkArea.Height;
                Width = SystemParameters.WorkArea.Width;
                
                IsMaxSized = true;
                
            }
            IsMaxSizeing = false;
        }

        #endregion

        private void FrameLessWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LastTiime = e.Timestamp;
            if (e.ClickCount >= 2) return;
            DependencyObject ue = e.OriginalSource as DependencyObject;
            Border pa = LogicalTreeHelper.GetParent(ue) as Border;
            
            if (pa == null) return;

            if (pa.Name == "PART_Shadow_Border" && e.LeftButton == MouseButtonState.Pressed && !IsMaxSizeing)
            {
                if (!IsMaxSizeing)
                {
                    IsCanMove = true;
                    Start = e.GetPosition(this);
                    //UpdateSizeLog();
                    IsCaptured = CaptureMouse();
                }
                e.Handled = true;
            }
        }
        private void FrameLessWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
            IsMaxSizeing = false;
            if (IsCanMove)
            {
                IsCanMove = false;
                if(IsCaptured)ReleaseMouseCapture();
            }

        }

        private void FrameLessWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (LastTiime==0 || e.Timestamp - LastTiime < 300) return;
            if (!IsMaxSizeing && e.LeftButton == MouseButtonState.Pressed)
            {


                if (IsCanMove)
                {
                    Point re = e.GetPosition(this);
                    if (IsMaxSized)
                    {
                        IsMaxSizeing = true;
                        ReStorySize(re);
                        ShadowSize = Original_ShadowSize;
                        Padding = Original_Padding;
                        IsMaxSized = false;
                        IsMaxSizeing = false;
                        return;
                    }
                    else {
                        var wmp = PointToScreen(re);
                        Left = wmp.X - Start.X;
                        Top = wmp.Y - Start.Y;
                        //UpdateSizeLog();
                    }
                    //e.Handled = true;
                }
            }

        }

        private void FrameLessWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ResizeMode == ResizeMode.NoResize) return;
            DependencyObject ue = e.OriginalSource as DependencyObject;
            Border pa = LogicalTreeHelper.GetParent(ue) as Border;
            if (pa == null) return;
            if (pa.Name == "PART_Shadow_Border" && e.LeftButton == MouseButtonState.Pressed)
            {
                FullSize(null);
            }
            e.Handled = true;
        }

        private void ReStorySize(Point re)
        {

            Start.X = Original_Padding.Left+((Start.X / Width) * Original.Bottom);
            Start.Y = Start.Y + Original_Padding.Top;

            Left = PointToScreen(re).X - (re.X / Width )* Original.Bottom;
            Top = PointToScreen(re).Y - re.Y - Original_Padding.Top;
            Height = Original.Right;
            Width = Original.Bottom;
            
        }
        private void ReStorySize() { 
            Left = Original.Left;
            Top = Original.Top;
            Height = Original.Right;
            Width = Original.Bottom;
        }
        private void UpdateSizeLog()
        {
            Original.Left = Left;
            Original.Top = Top;
            Original.Right = Height;
            Original.Bottom = Width;
        }

        #region 依赖属性
        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }
        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register("ShadowColor", typeof(Color), typeof(FrameLessWindow), new FrameworkPropertyMetadata(Color.FromArgb(255, 255, 255, 255)) {  BindsTwoWayByDefault = true});

        public double ShadowOpacity
        {
            get { return (double)GetValue(ShadowOpacityProperty); }
            set { SetValue(ShadowOpacityProperty, value); }
        }
        public static readonly DependencyProperty ShadowOpacityProperty =
            DependencyProperty.Register("ShadowOpacity", typeof(double), typeof(FrameLessWindow), new PropertyMetadata(1.0));

        public double ShadowSize
        {
            get { return (double)GetValue(ShadowSizeProperty); }
            set { SetValue(ShadowSizeProperty, value); }
        }
        public static readonly DependencyProperty ShadowSizeProperty =
            DependencyProperty.Register("ShadowSize", typeof(double), typeof(FrameLessWindow), new PropertyMetadata(10.0));

        public bool DynamicBorderColor
        {
            get { return (bool)GetValue(DynamicBorderColorProperty); }
            set { SetValue(DynamicBorderColorProperty, value); }
        }
        public static readonly DependencyProperty DynamicBorderColorProperty =
            DependencyProperty.Register("DynamicBorderColor", typeof(bool), typeof(FrameLessWindow), new PropertyMetadata(false));

        public GridLength TitleBarHeight
        {
            get { return (GridLength)GetValue(TitleBarHeightProperty); }
            set { SetValue(TitleBarHeightProperty, value); }
        }
        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register("TitleBarHeight", typeof(GridLength), typeof(FrameLessWindow), new PropertyMetadata(new GridLength(26)));

        public GridLength ToolBarHeight
        {
            get { return (GridLength)GetValue(ToolBarHeightProperty); }
            set { SetValue(ToolBarHeightProperty, value); }
        }
        public static readonly DependencyProperty ToolBarHeightProperty =
            DependencyProperty.Register("ToolBarHeight", typeof(GridLength), typeof(FrameLessWindow), new PropertyMetadata(new GridLength(60)));

        public GridLength StateBarHeight
        {
            get { return (GridLength)GetValue(StateBarHeightProperty); }
            set { SetValue(StateBarHeightProperty, value); }
        }
        public static readonly DependencyProperty StateBarHeightProperty =
            DependencyProperty.Register("StateBarHeight", typeof(GridLength), typeof(FrameLessWindow), new PropertyMetadata(new GridLength(46)));

        public object ToolBarExContent
        {
            get { return (object)GetValue(ToolBarExContentProperty); }
            set { SetValue(ToolBarExContentProperty, value); }
        }
        public static readonly DependencyProperty ToolBarExContentProperty =
            DependencyProperty.Register("ToolBarExContent", typeof(object), typeof(FrameLessWindow), null);

        public object TitleBarExContent
        {
            get { return (object)GetValue(TitleBarExContentProperty); }
            set { SetValue(TitleBarExContentProperty, value); }
        }
        public static readonly DependencyProperty TitleBarExContentProperty =
            DependencyProperty.Register("TitleBarExContent", typeof(object), typeof(FrameLessWindow), null);


        public object TitleBarContext
        {
            get { return (object)GetValue(TitleBarContextProperty); }
            set { SetValue(TitleBarContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TitleBarContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleBarContextProperty =
            DependencyProperty.Register("TitleBarContext", typeof(object), typeof(FrameLessWindow), null);



        public object StateBarExContent
        {
            get { return (object)GetValue(StateBarExContentProperty); }
            set { SetValue(StateBarExContentProperty, value); }
        }
        public static readonly DependencyProperty StateBarExContentProperty =
            DependencyProperty.Register("StateBarExContent", typeof(object), typeof(FrameLessWindow), null);
        #endregion

    }
}
