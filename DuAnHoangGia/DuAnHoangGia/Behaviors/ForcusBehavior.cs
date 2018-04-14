using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DuAnHoangGia.Behaviors
{
    public class ForcusBehavior : BehaviorBase<InputView>
    {
        public static readonly BindableProperty TriggerProperty = BindableProperty.Create("Trigger", typeof(bool), typeof(ForcusBehavior), false, BindingMode.OneWay, propertyChanged: (bind, o, n) =>
        {
            if (n is bool flag )
            {
                if (bind is ForcusBehavior input && input.AssociatedObject!=null)
                {
                    if (flag && input.Auto)
                        input.AssociatedObject.Focus();
                    else if(!flag)
                        input.AssociatedObject.Unfocus();
                }
            }
        });

        public bool Auto { get; set; } = false;
        public bool Trigger
        {
            get { return (bool)GetValue(TriggerProperty); }
            set { SetValue(TriggerProperty, value); }
        }

    }
}
