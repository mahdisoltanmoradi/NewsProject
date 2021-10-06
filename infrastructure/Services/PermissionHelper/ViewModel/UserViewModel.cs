using AutoMapper;
using DataLayer.Entities.Users;
using infrastructure.WebFramework.CustomMapping;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs.User
{

    public class UserViewModel
    {

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "{0} نباید خالی باشد")]
        [StringLength(50, ErrorMessage = "{0} نباید بیشتر از 50 کاراکتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "{0} نباید خالی باشد")]
        public string Password { get; set; }

        [Display(Name = "نقش ها")]
        public List<int> SelectedRoles { get; set; }

        [Display(Name = "نقش ها")]
        public string Roles { get; set; }

        public List<Role> AllRoles { get; set; }


        public SelectList GetRolesSelectList()
        {
            return new SelectList(AllRoles, nameof(Role.Id), nameof(Role.Name));
        }
        //public void CreateMappings(Profile profile)
        //{
        //    profile.CreateMap<Identity, DataLayer.Entities.Users.User>()
        //           .ReverseMap()
        //           .ForMember(p => p.SelectedRoles, p => p.MapFrom(q => q.UserRoles.Select(x => x.RoleId).ToList()))
        //           .ForMember(p => p.Roles, p => p.MapFrom(q => string.Join(", ", q.UserRoles.Select(x => x.Role.Name))));
        //}

       
    }
}
