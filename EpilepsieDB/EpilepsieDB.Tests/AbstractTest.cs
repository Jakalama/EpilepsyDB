using System;
using Xunit.Abstractions;
using System.IO;

namespace EpilepsieDB.Tests
{
    public abstract class AbstractTest : IDisposable
    {
        protected AbstractTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);
        }

        public virtual void Dispose()
        {
            Console.SetOut(Console.Out);
        }

        private class Converter : StringWriter
        {
            ITestOutputHelper _output;
            public Converter(ITestOutputHelper output)
            {
                _output = output;
            }

            public override void WriteLine(string message)
            {
                _output.WriteLine(message);
            }
        }


    }
}
