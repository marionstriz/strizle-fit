using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using App.Public.v1.Mappers;
using AutoMapper;
using Base.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

/// <summary>
/// Measurement type entity REST API operations
/// </summary>
[ApiVersion( "1.0" )]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MeasurementTypeController : ControllerBase
{
    private readonly IAppBll _bll;
    private readonly MeasurementTypeMapper _mapper;

    /// <summary>
    /// Measurement type Controller - takes in BLL and automapper interfaces
    /// </summary>
    /// <param name="bll">Business logic layer implementation</param>
    /// <param name="mapper">Measurement type mapper</param>
    public MeasurementTypeController(IAppBll bll, IMapper mapper)
    {
        _bll = bll;
        _mapper = new MeasurementTypeMapper(mapper);
    }

    // GET: api/MeasurementType
    /// <summary>
    /// Get all measurement types available in application, enables authentication
    /// </summary>
    /// <returns>Enumerable of measurement types</returns>
    [Produces( "application/json" )]
    [Consumes( "application/json" )]
    [ProducesResponseType( typeof( IEnumerable<App.Public.DTO.v1.MeasurementType> ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.MeasurementType>>> GetMeasurementTypes()
    {
        return Ok((await _bll.MeasurementTypes.GetAllAsync()).Select(x => _mapper.Map(x)));
    }

    // GET: api/MeasurementType/5
    /// <summary>
    /// Get measurement type by id
    /// </summary>
    /// <param name="id">Measurement type GUID</param>
    /// <returns>Measurement type if found, null if not found</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.MeasurementType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.MeasurementType>> GetMeasurementType(Guid id)
    {
        var measurementType = await _bll.MeasurementTypes.FirstOrDefaultAsync(id);

        if (measurementType == null)
        {
            return NotFound();
        }

        return _mapper.Map(measurementType)!;
    }

    // PUT: api/MeasurementType/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edit exercise type name, requires authorisation
    /// </summary>
    /// <param name="id">Exercise type id to edit, must match request measurement type id</param>
    /// <param name="exerciseType">Edited exercise type, id must match request id</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMeasurementType(Guid id, App.Public.DTO.v1.MeasurementType exerciseType)
    {
        if (id != exerciseType.Id) return BadRequest();

        var exerciseTypeFromBll = await _bll.MeasurementTypes.FirstOrDefaultAsync(id);
        if (exerciseTypeFromBll == null) return NotFound();

        var culture = LangStr.SupportedCultureOrDefault(
            Thread.CurrentThread.CurrentUICulture.Name);
            
        exerciseTypeFromBll.Name.SetTranslation(exerciseType.Name, culture);

        if (ModelState.IsValid)
        {
            _bll.MeasurementTypes.Update(exerciseTypeFromBll);
            await _bll.SaveChangesAsync();
        }

        return NoContent();
    }

    // POST: api/MeasurementType
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Add measurement type, requires authorisation
    /// </summary>
    /// <param name="measurementType">Measurement type to add</param>
    /// <returns>Added measurement type</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.MeasurementType), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.MeasurementType>> PostMeasurementType(App.Public.DTO.v1.MeasurementType measurementType)
    { 
        var culture = LangStr.SupportedCultureOrDefault(
            Thread.CurrentThread.CurrentUICulture.Name);
        
        measurementType.Id = Guid.NewGuid();

        if (!ModelState.IsValid) return BadRequest();
        
        var bllMeasurementType = _mapper.Map(measurementType, culture);
        _bll.MeasurementTypes.Add(bllMeasurementType!);
        await _bll.SaveChangesAsync();

        return CreatedAtAction(
        "GetMeasurementType", 
        new
        {
            id = measurementType.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString()
        }, 
        measurementType);
    }

    // DELETE: api/MeasurementType/5
    /// <summary>
    /// Delete measurement type with given GUID, requires authorisation
    /// </summary>
    /// <param name="id">Id of measurement type to delete</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeasurementType(Guid id)
    {
        var measurementType = await _bll.MeasurementTypes.FirstOrDefaultAsync(id);
        if (measurementType == null)
        {
            return NotFound();
        }

        _bll.MeasurementTypes.Remove(measurementType);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool MeasurementTypeExists(Guid id)
    {
        return _bll.MeasurementTypes.FirstOrDefault(id) == null;
    }
}