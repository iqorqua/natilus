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
using Android.Graphics.Drawables;

namespace natilus
{
    [Activity(Label = "natilus", Icon = "@drawable/icon", Theme = "@style/NoActionBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class Game : Activity//, View.IOnTouchListener
    {
        Bitmap b;
        GridLayout rootLO;

        ImageView t1;
        ImageView t2;
        ImageView t3;
        ImageView t4;
        ImageView t5;
        ImageView t6;

        ImageView tube1;
        ImageView tube2;
        ImageView tube3;

        ImageView lock1;
        ImageView lock2;
        ImageView lock3;

        Paint orange;
        Paint orange_bold;
        Paint green;
        Paint green_bold;
        Paint red;
        Paint red_bold;
        Paint gray;
        Paint gray_bold;
        Paint violet;
        Paint violet_bold;
        Paint blue;
        Paint blue_bold;
        Paint cleaner;

        Path p1_1, p1_2, p2_1, p2_2, p3_1, p3_2, p4_1, p4_2, p5_1, p5_2, p6_1, p6_2;

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
            SetContentView(Resource.Layout.reactor);
            t1 = FindViewById<ImageView>(Resource.Id.tumbler1);
            t2 = FindViewById<ImageView>(Resource.Id.tumbler2);
            t3 = FindViewById<ImageView>(Resource.Id.tumbler3);
            t4 = FindViewById<ImageView>(Resource.Id.tumbler4);
            t5 = FindViewById<ImageView>(Resource.Id.tumbler5);
            t6 = FindViewById<ImageView>(Resource.Id.tumbler6);

            tube1 = FindViewById<ImageView>(Resource.Id.tube1);
            tube2 = FindViewById<ImageView>(Resource.Id.tube2);
            tube3 = FindViewById<ImageView>(Resource.Id.tube3);

            lock1 = FindViewById<ImageView>(Resource.Id.lock1);
            lock2 = FindViewById<ImageView>(Resource.Id.lock2);
            lock3 = FindViewById<ImageView>(Resource.Id.lock3);

            t1.Click += OnTumblerClick;
            t2.Click += OnTumblerClick;
            t3.Click += OnTumblerClick;
            t4.Click += OnTumblerClick;
            t5.Click += OnTumblerClick;
            t6.Click += OnTumblerClick;

            InitBrushes();
        }

        private void InitPathLines()
        {
            rootLO = FindViewById<GridLayout>(Resource.Id.grid);
            var w = FindViewById<GridLayout>(Resource.Id.grid).Width;
            var h = FindViewById<GridLayout>(Resource.Id.grid).Height;
            var s1 = new Point((int)this.t1.GetX() + this.t1.Width / 2, (int)this.t1.GetY() + this.t1.Height / 2);
            var s2 = new Point((int)this.t2.GetX() + this.t2.Width / 2, (int)this.t2.GetY() + this.t2.Height / 2);
            var s3 = new Point((int)this.t3.GetX() + this.t3.Width / 2, (int)this.t3.GetY() + this.t3.Height / 2);
            var s4 = new Point((int)this.t4.GetX() + this.t4.Width / 2, (int)this.t4.GetY() + this.t4.Height / 2);
            var s5 = new Point((int)this.t5.GetX() + this.t5.Width / 2, (int)this.t5.GetY() + this.t5.Height / 2);
            var s6 = new Point((int)this.t6.GetX() + this.t6.Width / 2, (int)this.t6.GetY() + this.t6.Height / 2);
            var t1 = new Point((int)tube1.GetX() + tube1.Width / 2, (int)tube1.GetY() + tube1.Height / 2);
            var t2 = new Point((int)tube2.GetX() + tube2.Width / 2, (int)tube2.GetY() + tube2.Height / 2);
            var t3 = new Point((int)tube3.GetX() + tube3.Width / 2, (int)tube3.GetY() + tube3.Height / 2);
            var l1 = new Point((int)lock1.GetX() + lock1.Width / 2, (int)lock1.GetY() + lock1.Height / 2);
            var l2 = new Point((int)lock2.GetX() + lock2.Width / 2, (int)lock2.GetY() + lock2.Height / 2);
            var l3 = new Point((int)lock3.GetX() + lock3.Width / 2, (int)lock3.GetY() + lock3.Height / 2);

            lock1.SetX(t1.X - 150); lock1.SetY(t1.Y - 25);
            lock2.SetX(t3.X - 150); lock2.SetY(t3.Y - 25);
            lock3.SetX(t3.X + 150); lock3.SetY(t3.Y - 25);

            b = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);

            p1_1 = new Path();
            p1_1.MoveTo(s1.X, s1.Y);
            p1_1.LineTo(s1.X + 100, s1.Y);
            p1_1.LineTo(s1.X + 100, t1.Y);
            p1_1.LineTo(t1.X, t1.Y);
            p1_2 = new Path();
            p1_2.MoveTo(s1.X, s1.Y);
            p1_2.LineTo(s1.X, t2.Y);
            p1_2.LineTo(t1.X, t2.Y);

            p2_1 = new Path();
            p2_1.MoveTo(s2.X, s2.Y);
            p2_1.LineTo(t2.X, s2.Y);
            p2_1.LineTo(t2.X, t2.Y);
            p2_2 = new Path();
            p2_2.MoveTo(s2.X, s2.Y);
            p2_2.LineTo(s2.X, t3.Y);
            p2_2.LineTo(t3.X, t3.Y);

            p3_1 = new Path();
            p3_1.MoveTo(s3.X, s3.Y);
            p3_1.LineTo(s3.X, s3.Y + 200);
            p3_1.LineTo(l2.X, s3.Y + 200);
            p3_1.LineTo(l2.X, l2.Y);
            p3_2 = new Path();
            p3_2.MoveTo(s3.X, s3.Y);
            p3_2.LineTo(s3.X, t2.Y);
            p3_2.LineTo(t2.X, t2.Y);

            p4_1 = new Path();
            p4_1.MoveTo(s4.X, s4.Y);
            p4_1.LineTo(t1.X, s4.Y);
            p4_1.LineTo(t1.X, t1.Y);
            p4_2 = new Path();
            p4_2.MoveTo(s4.X, s4.Y);
            p4_2.LineTo(s4.X, t3.Y);
            p4_2.LineTo(t3.X, t3.Y);

            p5_1 = new Path();
            p5_1.MoveTo(s5.X, s5.Y);
            p5_1.LineTo(l3.X, s5.Y);
            p5_1.LineTo(l3.X, l3.Y);
            p5_2 = new Path();
            p5_2.MoveTo(s5.X, s5.Y);
            p5_2.LineTo(s5.X, s5.Y + 70);
            p5_2.LineTo(l1.X, s5.Y + 70);
            p5_2.LineTo(l1.X, l1.Y);

            p6_1 = new Path();
            p6_1.MoveTo(s6.X, s6.Y);
            p6_1.LineTo(t1.X, s6.Y);
            p6_1.LineTo(t1.X, t1.Y);
            p6_2 = new Path();
            p6_2.MoveTo(s6.X, s6.Y);
            p6_2.LineTo(s6.X, l3.Y - 70);
            p6_2.LineTo(l3.X, l3.Y - 70);
            p6_2.LineTo(l3.X, l3.Y);
        }

        private void InitBrushes()
        {
            orange = new Paint();
            orange.AntiAlias = true;
            orange.Dither = true;
            orange.Color = Color.Orange;
            orange.SetStyle(Paint.Style.Stroke);
            orange.StrokeJoin = Paint.Join.Round;
            orange.StrokeWidth = 5;
            orange.StrokeCap = Paint.Cap.Round;
            orange_bold = new Paint();
            orange_bold.AntiAlias = true;
            orange_bold.Dither = true;
            orange_bold.Color = Color.Orange;
            orange_bold.SetStyle(Paint.Style.Stroke);
            orange_bold.StrokeJoin = Paint.Join.Round;
            orange_bold.StrokeWidth = 15;
            orange_bold.StrokeCap = Paint.Cap.Round;
            orange.SetPathEffect(new DashPathEffect(new float[] { 5, 10, 15, 20 }, 0));

            green = new Paint();
            green.AntiAlias = true;
            green.Dither = true;
            green.Color = Color.Green;
            green.SetStyle(Paint.Style.Stroke);
            green.StrokeJoin = Paint.Join.Round;
            green.StrokeWidth = 5;
            green.StrokeCap = Paint.Cap.Round;
            green_bold = new Paint();
            green_bold.AntiAlias = true;
            green_bold.Dither = true;
            green_bold.Color = Color.Green;
            green_bold.SetStyle(Paint.Style.Stroke);
            green_bold.StrokeJoin = Paint.Join.Round;
            green_bold.StrokeWidth = 15;
            green_bold.StrokeCap = Paint.Cap.Round;
            green.SetPathEffect(new DashPathEffect(new float[] { 5, 10, 15, 20 }, 0));

            red = new Paint();
            red.AntiAlias = true;
            red.Dither = true;
            red.Color = Color.Red;
            red.SetStyle(Paint.Style.Stroke);
            red.StrokeJoin = Paint.Join.Round;
            red.StrokeWidth = 5;
            red.StrokeCap = Paint.Cap.Round;
            red_bold = new Paint();
            red_bold.AntiAlias = true;
            red_bold.Dither = true;
            red_bold.Color = Color.Red;
            red_bold.SetStyle(Paint.Style.Stroke);
            red_bold.StrokeJoin = Paint.Join.Round;
            red_bold.StrokeWidth = 15;
            red_bold.StrokeCap = Paint.Cap.Round;
            red.SetPathEffect(new DashPathEffect(new float[] { 5, 10, 15, 20 }, 0));

            gray = new Paint();
            gray.AntiAlias = true;
            gray.Dither = true;
            gray.Color = Color.DarkGray;
            gray.SetStyle(Paint.Style.Stroke);
            gray.StrokeJoin = Paint.Join.Round;
            gray.StrokeWidth = 5;
            gray.StrokeCap = Paint.Cap.Round;
            gray_bold = new Paint();
            gray_bold.AntiAlias = true;
            gray_bold.Dither = true;
            gray_bold.Color = Color.DarkGray;
            gray_bold.SetStyle(Paint.Style.Stroke);
            gray_bold.StrokeJoin = Paint.Join.Round;
            gray_bold.StrokeWidth = 15;
            gray_bold.StrokeCap = Paint.Cap.Round;
            gray.SetPathEffect(new DashPathEffect(new float[] { 5, 10, 15, 20 }, 0));

            blue = new Paint();
            blue.AntiAlias = true;
            blue.Dither = true;
            blue.Color = Color.Blue;
            blue.SetStyle(Paint.Style.Stroke);
            blue.StrokeJoin = Paint.Join.Round;
            blue.StrokeWidth = 5;
            blue.StrokeCap = Paint.Cap.Round;
            blue_bold = new Paint();
            blue_bold.AntiAlias = true;
            blue_bold.Dither = true;
            blue_bold.Color = Color.Blue;
            blue_bold.SetStyle(Paint.Style.Stroke);
            blue_bold.StrokeJoin = Paint.Join.Round;
            blue_bold.StrokeWidth = 15;
            blue_bold.StrokeCap = Paint.Cap.Round;
            blue.SetPathEffect(new DashPathEffect(new float[] { 5, 10, 15, 20 }, 0));

            violet = new Paint();
            violet.AntiAlias = true;
            violet.Dither = true;
            violet.Color = Color.Violet;
            violet.SetStyle(Paint.Style.Stroke);
            violet.StrokeJoin = Paint.Join.Round;
            violet.StrokeWidth = 5;
            violet.StrokeCap = Paint.Cap.Round;
            violet_bold = new Paint();
            violet_bold.AntiAlias = true;
            violet_bold.Dither = true;
            violet_bold.Color = Color.Violet;
            violet_bold.SetStyle(Paint.Style.Stroke);
            violet_bold.StrokeJoin = Paint.Join.Round;
            violet_bold.StrokeWidth = 15;
            violet_bold.StrokeCap = Paint.Cap.Round;
            violet.SetPathEffect(new DashPathEffect(new float[] { 5, 10, 15, 20 }, 0));

            cleaner = new Paint();
            cleaner.AntiAlias = true;
            cleaner.Dither = true;
            cleaner.Color = Color.DimGray;
            cleaner.SetStyle(Paint.Style.Stroke);
            cleaner.StrokeJoin = Paint.Join.Round;
            cleaner.StrokeWidth = 17;
            cleaner.StrokeCap = Paint.Cap.Round;



        }

        protected override void OnStart()
        {
            base.OnResume();
            ViewTreeObserver observer = FindViewById<GridLayout>(Resource.Id.grid).ViewTreeObserver;
            observer.GlobalLayout += (object sendr, EventArgs ev) =>
           {
               InitPathLines();

               InvalidateGraph();
           };
        }

        private void InvalidateGraph()
        {
            Canvas c = new Canvas(b);
            c.DrawColor(Color.DimGray);
            if (t1.Rotation == 90)
            {
                c.DrawPath(p1_1, orange_bold);
                c.DrawPath(p1_2, green);
            }
            if (t1.Rotation == 180)
            {
                c.DrawPath(p1_1, orange);
                c.DrawPath(p1_2, green_bold);
            }

            if (t2.Rotation == 0)
            {
                c.DrawPath(p2_2, blue_bold);
                c.DrawPath(p2_1, green);
            }
            if (t2.Rotation == 90)
            {
                c.DrawPath(p2_2, blue);
                c.DrawPath(p2_1, green_bold);
            }

            if (t3.Rotation == 0)
            {
                c.DrawPath(p3_1, gray);
                c.DrawPath(p3_2, green_bold);
            }
            if (t3.Rotation == 180)
            {
                c.DrawPath(p3_1, gray_bold);
                c.DrawPath(p3_2, green);
            }

            if (t4.Rotation == 270)
            {
                c.DrawPath(p4_1, orange_bold);
                c.DrawPath(p4_2, blue);
            }
            if (t4.Rotation == 180)
            {
                c.DrawPath(p4_1, orange);
                c.DrawPath(p4_2, blue_bold);
            }

            if (t5.Rotation == 0)
            {
                c.DrawPath(p5_1, red_bold);
                c.DrawPath(p5_2, violet);
            }
            if (t5.Rotation == 270)
            {
                c.DrawPath(p5_1, red);
                c.DrawPath(p5_2, violet_bold);
            }

            if (t6.Rotation == 90)
            {
                c.DrawPath(p6_1, orange_bold);
                c.DrawPath(p6_2, red);
            }
            if (t6.Rotation == 180)
            {
                c.DrawPath(p6_1, orange);
                c.DrawPath(p6_2, red_bold);
            }
            rootLO.SetBackgroundDrawable(new BitmapDrawable(b));
        }

        private void OnTumblerClick(object sender, EventArgs e)
        {
            var current_tubbler = (ImageView)sender;
            switch(current_tubbler.Id)
            {
                case Resource.Id.tumbler1:
                    {
                        if (current_tubbler.Rotation == 90)
                        {
                            current_tubbler.Rotation = 180;
                        }
                        else
                        {
                            current_tubbler.Rotation = 90;
                        }
                        break;
                    }
                case Resource.Id.tumbler2:
                    {
                        if (current_tubbler.Rotation == 0)
                        {
                            current_tubbler.Rotation = 90;
                        }
                        else
                        {
                            current_tubbler.Rotation = 0;
                        }
                        break;
                    }
                case Resource.Id.tumbler3:
                    {
                        if (current_tubbler.Rotation == 0)
                        {
                            current_tubbler.Rotation = 180;
                        }
                        else
                        {
                            current_tubbler.Rotation = 0;
                        }
                        break;
                    }
                case Resource.Id.tumbler4:
                    {
                        if (current_tubbler.Rotation == 270)
                        {
                            current_tubbler.Rotation = 180;
                        }
                        else
                        {
                            current_tubbler.Rotation = 270;
                        }
                        break;
                    }
                case Resource.Id.tumbler5:
                    {
                        if (current_tubbler.Rotation == 0)
                        {
                            current_tubbler.Rotation = 270;
                        }
                        else
                        {
                            current_tubbler.Rotation = 0;
                        }
                        break;
                    }
                case Resource.Id.tumbler6:
                    {

                        if (current_tubbler.Rotation == 90)
                        {
                            current_tubbler.Rotation = 180;
                        }
                        else
                        {
                            current_tubbler.Rotation = 90;
                        }
                        break;
                    }
            }
            InvalidateGraph();
        }
    }
}

