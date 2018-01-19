using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SpotlightXamarin
{
    public class SimpleTarget : Target
    {
        public SimpleTarget(PointF point, float radius, View view)
        {
            Point = point;
            Radius = radius;
            View = view;
        }
    }

    public class SimpleTargetBuilder : AbstractBuilder<SimpleTargetBuilder, SimpleTarget>
    {
        enum ContainerPosition
        {
            Above = 0,
            Below = 1
        }

        private string Title;
        private string Description;

        public SimpleTargetBuilder(Activity context) : base(context)
        {
        }

        public override SimpleTarget Build()
        {
            if (Context == null)
            {
                throw new Exception("context is null");
            }

            View view = LayoutInflater.FromContext(Context).Inflate(SpotlightXamarin.Resource.Layout.layout_spotlight, null);

            ((TextView)view.FindViewById(Resource.Id.title)).SetText(Title, TextView.BufferType.Normal);
            ((TextView)view.FindViewById(Resource.Id.description)).SetText(Description, TextView.BufferType.Normal);

            PointF point = new PointF(StartX, StartY);
            CalculatePosition(point, Radius, view);

            return new SimpleTarget(point, Radius, view);
        }

        protected override SimpleTargetBuilder Self()
        {
            return this;
        }

        public SimpleTargetBuilder SetTitle(string title)
        {
            Title = title;
            return this;
        }

        public SimpleTargetBuilder SetDescription(string description)
        {
            Description = description;
            return this;
        }

        private void CalculatePosition(PointF point, float radius, View spotlightView)
        {
            Point screenSize = new Point();

            IWindowManager windowManager = spotlightView.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            windowManager.DefaultDisplay.GetSize(screenSize);

            float aboveArea = point.Y / screenSize.Y;
            float belowArea = (screenSize.Y - point.Y) / screenSize.Y;

            ContainerPosition containerPosition;

            if (aboveArea > belowArea)
            {
                containerPosition = ContainerPosition.Above;
            }
            else
            {
                containerPosition = ContainerPosition.Below;
            }

            LinearLayout layout = spotlightView.FindViewById<LinearLayout>(Resource.Id.container);
            layout.SetPadding(100, 0, 100, 0);

            switch (containerPosition)
            {
                case ContainerPosition.Above:
                    spotlightView.ViewTreeObserver.GlobalLayout += (sender, e) =>
                    {
                        layout.SetY(point.Y - radius - 100 - layout.Height);
                    };
                    break;
                case ContainerPosition.Below:
                    layout.SetY((int)(point.Y + radius + 100));
                    break;
            }
        }
    }
}