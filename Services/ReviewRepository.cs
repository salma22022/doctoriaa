using Project.Models;

namespace Project.Services
{
    public class ReviewRepository : IReviewRepository
    {
        VeseetaDBContext Context;
        public ReviewRepository(VeseetaDBContext context)
        {
            this.Context = context;
        }

        public int AddReview(Review review)
        {
            Context.Reviews.Add(review);
            return Context.SaveChanges();
        }
        public List<Review> GetByDocId(int docid)
        {
            List<Review> reviews = new List<Review>();
            //reviews = Context.Reviews.Where(s => s.DoctorId == docid).ToList();
            return reviews;
        }
    }
}
