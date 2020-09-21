using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.InfraStructure
{
    public interface IGoogleOAuth
    {
        void Login(string clientId);
    }
}