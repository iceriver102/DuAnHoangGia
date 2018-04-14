using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DuAnHoangGia.Behaviors
{
    public class TransalteEffectBehavior : EffectBehavior
    {
        public override void RunAnimation()
        {
            if (this.anim != null)
            {
                AssociatedObject.AbortAnimation(ID.ToString());
            }
            this.anim = new Animation();

            if (this.FromX != this.ToX)
            {
                if (this.Reverse)
                {
                    this.anim.Add(0, 0.5, new Animation(v => this.AssociatedObject.TranslationX = v, this.FromX, this.ToX)); this.anim.Add(0.5, 1, new Animation(v => this.AssociatedObject.TranslationX = v, this.ToX, this.FromX, Easing.SpringOut));
                }
                else
                {
                    this.anim.Add(0, 1, new Animation(v => this.AssociatedObject.TranslationX = v, this.FromX, this.ToX));
                }
            }
            if (this.FromY != this.ToY)
            {
                if (this.Reverse)
                {
                    this.anim.Add(0, 0.5, new Animation(v => this.AssociatedObject.TranslationX = v, this.FromY, this.ToY));
                    this.anim.Add(0.5, 1, new Animation(v => this.AssociatedObject.TranslationX = v, this.ToY, this.FromY, Easing.SpringOut));
                }
                else
                    this.anim.Add(0, 1, new Animation(v => this.AssociatedObject.TranslationY = v, this.FromY, this.ToY));
            }
            base.RunAnimation();
        }

        public double FromX { get; set; } = 0;
        public double ToX { get; set; } = 0;
        public double FromY { get; set; } = 0;
        public double ToY { get; set; } = 0;

    }
}
