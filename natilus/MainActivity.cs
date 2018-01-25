using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Views;
using MySql.Data.MySqlClient;
using System;
using static natilus.DataStructures;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using Com.Felhr.Usbserial;
using Android.Hardware.Usb;
using Android.Graphics;

namespace natilus
{
    [Activity(Label = "natilus", Icon = "@drawable/icon", Theme = "@style/NoActionBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : Activity, View.IOnTouchListener
    {
        public static UsbSerialDevice _serialPort;
        public static Button btn_stop_reactor;
        int questions_count = 7;
        Button updateQuestions;
        List<SetQA> _set = new List<SetQA>();
        LinearLayout mainLO;
        ProgressDialog prog;
        ListView questionList;
        Button q1;
        Button q2;
        Button q3;
        Button q4;
        Button q5;
        Button q6;
        Button q7;
        private List<Question> questionQuery = new List<Question>();
        private bool isPaused = false;
        public int Inactivity_time { get; set; } = 15;

        [Android.Runtime.Register("onUserInteraction", "()V", "GetOnUserInteractionHandler")]
        public override void OnUserInteraction()
        {
            Inactivity_time = 5;
        }
        protected override void OnPause()
        {
            base.OnPause();
            isPaused = true;
        }
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
            

            StartActivityCheck();

            Window.DecorView.Touch += delegate
              {
                  Inactivity_time = 10;
              };

            try
            {
                this.Window.DecorView.SystemUiVisibilityChange += DecorView_SystemUiVisibilityChange;

                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.questions);
                FindViewById<TextView>(Resource.Id.text_communication_interface).SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
                q1 = FindViewById<Button>(Resource.Id.btn_q1);
                q2 = FindViewById<Button>(Resource.Id.btn_q2);
                q3 = FindViewById<Button>(Resource.Id.btn_q3);
                q4 = FindViewById<Button>(Resource.Id.btn_q4);
                q5 = FindViewById<Button>(Resource.Id.btn_q5);
                q6 = FindViewById<Button>(Resource.Id.btn_q6);
                q7 = FindViewById<Button>(Resource.Id.btn_q7);

                q1.SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
                q2.SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
                q3.SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
                q4.SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
                q5.SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
                q6.SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
                q7.SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);

                questionList = FindViewById<ListView>(Resource.Id.questions);
                updateQuestions = FindViewById<Button>(Resource.Id.UpdateQuestions);
                mainLO = FindViewById<LinearLayout>(Resource.Id.linearLayout1);

                btn_stop_reactor = FindViewById<Button>(Resource.Id.btn_stop_reactor);
                btn_stop_reactor.SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
                btn_stop_reactor.Click += (object o, EventArgs ev) =>
                {
                    Intent intent = new Intent(this, typeof(DesignedGame));
                    StartActivity(intent);
                };
                btn_stop_reactor.Visibility = ViewStates.Gone;


                /*StartActivityForResult(typeof(Dialog), 0);
                StartActivityForResult(typeof(ConnectToUsb), 0);*/
                mainLO.SetOnTouchListener(this);
                updateQuestions.Click += delegate
                {
                    CreateSet();
                };
                updateQuestions.SetTypeface(Typeface.CreateFromAsset(Assets, "h.otf"), TypefaceStyle.Normal);
                CreateSet();

                questionList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e)
                {
                    Intent intent = new Intent(this, typeof(Dialog));
                    intent.PutExtra("question", questionQuery[e.Position].Question_txt);
                    intent.PutExtra("answer", questionQuery[e.Position].Answer_text);
                    StartActivity(intent);
                };
                q1.Click += Q_Click;
                q2.Click += Q_Click;
                q3.Click += Q_Click;
                q4.Click += Q_Click;
                q5.Click += Q_Click;
                q6.Click += Q_Click;
                q7.Click += Q_Click;
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, "Oncreate event: " + ex.Message, ToastLength.Long);
            }
        }

        private void StartActivityCheck()
        {
            new Task(() =>
            {
                while (true)
                {
                    if(isPaused)
                    {
                        Thread.Sleep(1000);
                        Inactivity_time = 10;
                        continue;
                    }
                    if (Inactivity_time == 0)
                    {
                        Intent intent;
                        if (DialogQuestions.mode == DialogQuestions.damage)
                        {
                            intent = new Intent(this, typeof(Damage));
                            intent.AddFlags(ActivityFlags.SingleTop);
                            StartActivity(intent);
                        }
                        if (DialogQuestions.mode == DialogQuestions.combat)
                        {
                            intent = new Intent(this, typeof(Combat));
                            intent.AddFlags(ActivityFlags.SingleTop);
                            StartActivity(intent);
                        }
                        Inactivity_time = -1;
                    }
                    if (Inactivity_time < 0)
                    {
                        continue;
                    }
                    Thread.Sleep(1000);
                    Inactivity_time--;
                }

            }).Start();
        }

        private void Q_Click(object sender, EventArgs e)
        {
            Button b = (Button) sender;
            var name = Resources.GetResourceName(b.Id);
            var num = Convert.ToInt16(name[name.Length - 1].ToString()) - 1;
            Intent intent = new Intent(this, typeof(Dialog));
            intent.PutExtra("question", questionQuery[num].Question_txt);
            intent.PutExtra("answer", questionQuery[num].Answer_text);
            StartActivity(intent);
        }

        private void DecorView_SystemUiVisibilityChange(object sender, View.SystemUiVisibilityChangeEventArgs e)
        {
            try
            {
                ((FrameLayout)sender).SystemUiVisibility = (StatusBarVisibility)
                              (SystemUiFlags.LayoutStable
                              | SystemUiFlags.LayoutHideNavigation
                              | SystemUiFlags.LayoutFullscreen
                              | SystemUiFlags.HideNavigation
                              | SystemUiFlags.Fullscreen
                              | SystemUiFlags.Immersive);
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, "Decor event: " + ex.Message, ToastLength.Long);
            }
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
               CreateSet();
            }
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            //Toast.MakeText(this, "Updated!", ToastLength.Long).Show();
            //CreateSet();
        }
        protected override void OnResume()
        {
            base.OnResume();
            isPaused = false;
            try
            {
                this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
                this.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)
                              (SystemUiFlags.LayoutStable
                              | SystemUiFlags.LayoutHideNavigation
                              | SystemUiFlags.LayoutFullscreen
                              | SystemUiFlags.HideNavigation
                              | SystemUiFlags.Fullscreen
                              | SystemUiFlags.Immersive);
                Inactivity_time = 10;
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, "On  resume : " + ex.Message, ToastLength.Long);
            }

        }
        

        private void CreateSet()
        {
            try
            {
                mainLO.Visibility = ViewStates.Gone;

                prog = new ProgressDialog(this);
                prog.Window.SetFlags(WindowManagerFlags.NotFocusable, WindowManagerFlags.NotFocusable);
                prog.Show();
                prog.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)
                              (SystemUiFlags.LayoutStable
                              | SystemUiFlags.LayoutHideNavigation
                              | SystemUiFlags.LayoutFullscreen
                              | SystemUiFlags.HideNavigation
                              | SystemUiFlags.Fullscreen
                              | SystemUiFlags.Immersive);
                prog.SetCancelable(false);
                prog.SetTitle("Wait...");
                prog.SetMessage("I`m thinking...");

                var bckg = new BackgroundWorker();
                bckg.DoWork += delegate
                {
                    try
                    {
                        MySqlConnection sqlconn;
                        string connsqlstring = "server=nautilus.60out.com;uid=admin;" +
                    "pwd=admin;database=questions;";
                        sqlconn = new MySqlConnection(connsqlstring);
                        sqlconn.Open();
                        string queryString = @"SELECT * FROM `Question`
                                     ORDER BY RAND()
                                     LIMIT " + questions_count;
                        MySqlCommand sqlcmd = new MySqlCommand(queryString, sqlconn);
                        var result = sqlcmd.ExecuteReader();
                        questionQuery = new List<Question>();
                        while (result.Read())
                        {
                            var question = new Question()
                            {
                                Id = result.GetInt32("ID"),
                                Question_txt = result.GetString("question"),
                                Answer_text = result.GetString("answer")
                            };
                            questionQuery.Add(question);
                        }
                        sqlconn.Close();
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                };
                bckg.RunWorkerCompleted += delegate
                {
                    RunOnUiThread(() => UpdateQuestions());//NextQuestion());
                    mainLO.Visibility = ViewStates.Visible;
                    prog.Hide();
                };
                bckg.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, "Create set: " + ex.Message, ToastLength.Long);
            }
            
        }

        private void UpdateQuestions()
        {
            try
            {
                /*QuestionAdapter adapter = new QuestionAdapter(this, questionQuery);
                questionList.Adapter = adapter;*/
                q1.Text = questionQuery[0].Question_txt;
                q2.Text = questionQuery[1].Question_txt;
                q3.Text = questionQuery[2].Question_txt;
                q4.Text = questionQuery[3].Question_txt;
                q5.Text = questionQuery[4].Question_txt;
                q6.Text = questionQuery[5].Question_txt;
                q7.Text = questionQuery[6].Question_txt;
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, "Update set: " + ex.Message, ToastLength.Long);
            }
        }

        public bool OnTouch(View v, MotionEvent e)
        {

            if (e.Action == MotionEventActions.ButtonPress)
                return true;
            else
            {
                try
                {
                    this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
                    this.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)
                              (SystemUiFlags.LayoutStable
                              | SystemUiFlags.LayoutHideNavigation
                              | SystemUiFlags.LayoutFullscreen
                              | SystemUiFlags.HideNavigation
                              | SystemUiFlags.Fullscreen
                              | SystemUiFlags.Immersive);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(ApplicationContext, "Touch event: " + ex.Message, ToastLength.Long);
                }
        }
            return false;
        }
    }
}

