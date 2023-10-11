using System.Collections.Generic;
using System.Runtime.InteropServices;
using ApiRest;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private static List<TodoItem> todoItems = new List<TodoItem>
    {
        new TodoItem { Id = 1, Name = "Learn C#", IsComplete = false },
        new TodoItem { Id = 2, Name = "Build REST API", IsComplete = true },
    };

    [HttpGet]
    public ActionResult<IEnumerable<TodoItem>> Get()
    {
        return Ok(todoItems);
    }

    [HttpGet("{id}")]
    public ActionResult<TodoItem> GetById(long id)
    {
        var todoItem = todoItems.Find(item => item.Id == id);
        if (todoItem == null)
        {
            return NotFound();
        }
        return Ok(todoItem);
    }

    [HttpPost]
    public ActionResult<TodoItem> Post(TodoItem todoItem)
    {
        todoItem.Id = todoItems.Count + 1;
        todoItems.Add(todoItem);
        return CreatedAtAction(nameof(GetById), new { id = todoItem.Id }, todoItem);
    }

    [HttpPut("{id}")]
    public IActionResult Put(long id, TodoItem updatedTodoItem)
    {
        var existingTodoItem = todoItems.Find(item => item.Id == id);
        if (existingTodoItem == null)
        {
            return NotFound();
        }

        existingTodoItem.Name = updatedTodoItem.Name;
        existingTodoItem.IsComplete = updatedTodoItem.IsComplete;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var todoItem = todoItems.Find(item => item.Id == id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItems.Remove(todoItem);
        return NoContent();
    }
}
