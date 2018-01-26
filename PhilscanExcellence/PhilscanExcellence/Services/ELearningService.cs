using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhilscanExcellence.Models;
using System.Data.Entity;

namespace PhilscanExcellence.Services
{
    public class ELearningService
    {
        public static void TakeExamHeader(ELearningHeaderModel _header, out string message)
        {
            try
            {
                message = "";

                using (var db = new PhilscanExcellenceEntities())
                {
                    ResultHeader header = new ResultHeader
                    {
                        ID = Guid.NewGuid(),
                        ExamID = _header.ID,
                        Status = 0,
                        TakenBy = UniversalService.CurrentUser.ID,
                        TotalScore = 0
                    };

                    db.Entry(header).State = EntityState.Added;

                    db.SaveChanges();
                }
            }
            catch(Exception error)
            {
                message = error.Message;
            }
        }

        public static void SubmitAnswer (ELearningItemModel _item, Guid _resultID, string _answer, out string message)
        {
            try
            {
                message = "";

                using (var db = new PhilscanExcellenceEntities())
                {
                    ResultItem result = new ResultItem
                    {
                        ID = Guid.NewGuid(),
                        AnswerDescription = _answer,
                        ItemID = _item.ID,
                        ResultID = _resultID
                    };

                    if (_item.Answer == _answer)
                        result.Status = 1;
                    else
                        result.Status = 0;

                    db.Entry(result).State = EntityState.Added;
                }
            }
            catch(Exception error)
            {
                message = error.Message;
            }
        }

        public static TakeExamCountModel GetTakeExamCount(Guid? _headerID, out string message)
        {
            try
            {
                message = "";

                using (var db = new PhilscanExcellenceEntities())
                {
                    TakeExamCountModel examCount = new TakeExamCountModel();

                    examCount.TotalItem = db.ExamItems.Where(r => r.HeaderID == _headerID).Count();

                    var resultID = db.ResultHeader.FirstOrDefault(r => r.ExamID == _headerID && r.TakenBy
                        == UniversalService.CurrentUser.ID).ID;

                    examCount.CurrentItem = db.ResultItem.Where(r => r.ResultID == resultID).Count() + 1;

                    return examCount;
                }
            }
            catch(Exception error)
            {
                message = error.Message;

                return null;
            }
        }

        public static ELearningItemModel GetRandomQuestion(Guid? _headerID, out string message)
        {
            try
            {
                message = "";

                using (var db = new PhilscanExcellenceEntities())
                {
                    var query = from q in db.vm_RandomQuestion
                                where q.HeaderID == _headerID
                                select new ELearningItemModel
                                {
                                    ID = q.ID,
                                    Answer = q.Answer,
                                    AttachmentID = q.AttachmentID,
                                    ChoiceA = q.ChoiceA,
                                    ChoiceB = q.ChoiceB,
                                    ChoiceC = q.ChoiceC,
                                    ChoiceD = q.ChoiceD,
                                    ChoiceE = q.ChoiceE,
                                    CreatedBy = q.CreatedBy,
                                    CreatedDate = q.CreatedDate,
                                    HeaderID = q.HeaderID,
                                    Question = q.Question
                                };

                    return query.FirstOrDefault();
                }
            }
            catch(Exception error)
            {
                message = error.Message;

                return null;
            }
        }

