using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Models;
using UserManagementApi.Services;
using UserManagementApi.Dtos;
using Microsoft.AspNetCore.Authorization;
namespace UserManagementApi.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController: ControllerBase {
    private readonly UserService _service;
    public UsersController(UserService service) {
        _service = service;
    }
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAll() => Ok(_service.GetAllUsers().Select(u => new UserDto {
        Id = u.Id,
        FirstName = u.FirstName,
        LastName = u.LastName,
        Email = u.Email
    }));
    [HttpGet("{id:int}")]
    public ActionResult<UserDto> GetById(int id) {
        var user = _service.GetUserById(id);
        return user is null ? NotFound(new {message = $"User with ID {id} not found."}) : Ok(new UserDto {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        });
    }
    [HttpPost]
    public ActionResult<UserDto> Create([FromBody] CreateUserDto dto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        try {
            var created = _service.AddUser(dto);
            return CreatedAtAction(nameof(GetById), new {id = created.Id}, new UserDto {
                Id = created.Id,
                FirstName = created.FirstName,
                LastName = created.LastName,
                Email = created.Email
            });
        } catch (InvalidOperationException ex) {
            return Conflict(new {message = ex.Message});
        }
    }
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] UpdateUserDto dto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        if (dto.Id != 0 && dto.Id != id) {
            return BadRequest(new {message = "Body Id does not match route Id."});
        }
        var success = _service.UpdateUser(id, dto);
        return success ? NoContent() : NotFound(new {message = $"User with ID {id} not found."});
    }
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) {
        var success = _service.DeleteUser(id);
        return success ? NoContent() : NotFound(new {message = $"User with ID {id} not found."});
    }
}