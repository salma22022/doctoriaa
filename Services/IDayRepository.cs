using Project.Models;

namespace Project.Services
{
    public interface IDayRepository
    {
        public List<Day> GetDays(int docId);

        public List<Day> GetDays();
        
        public int AddDay(Day day);


        public int UpdateDay(Day day);

        public int DeleteDay(int dayId);

    }
}
