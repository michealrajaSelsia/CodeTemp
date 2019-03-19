using ServiceProvider.Models;
using ServiceProvider.Models.University;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using ServiceProvider.Models.Common;
using System.Web;
using System.IO;
using Microsoft.Owin;
using System.Web.Http.Description;

namespace ServiceProvider.Controllers.University
{


    public class CommonController : ApiController
    {
        // SqlConnectionStringBuilder sqlString =
        string sqlString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        SqlDataAdapter da = new SqlDataAdapter();
        SqlDataReader dr;
        CommonController()
        {
            //sqlString.DataSource = "DESKTOP-F231U8O\\MICHEALKING";
            //sqlString.InitialCatalog = "University";
            //sqlString.IntegratedSecurity = true;
        }

        //http://localhost:49728/api/Common/GetData?tblName=GetCourses
        //http://localhost:49728/api/Common/GetData?tblName=GetApplication
        //http://localhost:49728/api/Common/GetData?tblName=GetApplicationStatus
        //http://localhost:49728/api/Common/GetData?tblName=GetCourses
        //http://localhost:49728/api/Common/GetData?tblName=GetReligion
        //http://localhost:49728/api/Common/GetData?tblName=GetUserDetails

        [HttpGet]
        public IHttpActionResult GetData(string tblName)

