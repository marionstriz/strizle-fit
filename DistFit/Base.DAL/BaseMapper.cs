using Base.Contracts.Base;

namespace Base.DAL;

public class BaseMapper<TOut, TIn> : IMapper<TOut, TIn>
{
    protected readonly AutoMapper.IMapper Mapper;

    public BaseMapper(AutoMapper.IMapper mapper)
    {
        Mapper = mapper;
    }
    
    public virtual TOut? Map(TIn? entity)
    {
        return Mapper.Map<TOut>(entity);
    }

    public virtual TIn? Map(TOut? entity)
    {
        return Mapper.Map<TIn>(entity);
    }
}