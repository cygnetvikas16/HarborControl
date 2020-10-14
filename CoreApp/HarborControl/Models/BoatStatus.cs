using System;
using System.Collections.Generic;

namespace HarborControl.Models
{
    public partial class BoatStatus
    {
        public BoatStatus()
        {
            BoatSchedule = new HashSet<BoatSchedule>();
        }

        public short Id { get; set; }
        public string Status { get; set; }

        public virtual ICollection<BoatSchedule> BoatSchedule { get; set; }
    }
}
