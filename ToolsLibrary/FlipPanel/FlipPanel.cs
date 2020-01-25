using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToolsLibrary.Events;

namespace ToolsLibrary
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:FlipPanel"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:FlipPanel;assembly=FlipPanel"
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
    public class FlipPanel : Control
    {
        static FlipPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipPanel), new FrameworkPropertyMetadata(typeof(FlipPanel)));

        }

        public static readonly DependencyProperty FontContentProperty = DependencyProperty.Register("FontContent", typeof(object), typeof(FlipPanel), null);
        public object FontContent
        {
            get { return GetValue(FontContentProperty); }
            set { SetValue(FontContentProperty, value); }
        }
        public static readonly DependencyProperty BackContentProperty = DependencyProperty.Register("BackContent", typeof(object), typeof(FlipPanel), null);
        public object BackContent
        {
            get { return GetValue(BackContentProperty); }
            set { SetValue(BackContentProperty, value); }
        }
        public static readonly DependencyProperty IsFilppedProperty = DependencyProperty.Register("IsFilpped", typeof(bool), typeof(FlipPanel), new PropertyMetadata(false));
        public bool IsFilpped
        {
            get { return (bool)GetValue(IsFilppedProperty); }
            set { SetValue(IsFilppedProperty, value); ChangeVisualStates(true); }//
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ToggleButton tg = (ToggleButton)base.GetTemplateChild("FlipButton");
            if(tg!=null) tg.Click += Tg_Click;

        }

        private void Tg_Click(object sender, RoutedEventArgs e)
        {
            IsFilpped = !IsFilpped;
            OnFlipped();
        }

        private void ChangeVisualStates(bool useTransitions)
        {
            if (IsFilpped) {
                VisualStateManager.GoToState(this,"Flipped",useTransitions);
            }
            else{
               VisualStateManager.GoToState(this,"Normal" , useTransitions);
            }
        }

        public static readonly RoutedEvent FlippedEvent =
            EventManager.RegisterRoutedEvent("Flipped", RoutingStrategy.Bubble, typeof(EventHandler<FlippedEventArgs>), typeof(FlipPanel));

        public event RoutedEventHandler FlippedHandler
        {
            add { AddHandler(FlippedEvent,value); }
            remove { RemoveHandler(FlippedEvent, value); }
        }

        protected virtual void PreviewFlipped()
        {

        }
        protected virtual void OnFlipped()
        {
            FlippedEventArgs arg = new FlippedEventArgs(FlippedEvent, this);
            arg.IsFilpped = IsFilpped;
            this.RaiseEvent(arg);
        }
    }
}