        public static ElearningAttachmentModel GetAttachment(Guid? ID)
        {
            try
            {
                using (var db = new PhilscanExcellenceEntities())
                {
                    var query = from a in db.ExamAttachment
                                where a.Status == 1 && a.ID == ID
                                select new ElearningAttachmentModel
                                {
                                    ID = a.ID,
                                    Extension = a.Extension,
                                    FileName = a.FileName,
                                    FileSize = a.FileSize,
                                    Path = a.Path,
                                    Status = 1
                                };

                    return query.FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }

        public static void UpdateAttachment(ElearningAttachmentModel item, out string message)
        {
            try
            {
                message = "";

                using (var db = new PhilscanExcellenceEntities())
                {
                    if(item.Status == 0)
                    {
                        var file = db.ExamAttachment.FirstOrDefault(r => r.ID == item.ID);

                        if(file != null)
                        {
                            db.Entry(file).State = EntityState.Deleted;
                        }
                    }

                    db.SaveChanges();
                }
            }
            catch(Exception error)
            {
                message = error.Message;
            }
        }

        public static void SaveItem(ELearningItemModel _item, Guid? _attachmentID , out string message)
        {
            try
            {
                message = "";

                using (var db = new PhilscanExcellenceEntities())
                {
                    if(_item.ID == Guid.Empty)
                    {
                        ExamItems newItem = new ExamItems
                        {
                            ID = Guid.NewGuid(),
                            Answer = _item.Answer,
                            ChoiceA = _item.ChoiceA,
                            ChoiceB = _item.ChoiceB,
                            ChoiceC = _item.ChoiceC,
                            ChoiceD = _item.ChoiceD,
                            ChoiceE = _item.ChoiceE,
                            ChoiceF = "",
                            CreatedBy = UniversalService.CurrentUser.ID,
                            CreatedDate = DateTime.Now,
                            HeaderID = _item.HeaderID,
                            Question = _item.Question,
                            AttachmentID = _attachmentID
                        };

                        db.Entry(newItem).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        var item = db.ExamItems.FirstOrDefault(r => r.ID == _item.ID);

                        if(item != null)
                        {
                            if (_item.Status == 1)
                            {
                                item.Question = _item.Question;

                                item.ChoiceA = _item.ChoiceA;

                                item.ChoiceB = _item.ChoiceB;

                                item.ChoiceD = _item.ChoiceD;

                                item.ChoiceE = _item.ChoiceE;

                                item.CreatedDate = DateTime.Now;

                                item.CreatedBy = UniversalService.CurrentUser.ID;

                                item.AttachmentID = _attachmentID;

                                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                            {
                                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                            }
                        }
                    }

                    db.SaveChanges();
                }
            }
            catch(Exception error)
            {
                message = error.Message;
            }
        }

        public static List<ELearningItemModel> GetAllItem(Guid? _headerID, out string message)
        {
            try
            {
                message = "";

                using (var db = new PhilscanExcellenceEntities())
                {
                    var query = from i in db.ExamItems
                                join us in db.UserAccount on i.CreatedBy equals us.ID into qUS
                                from u in qUS.DefaultIfEmpty()
                                where i.HeaderID == _headerID
                                select new ELearningItemModel
                                {
                                    ID = i.ID,
                                    HeaderID = i.HeaderID,
                                    Question = i.Question,
                                    ChoiceA = i.ChoiceA,
                                    ChoiceB = i.ChoiceB,
                                    ChoiceC = i.ChoiceC,
                                    ChoiceD = i.ChoiceD,
                                    ChoiceE = i.ChoiceE,
                                    Answer = i.Answer,
                                    CreatedBy = i.CreatedBy,
                                    CreatedDate = i.CreatedDate,
                                    AttachmentID = i.AttachmentID
                                };


                    var retItem = query.ToList();

                    retItem.ForEach(item =>
                    {
                        item.Attachment = GetAttachment(item.AttachmentID);
                    });

                    return retItem;
                }
            }
            catch(Exception error)
            {
                message = error.Message;

                return null;
            }
        }

        public static List<ELearningHeaderModel> GetAllHeader(int status, out string message)
        {
            try
            {
                message = "";

                using(var db = new PhilscanExcellenceEntities())
                {
                    var query = from h in db.ExamHeader
                                join us in db.UserAccount on h.CreatedBy equals us.ID into qUS
                                from u in qUS.DefaultIfEmpty()
                                where h.Status == status
                                orderby h.CreatedBy
                                select new ELearningHeaderModel
                                {
                                    ID = h.ID,
                                    Title = h.Title,
                                    Description = h.Description,
                                    CreatedBy = h.CreatedBy,
                                    CreatedDate = h.CreatedDate,
                                    ShowCreatedBy = u.FirstName + " " + u.LastName,
                                    Status = h.Status
                                };

                    return query.ToList();
                                
                }
            }
            catch(Exception error)
            {
                message = error.Message;

                return null;
            }
        }

        public static void SaveHeader(ELearningHeaderModel _header, out string message)
        {
            try
            {
                message = "";

                using (var db = new PhilscanExcellenceEntities())
                {
                    if(_header.ID == Guid.Empty) //NEW
                    {
                        ExamHeader newHeader = new ExamHeader
                        {
                            ID = Guid.NewGuid(),
                            Title = _header.Title,
                            Description = _header.Description,
                            CreatedDate = DateTime.Now,
                            CreatedBy = UniversalService.CurrentUser.ID,
                            Status = 1
                        };

                        db.Entry(newHeader).State = System.Data.Entity.EntityState.Added;
                    }
                    else //UPDATE
                    {
                        var header = db.ExamHeader.FirstOrDefault(r => r.ID == _header.ID);

                        if(header != null)
                        {
                            header.Title = _header.Title;

                            header.Description = _header.Description;

                            header.CreatedBy = UniversalService.CurrentUser.ID;

                            header.CreatedDate = DateTime.Now;

                            header.Status = _header.Status;

                            db.Entry(header).State = System.Data.Entity.EntityState.Modified;
                        }
                    }

                    db.SaveChanges();
                }
            }
            catch(Exception error)
            {
                message = error.Message;
            }
        }
    }
}