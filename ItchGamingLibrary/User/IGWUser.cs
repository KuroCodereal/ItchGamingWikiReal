using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItchGamingLibrary.User
{
    public class IGWUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int Level {  get; set; }
        public string Email { get; set; }
        public string LevelText {  get; set; }
        public string Phone {  get; set; }
        public string Address {  get; set; }
        public string Description {  get; set; }
        public string Token {  get; set; }
        public int IsMenuVertical { get; set; } = 0;
        public int IsActiveInternet { get; set; } = 0;
        public int? LevelCV { get; set; }
    }
}
