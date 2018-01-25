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
using Android.Graphics.Drawables;

namespace natilus
{
    [Activity(Label = "natilus", Icon = "@drawable/icon", Theme = "@style/NoActionBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    class DesignedGame : Activity
    {
        private Button b1;
        private Button b2;
        private Button b3;
        private Button b4;
        private Button b5;
        private Button b6;

        private ImageView im1;
        private ImageView im2;
        private ImageView im3;
        private ImageView im4;
        private ImageView im5;
        private ImageView im6;

        private Dictionary<string, bool> stats = new Dictionary<string, bool>();

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


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.designedGame);

            b1 = FindViewById<Button>(Resource.Id.button1);
            b2 = FindViewById<Button>(Resource.Id.button2);
            b3 = FindViewById<Button>(Resource.Id.button3);
            b4 = FindViewById<Button>(Resource.Id.button4);
            b5 = FindViewById<Button>(Resource.Id.button5);
            b6 = FindViewById<Button>(Resource.Id.button6);
            stats.Add(b1.Tag.ToString(), false);
            stats.Add(b2.Tag.ToString(), false);
            stats.Add(b3.Tag.ToString(), false);
            stats.Add(b4.Tag.ToString(), false);
            stats.Add(b5.Tag.ToString(), false);
            stats.Add(b6.Tag.ToString(), false);

            b1.Click += Tumbler_Click;
            b2.Click += Tumbler_Click;
            b3.Click += Tumbler_Click;
            b4.Click += Tumbler_Click;
            b5.Click += Tumbler_Click;
            b6.Click += Tumbler_Click;


            FindViewById<Button>(Resource.Id.button_close).Click += (object s, EventArgs ee) => { this.Finish(); };
        }

        private void Tumbler_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            bool val = false;
            if (stats.TryGetValue(button.Tag.ToString(), out val))
            {
                if (!val)
                {
                    FindViewById<ImageView>(Resources.GetIdentifier(button.Tag.ToString(), "id", this.PackageName)).SetBackgroundResource(Resources.GetIdentifier(("bk"+ button.Tag.ToString()[1])+2, "drawable", this.PackageName));
                    stats[button.Tag.ToString()] = true;
                }
                else
                {
                    FindViewById<ImageView>(Resources.GetIdentifier(button.Tag.ToString(), "id", this.PackageName)).SetBackgroundResource(Resources.GetIdentifier(("bk" + button.Tag.ToString()[1]) + 1, "drawable", this.PackageName));
                    stats[button.Tag.ToString()] = false;
                }
            }
            PaintReactors();
            CheckStausAchievmwnt();
        }

        private void CheckStausAchievmwnt()
        {
            if (stats[b1.Tag.ToString()] & stats[b2.Tag.ToString()] & !stats[b3.Tag.ToString()] & stats[b4.Tag.ToString()] & !stats[b5.Tag.ToString()] & !stats[b6.Tag.ToString()])
            {
                StartActivityForResult(typeof(Continue), 0);
            }
        }

        private void PaintReactors()
        {/*         1 tum            */
            if (stats[b2.Tag.ToString()] && !stats[b3.Tag.ToString()])
                FindViewById<ImageView>(Resource.Id.r1).SetBackgroundResource(Resource.Drawable.r1_green);
            else
            {
                if (!stats[b2.Tag.ToString()])
                {
                    if (stats[b4.Tag.ToString()] | !stats[b1.Tag.ToString()])
                    {
                        FindViewById<ImageView>(Resource.Id.r1).SetBackgroundResource(Resource.Drawable.r1_green);
                    }
                    if (!stats[b4.Tag.ToString()] | stats[b1.Tag.ToString()])
                    {
                        FindViewById<ImageView>(Resource.Id.r1).SetBackgroundResource(Resource.Drawable.r1_red);
                    }

                }

                if (stats[b3.Tag.ToString()])
                {
                    if (stats[b6.Tag.ToString()])
                    {
                        FindViewById<ImageView>(Resource.Id.r1).SetBackgroundResource(Resource.Drawable.r1_red);
                    }
                }

                if (!stats[b2.Tag.ToString()] & stats[b3.Tag.ToString()])
                {
                    if (!stats[b6.Tag.ToString()])
                    {
                        FindViewById<ImageView>(Resource.Id.r1).SetBackgroundResource(Resource.Drawable.r1_red);
                    }
                    if (stats[b6.Tag.ToString()] & !stats[b1.Tag.ToString()])
                    {
                        FindViewById<ImageView>(Resource.Id.r1).SetBackgroundResource(Resource.Drawable.r1_green);
                    }
                }

                if (!stats[b1.Tag.ToString()] & stats[b6.Tag.ToString()] & stats[b2.Tag.ToString()] & stats[b3.Tag.ToString()])
                {
                    FindViewById<ImageView>(Resource.Id.r1).SetBackgroundResource(Resource.Drawable.r1_green);
                }

                if (stats[b3.Tag.ToString()] & !stats[b6.Tag.ToString()])
                {
                    FindViewById<ImageView>(Resource.Id.r1).SetBackgroundResource(Resource.Drawable.r1_red);
                }
                else
                    FindViewById<ImageView>(Resource.Id.r1).SetBackgroundResource(Resource.Drawable.r1_green);

                if(!stats[b2.Tag.ToString()]&(!stats[b4.Tag.ToString()] | stats[b1.Tag.ToString()]))
                {
                    FindViewById<ImageView>(Resource.Id.r1).SetBackgroundResource(Resource.Drawable.r1_red);
                }
            }

            /*         2 tum            */
            if (stats[b6.Tag.ToString()] | stats[b5.Tag.ToString()] | stats[b3.Tag.ToString()])
                FindViewById<ImageView>(Resource.Id.r2).SetBackgroundResource(Resource.Drawable.r2_red);
            else
                FindViewById<ImageView>(Resource.Id.r2).SetBackgroundResource(Resource.Drawable.r2_green);


            /*         3 tum            */
            if (!stats[b1.Tag.ToString()] | !stats[b2.Tag.ToString()] | ((!(!stats[b5.Tag.ToString()] & stats[b4.Tag.ToString()]))& !stats[b5.Tag.ToString()]))
                FindViewById<ImageView>(Resource.Id.r3).SetBackgroundResource(Resource.Drawable.r3_red);
            else
                FindViewById<ImageView>(Resource.Id.r3).SetBackgroundResource(Resource.Drawable.r3_green);

        }
    }
}