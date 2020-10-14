using System;
using System.Collections.Generic;

namespace HarborControl.Models
{
    public partial class BoatSchedule
    {
        public long Id { get; set; }
        public int BoatMasterId { get; set; }
        public short BoatStatus { get; set; }
        public string BoatName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public DateTime? ProcessStartDate { get; set; }
        public DateTime? ProcessEndDate { get; set; }

        public virtual BoatMaster BoatMaster { get; set; }
        public virtual BoatStatus BoatStatusNavigation { get; set; }
    }
}
