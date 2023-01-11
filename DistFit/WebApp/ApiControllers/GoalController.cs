using App.Contracts.BLL;
using App.Public.v1.Mappers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Errors = WebApp.Helpers.RestApiErrorHelpers;

namespace WebApp.ApiControllers;

/// <summary>
/// Goal entity REST API operations
/// </summary>
[ApiVersion( "1.0" )]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GoalController : ControllerBase
{
    private readonly IAppBll _bll;
    private readonly GoalMapper _mapper;

    /// <summary>
    /// Goal Controller - takes in BLL and automapper interfaces
    /// </summary>
    /// <param name="bll">Business logic layer implementation</param>
    /// /// <param name="mapper">Goal mapper</param>
    public GoalController(IAppBll bll, IMapper mapper)
    {
        _bll = bll;
        _mapper = new GoalMapper(mapper);
    }

    // GET: api/Goal
    /// <summary>
    /// Get all goals owned by user, requires authorisation
    /// </summary>
    /// <returns>Enumerable of goals</returns>
    [Produces( "application/json" )]
    [Consumes( "application/json" )]
    [ProducesResponseType( typeof( IEnumerable<App.Public.DTO.v1.Goal> ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Goal>>> GetGoals()
    {
        return Ok((await _bll.Goals
            .GetAllAsync(User.GetUserId(), true))
            .Select(x => _mapper.Map(x)));
    }

    // GET: api/Goal/5
    /// <summary>
    /// Get goal by id, requires authorisation and ownership by user
    /// </summary>
    /// <param name="id">Goal GUID</param>
    /// <returns>Goal if found, null if not found</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Goal), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.Goal>> GetGoal(Guid id)
    {
        var goal = await _bll.Goals.FirstOrDefaultAsync(id);

        if (goal == null || goal.AppUserId != User.GetUserId())
        {
            return NotFound();
        }

        return _mapper.Map(goal)!;
    }

    // PUT: api/Goal/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edit goal, requires authorisation and ownership by user
    /// </summary>
    /// <param name="id">Goal id to edit, must match request country id</param>
    /// <param name="goal">Edited goal, id must match request id</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGoal(Guid id, App.Public.DTO.v1.Goal goal)
    {
        if (id != goal.Id || goal.AppUserId != User.GetUserId())
        {
            return BadRequest();
        }
        
        if (ModelState.IsValid)
        {
            _bll.Goals.Update(_mapper.Map(goal)!);
            await _bll.SaveChangesAsync();
        }

        return NoContent();
    }

    // POST: api/Goal
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Add goal, requires authorisation and ownership by user
    /// </summary>
    /// <param name="goal">Goal to add</param>
    /// <returns>Added goal</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Goal), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.Goal>> PostGoal(App.Public.DTO.v1.Goal goal)
    {
        goal.Id = Guid.NewGuid();
        goal.AppUserId = User.GetUserId();
        
        if (ModelState.IsValid)
        {
            _bll.Goals.Add(_mapper.Map(goal)!);
            await _bll.SaveChangesAsync();
        }

        return CreatedAtAction("GetGoal", new
        {
            id = goal.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString()
        }, goal);
    }

    // DELETE: api/Goal/5
    /// <summary>
    /// Delete goal with given GUID, requires authorisation and ownership by user
    /// </summary>
    /// <param name="id">Id of goal to delete</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGoal(Guid id)
    {
        var goal = await _bll.Goals.FirstOrDefaultAsync(id);
        if (goal == null || goal.AppUserId != User.GetUserId())
        {
            return NotFound();
        }

        _bll.Goals.Remove(goal);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool GoalExists(Guid id)
    {
        return _bll.Goals.FirstOrDefault(id) != null;
    }
}