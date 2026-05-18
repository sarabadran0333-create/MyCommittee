using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCommittee.Models
{
    public class Actions
    {
        [Key]
        public int ActivityId { get; set; }
        public int JobId { get; set; }
        public string? ActionType { get; set; }
        public string? Details { get; set; }
        public DateTime Date { get; set; }
        public int CommitteeId { get; set; }
        [ForeignKey("JobId")]
        public virtual Member Member { get; set; }

        [ForeignKey("CommitteeId")]
        public virtual Committee Committee { get; set; }



        public string GetTimeAgo()
        {
            // حساب الفرق بين الوقت الحالي ووقت الأكشن
            TimeSpan timeDiff = DateTime.Now - this.Date;

            // 1. إذا كان الفرق أقل من دقيقة
            if (timeDiff.TotalMinutes < 1)
                return "Just now";

            // 2. إذا كان الفرق أقل من ساعة
            if (timeDiff.TotalHours < 1)
                return $"{(int)timeDiff.TotalMinutes} minutes ago";

            // 3. إذا كان الفرق أقل من يوم (هنا تظهر الساعات)
            if (timeDiff.TotalDays < 1)
            {
                int hours = (int)timeDiff.TotalHours;
                return hours == 1 ? "An hour ago" : $"{hours} hours ago";
            }

            // 4. إذا كان الفرق بالأيام
            if (timeDiff.TotalDays < 7)
            {
                int days = (int)timeDiff.TotalDays;
                return days == 1 ? "Yesterday" : $"{days} days ago";
            }

            // 5. إذا مر أكثر من أسبوع، نعرض التاريخ العادي
            //return this.Date.ToString("dd/MM/yyyy");
            int weeks = (int)(timeDiff.TotalDays / 7);
            return weeks == 1 ? "1 week ago" : $"{weeks} weeks ago";
        }
    }
}