using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using App.Public.v1.Mappers;
using AutoMapper;

namespace WebApp.ApiControllers;

/// <summary>
/// Set entry entity REST API operations
/// </summary>
[ApiVersion( "1.0" )]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class SetEntryController : ControllerBase
{
    private readonly IAppBll _bll;
    private readonly SetEntryMapper _mapper;
    
    /// <summary>
    /// Set entry Controller - takes in BLL and automapper interfaces
    /// </summary>
    /// <param name="bll">Business logic layer implementation</param>
    /// <param name="mapper">Set entry mapper</param>
    public SetEntryController(IAppBll bll, IMapper mapper)
    {
        _bll = bll;
        _mapper = new SetEntryMapper(mapper);
    }

    // GET: api/SetEntry
    /// <summary>
    /// Get all set entries available in application, requires authorisation
    /// </summary>
    /// <returns>Enumerable of set entries</returns>
    [Produces( "application/json" )]
    [Consumes( "application/json" )]
    [ProducesResponseType( typeof( IEnumerable<App.Public.DTO.v1.SetEntry> ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.SetEntry>>> GetSetEntry()
    {
        return Ok((await _bll.SetEntries.GetAllAsync()).Select(x => _mapper.Map(x)));
    }

    // GET: api/SetEntry/5
    /// <summary>
    /// Get set entry by id, requires authorisation
    /// </summary>
    /// <param name="id">Set entry GUID</param>
    /// <returns>Set entry if found, null if not found</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.SetEntry), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.SetEntry>> GetSetEntry(Guid id)
    {
        var setEntry = await _bll.SetEntries.FirstOrDefaultAsync(id);

        if (setEntry == null)
        {
            return NotFound();
        }

        return _mapper.Map(setEntry)!;
    }

    // PUT: api/SetEntry/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edit set entry, requires authorisation
    /// </summary>
    /// <param name="id">Set entry id to edit, must match request set entry id</param>
    /// <param name="setEntry">Edited set entry, id must match request id</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSetEntry(Guid id, App.Public.DTO.v1.SetEntry setEntry)
    {
        if (id != setEntry.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            _bll.SetEntries.Update(_mapper.Map(setEntry)!);
            await _bll.SaveChangesAsync();
        }

        return NoContent();
    }

    // POST: api/SetEntry
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Add set entry, requires authorisation
    /// </summary>
    /// <param name="setEntry">Set entry to add</param>
    /// <returns>Added set entry</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.SetEntry), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.SetEntry>> PostSetEntry(App.Public.DTO.v1.SetEntry setEntry)
    {
        _bll.SetEntries.Add(_mapper.Map(setEntry)!);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetSetEntry", new
        {
            id = setEntry.Id, 
            version = HttpContext.GetRequestedApiVersion()!.ToString()
        }, setEntry);
    }

    // DELETE: api/SetEntry/5
    /// <summary>
    /// Delete set entry with given GUID, requires authorisation
    /// </summary>
    /// <param name="id">Id of set entry to delete</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSetEntry(Guid id)
    {
        var setEntry = await _bll.SetEntries.FirstOrDefaultAsync(id);
        if (setEntry == null)
        {
            return NotFound();
        }

        _bll.SetEntries.Remove(setEntry);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool SetEntryExists(Guid id)
    {
        return _bll.SetEntries.FirstOrDefault(id) == null;
    }
}