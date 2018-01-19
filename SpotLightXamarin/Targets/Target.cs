using Android.Graphics;
using Android.Views;

namespace SpotlightXamarin
{
    public abstract class Target
    {
        public PointF Point { get; set; }
        public float Radius { get; set; }
        public View View { get; set; }
    }
}