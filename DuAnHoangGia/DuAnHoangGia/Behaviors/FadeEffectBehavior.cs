using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DuAnHoangGia.Behaviors
{
    public class FadeEffectBehavior : EffectBehavior
    {
        public override void RunAnimation()
        {
            if (this.anim != null)
            {
                AssociatedObject.AbortAnimation(ID.ToString());
            }
            this.anim = new Animation
                {
                    { 0,1, new Animation(v => this.AssociatedObject.Opacity = v, this.From, this.To) },
                };
            base.RunAnimation();
        }
    }
}
