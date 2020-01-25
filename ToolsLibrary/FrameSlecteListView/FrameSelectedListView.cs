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

namespace FrameSlecteListView
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:FrameSlecteListView"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:FrameSlecteListView;assembly=FrameSlecteListView"
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
    public class FrameSelectedListView : ListView
    {

        static FrameSelectedListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FrameSelectedListView), new FrameworkPropertyMetadata(typeof(FrameSelectedListView)));
        }
        //private Border Selecter;
        private bool FrameEnable = false;

        public FrameSelectedListView()
        {
            Loaded += FrameSelectListView_Loaded;
        }
        //将自动生成的子容器由默认的ListViewItem 变为 FrameSelectListViewItem 以支持框选
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new FrameSelectedListViewItem();
        }



        private void FrameSelectListView_Loaded(object sender, RoutedEventArgs e)
        {

            //Selecter = new Border();
            //Selecter.Background = new SolidColorBrush(Color.FromArgb(26, 0, 62, 255));
            //Selecter.BorderBrush = new SolidColorBrush(Color.FromArgb(155, 0, 82, 255));
            //Selecter.BorderThickness = new Thickness(1);
            //Selecter.HorizontalAlignment = HorizontalAlignment.Left;
            //Selecter.VerticalAlignment = VerticalAlignment.Top;
            //Selecter.Visibility = Visibility.Collapsed;

            MouseDown += FrameSelectListView_MouseDown;
            MouseMove += FrameSelectListView_MouseMove;
            MouseUp += FrameSelectListView_MouseUp;

            if (FrameContent == null)
            {
                Border bd = new Border();
                bd.Background = new SolidColorBrush(Color.FromArgb(26, 0, 62, 255));
                bd.BorderBrush = new SolidColorBrush(Color.FromArgb(155, 0, 82, 255));
                bd.BorderThickness = new Thickness(2, 2, 2, 2);
                FrameContent = bd;
            }
            Panel.SetZIndex((FrameworkElement)FrameContent, 999);
            ((FrameworkElement)FrameContent).HorizontalAlignment = HorizontalAlignment.Left;
            ((FrameworkElement)FrameContent).VerticalAlignment = VerticalAlignment.Top;
            ((FrameworkElement)FrameContent).Visibility = Visibility.Collapsed;
            Grid tg = (Grid)GetVisualTarget<Grid>(this, typeof(ScrollViewer));
            if (tg != null) tg.Children.Insert(0, (UIElement)FrameContent);

        }

        private void FrameSelectListView_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (FrameEnable)
            {
                ((FrameworkElement)FrameContent).Visibility = Visibility.Collapsed;
                //Selecter.Visibility = Visibility.Collapsed;
                ReleaseMouseCapture();
                //e.Handled = true;
                FrameEnable = false;
                
            }
        }

        private void FrameSelectListView_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                if (FrameEnable)
                {
                    try
                    {
                        FrameEndLocation = e.GetPosition(this);
                        //FrameEndLocation = new Point (FrameEndLocation.X + SV.HorizontalOffset ,FrameEndLocation.Y + SV.VerticalOffset);
                        Point A, B;
                        A = FrameStartLocation;
                        B = FrameEndLocation;
                        NormalizePoint(ref A, ref B);
                        if (A.X < 0) A.X = 0;
                        if (A.Y < 0) A.Y = 0;
                        if (B.X > ActualWidth) B.X = ActualWidth;
                        if (B.Y > ActualHeight) B.Y = ActualHeight;
                        DrawSelecter(A, B);
                        //e.Handled = true;
                        LinkedList<DependencyObject> list = new LinkedList<DependencyObject>();
                        GetVisualSubItems(this, ref list);
                        foreach (DependencyObject item in list)
                        {
                            FrameSelectedListViewItem FSLI = (FrameSelectedListViewItem)item;
                            FSLI.FrameSelect(ref A, ref B);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());

                    }
                    
                   
                }

                //FrameSelectedArgs fa = new FrameSelectedArgs(FrameSelectedEvent, this);
                //fa.Start = A;
                //fa.End = B;
                //RaiseEvent(fa);
            }

        }

        private void GetVisualSubItems(DependencyObject e, ref LinkedList<DependencyObject> list)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(e); i++)
            {
                DependencyObject dpo = VisualTreeHelper.GetChild(e, i);
                if (dpo is FrameSelectedListViewItem)
                {
                    list.AddLast(dpo);
                }
                else
                {
                    GetVisualSubItems(dpo, ref list);
                }
            }
        }

        private DependencyObject GetVisualTarget<T>(DependencyObject e,Type ParentType)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(e); i++)
            {
                DependencyObject dpo = VisualTreeHelper.GetChild(e, i);
                if (dpo is T)
                {
                    if (ParentType != null)
                    {
                        if (VisualTreeHelper.GetParent(dpo).GetType() == ParentType) return dpo;
                        else return GetVisualTarget<T>(dpo, ParentType);
                    }
                    else return dpo;
                }
                else
                {
                   return GetVisualTarget<T>(dpo,ParentType);
                }
            }
            return null;
        }
        private void FrameSelectListView_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LinkedList<DependencyObject> list = new LinkedList<DependencyObject>();
            GetVisualSubItems(this, ref list);
            foreach (DependencyObject item in list)
            {
                FrameSelectedListViewItem FSLI = (FrameSelectedListViewItem)item;
                FSLI.IsSelected = false;
            }

            if (FrameSelectMode)
            {
                FrameStartLocation = e.GetPosition(this);
                //FrameStartLocation = new Point(FrameStartLocation.X + SV.HorizontalOffset, FrameStartLocation.Y + SV.VerticalOffset);
                CaptureMouse();
                e.Handled = true;
                FrameEnable = true;
                this.SelectionMode = SelectionMode.Multiple;
                //Console.WriteLine(" LOcation :" + e.GetPosition(this).ToString());
            }

           // this.SelectionMode = SelectionMode.Single;
            
        }

        private void DrawSelecter(Point start, Point end)
        {
            //Selecter.Visibility = Visibility.Visible;
            //Selecter.Height = end.Y - start.Y;
            //Selecter.Width = end.X - start.X;
            //Selecter.Margin = new Thickness(start.X, start.Y, 0, 0);

            ((FrameworkElement)FrameContent).Visibility = Visibility.Visible;
            ((FrameworkElement)FrameContent).Height = end.Y - start.Y;
            ((FrameworkElement)FrameContent).Width = end.X - start.X;
            ((FrameworkElement)FrameContent).Margin = new Thickness(start.X, start.Y, 0, 0);
        }

        private void NormalizePoint(ref Point start, ref Point end)
        {
            double temp;

            if (start.X > end.X && start.Y < end.Y)
            {
                Point A = new Point(end.X, start.Y);
                Point B = new Point(start.X, end.Y);
                start = A;
                end = B;
            }
            else if (start.X < end.X && start.Y > end.Y)
            {
                Point B = new Point(end.X, start.Y);
                Point A = new Point(start.X, end.Y);
                start = A;
                end = B;
            }
            if (start.X > end.X)
            {
                temp = start.X; start.X = end.X; end.X = temp;
            }
            if (start.Y > end.Y)
            {
                temp = start.Y; start.Y = end.Y; end.Y = temp;
            }
        }

        public bool FrameSelectMode
        {
            get { return (bool)GetValue(FrameSelectModeProperty); }
            set { SetValue(FrameSelectModeProperty, value); }
        }
        public static readonly DependencyProperty FrameSelectModeProperty =
            DependencyProperty.Register("FrameSelectMode", typeof(bool), typeof(FrameSelectedListView), null);

        public object FrameContent
        {
            get { return (object)GetValue(FrameContentProperty); }
            set { SetValue(FrameContentProperty, value); }
        }
        public static readonly DependencyProperty FrameContentProperty =
            DependencyProperty.Register("FrameContent", typeof(object), typeof(FrameSelectedListView), null);
 


        public Point FrameStartLocation
        {
            get { return (Point)GetValue(FrameStartLocationProperty); }
            set { SetValue(FrameStartLocationProperty, value); }
        }
        public static readonly DependencyProperty FrameStartLocationProperty =
            DependencyProperty.Register("FrameStartLocation", typeof(Point), typeof(FrameSelectedListView), new PropertyMetadata(null));



        public Point FrameEndLocation
        {
            get { return (Point)GetValue(FrameEndLocationProperty); }
            set { SetValue(FrameEndLocationProperty, value); }
        }
        public static readonly DependencyProperty FrameEndLocationProperty =
            DependencyProperty.Register("FrameEndLocation", typeof(Point), typeof(FrameSelectedListView), new PropertyMetadata(null));


    }
}
