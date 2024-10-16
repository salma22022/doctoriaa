using Project.Models;

namespace Project.Services
{
    public interface IReviewRepository
    {
        public int AddReview(Review review);
        public List<Review> GetByDocId(int docid);
    }
}
