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
    [Activity(Label = "Continue", Icon = "@drawable/icon", Theme = "@style/NoActionBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, NoHistory = true)]
    public class Continue : Activity
    {
        string[] texts = { "Thank you, now that you've stopped the turbines, you don’t have much time. The ship has almost no energy left. Thanks to you my memory is restored.",
        "In fact, Captain Nemo has long since left me, but I want to believe that he will return! The captain was so self-assured that he went to fight with a kraken with one harpoon. ",
        "I waited a long 127 years of my master. Thank you, brave travelers, for having avenged my master. Now I can go to the bottom with a calm soul.",
        "The exit door is unlocked, you can leave the ship." };
        TextView txt;
        byte iterator = 1;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            this.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)
                          (SystemUiFlags.LayoutStable
                          | SystemUiFlags.LayoutHideNavigation
                          | SystemUiFlags.LayoutFullscreen
                          | SystemUiFlags.HideNavigation
                          | SystemUiFlags.Fullscreen
                          | SystemUiFlags.Immersive);
            SetContentView(Resource.Layout.text_continue);
            txt = FindViewById<TextView>(Resource.Id.textView_cont);
            txt.SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
            txt.Text = texts[0];

            txt.Click += delegate
            {
                if (iterator == 0) return;
                if (iterator < texts.Length)
                {
                    txt.Text = texts[iterator];
                    iterator++;
                }
                else
                {
                    new Task(() =>
                    {
                        iterator = 0;
                        Thread.Sleep(10000);
                        StartActivity(typeof(Recovery));
                    }).Start();
                }
            };
        }
    }
}