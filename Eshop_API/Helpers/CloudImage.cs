using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace eshop_api.Helpers
{
    public static class CloudImage
    {
        public static Cloudinary ConnectCloudianry()
        {
            Account account = new Account(
                "dyvlzl3cw",
                "545395683675983",
                "z1i3oIu7ssuIG-1Gk6UCCvfc-Hc"
            );
            Cloudinary cloudinary= new Cloudinary(account);
            return cloudinary;
        }

        public static string UploadImage(IFormFile Img)
        {
            if(Img.Length > 0){
                using(var stream  = Img.OpenReadStream()){
                    Cloudinary cloudinary = ConnectCloudianry();
                    var uploadParams = new ImageUploadParams(){
                        File = new FileDescription(Img.FileName,stream),
                        Folder = "projects/PBL6/Eshop"
                    };
                    var uploadResult = cloudinary.Upload(uploadParams);
                    return uploadResult.Url.ToString();
                }
            }
            return ""; 
        }

        public static string UploadVideo(IFormFile url)
        {
            throw new NotImplementedException();
        }
    }
}