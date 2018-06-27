using System;
using NUnit.Framework;

namespace OboeSharp.Tests
{
	[TestFixture]
	public class TestsSample
	{

		[SetUp]
		public void Setup () { }


		[TearDown]
		public void Tear () { }

		[Test]
		public void BasicProperties ()
		{
			using (var builder = new AudioStreamBuilder ()) {
				foreach (var pi in builder.GetType ().GetProperties ())
					if (pi.GetMethod != null)
						Console.Error.WriteLine ($"{pi}: {pi.GetValue (builder)}");
				var stream = builder.OpenStream ();
				foreach (var pi in stream.GetType ().GetProperties ())
					if (pi.GetMethod != null)
						Console.Error.WriteLine ($"{pi}: {pi.GetValue (stream)}");
			}
		}
	}
}
