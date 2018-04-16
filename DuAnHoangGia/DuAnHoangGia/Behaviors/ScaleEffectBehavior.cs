using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DuAnHoangGia.Behaviors
{
    public class ScaleEffectBehavior:EffectBehavior
    {
        public override void RunAnimation()
        {
            if (this.anim == null)
            {
                this.anim = new Animation();
                if (this.Reverse)
                {
                    this.anim.Add(0, 0.5, new Animation(v => this.AssociatedObject.Scale = v, this.From, this.To, Easing.BounceOut));
                    this.anim.Add(0.5, 1, new Animation(v => this.AssociatedObject.Scale = v, this.To, this.From,Easing.Linear));
                }
                else
                {
                    this.anim.Add(0, 1, new Animation(v => this.AssociatedObject.Scale = v, this.From, this.To, Easing.BounceOut));
                }
            }
            base.RunAnimation();
        }
    }
}
