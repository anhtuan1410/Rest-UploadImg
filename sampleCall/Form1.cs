using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sampleCall
{
    public partial class Form1 : Form
    {
        string global_host = "http://mapi.wellspringsg.khachhang.me:8089/api/app";//link api call api
        List<string> lstImg = new List<string>
        {
            { "57462603-3405-4d32-8d15-a2279524156d" },
            { "35c710ff-2fef-4e96-a0b3-1a7883b611ca" },
            { "2dfa8ba9-e1eb-4146-a3fd-597b20d45d4d" },
            { "1427c804-a7f4-4621-9aac-7796ba0cbc2e" }
        };

        List<string> lstBaiViet = new List<string>
        {
            { "Bài viết về nhà giáo việt nam" },
            { "Bài viết về phong cảnh" },
            { "Bài viết về sách giáo khoa cả cách" },
            { "Bài viết về học ở thư viện" },
            { "Bài viết về lòng tốt" },
            { "Bài viết về tấm gương đẹp" },
        };

        List<string> lstComment = new List<string>
        {
            { "Hay lắm" },
            { "Tuyệt vời" },
            { "Rất ý nghĩa" },
            { "Các bạn học sinh nên đọc bài này" },
            { "1 like cho tác giả" },
            { "Đáng ngưỡng mộ" },
        };

        DateTime dtGlobal = DateTime.Now;

        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                LoginResponse _login = Call_Api_Login();
                if (_login != null)
                {
                    UserInfoResponse _userInfo = Call_Api_UserInfo(_login);
                    if (_userInfo != null)
                    {

                        //Call_Api_UploadImage(_login);

                        CallUpload();

                        return;

                        for (int i = 0; i < 31; i++)
                        {
                            Thread.Sleep(15000);

                            //Call_Api_UploadImage(_login);
                            AddPostResponse _post = Call_Api_Add_Post(_login, _userInfo, lstImg);
                            if (_post != null)
                            {
                                List<AddCommentResponse> lstComment = new List<AddCommentResponse>();
                                int _demComment = 0;
                                while (_demComment < 5)
                                {
                                    _demComment++;
                                    Thread.Sleep(5000);

                                    AddCommentResponse _comment = Call_Api_Add_Comment(_login, _userInfo, _post);

                                    if (_comment != null)
                                    {

                                        lstComment.Add(_comment);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Add Comment failed");
                                        Application.Exit();
                                    }

                                }

                                //ghi log all
                                var _wellSpring = new
                                {
                                    Login = _login,
                                    UserInfo = _userInfo,
                                    Post = _post,
                                    Comments = lstComment
                                };

                                WriteLogByFunction(JsonConvert.SerializeObject(_wellSpring));

                            }
                            else
                            {
                                MessageBox.Show("Add Post failed");
                                Application.Exit();
                            }

                        }

                    }
                    else
                    {
                        MessageBox.Show("Userinfo failed");
                        Application.Exit();
                    }
                }
                else
                {
                    MessageBox.Show("Login failed");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ErrorApp: " + ex.Message);
            }
        }

        LoginResponse Call_Api_Login()
        {
            try
            {

                var _obj = new
                {
                    userName = "GVCN6.1A@gmail.com",
                    password = "TestGVCN01"
                };

                var client = new RestClient(global_host + "/authen/login/5df796fc-7676-49c3-b5c9-325fb81e3e3b");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "text/plain");
                request.AddHeader("Token", "aaaaa");
                request.AddParameter("undefined", JsonConvert.SerializeObject(_obj), ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var _logObj = new
                    {
                        request = JsonConvert.SerializeObject(request),
                        response = JsonConvert.SerializeObject(response)
                    };
                    WriteLogByFunction(JsonConvert.SerializeObject(_logObj), "ErrorLogin");
                    int _d = 0;
                    return null;
                }
                else
                {
                    int _k = 0;

                    LoginResponse _log = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
                    return _log;
                }

            }
            catch (Exception ex)
            {
                var _logObj = new
                {
                    Exception = ex.Message
                };
                WriteLogByFunction(JsonConvert.SerializeObject(_logObj), "ErrorLogin");

                return null;
            }
        }

        UserInfoResponse Call_Api_UserInfo(LoginResponse p)
        {
            try
            {
                var client = new RestClient(global_host + "/user/userProfile/" + p.userInfo.authenUserId);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "text/plain");
                request.AddHeader("AuthenUserId", p.userInfo.id.ToString());
                request.AddHeader("UserName", p.userInfo.email.ToString().Split('@')[0]);
                request.AddHeader("BranchId", p.userInfo.branchId.ToString());
                request.AddHeader("SchoolYearId", p.userInfo.schoolYearId.ToString());
                request.AddHeader("Authorization", "Bearer " + p.token);

                request.AddParameter("undefined", null, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var _logObj = new
                    {
                        request = JsonConvert.SerializeObject(request),
                        response = JsonConvert.SerializeObject(response)
                    };
                    WriteLogByFunction(JsonConvert.SerializeObject(_logObj), "ErrorUserInfo");

                    int _d = 0;
                    return null;
                }
                else
                {
                    int _k = 0;

                    UserInfoResponse _log = JsonConvert.DeserializeObject<UserInfoResponse>(response.Content);
                    return _log;
                }

            }
            catch (Exception ex)
            {
                var _logObj = new
                {
                    Exception = ex.Message
                };
                WriteLogByFunction(JsonConvert.SerializeObject(_logObj), "ErrorUserInfo");
                return null;
            }
        }


        object Call_Api_UploadImage(LoginResponse p)
        {
            try
            {


                var client = new RestClient(global_host + "/Image");
                var request = new RestRequest(Method.POST);
                //request.AlwaysMultipartFormData = true;
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddHeader("Accept", "application/json");

                #region OFF CODE STREAM

                //request.AddHeader("Authorization", "Bearer " + p.token);
                //
                //request.AddFile("fileData", openFileDialog1.FileNames[0], "bb.txt");
                //request.AddParameter("undefined", openFileDialog1.FileName, ParameterType.RequestBody);
                //request.AddFile()


                //        var files = new Dictionary<string, byte[]>
                //{
                //    { "testfile", File.ReadAllBytes(openFileDialog1.FileNames[0]) }
                //};
                //        request.AddParameter("files", files, RestSharp.ParameterType.RequestBody);

                //var fileName = Path.GetFileName(openFileDialog1.FileNames[0]);
                //byte[] data = File.ReadAllBytes(openFileDialog1.FileNames[0]);
                //using (BinaryReader reader = new BinaryReader(file.InputStream))
                //{
                //    data = reader.ReadBytes((int)file.InputStream.Length);
                //}
                //Stream newStream = new MemoryStream(data);
                //request.Files.Add(new FileParameter
                //{
                //    Name = "file",
                //    Writer = (s) =>
                //    {
                //        newStream.CopyTo(s);
                //    },
                //    FileName = fileName,
                //    // ContentType = "text/csv",
                //    ContentLength = newStream.Length
                //});
                // Stream s;

                //request.AddFile("files", fileName.CopyTo(), fileName);


                //request.AddFileBytes("file", data, fileName, "application/octet-stream");

                #endregion

                string filevalue = System.Convert.ToBase64String(file_get_byte_contents(@"D:\autumn.png"));
                //request.AddParameter("file", filevalue);
                request.AddFile("file", @"D:\autumn.png");

                IRestResponse response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    //Ulities.WriteLogByFunction(
                    //    "ThanhToan_UpdateFails_SSC" + DateTime.Now.ToString("yyyy_MM_dd"),
                    //    "\r\nRequest: " + JsonConvert.SerializeObject(request) +
                    //    "\r\nObjectResult: " + JsonConvert.SerializeObject(response),
                    //    "2821",
                    //    "ThanhToan_UpdateFails_SSC"
                    //    );

                    //result.success = false;
                    //result.data = null;
                    //result.error = new ErrorModel("Lỗi gọi API FindBill!", response.ErrorMessage);
                    //return result;
                    int _d = 0;
                    return null;
                }
                else
                {
                    int _k = 0;

                    UserInfoResponse _log = JsonConvert.DeserializeObject<UserInfoResponse>(response.Content);
                    return _log;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        void CallUpload()
        {
            UploadFileAsync("asdasdasd", @"D:\autumn.png", "aaa");
        }

        //upload image bằng form data
        public async Task UploadFileAsync(string token, string path, string channels)
        {
            // send request to API
            var url = global_host + "/Image";

            // we need to send a request with multipart/form-data
            var multiForm = new MultipartFormDataContent();

            // add API method parameters
            multiForm.Add(new StringContent(token), "token");
            multiForm.Add(new StringContent(channels), "channels");

            // add file and directly upload it
            FileStream fs = File.OpenRead(path);
            multiForm.Add(new StreamContent(fs), "file", Path.GetFileName(path));

            var client = new HttpClient();

            //cách 1 
            //var response = await client.PostAsync(url, multiForm);

            //cách 2
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Headers = {
            { HttpRequestHeader.Authorization.ToString(), "Bearer 123123123" },
            { HttpRequestHeader.Accept.ToString(), "application/json" },
            { HttpRequestHeader.ContentType.ToString(), "multipart/form-data" },

        },
                Content = multiForm
            };                      

            var response = await client.SendAsync(httpRequestMessage);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Upload image failed");
            }
            else
            {
                var _obj = response.Content.ReadAsStringAsync().Result;
                MessageBox.Show(_obj.ToString());
                int _p = 0;
            }

        }



        //Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec at https://tools.ietf.org/html/rfc2046#section-5.1 states that 70 characters is a reasonable limit.
        //public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        //{
        //    var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

        //    if (string.IsNullOrWhiteSpace(boundary))
        //    {
        //        throw new InvalidDataException("Missing content-type boundary.");
        //    }

        //    if (boundary.Length > lengthLimit)
        //    {
        //        throw new InvalidDataException(
        //            $"Multipart boundary length limit {lengthLimit} exceeded.");
        //    }

        //    return boundary;
        //}

        static byte[] file_get_byte_contents(string fileName)
        {
            byte[] sContents;
            if (fileName.ToLower().IndexOf("http:") > -1)
            {
                // URL 
                System.Net.WebClient wc = new System.Net.WebClient();
                sContents = wc.DownloadData(fileName);
            }
            else
            {
                // Get file size
                FileInfo fi = new FileInfo(fileName);

                // Disk
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                sContents = br.ReadBytes((int)fi.Length);
                br.Close();
                fs.Close();
            }

            return sContents;
        }


        AddPostResponse Call_Api_Add_Post(LoginResponse p, UserInfoResponse _userInfo, List<string> imagesId)
        {
            try
            {
                dtGlobal = dtGlobal.AddDays(-1);
                var _body = new
                {
                    content = lstBaiViet[(new Random()).Next(0, lstBaiViet.Count - 1)] + " " + dtGlobal.ToString("dd/MM/yyyy"),
                    imageFileIds = new List<string>{
                          { imagesId[(new Random()).Next(0, imagesId.Count - 1)]
                          }
                      },
                    publishingClassIds = new List<string> {
                        { _userInfo.classes[ (new Random()).Next(0, _userInfo.classes.Count - 1) ].classId.ToString() }
                      }
                }
            ;

                var client = new RestClient(global_host + "/post");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "text/plain");
                request.AddHeader("AuthenUserId", p.userInfo.id.ToString());
                request.AddHeader("UserName", p.userInfo.email.ToString().Split('@')[0]);
                request.AddHeader("BranchId", p.userInfo.branchId.ToString());
                request.AddHeader("SchoolYearId", p.userInfo.schoolYearId.ToString());
                request.AddHeader("Authorization", "Bearer " + p.token);

                request.AddParameter("undefined", JsonConvert.SerializeObject(_body), ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var _logObj = new
                    {
                        request = JsonConvert.SerializeObject(request),
                        response = JsonConvert.SerializeObject(response)
                    };
                    WriteLogByFunction(JsonConvert.SerializeObject(_logObj), "ErrorAddPost");

                    int _d = 0;
                    return null;
                }
                else
                {
                    int _k = 0;

                    AddPostResponse _log = JsonConvert.DeserializeObject<AddPostResponse>(response.Content);
                    return _log;
                }

            }
            catch (Exception ex)
            {
                var _logObj = new
                {
                    Exception = ex.Message
                };
                WriteLogByFunction(JsonConvert.SerializeObject(_logObj), "ErrorAddPost");
                return null;
            }
        }

        List<AddPostResponse> Call_Api_Get_List_Post(LoginResponse p, UserInfoResponse _userInfo, List<string> imagesId)
        {
            try
            {

                var client = new RestClient(global_host + "/post?PageIndex=1&PageSize=10");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "text/plain");
                request.AddHeader("AuthenUserId", p.userInfo.id.ToString());
                request.AddHeader("UserName", p.userInfo.email.ToString().Split('@')[0]);
                request.AddHeader("BranchId", p.userInfo.branchId.ToString());
                request.AddHeader("SchoolYearId", p.userInfo.schoolYearId.ToString());
                request.AddHeader("Authorization", "Bearer " + p.token);

                request.AddParameter("undefined", null, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    //Ulities.WriteLogByFunction(
                    //    "ThanhToan_UpdateFails_SSC" + DateTime.Now.ToString("yyyy_MM_dd"),
                    //    "\r\nRequest: " + JsonConvert.SerializeObject(request) +
                    //    "\r\nObjectResult: " + JsonConvert.SerializeObject(response),
                    //    "2821",
                    //    "ThanhToan_UpdateFails_SSC"
                    //    );

                    //result.success = false;
                    //result.data = null;
                    //result.error = new ErrorModel("Lỗi gọi API FindBill!", response.ErrorMessage);
                    //return result;
                    int _d = 0;
                    return null;
                }
                else
                {
                    int _k = 0;

                    List<AddPostResponse> _log = JsonConvert.DeserializeObject<List<AddPostResponse>>(response.Content);
                    return _log;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        AddCommentResponse Call_Api_Add_Comment(LoginResponse p, UserInfoResponse _userInfo, AddPostResponse post)
        {
            try
            {
                var _body = new
                {
                    postId = post.id,
                    body = lstComment[(new Random()).Next(0, lstComment.Count - 1)] + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff")
                };
                ;

                var client = new RestClient(global_host + "/postComment");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "text/plain");
                request.AddHeader("AuthenUserId", p.userInfo.id.ToString());
                request.AddHeader("UserName", p.userInfo.email.ToString().Split('@')[0]);
                request.AddHeader("BranchId", p.userInfo.branchId.ToString());
                request.AddHeader("SchoolYearId", p.userInfo.schoolYearId.ToString());
                request.AddHeader("Authorization", "Bearer " + p.token);

                request.AddParameter("undefined", JsonConvert.SerializeObject(_body), ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var _logObj = new
                    {
                        request = JsonConvert.SerializeObject(request),
                        response = JsonConvert.SerializeObject(response)
                    };
                    WriteLogByFunction(JsonConvert.SerializeObject(_logObj), "ErrorAddComment");
                    int _d = 0;
                    return null;
                }
                else
                {
                    int _k = 0;

                    AddCommentResponse _log = JsonConvert.DeserializeObject<AddCommentResponse>(response.Content);
                    return _log;
                }

            }
            catch (Exception ex)
            {
                var _logObj = new
                {
                    Exception = ex.Message
                };
                WriteLogByFunction(JsonConvert.SerializeObject(_logObj), "ErrorAddComment");
                return null;
            }
        }


        public static void WriteLogByFunction(string pContent, string fileName = "wellSpringAuto")
        {
            StreamWriter w;
            {
                string fullpath_filename = @"D:\" + fileName + ".txt";

                if (!File.Exists(fullpath_filename))
                {
                    w = File.CreateText(fullpath_filename);
                }
                else w = File.AppendText(fullpath_filename);
                try
                {
                    w.WriteLine("\r\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    w.WriteLine("\r\n" + pContent);
                    w.Flush();
                    w.Close();
                }
                catch (Exception ex)
                {
                    w.Flush();
                    w.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();
        }

    }

}
