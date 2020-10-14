using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HarborControl.Models
{
    public class InProcessBoat
    {
        public long Id { get; set; }
        public int BoatMasterId { get; set; }
        public short BoatStatus { get; set; }
        public string BoatName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public DateTime? ProcessStartDate { get; set; }
        public DateTime? ProcessEndDate { get; set; }
    }
}
