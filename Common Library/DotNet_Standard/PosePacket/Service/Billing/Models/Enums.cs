using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Billing.Models.Enums
{
    public enum PosePurchaseStateType
    {
        Unknown,
        Canceled,   // 취소
        Purchased,  // 구매
        Pause,      // 일시 중지
        Grace,      // 유예
        Pending,    // 보류
        Refund,     // 환불
        Deferred    // 연기
    }

    public enum PosePurchaseErrorType
    {
        _None_,
        FailStoreConnect,
        ServerError,
        SuccessPurchaseButServerError,
        AlreadyOwned,
        UnknownError,
    }
}