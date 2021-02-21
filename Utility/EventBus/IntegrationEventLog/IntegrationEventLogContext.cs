#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1

#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using Utility.DAL;

namespace Utility.IntegrationEventLog
{
    /// <summary>
    /// 集成事件日志  DbContext
    /// </summary>
    public class IntegrationEventLogContext : DbContext
    {
        /// <summary>
        /// 集成事件日志  DbContext
        /// </summary>

#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
        /// <param name="nameOrConnectString"></param>
        public IntegrationEventLogContext(string nameOrConnectString) : base(nameOrConnectString)
        {
        }
#else
        /// <param name="options"></param>
        public IntegrationEventLogContext(DbContextOptions<IntegrationEventLogContext> options) : base(options)
        {
        }
#endif

        /// <summary>
        /// 集成事件日志
        /// </summary>
        public DbSet<IntegrationEventLogEntry> IntegrationEventLogs { get; set; }

#if !(NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48)
        protected override void OnModelCreating(ModelBuilder builder)
        {          
            builder.Entity<IntegrationEventLogEntry>(ConfigureIntegrationEventLogEntry);
        }

        void ConfigureIntegrationEventLogEntry(EntityTypeBuilder<IntegrationEventLogEntry> builder)
        {
            builder.ToTable("IntegrationEventLog");

            builder.HasKey(e => e.EventId);

            var propertyBuilderEventId = builder.Property(e => e.EventId)
                .IsRequired();
            switch (DbConfig.Flag)
            {
                case DbFlag.SqlServer:
                    break;
                case DbFlag.MySql:
                    propertyBuilderEventId.HasColumnType("varchar(36)");//mysql  ef
                    break;
                case DbFlag.Sqlite:
                    propertyBuilderEventId.HasColumnType("text");//nhibernate sqlite
                    break;
                case DbFlag.Oracle:
                    break;
                case DbFlag.Postgre:
                    break;
                default:
                    break;
            }
            builder.Ignore(it => it.EventTypeShortName);
            builder.Ignore(it => it.IntegrationEvent);
            builder.Property(e => e.Content)
                .IsRequired();
            
           var propertyBuilderCreationTime = builder.Property(e => e.CreationTime)
                .IsRequired();
            switch (DbConfig.Flag)
            {
                case DbFlag.SqlServer:
                    break;
                case DbFlag.MySql:
                    propertyBuilderCreationTime.HasColumnType("datetime");
                    break;
                case DbFlag.Sqlite:
                   
                    break;
                case DbFlag.Oracle:
                    break;
                case DbFlag.Postgre:
                    break;
                default:
                    break;
            }

            builder.Property(e => e.State)
                .IsRequired();

            builder.Property(e => e.TimesSent)
                .IsRequired();

            builder.Property(e => e.EventTypeName)
                .IsRequired();

        }
#endif
    }
}
#endif