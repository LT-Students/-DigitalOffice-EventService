using System;
using System.Collections.Generic;
using System.Net;
using LT.DigitalOffice.EventService.Business.Commands.Event.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace LT.DigitalOffice.EventService.Business.UnitTests.Commands.Event;

public class CreateEventCommandTests
{
  private AutoMocker _autoMocker;
  private ICreateEventCommand _command;

  private CreateEventRequest _request;
  private DbEvent _dbEvent;

  [OneTimeSetUp]
  public void OneTimeSetUp()
  {
    

  }

  [SetUp]
  public void Setup()
  {

  }
}
