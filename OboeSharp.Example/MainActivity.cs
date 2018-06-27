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
			int sample_rate = 48000;
			double seconds_per_frame = 1.0 / (double) sample_rate;
			double pitch = 440.0;
			double radians_per_second = pitch * 2.0 * Math.PI;
			double volume_rate = 4.0;

			button.Click += delegate {
				using (var builder = new AudioStreamBuilder ()) {
					builder.SharingMode = Natives.OboeSharingMode.Exclusive;
					builder.Usage = Natives.OboeUsage.Media;
					builder.PerformanceMode = Natives.OboePerformanceMode.LowLatency;
					builder.AudioApi = Natives.OboeAudioApi.AAudio;
					foreach (var pi in builder.GetType ().GetProperties ())
						if (pi.GetMethod != null)
							Console.Error.WriteLine ($"Builder: {pi}: {pi.GetValue (builder)}");
					var callbacks = new AudioStreamCallback ();
					AudioStream stream = null;
					callbacks.AudioReady += (obj, audioData, numFrames) => {
						Console.Error.WriteLine ($"AUDIO READY: {obj} {numFrames}");
						int channels = stream.ChannelCount;

						// generate sine wave.
						for (int frame = 0; frame < numFrames; frame++) {
							double sample = Math.Sin ((seconds_offset + frame * seconds_per_frame) * radians_per_second) * volume_rate;
							for (int ch = 0; ch < channels; ch++)
								unsafe { *((float*) audioData + frame * channels + ch) = (float) sample; }
						}

						return Natives.OboeDataCallbackResult.Continue;
					};
					builder.Callbacks = callbacks;
					stream = builder.OpenStream ();
					foreach (var pi in stream.GetType ().GetProperties ())
						if (pi.GetMethod != null)
							Console.Error.WriteLine ($"Stream: {pi}: {pi.GetValue (stream)}");

					stream.Start ();

					Task.Delay (3000).Wait ();

					stream.Stop ();
				}
			};
		}
	}
}

