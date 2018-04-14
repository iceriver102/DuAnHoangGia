using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DuAnHoangGia.Behaviors
{
    public interface IEffectBehavior
    {
        void RunAnimation();
    }

    public class EffectBehavior : BehaviorBase<View>
    {
        public static readonly BindableProperty TriggerProperty = BindableProperty.Create("Trigger", typeof(bool), typeof(EffectBehavior), false, BindingMode.OneWay, propertyChanging: (bind, o, n) =>
        {
            if (n is bool flag && n != o)
            {
                if (bind is EffectBehavior effect)
                {
                    if (flag)
                    {
                        effect.RunAnimation();
                    }
                    else
                    {
                        if (effect.anim != null)
                            effect.AssociatedObject.AbortAnimation(effect.ID.ToString());
                    }
                }
            }
        });
        public Animation anim;
        public bool Loop { get; set; } = false;

        public Guid ID { get; set; } = Guid.NewGuid();

        public bool Trigger
        {
            get { return (bool)GetValue(TriggerProperty); }
            set { SetValue(TriggerProperty, value); }
        }

        public int Time { get; set; } = 250;
        public double To { get; set; } = 1;
        public double From { get; set; } = 0;
        public bool Reverse { get; set; } = false;
        public virtual void RunAnimation()
        {
            if (this.anim == null)
                return;
            this.anim.Commit(this.AssociatedObject, this.ID.ToString(), length: (uint)this.Time, repeat: () => this.Loop,rate:16);
        }
    }
}
