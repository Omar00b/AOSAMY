using Microsoft.AspNetCore.Mvc;
using AOSAMY.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AOSAMY.Controllers
{
    public class OperationController : Controller
    {
        DbAOSAMYContext db = new DbAOSAMYContext();
        public void creatSelectList(int selectId = 1)
        {


            List<TypeProduct> typeProducts = db.TypeProducts.ToList();
            SelectList listTypes = new SelectList(typeProducts, "Id", "Name", selectId);
            ViewBag.TypeList = listTypes;

        }
        public IActionResult AddProduct(int? Userlogid)
        {
            ViewBag.DefaultId = Userlogid ?? 0;
            creatSelectList();
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult AddProduct(Product product)
        {
            var U =db.Products.Find(product.UserLogid);
            if (product.clientFile != null)
            {
                //string MyUpload = Path.Combine(_host.WebRootPath, "img");
                //fileName =userInfo.clientFile.FileName;
                //string fullPath = Path.Combine(MyUpload, fileName);
                //userInfo.clientFile.CopyTo(new FileStream(fullPath, FileMode.Create));
                //userInfo.
                MemoryStream stream = new MemoryStream();
                product.clientFile.CopyTo(stream);
                product.Img = stream.ToArray();
            }
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("UserPage", "User",  new { id = U.Id });
        }

       
        //Get
        public IActionResult Portfolio(int? Id)
        {
            ViewBag.Default = Id ?? 0;

            // تجلب البريد الإلكتروني والصورة الشخصية للمستخدم من قاعدة البيانات
            // وتعيينهما في ViewBag أو ViewData
            var userInfo = db.UserInfos.Find(Id);
            ViewBag.Email = userInfo.Email;
            ViewBag.ProfileImage = userInfo.imgSrs;
            ViewBag.FirstName = userInfo.FirstName;
            ViewBag.secndName = userInfo.ScendName;

            ViewBag.Default = Id ?? 0;
            TempData["SuccessMessage"] = "تم الحفظ بنجاح!";
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
                TempData.Remove("SuccessMessage");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Portfolio(UserInfo userInfo)
        {
            

            var existingUserInfo = db.UserInfos.Attach(userInfo);

            if (existingUserInfo.Entity.clientFile != null ||
                existingUserInfo.Entity.FirstName != null ||
                existingUserInfo.Entity.ScendName != null ||
                existingUserInfo.Entity.Description != null ||
                existingUserInfo.Entity.CopnyName != null||
                existingUserInfo.Entity.PhoNum!=null)
            {
                if (existingUserInfo.Entity.clientFile != null){
                    
                    MemoryStream stream = new MemoryStream();
                    existingUserInfo.Entity.clientFile.CopyTo(stream);
                    existingUserInfo.Entity.Img = stream.ToArray();
                    existingUserInfo.Property(u => u.Img).IsModified = true;
                }
                if (existingUserInfo.Entity.FirstName != null){existingUserInfo.Property(u => u.FirstName).IsModified = true;}
                if (existingUserInfo.Entity.ScendName != null){existingUserInfo.Property(u => u.ScendName).IsModified = true;}
                if (existingUserInfo.Entity.Description != null){existingUserInfo.Property(u => u.Description).IsModified = true;}
                if (existingUserInfo.Entity.CopnyName != null){existingUserInfo.Property(u => u.CopnyName).IsModified = true;}
                if (existingUserInfo.Entity.Addres != null) { existingUserInfo.Property(u=>u.Addres).IsModified = true;}
                if (existingUserInfo.Entity.Specialty != null) { existingUserInfo.Property(u => u.Specialty).IsModified = true; }
                if (existingUserInfo.Entity.PhoNum!= null) { existingUserInfo.Property(u => u.PhoNum).IsModified = true; }

                db.SaveChanges(); 
            }
            //var U = db.UserInfos.Find();

            return RedirectToAction("UserPage", "User",  new { id = existingUserInfo.Entity.Id });
        }
    }
}
