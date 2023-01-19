using App.BLL;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.DAL.EF;
using AutoMapper;
using Base.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using AutomapperConfig = App.DAL.EF.AutomapperConfig;

namespace Tests.WebApp.Services;

public class UnitTestProgramService
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IProgramService _programService;
    private readonly IAppBll _bll;
    private readonly AppDbContext _dbContext;
    private const string DefaultCulture = LangStr.DefaultCulture;

    public UnitTestProgramService(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _dbContext = new AppDbContext(optionsBuilder.Options);

        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        
        var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutomapperConfig()));
        var mapper = mockMapper.CreateMapper();
        
        var uow = new AppUow(_dbContext, mapper);
        
        mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new App.BLL.AutomapperConfig()));
        mapper = mockMapper.CreateMapper();

        _bll = new AppBll(uow, mapper);
        
        _programService = _bll.Programs;
    }

    [Fact]
    public void AddAction_ReturnsProgramWithValuesEqualToAddedProgram()
    {
        var program = new App.BLL.DTO.Program
        {
            Name = new LangStr("TestProgram", DefaultCulture),
            IsPublic = true
        };

        var addedProgram = _programService.Add(program);

        Assert.Equal(addedProgram.Name[DefaultCulture], program.Name[DefaultCulture]);
        Assert.Equal(addedProgram.IsPublic, program.IsPublic);
    }

    [Fact]
    public void GetAll_GetsCorrectAmountOfEntities()
    {
        var initialProgramCount = _programService.GetAll().Count();
        var program = new App.BLL.DTO.Program
        {
            Name = new LangStr("TestProgram", DefaultCulture),
            IsPublic = true
        };

        _programService.Add(program);
        _bll.SaveChanges();

        var postAddingProgramCount = _programService.GetAll().Count();
        
        Assert.Equal(initialProgramCount + 1, postAddingProgramCount);
    }
    
    [Fact]
    public async void GetAllAsync_GetsCorrectAmountOfEntities()
    {
        var initialProgramCount = (await _programService.GetAllAsync()).Count();
        var program = new App.BLL.DTO.Program
        {
            Name = new LangStr("TestProgram", DefaultCulture),
            IsPublic = true
        };

        _programService.Add(program);
        await _bll.SaveChangesAsync();

        var postAddingProgramCount = (await _programService.GetAllAsync()).Count();
        
        Assert.Equal(initialProgramCount + 1, postAddingProgramCount);
    }
    
    [Fact]
    public async void Update_ReturnsUpdatedEntity()
    {
        var updatedProgram = new App.BLL.DTO.Program
        {
            Name = new LangStr("UpdatedProgram", DefaultCulture),
            IsPublic = false
        };

        var returnedProgram = _programService.Update(updatedProgram);

        Assert.Equal(returnedProgram.Name, updatedProgram.Name);
        Assert.Equal(returnedProgram.IsPublic, updatedProgram.IsPublic);
    }
    
    [Fact]
    public async void Update_UpdatesCorrectEntity()
    {
        var programId = Guid.NewGuid();
        var program = new App.BLL.DTO.Program
        {
            Id = programId,
            Name = new LangStr("TestProgram", DefaultCulture),
            IsPublic = true
        };
        _programService.Add(program);
        var initialIsPublic = program.IsPublic;
        await _bll.SaveChangesAsync();

        program.IsPublic = !program.IsPublic;
        
        await _bll.SaveChangesAsync();

        Assert.NotNull(program);
        Assert.NotEqual(initialIsPublic, program.IsPublic);
    }
    
    [Fact]
    public void RemoveById_ReturnsRemovedEntity()
    {
        var programGuid = Guid.NewGuid();
        var program = new App.BLL.DTO.Program
        {
            Id = programGuid,
            Name = new LangStr("TestProgram", DefaultCulture),
            IsPublic = true
        };
        _programService.Add(program);
        _bll.SaveChanges();
        _dbContext.ChangeTracker.Clear();
        
        var returnedProgram = _programService.Remove(programGuid);
        
        Assert.Equal(returnedProgram.Name, program.Name);
        Assert.Equal(returnedProgram.IsPublic, program.IsPublic);
    }
    
    [Fact]
    public void RemoveByEntity_RemovesAndReturnsEntity()
    {
        var programId = Guid.NewGuid();
        var program = new App.BLL.DTO.Program
        {
            Id = programId,
            Name = new LangStr("TestProgram", DefaultCulture),
            IsPublic = true
        };
        _programService.Add(program);
        _bll.SaveChanges();
        _dbContext.ChangeTracker.Clear();

        var existsAfterAdd = _programService.Exists(programId);

        var returnedProgram = _programService.Remove(program);
        _bll.SaveChanges();
        _dbContext.ChangeTracker.Clear();
        
        var existsAfterRemove = _programService.Exists(programId);

        Assert.True(existsAfterAdd);
        Assert.False(existsAfterRemove);
        Assert.Equal(program.IsPublic, returnedProgram.IsPublic);
        Assert.Equal(program.Name, returnedProgram.Name);
    }
    
    [Fact]
    public async void RemoveAsync_RemovesAndReturnsEntity()
    {
        var programId = Guid.NewGuid();
        var program = new App.BLL.DTO.Program
        {
            Id = programId,
            Name = new LangStr("TestProgram", DefaultCulture),
            IsPublic = true
        };
        _programService.Add(program);
        await _bll.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();

        var existsAfterAdd = await _programService.ExistsAsync(programId);

        var returnedProgram = await _programService.RemoveAsync(programId);
        await _bll.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();
        
        var existsAfterRemove = await _programService.ExistsAsync(programId);

        Assert.True(existsAfterAdd);
        Assert.False(existsAfterRemove);
        Assert.Equal(program.IsPublic, returnedProgram.IsPublic);
        Assert.Equal(program.Name, returnedProgram.Name);
    }
    
    [Fact]
    public async void RemoveWithWrongGuid_ThrowsNullReference()
    {
        var programGuid = Guid.NewGuid();
        Assert.Throws<NullReferenceException>(() => _programService.Remove(programGuid));
    }
    
    [Fact]
    public void FirstOrDefault_ReturnsNullWhenNotFound()
    {
        var nullProgram = _programService.FirstOrDefault(Guid.NewGuid());
        Assert.Null(nullProgram);
    }
    
    [Fact]
    public async void FirstOrDefaultAsync_ReturnsNullWhenNotFound()
    {
        var nullProgram = await _programService.FirstOrDefaultAsync(Guid.NewGuid());
        Assert.Null(nullProgram);
    }
    
    [Fact]
    public void FirstOrDefault_ReturnsCorrectEntity()
    {
        var programId = Guid.NewGuid();
        var program = new App.BLL.DTO.Program
        {
            Id = programId,
            Name = new LangStr("TestProgram", DefaultCulture),
            IsPublic = true
        };
        _programService.Add(program);
        _bll.SaveChanges();
        _dbContext.ChangeTracker.Clear();
        
        var foundProgram = _programService.FirstOrDefault(programId);
        
        Assert.NotNull(foundProgram);
        Assert.Equal(program.Name, foundProgram!.Name);
        Assert.Equal(program.IsPublic, foundProgram!.IsPublic);
    }
    
    [Fact]
    public async void FirstOrDefaultAsync_ReturnsCorrectEntity()
    {
        var programId = Guid.NewGuid();
        var program = new App.BLL.DTO.Program
        {
            Id = programId,
            Name = new LangStr("TestProgram", DefaultCulture),
            IsPublic = true
        };
        _programService.Add(program);
        await _bll.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();
        
        var foundProgram = await _programService.FirstOrDefaultAsync(programId);
        
        Assert.NotNull(foundProgram);
        Assert.Equal(program.Name, foundProgram!.Name);
        Assert.Equal(program.IsPublic, foundProgram.IsPublic);
    }

    [Fact]
    public void AddingProgramWithSave_ReturnsAddedProgram()
    {
        var programId = Guid.NewGuid();
        var program = new App.BLL.DTO.Program
        {
            Id = programId,
            Name = new LangStr("TestProgram", DefaultCulture),
            IsPublic = false
        };

        var addedProgram = _programService.AddProgramWithSave(program, Guid.NewGuid());
        
        Assert.NotNull(addedProgram);
        Assert.Equal(program.Id, addedProgram.Id);
        Assert.Equal(program.Name, addedProgram.Name);
        Assert.Equal(program.IsPublic, addedProgram.IsPublic);
    }
}