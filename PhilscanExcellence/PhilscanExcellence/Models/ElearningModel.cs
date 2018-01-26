using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhilscanExcellence.Models
{
    public class ElearningModel
    {
       
    }

    public class ELearningItemModel
    {
        public Guid ID { get; set; }
        public Guid HeaderID { get; set; }
        public string Question { get; set; }
        public string ChoiceA { get; set; }
        public string ChoiceB { get; set; }
        public string ChoiceC { get; set; }
        public string ChoiceD { get; set; }
        public string ChoiceE { get; set; }
        public string Answer { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int Status { get; set; }
        public Guid? AttachmentID { get; set; }
        public ElearningAttachmentModel Attachment { get; set; }
    }

    public class ElearningAttachmentModel
    {
        public Guid ID { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string FileSize { get; set; }
        public int Status { get; set; }
    }

    public class ELearningHeaderModel
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? CreatedBy { get; set; }
        public string ShowCreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ShowCreatedDate
        {
            get
            {
                if (CreatedDate != null)
                    return DateTime.Parse(CreatedDate.ToString()).ToShortDateString();
                else
                    return "";
            }
        }
        public int? Status { get; set; }
        public string ShowStatus
        {
            get
            {
                if (Status == 1)
                    return "Active";
                else if (Status == 2)
                    return "Inactive";
                else
                    return "";
            }
        }
    }

    public class ResultHeaderModel
    {
        public Guid ID { get; set; }
        public Guid ExamID { get; set; }
        public Guid TakenBy { get; set; }
        public int TotalScore { get; set; }
        public int Status { get; set; }
        public string ShowStatus
        {
            get
            {
                if (Status == 1)
                    return "Passed";
                else if (Status == 2)
                    return "Failed";
                else
                    return "Incomplete";
            }
        }
    }

    public class ResultItemModel
    {
        public Guid ID { get; set; }
        public Guid ItemID { get; set; }
        public string Anwser { get; set; }
        public string AnswerDescription { get; set; }
        public int Status { get; set; }
        public string ShowStatus
        {
            get
            {
                if (Status == 1)
                    return "Correct";
                else if (Status == 0)
                    return "Wrong";
                else
                    return "";
            }
        }
    }

    public class TakeExamCountModel
    {
        public int CurrentItem { get; set; }
        public int TotalItem { get; set; }
    }
}