using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech.Tts;
using Android.Views;
using Android.Widget;
using DuAnHoangGia.Droid.Dependencys;
using DuAnHoangGia.Sevices;
using Xamarin.Forms;

[assembly: Dependency(typeof(TextToSpeechImplementation))]
namespace DuAnHoangGia.Droid.Dependencys
{
    public class TextToSpeechImplementation : Java.Lang.Object, ITextToSpeech, TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        string toSpeak;

        public void Speak(string text)
        {
            toSpeak = text;
            if (speaker == null)
            {
                speaker = new TextToSpeech(MainActivity.Instance, this);
                speaker.SetLanguage(new Java.Util.Locale("vi-VN"));
                speaker.SetPitch(0.5f);
                speaker.SetSpeechRate(1);
            }
            else
            {
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }

        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }
    }
}