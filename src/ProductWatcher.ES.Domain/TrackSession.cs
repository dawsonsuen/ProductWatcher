using System;
using NEvilES;

namespace ProductWatcher.ES.Domain
{
    public abstract class TrackSession
    {


        public class CreateSession : ICommand
        {
            public Guid StreamId { get; set; }
            public string IpAddress { get; set; }
            public string Browser { get; set; }
        }

        public class SessionCreated : CreateSession, IEvent
        {

        }


        public class Aggregate : AggregateBase,
            IHandleAggregateCommand<CreateSession>
        {
            public void Handle(CreateSession command)
            {
                Raise<SessionCreated>(command);
            }
        }
    }
}