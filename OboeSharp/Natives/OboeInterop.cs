using System;
using System.Runtime.InteropServices;

using clockid_t = System.Int32;
using oboe_audio_stream_ptr_t = System.IntPtr;
using oboe_audio_stream_base_ptr_t = System.IntPtr;
using oboe_audio_stream_builder_ptr_t = System.IntPtr;
using oboe_audio_stream_callback_ptr_t = System.IntPtr;

using static OboeSharp.Natives.AAudioError;
using static OboeSharp.Natives.AAudioUsage;
using static OboeSharp.Natives.AAudioSessionId;
using static OboeSharp.Natives.AAudioContentType;
using static OboeSharp.Natives.AAudioInputPreset;
using static OboeSharp.Natives.AAudioSharingMode;
using static OboeSharp.Natives.AAudioStreamState;
using static OboeSharp.Natives.AAudioPerformanceMode;
using static OboeSharp.Natives.AAudioDirection;
using static OboeSharp.Natives.AAudioFormat;
using static OboeSharp.Natives.AAudioDataCallbackResult;

namespace OboeSharp.Natives
{
	#region AAudio enumerations

	enum AAudioDirection
	{
		AAUDIO_DIRECTION_OUTPUT,
		AAUDIO_DIRECTION_INPUT
	}

	enum AAudioFormat
	{
		AAUDIO_FORMAT_INVALID = -1,
		AAUDIO_FORMAT_UNSPECIFIED = 0,
		AAUDIO_FORMAT_PCM_I16,
		AAUDIO_FORMAT_PCM_FLOAT
	}

	enum AAudioError
	{
		AAUDIO_OK,
		AAUDIO_ERROR_BASE = -900,
		AAUDIO_ERROR_DISCONNECTED,
		AAUDIO_ERROR_ILLEGAL_ARGUMENT,
		AAUDIO_ERROR_INTERNAL = AAUDIO_ERROR_ILLEGAL_ARGUMENT + 2,
		AAUDIO_ERROR_INVALID_STATE,
		AAUDIO_ERROR_INVALID_HANDLE = AAUDIO_ERROR_INVALID_STATE + 3,
		AAUDIO_ERROR_UNIMPLEMENTED = AAUDIO_ERROR_INVALID_HANDLE + 2,
		AAUDIO_ERROR_UNAVAILABLE,
		AAUDIO_ERROR_NO_FREE_HANDLES,
		AAUDIO_ERROR_NO_MEMORY,
		AAUDIO_ERROR_NULL,
		AAUDIO_ERROR_TIMEOUT,
		AAUDIO_ERROR_WOULD_BLOCK,
		AAUDIO_ERROR_INVALID_FORMAT,
		AAUDIO_ERROR_OUT_OF_RANGE,
		AAUDIO_ERROR_NO_SERVICE,
		AAUDIO_ERROR_INVALID_RATE,
	}

	enum AAudioStreamState
	{
		AAUDIO_STREAM_STATE_UNINITIALIZED = 0,
		AAUDIO_STREAM_STATE_UNKNOWN,
		AAUDIO_STREAM_STATE_OPEN,
		AAUDIO_STREAM_STATE_STARTING,
		AAUDIO_STREAM_STATE_STARTED,
		AAUDIO_STREAM_STATE_PAUSING,
		AAUDIO_STREAM_STATE_PAUSED,
		AAUDIO_STREAM_STATE_FLUSHING,
		AAUDIO_STREAM_STATE_FLUSHED,
		AAUDIO_STREAM_STATE_STOPPING,
		AAUDIO_STREAM_STATE_STOPPED,
		AAUDIO_STREAM_STATE_CLOSING,
		AAUDIO_STREAM_STATE_CLOSED,
		AAUDIO_STREAM_STATE_DISCONNECTED
	}


	enum AAudioSharingMode
	{
		AAUDIO_SHARING_MODE_EXCLUSIVE,
		AAUDIO_SHARING_MODE_SHARED
	}


