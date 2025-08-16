using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
namespace Bookify.Infrastructure.Outbox
{
    public class ProcessOutboxMessagesJobSetup : IConfigureOptions<QuartzOptions>
    {
        private readonly OutboxOptions _outboxOptions;

        public ProcessOutboxMessagesJobSetup(IOptions<OutboxOptions> outboxOptions)
        {
            _outboxOptions = outboxOptions.Value;
        }

        public void Configure(QuartzOptions options)
        {
            const string jobName = nameof(ProcessOutboxMessagesJob);

            options
                .AddJob<ProcessOutboxMessagesJob>(configure => configure.WithIdentity(jobName))
                .AddTrigger(configure =>
                    configure
                        .ForJob(jobName)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds).RepeatForever()));
        }
    }
}
