﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface IEventCategoryRepository
{
  bool DoesExistAsync(Guid eventId, List<Guid> categoryIds);

  Task<bool> RemoveAsync(Guid eventId, List<Guid> categoryIds);
}
