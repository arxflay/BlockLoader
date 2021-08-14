using System.Windows;

namespace BlockLoader.PresentationLayer
{
    //binding hack
    public class BindingProxy : Freezable
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy));
        public object Data { get; set; }
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}
