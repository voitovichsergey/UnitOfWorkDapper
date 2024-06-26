﻿# Unit of Work
## Theory
Unit of Work is a pattern that defines a logical transaction, i.e. atomic synchronization of changes in objects placed in a UoW object with a repository (database).
If we turn to the original description of this pattern by Martin Fowler, it can be seen that the object implementing this pattern is responsible for accumulating information about which objects are included in the transaction and what their changes are relative to the original values in the repository. The main work is done in the commit() method, which is responsible for calculating changes in objects stored in UoW and synchronizing these changes with the repository (database).

## This implementation (with Dapper)
1. Install this nuget package
2. Create an IDbConnection inheritor class
```cs
    public sealed class MyAppConnection : SqliteConnection
    {
    }
```
3. In the Program.cs file, add the IUnitOfWork singleton to the services with the new connection class from the previous paragraph
```cs
    const string ConnectionString = "Data Source=AppDatabase.db";
    builder.Services.AddSingleton<IUnitOfWork>(new UnitOfWork<MyAppConnection>(ConnectionString));
```
4. Create your own repository interfaces inherited from IUowRepository
```cs
public interface ICarRepository : IUowRepository
```
5. When implementing repository interfaces, create a constructor with the IDbConnection parameter
```cs
/// <inheritdoc/>
public sealed class CarRepository : ICarRepository
{
    private readonly MyAppConnection _connection;

    public CarRepository(IDbConnection connection)
    {
        _connection = (MyAppConnection)connection;
        //...
    }

    //...
```
6. At the BLL, use StartTransaction() and CommitAsync() to ensure atomicity
```cs
public class DriverService : IDriverService
{
    private readonly IUnitOfWork _unitOfWork;
    //...

    public async Task DeleteAsync(long[] ids)
    {
        using (var uow = _unitOfWork.StartTransaction())
        {
            var cars = await uow.Repository<ICarRepository>().GetCarsByDriverIdsAsync(ids);
            await uow.Repository<ICarRepository>().DeleteAsync(cars.Select(x => x.Id));
            await uow.Repository<IDriverRepository>().DeleteAsync(ids);
            await uow.CommitAsync();
        }
    }
```
If execution reaches CommitAsync(), then all changes will be saved, otherwise all changes will be rolledback.

7. If you don't need to change the data (don't need transactions), you can use the Selector()
```cs
public class DriverService : IDriverService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    //...

    /// <inheritdoc/>
    public async Task<DriverDto[]> GetAsync(long[] ids)
    {
        using (var uow = _unitOfWork.Selector())
        {
            var cars = await uow.Repository<ICarRepository>().GetCarsByDriverIdsAsync(ids);
            var drivers = await uow.Repository<IDriverRepository>().GetAsync(ids);

            return drivers
                .Select(x =>
                {
                    var driver = _mapper.Map<DriverDto>(x);
                    driver.Cars = cars
                        .Where(z => z.DriverId == driver.Id)
                        .Select(z => _mapper.Map<CarDto>(z))
                        .ToArray();
                    return driver;
                })
                .ToArray();
        }
    }
```

## License
MIT