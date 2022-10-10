using System;
using System.ComponentModel.DataAnnotations;

namespace Vu_Victoria_HW3.Models
{
    public enum VoteSearch { [Display(Name = "Greater Than")] GreaterThan, [Display(Name = "Less Than")] LessThan }
    public enum SortOrder { Ascending, Descending }

    public class SearchViewModel
    {
        [Display(Name = "Search by Title:")]
        public String SearchTitle { get; set; }

        [Display(Name = "Search by Description:")]
        public String SearchDescription { get; set; }

        [Display(Name = "Search by Rating:")]
        public Rating? SelectedRating { get; set; }

        [Display(Name = "Search by Genre:")]
        public Int32 GenreID { get; set; }

        [Display(Name = "Search by User Rating:")]
        [Range(0.0, 10.00 )]
        public Decimal? SelectedVoteAverage { get; set; }

        [Display(Name = "Type of Search:")]
        public VoteSearch SelectedVote { get; set; }

        [Display(Name = "Released After:")]
        [DataType(DataType.Date)]
        public DateTime? SelectedDate { get; set; }
    }
}
