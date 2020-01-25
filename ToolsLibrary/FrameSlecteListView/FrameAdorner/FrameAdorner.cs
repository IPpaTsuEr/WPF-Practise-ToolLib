using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FrameSlecteListView.FrameAdorner
{
    class FrameAdorner:Adorner
    {
        
        Border Selecter;
        public FrameAdorner(UIElement ue):base(ue)
        {
            Selecter = new Border();
            Selecter.Background = new SolidColorBrush(Color.FromArgb(26, 0, 62, 255));
            Selecter.BorderBrush = new SolidColorBrush(Color.FromArgb(155, 0, 82, 255));
            Selecter.BorderThickness = new Thickness(2, 2, 2, 2);
        }

        public void Display(){
            Selecter.Visibility = Visibility.Visible;
        }
        public void Hidden(){
            Selecter.Visibility = Visibility.Collapsed;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
           
            base.OnRender(drawingContext);
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            Selecter.Margin = new Thickness(0,0,0,0);
            Selecter.Width = 120;
            Selecter.Height = 120;
            return base.ArrangeOverride(finalSize);
        }
    }
}
