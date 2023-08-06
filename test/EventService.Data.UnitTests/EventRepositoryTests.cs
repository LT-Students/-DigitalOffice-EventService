using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.EntityFrameworkCore;
using Moq.AutoMock;
using NUnit.Framework;

namespace LT.DigitalOffice.EventService.Data.UnitTests;

public class EventRepositoryTests
{
  private IDataProvider _provider;
  private IEventRepository _repository;
  private DbContextOptions<EventServiceDbContext> _dbContext;

  private DbEvent _eventSimple;
  private DbEvent _eventInactive;
  private DbEvent _eventWithCategories;
  private DbEvent _eventWithUsers;

  private Guid _creatorId = Guid.NewGuid();
  private Guid _eventId = Guid.NewGuid();
  private Guid _categoryId1 = Guid.NewGuid();
  private Guid _categoryId2 = Guid.NewGuid();
  private Guid _userId1 = Guid.NewGuid();
  private Guid _userId2 = Guid.NewGuid();

  private AutoMocker _autoMocker;

  private void CreateEvents()
  {
    _eventSimple = new DbEvent()
    {
      Id = Guid.NewGuid(),
      Name = "Name1",
      Address = "Address1",
      Description = "Description1",
      Date = DateTime.Now,
      EndDate = DateTime.Now + TimeSpan.FromDays(2),
      Format = FormatType.Online,
      Access = AccessType.Opened,
      IsActive = true,
      CreatedBy = _creatorId,
      CreatedAtUtc = DateTime.Now,
      Users = new List<DbEventUser>()
    };

    _eventInactive = new DbEvent()
    {
      Id = Guid.NewGuid(),
      Name = "Name2",
      Address = "Address2",
      Description = "Description2",
      Date = DateTime.Now,
      EndDate = DateTime.Now + TimeSpan.FromDays(2),
      Format = FormatType.Online,
      Access = AccessType.Opened,
      IsActive = false,
      CreatedBy = _creatorId,
      CreatedAtUtc = DateTime.Now,
      Users = new List<DbEventUser>()
    };

    _eventWithCategories = new DbEvent()
    {
      Id = Guid.NewGuid(),
      Name = "Name3",
      Address = "Address3",
      Description = "Description3",
      Date = DateTime.Now,
      EndDate = DateTime.Now + TimeSpan.FromDays(2),
      Format = FormatType.Online,
      Access = AccessType.Opened,
      IsActive = true,
      CreatedBy = _creatorId,
      CreatedAtUtc = DateTime.Now,
      Users = new List<DbEventUser>(),
      EventsCategories = new List<DbEventCategory>()
      {
        new DbEventCategory()
        {
          Id = Guid.NewGuid(),
          EventId = _eventId,
          CategoryId = _categoryId1,
          CreatedBy = _creatorId,
          CreatedAtUtc= DateTime.Now
        },
        new DbEventCategory()
        {
          Id = Guid.NewGuid(),
          EventId = _eventId,
          CategoryId = _categoryId2,
          CreatedBy = _creatorId,
          CreatedAtUtc= DateTime.Now
        }
      }
    };

    _eventWithUsers = new DbEvent()
    {
      Id = Guid.NewGuid(),
      Name = "Name4",
      Address = "Address4",
      Description = "Description4",
      Date = DateTime.Now,
      EndDate = DateTime.Now + TimeSpan.FromDays(2),
      Format = FormatType.Online,
      Access = AccessType.Opened,
      IsActive = true,
      CreatedBy = _creatorId,
      CreatedAtUtc = DateTime.Now,
      Users = new List<DbEventUser>()
      {
        new DbEventUser()
        {
          Id = Guid.NewGuid(),
          EventId = _eventId,
          UserId = _userId1,
          Status = EventUserStatus.Participant,
          NotifyAtUtc = DateTime.Now + TimeSpan.FromDays(2),
          CreatedBy = _creatorId,
          CreatedAtUtc = DateTime.Now,
        },
        new DbEventUser()
        {
          Id = Guid.NewGuid(),
          EventId = _eventId,
          UserId = _userId2,
          Status = EventUserStatus.Invited,
          NotifyAtUtc = DateTime.Now + TimeSpan.FromDays(2),
          CreatedBy = _creatorId,
          CreatedAtUtc = DateTime.Now,
        }
      }
    };
  }

