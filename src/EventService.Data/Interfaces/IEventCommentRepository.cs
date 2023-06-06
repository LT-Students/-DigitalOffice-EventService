using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface IEventCommentRepository
{
  Task<Guid?> CreateAsync(DbEventComment dbEventComment);
  Task<bool> EditContentAsync(Guid commentId, JsonPatchDocument<DbEventComment> request);
  Task<(bool, List<Guid> filesIds, List<Guid> imagesIds)> EditIsActiveAsync(Guid commentId, JsonPatchDocument<DbEventComment> request);
  Task<DbEventComment> GetAsync(Guid commentId);
  Task<bool> DoesExistAsync(Guid commentId);
  Task<bool> HasChildCommentsAsync(Guid commentId);
}
