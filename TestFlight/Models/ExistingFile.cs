using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace TestFlight.Models
{
    public class ExistingFile : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var FileNames = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Images/Products/"));
            var PhotoUpload = (ProductUpload)validationContext.ObjectInstance;
            var Exist = false;
            var i = 0;
            var ErrorFiles = "";
            
            foreach (var st in FileNames)
            {
                FileNames[i] = Path.GetFileName(FileNames[i]);
                i++;
            }
            
            foreach (var file in PhotoUpload.PhotoUpload)
            {
                if (file != null)
                {
                    string photoName = Path.GetFileName(file.FileName);
                    foreach (var ExistName in FileNames)
                    {
                        if (ExistName == photoName)
                        {
                            Exist = true;
                            ErrorFiles = ErrorFiles + " " + ExistName;
                        }
                    }
                }
            }

            if (!Exist) return ValidationResult.Success;
            else return new ValidationResult("Following files are already exist - " + ErrorFiles);
        }
    }
}