	enum AAudioPerformanceMode
	{
		AAUDIO_PERFORMANCE_MODE_NONE = 10,
		AAUDIO_PERFORMANCE_MODE_POWER_SAVING,
		AAUDIO_PERFORMANCE_MODE_LOW_LATENCY
	}

	enum AAudioUsage
	{
		AAUDIO_USAGE_MEDIA = 1,
		AAUDIO_USAGE_VOICE_COMMUNICATION = 2,
		AAUDIO_USAGE_VOICE_COMMUNICATION_SIGNALLING = 3,
		AAUDIO_USAGE_ALARM = 4,
		AAUDIO_USAGE_NOTIFICATION = 5,
		AAUDIO_USAGE_NOTIFICATION_RINGTONE = 6,
		AAUDIO_USAGE_NOTIFICATION_EVENT = 10,
		AAUDIO_USAGE_ASSISTANCE_ACCESSIBILITY = 11,
		AAUDIO_USAGE_ASSISTANCE_NAVIGATION_GUIDANCE = 12,
		AAUDIO_USAGE_ASSISTANCE_SONIFICATION = 13,
		AAUDIO_USAGE_GAME = 14,
		AAUDIO_USAGE_ASSISTANT = 16
	}

	enum AAudioContentType
	{
		AAUDIO_CONTENT_TYPE_SPEECH = 1,
		AAUDIO_CONTENT_TYPE_MUSIC = 2,
		AAUDIO_CONTENT_TYPE_MOVIE = 3,
		AAUDIO_CONTENT_TYPE_SONIFICATION = 4
	}

	enum AAudioInputPreset
	{
		AAUDIO_INPUT_PRESET_GENERIC = 1,
		AAUDIO_INPUT_PRESET_CAMCORDER = 5,
		AAUDIO_INPUT_PRESET_VOICE_RECOGNITION = 6,
		AAUDIO_INPUT_PRESET_VOICE_COMMUNICATION = 7,
		AAUDIO_INPUT_PRESET_UNPROCESSED = 9,
	}

	enum AAudioSessionId
	{
		AAUDIO_SESSION_ID_NONE = -1,
		AAUDIO_SESSION_ID_ALLOCATE = 0,
	}

	enum AAudioDataCallbackResult
	{
		AAUDIO_CALLBACK_RESULT_CONTINUE = 0,
		AAUDIO_CALLBACK_RESULT_STOP,
	}

	#endregion

	#region native enums

	enum OboeStreamState
	{
		Uninitialized = AAUDIO_STREAM_STATE_UNINITIALIZED,
        Unknown = AAUDIO_STREAM_STATE_UNKNOWN,
		Open = AAUDIO_STREAM_STATE_OPEN,
        Starting = AAUDIO_STREAM_STATE_STARTING,
        Started = AAUDIO_STREAM_STATE_STARTED,
        Pausing = AAUDIO_STREAM_STATE_PAUSING,
        Paused = AAUDIO_STREAM_STATE_PAUSED,
        Flushing = AAUDIO_STREAM_STATE_FLUSHING,
        Flushed = AAUDIO_STREAM_STATE_FLUSHED,
        Stopping = AAUDIO_STREAM_STATE_STOPPING,
        Stopped = AAUDIO_STREAM_STATE_STOPPED,
        Closing = AAUDIO_STREAM_STATE_CLOSING,
        Closed = AAUDIO_STREAM_STATE_CLOSED,
        Disconnected = AAUDIO_STREAM_STATE_DISCONNECTED,
    };

	enum OboeDirection
	{
		Output = AAUDIO_DIRECTION_OUTPUT,
		Input = AAUDIO_DIRECTION_INPUT,
    };

	enum OboeAudioFormat
	{
		Invalid = AAUDIO_FORMAT_INVALID,
        Unspecified = AAUDIO_FORMAT_UNSPECIFIED,
        I16 = AAUDIO_FORMAT_PCM_I16,
        Float = AAUDIO_FORMAT_PCM_FLOAT,
    };

