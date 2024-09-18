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
            BenchmarkRunner.Run<SpanCls>();
            //Console.ReadLine();
        }


    }

   

    

    
}
