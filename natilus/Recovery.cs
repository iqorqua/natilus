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
    [Activity(Label = "Recovery", Icon = "@drawable/icon", Theme = "@style/NoActionBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, NoHistory = true)]
    public class Recovery : Activity
    {
        TextView textRecovery;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            bool sended = false;
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

            SetContentView(Resource.Layout.recovery);
            DialogQuestions.mode = DialogQuestions.recovery;
            DialogQuestions.mqttClient.Publish("nautilus/room3_cabin/tablet/mode", Encoding.UTF8.GetBytes(DialogQuestions.mode), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            textRecovery = FindViewById<TextView>(Resource.Id.blink_txt_recovery);
            FindViewById<TextView>(Resource.Id.text_flooding).SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
            textRecovery.SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
            var textColorAnim = ObjectAnimator.OfInt(textRecovery, "textColor", Color.Red, Color.Transparent);
            textColorAnim.SetDuration(500);
            textColorAnim.SetEvaluator(new ArgbEvaluator());
            textColorAnim.RepeatCount = ValueAnimator.Infinite;
            textColorAnim.RepeatMode = ValueAnimatorRepeatMode.Reverse;
            //textColorAnim.Start();
            var initTime = Intent.GetIntExtra("time", 5);
            var ticks = TimeSpan.FromSeconds(initTime);
            new Timer((object o) =>
            {
                if (ticks.TotalMilliseconds < 100)
                {
                    if(!sended)
                    {
                        RunOnUiThread(() =>
                            {
                            textRecovery.Text = String.Format("Before the Flood Remained {0:d2}:{1:d2}:{2:d3}", 0, 0, 0);
                            DialogQuestions.mode = DialogQuestions.timeout;
                            DialogQuestions.mqttClient.Publish("nautilus/room3_cabin/tablet/mode", Encoding.UTF8.GetBytes(DialogQuestions.mode), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                            sended = true;
                        });
                    }
                    return;
                }
                    
                RunOnUiThread(() => { textRecovery.Text = String.Format("Before the Flood Remained {0:d2}:{1:d2}:{2:d3}", ticks.Minutes, ticks.Seconds, ticks.Milliseconds); });
                ticks = TimeSpan.FromMilliseconds(ticks.TotalMilliseconds - 50);
            }, null, 10000, 50);
            /*new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        if (textRecovery.Visibility == ViewStates.Gone)
                            RunOnUiThread(() => textRecovery.Visibility = ViewStates.Visible);
                        else
                            RunOnUiThread(() => textRecovery.Visibility = ViewStates.Gone);
                    }
                    catch
                    {

                    }

                    Thread.Sleep(100);
                }

            }).Start();*/
        }
    }
}