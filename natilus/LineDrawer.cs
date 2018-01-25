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
using Android.Graphics;

namespace natilus
{
    class LineDrawer : View
    {
        public LineDrawer(Context context) : base(context)
        {

            mPath = new Path();
            mBitmapPaint = new Paint(PaintFlags.Dither);
        }

        private static float MINP = 0.25f;
        private static float MAXP = 0.75f;

        private Bitmap mBitmap;
        private Canvas mCanvas;
        private Path mPath;
        private Paint mBitmapPaint;
        private Paint mPaint;

        protected void onSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            mBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
            mCanvas = new Canvas(mBitmap);
        }

        protected override void OnDraw(Canvas canvas)
        {
            mPaint = new Paint();
            mPaint.AntiAlias = true;
            mPaint.Dither = true;
            mPaint.Color = Color.Red;
            mPaint.SetStyle(Paint.Style.Stroke);
            mPaint.StrokeJoin = Paint.Join.Round;
            // mPaint.setStrokeCap(Paint.Cap.ROUND);
            mPaint.StrokeWidth = 10;
            canvas.DrawColor(Color.Black);
            // canvas.drawLine(mX, mY, Mx1, My1, mPaint);
            // canvas.drawLine(mX, mY, x, y, mPaint);
            mBitmap = Bitmap.CreateBitmap(this.RootView.Width, this.RootView.Height, Bitmap.Config.Argb8888);
            mCanvas = new Canvas(mBitmap);
            canvas.DrawBitmap(mBitmap, 0, 0, mBitmapPaint);
            canvas.DrawPath(mPath, mPaint);

        }

        private float mX, mY;
        private static float TOUCH_TOLERANCE = 4;

        private void touch_start(float x, float y)
        {
            mPath.Reset();
            mPath.MoveTo(x, y);
            mX = x;
            mY = y;
        }
        private void touch_move(float x, float y)
        {
            float dx = Math.Abs(x - mX);
            float dy = Math.Abs(y - mY);
            if (dx >= TOUCH_TOLERANCE || dy >= TOUCH_TOLERANCE)
            {
                // mPath.quadTo(mX, mY, (x + mX)/2, (y + mY)/2);
                mX = x;
                mY = y;
            }
        }
        private void touch_up()
        {
            mPath.LineTo(mX, mY);
            // commit the path to our offscreen
            mCanvas.DrawPath(mPath, mPaint);
            // kill this so we don't double draw
            mPath.Reset();
        }
        
        public override  bool OnTouchEvent(MotionEvent e) {
            float x = e.GetX();
            float y = e.GetY();

            switch (e.Action) {
                case MotionEventActions.Down:
                    touch_start(x, y);
                    Invalidate();
                    break;
                case MotionEventActions.Move:
                    touch_move(x, y);
                    Invalidate();
                    break;
                case MotionEventActions.Up:
                    touch_up();
               //   Mx1=(int) event.getX();
               //  My1= (int) event.getY();
                   Invalidate();
                    break;
            }
            return true;
        }
    }
}