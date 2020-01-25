using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace FrameSlecteListView
{
    class FrameSelectedListViewItem : ListViewItem
    {
        static FrameSelectedListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FrameSelectedListViewItem), new FrameworkPropertyMetadata(typeof(FrameSelectedListViewItem)));
        }
        public FrameSelectedListViewItem()
        {
            Loaded += FrameSelectedListViewItem_Loaded;

            // FrameSelectListViewItem.FrameSelectedEvent =  FrameSelectListView.FrameSelectedEvent.AddOwner(typeof(FrameSelectListViewItem));
            // FrameSelectedHandler += OnFrameSelected;
        }

        public void FrameSelect(ref Point start, ref Point end)
        {
            if (FrameSelectAble)
            {
                Point location = (Point)VisualTreeHelper.GetOffset(this);
                Point Elocation = new Point(location.X + ActualWidth, location.Y + ActualHeight);
                if (Elocation.X < start.X || location.X > end.X) IsSelected = false;
                else if (location.Y > end.Y || Elocation.Y < start.Y) IsSelected = false;
                else IsSelected = true;
            }

        }

        private void FrameSelectedListViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            RelativeSource rs = new RelativeSource(RelativeSourceMode.FindAncestor);
            //设定为离自己控件最近的第一层父控件  
            rs.AncestorLevel = 1;
            //设定父控件类型
            rs.AncestorType = typeof(FrameSelectedListView);
            Binding binding = new Binding("FrameSelectMode")
            {
                RelativeSource = rs,
                Mode = BindingMode.OneWay
            };
            this.SetBinding(FrameSelectAbleProperty, binding);
        }

        public bool FrameSelectAble
        {
            get { return (bool)GetValue(FrameSelectAbleProperty); }
            set { SetValue(FrameSelectAbleProperty, value); }
        }

        public static RoutedEvent FrameSelectedEvent { get; private set; }

        public static readonly DependencyProperty FrameSelectAbleProperty =
            DependencyProperty.Register("FrameSelectAble", typeof(bool), typeof(FrameSelectedListView), new PropertyMetadata(false));

    }
}
