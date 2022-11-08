using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Models.Delegates;

namespace UKParliament.CodeTest.Data.Repository;

public class BaseRepository
{
    private readonly PersonManagerContext _context;
    private readonly CurrentTime _currentTime;

    protected BaseRepository(PersonManagerContext context, CurrentTime currentTime)
    {
        _context = context;
        _currentTime = currentTime;
    }

    protected void Save<T>(T entity) where T : BaseEntity
    {
        if (entity.Id != 0)
        {
            entity.Updated = _currentTime();
            DetachLocalChanges(entity);
        }

        _context.Update(entity);
    }

    protected void Delete<T>(T? entity) where T : BaseEntity
    {
        if (entity != null)
        {
            DetachLocalChanges(entity);
            _context.Remove(entity);
        }
    }
    
    protected void Delete<T>(IEnumerable<T>? entities) where T : BaseEntity
    {
        if (entities?.Any() ?? false)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }
    }

    //Looks like EntityFrameWork QueryTracking disabled doesn't work with InMemoryDatabase - https://stackoverflow.com/a/58420958
    //This just manually detach old entity before saving/deleting new one
    private void DetachLocalChanges<T>(T entity) where T : BaseEntity
    {
        var local = _context.Set<T>().Local.FirstOrDefault(entry => entry.Id.Equals(entity.Id));

        if (local != null)
        {
            _context.Entry(local).State = EntityState.Detached;
        }
    }
}