	enum OboeDataCallbackResult
	{
		Continue = AAUDIO_CALLBACK_RESULT_CONTINUE,
        Stop = AAUDIO_CALLBACK_RESULT_STOP,
    }

	enum OboeResult
	{
		OK,
        ErrorBase = AAUDIO_ERROR_BASE,
        ErrorDisconnected = AAUDIO_ERROR_DISCONNECTED,
        ErrorIllegalArgument = AAUDIO_ERROR_ILLEGAL_ARGUMENT,
        ErrorInternal = AAUDIO_ERROR_INTERNAL,
        ErrorInvalidState = AAUDIO_ERROR_INVALID_STATE,
        ErrorInvalidHandle = AAUDIO_ERROR_INVALID_HANDLE,
        ErrorUnimplemented = AAUDIO_ERROR_UNIMPLEMENTED,
        ErrorUnavailable = AAUDIO_ERROR_UNAVAILABLE,
        ErrorNoFreeHandles = AAUDIO_ERROR_NO_FREE_HANDLES,
        ErrorNoMemory = AAUDIO_ERROR_NO_MEMORY,
        ErrorNull = AAUDIO_ERROR_NULL,
        ErrorTimeout = AAUDIO_ERROR_TIMEOUT,
        ErrorWouldBlock = AAUDIO_ERROR_WOULD_BLOCK,
        ErrorInvalidFormat = AAUDIO_ERROR_INVALID_FORMAT,
        ErrorOutOfRange = AAUDIO_ERROR_OUT_OF_RANGE,
        ErrorNoService = AAUDIO_ERROR_NO_SERVICE,
        ErrorInvalidRate = AAUDIO_ERROR_INVALID_RATE,
    }

	enum OboeSharingMode
	{
		Exclusive = AAUDIO_SHARING_MODE_EXCLUSIVE,
        Shared = AAUDIO_SHARING_MODE_SHARED,
    }

	enum OboePerformanceMode
	{
		None = AAUDIO_PERFORMANCE_MODE_NONE,
        PowerSaving = AAUDIO_PERFORMANCE_MODE_POWER_SAVING,
        LowLatency = AAUDIO_PERFORMANCE_MODE_LOW_LATENCY
	}

	enum OboeAudioApi
	{
		Unspecified = 0,
		OpenSLES,
		AAudio
	}

	enum OboeUsage
	{
		Media = AAUDIO_USAGE_MEDIA,
        VoiceCommunication = AAUDIO_USAGE_VOICE_COMMUNICATION,
        VoiceCommunicationSignalling = AAUDIO_USAGE_VOICE_COMMUNICATION_SIGNALLING,
        Alarm = AAUDIO_USAGE_ALARM,
        Notification = AAUDIO_USAGE_NOTIFICATION,
        NotificationRingtone = AAUDIO_USAGE_NOTIFICATION_RINGTONE,
        NotificationEvent = AAUDIO_USAGE_NOTIFICATION_EVENT,
        AssistanceAccessibility = AAUDIO_USAGE_ASSISTANCE_ACCESSIBILITY,
        AssistanceNavigationGuidance = AAUDIO_USAGE_ASSISTANCE_NAVIGATION_GUIDANCE,
        AssistanceSonification = AAUDIO_USAGE_ASSISTANCE_SONIFICATION,
        Game = AAUDIO_USAGE_GAME,
        Assistant = AAUDIO_USAGE_ASSISTANT,
    }

	enum OboeContentType
	{
		Speech = AAUDIO_CONTENT_TYPE_SPEECH,
		Music = AAUDIO_CONTENT_TYPE_MUSIC,
		Movie = AAUDIO_CONTENT_TYPE_MOVIE,
		Sonification = AAUDIO_CONTENT_TYPE_SONIFICATION,
	}

	enum OboeInputPreset
	{
		Generic = AAUDIO_INPUT_PRESET_GENERIC,
		Camcorder = AAUDIO_INPUT_PRESET_CAMCORDER,
		VoiceRecognition = AAUDIO_INPUT_PRESET_VOICE_RECOGNITION,
		VoiceCommunication = AAUDIO_INPUT_PRESET_VOICE_COMMUNICATION,
		Unprocessed = AAUDIO_INPUT_PRESET_UNPROCESSED,
	}

