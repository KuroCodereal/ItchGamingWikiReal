using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ItchGamingWiki.ItchCommon;
using ItchGamingWiki.Models;
using Microsoft.Owin.Security.Provider;
namespace ItchGamingWiki.Authorization
{
    public class AuthorizeAccessRole: AuthorizeAttribute,IAuthorizationFilter
    {
        //Kiem tra xem co quyen truy cap chuc nang hien tai khong
        public int Level = UserLevel.Default;

        //loai xu li du lieu tuong ung voi level: View, Edit, Add, Print, Import, Export
        public string TypeHandle = string.Empty;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            User us = filterContext.HttpContext.Session["GET_USER"] as User;
            if(us == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "action", "Logout" }, { "controller", "LogOut" } });
                return;
            }
            ItchGamingWikiEntities db = new ItchGamingWikiEntities();
            SYSUser getUser = db.SYSUsers.Find(us.UserId);
            if(getUser == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "action", "Logout" }, { "controller", "LogOut" } });
                return;
            }
            if (us.LevelCV == UserLevel.Admin) return;
            if(this.Level < UserLevel.Default || (this.Level != UserLevel.Default && us.LevelCV > this.Level))
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "action", "Notification" }, { "controller", "Redirect" } });
                return;
            }
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            string actionName = routeValues["action"].ToString();
            string controllerName = routeValues["controller"].ToString();
            controllerName = (controllerName == "SYSGroup" || controllerName == "SYSUserRole" ? "SYSUser" : controllerName);
            //Kiem tra quyen xu ly du lieu: xem, sua, them, xoa, ...
            SYSMenu menuSelected = db.SYSMenus.Where(x => x.Controller == controllerName && x.ParentId > 0 /*&& x.Action == actionName*/).FirstOrDefault();
            if (menuSelected == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "action", "Notification" }, { "controller", "Redirect" } });
                return;
            }
            if (string.IsNullOrEmpty(this.TypeHandle))
            {
                return;
            }
            bool HasRoleAccess = HasConfirmRoleRequest(menuSelected,us);
            if(!HasRoleAccess)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "action", "Notification" }, { "controller", "Redirect" } });
                return;
            }
            //base.OnAuthorization(filterContext);
        }
        bool HasConfirmRoleRequest(SYSMenu menuSelected, User us)
        {
            if (string.IsNullOrEmpty(this.TypeHandle)) return true;
            this.TypeHandle = this.TypeHandle.ToLower();
            char OR = '|', AND = ',';
            if(!this.TypeHandle.Contains("|") && this.TypeHandle.Contains(","))
            {
                return confirmRoleAccessMenu(this.TypeHandle, menuSelected, us);
            }
            bool bResult = false;
            List<string> LstRoleOR = this.TypeHandle.Split(OR).ToList();
            if(LstRoleOR.Count > 1)
            {
                foreach(var itemRole in LstRoleOR)
                {
                    bResult = confirmRoleAccessMenu(itemRole,menuSelected, us);
                    if (bResult) break;
                }
                return bResult;
            }
            bResult = true;
            List<string> LstRoleAND = this.TypeHandle.Split(AND).ToList();
            foreach(var itemRole in LstRoleAND)
            {
                bool bSubResult = confirmRoleAccessMenu(itemRole, menuSelected, us);
                if (!bResult)
                {
                    bResult = false;
                    break;
                }
            }
            return bResult;
        }

        bool confirmRoleAccessMenu(string TypeHandle, SYSMenu menuSelected, User us)
        {
            //SYSGroupRole itemRoleGroup = us.listGroupRoleMenu.Where(x => x.GroupId == us.GroupId && x.MenuId == menuSelected.MenuId);
            //SYSUserRole itemRoleUser = us.listUserRoleMenu.Where(x => x.GroupId == us.GroupId && x.MenuId == menuSelected.MenuId).FirstOrDefault();
            SYSGroupRole itemRoleGroup = us.listGroupRoleMenu.Where(x => x.MenuId == menuSelected.MenuId).FirstOrDefault();
            SYSUserRole itemRoleUser = us.listUserRoleMenu.Where(x => x.UserId == us.UserId && x.MenuId == menuSelected.MenuId).FirstOrDefault();
            if (itemRoleGroup == null && itemRoleUser == null) return false;
            if(TypeHandle == "view")
            {
                if (itemRoleUser != null) return itemRoleUser.RoleView == KEYWORD_PAGE.Yes;
                return itemRoleGroup.RoleView == KEYWORD_PAGE.Yes;
            }
            if (TypeHandle == "add")
            {
                if (itemRoleUser != null) return itemRoleUser.RoleView == KEYWORD_PAGE.Yes;
                return itemRoleGroup.RoleAdd == KEYWORD_PAGE.Yes;
            }
            if (TypeHandle == "edit")
            {
                if (itemRoleUser != null) return itemRoleUser.RoleView == KEYWORD_PAGE.Yes;
                return itemRoleGroup.RoleEdit == KEYWORD_PAGE.Yes;
            }
            if (TypeHandle == "delete")
            {
                if (itemRoleUser != null) return itemRoleUser.RoleView == KEYWORD_PAGE.Yes;
                return itemRoleGroup.RoleDelete == KEYWORD_PAGE.Yes;
            }
            if (TypeHandle == "import")
            {
                if (itemRoleUser != null) return itemRoleUser.RoleView == KEYWORD_PAGE.Yes;
                return itemRoleGroup.RoleImport == KEYWORD_PAGE.Yes;
            }
            if (TypeHandle == "export")
            {
                if (itemRoleUser != null) return itemRoleUser.RoleView == KEYWORD_PAGE.Yes;
                return itemRoleGroup.RoleExport == KEYWORD_PAGE.Yes;
            }
            if (TypeHandle == "print")
            {
                if (itemRoleUser != null) return itemRoleUser.RoleView == KEYWORD_PAGE.Yes;
                return itemRoleGroup.RolePrint == KEYWORD_PAGE.Yes;
            }
            //if (TypeHandle == "role1")
            //{
            //    if (itemRoleUser != null) return itemRoleUser.RoleView == KEYWORD_PAGE.Yes;
            //    return itemRoleGroup.RoleView == KEYWORD_PAGE.Yes;
            //}
            return false;
        }
    }
}