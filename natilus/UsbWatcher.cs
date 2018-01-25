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

namespace natilus
{
    public class UsbWatcher : BroadcastReceiver
    {
        public event EventHandler Detached;
        public override void OnReceive(Context context, Intent intent)
        {
            String action = intent.Action;

            if (UsbManager.ActionUsbDeviceDetached.Equals(action))
            {
                if (Detached != null)
                {
                    Detached(this, new EventArgs());
                }
            }
        }
    }
}