using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace ArrayPoolExtensions
//{
    static byte[] FromBase64String(string value, out int bytesWritten)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(Encoding.UTF8.GetMaxByteCount(value.Length));
        int bufferSize = Encoding.UTF8.GetBytes(value, buffer);
        var decodedBuffer = ArrayPool<byte>.Shared.Rent(Base64.GetMaxDecodedFromUtf8Length(value.Length));
        try
        {
            Base64.DecodeFromUtf8(buffer.AsSpan(0, bufferSize), decodedBuffer, out int _, out bytesWritten);
            if (bytesWritten == 0)
            {
                throw new InvalidOperationException("Error writing to buffer.");
            }
        }
        catch
        {
            //decodedBuffer.;
            Console.WriteLine("Exception is occured!");
            throw;
        }
        return decodedBuffer;
    }
//}
