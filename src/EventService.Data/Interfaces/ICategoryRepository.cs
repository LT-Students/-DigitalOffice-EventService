using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface ICategoryRepository
{
  bool DoesExistAllAsync(List<Guid> categoryIds);
}
