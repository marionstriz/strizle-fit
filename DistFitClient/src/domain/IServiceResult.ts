export default interface IServiceResult<TEntity> {
    status: number,
    data: TEntity
}