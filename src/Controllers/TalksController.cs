﻿using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers
{
  [ApiController]
  [Route("api/camps/{moniker}/talks")]
  public class TalksController : ControllerBase
  {
    private readonly ICampRepository _repository;
    private readonly IMapper _mapper;
    private readonly LinkGenerator _linkGenerator;

    public TalksController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator)
    {
      _repository = repository;
      _mapper = mapper;
      _linkGenerator = linkGenerator;
    }

    [HttpGet]
    public async Task<ActionResult<TalkModel[]>> Get(string moniker)
    {
      try
      {
        var talks = await _repository.GetTalksByMonikerAsync(moniker, true);
        return _mapper.Map<TalkModel[]>(talks);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get Talks for this Camp");
      }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TalkModel>> Get(string moniker, int id)
    {
      try
      {
        var talk = await _repository.GetTalkByMonikerAsync(moniker, id, true);
        if (talk == null)
        {
          return NotFound("Cound not find talk");
        }
        return _mapper.Map<TalkModel>(talk);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get this Talk");
      }
    }

    [HttpPost]
    public async Task<ActionResult<TalkModel>> Post(string moniker, TalkModel model)
    {
      try
      {
        var camp = await _repository.GetCampAsync(moniker);
        if (camp == null)
        {
          return BadRequest("Camp does not exist");
        }

        var talk =_mapper.Map<Talk>(model);
        talk.Camp = camp;

        if (model.Speaker == null)
        {
          return BadRequest("Speaker not provided");
        }
        var speaker = await _repository.GetSpeakerAsync(model.Speaker.SpeakerId);
        if (speaker == null)
        {
          return BadRequest("Speaker could not be found");
        }

        talk.Speaker = speaker;

        _repository.Add(talk);

        if (await _repository.SaveChangesAsync())
        {
          var url = _linkGenerator.GetPathByAction(HttpContext, "Get", values: new { moniker, id = talk.TalkId });

          return Created(url, _mapper.Map<TalkModel>(talk));
        }

        return BadRequest("Failed to create new talk");
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get this Talk");
      }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TalkModel>> Put(string moniker, int id, TalkModel model)
    {
      try
      {
        var talk = await _repository.GetTalkByMonikerAsync(moniker, id, true);
        if (talk == null)
        {
          return NotFound("Talk does not exist");
        }

        _mapper.Map(model, talk);

        if (model.Speaker != null)
        {
          var speaker = await _repository.GetSpeakerAsync(model.Speaker.SpeakerId);
          if (speaker != null)
          {
            talk.Speaker = speaker;
          }
        }

        if (await _repository.SaveChangesAsync())
        {
          return _mapper.Map<TalkModel>(talk);
        }

        return BadRequest("Could not save Talk");
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get this Talk");
      }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(string moniker, int id)
    {
      try
      {
        var talk = await _repository.GetTalkByMonikerAsync(moniker, id);
        if (talk == null)
        {
          return NotFound("Talk does not exist");
        }

        _repository.Delete(talk);

        if (await _repository.SaveChangesAsync())
        {
          return Ok();
        }

        return BadRequest("Could not delete Talk");
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get this Talk");
      }
    }
  }
}
