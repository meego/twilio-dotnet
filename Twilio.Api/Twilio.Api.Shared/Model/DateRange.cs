// https://huntjason.wordpress.com/2005/10/28/net-c-date-range-implementation-using-generics/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twilio
{
    public class DateRange : IEquatable<DateRange>
    {

        Nullable<DateTime> startDate, endDate;
        public DateRange(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            AssertStartDateFollowsEndDate(startDate, endDate);
            this.startDate = startDate;
            this.endDate = endDate;
        }


        public Nullable<TimeSpan> TimeSpan
        {
            get { return endDate.Value.Subtract(startDate.Value); }
        }
        public Nullable<DateTime> StartDate
        {
            get { return startDate; }
            set
            {
                AssertStartDateFollowsEndDate(value, this.endDate);
                startDate = value;
            }
        }
        public Nullable<DateTime> EndDate
        {
            get { return endDate; }
            set
            {
                AssertStartDateFollowsEndDate(this.startDate, value);
                endDate = value;
            }
        }
        private void AssertStartDateFollowsEndDate(Nullable<DateTime> startDate,
            Nullable<DateTime> endDate)
        {
            if ((startDate.HasValue && endDate.HasValue) &&
                (endDate.Value < startDate.Value))
                throw new InvalidOperationException("Start Date must be less than or equal to End Date");
        }
        public bool Equals(DateRange other)
        {
            if (object.ReferenceEquals(other, null)) return false;
            return ((startDate == other.StartDate) && (endDate == other.EndDate));
        }
    }
}
