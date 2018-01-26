using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhilscanExcellence.Models;
using PhilscanExcellence.Services;
using System.IO;
using System.Data.Entity;


namespace PhilscanExcellence.Controllers
{
    public class ELearningController : Controller
    {
        // GET: ELearning
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Items(Guid? ID)
        {
            return View();
        }

        public ActionResult TakeExam(Guid? ID)
        {
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload()
        {
            var httpRequest = System.Web.HttpContext.Current.Request;

            HttpFileCollection uploadFiles = httpRequest.Files;

            var docFiles = new List<string>();

            string newFileName = "";

            string ext = "";

            List<ExamAttachment> listAttachment = new List<ExamAttachment>();

            if (httpRequest.Files.Count > 0)
            {
                for (int i = 0; i < uploadFiles.Count; i++)
                {
                    HttpPostedFile postedFile = uploadFiles[i];

                    ext = Path.GetExtension(postedFile.FileName);

                    newFileName = DateTime.Now.ToString("MMddyyyHHmmSS");

                    var filePath = Server.MapPath(@"\FileUploads\" + newFileName + ext);

                    postedFile.SaveAs(filePath);

                    using (var db = new PhilscanExcellenceEntities())
                    {
                        ExamAttachment newAttachment = new ExamAttachment
                        {
                            ID = Guid.NewGuid(),
                            Extension = ext,
                            FileName = postedFile.FileName,
                            FileSize = (postedFile.ContentLength / 1024).ToString(),
                            Path = newFileName + ext,
                            Status = 1
                        };

                        db.Entry(newAttachment).State = EntityState.Added;

                        db.SaveChanges();

                        listAttachment.Add(newAttachment);
                    }
                }
            }

            return Json(new { uploadFile = listAttachment.FirstOrDefault() });
        }

        [HttpPost]
        public JsonResult DeleteAttachment(ElearningAttachmentModel _file)
        {
            string serverResponse = "";

            if(_file != null)
            {
                _file.Status = 0;

                ELearningService.UpdateAttachment(_file, out serverResponse);
            }

            return Json(serverResponse);
        }

        [HttpPost]
        public JsonResult GetItems(Guid _headerID)
        {
            string serverResponse = "";

            var items = ELearningService.GetAllItem(_headerID, out serverResponse);

            return Json(new { items = items, error = serverResponse });
        }

        [HttpPost]
        public JsonResult DeleteItem(ELearningItemModel _item, Guid? _attachmentID)
        {
            string serverResponse = "";

            if(_item != null)
            {
                _item.Status = 0;

                ELearningService.SaveItem(_item, _attachmentID, out serverResponse);
            }

            return Json(serverResponse);
        }

        [HttpPost]
        public JsonResult SaveItem(ELearningItemModel _item, Guid? _attachmentID, Guid _headerID)
        {
            string serverResponse = "";

            if (_item != null)
            {
                _item.HeaderID = _headerID;

                ELearningService.SaveItem(_item, _attachmentID, out serverResponse);
            }

            return Json(serverResponse);
        }

        [HttpPost]
        public JsonResult GetAllActiveHeader()
        {
            string serverResponse = "";

            var header = ELearningService.GetAllHeader(1, out serverResponse);

            return Json(new { error = serverResponse, header = header });
        }

        [HttpPost]
        public JsonResult SaveHeader(ELearningHeaderModel _header)
        {
            string serverResponse = "";

            if (_header != null)
                ELearningService.SaveHeader(_header, out serverResponse);

            return Json(serverResponse);
        }

        //====TAKE EXAM====
        [HttpPost]
        public ActionResult GetRandomQuestion(Guid? _headerID)
        {
            string serverResponse = "";

            var question = ELearningService.GetRandomQuestion(_headerID, out serverResponse);

            var examCount = ELearningService.GetTakeExamCount(_headerID, out serverResponse);

            return Json(new { question = question, error = serverResponse,
                examCount = examCount });
        }

        [HttpPost]
        public ActionResult Submit(ELearningItemModel _model, Guid _resultID, string _answer)
        {
            string serverResponse = "";

            ELearningService.SubmitAnswer(_model, _resultID, _answer, out serverResponse);

            return Json(new { error = serverResponse });
        }
    }
}