﻿using System;
using System.Xml.Serialization;

namespace PosePacket.WebError
{
    public class ErrorDetail
    {
        public string Message { get; set; }
        public int ErrorCode { get; set; }
    }
}