﻿using Dapper;
using LogicCore.Debug;
using LogicCore.Thread;
using LogicCore.Utility.ThirdPartyLog;
using Repository.Mysql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Request
{
    /// <summary>
    /// 사용후 Dispose 해줄것
    /// </summary>
    /// <typeparam name="Tout"></typeparam>
    public abstract class Query<T_in, T_out> : IDisposable
    {
        protected T_in _input;
        protected T_out _output;

        public Query()
        {
            OnAlloc();
        }

        public abstract void OnAlloc();

        public virtual void OnFree()
        {
            _input = default;
            _output = default;
        }

        public virtual void SetInput(T_in input) => _input = input;

        public abstract T_out OnQuery();

        public Task<T_out> OnQueryAsync()
        {
            return AsyncHelper.Async(OnQuery);
        }

        public void Dispose()
        {
            OnFree();
        }
    }

    public abstract class MysqlQuery<T_in, T_out> : Query<T_in, T_out>
    {
        public DynamicParameters Parmeters { get; private set; }
        public EntityStatus EntityStatus { get; private set; }
        public IDbTransaction transaction { get; set; }

        public override void OnAlloc()
        {
            Parmeters = new DynamicParameters();

            Dev.Assert(EntityStatus == null);
        }

        public override void OnFree()
        {
            base.OnFree();

            Parmeters = null;
            EntityStatus = null;

            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }
        }

        public override void SetInput(T_in input)
        {
            base.SetInput(input);

            BindParameters();
        }

        public abstract void BindParameters();

        public virtual void OnError(EntityStatus entityStatus)
        {
            transaction?.Rollback();
            EntityStatus = entityStatus;

            foreach (var error in entityStatus.Errors)
            {
                Log4Net.WriteLog($"Error: {error.ErrorMessage} - Member: {error.MemberNames}", Log4Net.Level.ERROR);
            }
        }
    }

    public abstract class RedisQuery<T_in, T_out> : Query<T_in, T_out>
    {
    }
}