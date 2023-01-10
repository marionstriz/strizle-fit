using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using App.Public.v1.Mappers;
using AutoMapper;

namespace WebApp.ApiControllers;

/// <summary>
/// Performance entity REST API operations
/// </summary>
[ApiVersion( "1.0" )]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PerformanceController : ControllerBase
{
    private readonly IAppBll _bll;
    private readonly PerformanceMapper _mapper;
    
    /// <summary>
    /// Performance Controller - takes in BLL and automapper interfaces
    /// </summary>
    /// <param name="bll">Business logic layer implementation</param>
    /// <param name="mapper">Performance mapper</param>
    public PerformanceController(IAppBll bll, IMapper mapper)
    {
        _bll = bll;
        _mapper = new PerformanceMapper(mapper);
    }

    // GET: api/Performance
    /// <summary>
    /// Get all performances available in application, requires authorisation
    /// </summary>
    /// <returns>Enumerable of performances</returns>
    [Produces( "application/json" )]
    [Consumes( "application/json" )]
    [ProducesResponseType( typeof( IEnumerable<App.Public.DTO.v1.Performance> ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Performance>>> GetPerformances()
    {
        return Ok((await _bll.Performances.GetAllAsync()).Select(x => _mapper.Map(x)));
    }

    // GET: api/Performance/5
    /// <summary>
    /// Get performance by id, requires authorisation
    /// </summary>
    /// <param name="id">Performance GUID</param>
    /// <returns>Performance if found, null if not found</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Performance), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.Performance>> GetPerformance(Guid id)
    {
        var performance = await _bll.Performances.FirstOrDefaultAsync(id);

        if (performance == null)
        {
            return NotFound();
        }

        return _mapper.Map(performance)!;
    }

    // PUT: api/Performance/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edit performance, requires authorisation
    /// </summary>
    /// <param name="id">Performance id to edit, must match request performance id</param>
    /// <param name="performance">Edited performance, id must match request id</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerformance(Guid id, App.Public.DTO.v1.Performance performance)
    {
        if (id != performance.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            _bll.Performances.Update(_mapper.Map(performance)!);
            await _bll.SaveChangesAsync();
        }

        return NoContent();
    }

    // POST: api/Performance
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Add performance, requires authorisation
    /// </summary>
    /// <param name="performance">Performance to add</param>
    /// <returns>Added performance</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Performance), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.Performance>> PostPerformance(App.Public.DTO.v1.Performance performance)
    {
        _bll.Performances.Add(_mapper.Map(performance)!);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetPerformance", new
        {
            id = performance.Id, 
            version = HttpContext.GetRequestedApiVersion()!.ToString()
        }, performance);
    }

    // DELETE: api/Performance/5
    /// <summary>
    /// Delete performance with given GUID, requires authorisation
    /// </summary>
    /// <param name="id">Id of performance to delete</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerformance(Guid id)
    {
        var performance = await _bll.Performances.FirstOrDefaultAsync(id);
        if (performance == null)
        {
            return NotFound();
        }

        _bll.Performances.Remove(performance);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool PerformanceExists(Guid id)
    {
        return _bll.Performances.FirstOrDefault(id) == null;
    }
}