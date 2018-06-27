using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Threading.Tasks;

namespace OboeSharp.Example
{
	[Activity (Label = "OboeSharp.Example", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);

			double seconds_offset = 0.0;
			button.Click += delegate {
				using (var builder = new AudioStreamBuilder ()) {
					foreach (var pi in builder.GetType ().GetProperties ())
						if (pi.GetMethod != null)
							Console.Error.WriteLine ($"{pi}: {pi.GetValue (builder)}");
					var callbacks = new AudioStreamCallback ();
					callbacks.AudioReady += (obj, audioData, numFrames) => {

						Console.Error.WriteLine ($"AUDIO READY: {(int) audioData} {numFrames}");
						// generate sine wave.
						int sample_rate = 48000;
						double seconds_per_frame = 1.0 / (double) sample_rate;
						double pitch = 440.0;
						double radians_per_second = pitch * 2.0 * Math.PI;
						for (int frame = 0; frame < numFrames; frame += 1) {
							double sample = Math.Sin ((seconds_offset + frame * seconds_per_frame) * radians_per_second);
							unsafe { *((float*) audioData + frame) = (float) sample; }
						}

						return Natives.OboeDataCallbackResult.Continue;
					};
					builder.Callbacks = callbacks;
					var stream = builder.OpenStream ();
					foreach (var pi in stream.GetType ().GetProperties ())
						if (pi.GetMethod != null)
							Console.Error.WriteLine ($"{pi}: {pi.GetValue (stream)}");

					stream.Start ();

					Task.Delay (3000);

					stream.Stop ();
				}
			};
		}
	}
}

