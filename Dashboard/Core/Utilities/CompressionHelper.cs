using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Core.Utilities
{
    public class CompressionHelper
    {
        private const int BufferChunkSize = 256;

        public static string EncodeToZipBase64(string s)
        {
            return EncodeToZipBase64(s, Encoding.ASCII);
        }

        public static string EncodeToZipBase64(string s, Encoding oEncoding)
        {
            if (s == null)
            {
                throw new ArgumentException("s");
            }

            if (s == string.Empty)
            {
                return string.Empty;
            }

            byte[] oEncodedBytes = oEncoding.GetBytes(s);

            using (MemoryStream oMemoryStream = new MemoryStream())
            {
                using (GZipStream oZipStream = new GZipStream(oMemoryStream, CompressionMode.Compress, true))
                {
                    oZipStream.Write(oEncodedBytes, 0, oEncodedBytes.Length);
                    oZipStream.Close();
                }

                byte[] oBuffer = oMemoryStream.ToArray();
                return Convert.ToBase64String(oBuffer, 0, oBuffer.Length);
            }
        }

        public static string DecodeFromZipBase64(string s)
        {
            return DecodeFromZipBase64(s, Encoding.ASCII);
        }

        public static string DecodeFromZipBase64(string s, Encoding oEncoding)
        {
            if (s == null)
            {
                throw new ArgumentException("s");
            }

            if (s == string.Empty)
            {
                return string.Empty;
            }

            try
            {
                using (MemoryStream oMemoryStream = new MemoryStream(Convert.FromBase64String(s)))
                {
                    if (oMemoryStream.Length < 4)
                    {
                        throw new Exception("Invalid zip stream. Encoded value: " + s);
                    }

                    byte[] oSizeBytes = new byte[4];
                    oMemoryStream.Position = oMemoryStream.Length - 4;
                    oMemoryStream.Read(oSizeBytes, 0, 4);
                    oMemoryStream.Position = 0;
                    int iDecodedBytesCount = BitConverter.ToInt32(oSizeBytes, 0);

                    using (GZipStream oZipStream = new GZipStream(oMemoryStream, CompressionMode.Decompress))
                    {
                        byte[] oBuffer = new byte[iDecodedBytesCount + BufferChunkSize];
                        int iReadBytesCount = readAllBytesFromStream(oZipStream, oBuffer);
                        return oEncoding.GetString(oBuffer, 0, iReadBytesCount);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to decompess string: [{0}]", s), ex);
            }
        }

        public static int readAllBytesFromStream(Stream oStream, byte[] oBuffer)
        {
            int iOffset = 0;
            int iTotalCount = 0;

            while (true)
            {
                int oBytesRead = oStream.Read(oBuffer, iOffset, BufferChunkSize);
                if (oBytesRead == 0)
                {
                    break;
                }
                iOffset += oBytesRead;
                iTotalCount += oBytesRead;
            }
            return iTotalCount;
        }
        
    }
}
