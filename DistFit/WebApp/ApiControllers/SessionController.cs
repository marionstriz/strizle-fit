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
public class SessionController : ControllerBase
{
    private readonly IAppBll _bll;
    private readonly SessionMapper _mapper;
    
    /// <summary>
    /// Session Controller - takes in BLL and automapper interfaces
    /// </summary>
    /// <param name="bll">Business logic layer implementation</param>
    /// <param name="mapper">Session mapper</param>
    public SessionController(IAppBll bll, IMapper mapper)
    {
        _bll = bll;
        _mapper = new SessionMapper(mapper);
    }

    // GET: api/Session
    /// <summary>
    /// Get all sessions available in application, requires authorisation
    /// </summary>
    /// <returns>Enumerable of sessions</returns>
    [Produces( "application/json" )]
    [Consumes( "application/json" )]
    [ProducesResponseType( typeof( IEnumerable<App.Public.DTO.v1.Session> ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Session>>> GetSessions()
    {
        return Ok((await _bll.Sessions.GetAllAsync()).Select(x => _mapper.Map(x)));
    }

    // GET: api/Session/5
    /// <summary>
    /// Get session by id, requires authorisation
    /// </summary>
    /// <param name="id">Session GUID</param>
    /// <returns>Session if found, null if not found</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Session), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.Session>> GetSession(Guid id)
    {
        var session = await _bll.Sessions.FirstOrDefaultAsync(id);

        if (session == null)
        {
            return NotFound();
        }

        return _mapper.Map(session)!;
    }

    // PUT: api/Session/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edit session, requires authorisation
    /// </summary>
    /// <param name="id">Session id to edit, must match request session id</param>
    /// <param name="session">Edited session, id must match request id</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSession(Guid id, App.Public.DTO.v1.Session session)
    {
        if (id != session.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            _bll.Sessions.Update(_mapper.Map(session)!);
            await _bll.SaveChangesAsync();
        }

        return NoContent();
    }

    // POST: api/Session
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Add session, requires authorisation
    /// </summary>
    /// <param name="session">Session to add</param>
    /// <returns>Added session</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Session), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.Session>> PostSession(App.Public.DTO.v1.Session session)
    {
        _bll.Sessions.Add(_mapper.Map(session)!);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetSession", new
        {
            id = session.Id, 
            version = HttpContext.GetRequestedApiVersion()!.ToString()
        }, session);
    }

    // DELETE: api/Session/5
    /// <summary>
    /// Delete session with given GUID, requires authorisation
    /// </summary>
    /// <param name="id">Id of session to delete</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSession(Guid id)
    {
        var session = await _bll.Sessions.FirstOrDefaultAsync(id);
        if (session == null)
        {
            return NotFound();
        }

        _bll.Sessions.Remove(session);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool SessionExists(Guid id)
    {
        return _bll.Sessions.FirstOrDefault(id) == null;
    }
}