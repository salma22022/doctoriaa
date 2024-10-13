using Project.Models;

namespace Project.Services
{
    public class DayRepository : IDayRepository
    {
        VeseetaDBContext context;

        public DayRepository(VeseetaDBContext context)
        {
            this.context = context;
        }
        public int AddDay(Day day)
        {
            context.Days.Add(day);
            context.SaveChanges();
            return 1;
        }

        public int DeleteDay(int dayId)
        {
            throw new NotImplementedException();
        }

        public List<Day> GetDays(int docId)
        {
            throw new NotImplementedException();
        }

        public List<Day> GetDays()
        {
            throw new NotImplementedException();
        }

        public int UpdateDay(Day day)
        {
            throw new NotImplementedException();
        }
    }
}
