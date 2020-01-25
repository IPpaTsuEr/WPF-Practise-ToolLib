using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TabMenu
{
    public class TabMenuGroupItem : ContentControl
    {
        static TabMenuGroupItem()
        {
           //DefaultStyleKeyProperty.OverrideMetadata(
           //     typeof(TabMenuGroupItem),
           //     new System.Windows.PropertyMetadata(typeof(TabMenuGroupItem)));
        }
        public TabMenuGroupItem() : base() {
            DefaultStyleKey = typeof(TabMenuGroupItem);
        }

        public Orientation Direction
        {
            get { return (Orientation)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(Orientation), typeof(TabMenuGroupItem), new PropertyMetadata(Orientation.Horizontal));

    }
}
