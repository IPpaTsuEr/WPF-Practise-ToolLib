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

namespace SearchBox
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:SearchBox"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:SearchBox;assembly=SearchBox"
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
    public class SearchBox : Control
    {


        static SearchBox()
        {
            
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBox), new FrameworkPropertyMetadata(typeof(SearchBox)));
        }
        public SearchBox()
        {
            Loaded += SearchBox_Loaded;
        }

        private void SearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            AddClickHandler(this);
            //object sbo = this.Template.FindName("PART_SearchButton", (FrameworkElement)this.Parent); ;
            //if (sbo != null && sbo.GetType() == typeof(Button))
            //{
            //    Button bpd = sbo as Button;
            //    bpd.Click += Bpd_Click;

            //}
            this.DataContext = this;
        }

        public void AddClickHandler(DependencyObject dpo)
        {
            //PART_SearchButton
            int size = VisualTreeHelper.GetChildrenCount(dpo);
            for(int i =0;i<size;i++)
            {
                DependencyObject dp = VisualTreeHelper.GetChild(dpo, i);

                if (VisualTreeHelper.GetChildrenCount(dp) > 0) AddClickHandler(dp);
                
                if (dp.GetType() == typeof(Button))
                {
                    Button bpd = dp as Button;
                    if(bpd.Name == "PART_SearchButton")
                    {
                        bpd.Click += Bpd_Click;
                        
                    }
                    else if (bpd.Name == "PART_ClearButton")
                    {
                        bpd.Click += Bt_Click;
                       
                    }
                }else if(dp.GetType() == typeof(TextBox))
                {
                    TextBox tdp = dp as TextBox;
                    if (tdp.Name == "PART_SearchText")
                    {
                        tdp.TextChanged += Tdp_TextChanged;
                        tdp.KeyUp += Tdp_KeyUp;
                    }
                }
            }   
        }

        private void Bt_Click(object sender, RoutedEventArgs e)
        {
            SearchText = "";

        }

        private void Tdp_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.IsUp && e.Key==Key.Return) RaiseEvent(new RoutedEventArgs(SearchEvent, e));
            //Console.WriteLine(e.Key);
        }

        private void Tdp_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            SearchText = tb.Text;
            if(RealTimeSearch) RaiseEvent(new RoutedEventArgs(SearchEvent, e));
            //Console.WriteLine("[Search Text ] "+SearchText + "[Input Text ] "+ tb.Text);
        }

        private void Bpd_Click(object sender, RoutedEventArgs e)
        {
            
            //Console.WriteLine("RaiseEvent : "+ SearchText);
            
            RaiseEvent(new RoutedEventArgs(SearchEvent, SearchText));
        }

        public event RoutedEventHandler OnSearchClick
        {
            add { AddHandler(SearchEvent, value); }
            remove { RemoveHandler(SearchEvent, value); }
        }

        public static readonly RoutedEvent SearchEvent = EventManager.RegisterRoutedEvent("OnSearchClick",
            RoutingStrategy.Bubble,typeof(RoutedEventHandler),typeof(SearchBox));

       





        public bool RealTimeSearch
        {
            get { return (bool)GetValue(RealTimeSearchProperty); }
            set { SetValue(RealTimeSearchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RealTimeSearch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RealTimeSearchProperty =
            DependencyProperty.Register("RealTimeSearch", typeof(bool), typeof(SearchBox), new PropertyMetadata(false));



        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }
        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(SearchBox), new PropertyMetadata(new CornerRadius(10)));




        public double SearchButtonHeight
        {
            get { return (double)GetValue(SearchButtonHeightProperty); }
            set { SetValue(SearchButtonHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchButtonHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchButtonHeightProperty =
            DependencyProperty.Register("SearchButtonHeight", typeof(double), typeof(SearchBox), new PropertyMetadata(24.0));



        public double SearchButtonWidth
        {
            get { return (double)GetValue(SearchButtonWidthProperty); }
            set { SetValue(SearchButtonWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchButtonWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchButtonWidthProperty =
            DependencyProperty.Register("SearchButtonWidth", typeof(double), typeof(SearchBox), new PropertyMetadata(24.0));



        public object SearchIcon
        {
            get { return (object)GetValue(SearchIconProperty); }
            set { SetValue(SearchIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchIconProperty =
            DependencyProperty.Register("SearchIcon", typeof(object), typeof(SearchBox), new PropertyMetadata("O"));



        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(SearchBox), new PropertyMetadata(""));



    }
}
