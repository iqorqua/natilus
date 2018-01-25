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
using uPLibrary.Networking.M2Mqtt.Messages;

namespace natilus
{
    [Activity(Label = "Damage", Icon = "@drawable/icon", Theme = "@style/NoActionBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, NoHistory = true)]
    public class Damage : Activity
    {
        TextView textDamage;
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

            SetContentView(Resource.Layout.damage);
            DialogQuestions.mode = DialogQuestions.damage;
            textDamage = FindViewById<TextView>(Resource.Id.blink_txt_damage);
            textDamage.SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
            textDamage.Click += delegate { Finish(); };
            var textColorAnim = ObjectAnimator.OfInt(textDamage, "textColor", Color.Red, Color.Transparent);
            textColorAnim.SetDuration(500);
            textColorAnim.SetEvaluator(new ArgbEvaluator());
            textColorAnim.RepeatCount = ValueAnimator.Infinite;
            textColorAnim.RepeatMode = ValueAnimatorRepeatMode.Reverse;
            textColorAnim.Start();
            FindViewById<LinearLayout>(Resource.Id.damage_LO).Click += delegate 
            {
                Finish();
                DialogQuestions.mode = DialogQuestions.dialog;
                DialogQuestions.mqttClient.Publish("nautilus/room3_cabin/tablet/mode", Encoding.UTF8.GetBytes(DialogQuestions.mode), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.SingleTop);
                StartActivity(intent);
            };

            new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        if (textDamage.Visibility == ViewStates.Gone)
                            RunOnUiThread(()=>textDamage.Visibility = ViewStates.Visible);
                        else
                            RunOnUiThread(() => textDamage.Visibility = ViewStates.Gone);
                    }
                    catch
                    {

                    }

                    Thread.Sleep(700);
                }

            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            /*Intent intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            StartActivity(intent);*/
        }
    }
}