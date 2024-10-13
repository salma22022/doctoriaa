using System.ComponentModel.DataAnnotations;
namespace Project.ViewModels
{
    public class DayViewModel
    {
        public List<DayDto> Days { get; set; }
    }

    public class DayDto
    {
        public string DayName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int WaitingPeriod { get; set; }

        public DateTime AppointmentTime { get; set; }

        // BlockOutHours for each day
        public List<BlockOutHourDto> BlockOutHours { get; set; }
    }

    public class BlockOutHourDto
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }


}
