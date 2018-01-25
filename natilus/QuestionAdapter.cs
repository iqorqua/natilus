using System;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.App;
using Android.Provider;
using System.Collections.Generic;
using Android;
using static natilus.DataStructures;
using static Android.Views.View;

namespace natilus
{
    public class ViewHolder : Java.Lang.Object
    {
        public Button questionBtn { get; set; }
    }
    public class QuestionAdapter : BaseAdapter
    {
        private static List<Question> _questionList = new List<Question>();
        Activity _activity;

        public QuestionAdapter(Activity activity, List<Question> q)
        {
            _activity = activity;
            _questionList = q;
        }

        public override int Count
        {
            get
            {
                if (_questionList != null)
                    return _questionList.Count;
                else return 0;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null; // could wrap a Contact in a Java.Lang.Object to return it here if needed
        }

        public override long GetItemId(int position)
        {
            return _questionList[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.question_item, parent, false);
            var question_element = view.FindViewById<TextView>(Resource.Id.question_textView);
            var container = view.FindViewById<LinearLayout>(Resource.Id.questionContainerDialog);

            /*question_element.Click += (sender, args) =>
            {
                Intent intent = new Intent(_activity, typeof(Dialog));
                intent.PutExtra("question", _questionList[position].Question_txt);
                intent.PutExtra("answer", _questionList[position].Answer_text);
                _activity.StartActivity(intent);
            };*/
            question_element.Text = _questionList[position].Question_txt;
            container.BringToFront();
            return view;
        }
    }
}