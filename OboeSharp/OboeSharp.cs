using System;
using OboeSharp.Natives;

namespace OboeSharp
{
	static class Extensions
	{
		public static void CheckStatus (this OboeResult error)
		{
			if (error != OboeResult.OK)
				throw new OboeException (error);
		}

		public static double CheckStatus (this OboeResultWithValueDouble result)
		{
			if (result.error != OboeResult.OK)
				throw new OboeException (result.error);
			return result.value;
		}

		public static int CheckStatus (this OboeResultWithValueInt32 result)
		{
			if (result.error != OboeResult.OK)
				throw new OboeException (result.error);
			return result.value;
		}
	}

	public class AudioStreamBuilder : IDisposable
	{
		public AudioStreamBuilder ()
		{
			instance = OboeInterop.oboe_audio_stream_builder_create ();
		}

		IntPtr instance;

		public void Dispose ()
		{
			if (instance != IntPtr.Zero)
				OboeInterop.oboe_audio_stream_builder_delete (instance);
			instance = IntPtr.Zero;
		}

		public OboeAudioApi AudioApi {
			get => OboeInterop.oboe_audio_stream_builder_get_audio_api (instance);
			set => OboeInterop.oboe_audio_stream_builder_set_audio_api (instance, value);
		}
		public int DefaultFramesPerBurst {
			get => OboeInterop.oboe_audio_stream_builder_get_default_frames_per_burst (instance);
			set => OboeInterop.oboe_audio_stream_builder_set_default_frames_per_burst (instance, value);
		}
		public bool IsAAudioSupported => OboeInterop.oboe_audio_stream_builder_is_aaudio_supported (instance);
		public bool IsAAudioRecommended => OboeInterop.oboe_audio_stream_builder_is_aaudio_recommended (instance);

		public int BufferCapacityInFrames {
			set => OboeInterop.oboe_audio_stream_builder_set_buffer_capacity_in_frames (instance, value);
		}
		public int ChannelCount {
			set => OboeInterop.oboe_audio_stream_builder_set_channel_count (instance, value);
		}
		public OboeContentType ContentType {
			set => OboeInterop.oboe_audio_stream_builder_set_content_type (instance, value);
		}
		public int DeviceId {
			set => OboeInterop.oboe_audio_stream_builder_set_device_id (instance, value);
		}
		public OboeDirection Direction {
			set => OboeInterop.oboe_audio_stream_builder_set_direction (instance, value);
		}
		public OboeAudioFormat Format {
			set => OboeInterop.oboe_audio_stream_builder_set_format (instance, value);
		}
		public OboeInputPreset InputPreset {
			set => OboeInterop.oboe_audio_stream_builder_set_input_preset (instance, value);
		}
		public OboePerformanceMode PerformanceMode {
			set => OboeInterop.oboe_audio_stream_builder_set_performance_mode (instance, value);
		}
		public int SampleRate {
			set => OboeInterop.oboe_audio_stream_builder_set_sample_rate (instance, value);
		}
		public OboeSessionId SessionId {
			set => OboeInterop.oboe_audio_stream_builder_set_session_id (instance, value);
		}
		public OboeSharingMode SharingMode {
			set => OboeInterop.oboe_audio_stream_builder_set_sharing_mode (instance, value);
		}
		public OboeUsage Usage {
			set => OboeInterop.oboe_audio_stream_builder_set_usage (instance, value);
		}

		public AudioStreamCallback Callbacks {
			set => OboeInterop.oboe_audio_stream_builder_set_callback (instance, value.Handle);
		}

		public AudioStream OpenStream ()
		{
			OboeInterop.oboe_audio_stream_builder_open_stream (instance, out IntPtr stream).CheckStatus ();
			return new AudioStream (stream);
		}
	}

	// Those first arguments are kept as IntPtr. We don't want to populate or even lookup AudioStream instance many times.
	public delegate OboeDataCallbackResult OnAudioReadyDelegate (IntPtr stream, IntPtr audioData, int numFrames);
	public delegate OboeDataCallbackResult OnAudioErrorDelegate (IntPtr stream, OboeResult error);

	public class AudioStreamCallback : IDisposable
	{
		public AudioStreamCallback ()
		{
			instance = OboeInterop.oboe_audio_stream_callback_create ();
			OboeInterop.oboe_audio_stream_callback_set_on_audio_ready (instance, (oboeStream, audioData, numFrames) => AudioReady (oboeStream, audioData, numFrames));
			OboeInterop.oboe_audio_stream_callback_set_on_error_before_close (instance, (oboeStream, error) => ClosingForError (oboeStream, error));
			OboeInterop.oboe_audio_stream_callback_set_on_error_after_close (instance, (oboeStream, error) => ClosedForError (oboeStream, error));
		}

		public event OnAudioReadyDelegate AudioReady;
		public event OnAudioErrorDelegate ClosingForError;
		public event OnAudioErrorDelegate ClosedForError;

		IntPtr instance;

		internal IntPtr Handle => instance;

		public void Dispose ()
		{
			if (instance != IntPtr.Zero)
				OboeInterop.oboe_audio_stream_callback_free (instance);
			instance = IntPtr.Zero;
		}
	}

