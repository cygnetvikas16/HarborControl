using System;
using System.Collections.Generic;

namespace HarborControl.Models
{
    public partial class BoatMaster
    {
        public BoatMaster()
        {
            BoatSchedule = new HashSet<BoatSchedule>();
        }

        public int Id { get; set; }
        public string BoatType { get; set; }
        public decimal? Speed { get; set; }

        public virtual ICollection<BoatSchedule> BoatSchedule { get; set; }
    }
}
