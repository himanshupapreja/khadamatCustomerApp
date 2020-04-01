using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class ReviewModel
    {
        public int job_request_id { get; set; }
        public string user_review { get; set; }
        public int rating { get; set; }
        public int worker_id { get; set; }
        public int customer_id { get; set; }
    }
    public class WorkerReviewResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public ReviewModel jobReview { get; set; }
    }
}
