using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MonctonEventsNet.Model;

namespace MonctonEventsNet.Infrastructure;

public class EventContext : DbContext
{
    #region DbSets
    
    public DbSet<Event> Events { get; set; }
    public DbSet<Cost> Costs { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<EventType> EventTypes { get; set; }    
    
    #endregion
    
    #region Constructor
    
    public EventContext(DbContextOptions<EventContext> options) : base(options) {}
    
    #endregion
    
    #region Overrides
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    
    #endregion
}