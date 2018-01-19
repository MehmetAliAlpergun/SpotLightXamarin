using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;

namespace SpotlightXamarin
{
    public abstract class AbstractBuilder<T, S>
    {
        protected Context Context;
        protected float StartX = 0f;
        protected float StartY = 0f;
        protected float Radius = 100f;

        protected abstract T Self();
        public abstract S Build();

        protected AbstractBuilder(Activity context)
        {
            Context = context;
        }

        public T SetPoint(float x, float y)
        {
            StartX = x;
            StartY = y;

            return Self();
        }

        public T SetPoint(PointF point)
        {
            return SetPoint(point.X, point.Y);
        }

        public T SetPoint(View view)
        {
            int[] location = new int[2];
            view.GetLocationInWindow(location);

            int x = location[0] + view.Width / 2;
            int y = location[1] + view.Height / 2;

            return SetPoint(x, y);
        }

        public T SetRadius(float radius)
        {
            if (radius <= 0)
            {
                throw new Exception("Spotlight: Radius must be greater than 0");
            }

            Radius = radius;
            return Self();
        }
    }
}