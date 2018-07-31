using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HandsToOfferApi.Models;


namespace HandsToOfferApi.Dto
{
    public class EventDetailsDto
    {
        private int EventId = 0;
        private H2OContext db = new H2OContext();

        public EventDetailsDto(int eventId)
        {
            this.EventId = eventId;
        }

        public Event evenT
        {  
            get{
                return db.Event.Where(x => x.EventId == EventId).FirstOrDefault();
            }
        }

        public List<EventParticipants> participants {
            get {

                List<EventParticipants> p = (from e in db.EventUsers
                                             join u in db.H2OUsers on e.UserId equals u.Id
                                             where e.EventId == EventId
                                             select new EventParticipants
                                             {
                                                 Name = u.UserName,
                                                 Joining = e.Joining
                                             }).ToList();
                return p;
            }
        }

        public List<string> imageList { get {
                return db.ImageUpload.Where(x => x.EventId == EventId).Select(y => y.ImageLocation).ToList();
            }
        }

    }

    public class EventParticipants
    {
        public string Name { get; set; }

        public string Joining { get; set; }
    }
}


