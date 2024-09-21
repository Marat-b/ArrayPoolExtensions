using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Buffers;

namespace ArrayPoolExtensions
{
    internal class Program
    {
        static void Main()
        {

            //chars = SpanIndexOf(chars.ToString(), replaceable, substitute);
            //ArrayPoolFunc(text, replaceable, substitute);

            //SpanCls spanCls = new SpanCls();
            //spanCls.SpanIndexOfWithArrayPoolCount();

            BenchmarkRunner.Run<SpanCls>();
            //Console.ReadLine();
        }


    }

   

    

    
}
