using App.Contracts.BLL;
using App.Public.v1.Mappers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Base.Extensions;
using Errors = WebApp.Helpers.RestApiErrorHelpers;

namespace WebApp.ApiControllers;

/// <summary>
/// Measurement entity REST API operations
/// </summary>
[ApiVersion( "1.0" )]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class MeasurementController : ControllerBase
{
    private readonly IAppBll _bll;
    private readonly MeasurementMapper _mapper;

    /// <summary>
    /// Measurement Controller - takes in BLL and automapper interfaces
    /// </summary>
    /// <param name="bll">Business logic layer implementation</param>
    /// /// <param name="mapper">Measurement mapper</param>
    public MeasurementController(IAppBll bll, IMapper mapper)
    {
        _bll = bll;
        _mapper = new MeasurementMapper(mapper);
    }

    // GET: api/Measurement
    /// <summary>
    /// Get all measurements owned by user, requires authorisation
    /// </summary>
    /// <returns>Enumerable of measurements</returns>
    [Produces( "application/json" )]
    [Consumes( "application/json" )]
    [ProducesResponseType( typeof( IEnumerable<App.Public.DTO.v1.Measurement> ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Measurement>>> GetMeasurements()
    {
        return Ok((await _bll.Measurements
            .GetAllAsync(User.GetUserId(), true))
            .Select(x => _mapper.Map(x)));
    }

    // GET: api/Measurement/5
    /// <summary>
    /// Get measurement by id, requires authorisation and ownership by user
    /// </summary>
    /// <param name="id">Measurement GUID</param>
    /// <returns>Measurement if found, null if not found</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Measurement), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.Measurement>> GetMeasurement(Guid id)
    {
        var measurement = await _bll.Measurements.FirstOrDefaultAsync(id);

        if (measurement == null || measurement.AppUserId != User.GetUserId())
        {
            return NotFound();
        }

        return _mapper.Map(measurement)!;
    }

    // PUT: api/Measurement/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edit measurement, requires authorisation and ownership by user
    /// </summary>
    /// <param name="id">Measurement id to edit, must match request measurement id</param>
    /// <param name="measurement">Edited measurement, id must match request id</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMeasurement(Guid id, App.Public.DTO.v1.Measurement measurement)
    {
        if (id != measurement.Id || measurement.AppUserId != User.GetUserId())
        {
            return BadRequest();
        }
        
        if (ModelState.IsValid)
        {
            _bll.Measurements.Update(_mapper.Map(measurement)!);
            await _bll.SaveChangesAsync();
        }

        return NoContent();
    }

    // POST: api/Measurement
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Add measurement, requires authorisation and ownership by user
    /// </summary>
    /// <param name="measurement">Measurement to add</param>
    /// <returns>Added measurement</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Measurement), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.Measurement>> PostMeasurement(App.Public.DTO.v1.Measurement measurement)
    {
        measurement.Id = Guid.NewGuid();
        measurement.AppUserId = User.GetUserId();
        
        if (ModelState.IsValid)
        {
            _bll.Measurements.Add(_mapper.Map(measurement)!);
            await _bll.SaveChangesAsync();
        }

        return CreatedAtAction("GetMeasurement", new
        {
            id = measurement.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString()
        }, measurement);
    }

    // DELETE: api/Measurement/5
    /// <summary>
    /// Delete measurement with given GUID, requires authorisation and ownership by user
    /// </summary>
    /// <param name="id">Id of measurement to delete</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeasurement(Guid id)
    {
        var measurement = await _bll.Measurements.FirstOrDefaultAsync(id);
        if (measurement == null || measurement.AppUserId != User.GetUserId())
        {
            return NotFound();
        }

        _bll.Measurements.Remove(measurement);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool MeasurementExists(Guid id)
    {
        return _bll.Measurements.FirstOrDefault(id) != null;
    }
}