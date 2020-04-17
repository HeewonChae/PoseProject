using LogicCore.Utility;
using MessagePack;
using SportsWebService.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SportsWebService.Logics
{
    public static class StreamExtensions
    {
        public static T StreamDeserialize<T>(this Stream stream)
        {
            byte[] data = stream.ConvertToByteArray();

            if (data == null || data.Length == 0)
                return default;

            return MessagePackSerializer.Deserialize<T>(data);
        }

        public static T StreamDeserialize<T>(this Stream stream, byte[] signature, byte[] signatureIV)
        {
            byte[] e_data = stream.ConvertToByteArray();

            if (e_data == null || e_data.Length == 0)
                return default;

            var data = Singleton.Get<CryptoFacade>().Decrypt_AES(e_data, signature, signatureIV);

            return MessagePackSerializer.Deserialize<T>(data);
        }

        public static Stream SerializeToStream(this object data)
        {
            var byteArray = MessagePackSerializer.Serialize(data);

            return new MemoryStream(byteArray);
        }

        public static Stream SerializeToStream(this object data, byte[] signature, byte[] signatureIV)
        {
            var byteArray = MessagePackSerializer.Serialize(data);
            var e_byteArray = Singleton.Get<CryptoFacade>().Encrypt_AES(byteArray, signature, signatureIV);

            return new MemoryStream(e_byteArray);
        }

        public static byte[] ConvertToByteArray(this Stream Stream)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}