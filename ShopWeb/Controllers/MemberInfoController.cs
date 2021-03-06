﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopWeb.Models;

namespace ShopWeb.Controllers
{
    public class MemberInfoController : Controller
    {
        // GET: MemberInfo
        public ActionResult Index()
        {
            if (Session["has_login"] == null)
            {
                if (Session["ReturnToMemberInfo"] == null) Session["ReturnToMemberInfo"] = "true";
                else Session["ReturnToMemberInfo"] = null;
                return Redirect("/Login");
            }
            else
            {
                Session.Remove("ReturnToMemberInfo");
                return View();
            }
        }

        //用于在用户信息页修改信息
        [HttpPost]
        public ActionResult Index(MemberModifyInfo memberModifyInfo)
        {
            ShopBusinessLogic.LoginMember loginMember = new ShopBusinessLogic.LoginMember();
            string mem_phone = Session["mem_phone"].ToString();
            string new_pwd = memberModifyInfo.new_mem_pwd;
            string new_name = memberModifyInfo.new_mem_name;
            if (new_pwd != null)
            {
                loginMember.ModifyMemberPwd(mem_phone, new_pwd);
                Session["mem_pwd"] = new_pwd;
            }
            if (new_name != null)
            {
                loginMember.ModifyMemberName(mem_phone, new_name);
                Session["mem_name"] = new_name;
            }
            return Redirect("/MemberInfo");
        }

        [HttpGet]
        public ActionResult Validate(string old_mem_pwd)
        {
            string phone = Session["mem_phone"].ToString();
            ShopBusinessLogic.LoginMember loginMember = new ShopBusinessLogic.LoginMember();
            string true_pwd = loginMember.GetMemberByPhone(phone).mem_pwd;
            return Json(old_mem_pwd == true_pwd, JsonRequestBehavior.AllowGet);
        }
    }
}