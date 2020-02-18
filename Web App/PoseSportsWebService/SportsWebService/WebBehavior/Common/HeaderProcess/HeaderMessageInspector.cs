using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using PosePacket.Header;
using SportsWebService.Authentication;

namespace SportsWebService.WebBehavior.Common.HeaderProcess
{
    /// <summary>
    /// This class is used to inspect the message and headers on the server side,
    /// This class is also used to intercept the message on the
    /// client side, before/after any request is made to the server.
    /// </summary>
    public class HeaderMessageInspector : IDispatchMessageInspector//, IClientMessageInspector
    {
        #region Message Inspector for Server

        /// <summary>
        /// This method is called on the server when a request is received from the client.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <param name="instanceContext"></param>
        /// <returns></returns>
        public object AfterReceiveRequest(ref Message request,
               IClientChannel channel, InstanceContext instanceContext)
        {
            if (ServerContext.IsExistData)
                return null;

            //Create a copy of the original message so that we can mess with it.
            //MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
            //request = buffer.CreateMessage();
            //Message messageCopy = buffer.CreateMessage();

            // Read the custom context data from the headers
            PoseHeaderMessage.ReadHeader(request);

            return null;
        }

        /// <summary>
        /// This method is called after processing a method on the server side and just
        /// before sending the response to the client.
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            // Prepare the request message copy to be modified
            //MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
            //reply = buffer.CreateMessage();

            //ServerContext.Current.EnCryptCredentials();
            //PoseHeader serviceHeader = new PoseHeader()
            //{
            //    Signature = ServerContext.Current._eSignature,
            //    SignatureIV = ServerContext.Current._eSignatureIV,
            //    Credentials = ServerContext.Current._eCredentials,
            //};

            //// Add the custom header to the request.
            //PoseHeaderMessage header = new PoseHeaderMessage(serviceHeader);
            //reply.Headers.Add(header);

            ServerContext.Current.Detach(OperationContext.Current);
        }

        #endregion

        #region Message Inspector for Client

        ///// <summary>
        ///// This method will be called from the client side just before any method is called.
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="channel"></param>
        ///// <returns></returns>
        //public object BeforeSendRequest(ref Message request, IClientChannel channel)
        //{
        //    // Prepare the request message copy to be modified
        //    MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
        //    request = buffer.CreateMessage();

        //    PoseHeader serviceHeader = new PoseHeader();
        //    ClientContext.CopyTo(serviceHeader);

        //    // Add the custom header to the request.
        //    PoseHeaderMessage header = new PoseHeaderMessage(serviceHeader);
        //    request.Headers.Add(header);

        //    return null;
        //}

        ///// <summary>
        ///// This method will be called after completion of a request to the server.
        ///// </summary>
        ///// <param name="reply"></param>
        ///// <param name="correlationState"></param>
        //public void AfterReceiveReply(ref Message reply, object correlationState)
        //{

        //}

        #endregion
    }
}
