﻿using System;

namespace EAuction.Persistence
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
