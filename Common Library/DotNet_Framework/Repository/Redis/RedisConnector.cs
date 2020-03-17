using LogicCore.Debug;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Redis
{
    public class RedisConnector
    {
        private readonly ConnectionMultiplexer _redis;
        public ConnectionMultiplexer RedisClient => _redis;

        public RedisConnector(string configuration)
        {
            try
            {
                _redis = ConnectionMultiplexer.Connect(configuration);
                _redis.ConfigurationChangedBroadcast += Redis_ConfigurationChangedBroadcast;
                _redis.ErrorMessage += Redis_ErrorMessage;
                _redis.ConnectionFailed += Redis_ConnectionFailed;
                _redis.InternalError += Redis_InternalError;
                _redis.ConnectionRestored += Redis_ConnectionRestored;
                _redis.ConfigurationChanged += Redis_ConfigurationChanged;
                _redis.HashSlotMoved += Redis_HashSlotMoved;
            }
            catch (Exception ex)
            {
                Dev.Assert(false, ex.StackTrace);
            }
        }

        private void Redis_HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
        }

        private void Redis_ConfigurationChanged(object sender, EndPointEventArgs e)
        {
        }

        private void Redis_ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
        }

        private void Redis_InternalError(object sender, InternalErrorEventArgs e)
        {
        }

        private void Redis_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
        }

        private void Redis_ErrorMessage(object sender, RedisErrorEventArgs e)
        {
        }

        private void Redis_ConfigurationChangedBroadcast(object sender, EndPointEventArgs e)
        {
        }
    }
}