﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#pragma warning disable 1591

using FreeSql.DataAnnotations;

namespace ViazyNetCore.IdentityService4.FreeSql.Entities
{
    [Table(Name = "ids_client_properties")]
    public class ClientProperty : Property
    {
        public long ClientId { get; set; }

        [Navigate(nameof(Entities.Client.ClientId))]
        public Client Client { get; set; }
    }
}