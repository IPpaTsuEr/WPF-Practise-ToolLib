using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TabMenu
{
    public class TabMenuGroup : ItemsControl
    {
        static TabMenuGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TabMenuGroup), 
                new FrameworkPropertyMetadata(typeof(TabMenuGroup)));
        }
        public TabMenuGroup() : base()
        {

        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TabMenuGroupItem; ;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabMenuGroupItem();
        }

        public Orientation Direction
        {
            get { return (Orientation)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(Orientation), typeof(TabMenuGroup), new PropertyMetadata(Orientation.Horizontal));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty,value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title",typeof(string),typeof(TabMenuGroup),new PropertyMetadata(null));
    }
}
