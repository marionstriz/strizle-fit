using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using App.Public.v1.Mappers;
using AutoMapper;
using Base.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

/// <summary>
/// Unit entity REST API operations
/// </summary>
[ApiVersion( "1.0" )]
[ApiVersion( "2.0" )]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UnitController : ControllerBase
{
    private readonly ILogger<UnitController> _logger;
    private readonly IAppBll _bll;
    private readonly UnitMapper _mapper;
    
    /// <summary>
    /// Unit Controller - takes in BLL and automapper interfaces
    /// </summary>
    /// <param name="bll">Business logic layer implementation</param>
    /// <param name="mapper">Unit mapper</param>
    /// <param name="logger">Unit Controller logger</param>
    public UnitController(IAppBll bll, IMapper mapper, ILogger<UnitController> logger)
    {
        _bll = bll;
        _logger = logger;
        _mapper = new UnitMapper(mapper);
    }

    // GET: api/Unit
    /// <summary>
    /// Get all units available in application
    /// </summary>
    /// <returns>Enumerable of units</returns>
    [Produces( "application/json" )]
    [Consumes( "application/json" )]
    [ProducesResponseType( typeof( IEnumerable<App.Public.DTO.v1.Unit> ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Unit>>> GetUnits()
    {
        return Ok((await _bll.Units.GetAllAsync()).Select(x => _mapper.Map(x)));
    }

    // GET: api/Unit/5
    /// <summary>
    /// Get unit by id
    /// </summary>
    /// <param name="id">Unit GUID</param>
    /// <returns>Unit if found, null if not found</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Unit), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.Unit>> GetUnit(Guid id)
    {
        var unit = await _bll.Units.FirstOrDefaultAsync(id);

        if (unit == null)
        {
            return NotFound();
        }

        return _mapper.Map(unit)!;
    }

    // PUT: api/Unit/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edit unit, requires authorisation
    /// </summary>
    /// <param name="id">Unit id to edit, must match request unit id</param>
    /// <param name="unit">Edited unit, id must match request id</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUnit(Guid id, App.Public.DTO.v1.Unit unit)
    {
        if (id != unit.Id) return BadRequest();

        var bllUnit = await _bll.Units.FirstOrDefaultAsync(id);
        if (bllUnit == null) return NotFound();

        var culture = LangStr.SupportedCultureOrDefault(
            Thread.CurrentThread.CurrentUICulture.Name);
            
        bllUnit.Name.SetTranslation(unit.Name, culture);
        bllUnit.Symbol.SetTranslation(unit.Symbol, culture);

        if (ModelState.IsValid)
        {
            _bll.Units.Update(bllUnit);
            await _bll.SaveChangesAsync();
        }

        return NoContent();
    }

    // POST: api/Unit
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Add unit, requires authorisation
    /// </summary>
    /// <param name="unit">Unit to add</param>
    /// <returns>Added unit</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Unit), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.Unit>> PostUnit(App.Public.DTO.v1.Unit unit)
    {
        var culture = LangStr.SupportedCultureOrDefault(
            Thread.CurrentThread.CurrentUICulture.Name);
        
        unit.Id = Guid.NewGuid();
        
        if (ModelState.IsValid)
        {
            var bllUnit = _mapper.Map(unit, culture);
            _bll.Units.Add(bllUnit!);
            await _bll.SaveChangesAsync();
        }

        return CreatedAtAction(
            "GetUnit", 
            new
            {
                id = unit.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, 
            unit);
    }

    // DELETE: api/Unit/5
    /// <summary>
    /// Delete unit with given GUID, requires authorisation
    /// </summary>
    /// <param name="id">Id of unit to delete</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUnit(Guid id)
    {
        var unit = await _bll.Units.FirstOrDefaultAsync(id);
        if (unit == null)
        {
            return NotFound();
        }

        _bll.Units.Remove(unit);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool UnitExists(Guid id)
    {
        return _bll.Units.FirstOrDefault(id) == null;
    }
}