	enum OboeSessionId
	{
		None = AAUDIO_SESSION_ID_NONE,
		Allocate = AAUDIO_SESSION_ID_ALLOCATE,
	}

	#endregion

	[StructLayout (LayoutKind.Sequential)]
	struct OboeResultWithValueInt32
	{
		internal OboeResult error;
		internal int value;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct OboeResultWithValueDouble
	{
		internal OboeResult error;
		internal double value;
	}

	class OboeInterop
	{
		const string LibraryName = "oboe-c";

		// AudioStream

		[DllImport (LibraryName)]
		static extern OboeResult oboe_audio_stream_open (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeResult oboe_audio_stream_close (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeResult oboe_audio_stream_start (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeResult oboe_audio_stream_pause (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeResult oboe_audio_stream_flush (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeResult oboe_audio_stream_stop (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeStreamState oboe_audio_stream_get_state (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeResult oboe_audio_stream_wait_for_state_change (oboe_audio_stream_ptr_t instance, OboeStreamState inputState, out OboeStreamState nextState, long timeoutNanoseconds);

		[DllImport (LibraryName)]
		static extern OboeResultWithValueInt32 oboe_audio_stream_set_buffer_size_in_frames (oboe_audio_stream_ptr_t instance, int requestedFrames);

		[DllImport (LibraryName)]
		static extern OboeResultWithValueInt32 oboe_audio_stream_get_x_run_count (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern int oboe_audio_stream_get_frames_per_burst (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern bool oboe_audio_stream_is_playing (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern int oboe_audio_stream_get_bytes_per_frame (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern int oboe_audio_stream_get_bytes_per_sample (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern long oboe_audio_stream_get_frames_written (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern long oboe_audio_stream_get_frames_read (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeResultWithValueDouble oboe_audio_stream_calculate_latency_millis (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeResult oboe_audio_stream_get_timestamp (oboe_audio_stream_ptr_t instance, clockid_t clockId, out long framePosition, out long timeNanoseconds);

		[DllImport (LibraryName)]
		static extern OboeResultWithValueInt32 oboe_audio_stream_write (oboe_audio_stream_ptr_t instance, IntPtr buffer, int numFrames, long timeoutNanoseconds);

		[DllImport (LibraryName)]
		static extern OboeResultWithValueInt32 oboe_audio_stream_read (oboe_audio_stream_ptr_t instance, IntPtr buffer, int numFrames, long timeoutNanoseconds);

		[DllImport (LibraryName)]
		static extern OboeAudioApi oboe_audio_stream_get_audio_api (oboe_audio_stream_ptr_t instance);

		[DllImport (LibraryName)]
		static extern bool oboe_audio_stream_uses_aaudio (oboe_audio_stream_ptr_t instance);

		// AudioStreamBase

		[DllImport (LibraryName)]
		static extern int oboe_audio_stream_base_get_channel_count (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeDirection oboe_audio_stream_base_get_direction (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern int oboe_audio_stream_base_get_sample_rate (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern int oboe_audio_stream_base_get_frames_per_callback (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeAudioFormat oboe_audio_stream_base_get_format (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern int oboe_audio_stream_base_get_buffer_size_in_frames (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern int oboe_audio_stream_base_get_buffer_capacity_in_frames (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeSharingMode oboe_audio_stream_base_get_sharing_mode (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboePerformanceMode oboe_audio_stream_base_get_performance_mode (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern int oboe_audio_stream_base_get_device_id (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_callback_ptr_t oboe_audio_stream_base_get_callback (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeUsage oboe_audio_stream_base_get_usage (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeContentType oboe_audio_stream_base_get_content_type (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeInputPreset oboe_audio_stream_base_get_input_preset (oboe_audio_stream_base_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeSessionId oboe_audio_stream_base_get_session_id (oboe_audio_stream_base_ptr_t instance);

		// AudioStreamBuilder

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_create ();

		[DllImport (LibraryName)]
		static extern void oboe_audio_stream_builder_delete (oboe_audio_stream_builder_ptr_t instance);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_channel_count (oboe_audio_stream_builder_ptr_t instance, int channelCount);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_direction (oboe_audio_stream_builder_ptr_t instance, OboeDirection direction);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_sample_rate (oboe_audio_stream_builder_ptr_t instance, int sampleRate);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_frames_per_callback (oboe_audio_stream_builder_ptr_t instance, int framesPerCallback);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_format (oboe_audio_stream_builder_ptr_t instance, OboeAudioFormat format);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_buffer_capacity_in_frames (oboe_audio_stream_builder_ptr_t instance, int bufferCapacityInFrames);

		[DllImport (LibraryName)]
		static extern OboeAudioApi oboe_audio_stream_builder_get_audio_api (oboe_audio_stream_builder_ptr_t instance);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_audio_api (oboe_audio_stream_builder_ptr_t instance, OboeAudioApi audioApi);

		[DllImport (LibraryName)]
		static extern bool oboe_audio_stream_builder_is_aaudio_supported (oboe_audio_stream_builder_ptr_t instance);

		[DllImport (LibraryName)]
		static extern bool oboe_audio_stream_builder_is_aaudio_recommended (oboe_audio_stream_builder_ptr_t instance);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_sharing_mode (oboe_audio_stream_builder_ptr_t instance, OboeSharingMode sharingMode);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_performance_mode (oboe_audio_stream_builder_ptr_t instance, OboePerformanceMode performanceMode);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_usage (oboe_audio_stream_builder_ptr_t instance, OboeUsage usage);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_content_type (oboe_audio_stream_builder_ptr_t instance, OboeContentType contentType);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_input_preset (oboe_audio_stream_builder_ptr_t instance, OboeInputPreset inputPreset);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_session_id (oboe_audio_stream_builder_ptr_t instance, OboeSessionId sessionId);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_device_id (oboe_audio_stream_builder_ptr_t instance, int deviceId);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_callback (oboe_audio_stream_builder_ptr_t instance, oboe_audio_stream_callback_ptr_t streamCallback);

		[DllImport (LibraryName)]
		static extern oboe_audio_stream_builder_ptr_t oboe_audio_stream_builder_set_default_frames_per_burst (oboe_audio_stream_builder_ptr_t instance, int defaultFramesPerBurst);

		[DllImport (LibraryName)]
		static extern int oboe_audio_stream_builder_get_default_frames_per_burst (oboe_audio_stream_builder_ptr_t instance);

		[DllImport (LibraryName)]
		static extern OboeResult oboe_audio_stream_builder_open_stream (oboe_audio_stream_builder_ptr_t instance, oboe_audio_stream_ptr_t stream);

		// AudioStreamCallback
		[DllImport (LibraryName)]
		static extern oboe_audio_stream_callback_ptr_t oboe_audio_stream_callback_create ();

		[DllImport (LibraryName)]
		static extern void oboe_audio_stream_callback_free (oboe_audio_stream_callback_ptr_t instance);

		delegate OboeDataCallbackResult on_audio_ready_func (oboe_audio_stream_ptr_t oboeStream, IntPtr audioData, int numFrames);
		delegate void on_error_close_func (oboe_audio_stream_ptr_t oboeStream, OboeResult error);

		[DllImport (LibraryName)]
		static extern void oboe_audio_stream_callback_set_on_audio_ready (oboe_audio_stream_callback_ptr_t instance, on_audio_ready_func onAudioReady);

		[DllImport (LibraryName)]
		static extern void oboe_audio_stream_callback_set_on_error_before_close (oboe_audio_stream_callback_ptr_t instance, on_error_close_func onErrorBeforeClose);

		[DllImport (LibraryName)]
		static extern void oboe_audio_stream_callback_set_on_error_after_close (oboe_audio_stream_callback_ptr_t instance, on_error_close_func onErrorAfterClose);

	}
}
