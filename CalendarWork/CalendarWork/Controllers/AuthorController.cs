using AutoMapper;
using CalendarWork.Authorization;
using CalendarWork.Core;
using CalendarWork.Core.Constants;
using CalendarWork.Core.Dtos;
using CalendarWork.Core.Entities;
using CalendarWork.Core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using static CalendarWork.Core.Constants.SystemConstants;

namespace CalendarWork.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly CalendarWorkDbContext _context;
        private readonly IMapper _mapper;

        public AuthorController(CalendarWorkDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ClaimRequirement(FunctionCode.AUTHOR, CommandCode.VIEW)]
        public async Task<IActionResult> Get()
        {
            ResponseRequest response = new ResponseRequest();

            var authors = await _context.Authors.ToListAsync();

            if (authors.Count > 0)
            {
                var authorDtos = _mapper.Map<List<AuthorDto>>(authors);

                response.StatusCode = HttpStatusCode.OK;
                response.Message = Notification.GET_SUCCESS;
                response.Data = authorDtos;

                return Ok(response);
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = Notification.GET_FAIL;

                return BadRequest(response);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.AUTHOR, CommandCode.CREATE)]
        public async Task<IActionResult> Post(AuthorDto obj)
        {
            ResponseRequest response = new ResponseRequest();

            try
            {
                var author = _mapper.Map<Author>(obj);
                _context.Authors.Add(author);
                if(await _context.SaveChangesAsync() > 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = Notification.POST_SUCCESS;
                    response.Data = author;

                    return Ok(response);
                }
            }
            catch (Exception ex) {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = ex.Message;

                return BadRequest(response);
            }

            response.StatusCode = HttpStatusCode.BadRequest;
            response.Message = Notification.POST_FAIL;

            return BadRequest(response);
        }

        [HttpPut]
        [ClaimRequirement(FunctionCode.AUTHOR, CommandCode.UPDATE)]
        public async Task<IActionResult> Put(AuthorDto obj)
        {
            ResponseRequest response = new ResponseRequest();

            try
            {
                var author = _mapper.Map<Author>(obj);
                _context.Authors.Update(author);
                if (await _context.SaveChangesAsync() > 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = Notification.PUT_SUCCESS;
                    response.Data = author;

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = ex.Message;

                return BadRequest(response);
            }

            response.StatusCode = HttpStatusCode.BadRequest;
            response.Message = Notification.PUT_FAIL;

            return BadRequest(response);
        }

        [HttpDelete]
        [ClaimRequirement(FunctionCode.AUTHOR, CommandCode.DELETE)]
        public async Task<IActionResult> Delete(int id)
        {
            ResponseRequest response = new ResponseRequest();

            var author = await _context.Authors.FindAsync(id);
            if(author != null)
            {
                _context.Authors.Remove(author);
                if (await _context.SaveChangesAsync() > 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = Notification.DELETE_SUCCESS;
                    response.Data = author;

                    return Ok(response);
                }
            }

            response.StatusCode = HttpStatusCode.BadRequest;
            response.Message = Notification.DELETE_FAIL;

            return BadRequest(response);
        }
    }
}
