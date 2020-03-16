using System;
using System.Collections.Generic;

namespace PosePacket.Header
{
    public class PoseHeader
    {
        public const string HEADER_NAME = "EncryptData";
        public const string HEADER_NAMESPACE = "http://pose-sports-predict.com";

        public byte[] eSignature { get; set; }
        public byte[] eSignatureIV { get; set; }
        public byte[] eCredentials { get; set; }

        public override string ToString()
        {
            List<byte> buffer = new List<byte>();

            int lenth = eSignature.Length;
            buffer.AddRange(BitConverter.GetBytes(lenth));
            buffer.AddRange(eSignature);

            lenth = eSignatureIV.Length;
            buffer.AddRange(BitConverter.GetBytes(lenth));
            buffer.AddRange(eSignatureIV);

            lenth = eCredentials.Length;
            buffer.AddRange(BitConverter.GetBytes(lenth));
            buffer.AddRange(eCredentials);

            return Convert.ToBase64String(buffer.ToArray());
        }

        public static PoseHeader ParseFromBase64(string base64String)
        {
            PoseHeader result = new PoseHeader();

            byte[] buffer = Convert.FromBase64String(base64String);

            int curPosition = 0;
            int bufferSize = 0;

            bufferSize = BitConverter.ToInt32(buffer, curPosition);
            curPosition += 4;
            result.eSignature = new byte[bufferSize];
            Array.Copy(buffer, curPosition, result.eSignature, 0, bufferSize);
            curPosition += bufferSize;

            bufferSize = BitConverter.ToInt32(buffer, curPosition);
            curPosition += 4;
            result.eSignatureIV = new byte[bufferSize];
            Array.Copy(buffer, curPosition, result.eSignatureIV, 0, bufferSize);
            curPosition += bufferSize;

            bufferSize = BitConverter.ToInt32(buffer, curPosition);
            curPosition += 4;
            result.eCredentials = new byte[bufferSize];
            Array.Copy(buffer, curPosition, result.eCredentials, 0, bufferSize);
            curPosition += bufferSize;

            return result;
        }
    }
}