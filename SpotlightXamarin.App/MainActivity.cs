using Android.App;
using Android.Widget;
using Android.OS;
using SpotlightXamarin;
using System;
using Android.Views.Animations;

namespace SpotlightXamarin.App
{
    [Activity(Label = "SpotlightXamarin.App", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            Button buttonShow = FindViewById<Button>(Resource.Id.ShowSpotlight);
            buttonShow.Click += ShowSpotlight;
        }

        private void ShowSpotlight(object sender, System.EventArgs e)
        {
            SimpleTarget firstTarget = new SimpleTargetBuilder(this).SetPoint(FindViewById(Resource.Id.FirstView))
                                                                    .SetRadius(200f)
                                                                    .SetTitle("First title")
                                                                    .SetDescription("This description is for first view.")
                                                                    .Build();

            SimpleTarget secondTarget = new SimpleTargetBuilder(this).SetPoint(FindViewById(Resource.Id.SecondView))
                                                                     .SetRadius(160f)
                                                                     .SetTitle("Second title")
                                                                     .SetDescription("This description is for second view.")
                                                                     .Build();

            SimpleTarget thirdTarget = new SimpleTargetBuilder(this).SetPoint(FindViewById(Resource.Id.ThirdView))
                                                                    .SetRadius(300f)
                                                                    .SetTitle("Third title")
                                                                    .SetDescription("This description is for third view.")
                                                                    .Build();

            Spotlight spotlight = new SpotlightBuilder(this).SetTargets(firstTarget, secondTarget, thirdTarget)
                                                            .SetDuration(1000)
                                                            .SetAnimation(new DecelerateInterpolator(2f))
                                                            .Start();


            spotlight.OnSpotlightEnded += () => 
            {
                Console.WriteLine("SpotLight Ended");
            };

            spotlight.OnSpotlightStarted += () =>
            {
                Console.WriteLine("SpotLight started");
            };

            spotlight.OnTargetStarted += (Target target) =>
            {
                Console.WriteLine($"Target started");
            };

            spotlight.OnTargetEnded += (Target target) =>
            {
                Console.WriteLine($"Target ended");
            };
        }
    }
}

