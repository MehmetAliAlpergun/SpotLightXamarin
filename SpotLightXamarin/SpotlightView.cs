using System;
using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SpotlightXamarin
{
    public class SpotlightView : FrameLayout, View.IOnClickListener
    {
        public delegate void OnTargetClosedHandler();
        public OnTargetClosedHandler OnTargetClosed;

        public delegate void OnTargetClickHandler();
        public OnTargetClickHandler OnTargetClicked;

        private Paint BackgroundPaint = new Paint();
        private Paint SpotPaint = new Paint();
        private PointF Point = new PointF();
        private ValueAnimator Animator;

        public SpotlightView(Context context) : base(context)
        {
            Initialize();
        }

        public SpotlightView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public SpotlightView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }

        public SpotlightView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize();
        }

        protected SpotlightView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private void Initialize()
        {
            BringToFront();
            SetWillNotDraw(false);
            SetLayerType(LayerType.Hardware, null);
            SpotPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.Clear));
            SetOnClickListener(this);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            BackgroundPaint.Color = Color.ParseColor("#CC000000");
            canvas.DrawRect(0, 0, canvas.Width, canvas.Height, BackgroundPaint);

            if (Animator != null)
            {
                canvas.DrawCircle(Point.X, Point.Y, (float)Animator.AnimatedValue, SpotPaint);
            }
        }

        public void TurnUp(float x, float y, float radius, long duration, ITimeInterpolator animation)
        {
            Point.Set(x, y);

            Animator = ValueAnimator.OfFloat(0f, radius);
            Animator.Update += (sender, e) =>
            {
                Invalidate();
            };

            Animator.SetInterpolator(animation);
            Animator.SetDuration(duration);
            Animator.Start();
        }

        public void TurnDown(float radius, long duration, ITimeInterpolator animation)
        {
            Animator = ValueAnimator.OfFloat(radius, 0f);
            Animator.Update += (sender, e) => 
            {
                Invalidate();
            };

            Animator.AnimationEnd += (sender, e) => 
            {
                OnTargetClosed?.Invoke();
            };

            Animator.SetInterpolator(animation);
            Animator.SetDuration(duration);
            Animator.Start();
        }

        public void OnClick(View v)
        {
            if (Animator != null && !Animator.IsRunning && (float)Animator.AnimatedValue > 0)
            {
                OnTargetClicked?.Invoke();
            }
        }
    }
}