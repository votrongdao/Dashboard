using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DashboardSite.Model
{
    public class SecurityQuestionViewModel
    {
        public IList<SelectListItem> QuestionList { get; set; }

        public SecurityQuestionViewModel()
        {
            QuestionList = new List<SelectListItem>();
            IsAnswerStep = true;     
        }

        public bool IsAnswerStep { get; set; }

        [Required]
        [Display(Name = "First Security Question")]
        [DifferFrom("SecondQuestionId", "ThirdQuestionId", ErrorMessage = "Not unique question one")]
        public string FirstQuestionId { get; set; }

        [Required]
        [Display(Name = "Answer 1")]
        public string FirstAnswer { get; set; }

        [Required]
        [Display(Name = "Second Security Question")]
        [DifferFrom("FirstQuestionId", "ThirdQuestionId", ErrorMessage = "Not unique question two")]
        public string SecondQuestionId { get; set; }

        [Required]
        [Display(Name = "Answer 2")]
        public string SecondAnswer { get; set; }

        [Required]
        [Display(Name = "Third Security Question")]
        [DifferFrom("FirstQuestionId", "SecondQuestionId", ErrorMessage = "Not unique question three")]
        public string ThirdQuestionId { get; set; }

        [Required]
        [Display(Name = "Answer 3")]
        public string ThirdAnswer { get; set; }

        public string UserAnswer1 { get; set; }
        public string UserAnswer2 { get; set; }
        public string UserAnswer3 { get; set; }
        public string UserName { get; set; }
    }
}
