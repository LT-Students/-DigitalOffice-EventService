using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface ICategoryRepository
{
  public bool DoesExistAllAsync(List<Guid> categoryIds);
}
