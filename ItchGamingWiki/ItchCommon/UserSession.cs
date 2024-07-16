using ItchGamingWiki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ItchGamingLibrary.User;

namespace ItchGamingWiki.ItchCommon
{
    public class User : IGWUser
    {
        public List<SYSGroupRole> listGroupRoleMenu {  get; set; }
        public List<SYSUserRole> listUserRoleMenu { get; set; }
        public List<SYSMenu> listMenu { get; set; }
        public string SecurityStamp {  get; private set; }
        public User() {
            this.listGroupRoleMenu = new List<SYSGroupRole>();
            this.listUserRoleMenu = new List<SYSUserRole>();
            this.listMenu = new List<SYSMenu>();
            
            //this.SecurityStamp = Guid.NewGuid().ToString();
        }

        public User(Guid _UserID, string _UserName, string _FullName, int _Level, string _Email)
        {
            this.UserId = _UserID;
            this.UserName = _UserName;
            this.FullName = _FullName;
            this.Level = _Level;
            this.Email = _Email;
            this.listGroupRoleMenu = new List<SYSGroupRole>();
            this.listUserRoleMenu = new List<SYSUserRole>();
            this.listMenu = new List<SYSMenu>();
        }

        public User(SYSUser us, ItchGamingWikiEntities entity)
        {
            this.UserId = us.UserId;
            this.UserName = us.Username;
            this.FullName = us.FullName;
            this.Level = us.Level;
            this.LevelText = us.Level > 0 ? entity.Positions.FirstOrDefault(x => x.Position_Id == us.Level)?.PositionName : "Quản trị viên";
            this.Email = us.Email;
            this.Phone = us.Phone;
            this.Address = us.Address;
            this.Description = us.Description;
            this.listMenu = entity.SYSMenus.Where(x => x.Enable == KEYWORD_PAGE.Yes).OrderBy(x => x.MenuId).ToList();
            //this.LevelCV = us.Level > 0 ? entity.Positions.FirstOrDefault(x => x.Position_Id == us.Level)?.LevelCV : 0;
            List<SYSGroupUser> lstGroupUser = entity.SYSGroupUsers.Where(x => x.UserId == us.UserId).ToList();
            //if (lstGroupUser.Count > 0)
            //{

            //}
            
        }
        private List<SYSGroupRole> getListGroupRole(List<Guid> LstGroupId,ItchGamingWikiEntities db)
        {
            List<SYSGroupRole> lstResult = new List<SYSGroupRole>();
            foreach (Guid id in LstGroupId)
            {
                SYSGroupGame getGroup = db.SYSGroupGames.Find(id);
                if (getGroup == null) continue;
                if (getGroup.Enable == KEYWORD_PAGE.No) continue;
                List<SYSGroupRole> lstChild = db.SYSGroupRoles.Where(x => x.GroupId == id).ToList();
                if (lstChild.Count == 0) continue;
                if(lstResult.Count == 0)
                {
                    lstResult.AddRange(lstChild);
                    continue;
                }

                foreach (var item in lstChild)
                {
                    SYSGroupRole Exsist = lstResult.Where(x => x.MenuId == item.MenuId).FirstOrDefault();
                    if (Exsist == null)
                    {
                        lstResult.Add(item);
                        continue;
                    }
                    int index = lstResult.IndexOf(Exsist);

                    lstResult[index].RoleView = (item.RoleView == KEYWORD_PAGE.Yes ? item.RoleView : Exsist.RoleView);
                    lstResult[index].RoleAdd = (item.RoleAdd == KEYWORD_PAGE.Yes ? item.RoleAdd : Exsist.RoleAdd);
                    lstResult[index].RoleEdit = (item.RoleEdit == KEYWORD_PAGE.Yes ? item.RoleEdit : Exsist.RoleEdit);
                    lstResult[index].RoleDelete = (item.RoleDelete == KEYWORD_PAGE.Yes ? item.RoleDelete : Exsist.RoleDelete);
                    lstResult[index].RoleImport = (item.RoleImport == KEYWORD_PAGE.Yes ? item.RoleImport: Exsist.RoleImport);
                    lstResult[index].RoleExport = (item.RoleExport == KEYWORD_PAGE.Yes ? item.RoleExport : Exsist.RoleExport);
                    lstResult[index].RolePrint = (item.RolePrint == KEYWORD_PAGE.Yes ? item.RolePrint: Exsist.RolePrint);
                }
            }
            return new HashSet<SYSGroupRole>(lstResult).ToList();
        }

        public List<SYSUserRole> GetListUserRole(List<Guid> LstGroupId, Guid UserId, ItchGamingWikiEntities db)
        {
            List<SYSUserRole> lstResult = new List<SYSUserRole>();
            foreach(Guid Id in LstGroupId)
            {
                SYSGroupGame getGroup = db.SYSGroupGames.Find(Id);
                if (getGroup == null) continue;
                if (getGroup.Enable == KEYWORD_PAGE.No) continue;
                List<SYSUserRole> lstChild = db.SYSUserRoles.Where(x => x.GroupId == Id && x.UserId == UserId).ToList();
                if(lstChild.Count == 0) continue;
                if (lstResult.Count == 0)
                {
                    lstResult.AddRange(lstChild);
                    continue;
                }
                foreach (var item in lstChild)
                {
                    SYSUserRole Exsist = lstResult.Where(x => x.UserId == UserId && x.MenuId == item.MenuId).FirstOrDefault();
                    if (Exsist == null)
                    {
                        lstResult.Add(item);
                        continue;
                    }
                    int index = lstResult.IndexOf(Exsist);

                    lstResult[index].RoleView = (item.RoleView == KEYWORD_PAGE.Yes ? item.RoleView : Exsist.RoleView);
                    lstResult[index].RoleAdd = (item.RoleAdd == KEYWORD_PAGE.Yes ? item.RoleAdd : Exsist.RoleAdd);
                    lstResult[index].RoleEdit = (item.RoleEdit == KEYWORD_PAGE.Yes ? item.RoleEdit : Exsist.RoleEdit);
                    lstResult[index].RoleDelete = (item.RoleDelete == KEYWORD_PAGE.Yes ? item.RoleDelete : Exsist.RoleDelete);
                    lstResult[index].RoleImport = (item.RoleImport == KEYWORD_PAGE.Yes ? item.RoleImport : Exsist.RoleImport);
                    lstResult[index].RoleExport = (item.RoleExport == KEYWORD_PAGE.Yes ? item.RoleExport : Exsist.RoleExport);
                    lstResult[index].RolePrint = (item.RolePrint == KEYWORD_PAGE.Yes ? item.RolePrint : Exsist.RolePrint);
                }
            }
            return new HashSet<SYSUserRole>(lstResult).ToList();
        }
    }
    public class UserLevel : IGWUserLevel
    {
        public static string GetLevelUserDescription(int level = KEYWORD_PAGE.Default)
        {
            if (level == Admin) return "Quản trị viên";
            if (level == User) return "User";
            if (level == Dev) return "Dev";
            return "Khác";
        }

        public static string GetLevelUserDescription(SYSUser us)
        {
            var user = new User(us,new ItchGamingWikiEntities());
            //Khong phai null thi tra ve user.LevelCV neu null tra ve -1
            int level = user.LevelCV ?? -1;
            if (level == Admin) return "Quản trị viên";
            if (level == User) return "User";
            if (level == Dev) return "Dev";
            return "Khác";
        }
    }
}