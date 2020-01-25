using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ToolsLibrary.Events
{
   public class FlippedEventArgs : RoutedEventArgs
    {
        public FlippedEventArgs(RoutedEvent routedEvent, object sender): base(routedEvent,sender)
        {

        }

        public bool IsFilpped
        {
            get;
            set;
        }
    }
}
