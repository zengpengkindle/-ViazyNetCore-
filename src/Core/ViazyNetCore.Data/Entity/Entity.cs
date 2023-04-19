﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;

namespace ViazyNetCore
{
    public interface IEntity<TKey>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        TKey Id { get; set; }
    }

    public interface IEntity : IEntity<long>
    {
    }

    public class Entity<TKey> : IEntity<TKey>
    {

        /// <summary>
        /// 主键Id
        /// </summary>
        [Description("主键Id")]
        [Snowflake]
        [Column(Position = 1, IsIdentity = false, IsPrimary = true)]
        [JsonProperty(Order = -30)]
        public virtual TKey Id { get; set; }
    }

    public class Entity : Entity<long>
    {
    }
}