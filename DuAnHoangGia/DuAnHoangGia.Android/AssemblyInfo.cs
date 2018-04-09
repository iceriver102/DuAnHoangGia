#if DEBUG
using Android.App;
[assembly: Application(Debuggable=true)]
#else
using Android.App;
[assembly: Application(Debuggable = false)]
#endif