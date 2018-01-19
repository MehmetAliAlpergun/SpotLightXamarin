using System;
using System.Collections.Generic;
using System.Linq;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace SpotlightXamarin
{
    public class Spotlight
    {
        public delegate void OnSpotlightStartedHandler();
        public OnSpotlightStartedHandler OnSpotlightStarted;

        public delegate void OnSpotlightEndedHandler();
        public OnSpotlightEndedHandler OnSpotlightEnded;

        public delegate void OnTargetStartedHandler(Target target);
        public OnTargetStartedHandler OnTargetStarted;

        public delegate void OnTargetEndedHandler(Target target);
        public OnTargetEndedHandler OnTargetEnded;

        private SpotlightView SpotlightView;
        private Context Context;
        private List<Target> Targets;
        private long Duration;
        private ITimeInterpolator Animation;

        private long StartStoplightAnimationDuration = 500;
        private long FinishStoplightAnimationDuration = 500;

        public Spotlight(Context context,List<Target> targets, long duration, ITimeInterpolator animation )
        {
            Context = context;
            Targets = targets;
            Duration = duration;
            Animation = animation;
        }

        public void Start()
        {
            if (Context == null)
            {
                throw new Exception("Spotlight: Context is null");
            }

            SpotlightView = new SpotlightView(Context)
            {
                LayoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
            };

            View decorView = ((Activity)Context).Window.DecorView;
            ((ViewGroup)decorView).AddView(SpotlightView);

            SpotlightView.OnTargetClosed += () =>
            {
                if (Targets.Count > 0)
                {
                    Target target = Targets[0];

                    OnTargetEnded?.Invoke(target);

                    Targets.Remove(target);

                    if (Targets.Count > 0)
                    {
                        StartTarget();
                    }
                    else
                    {
                        FinishSpotlight();
                    }
                }
            };

            SpotlightView.OnTargetClicked += () =>
            {
                FinishTarget();
            };

            StartSpotlight();
        }

        private void StartTarget()
        {
            if (Targets != null && Targets.Count > 0)
            {
                Target target = Targets[0];

                SpotlightView.RemoveAllViews();
                SpotlightView.AddView(target.View);

                SpotlightView.TurnUp(target.Point.X, target.Point.Y, target.Radius, Duration, Animation);

                OnTargetStarted?.Invoke(target);
            }
        }

        private void StartSpotlight()
        {
            ObjectAnimator objectAnimator = ObjectAnimator.OfFloat(SpotlightView, "alpha", 0f, 1f);
            objectAnimator.SetDuration(StartStoplightAnimationDuration);

            objectAnimator.AnimationStart += (sender, e) =>
            {
                OnSpotlightStarted?.Invoke();
            };

            objectAnimator.AnimationEnd += (sender, e) =>
            {
                StartTarget();
            };

            objectAnimator.Start();
        }

        private void FinishTarget()
        {
            if (Targets != null && Targets.Count > 0)
            {
                Target target = Targets[0];
                SpotlightView.TurnDown(target.Radius, Duration, Animation);
            }
        }

        private void FinishSpotlight()
        {
            ObjectAnimator objectAnimator = ObjectAnimator.OfFloat(SpotlightView, "alpha", 1f, 0f);
            objectAnimator.SetDuration(FinishStoplightAnimationDuration);

            objectAnimator.AnimationEnd += (sender, e) =>
            {
                View decorView = ((Activity)Context).Window.DecorView;
                ((ViewGroup)decorView).RemoveView(SpotlightView);

                OnSpotlightEnded?.Invoke();
            };

            objectAnimator.Start();
        }
    }

    public class SpotlightBuilder
    {
        private Context Context;
        private List<Target> Targets;
        private long Duration = 1000L;
        private ITimeInterpolator Animation = new DecelerateInterpolator(2f);

        public SpotlightBuilder(Context context)
        {
            Context = context;
        }

        public SpotlightBuilder SetTargets(List<Target> targets)
        {
            Targets = targets;
            return this;
        }

        public SpotlightBuilder SetTargets(params Target[] targets)
        {
            Targets = targets.ToList();
            return this;
        }

        public SpotlightBuilder SetDuration(long duration)
        {
            Duration = duration;
            return this;
        }

        public SpotlightBuilder SetAnimation(ITimeInterpolator animation)
        {
            Animation = animation;
            return this;
        }

        public Spotlight Start()
        {
            Spotlight spotlight = new Spotlight(Context, Targets, Duration, Animation);
            spotlight.Start();

            return spotlight;
        }
    }
}