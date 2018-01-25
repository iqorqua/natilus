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

namespace natilus
{
    public class DataStructures
    {
        public struct Question
        {
            public int Id { get; set; }
            public string Question_txt { get; set; }
            public string Answer_text { get; set; }
        }
        public struct Answer
        {
            public int Id { get; set; }
            public string Answer_txt { get; set; }
        }

        public struct SetQA
        {
            public Question Q { get; set; }
            public List<Answer> A { get; set; }
        }
    }
}