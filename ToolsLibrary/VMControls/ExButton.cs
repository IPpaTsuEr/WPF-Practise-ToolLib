using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace VMControls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:VMControls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:VMControls;assembly=VMControls"
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
    public class ExButton : Control
    {
        static ExButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExButton), new FrameworkPropertyMetadata(typeof(ExButton)));
        }

        public ExButton()
        {
            Loaded += ExButton_Loaded;
        }

        private void ExButton_Loaded(object sender, RoutedEventArgs e)
        {
            AddClickHandler(this);
        }


        public void AddClickHandler(DependencyObject dpo)
        {
            //PART_SearchButton
            int size = VisualTreeHelper.GetChildrenCount(dpo);
            for (int i = 0; i < size; i++)
            {
                DependencyObject dp = VisualTreeHelper.GetChild(dpo, i);

                if (VisualTreeHelper.GetChildrenCount(dp) > 0) AddClickHandler(dp);

                if (dp.GetType() == typeof(Button))
                {
                    Button PART_Enter = dp as Button;
                    if (PART_Enter.Name == "PART_Enter")
                    {
                        PART_Enter.Click += PART_Enter_Click; ;

                    }
                }
                else if (dp.GetType() == typeof(ComboBox))
                {
                    ComboBox PART_ChilderList = dp as ComboBox;
                    if (PART_ChilderList.Name == "PART_ChilderList")
                    {
                        PART_ChilderList.SelectionChanged += PART_ChilderList_SelectionChanged;
                    }
                }
            }
        }



        private void PART_ChilderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ExButtonEvent, e.AddedItems[0]));
            
        }

        private void PART_Enter_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ExButtonEvent,DataContext));
           
        }

        #region 事件

        static readonly RoutedEvent ExButtonEvent = 
            EventManager.RegisterRoutedEvent("OnEnterTarget", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ExButton));

        public event RoutedEventHandler OnEnterTarget
        {
            add { AddHandler(ExButtonEvent,value); }
            remove { RemoveHandler(ExButtonEvent,value); }
        }

        #endregion

        #region 依赖属性

        public IEnumerable<object> ChildrenSource
        {
            get { return (IEnumerable<object>)GetValue(ChildrenSourceProperty); }
            set { SetValue(ChildrenSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Collection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChildrenSourceProperty =
            DependencyProperty.Register("ChildrenSource", typeof(IEnumerable<object>), typeof(ExButton), new PropertyMetadata(null));
        

        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register("DisplayName", typeof(string), typeof(ExButton), new PropertyMetadata(null));



        public Visibility ChildVisiable
        {
            get { return (Visibility)GetValue(ChildVisiableProperty); }
            set { SetValue(ChildVisiableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasChiledren.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChildVisiableProperty =
            DependencyProperty.Register("ChildVisiable", typeof(Visibility), typeof(ExButton), new PropertyMetadata(Visibility.Collapsed));

        #endregion
    }
}