        {
            DataSet ds = new DataSet("TimeRanges");
            try
            {
                using (SqlConnection con = new SqlConnection(sqlString.ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(tblName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //con.Open();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);

                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed" + ex.Message.ToString());
            }
            finally
            {

            }
            return Ok(ds);
        }

        [ResponseType(typeof(Application))]
        [HttpPost]
        public IHttpActionResult GetApplication(string email="")
        {
            DataSet ds = new DataSet("TimeRanges");
                List<Application> applist = new List<Application>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlString.ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GetApplication", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@emailid", email);
                        //con.Open();
                        SqlDataAdapter da = new SqlDataAdapter();                        
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            applist.Add(
                                new Application
                                {
                                    ApplicationID = Convert.ToString(dr["ApplicationID"]),
                                    Course = Convert.ToString(dr["Course"]),
                                    Fname = Convert.ToString(dr["Fname"]),
                                    Lname = Convert.ToString(dr["Lname"]),
                                    Fullname = Convert.ToString(dr["Fullname"]),

                                    DOB = Convert.ToDateTime(dr["DOB"]),
                                    Gender = Convert.ToBoolean(dr["Gender"]),
                                    FatherName = Convert.ToString(dr["FatherName"]),
                                    MotherName = Convert.ToString(dr["MotherName"]),
                                    Religion = Convert.ToString(dr["Religion"]),
                                    Nationality = Convert.ToString(dr["Nationality"]),
                                    Email = Convert.ToString(dr["Email"]),
                                    MobileNo = Convert.ToInt32(dr["MobileNo"]),
                                    P_Address = Convert.ToString(dr["P_Address"]),
                                    C_Address = Convert.ToString(dr["C_Address"]),
                                    photo = Convert.ToString(dr["photo"]),

                                    SSLCSchoolName = Convert.ToString(dr["SSLCSchoolName"]),
                                    SSLCMark = Convert.ToInt32(dr["SSLCMark"]),
                                    SSLCPercentage = Convert.ToInt32(dr["SSLCPercentage"]),
                                    HSCSchoolName = Convert.ToString(dr["HSCSchoolName"]),
                                    HSCMark = Convert.ToInt32(dr["HSCMark"]),
                                    HSCPercentage = Convert.ToInt32(dr["HSCPercentage"]),
                                    UGSchoolName = Convert.ToString(dr["UGSchoolName"]),
                                    UGMark = Convert.ToInt32(dr["UGMark"]),
                                    UGPercentage = Convert.ToInt32(dr["UGPercentage"]),
                                    PGSchoolName = Convert.ToString(dr["PGSchoolName"]),
                                    PGMark = Convert.ToInt32(dr["PGMark"]),
                                    PGPercentage = Convert.ToInt32(dr["PGPercentage"]),
                                    CreateOn = Convert.ToDateTime(dr["CreateOn"]),
                                    isActive = Convert.ToBoolean(dr["isActive"])
                                });
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed" + ex.Message.ToString());
            }
            finally
            {

            }
            return Ok(applist);
        }




        [HttpGet]
        public IHttpActionResult GetApplicationID()

        {
            DataSet ds = new DataSet("ApplicationID");
            try
            {
                using (SqlConnection con = new SqlConnection(sqlString.ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("getNewApplicationID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed" + ex.Message.ToString());
            }
            finally
            {

            }
            return Json(ds.Tables[0].Rows[0].ItemArray[0]);
        }


        //http://localhost:49728/api/Common/ValidateAdminLogin?Username=Admin&Password=Admin
        [HttpPost]
        public IHttpActionResult ValidateAdminLogin(UserLogin obj)
        {

            Boolean isAdmin = false;

            try
            {

                using (SqlConnection con = new SqlConnection(sqlString.ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("ValidateAdminLogin", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", obj.Username);
                        cmd.Parameters.AddWithValue("@Password", obj.Password);
                        con.Open();
                        isAdmin = Convert.ToBoolean(cmd.ExecuteScalar());
                        return Json(isAdmin);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed" + ex.Message.ToString());
            }
            finally
            {

            }
        }


        //http://localhost:49728/api/Common/ValidateUserLogin?Username=Micheal1&Password=Micheal1
        [HttpPost]
        public IHttpActionResult ValidateUserLogin(UserLogin obj)
        {
            Boolean isAdmin = false;
            try
            {

                using (SqlConnection con = new SqlConnection(sqlString.ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("[ValidateUserLogin]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", obj.Username);
                        cmd.Parameters.AddWithValue("@Password", obj.Password);
                        con.Open();
                        isAdmin = Convert.ToBoolean(cmd.ExecuteScalar());
                        return Json(isAdmin);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed" + ex.Message.ToString());
            }
            finally
            {

            }


        }

        [HttpPost]
        public IHttpActionResult UpdateApplicationStatus(ApplicationStatus objNewUser)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ApplicationID", typeof(string));
                dt.Columns.Add("ApprovedStatus", typeof(string));
                dt.Columns.Add("ApprovedBy", typeof(string));
                dt.Columns.Add("ApplicationComment", typeof(string));
                dt.Columns.Add("ApprovedOn", typeof(DateTime));

                dt.Rows.Add(
                            objNewUser.ApplicationID,
                            objNewUser.ApprovedStatus,
                            objNewUser.ApprovedBy,
                            objNewUser.ApplicationComment,
                            objNewUser.ApprovedOn
                            );

                using (SqlConnection con = new SqlConnection(sqlString.ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateApplicationStatus", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ApplicationStatus", dt);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed" + ex.Message.ToString());
            }
            finally
            {

            }
            return Json("Success");

        }

        [HttpPost]
        //http://localhost:49728/api/Common/SaveUser
        public IHttpActionResult SaveUser(NewUser objNewUser)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Userid", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Username", typeof(string));
                dt.Columns.Add("Password", typeof(string));
                dt.Columns.Add("EmailID", typeof(string));
                dt.Columns.Add("PhoneNo", typeof(string));
                dt.Columns.Add("Createdon", typeof(DateTime));
                dt.Columns.Add("isActive", typeof(int));

                dt.Rows.Add("",
                            objNewUser.Name,
                            objNewUser.Username,
                            objNewUser.Password,
                            objNewUser.EmailID,
                            objNewUser.PhoneNo,
                            DateTime.Now,
                            1
                            );

                using (SqlConnection con = new SqlConnection(sqlString.ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserRegistration", dt);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) { return Json("Failed" + ex.Message.ToString()); }
            finally { }
            return Json("Success");
        }

        [HttpPost]
        public IHttpActionResult SaveContactUs(ContactUS objContactus)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Subject", typeof(string));
            dt.Columns.Add("Message", typeof(string));
            dt.Rows.Add(
                objContactus.Name,
                objContactus.Email,
                objContactus.Subject,
                objContactus.Message
                );
            try
            {


                using (SqlConnection con = new SqlConnection(sqlString.ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("saveContactUS", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ContactUStbl", dt);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed" + ex.Message.ToString());
            }
            return Json("Success");
        }

        //http://localhost:49728/api/Common/addApplication
        [HttpPost]
        public IHttpActionResult addApplication()
        {
            try
            {
                Application objApplication = new Application();
                objApplication.ApplicationID = HttpContext.Current.Request.Params["ApplicationID"].ToString();
                objApplication.Course = HttpContext.Current.Request.Params["Course"].ToString();
                objApplication.Fname = HttpContext.Current.Request.Params["Fname"].ToString();
                objApplication.Lname = HttpContext.Current.Request.Params["Lname"].ToString();
                objApplication.Fullname = HttpContext.Current.Request.Params["Fullname"].ToString();
                objApplication.DOB = Convert.ToDateTime(HttpContext.Current.Request.Params["DOB"].ToString());
                objApplication.Gender = Convert.ToBoolean(HttpContext.Current.Request.Params["Gender"].ToString());
                objApplication.FatherName = HttpContext.Current.Request.Params["FatherName"].ToString();
                objApplication.MotherName = HttpContext.Current.Request.Params["MotherName"].ToString();
                objApplication.Religion = HttpContext.Current.Request.Params["Religion"].ToString();
                objApplication.Nationality = HttpContext.Current.Request.Params["Nationality"].ToString();
                objApplication.Email = HttpContext.Current.Request.Params["Email"].ToString();
                objApplication.MobileNo = Convert.ToInt64(HttpContext.Current.Request.Params["MobileNo"].ToString());
                objApplication.P_Address = HttpContext.Current.Request.Params["P_Address"].ToString();
                objApplication.C_Address = HttpContext.Current.Request.Params["C_Address"].ToString();
                objApplication.SSLCSchoolName = HttpContext.Current.Request.Params["SSLCSchoolName"].ToString();
                objApplication.SSLCMark = Convert.ToInt32(HttpContext.Current.Request.Params["SSLCMark"].ToString());
                objApplication.SSLCPercentage = Convert.ToInt32(HttpContext.Current.Request.Params["SSLCPercentage"].ToString());
                objApplication.HSCSchoolName = HttpContext.Current.Request.Params["HSCSchoolName"].ToString();
                objApplication.HSCMark = Convert.ToInt32(HttpContext.Current.Request.Params["HSCMark"].ToString());
                objApplication.HSCPercentage = Convert.ToInt32(HttpContext.Current.Request.Params["HSCPercentage"].ToString());
                objApplication.UGSchoolName = HttpContext.Current.Request.Params["UGSchoolName"].ToString();
                objApplication.UGMark = Convert.ToInt32(HttpContext.Current.Request.Params["UGMark"].ToString());
                objApplication.UGPercentage = Convert.ToInt32(HttpContext.Current.Request.Params["UGPercentage"].ToString());
                objApplication.PGSchoolName = HttpContext.Current.Request.Params["PGSchoolName"].ToString();
                objApplication.PGMark = Convert.ToInt32(HttpContext.Current.Request.Params["PGMark"].ToString());
                objApplication.PGPercentage = Convert.ToInt32(HttpContext.Current.Request.Params["PGPercentage"].ToString());
                objApplication.CreateOn = DateTime.Now;// Convert.ToDateTime( HttpContext.Current.Request.Params["CreateOn"].ToString());
                objApplication.isActive = true; //Convert.ToBoolean( HttpContext.Current.Request.Params["isActive"].ToString());


                string imageName = null;
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files["Image"];
                imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                imageName = imageName + DateTime.Now.ToString("yymmssffff") + Path.GetExtension(postedFile.FileName);
                var filePath = HttpContext.Current.Server.MapPath("~/Image/" + imageName);
                postedFile.SaveAs(filePath);
                string ImageCaption = HttpContext.Current.Request.Params["ImageCaption"];
                objApplication.photo = filePath;


                DataTable dt = new DataTable();
                dt.Columns.Add("ApplicationID", typeof(string));
                dt.Columns.Add("Course", typeof(string));
                dt.Columns.Add("Fname", typeof(string));
                dt.Columns.Add("Lname", typeof(string));
                dt.Columns.Add("Fullname", typeof(string));
                dt.Columns.Add("DOB", typeof(DateTime));
                dt.Columns.Add("Gender", typeof(bool));
                dt.Columns.Add("FatherName", typeof(string));
                dt.Columns.Add("MotherName", typeof(string));
                dt.Columns.Add("Religion", typeof(string));
                dt.Columns.Add("Nationality", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("MobileNo", typeof(Int64));
                dt.Columns.Add("P_Address", typeof(string));
                dt.Columns.Add("C_Address", typeof(string));
                dt.Columns.Add("SSLCSchoolName", typeof(string));
                dt.Columns.Add("SSLCMark", typeof(Int32));
                dt.Columns.Add("SSLCPercentage", typeof(Int32));
                dt.Columns.Add("HSCSchoolName", typeof(string));
                dt.Columns.Add("HSCMark", typeof(Int32));
                dt.Columns.Add("HSCPercentage", typeof(Int32));
                dt.Columns.Add("UGSchoolName", typeof(string));
                dt.Columns.Add("UGMark", typeof(Int32));
                dt.Columns.Add("UGPercentage", typeof(Int32));
                dt.Columns.Add("PGSchoolName", typeof(string));
                dt.Columns.Add("PGMark", typeof(Int32));
                dt.Columns.Add("PGPercentage", typeof(Int32));
                dt.Columns.Add("CreateOn", typeof(DateTime));
                dt.Columns.Add("isActive", typeof(bool));
                dt.Columns.Add("photo", typeof(string));

                dt.Rows.Add(
                            objApplication.ApplicationID,
                            objApplication.Course,
                            objApplication.Fname,
                            objApplication.Lname,
                            objApplication.Fullname,
                            objApplication.DOB,
                            objApplication.Gender,
                            objApplication.FatherName,
                            objApplication.MotherName,
                            objApplication.Religion,
                            objApplication.Nationality,
                            objApplication.Email,
                            objApplication.MobileNo,
                            objApplication.P_Address,
                            objApplication.C_Address,
                            objApplication.SSLCSchoolName,
                            objApplication.SSLCMark,
                            objApplication.SSLCPercentage,
                            objApplication.HSCSchoolName,
                            objApplication.HSCMark,
                            objApplication.HSCPercentage,
                            objApplication.UGSchoolName,
                            objApplication.UGMark,
                            objApplication.UGPercentage,
                            objApplication.PGSchoolName,
                            objApplication.PGMark,
                            objApplication.PGPercentage,
                            objApplication.CreateOn,
                            objApplication.isActive,
                            objApplication.photo
                            );


                using (SqlConnection con = new SqlConnection(sqlString.ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateApplication", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TypeApplicationParam", dt);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed" + ex.Message.ToString());
            }
            finally
            {

            }
            return Json("Success");
        }

        //http://localhost:49728/api/Common/GetGalleryImages
        [HttpGet]
        public IHttpActionResult GetGalleryImages()
        {
            DataSet ds = new DataSet("TimeRanges");
            List<GalleryModel> imgCollection = new List<GalleryModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlString.ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GetGalleryImages", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);

                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            imgCollection.Add(
                                new GalleryModel
                                {
                                    Id = Convert.ToInt32(dr["Id"]),
                                    Name = Convert.ToString(dr["Name"]),
                                    ImgPath = Convert.ToString(dr["ImgPath"]),
                                    Description = Convert.ToString(dr["Description"]),
                                    Liked = Convert.ToString(dr["Liked"]),
                                    DisLiked = Convert.ToString(dr["DisLiked"])
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed" + ex.Message.ToString());
            }
            finally
            {

            }
            return Json(imgCollection);
        }
        [HttpPost]
        public HttpResponseMessage UpLoadImage()
        {
            string imageName = null;
            var httpRequest = HttpContext.Current.Request;
            var postedFile = httpRequest.Files["Image"];
            imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssffff") + Path.GetExtension(postedFile.FileName);
            var filePath = HttpContext.Current.Server.MapPath("~/Image/" + imageName);
            postedFile.SaveAs(filePath);
            string ImageCaption = HttpContext.Current.Request.Params["ImageCaption"];

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        // GET: api/Common
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Common/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Common
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Common/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Common/5
        public void Delete(int id)
        {
        }
    }
}
