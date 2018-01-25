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
using Android.Hardware.Usb;
using Com.Felhr.Usbserial;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace natilus
{
    [Activity(Label = "Activity1", MainLauncher = true, Theme = "@style/NoActionBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class DialogQuestions : Activity
    {
        public static string undone = "undone", complete = "complete";
        public static string combat = "combat", dialog = "dialog", timeout = "timeout", damage = "damage", reset = "reset", recovery = "recovery", activate = "recovery";
        public static string state = undone;
        public static string mode = combat;
        public static MqttClient mqttClient;
        private static int Time_for_damage { get; set; } = 1000;
        public static void ResetTimerForDanger()
        {
            Time_for_damage = 10;
        }
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
            //********************************

            try
            {

                {
                    mqttClient = new MqttClient("m2m.eclipse.org");//MqttClient("192.168.10.10");
                    mqttClient.MqttMsgPublishReceived += (object sender, MqttMsgPublishEventArgs e) =>
                    {

                        RunOnUiThread(() =>
                        {
                            var command = System.Text.Encoding.UTF8.GetString(e.Message);
                            switch (e.Topic)
                            {
                                case "nautilus/room3_cabin/tablet/set":
                                    {
                                        switch (command)
                                        {
                                            case "reset":
                                                {
                                                    state = undone;
                                                    mqttClient.Publish("nautilus/room3_cabin/tablet", Encoding.UTF8.GetBytes("reset"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                                    StartActivityForResult(typeof(Combat), 0);
                                                    break;
                                                }
                                            case "activate":
                                                {
                                                    state = undone;
                                                    mqttClient.Publish("nautilus/room3_cabin/tablet", Encoding.UTF8.GetBytes("activate"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                                    StartActivityForResult(typeof(Combat), 0);
                                                    break;
                                                }
                                            case "play":
                                                {
                                                    state = undone;
                                                    mqttClient.Publish("nautilus/room3_cabin/tablet", Encoding.UTF8.GetBytes("play"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                                    StartActivityForResult(typeof(Combat), 0);
                                                    break;
                                                }
                                            case "pause":
                                                {
                                                    mqttClient.Publish("nautilus/room3_cabin/periscope", Encoding.UTF8.GetBytes("pause"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                                    break;
                                                }
                                            case "finish":
                                                {
                                                    mqttClient.Publish("nautilus/room3_cabin/tablet", Encoding.UTF8.GetBytes("finish"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                                    break;
                                                }
                                            case "deactivate":
                                                {
                                                    mqttClient.Publish("nautilus/room3_cabin/tablet", Encoding.UTF8.GetBytes("deactivate"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                                    break;
                                                }
                                            case "clear":
                                                {
                                                    mqttClient.Publish("nautilus/room3_cabin/tablet", Encoding.UTF8.GetBytes("clear"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                                    break;
                                                }
                                            default:
                                                Console.Error.WriteLine("There might be an error, because there is no such command \"" + command + "\" in topic \"" + e.Topic + "\"");
                                                break;
                                        }
                                        break;
                                    }
                                case "nautilus/room3_cabin/tablet/get":
                                    {
                                        if (command == "status")
                                        {
                                            mqttClient.Publish("nautilus/room3_cabin/tablet", Encoding.UTF8.GetBytes(state), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                        }
                                        break;
                                    }
                                case "nautilus/room3_cabin/tablet/puzzles/set":
                                    {
                                        switch (command)
                                        {
                                            case "getall":
                                                {
                                                    state = undone;
                                                    mqttClient.Publish("nautilus/room3_cabin/tablet/puzzles", Encoding.UTF8.GetBytes(state), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                case "nautilus/room3_cabin/tablet/state/set":
                                    {
                                        if( command == complete)
                                        {
                                                    state = complete;
                                                    mqttClient.Publish("nautilus/room3_cabin/tablet/state", Encoding.UTF8.GetBytes(state), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                                    break;
                                        }
                                        break;
                                    }
                                case "nautilus/room3_cabin/tablet/state/get":
                                    {
                                    if (command == complete)
                                        {
                                            mqttClient.Publish("nautilus/room3_cabin/tablet/state", Encoding.UTF8.GetBytes(state), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                        }
                                        break;
                                    }
                                case "nautilus/room3_cabin/tablet/mode/set":
                                    {
                                        if (command == combat)
                                        {
                                            mode = combat;
                                            mqttClient.Publish("nautilus/room3_cabin/tablet/mode", Encoding.UTF8.GetBytes(mode), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                            var intent = new Intent(this, typeof(Combat));
                                            StartActivity(intent);
                                        }
                                        if (command == dialog)
                                        {
                                            //mode = dialog;
                                            mqttClient.Publish("nautilus/room3_cabin/tablet/mode", Encoding.UTF8.GetBytes(mode), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                            Intent intent = new Intent(this, typeof(MainActivity));
                                            intent.AddFlags(ActivityFlags.SingleTop);
                                            StartActivity(intent);
                                        }
                                        if (command == damage)
                                        {
                                            mode = damage;
                                            mqttClient.Publish("nautilus/room3_cabin/tablet/mode", Encoding.UTF8.GetBytes(command), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                            StartActivityForResult(typeof(Damage), 0);
                                        }
                                        
                                        if (command == recovery)
                                        {
                                            mode = recovery;
                                            mqttClient.Publish("nautilus/room3_cabin/tablet/mode", Encoding.UTF8.GetBytes(mode), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                            StartActivityForResult(typeof(Recovery), 0);
                                        }
                                        if (command == timeout)
                                        {
                                            mode = combat;
                                            mqttClient.Publish("nautilus/room3_cabin/tablet/mode", Encoding.UTF8.GetBytes(mode), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                            var intent = new Intent(this, typeof(Recovery));
                                            intent.PutExtra("time", 0);
                                            StartActivity(intent);
                                        }
                                        break;
                                    }
                                case "nautilus/room3_cabin/tablet/mode/get":
                                    {
                                        mqttClient.Publish("nautilus/room3_cabin/tablet/mode", Encoding.UTF8.GetBytes(mode), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                        break;
                                    }
                                case "nautilus/room3_cabin/tablet/shipstatus/set":
                                    {
                                        if ((command == "on") & (MainActivity.btn_stop_reactor!=null))
                                        {
                                            try
                                            {
                                                RunOnUiThread(() => MainActivity.btn_stop_reactor.Visibility = ViewStates.Visible);
                                                mqttClient.Publish("nautilus/room3_cabin/tablet/shipstatus", Encoding.UTF8.GetBytes(command), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                            }
                                            catch(Exception ex)
                                            { }
                                            //mqttClient.Publish("nautilus/room3_cabin/tablet/mode", Encoding.UTF8.GetBytes(mode), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                        }
                                        if ((command == "off") & (MainActivity.btn_stop_reactor != null))
                                        {
                                            try
                                            {
                                                RunOnUiThread(() => MainActivity.btn_stop_reactor.Visibility = ViewStates.Gone);
                                                mqttClient.Publish("nautilus/room3_cabin/tablet/shipstatus", Encoding.UTF8.GetBytes(command), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                            }
                                            catch
                                            { }
                                            //mqttClient.Publish("nautilus/room3_cabin/tablet/mode", Encoding.UTF8.GetBytes(mode), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                                        }
                                        break;
                                    }
                            }

                        });
                    };
                    mqttClient.ConnectionClosed += (object sender, EventArgs e) =>
                    {
                        Toast.MakeText(ApplicationContext, "lost connection", ToastLength.Long);
                    };
                    
                    mqttClient.Connect("NautiliusWindow");
                    //mqttClient.Connect (customerDB.customerId+"", Mqtt_Username, Mqtt_Password, false, KeepAlives);
                    if (mqttClient.IsConnected)
                    {
                        Toast.MakeText(this, "Connect OK -- let's sub topic", ToastLength.Long);
                        mqttClient.Subscribe(new string[] { "nautilus/room3_cabin/tablet/set" }, new byte[] { (byte)1 });
                        mqttClient.Subscribe(new string[] { "nautilus/room3_cabin/tablet/state/set" }, new byte[] { (byte)1 });
                        mqttClient.Subscribe(new string[] { "nautilus/room3_cabin/tablet/state/get" }, new byte[] { (byte)1 });
                        mqttClient.Subscribe(new string[] { "nautilus/room3_cabin/tablet/mode/set" }, new byte[] { (byte)1 });
                        mqttClient.Subscribe(new string[] { "nautilus/room3_cabin/tablet/mode/get" }, new byte[] { (byte)1 });
                        mqttClient.Subscribe(new string[] { "nautilus/room3_cabin/tablet/get" }, new byte[] { (byte)1 });
                        mqttClient.Subscribe(new string[] { "nautilus/room3_cabin/tablet/puzzles/set" }, new byte[] { (byte)1 });
                        mqttClient.Subscribe(new string[] { "nautilus/room3_cabin/tablet/shipstatus/set" }, new byte[] { (byte)1 });
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Connection error " + ex.Message, ToastLength.Long);
            }

            
            StartActivityForResult(typeof(Combat), 0);
        }
        
    }
}