using Blog_BussinesLayer;
using Blog_Entities;
using Blog_WebUI.Filter;
using Blog_WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog_WebUI.Controllers
{
    [HandleException]
    public class CommentController : Controller
    {
        NoteManager noteManager =new NoteManager();
        CommentManager commentManager = new CommentManager();
        // GET: Comment
        public ActionResult ShowNoteComments(int? id)
        {
            // bu action içinden geriye bir PartialView dönecek.. ve çağırıldığı noktada seçilen id'li elementin olduğu yere html olarak bu partial view yerleştirilecek.
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // eğer id null değilse veritabanından ilgili note'a ait yorumlar getirilmeli.

            Note note=noteManager.Find(x=>x.Id==id);

            if (note==null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialComment",note.Comments);
        }
        [Auth]
        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {   // parametredeki text değeri ajax'taki data içindeki değişken ile aynı isimde olmalı.

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = commentManager.Find(x => x.Id == id);

            if (comment == null)
            {
                return new HttpNotFoundResult();
            }
            comment.Text = text;

            if (commentManager.Update(comment) > 0)
            {
                // Update işlemi gerçekleşmiştir.
                // result ismi JS tarafındaki if (data.result) resul ile aynı olmalı.
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // update işlemi sırasında bir hata oluşmuştur.
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }



        }
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = commentManager.Find(x => x.Id == id);

            if (comment == null)
            {
                return new HttpNotFoundResult();
            }
           

            if (commentManager.Delete(comment) > 0)
            {
                // Delete işlemi gerçekleşmiştir.
                // result ismi JS tarafındaki if (data.result) resul ile aynı olmalı.
                return Json(new { data = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // update işlemi sırasında bir hata oluşmuştur.
                return Json(new { data = false }, JsonRequestBehavior.AllowGet);
            }
        }


        // Yorum ekleme: Parametre olarak Comment nesnesini verdim. ajax tarafından burası için bana gönderilen değer Text değişkeni ile geliyor. Eger comment nesnesinin içinde Text isimli bir property tanımlı ise ben bu değeri comment nesnesi ile de yakalayabilirim. Binding işlemi aynı isme sahip değişkenler oldugu için yapılmıs olacak.
        [Auth]
        [HttpPost]
        public ActionResult Create(Comment comment, int? noteId)
        {
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                if (noteId==null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Note note = noteManager.Find(x=>x.Id==noteId);
                if (note==null)
                {
                    return new HttpNotFoundResult();
                }

                comment.Note=note;
                comment.Owner=CurrentSession.User;
                comment.ModifiedDate=DateTime.Now;
                comment.CreatedDate=DateTime.Now;
                comment.ModifiedUserName=CurrentSession.User.Username;

                if (commentManager.Insert(comment) > 0)
                {
                    // Yorum ekleme işlemi başarılı oldugu için data=true diyerek true bilgisini ajax tarafın geri gönderiyorum
                    return Json(new { data = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { data = false}, JsonRequestBehavior.AllowGet);
        }
    }
}