  private void CreateMemoryDb()
  {
    _dbContext = new DbContextOptionsBuilder<EventServiceDbContext>()
      .UseInMemoryDatabase(databaseName: "EventService")
      .Options;

    _provider = new EventServiceDbContext(_dbContext);
    _repository = new EventRepository(_provider);
  }

  private void SaveEvents()
  {
    _provider.Events.Add(_eventSimple);
    _provider.Events.Add(_eventInactive);
    _provider.Events.Add(_eventWithCategories);
    _provider.Events.Add(_eventWithUsers);
    _provider.Save();
  }

  [OneTimeSetUp]
  public void OneTimeSetUp()
  {
    _autoMocker = new AutoMocker();
  }

  [SetUp]
  public void SetUp()
  {
    CreateEvents();
    CreateMemoryDb();
    SaveEvents();
  }

  [TearDown]
  public void CleanDb()
  {
    if (_provider.IsInMemory())
    {
      _provider.EnsureDeleted();
    }
  }

  #region AddEvent

  [Test]
  public async Task ShouldCreateEventAsync()
  {
    DbEvent dbEvent = new DbEvent()
    {
      Id = Guid.NewGuid(),
      Name = "Name",
      Address = "Address",
      Description = "Description",
      Date = DateTime.Now,
      EndDate = DateTime.Now + TimeSpan.FromDays(2),
      Format = FormatType.Online,
      Access = AccessType.Opened,
      IsActive = true,
      CreatedBy = _creatorId,
      CreatedAtUtc = DateTime.Now,
      Users = new List<DbEventUser>()
    };

    Assert.DoesNotThrowAsync(async () => await _repository.CreateAsync(dbEvent));
    SerializerAssert.AreEqual(dbEvent, await _provider.Events.FirstOrDefaultAsync(e => e.Id == dbEvent.Id));
  }

  [Test]
  public async Task ShouldCreateEventWithCategoriesAsync()
  {
    DbEventCategory dbEventCategory1 = new DbEventCategory()
    {
      Id = Guid.NewGuid(),
      EventId = _eventId,
      CategoryId = _categoryId1,
      CreatedBy = _creatorId,
      CreatedAtUtc = DateTime.Now
    };
    DbEventCategory dbEventCategory2 = new DbEventCategory()
    {
      Id = Guid.NewGuid(),
      EventId = _eventId,
      CategoryId = _categoryId2,
      CreatedBy = _creatorId,
      CreatedAtUtc = DateTime.Now
    };

    DbEvent dbEvent = new DbEvent()
    {
      Id = Guid.NewGuid(),
      Name = "Name",
      Address = "Address",
      Description = "Description",
      Date = DateTime.Now,
      EndDate = DateTime.Now + TimeSpan.FromDays(2),
      Format = FormatType.Online,
      Access = AccessType.Opened,
      IsActive = true,
      CreatedBy = _creatorId,
      CreatedAtUtc = DateTime.Now,
      Users = new List<DbEventUser>(),
      EventsCategories = new List<DbEventCategory>()
      {
        dbEventCategory1,
        dbEventCategory2
      }
    };

    Assert.DoesNotThrowAsync(async () => await _repository.CreateAsync(dbEvent));
    Assert.AreSame(dbEvent, await _provider.Events.FirstOrDefaultAsync(e => e.Id == dbEvent.Id));
    Assert.AreSame(dbEventCategory1, await _provider.EventsCategories.FirstOrDefaultAsync(ec => ec.Id == dbEventCategory1.Id));
  }

  [Test]
  public void ShouldReturnNullForAddNullEventAsync()
  {
    Guid? dbEventId = null;

    Assert.DoesNotThrowAsync(async () => dbEventId = await _repository.CreateAsync(null));
    Assert.IsNull(dbEventId);
  }

  #endregion
}
