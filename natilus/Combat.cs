using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Animation;
using Android.Graphics;
using System.Threading.Tasks;
using System.Threading;

namespace natilus
{
    [Activity(Label = "Alarm", Icon = "@drawable/icon", Theme = "@style/NoActionBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, NoHistory = true)]
    public class Combat : Activity
    {
        private static TextView textAlarm;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.RequestWindowFeature(WindowFeatures.NoTitle);
            this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            this.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)
                          (SystemUiFlags.LayoutStable
                          | SystemUiFlags.LayoutHideNavigation
                          | SystemUiFlags.LayoutFullscreen
                          | SystemUiFlags.HideNavigation
                          | SystemUiFlags.Fullscreen
                          | SystemUiFlags.Immersive);

            SetContentView(Resource.Layout.combat);

            textAlarm = FindViewById<TextView>(Resource.Id.combat_txt_viev);
            DialogQuestions.mode = DialogQuestions.combat;
            var textColorAnim = ObjectAnimator.OfInt(textAlarm, "textColor", Color.Black, Color.Transparent);
            textColorAnim.SetDuration(500);
            textColorAnim.SetEvaluator(new ArgbEvaluator());
            textColorAnim.RepeatCount = ValueAnimator.Infinite;
            textColorAnim.RepeatMode = ValueAnimatorRepeatMode.Reverse;
            textColorAnim.Start();
            textAlarm.SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
            textAlarm.Click += delegate
            {
                Finish();
            };
          
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            StartActivity(intent);
        }
    }
}