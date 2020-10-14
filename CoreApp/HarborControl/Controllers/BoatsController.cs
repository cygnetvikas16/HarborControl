using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Hangfire;
using HarborControl.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HarborControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BoatsController : ControllerBase
    {
        private readonly HarborControlContext _context;
        // Instantiate random number generator.  
        private readonly Random _random = new Random();
        public IConfiguration _configuration { get; }
        public BoatsController(HarborControlContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        /// <summary>
        /// GetBoatsCounts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBoatsCounts")]
        public async Task<ActionResult<BoatsCounts>> GetBoatsCounts()
        {
            BoatsCounts boatsCounts = new BoatsCounts();
            var dbResult = await _context.BoatSchedule.ToListAsync();
            // Perimeter
            boatsCounts.SpeedboatPerimeter = dbResult.Where(x => x.BoatStatus == (int)BoatStatus.At_Perimeter && x.BoatMasterId == (int)BoatName.Speedboat).Count();
            boatsCounts.SailboatPerimeter = dbResult.Where(x => x.BoatStatus == (int)BoatStatus.At_Perimeter && x.BoatMasterId == (int)BoatName.Sailboat).Count();
            boatsCounts.CargoShipPerimeter = dbResult.Where(x => x.BoatStatus == (int)BoatStatus.At_Perimeter && x.BoatMasterId == (int)BoatName.Cargo_ship).Count();
            //Dock
            boatsCounts.SpeedboatDock = dbResult.Where(x => x.BoatStatus == (int)BoatStatus.At_Harbor && x.BoatMasterId == (int)BoatName.Speedboat).Count();
            boatsCounts.SailboatDock = dbResult.Where(x => x.BoatStatus == (int)BoatStatus.At_Harbor && x.BoatMasterId == (int)BoatName.Sailboat).Count();
            boatsCounts.CargoShipDock = dbResult.Where(x => x.BoatStatus == (int)BoatStatus.At_Harbor && x.BoatMasterId == (int)BoatName.Cargo_ship).Count();

            // add job as RecurringJob
            RecurringJob.AddOrUpdate(() => UpdateBoatStatus(), "*/5 * * * * *");
            RecurringJob.AddOrUpdate(() => CreateRandomBoats(), Cron.MinuteInterval(03));
            return Ok(boatsCounts);
        }

        /// <summary>
        /// GetInProcessBoat
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetInProcessBoat")]
        public async Task<ActionResult<InProcessBoat>> GetInProcessBoat()
        {

            var dbResult = await _context.BoatSchedule.Where(x => x.BoatStatus == (int)BoatStatus.InProcess).FirstOrDefaultAsync();
            InProcessBoat inPBoat = new InProcessBoat();
            if (dbResult != null)
            {
                inPBoat.BoatMasterId = dbResult.BoatMasterId;
                inPBoat.BoatName = dbResult.BoatName;
                inPBoat.BoatStatus = dbResult.BoatStatus;
                inPBoat.CreatedDate = dbResult.CreatedDate;
                inPBoat.Id = dbResult.Id;
                inPBoat.ModifyDate = dbResult.ModifyDate;
                inPBoat.ProcessEndDate = dbResult.ProcessEndDate;
                inPBoat.ProcessStartDate = dbResult.ProcessStartDate;
            }
            return Ok(inPBoat);
        }

        /// <summary>
        /// ScheduleBoat
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ScheduleBoat")]
        public async Task<IActionResult> ScheduleBoat(BoatScheDetails bId)
        {
            var disPerimeterTODock =Convert.ToDouble(_configuration.GetSection("Distance").GetSection("distPerimeterTODock").Value);
            var dbResult = await _context.BoatSchedule
                .Join(_context.BoatMaster,
                 s => s.BoatMasterId,
                 sa => sa.Id,
                 (s, sa) => new { BoatSchedule = s, BoatMaster = sa })


                .Where(x => x.BoatSchedule.Id == bId.Id).FirstOrDefaultAsync();
            // get time to reach 
            TimeSpan duration = TimeSpan.FromMinutes(cal_time(disPerimeterTODock, Convert.ToDouble(dbResult.BoatMaster.Speed)) * 60);
            DateTime today = DateTime.Now;
            dbResult.BoatSchedule.ModifyDate = today;
            dbResult.BoatSchedule.ProcessStartDate = today;
            dbResult.BoatSchedule.ProcessEndDate = today.Add(duration);
            dbResult.BoatSchedule.BoatStatus = (int)BoatStatus.InProcess;

            BoatSchedule boatSchedule = new BoatSchedule();
            boatSchedule = dbResult.BoatSchedule;
            // Console.WriteLine(span.ToString(@"hh\:mm\:ss"));
            _context.Entry(boatSchedule).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(NoContent());
            
        }

        /// <summary>
        /// GetWindSpeed from external api
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetWindSpeed")]
        public async Task<ActionResult<WindSpeed>> GetWindSpeed()
        {
            WindSpeed windSpeed = new WindSpeed();
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // New code:
                HttpResponseMessage response = await client.GetAsync(new Uri(_configuration.GetSection("wetherAPI").GetSection("baseUrl").Value));
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<WeatherRoot>(result);
                    // convert wind speed m/s to km/h

                    var step1 = data.wind.speed * 18;
                    var KMH = Math.Round((step1 / 5), 2);

                    windSpeed.windKmH = Convert.ToInt32(KMH);
                    windSpeed.windMS = Convert.ToDecimal(data.wind.speed);


                }
            }
            return windSpeed;
        }

        /// <summary>
        /// GetAllAtPerimeterBoats
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllAtPerimeterBoats")]
        public async Task<ActionResult<List<AtPerimeterBoats>>> GetAllAtPerimeterBoats()
        {
            List<AtPerimeterBoats> lstboats = new List<AtPerimeterBoats>();
            var boats = await _context.BoatSchedule
                 .Join(_context.BoatMaster,
                  s => s.BoatMasterId,
                  sa => sa.Id,
                  (s, sa) => new { BoatSchedule = s, BoatMaster = sa })
                 .Where(x => x.BoatSchedule.BoatStatus == (int)BoatStatus.At_Perimeter).ToListAsync();

            foreach (var boat in boats)
            {
                AtPerimeterBoats atPerimeterBoats = new AtPerimeterBoats();
                atPerimeterBoats.Id = boat.BoatSchedule.Id;
                atPerimeterBoats.BoatName = boat.BoatSchedule.BoatName;
                atPerimeterBoats.BoatType = boat.BoatMaster.BoatType;

                lstboats.Add(atPerimeterBoats);
            }
            return lstboats;
        }



        #region hangFireJob methods
        // call this method in the hanfgFire
        // Updtae the records which boat is reached at the dock by triggering the schedular 
        [HttpPost]
        [Route("HangFireJob")]
        public async Task<IActionResult> UpdateBoatStatus()
        {
            var boats = await _context.BoatSchedule.Where(x => x.BoatStatus == (int)BoatStatus.InProcess).ToListAsync();
            foreach (var boat in boats)
            {
                if (DateTime.Now > boat.ProcessEndDate)
                {
                    boat.BoatStatus = (int)BoatStatus.At_Harbor;
                    boat.ModifyDate = DateTime.Now;
                    _context.Entry(boat).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
            return Ok();
        }
        /// <summary>
        /// CreateRandomBoats
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateRandomBoats")]
        public async Task<IActionResult> CreateRandomBoats()
        {
            BoatSchedule boatSchedule = new BoatSchedule();
            boatSchedule.BoatMasterId = RandomNumber(1, 4);
            boatSchedule.BoatStatus = (int)BoatStatus.At_Perimeter; // at Perimeter;
            boatSchedule.BoatName = GenerateName(5);
            boatSchedule.CreatedDate = DateTime.Now;
            _context.BoatSchedule.Add(boatSchedule);
            await _context.SaveChangesAsync();
            return Ok("Boat created");
        }

        #endregion
        #region enum

        protected enum BoatStatus
        {
            At_Perimeter = 1,
            At_Harbor = 2,
            InProcess = 3
        }

        protected enum BoatName
        {
            Speedboat = 1,
            Sailboat = 2,
            Cargo_ship = 3
        }

        #endregion+
        #region comman methods 
        // Generates a random number within a range.      
        protected int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        protected static string GenerateName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;


        }

        // Function to calculate time taken to reach
        protected static double cal_time(double dist, double speed)
        {
            return dist / speed;
        }

        #endregion
    }
}
