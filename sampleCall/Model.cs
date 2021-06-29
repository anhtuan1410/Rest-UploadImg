using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sampleCall
{
    class Model
    {
    }

    public class LoginResponse
    {
        public string token { get; set; }
        public Login_UserInfo userInfo { get; set; }
    }

    public class Login_UserInfo
    {
        public object id { get; set; }
        public object fullName { get; set; }
        public object yearOfBirth { get; set; }
        public object email { get; set; }
        public object primaryPhoneNumber { get; set; }
        public List<object> imageData { get; set; }
        public object avatar { get; set; }
        public object type { get; set; }
        public object gender { get; set; }
        public object nationality { get; set; }
        public object classes { get; set; }
        public object authenUserId { get; set; }
        public object isSuperAdmin { get; set; }
        public List<string> gradeIds { get; set; }
        public object academicStartDate { get; set; }
        public object academicEndDate { get; set; }
        public object branchId { get; set; }
        public object schoolYearId { get; set; }
    }



    public class UserInfoResponse
    {
        public object id { get; set; }
        public object fullName { get; set; }
        public object yearOfBirth { get; set; }
        public object email { get; set; }
        public object primaryPhoneNumber { get; set; }
        public object imageData { get; set; }
        public object avatar { get; set; }
        public object type { get; set; }
        public object gender { get; set; }
        public object nationality { get; set; }
        public List<ClasseseUI> classes { get; set; }
        public object authenUserId { get; set; }
        public object isSuperAdmin { get; set; }
        public List<string> gradeIds { get; set; }
        public object academicStartDate { get; set; }
        public object academicEndDate { get; set; }
        public object branchId { get; set; }
        public object schoolYearId { get; set; }
    }

    public class ClasseseUI
    {
        public object classRoom { get; set; }
        public object total { get; set; }
        public object isHomeRoom { get; set; }
        public object classId { get; set; }
        public object gradeId { get; set; }
        public object gradeName { get; set; }
        public object totalSection { get; set; }
    }

    public class AddPostResponse
    {
        public object className { get; set; }
        public object content { get; set; }
        public object creatorFullName { get; set; }
        public object creationTime { get; set; }
        public object imageUrls { get; set; }
        public object comments { get; set; }
        public object canEdit { get; set; }
        public object likeCounts { get; set; }
        public object commentCounts { get; set; }
        public object creatorId { get; set; }
        public object id { get; set; }
    }

    public class AddCommentResponse
    {
        public object body { get; set; }
        public object creatorFullName { get; set; }
        public object createdDate { get; set; }
        public object id { get; set; }
    }



}
