using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TabMenu
{
    class Common
    {

        public static T FindVisualParent<T>(DependencyObject element) where T : DependencyObject
        {
            if (element == null) return null;
            var parent = VisualTreeHelper.GetParent(element);
            if (parent == null) return null;
            T target = parent as T;
            if (target == null) return FindVisualParent<T>(parent);
            else return target;
        }
        public static T FindLogicParent<T>(DependencyObject element) where T : DependencyObject
        {
            if (element == null) return null;
            var parent = LogicalTreeHelper.GetParent(element);
            if (parent == null) return null;
            T target = parent as T;
            if (parent == null) return FindLogicParent<T>(parent);
            else return target;
        }
    }
}
