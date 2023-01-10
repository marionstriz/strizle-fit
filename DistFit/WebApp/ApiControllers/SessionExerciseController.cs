using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using App.Public.v1.Mappers;
using AutoMapper;

namespace WebApp.ApiControllers;

/// <summary>
/// Session entity REST API operations
/// </summary>
[ApiVersion( "1.0" )]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class SessionExerciseController : ControllerBase
{
    private readonly IAppBll _bll;
    private readonly SessionExerciseMapper _mapper;
    
    /// <summary>
    /// Session exercise Controller - takes in BLL and automapper interfaces
    /// </summary>
    /// <param name="bll">Business logic layer implementation</param>
    /// <param name="mapper">Session exercise mapper</param>
    public SessionExerciseController(IAppBll bll, IMapper mapper)
    {
        _bll = bll;
        _mapper = new SessionExerciseMapper(mapper);
    }

    // GET: api/SessionExercise
    /// <summary>
    /// Get all session exercises available in application, requires authorisation
    /// </summary>
    /// <returns>Enumerable of session exercises</returns>
    [Produces( "application/json" )]
    [Consumes( "application/json" )]
    [ProducesResponseType( typeof( IEnumerable<App.Public.DTO.v1.SessionExercise> ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.SessionExercise>>> GetSessionExercises()
    {
        return Ok((await _bll.SessionExercises.GetAllAsync()).Select(x => _mapper.Map(x)));
    }

    // GET: api/SessionExercise/5
    /// <summary>
    /// Get session exercise by id, requires authorisation
    /// </summary>
    /// <param name="id">Session exercise GUID</param>
    /// <returns>Session exercise if found, null if not found</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.SessionExercise), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.SessionExercise>> GetSessionExercise(Guid id)
    {
        var sessionExercise = await _bll.SessionExercises.FirstOrDefaultAsync(id);

        if (sessionExercise == null)
        {
            return NotFound();
        }

        return _mapper.Map(sessionExercise)!;
    }

    // PUT: api/SessionExercise/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edit session exercise, requires authorisation
    /// </summary>
    /// <param name="id">Session exercise id to edit, must match request session exercise id</param>
    /// <param name="sessionExercise">Edited session exercise, id must match request id</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSessionExercise(Guid id, App.Public.DTO.v1.SessionExercise sessionExercise)
    {
        if (id != sessionExercise.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            _bll.SessionExercises.Update(_mapper.Map(sessionExercise)!);
            await _bll.SaveChangesAsync();
        }

        return NoContent();
    }

    // POST: api/SessionExercise
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Add session exercise, requires authorisation
    /// </summary>
    /// <param name="sessionExercise">Session exercise to add</param>
    /// <returns>Added session exercise</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.SessionExercise), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.SessionExercise>> PostPerformance(App.Public.DTO.v1.SessionExercise sessionExercise)
    {
        _bll.SessionExercises.Add(_mapper.Map(sessionExercise)!);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetSessionExercise", new
        {
            id = sessionExercise.Id, 
            version = HttpContext.GetRequestedApiVersion()!.ToString()
        }, sessionExercise);
    }

    // DELETE: api/SessionExercise/5
    /// <summary>
    /// Delete session exercise with given GUID, requires authorisation
    /// </summary>
    /// <param name="id">Id of session exercise to delete</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSessionExercise(Guid id)
    {
        var sessionExercise = await _bll.SessionExercises.FirstOrDefaultAsync(id);
        if (sessionExercise == null)
        {
            return NotFound();
        }

        _bll.SessionExercises.Remove(sessionExercise);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool SessionExerciseExists(Guid id)
    {
        return _bll.SessionExercises.FirstOrDefault(id) == null;
    }
}