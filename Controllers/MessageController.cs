using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wall.Models;
using wall.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace wall.Controllers
{
    public class MessageController : Controller
    {
         private readonly MessageFactory messageFactory;
         public MessageController(MessageFactory message)
        {
            messageFactory = message;
        }
        
        // GET: /Home/
        [HttpGet]
        [Route("wall")]
        public IActionResult Index()
        {
            ViewBag.Errors = "";
            ViewBag.Messages = messageFactory.Getmessages();
            ViewBag.first_name = (string)HttpContext.Session.GetString("name");
            System.Console.WriteLine((string)HttpContext.Session.GetString("name"));
            ViewBag.Comments = messageFactory.Getcomments();
            return View("wall");
        }
        [HttpPost]
        [Route("submit_message")]
        public IActionResult addMessage(Message newMessage)
        {
            if(ModelState.IsValid)
            {
                long user = (long)HttpContext.Session.GetInt32("id");
                messageFactory.Add(newMessage, user);
            }
            ViewBag.Errors = ModelState.Values;
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("submit_comment/{messageId}")]
        public IActionResult addComment(Comment newComment, long messageId)
        {
            if(ModelState.IsValid)
            {
                long user = (long)HttpContext.Session.GetInt32("id");
                messageFactory.AddComment(newComment, user, messageId);
            }
            ViewBag.Errors = ModelState.Values;
            return RedirectToAction("Index");
        }
    }
    
}