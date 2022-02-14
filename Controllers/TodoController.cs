using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyTodoApp.Models;
using MyTodoApp.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyTodoApp.ViewModels;
using System;

namespace MyTodoApp.Controllers
{
  [ApiController]
  [Route(template: "v1")]
  public class TodoController : ControllerBase
  {
    [HttpGet]
    [Route(template: "todos")]
    public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
    {
      var todos = await context
          .Todos
          .AsNoTracking()
          .ToListAsync();

      return Ok(todos);
    }

    [HttpGet]
    [Route(template: "todos/{id}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromServices] AppDbContext context,
        [FromRoute] int id)
    {
      var todo = await context
          .Todos
          .AsNoTracking()
          .FirstOrDefaultAsync(x => x.Id == id);
      return todo == null ? NotFound() : Ok(todo);
    }

    [HttpPost(template: "todos")]
    public async Task<IActionResult> PostAsync(
        [FromServices] AppDbContext context,
        [FromBody] CreateTodoViewModel model
    )
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      Todo todo = new Todo
      {
        Date = DateTime.Now,
        Done = false,
        Title = model.Title
      };

      try
      {
        await context.Todos.AddAsync(todo);
        await context.SaveChangesAsync();
        return Created(uri: $"v1/todos/{todo.Id}", todo);
      }
      catch (Exception e)
      {
        /* it's not suposed to be a BadRequest but balta doesn't 
        remember what is the right one. It should be a Server error */

        return BadRequest();
      }
    }

    [HttpPut(template: "todos/{id}")]
    public async Task<IActionResult> PutAsync(
        [FromServices] AppDbContext context,
        [FromBody] CreateTodoViewModel model,
        [FromRoute] int id
    )
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      Todo todo = await context
      .Todos
      .FirstOrDefaultAsync(x => x.Id == id);

      if (todo == null)
      {
        return NotFound();
      }

      try
      {
        todo.Title = model.Title;

        context.Todos.Update(todo);
        await context.SaveChangesAsync();

        return Ok(todo);
      }
      catch (Exception e)
      {
        /* it's not suposed to be a BadRequest but balta doesn't 
        remember what is the right one. It should be a Server error */

        return BadRequest();
      }

    }

    [HttpDelete(template: "todos/{id}")]
    public async Task<IActionResult> DeleteAsync(
        [FromServices] AppDbContext context,
        [FromRoute] int id
    )
    {
      var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

      try
      {
        context.Todos.Remove(todo);
        await context.SaveChangesAsync();

        return Ok();
      }
      catch (Exception e)
      {
        /* it's not suposed to be a BadRequest but balta doesn't 
        remember what is the right one. It should be a Server error */

        return BadRequest();
      }
    }
  }

}