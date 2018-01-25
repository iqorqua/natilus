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
using System.Threading;
using Android.Graphics;

namespace natilus
{
    [Activity(Label = "natilus_dialog", Icon = "@drawable/icon", Theme = "@android:style/Theme.Dialog", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, NoHistory = true)]
    class Dialog : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.RequestWindowFeature(WindowFeatures.NoTitle);
            this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            this.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)
                          (SystemUiFlags.LowProfile
                          | SystemUiFlags.Fullscreen
                          | SystemUiFlags.HideNavigation
                          | SystemUiFlags.Immersive
                          | SystemUiFlags.ImmersiveSticky);

            var question = Intent.GetStringExtra("question");
            var answer = Intent.GetStringExtra("answer");
            SetContentView(Resource.Layout.dialog);

            FindViewById<Button>(Resource.Id.ok).Click += delegate
            {
                Finish();
            };

            //FindViewById<TextView>(Resource.Id.question_textview).Text = question;
            FindViewById<TextView>(Resource.Id.answer_textview).Text = answer;
            FindViewById<TextView>(Resource.Id.question_textview).SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.answer_textview).SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);

        }
    }
}