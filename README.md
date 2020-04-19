It is an experimental project that aims to bind Google/oboe for Xamarin.Android.

Not in active development anymore.

If you are somewhat serious about real-time audio, what you should do is not
to write audio processing callback handlers in .NET, but write them in native
languages (C/C++/Rust etc.) that does not involve GC and JIT, create some
entry point in PInvoke-able C function, and call it from .NET.

Therefore you most unlikely need this .NET binding.

