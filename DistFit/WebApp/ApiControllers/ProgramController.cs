using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using App.Public.v1.Mappers;
using AutoMapper;
using Base.Domain;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

/// <summary>
/// Program entity REST API operations
/// </summary>
[ApiVersion( "1.0" )]
[ApiVersion( "2.0" )]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProgramController : ControllerBase
{
    private readonly ILogger<ProgramController> _logger;
    private readonly IAppBll _bll;
    private readonly ProgramMapper _mapper;
    
    /// <summary>
    /// Program Controller - takes in BLL and automapper interfaces
    /// </summary>
    /// <param name="bll">Business logic layer implementation</param>
    /// <param name="mapper">Program mapper</param>
    /// <param name="logger">Program Controller logger</param>
    public ProgramController(IAppBll bll, IMapper mapper, ILogger<ProgramController> logger)
    {
        _bll = bll;
        _logger = logger;
        _mapper = new ProgramMapper(mapper);
    }

    // GET: api/Program
    /// <summary>
    /// Get all programs available in application, requires authorisation
    /// </summary>
    /// <returns>Enumerable of programs</returns>
    [Produces( "application/json" )]
    [Consumes( "application/json" )]
    [ProducesResponseType( typeof( IEnumerable<App.Public.DTO.v1.Program> ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Program>>> GetPrograms()
    {
        return Ok((await _bll.Programs.GetAllAsync()).Select(x => _mapper.Map(x)));
    }

    // GET: api/Program/5
    /// <summary>
    /// Get program by id, requires authorisation
    /// </summary>
    /// <param name="id">Program GUID</param>
    /// <returns>Program if found, null if not found</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Program), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.Program>> GetProgram(Guid id)
    {
        var program = await _bll.Programs.FirstOrDefaultAsync(id);

        if (program == null)
        {
            return NotFound();
        }

        return _mapper.Map(program)!;
    }

    // PUT: api/Program/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edit program, requires authorisation
    /// </summary>
    /// <param name="id">Program id to edit, must match request program id</param>
    /// <param name="program">Edited program, id must match request id</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProgram(Guid id, App.Public.DTO.v1.Program program)
    {
        if (id != program.Id) return BadRequest();

        var bllProgram = await _bll.Programs.FirstOrDefaultAsync(id);
        if (bllProgram == null) return NotFound();

        var culture = LangStr.SupportedCultureOrDefault(
            Thread.CurrentThread.CurrentUICulture.Name);
            
        bllProgram.Name.SetTranslation(program.Name, culture);
        if (program.Description != null) bllProgram.Description!.SetTranslation(program.Description, culture);
        bllProgram.Duration = program.Duration;
        bllProgram.DurationUnitId = program.DurationUnitId;

        if (ModelState.IsValid)
        {
            _bll.Programs.Update(bllProgram);
            await _bll.SaveChangesAsync();
        }

        return NoContent();
    }

    // POST: api/Program
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Add program, requires authorisation
    /// </summary>
    /// <param name="program">Program to add</param>
    /// <returns>Added program</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Program), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.Program>> PostProgram(App.Public.DTO.v1.Program program)
    {
        var culture = LangStr.SupportedCultureOrDefault(
            Thread.CurrentThread.CurrentUICulture.Name);
        
        program.Id = Guid.NewGuid();
        
        if (ModelState.IsValid)
        {
            var bllProgram = _mapper.Map(program, culture);
            _bll.Programs.Add(bllProgram!);
            await _bll.SaveChangesAsync();
        }

        return CreatedAtAction(
            "GetProgram", 
            new
            {
                id = program.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, 
            program);
    }

    // DELETE: api/Program/5
    /// <summary>
    /// Delete program with given GUID, requires authorisation
    /// </summary>
    /// <param name="id">Id of program to delete</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProgram(Guid id)
    {
        var program = await _bll.Programs.FirstOrDefaultAsync(id);
        if (program == null)
        {
            return NotFound();
        }

        _bll.Programs.Remove(program);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool ProgramExists(Guid id)
    {
        return _bll.Programs.FirstOrDefault(id) == null;
    }
}
