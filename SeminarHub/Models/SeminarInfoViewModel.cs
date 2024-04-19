using SeminarHub.Data;

namespace SeminarHub.Models
{
    
    public class SeminarInfoViewModel
    {
        public SeminarInfoViewModel(
            int id, 
            string topic, 
            string lecturer, 
            string category, 
            string organizer,
            DateTime dateAndTime)
        {
            Id = id;
            Topic = topic;
            Lecturer = lecturer;
            Category = category;
            Organizer = organizer;
            DateAndTime = dateAndTime.ToString(DataRequirments.DateAndTimeFormat);
        }

        public int Id { get; set; }

        public string Topic { get; set; }

        public string Lecturer { get; set; }

        public string Category { get; set; }

        public string Organizer { get; set; } 

        public string DateAndTime { get; set; }
    }
}
