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
using System.Net;

namespace CalendarWork.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly CalendarWorkDbContext _context;
        private readonly IMapper _mapper;

        public BookController(CalendarWorkDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ClaimRequirement(FunctionCode.BOOK, CommandCode.VIEW)]
        public async Task<IActionResult> Get()
        {
            ResponseRequest response = new ResponseRequest();

            var books = await _context.Books.ToListAsync();
            if (books.Count > 0)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Message = Notification.GET_SUCCESS;
                response.Data = books;

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
        [ClaimRequirement(FunctionCode.BOOK, CommandCode.CREATE)]
        public async Task<IActionResult> Post(BookDto obj)
        {
            ResponseRequest response = new ResponseRequest();

            try
            {
                var book = _mapper.Map<Book>(obj);
                _context.Books.Add(book);
                if (await _context.SaveChangesAsync() > 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = Notification.POST_SUCCESS;
                    response.Data = book;

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
            response.Message = Notification.POST_FAIL;

            return BadRequest(response);
        }

        [HttpPut]
        [ClaimRequirement(FunctionCode.BOOK, CommandCode.UPDATE)]
        public async Task<IActionResult> Put(BookDto obj)
        {
            ResponseRequest response = new ResponseRequest();

            try
            {
                var book = _mapper.Map<Book>(obj);
                _context.Books.Update(book);
                if (await _context.SaveChangesAsync() > 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = Notification.PUT_SUCCESS;
                    response.Data = book;

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
        [ClaimRequirement(FunctionCode.BOOK, CommandCode.DELETE)]
        public async Task<IActionResult> Delete(int id)
        {
            ResponseRequest response = new ResponseRequest();

            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                if (await _context.SaveChangesAsync() > 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = Notification.DELETE_SUCCESS;
                    response.Data = book;

                    return Ok(response);
                }
            }

            response.StatusCode = HttpStatusCode.BadRequest;
            response.Message = Notification.DELETE_FAIL;

            return BadRequest(response);
        }
    }
}