	public abstract class AudioStreamBase
	{
		protected AudioStreamBase (IntPtr instance)
		{
			this.instance = instance;
		}

		readonly IntPtr instance;

		public OboeUsage Usage => OboeInterop.oboe_audio_stream_base_get_usage (instance);
		public OboeAudioFormat Format => OboeInterop.oboe_audio_stream_base_get_format (instance);
		// public AudioStreamCallback Callback => new AudioStreamCallback (OboeInterop.oboe_audio_stream_base_get_callback (instance));
		public int DeviceId => OboeInterop.oboe_audio_stream_base_get_device_id (instance);
		public OboeDirection Direction => OboeInterop.oboe_audio_stream_base_get_direction (instance);
		public OboeSessionId SessionId => OboeInterop.oboe_audio_stream_base_get_session_id (instance);
		public int SampleRate => OboeInterop.oboe_audio_stream_base_get_sample_rate (instance);
		public OboeContentType ContentType => OboeInterop.oboe_audio_stream_base_get_content_type (instance);
		public OboeInputPreset InputPreset => OboeInterop.oboe_audio_stream_base_get_input_preset (instance);
		public OboeSharingMode SharingMode => OboeInterop.oboe_audio_stream_base_get_sharing_mode (instance);
		public int ChannelCount => OboeInterop.oboe_audio_stream_base_get_channel_count (instance);
		public OboePerformanceMode PerformanceMode => OboeInterop.oboe_audio_stream_base_get_performance_mode (instance);
		public int FramesPerCallback => OboeInterop.oboe_audio_stream_base_get_frames_per_callback (instance);
		public int BufferSizeInFrames => OboeInterop.oboe_audio_stream_base_get_buffer_size_in_frames (instance);
		public int BufferCapacityInFrames => OboeInterop.oboe_audio_stream_base_get_buffer_capacity_in_frames (instance);
	}

	public class AudioStream : AudioStreamBase, IDisposable
	{
		public AudioStream (IntPtr instance)
			: base (instance)
		{
			this.instance = instance;
		}

		bool disposed;
		readonly IntPtr instance;

		public void Dispose () // same as Close() ?
		{
			if (!disposed)
				OboeInterop.oboe_audio_stream_close (instance).CheckStatus ();
			disposed = true;
		}

		public OboeAudioApi AudioApi => OboeInterop.oboe_audio_stream_get_audio_api (instance);

		public double CalculateLatencyMillis ()
		{
			return OboeInterop.oboe_audio_stream_calculate_latency_millis (instance).CheckStatus ();
		}

		public int BytesPerFrame => OboeInterop.oboe_audio_stream_get_bytes_per_frame (instance);
		public int BytesPerSample => OboeInterop.oboe_audio_stream_get_bytes_per_sample (instance);
		public OboeStreamState State => OboeInterop.oboe_audio_stream_get_state (instance);
		public bool IsPlaying => OboeInterop.oboe_audio_stream_is_playing (instance);
		public bool UsesAAudio => OboeInterop.oboe_audio_stream_uses_aaudio (instance);
		public long FramesRead => OboeInterop.oboe_audio_stream_get_frames_read (instance);
		public int XRunCount => OboeInterop.oboe_audio_stream_get_x_run_count (instance).CheckStatus ();
		public long FramesWritten => OboeInterop.oboe_audio_stream_get_frames_written (instance);
		public long FramesPerBurst => OboeInterop.oboe_audio_stream_get_frames_per_burst (instance);

		public void SetBufferSizeInFrames (int value) => OboeInterop.oboe_audio_stream_set_buffer_size_in_frames (instance, value);

		public void GetTimestampe (int clockId, out long framePosition, out long timeNanoseconds)
			=> OboeInterop.oboe_audio_stream_get_timestamp (instance, clockId, out framePosition, out timeNanoseconds).CheckStatus ();

		public void Open () => OboeInterop.oboe_audio_stream_open (instance).CheckStatus ();

		public void Close () => OboeInterop.oboe_audio_stream_close (instance).CheckStatus ();

		public void Flush () => OboeInterop.oboe_audio_stream_flush (instance);

		public int Read (IntPtr buffer, int numFrames, long timeoutNanoseconds)
			=> OboeInterop.oboe_audio_stream_read (instance, buffer, numFrames, timeoutNanoseconds).CheckStatus ();

		public void Start () => OboeInterop.oboe_audio_stream_start (instance);

		public void Stop () => OboeInterop.oboe_audio_stream_stop (instance).CheckStatus ();

		public void Pause () => OboeInterop.oboe_audio_stream_pause (instance).CheckStatus ();

		public void Write (IntPtr buffer, int numFrames, long timeoutNanoseconds)
			=> OboeInterop.oboe_audio_stream_write (instance, buffer, numFrames, timeoutNanoseconds).CheckStatus ();
	}

	
	[System.Serializable]
	public class OboeException : Exception
	{
		static string ToMessage (OboeResult error)
		{
			return $"Oboe error occured (error code {error}";
		}

		public OboeException (OboeResult error)
			: this (ToMessage (error))
		{
		}

		public OboeException ()
		{
		}

		public OboeException (string message) : base (message)
		{
		}

		public OboeException (string message, Exception inner) : base (message, inner)
		{
		}

		protected OboeException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
		{
		}
	}
}
