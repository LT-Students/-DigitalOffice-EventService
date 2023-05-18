﻿using System;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Mappers.Db.Interfaces;

[AutoInject]
public interface IDbEventFileMapper
{
  DbEventFile Map(Guid fileId, Guid eventId);
}