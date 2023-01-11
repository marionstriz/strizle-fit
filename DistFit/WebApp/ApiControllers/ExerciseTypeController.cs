using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using App.Public.v1.Mappers;
using AutoMapper;
using Base.Domain;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

/// <summary>
/// Exercise type entity REST API operations
/// </summary>
[ApiVersion( "1.0" )]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class ExerciseTypeController : ControllerBase
{
    private readonly IAppBll _bll;
    private readonly ExerciseTypeMapper _mapper;

    /// <summary>
    /// Exercise type Controller - takes in BLL and automapper interfaces
    /// </summary>
    /// <param name="bll">Business logic layer implementation</param>
    /// <param name="mapper">Exercise type mapper</param>
    public ExerciseTypeController(IAppBll bll, IMapper mapper)
    {
        _bll = bll;
        _mapper = new ExerciseTypeMapper(mapper);
    }

    // GET: api/ExerciseType
    /// <summary>
    /// Get all exercise types available in application
    /// </summary>
    /// <returns>Enumerable of exercise types</returns>
    [Produces( "application/json" )]
    [Consumes( "application/json" )]
    [ProducesResponseType( typeof( IEnumerable<App.Public.DTO.v1.ExerciseType> ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.ExerciseType>>> GetExerciseTypes()
    {
        return Ok((await _bll.ExerciseTypes.GetAllAsync()).Select(x => _mapper.Map(x)));
    }

    // GET: api/ExerciseType/5
    /// <summary>
    /// Get exercise type by id
    /// </summary>
    /// <param name="id">Exercise type GUID</param>
    /// <returns>Exercise type if found, null if not found</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.ExerciseType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.ExerciseType>> GetExerciseType(Guid id)
    {
        var exerciseType = await _bll.ExerciseTypes.FirstOrDefaultAsync(id);

        if (exerciseType == null)
        {
            return NotFound();
        }

        return _mapper.Map(exerciseType)!;
    }

    // PUT: api/ExerciseType/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edit exercise type name, requires authorisation
    /// </summary>
    /// <param name="id">Exercise type id to edit, must match request exercise type id</param>
    /// <param name="exerciseType">Edited exercise type, id must match request id</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutExerciseType(Guid id, App.Public.DTO.v1.ExerciseType exerciseType)
    {
        if (id != exerciseType.Id) return BadRequest();

        var exerciseTypeFromBll = await _bll.ExerciseTypes.FirstOrDefaultAsync(id);
        if (exerciseTypeFromBll == null) return NotFound();

        var culture = LangStr.SupportedCultureOrDefault(
            Thread.CurrentThread.CurrentUICulture.Name);
            
        exerciseTypeFromBll.Name.SetTranslation(exerciseType.Name, culture);

        if (ModelState.IsValid)
        {
            _bll.ExerciseTypes.Update(exerciseTypeFromBll);
            await _bll.SaveChangesAsync();
        }

        return NoContent();
    }

    // POST: api/ExerciseType
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Add exercise type, requires authorisation
    /// </summary>
    /// <param name="exerciseType">Exercise type to add</param>
    /// <returns>Added exercise type</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.ExerciseType), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.ExerciseType>> PostExerciseType(App.Public.DTO.v1.ExerciseType exerciseType)
    {
        var culture = LangStr.SupportedCultureOrDefault(
            Thread.CurrentThread.CurrentUICulture.Name);
        
        exerciseType.Id = Guid.NewGuid();
        
        if (ModelState.IsValid)
        {
            var bllExerciseType = _mapper.Map(exerciseType, culture);
            _bll.ExerciseTypes.Add(bllExerciseType!);
            await _bll.SaveChangesAsync();
        }

        return CreatedAtAction(
            "GetExerciseType", 
            new
            {
                id = exerciseType.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, 
            exerciseType);
    }

    // DELETE: api/ExerciseType/5
    /// <summary>
    /// Delete exercise type with given GUID, requires authorisation
    /// </summary>
    /// <param name="id">Id of exercise type to delete</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExerciseType(Guid id)
    {
        var exerciseType = await _bll.ExerciseTypes.FirstOrDefaultAsync(id);
        if (exerciseType == null)
        {
            return NotFound();
        }

        _bll.ExerciseTypes.Remove(exerciseType);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool ExerciseTypeExists(Guid id)
    {
        return _bll.ExerciseTypes.FirstOrDefault(id) == null;
    }
}